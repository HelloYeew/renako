using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osu.Framework.Timing;
using osuTK;
using osuTK.Input;
using Renako.Game.Audio;
using Renako.Game.Beatmaps;
using Renako.Game.Configurations;
using Renako.Game.Graphics.Drawables;
using Renako.Game.Graphics.ScreenStacks;
using Renako.Game.Graphics.UserInterface;
using Renako.Game.Utilities;

namespace Renako.Game.Graphics.Screens;

public partial class SongSelectionScreen : RenakoScreen
{
    private FillFlowContainer songTitleContainer;
    private Container songListContainer;
    private Container beatmapSelectionContainer;
    private Container finalSettingsContainer;
    private readonly Bindable<SongSelectionScreenState> currentScreenState = new Bindable<SongSelectionScreenState>();

    [Resolved]
    private BeatmapsCollection beatmapsCollection { get; set; }

    [Resolved]
    private WorkingBeatmap workingBeatmap { get; set; }

    [Resolved]
    private GameHost host { get; set; }

    [Resolved]
    private RenakoBackgroundScreenStack backgroundScreenStack { get; set; }

    [Resolved]
    private RenakoConfigManager config { get; set; }

    [Resolved]
    private RenakoScreenStack mainScreenStack { get; set; }

    [Resolved]
    private RenakoAudioManager renakoAudioManager { get; set; }

    private HorizontalTextureSwiper<BeatmapSet> beatmapSetSwiper;
    private List<TextureSwiperItem<BeatmapSet>> beatmapSetSwiperItemList;
    private BeatmapSelectionSwiper beatmapSwiper;
    private List<Beatmap> beatmapList;
    private BindableSettingsSwiper finalSettingsSwiper;
    private MenuTitleWithTexture songTitle;
    private SpriteText sourceText;
    private SpriteText totalBeatmapSetDifficultyText;
    private SpriteText bpmText;
    private SpriteText beatmapSetCreatorText;
    private SpriteText lengthText;
    private Container beatmapInfoContainer;
    private Box beatmapInfoBox;
    private SpriteIcon beatmapCreatorIcon;
    private SpriteText beatmapCreatorText;
    private SpriteIcon beatmapDifficultySpriteIcon;
    private SpriteText beatmapDifficultyRatingText;
    private SpriteIcon beatmapLevelNameSpriteIcon;
    private SpriteText beatmapLevelNameText;
    private Container beatmapNoteCountContainer;
    private Box beatmapNoteCountBox;

    private Box modeBackgroundBox;
    private Box sourceBackgroundBox;
    private Box songInfoBackgroundBox;

    private Container idleRenakoLogoContainer;
    private RightBottomBeatmapSetDetailContainer rightBottomDetailsContainer;

    private RightBottomButton rightBottomButton;
    private BackButton backButton;

    private AudioVisualizer audioVisualizer;

    private double lastBeatmapChangeTime;
    private bool isBeatmapChanged;

    private readonly StopwatchClock interactionTimer = new StopwatchClock();

    private double lastInteractionTime;
    private readonly BindableBool isHiding = new BindableBool();

    private Sample leftClickSample;
    private Sample rightClickSample;
    private Sample backClickSample;
    private Sample goClickSample;
    private TextureStore textureStore;

    private Bindable<bool> useUnicodeInfo;
    private Bindable<bool> disableVideoBackground;

    private const int icon_size = 13;
    private const int song_description_font_size = 15;

    private const int default_beatmapset_id = 0;
    private const int default_beatmap_id = 0;

    public const double INTERACTION_TIMEOUT = 15000;

    [BackgroundDependencyLoader]
    private void load(TextureStore textureStore, RenakoConfigManager config, AudioManager audioManager, RenakoAudioManager renakoAudioManager)
    {
        leftClickSample = audioManager.Samples.Get("UI/click-small-left");
        rightClickSample = audioManager.Samples.Get("UI/click-small-right");
        backClickSample = audioManager.Samples.Get("UI/click-enter1");
        goClickSample = audioManager.Samples.Get("UI/click-enter2");
        this.textureStore = textureStore;

        beatmapSetSwiperItemList = new List<TextureSwiperItem<BeatmapSet>>();
        beatmapSetSwiper = new HorizontalTextureSwiper<BeatmapSet>()
        {
            Items = beatmapSetSwiperItemList,
            Position = new Vector2(0, 0)
        };
        beatmapList = new List<Beatmap>();
        beatmapSwiper = new BeatmapSelectionSwiper()
        {
            BeatmapList = beatmapList,
            Position = new Vector2(0, 0)
        };
        finalSettingsSwiper = new BindableSettingsSwiper()
        {
            Position = new Vector2(0, 0)
        };

        workingBeatmap.BeatmapSet = config.Get<int>(RenakoSetting.LatestBeatmapSetID) == 0 ? beatmapsCollection.GetBeatmapSetByID(default_beatmapset_id) : beatmapsCollection.GetBeatmapSetByID(config.Get<int>(RenakoSetting.LatestBeatmapSetID));
        workingBeatmap.Beatmap = config.Get<int>(RenakoSetting.LatestBeatmapID) == 0 ? beatmapsCollection.GetBeatmapByID(default_beatmap_id) : beatmapsCollection.GetBeatmapByID(config.Get<int>(RenakoSetting.LatestBeatmapID));

        if (workingBeatmap.BeatmapSet == null || workingBeatmap.BeatmapSet.Hide)
        {
            workingBeatmap.BeatmapSet = beatmapsCollection.GetBeatmapSetByID(default_beatmapset_id);
        }

        workingBeatmap.Beatmap ??= beatmapsCollection.GetBeatmapByID(default_beatmap_id);

        foreach (BeatmapSet beatmapSet in beatmapsCollection.BeatmapSets)
        {
            Texture texture;

            if (beatmapSet.Hide)
            {
                continue;
            }

            if (beatmapSet.UseLocalSource)
            {
                texture = textureStore.Get(beatmapSet.CoverPath);
            }
            else
            {
                string coverPath = BeatmapSetUtility.GetCoverPath(beatmapSet);
                texture = textureStore.Get(coverPath);

                if (texture == null)
                {
                    Stream coverTextureStream = host.Storage.GetStream(coverPath);
                    texture = Texture.FromStream(host.Renderer, coverTextureStream);
                    coverTextureStream?.Close();
                }
            }

            beatmapSetSwiperItemList.Add(new TextureSwiperItem<BeatmapSet>()
            {
                Item = beatmapSet,
                Texture = texture
            });
        }

        if (workingBeatmap.BeatmapSet != null)
        {
            beatmapSetSwiper.CurrentIndex = beatmapSetSwiperItemList.FindIndex(x => x.Item.Equals(workingBeatmap.BeatmapSet));
            beatmapList = beatmapsCollection.GetBeatmapsFromBeatmapSet(workingBeatmap.BeatmapSet).ToList();
        }

        Alpha = 0;
        InternalChildren = new Drawable[]
        {
            new Container()
            {
                Anchor = Anchor.BottomCentre,
                Origin = Anchor.BottomCentre,
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(1),
                Child = audioVisualizer = new AudioVisualizer()
                {
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1),
                    Alpha = 0.75f
                }
            },
            backButton = new BackButton()
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Action = toggleBackButton
            },
            rightBottomButton = new RightBottomButton()
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Icon = FontAwesome.Solid.ArrowRight,
                Width = 200,
                Text = "Go!",
                Action = toggleGoButton
            },
            songTitleContainer = new FillFlowContainer()
            {
                Anchor = Anchor.TopLeft,
                Origin = Anchor.TopLeft,
                RelativeSizeAxes = Axes.Both,
                Direction = FillDirection.Vertical,
                Spacing = new Vector2(0, 14),
                Position = new Vector2(-600, 0),
                Size = new Vector2(1f, 0.50f),
                Children = new Drawable[]
                {
                    // Mode text
                    new Container()
                    {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        RelativeSizeAxes = Axes.X,
                        Size = new Vector2(0.25f, 30),
                        Masking = true,
                        CornerRadius = 15,
                        Children = new Drawable[]
                        {
                            modeBackgroundBox = new Box()
                            {
                                Colour = Color4Extensions.FromHex("E0BCD5"),
                                RelativeSizeAxes = Axes.Both,
                                Shear = new Vector2(0.45f, 0f)
                            },
                            new SpriteText()
                            {
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                Text = "Single Player".ToUpper(),
                                Font = RenakoFont.GetFont(RenakoFont.Typeface.JosefinSans, 35f, RenakoFont.FontWeight.Bold),
                                Colour = Color4Extensions.FromHex("6C375C"),
                                Padding = new MarginPadding()
                                {
                                    Left = 40
                                }
                            }
                        }
                    },
                    // Song title
                    songTitle = new MenuTitleWithTexture()
                    {
                        ButtonWidth = 0.3375f,
                        BackgroundColor = Color4Extensions.FromHex("F2DFE9"),
                        TitleColor = Color4Extensions.FromHex("67344D"),
                        DescriptionColor = Color4Extensions.FromHex("251319"),
                        AutoUpperCaseTitle = false,
                        Title = "No beatmap selected",
                        Description = "Please add some beatmaps first!"
                    },
                    // Source
                    new Container()
                    {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        RelativeSizeAxes = Axes.X,
                        Size = new Vector2(0.3125f, 30),
                        Masking = true,
                        CornerRadius = 15,
                        Children = new Drawable[]
                        {
                            sourceBackgroundBox = new Box()
                            {
                                Colour = Color4Extensions.FromHex("E0BCD5"),
                                RelativeSizeAxes = Axes.Both,
                                Shear = new Vector2(0.45f, 0f)
                            },
                            new FillFlowContainer()
                            {
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                RelativeSizeAxes = Axes.Both,
                                Padding = new MarginPadding()
                                {
                                    Left = 40,
                                    Right = 20,
                                    Top = 20,
                                    Bottom = 20
                                },
                                Spacing = new Vector2(5, 0),
                                Direction = FillDirection.Horizontal,
                                Children = new Drawable[]
                                {
                                    new SpriteIcon()
                                    {
                                        Anchor = Anchor.CentreLeft,
                                        Origin = Anchor.CentreLeft,
                                        Size = new Vector2(icon_size),
                                        Icon = FontAwesome.Solid.ShareAlt,
                                        Colour = Color4Extensions.FromHex("67344D")
                                    },
                                    sourceText = new SpriteText()
                                    {
                                        Anchor = Anchor.CentreLeft,
                                        Origin = Anchor.CentreLeft,
                                        Font = RenakoFont.GetFont(RenakoFont.Typeface.MPlus1P, song_description_font_size),
                                        Colour = Color4Extensions.FromHex("251319")
                                    }
                                }
                            }
                        }
                    },
                    // Song info
                    new Container()
                    {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        RelativeSizeAxes = Axes.X,
                        Size = new Vector2(0.3f, 30),
                        Masking = true,
                        CornerRadius = 15,
                        Children = new Drawable[]
                        {
                            songInfoBackgroundBox = new Box()
                            {
                                Colour = Color4Extensions.FromHex("BEB6BA"),
                                RelativeSizeAxes = Axes.Both,
                                Shear = new Vector2(0.45f, 0f)
                            },
                            new FillFlowContainer()
                            {
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                RelativeSizeAxes = Axes.Both,
                                Padding = new MarginPadding()
                                {
                                    Left = 40,
                                    Right = 20,
                                    Top = 20,
                                    Bottom = 20
                                },
                                Spacing = new Vector2(20, 0),
                                Direction = FillDirection.Horizontal,
                                Children = new Drawable[]
                                {
                                    new FillFlowContainer()
                                    {
                                        Anchor = Anchor.CentreLeft,
                                        Origin = Anchor.CentreLeft,
                                        RelativeSizeAxes = Axes.X,
                                        Direction = FillDirection.Horizontal,
                                        Spacing = new Vector2(5, 0),
                                        Children = new Drawable[]
                                        {
                                            new SpriteIcon()
                                            {
                                                Anchor = Anchor.CentreLeft,
                                                Origin = Anchor.CentreLeft,
                                                Size = new Vector2(icon_size),
                                                Icon = FontAwesome.Solid.User,
                                                Colour = Color4Extensions.FromHex("593145")
                                            },
                                            beatmapSetCreatorText = new SpriteText()
                                            {
                                                Anchor = Anchor.CentreLeft,
                                                Origin = Anchor.CentreLeft,
                                                Font = RenakoFont.GetFont(RenakoFont.Typeface.MPlus1P, song_description_font_size),
                                                Colour = Color4Extensions.FromHex("170C10")
                                            },
                                            new SpriteIcon()
                                            {
                                                Anchor = Anchor.CentreLeft,
                                                Origin = Anchor.CentreLeft,
                                                Size = new Vector2(icon_size),
                                                Icon = FontAwesome.Solid.Rocket,
                                                Colour = Color4Extensions.FromHex("593145")
                                            },
                                            totalBeatmapSetDifficultyText = new SpriteText()
                                            {
                                                Anchor = Anchor.CentreLeft,
                                                Origin = Anchor.CentreLeft,
                                                Font = RenakoFont.GetFont(RenakoFont.Typeface.MPlus1P, song_description_font_size),
                                                Colour = Color4Extensions.FromHex("170C10")
                                            },
                                            new SpriteIcon()
                                            {
                                                Anchor = Anchor.CentreLeft,
                                                Origin = Anchor.CentreLeft,
                                                Size = new Vector2(icon_size),
                                                Icon = FontAwesome.Solid.Clock,
                                                Colour = Color4Extensions.FromHex("593145")
                                            },
                                            lengthText = new SpriteText()
                                            {
                                                Anchor = Anchor.CentreLeft,
                                                Origin = Anchor.CentreLeft,
                                                Font = RenakoFont.GetFont(RenakoFont.Typeface.MPlus1P, song_description_font_size),
                                                Colour = Color4Extensions.FromHex("170C10")
                                            },
                                            new SpriteIcon()
                                            {
                                                Anchor = Anchor.CentreLeft,
                                                Origin = Anchor.CentreLeft,
                                                Size = new Vector2(icon_size),
                                                Icon = FontAwesome.Solid.Heartbeat,
                                                Colour = Color4Extensions.FromHex("593145")
                                            },
                                            bpmText = new SpriteText()
                                            {
                                                Anchor = Anchor.CentreLeft,
                                                Origin = Anchor.CentreLeft,
                                                Font = RenakoFont.GetFont(RenakoFont.Typeface.MPlus1P, song_description_font_size),
                                                Colour = Color4Extensions.FromHex("170C10")
                                            }
                                        }
                                    }
                                }
                            }
                        },
                    },
                    // Beatmap info (difficulty and level name)
                    new Container()
                    {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        RelativeSizeAxes = Axes.X,
                        Size = new Vector2(0.2875f, 30),
                        Masking = true,
                        CornerRadius = 15,
                        Child = beatmapInfoContainer = new Container()
                        {
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                            RelativeSizeAxes = Axes.Both,
                            Position = new Vector2(-600, 0),
                            Alpha = 0,
                            Children = new Drawable[]
                            {
                                beatmapInfoBox = new Box()
                                {
                                    Colour = Color4Extensions.FromHex("BEB6BA"),
                                    RelativeSizeAxes = Axes.Both,
                                    Shear = new Vector2(0.45f, 0f)
                                },
                                new FillFlowContainer()
                                {
                                    Anchor = Anchor.CentreLeft,
                                    Origin = Anchor.CentreLeft,
                                    RelativeSizeAxes = Axes.Both,
                                    Padding = new MarginPadding()
                                    {
                                        Left = 40,
                                        Right = 20,
                                        Top = 20,
                                        Bottom = 20
                                    },
                                    Spacing = new Vector2(20, 0),
                                    Direction = FillDirection.Horizontal,
                                    Children = new Drawable[]
                                    {
                                        new FillFlowContainer()
                                        {
                                            Anchor = Anchor.CentreLeft,
                                            Origin = Anchor.CentreLeft,
                                            RelativeSizeAxes = Axes.X,
                                            Direction = FillDirection.Horizontal,
                                            Spacing = new Vector2(5, 0),
                                            Children = new Drawable[]
                                            {
                                                beatmapCreatorIcon = new SpriteIcon()
                                                {
                                                    Anchor = Anchor.CentreLeft,
                                                    Origin = Anchor.CentreLeft,
                                                    Size = new Vector2(icon_size),
                                                    Icon = FontAwesome.Solid.User,
                                                    Colour = Color4Extensions.FromHex("593145")
                                                },
                                                beatmapCreatorText = new SpriteText()
                                                {
                                                    Anchor = Anchor.CentreLeft,
                                                    Origin = Anchor.CentreLeft,
                                                    Font = RenakoFont.GetFont(RenakoFont.Typeface.MPlus1P, song_description_font_size),
                                                    Colour = Color4Extensions.FromHex("170C10")
                                                },
                                                beatmapDifficultySpriteIcon = new SpriteIcon()
                                                {
                                                    Anchor = Anchor.CentreLeft,
                                                    Origin = Anchor.CentreLeft,
                                                    Size = new Vector2(icon_size),
                                                    Icon = FontAwesome.Solid.Rocket,
                                                    Colour = Color4Extensions.FromHex("593145")
                                                },
                                                beatmapDifficultyRatingText = new SpriteText()
                                                {
                                                    Anchor = Anchor.CentreLeft,
                                                    Origin = Anchor.CentreLeft,
                                                    Font = RenakoFont.GetFont(RenakoFont.Typeface.MPlus1P, song_description_font_size),
                                                    Colour = Color4Extensions.FromHex("170C10")
                                                },
                                                beatmapLevelNameSpriteIcon = new SpriteIcon()
                                                {
                                                    Anchor = Anchor.CentreLeft,
                                                    Origin = Anchor.CentreLeft,
                                                    Size = new Vector2(icon_size),
                                                    Icon = FontAwesome.Solid.Music,
                                                    Colour = Color4Extensions.FromHex("593145")
                                                },
                                                beatmapLevelNameText = new SpriteText()
                                                {
                                                    Anchor = Anchor.CentreLeft,
                                                    Origin = Anchor.CentreLeft,
                                                    Font = RenakoFont.GetFont(RenakoFont.Typeface.MPlus1P, song_description_font_size),
                                                    Colour = Color4Extensions.FromHex("170C10")
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    },
                    // Beatmap info (note count)
                    new Container()
                    {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        RelativeSizeAxes = Axes.X,
                        Size = new Vector2(0.275f, 30),
                        Masking = true,
                        CornerRadius = 15,
                        Child = beatmapNoteCountContainer = new Container()
                        {
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                            RelativeSizeAxes = Axes.Both,
                            Position = new Vector2(-600, 0),
                            Alpha = 0,
                            Children = new Drawable[]
                            {
                                beatmapNoteCountBox = new Box()
                                {
                                    Colour = Color4Extensions.FromHex("BEB6BA"),
                                    RelativeSizeAxes = Axes.Both,
                                    Shear = new Vector2(0.45f, 0f)
                                },
                                new FillFlowContainer()
                                {
                                    Anchor = Anchor.CentreLeft,
                                    Origin = Anchor.CentreLeft,
                                    RelativeSizeAxes = Axes.Both,
                                    Padding = new MarginPadding()
                                    {
                                        Left = 40,
                                        Right = 20,
                                        Top = 20,
                                        Bottom = 20
                                    },
                                    Spacing = new Vector2(20, 0),
                                    Direction = FillDirection.Horizontal,
                                    Children = new Drawable[]
                                    {
                                        new FillFlowContainer()
                                        {
                                            Anchor = Anchor.CentreLeft,
                                            Origin = Anchor.CentreLeft,
                                            RelativeSizeAxes = Axes.X,
                                            Direction = FillDirection.Horizontal,
                                            Spacing = new Vector2(5, 0),
                                            Children = new Drawable[]
                                            {
                                                // TODO: Fill info here
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            },
            // Song list
            songListContainer = new Container()
            {
                Anchor = Anchor.BottomCentre,
                Origin = Anchor.BottomCentre,
                RelativeSizeAxes = Axes.X,
                Size = new Vector2(1, 150),
                Position = new Vector2(0, 600),
                Children = new Drawable[]
                {
                    new Box()
                    {
                        Anchor = Anchor.BottomCentre,
                        Origin = Anchor.BottomCentre,
                        RelativeSizeAxes = Axes.Both,
                        Colour = Color4Extensions.FromHex("82767E")
                    },
                    new BasicButton()
                    {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        RelativeSizeAxes = Axes.Y,
                        Size = new Vector2(30, 1),
                        Position = new Vector2(15, 0),
                        Colour = Colour4.White,
                        Action = togglePreviousButton,
                        Child = new FillFlowContainer()
                        {
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                            RelativeSizeAxes = Axes.Both,
                            Direction = FillDirection.Horizontal,
                            Children = new Drawable[]
                            {
                                new SpriteIcon()
                                {
                                    Anchor = Anchor.CentreLeft,
                                    Origin = Anchor.CentreLeft,
                                    Size = new Vector2(20),
                                    Icon = FontAwesome.Solid.ChevronLeft,
                                    Colour = Color4Extensions.FromHex("D2C9D8"),
                                },
                                new SpriteIcon()
                                {
                                    Anchor = Anchor.CentreLeft,
                                    Origin = Anchor.CentreLeft,
                                    Size = new Vector2(20),
                                    Icon = FontAwesome.Solid.ChevronLeft,
                                    Colour = Color4Extensions.FromHex("D2C9D8"),
                                }
                            }
                        }
                    },
                    new BasicButton()
                    {
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.CentreRight,
                        RelativeSizeAxes = Axes.Y,
                        Size = new Vector2(30, 1),
                        Position = new Vector2(-15, 0),
                        Colour = Colour4.White,
                        Action = toggleNextButton,
                        Child = new FillFlowContainer()
                        {
                            Anchor = Anchor.CentreRight,
                            Origin = Anchor.CentreRight,
                            RelativeSizeAxes = Axes.Both,
                            Direction = FillDirection.Horizontal,
                            Children = new Drawable[]
                            {
                                new SpriteIcon()
                                {
                                    Anchor = Anchor.CentreRight,
                                    Origin = Anchor.CentreRight,
                                    Size = new Vector2(20),
                                    Icon = FontAwesome.Solid.ChevronRight,
                                    Colour = Color4Extensions.FromHex("D2C9D8"),
                                },
                                new SpriteIcon()
                                {
                                    Anchor = Anchor.CentreRight,
                                    Origin = Anchor.CentreRight,
                                    Size = new Vector2(20),
                                    Icon = FontAwesome.Solid.ChevronRight,
                                    Colour = Color4Extensions.FromHex("D2C9D8"),
                                }
                            }
                        }
                    },
                    beatmapSetSwiper
                }
            },
            // Beatmap selection
            beatmapSelectionContainer = new Container()
            {
                Anchor = Anchor.BottomCentre,
                Origin = Anchor.BottomCentre,
                RelativeSizeAxes = Axes.X,
                Alpha = 0,
                Size = new Vector2(1, 150),
                Position = new Vector2(2000, -115),
                Children = new Drawable[]
                {
                    new Box()
                    {
                        Anchor = Anchor.BottomCentre,
                        Origin = Anchor.BottomCentre,
                        RelativeSizeAxes = Axes.Both,
                        Colour = Color4Extensions.FromHex("82767E")
                    },
                    new BasicButton()
                    {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        RelativeSizeAxes = Axes.Y,
                        Size = new Vector2(30, 1),
                        Position = new Vector2(15, 0),
                        Colour = Colour4.White,
                        Action = togglePreviousButton,
                        Child = new FillFlowContainer()
                        {
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                            RelativeSizeAxes = Axes.Both,
                            Direction = FillDirection.Horizontal,
                            Children = new Drawable[]
                            {
                                new SpriteIcon()
                                {
                                    Anchor = Anchor.CentreLeft,
                                    Origin = Anchor.CentreLeft,
                                    Size = new Vector2(20),
                                    Icon = FontAwesome.Solid.ChevronLeft,
                                    Colour = Color4Extensions.FromHex("D2C9D8"),
                                },
                                new SpriteIcon()
                                {
                                    Anchor = Anchor.CentreLeft,
                                    Origin = Anchor.CentreLeft,
                                    Size = new Vector2(20),
                                    Icon = FontAwesome.Solid.ChevronLeft,
                                    Colour = Color4Extensions.FromHex("D2C9D8"),
                                }
                            }
                        }
                    },
                    new BasicButton()
                    {
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.CentreRight,
                        RelativeSizeAxes = Axes.Y,
                        Size = new Vector2(30, 1),
                        Position = new Vector2(-15, 0),
                        Colour = Colour4.White,
                        Action = toggleNextButton,
                        Child = new FillFlowContainer()
                        {
                            Anchor = Anchor.CentreRight,
                            Origin = Anchor.CentreRight,
                            RelativeSizeAxes = Axes.Both,
                            Direction = FillDirection.Horizontal,
                            Children = new Drawable[]
                            {
                                new SpriteIcon()
                                {
                                    Anchor = Anchor.CentreRight,
                                    Origin = Anchor.CentreRight,
                                    Size = new Vector2(20),
                                    Icon = FontAwesome.Solid.ChevronRight,
                                    Colour = Color4Extensions.FromHex("D2C9D8"),
                                },
                                new SpriteIcon()
                                {
                                    Anchor = Anchor.CentreRight,
                                    Origin = Anchor.CentreRight,
                                    Size = new Vector2(20),
                                    Icon = FontAwesome.Solid.ChevronRight,
                                    Colour = Color4Extensions.FromHex("D2C9D8"),
                                }
                            }
                        }
                    },
                    beatmapSwiper
                }
            },
            // Beatmap selection
            finalSettingsContainer = new Container()
            {
                Anchor = Anchor.BottomCentre,
                Origin = Anchor.BottomCentre,
                RelativeSizeAxes = Axes.X,
                Alpha = 0,
                Size = new Vector2(1, 150),
                Position = new Vector2(2000, -115),
                Children = new Drawable[]
                {
                    new Box()
                    {
                        Anchor = Anchor.BottomCentre,
                        Origin = Anchor.BottomCentre,
                        RelativeSizeAxes = Axes.Both,
                        Colour = Color4Extensions.FromHex("82767E")
                    },
                    new BasicButton()
                    {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        RelativeSizeAxes = Axes.Y,
                        Size = new Vector2(30, 1),
                        Position = new Vector2(15, 0),
                        Colour = Colour4.White,
                        Action = togglePreviousButton,
                        Child = new FillFlowContainer()
                        {
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                            RelativeSizeAxes = Axes.Both,
                            Direction = FillDirection.Horizontal,
                            Children = new Drawable[]
                            {
                                new SpriteIcon()
                                {
                                    Anchor = Anchor.CentreLeft,
                                    Origin = Anchor.CentreLeft,
                                    Size = new Vector2(20),
                                    Icon = FontAwesome.Solid.ChevronLeft,
                                    Colour = Color4Extensions.FromHex("D2C9D8"),
                                },
                                new SpriteIcon()
                                {
                                    Anchor = Anchor.CentreLeft,
                                    Origin = Anchor.CentreLeft,
                                    Size = new Vector2(20),
                                    Icon = FontAwesome.Solid.ChevronLeft,
                                    Colour = Color4Extensions.FromHex("D2C9D8"),
                                }
                            }
                        }
                    },
                    new BasicButton()
                    {
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.CentreRight,
                        RelativeSizeAxes = Axes.Y,
                        Size = new Vector2(30, 1),
                        Position = new Vector2(-15, 0),
                        Colour = Colour4.White,
                        Action = toggleNextButton,
                        Child = new FillFlowContainer()
                        {
                            Anchor = Anchor.CentreRight,
                            Origin = Anchor.CentreRight,
                            RelativeSizeAxes = Axes.Both,
                            Direction = FillDirection.Horizontal,
                            Children = new Drawable[]
                            {
                                new SpriteIcon()
                                {
                                    Anchor = Anchor.CentreRight,
                                    Origin = Anchor.CentreRight,
                                    Size = new Vector2(20),
                                    Icon = FontAwesome.Solid.ChevronRight,
                                    Colour = Color4Extensions.FromHex("D2C9D8"),
                                },
                                new SpriteIcon()
                                {
                                    Anchor = Anchor.CentreRight,
                                    Origin = Anchor.CentreRight,
                                    Size = new Vector2(20),
                                    Icon = FontAwesome.Solid.ChevronRight,
                                    Colour = Color4Extensions.FromHex("D2C9D8"),
                                }
                            }
                        }
                    },
                    finalSettingsSwiper
                }
            },
            // Idle details
            idleRenakoLogoContainer = new Container()
            {
                Anchor = Anchor.BottomLeft,
                Origin = Anchor.BottomLeft,
                Margin = new MarginPadding()
                {
                    Left = 20,
                    Bottom = 20
                },
                Height = 140,
                Alpha = 0,
                AutoSizeAxes = Axes.X,
                Child = new RenakoLogo()
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft
                }
            },
            rightBottomDetailsContainer = new RightBottomBeatmapSetDetailContainer()
            {
                Alpha = 0
            }
        };

        #region Global settings bindable changes

        useUnicodeInfo = config.GetBindable<bool>(RenakoSetting.UseUnicodeInfo);
        useUnicodeInfo.ValueChanged += delegate
        {
            if (useUnicodeInfo.Value)
            {
                songTitle.Title = workingBeatmap.BeatmapSet.TitleUnicode;
                songTitle.Description = workingBeatmap.BeatmapSet.ArtistUnicode;
                sourceText.Text = workingBeatmap.BeatmapSet.SourceUnicode;

                rightBottomDetailsContainer.Title = workingBeatmap.BeatmapSet.TitleUnicode;
                rightBottomDetailsContainer.Artist = workingBeatmap.BeatmapSet.ArtistUnicode;
                rightBottomDetailsContainer.Source = workingBeatmap.BeatmapSet.SourceUnicode;
            }
            else
            {
                songTitle.Title = workingBeatmap.BeatmapSet.Title;
                songTitle.Description = workingBeatmap.BeatmapSet.Artist;
                sourceText.Text = workingBeatmap.BeatmapSet.Source;

                rightBottomDetailsContainer.Title = workingBeatmap.BeatmapSet.Title;
                rightBottomDetailsContainer.Artist = workingBeatmap.BeatmapSet.Artist;
                rightBottomDetailsContainer.Source = workingBeatmap.BeatmapSet.Source;
            }
        };
        disableVideoBackground = config.GetBindable<bool>(RenakoSetting.DisableVideoPreview);
        disableVideoBackground.ValueChanged += delegate
        {
            if (workingBeatmap.BeatmapSet.HasVideo && workingBeatmap.BeatmapSet.VideoPath != null && !config.Get<bool>(RenakoSetting.DisableVideoPreview))
            {
                backgroundScreenStack.ChangeBackgroundVideo(BeatmapSetUtility.GetVideoPath(workingBeatmap.BeatmapSet), workingBeatmap.BeatmapSet.PreviewTime, workingBeatmap.BeatmapSet.TotalLength);
                backgroundScreenStack.SeekBackgroundVideo(renakoAudioManager.Track.CurrentTime);
            }
            else
            {
                backgroundScreenStack.HideBackgroundVideo(true);
            }
        };

        #endregion

        #region Beatmap bindable changes

        beatmapSetSwiper.CurrentItem.BindValueChanged(_ =>
        {
            isBeatmapChanged = false;
            lastBeatmapChangeTime = interactionTimer.CurrentTime;
        });
        beatmapSwiper.CurrentItem.BindValueChanged(_ =>
        {
            isBeatmapChanged = false;
            lastBeatmapChangeTime = interactionTimer.CurrentTime;
        });

        workingBeatmap.BindableWorkingBeatmapSet.BindValueChanged(item =>
        {
            if (item.NewValue == null) return;

            if (Equals(item.NewValue, item.OldValue)) return;

            beatmapSwiper.BeatmapList = beatmapsCollection.GetBeatmapsFromBeatmapSet(item.NewValue).ToList();
            beatmapSwiper.SetTexture(textureStore.Get(item.NewValue.CoverPath));
            beatmapSwiper.SetIndex(0);

            songTitle.Title = useUnicodeInfo.Value ? item.NewValue.TitleUnicode : item.NewValue.Title;
            songTitle.Description = useUnicodeInfo.Value ? item.NewValue.ArtistUnicode : item.NewValue.Artist;
            beatmapSetCreatorText.Text = item.NewValue.Creator;
            lengthText.Text = BeatmapSetUtility.GetFormattedTime(item.NewValue);
            sourceText.Text = useUnicodeInfo.Value ? item.NewValue.SourceUnicode : item.NewValue.Source;
            Dictionary<string, int> calculatedMinMax = beatmapsCollection.GetMixMaxDifficultyLevel(item.NewValue);
            totalBeatmapSetDifficultyText.Text = $"{calculatedMinMax["min"]} - {calculatedMinMax["max"]}";
            bpmText.Text = item.NewValue.BPM.ToString(CultureInfo.InvariantCulture);

            rightBottomDetailsContainer.Title = useUnicodeInfo.Value ? item.NewValue.TitleUnicode : item.NewValue.Title;
            rightBottomDetailsContainer.Artist = useUnicodeInfo.Value ? item.NewValue.ArtistUnicode : item.NewValue.Artist;
            rightBottomDetailsContainer.Source = useUnicodeInfo.Value ? item.NewValue.SourceUnicode : item.NewValue.Source;

            songTitle.Texture?.Dispose();
            rightBottomDetailsContainer.CoverImage?.Dispose();

            if (item.NewValue.UseLocalSource)
            {
                Texture texture = textureStore.Get(item.NewValue.CoverPath);
                songTitle.Texture = texture;
                rightBottomDetailsContainer.CoverImage = texture;
            }
            else
            {
                string coverPath = BeatmapSetUtility.GetCoverPath(item.NewValue);

                Texture coverTexture = textureStore.Get(coverPath);

                if (coverTexture == null)
                {
                    Stream coverTextureStream = host.Storage.GetStream(coverPath);
                    coverTexture = Texture.FromStream(host.Renderer, coverTextureStream);
                    beatmapSwiper.SetTexture(coverTexture);
                    coverTextureStream?.Close();
                }

                songTitle.Texture = coverTexture;
                rightBottomDetailsContainer.CoverImage = coverTexture;
            }

            Scheduler.Add(() => config.SetValue(RenakoSetting.LatestBeatmapSetID, item.NewValue.ID));
            isBeatmapChanged = true;

            if (beatmapsCollection.GetBeatmapsFromBeatmapSet(item.NewValue).Length >= 1)
            {
                workingBeatmap.Beatmap = beatmapsCollection.GetBeatmapsFromBeatmapSet(item.NewValue)[0];
            }

            // Video background
            if (item.NewValue.HasVideo && item.NewValue.VideoPath != null && !config.Get<bool>(RenakoSetting.DisableVideoPreview))
            {
                backgroundScreenStack.ChangeBackgroundVideo(BeatmapSetUtility.GetVideoPath(item.NewValue), item.NewValue.PreviewTime, item.NewValue.TotalLength);
            }
            else
            {
                backgroundScreenStack.HideBackgroundVideo(true);
            }

            // Disable the right bottom button if there is no beatmap in the beatmap set.
            if (beatmapsCollection.GetBeatmapsFromBeatmapSet(item.NewValue).Length < 1)
            {
                rightBottomButton.Enabled.Value = false;
                Scheduler.Add(() => rightBottomButton.ClearTransforms());
                workingBeatmap.Beatmap = null;
                Scheduler.Add(() => config.SetValue(RenakoSetting.LatestBeatmapID, default_beatmap_id));
            }
            else
            {
                rightBottomButton.Enabled.Value = true;
                rightBottomButton.FlashBackground(60000 / item.NewValue.BPM, true);
            }

            // Flash back button with speed using beatmapset's BPM
            backButton.FlashBackground(60000 / item.NewValue.BPM, true);

            lastInteractionTime = interactionTimer.CurrentTime;

            // Change track in visualizer
            if (renakoAudioManager.Track != null)
            {
                audioVisualizer.ChangeTrack(renakoAudioManager.Track);
                audioVisualizer.ChangeSpeedByBpm(item.NewValue.BPM == 0 ? 120 : item.NewValue.BPM);
            }
        }, true);
        workingBeatmap.BindableWorkingBeatmap.BindValueChanged(item =>
        {
            if (item.NewValue == null) return;

            if (Equals(item.NewValue, item.OldValue)) return;

            Scheduler.Add(() =>
            {
                beatmapInfoBox.FadeColour(RenakoColour.ForDifficultyLevel(item.NewValue.DifficultyRating), 500, Easing.OutQuart);
                beatmapDifficultyRatingText.FadeColour(RenakoColour.ForDifficultyLevel(item.NewValue.DifficultyRating).Darken(2f), 500, Easing.OutQuart);
                beatmapDifficultySpriteIcon.FadeColour(RenakoColour.ForDifficultyLevel(item.NewValue.DifficultyRating).Darken(2f), 500, Easing.OutQuart);
                beatmapLevelNameText.FadeColour(RenakoColour.ForDifficultyLevel(item.NewValue.DifficultyRating).Darken(2f), 500, Easing.OutQuart);
                beatmapLevelNameSpriteIcon.FadeColour(RenakoColour.ForDifficultyLevel(item.NewValue.DifficultyRating).Darken(2f), 500, Easing.OutQuart);
                beatmapCreatorIcon.Colour = RenakoColour.ForDifficultyLevel(item.NewValue.DifficultyRating).Darken(2f);
                beatmapLevelNameText.Text = item.NewValue.DifficultyName;
                beatmapDifficultyRatingText.Text = $"{item.NewValue.DifficultyRating:0.00}";
                beatmapCreatorText.Text = item.NewValue.Creator;

                beatmapNoteCountBox.FadeColour(RenakoColour.ForDifficultyLevel(item.NewValue.DifficultyRating), 500, Easing.OutQuart);
            });

            Scheduler.Add(() => config.SetValue(RenakoSetting.LatestBeatmapID, item.NewValue.ID));

            lastInteractionTime = interactionTimer.CurrentTime;
        }, true);

        #endregion

        #region State change action

        currentScreenState.BindValueChanged(e =>
        {
            if (e.OldValue == e.NewValue) return;

            if (e.OldValue == SongSelectionScreenState.SongList && e.NewValue == SongSelectionScreenState.BeatmapSelection)
            {
                songListContainer.MoveToX(-2000, 500, Easing.OutQuart)
                                 .FadeOut(500, Easing.OutQuart);
                beatmapSelectionContainer.MoveToX(0, 500, Easing.OutQuart)
                                         .FadeIn(500, Easing.OutQuart);
                beatmapInfoContainer.MoveToX(0, 500, Easing.OutQuart)
                                    .FadeIn(500, Easing.OutQuart);
                beatmapNoteCountContainer.MoveToX(0, 750, Easing.OutQuart)
                                         .FadeIn(750, Easing.OutQuart);
            }
            else if (e.OldValue == SongSelectionScreenState.BeatmapSelection && e.NewValue == SongSelectionScreenState.SongList)
            {
                beatmapSelectionContainer.MoveToX(2000, 500, Easing.OutQuart)
                                         .FadeOut(500, Easing.OutQuart);
                songListContainer.MoveToX(0, 500, Easing.OutQuart)
                                 .FadeIn(500, Easing.OutQuart);
                beatmapInfoContainer.MoveToX(-600, 750, Easing.OutQuart)
                                    .FadeOut(750, Easing.OutQuart);
                beatmapNoteCountContainer.MoveToX(-600, 500, Easing.OutQuart)
                                         .FadeOut(500, Easing.OutQuart);
            }
            else if (e.OldValue == SongSelectionScreenState.BeatmapSelection && e.NewValue == SongSelectionScreenState.LastSetting)
            {
                beatmapSelectionContainer.MoveToX(-2000, 500, Easing.OutQuart)
                                         .FadeOut(500, Easing.OutQuart);

                songTitle.ShowTexture();

                rightBottomButton.Text = "Let's start!";
                rightBottomButton.Icon = FontAwesome.Solid.Rocket;
                rightBottomButton.ResizeWidthTo(275, 200, Easing.OutQuart);

                finalSettingsContainer.MoveToX(0, 500, Easing.OutQuart)
                                      .FadeIn(500, Easing.OutQuart);
            }
            else if (e.OldValue == SongSelectionScreenState.LastSetting && e.NewValue == SongSelectionScreenState.BeatmapSelection)
            {
                beatmapSelectionContainer.MoveToX(0, 500, Easing.OutQuart)
                                         .FadeIn(500, Easing.OutQuart);
                finalSettingsContainer.MoveToX(2000, 500, Easing.OutQuart)
                                      .FadeOut(500, Easing.OutQuart);

                songTitle.HideTexture();

                rightBottomButton.Text = "Go!";
                rightBottomButton.Icon = FontAwesome.Solid.ArrowRight;
                rightBottomButton.ResizeWidthTo(200, 200, Easing.OutQuart);
            }
        }, true);

        #endregion

        #region Idle mode action

        isHiding.BindValueChanged(e =>
        {
            if (config.GetBindable<bool>(RenakoSetting.DisableIdleMode).Value) return;

            if (e.NewValue)
            {
                songTitleContainer.MoveToX(-600, 500, Easing.OutQuart)
                                  .FadeOut(500, Easing.OutQuart);
                songListContainer.MoveToY(600, 750, Easing.OutQuart)
                                 .FadeOut(600, Easing.OutQuart);
                beatmapSelectionContainer.MoveToY(600, 750, Easing.OutQuart);
                finalSettingsContainer.MoveToY(600, 750, Easing.OutQuart);

                backButton.FadeOut(500, Easing.OutQuart);
                rightBottomButton.FadeOut(500, Easing.OutQuart);

                idleRenakoLogoContainer.FadeIn(1000, Easing.OutQuart);
                rightBottomDetailsContainer.FadeIn(1000, Easing.OutQuart);

                backgroundScreenStack.AdjustMaskAlpha(0.25f);

                audioVisualizer.FadeOut(500, Easing.OutQuart);
            }
            else
            {
                songTitleContainer.MoveToX(-MenuButton.CONTAINER_PADDING, 500, Easing.OutQuart)
                                  .FadeIn(500, Easing.OutQuart);
                songListContainer.MoveToY(-115, 750, Easing.OutBack)
                                 .FadeIn(750, Easing.OutBack);
                beatmapSelectionContainer.MoveToY(-115, 750, Easing.OutBack);
                finalSettingsContainer.MoveToY(-115, 750, Easing.OutBack);

                backButton.FadeIn(500, Easing.OutQuart);
                rightBottomButton.FadeIn(500, Easing.OutQuart);

                idleRenakoLogoContainer.FadeOut(250, Easing.OutQuart);
                rightBottomDetailsContainer.FadeOut(250, Easing.OutQuart);

                backgroundScreenStack.AdjustMaskAlpha(0f);

                audioVisualizer.FadeIn(500, Easing.OutQuart);
            }
        }, true);

        #endregion
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();
        songTitle.HideTexture();
        interactionTimer.Start();

        finalSettingsSwiper.Items =
        [
            new BindableSettingsSwiperItem("Scroll Speed", config.GetBindable<int>(RenakoSetting.ScrollSpeed))
            {
                MinValue = 1,
                MaxValue = 20,
                IncrementStep = 1,
                Setting = RenakoSetting.ScrollSpeed
            },
            new BindableSettingsSwiperItem("Background Dim", config.GetBindable<int>(RenakoSetting.PlayfieldBackgroundDim))
            {
                MinValue = 0,
                MaxValue = 100,
                IncrementStep = 1,
                Unit = "%",
                Setting = RenakoSetting.PlayfieldBackgroundDim
            }
        ];

        modeBackgroundBox.FlashColour(Color4Extensions.FromHex("f7f0f4"), 600, Easing.OutCirc).Delay(1000).Loop();
        songTitle.BackgroundBox.Delay(100).FlashColour(Color4Extensions.FromHex("f7f0f4"), 600, Easing.OutCirc).Delay(900).Loop();
        sourceBackgroundBox.Delay(200).FlashColour(Color4Extensions.FromHex("f7f0f4"), 600, Easing.OutCirc).Delay(800).Loop();
        songInfoBackgroundBox.Delay(300).FlashColour(Color4Extensions.FromHex("f7f0f4"), 600, Easing.OutCirc).Delay(700).Loop();
        beatmapInfoBox.Delay(400).FlashColour(Color4Extensions.FromHex("f7f0f4"), 600, Easing.OutCirc).Delay(600).Loop();
    }

    protected override void Update()
    {
        if (lastBeatmapChangeTime + 200 < interactionTimer.CurrentTime && !isBeatmapChanged)
        {
            if (currentScreenState.Value == SongSelectionScreenState.SongList)
                workingBeatmap.BeatmapSet = beatmapSetSwiper.CurrentItem.Value;
            else if (currentScreenState.Value == SongSelectionScreenState.BeatmapSelection)
                workingBeatmap.Beatmap = beatmapSwiper.CurrentItem.Value;
            lastBeatmapChangeTime = interactionTimer.CurrentTime;
            isBeatmapChanged = true;
        }

        isHiding.Value = lastInteractionTime + INTERACTION_TIMEOUT < interactionTimer.CurrentTime;

        base.Update();
    }

    public override void OnEntering(ScreenTransitionEvent e)
    {
        this.FadeIn(500, Easing.OutQuart);
        songTitleContainer.MoveToX(-MenuButton.CONTAINER_PADDING, 500, Easing.OutQuart);
        songListContainer.MoveToY(-115, 750, Easing.OutBack);

        base.OnEntering(e);
    }

    public override bool OnExiting(ScreenExitEvent e)
    {
        interactionTimer.Stop();
        interactionTimer.Reset();
        this.FadeOut(500, Easing.OutQuart);
        songTitleContainer.MoveToX(-600, 500, Easing.OutQuart);
        songListContainer.MoveToY(600, 750, Easing.OutQuart);

        return base.OnExiting(e);
    }

    public override void OnSuspending(ScreenTransitionEvent e)
    {
        interactionTimer.Stop();
        backgroundScreenStack.HideBackgroundVideo();
        base.OnSuspending(e);
    }

    public override void OnResuming(ScreenTransitionEvent e)
    {
        base.OnResuming(e);
        interactionTimer.Start();
        backgroundScreenStack.AdjustMaskAlpha(0f);

        if (backgroundScreenStack.HaveBackgroundVideo())
        {
            backgroundScreenStack.ShowBackgroundVideo();
        }
    }

    private void toggleNextButton()
    {
        if (isHiding.Value)
        {
            isHiding.Value = false;
            return;
        }

        if (currentScreenState.Value == SongSelectionScreenState.SongList)
            beatmapSetSwiper.Next();
        else if (currentScreenState.Value == SongSelectionScreenState.BeatmapSelection)
            beatmapSwiper.Next();
        else if (currentScreenState.Value == SongSelectionScreenState.LastSetting)
            finalSettingsSwiper.Next();
        leftClickSample?.Play();
    }

    private void togglePreviousButton()
    {
        if (isHiding.Value)
        {
            isHiding.Value = false;
            return;
        }

        if (currentScreenState.Value == SongSelectionScreenState.SongList)
            beatmapSetSwiper.Previous();
        else if (currentScreenState.Value == SongSelectionScreenState.BeatmapSelection)
            beatmapSwiper.Previous();
        else if (currentScreenState.Value == SongSelectionScreenState.LastSetting)
            finalSettingsSwiper.Previous();
        rightClickSample?.Play();
    }

    private void toggleBackButton()
    {
        if (isHiding.Value)
        {
            isHiding.Value = false;
            return;
        }

        switch (currentScreenState.Value)
        {
            case SongSelectionScreenState.SongList:
                this.Exit();
                break;

            case SongSelectionScreenState.BeatmapSelection:
                currentScreenState.Value = SongSelectionScreenState.SongList;
                break;

            case SongSelectionScreenState.LastSetting:
                currentScreenState.Value = SongSelectionScreenState.BeatmapSelection;
                break;

            default:
                this.Exit();
                break;
        }

        backClickSample?.Play();
    }

    private void toggleUpButton()
    {
        if (isHiding.Value)
        {
            isHiding.Value = false;
            return;
        }

        if (currentScreenState.Value == SongSelectionScreenState.LastSetting)
        {
            finalSettingsSwiper.IncrementCurrentItem();
            leftClickSample?.Play();
        }
    }

    private void toggleDownButton()
    {
        if (isHiding.Value)
        {
            isHiding.Value = false;
            return;
        }

        if (currentScreenState.Value == SongSelectionScreenState.LastSetting)
        {
            finalSettingsSwiper.DecrementCurrentItem();
            rightClickSample?.Play();
        }
    }

    private void toggleGoButton()
    {
        if (isHiding.Value)
        {
            isHiding.Value = false;
            return;
        }

        if (beatmapsCollection.GetBeatmapsFromBeatmapSet(workingBeatmap.BeatmapSet).Length < 1)
            return;

        switch (currentScreenState.Value)
        {
            case SongSelectionScreenState.SongList:
                currentScreenState.Value = SongSelectionScreenState.BeatmapSelection;
                break;

            case SongSelectionScreenState.BeatmapSelection:
                currentScreenState.Value = SongSelectionScreenState.LastSetting;
                break;

            case SongSelectionScreenState.LastSetting:
                mainScreenStack?.Push(new PlayerLoadingScreen(true));
                backgroundScreenStack.AdjustMaskAlpha(0.5f);
                break;

            default:
                currentScreenState.Value = SongSelectionScreenState.BeatmapSelection;
                break;
        }

        goClickSample?.Play();
    }

    protected override bool OnKeyDown(KeyDownEvent e)
    {
        switch (e.Key)
        {
            case Key.Right:
                toggleNextButton();
                break;

            case Key.Left:
                togglePreviousButton();
                break;

            case Key.Escape:
                toggleBackButton();
                break;

            case Key.Enter or Key.P:
                toggleGoButton();
                break;

            case Key.Up:
                toggleUpButton();
                break;

            case Key.Down:
                toggleDownButton();
                break;
        }

        lastInteractionTime = interactionTimer.CurrentTime;

        return base.OnKeyDown(e);
    }

    protected override bool OnJoystickPress(JoystickPressEvent e)
    {
        switch (e.Button)
        {
            case JoystickButton.FirstHatRight:
                toggleNextButton();
                break;

            case JoystickButton.FirstHatLeft:
                togglePreviousButton();
                break;

            case JoystickButton.Button9 or JoystickButton.GamePadB or JoystickButton.GamePadBack or JoystickButton.GamePadLeftShoulder:
                toggleBackButton();
                break;

            case JoystickButton.Button10 or JoystickButton.GamePadA or JoystickButton.GamePadStart or JoystickButton.GamePadRightShoulder:
                toggleGoButton();
                break;

            case JoystickButton.FirstHatUp:
                toggleUpButton();
                break;

            case JoystickButton.FirstHatDown:
                toggleDownButton();
                break;
        }

        lastInteractionTime = interactionTimer.CurrentTime;

        Logger.Log($"Joystick button: {e.Button}");

        return base.OnJoystickPress(e);
    }

    protected override bool OnScroll(ScrollEvent e)
    {
        switch (e.ScrollDelta.Y)
        {
            case > 0:
                togglePreviousButton();
                break;

            case < 0:
                toggleNextButton();
                break;
        }

        lastInteractionTime = interactionTimer.CurrentTime;

        return base.OnScroll(e);
    }

    protected override bool OnMouseDown(MouseDownEvent e)
    {
        lastInteractionTime = interactionTimer.CurrentTime;

        return base.OnMouseDown(e);
    }
}

internal enum SongSelectionScreenState
{
    SongList,
    BeatmapSelection,
    LastSetting
}

using System.Collections.Generic;
using System.Globalization;
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
using osu.Framework.Input.Events;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osu.Framework.Timing;
using osuTK;
using osuTK.Input;
using Renako.Game.Beatmaps;
using Renako.Game.Configurations;
using Renako.Game.Graphics.Drawables;
using Renako.Game.Graphics.UserInterface;
using Renako.Game.Utilities;

namespace Renako.Game.Graphics.Screens;

public partial class SongSelectionScreen : RenakoScreen
{
    private FillFlowContainer songTitleContainer;
    private Container songListContainer;
    private Container beatmapSelectionContainer;
    private readonly Bindable<SongSelectionScreenState> currentScreenState = new Bindable<SongSelectionScreenState>();

    [Resolved]
    private BeatmapsCollection beatmapsCollection { get; set; }

    [Resolved]
    private WorkingBeatmap workingBeatmap { get; set; }

    [Resolved]
    private GameHost host { get; set; }

    private HorizontalTextureSwiper<BeatmapSet> beatmapSetSwiper;
    private List<TextureSwiperItem<BeatmapSet>> beatmapSetSwiperItemList;
    private BeatmapSelectionSwiper beatmapSwiper;
    private List<Beatmap> beatmapList;
    private MenuTitleWithTexture songTitle;
    private SpriteText sourceText;
    private SpriteText totalBeatmapSetDifficultyText;
    private SpriteText bpmText;
    private SpriteText creatorText;
    private SpriteText lengthText;
    private Container beatmapInfoContainer;
    private Box beatmapInfoBox;
    private SpriteIcon beatmapDifficultySpriteIcon;
    private SpriteText beatmapDifficultyRatingText;
    private SpriteIcon beatmapLevelNameSpriteIcon;
    private SpriteText beatmapLevelNameText;
    private readonly StopwatchClock beatmapChangeTimer = new StopwatchClock();

    private RightBottomButton rightBottomButton;

    private double lastBeatmapChangeTime;
    private bool isBeatmapChanged;

    private Sample leftClickSample;
    private Sample rightClickSample;
    private Sample backClickSample;
    private Sample goClickSample;
    private TextureStore textureStore;

    private Bindable<bool> useUnicodeInfo;

    private const int icon_size = 13;
    private const int song_description_font_size = 15;

    private const int default_beatmapset_id = 0;
    private const int default_beatmap_id = 0;

    [BackgroundDependencyLoader]
    private void load(TextureStore textureStore, RenakoConfigManager config, AudioManager audioManager)
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

        workingBeatmap.BeatmapSet = config.Get<int>(RenakoSetting.LatestBeatmapSetID) == 0 ? beatmapsCollection.GetBeatmapSetByID(default_beatmapset_id) : beatmapsCollection.GetBeatmapSetByID(config.Get<int>(RenakoSetting.LatestBeatmapSetID));
        workingBeatmap.Beatmap = config.Get<int>(RenakoSetting.LatestBeatmapID) == 0 ? beatmapsCollection.GetBeatmapByID(default_beatmap_id) : beatmapsCollection.GetBeatmapByID(config.Get<int>(RenakoSetting.LatestBeatmapID));

        foreach (BeatmapSet beatmapSet in beatmapsCollection.BeatmapSets)
        {
            Texture texture;

            if (beatmapSet.UseLocalSource)
            {
                texture = textureStore.Get(beatmapSet.CoverPath);
            }
            else
            {
                // TODO: Create texture atlas dedicate for stream
                texture = textureStore.Get(BeatmapSetUtility.GetCoverPath(beatmapSet)) ?? Texture.FromStream(host.Renderer, host.Storage.GetStream(BeatmapSetUtility.GetCoverPath(beatmapSet)));
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
            new BackButton()
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
                Size = new Vector2(1f, 0.45f),
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
                            new Box()
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
                        Title = "Innocence (TV Size)",
                        Description = "Eir Aoi"
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
                            new Box()
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
                            new Box()
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
                                            creatorText = new SpriteText()
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
                    // Beatmap info
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
            }
        };

        useUnicodeInfo = config.GetBindable<bool>(RenakoSetting.UseUnicodeInfo);
        useUnicodeInfo.ValueChanged += delegate
        {
            if (useUnicodeInfo.Value)
            {
                songTitle.Title = workingBeatmap.BeatmapSet.TitleUnicode;
                songTitle.Description = workingBeatmap.BeatmapSet.ArtistUnicode;
                sourceText.Text = workingBeatmap.BeatmapSet.SourceUnicode;
            }
            else
            {
                songTitle.Title = workingBeatmap.BeatmapSet.Title;
                songTitle.Description = workingBeatmap.BeatmapSet.Artist;
                sourceText.Text = workingBeatmap.BeatmapSet.Source;
            }
        };

        beatmapSetSwiper.CurrentItem.BindValueChanged(_ =>
        {
            isBeatmapChanged = false;
            lastBeatmapChangeTime = beatmapChangeTimer.CurrentTime;
        });
        beatmapSwiper.CurrentItem.BindValueChanged(_ =>
        {
            isBeatmapChanged = false;
            lastBeatmapChangeTime = beatmapChangeTimer.CurrentTime;
        });

        workingBeatmap.BindableWorkingBeatmapSet.BindValueChanged(item =>
        {
            if (item.NewValue == null) return;

            beatmapSwiper.BeatmapList = beatmapsCollection.GetBeatmapsFromBeatmapSet(item.NewValue).ToList();
            beatmapSwiper.SetTexture(textureStore.Get(item.NewValue.CoverPath));
            beatmapSwiper.SetIndex(0);

            songTitle.Title = useUnicodeInfo.Value ? item.NewValue.TitleUnicode : item.NewValue.Title;
            songTitle.Description = useUnicodeInfo.Value ? item.NewValue.ArtistUnicode : item.NewValue.Artist;
            creatorText.Text = item.NewValue.Creator;
            lengthText.Text = BeatmapSetUtility.GetFormattedTime(item.NewValue);
            sourceText.Text = useUnicodeInfo.Value ? item.NewValue.SourceUnicode : item.NewValue.Source;
            Dictionary<string, int> calculatedMinMix = beatmapsCollection.GetMixMaxDifficultyLevel(item.NewValue);
            totalBeatmapSetDifficultyText.Text = $"{calculatedMinMix["min"]} - {calculatedMinMix["max"]}";
            bpmText.Text = item.NewValue.BPM.ToString(CultureInfo.InvariantCulture);
            songTitle.Texture = textureStore.Get(BeatmapSetUtility.GetCoverPath(item.NewValue)) ?? Texture.FromStream(host.Renderer, host.Storage.GetStream(BeatmapSetUtility.GetCoverPath(item.NewValue)));

            Scheduler.Add(() => config.SetValue(RenakoSetting.LatestBeatmapSetID, item.NewValue.ID));
            isBeatmapChanged = true;
        }, true);
        workingBeatmap.BindableWorkingBeatmap.BindValueChanged(item =>
        {
            if (item.NewValue == null) return;

            Scheduler.Add(() =>
            {
                beatmapInfoBox.FadeColour(RenakoColour.ForDifficultyLevel(item.NewValue.DifficultyRating), 500, Easing.OutQuart);
                beatmapDifficultyRatingText.FadeColour(RenakoColour.ForDifficultyLevel(item.NewValue.DifficultyRating).Darken(2f), 500, Easing.OutQuart);
                beatmapDifficultySpriteIcon.FadeColour(RenakoColour.ForDifficultyLevel(item.NewValue.DifficultyRating).Darken(2f), 500, Easing.OutQuart);
                beatmapLevelNameText.FadeColour(RenakoColour.ForDifficultyLevel(item.NewValue.DifficultyRating).Darken(2f), 500, Easing.OutQuart);
                beatmapLevelNameSpriteIcon.FadeColour(RenakoColour.ForDifficultyLevel(item.NewValue.DifficultyRating).Darken(2f), 500, Easing.OutQuart);
                beatmapLevelNameText.Text = item.NewValue.DifficultyName;
                beatmapDifficultyRatingText.Text = $"{item.NewValue.DifficultyRating:0.00}";
            });

            Scheduler.Add(() => config.SetValue(RenakoSetting.LatestBeatmapID, item.NewValue.ID));
        }, true);

        currentScreenState.BindValueChanged(e =>
        {
            if (e.OldValue == e.NewValue) return;

            if (e.OldValue == SongSelectionScreenState.SongList && e.NewValue == SongSelectionScreenState.BeatmapSelection)
            {
                songListContainer.MoveToX(-2000, 500, Easing.OutQuart);
                songListContainer.FadeOut(500, Easing.OutQuart);
                beatmapSelectionContainer.MoveToX(0, 500, Easing.OutQuart);
                beatmapSelectionContainer.FadeIn(500, Easing.OutQuart);
                beatmapInfoContainer.MoveToX(0, 500, Easing.OutQuart);
                beatmapInfoContainer.FadeIn(500, Easing.OutQuart);
            }
            else if (e.OldValue == SongSelectionScreenState.BeatmapSelection && e.NewValue == SongSelectionScreenState.SongList)
            {
                beatmapSelectionContainer.MoveToX(2000, 500, Easing.OutQuart);
                beatmapSelectionContainer.FadeOut(500, Easing.OutQuart);
                songListContainer.MoveToX(0, 500, Easing.OutQuart);
                songListContainer.FadeIn(500, Easing.OutQuart);
                beatmapInfoContainer.MoveToX(-600, 500, Easing.OutQuart);
                beatmapInfoContainer.FadeOut(500, Easing.OutQuart);
            }
            else if (e.OldValue == SongSelectionScreenState.BeatmapSelection && e.NewValue == SongSelectionScreenState.LastSetting)
            {
                beatmapSelectionContainer.MoveToX(-2000, 500, Easing.OutQuart);
                beatmapSelectionContainer.FadeOut(500, Easing.OutQuart);

                songTitle.ShowTexture();

                rightBottomButton.Text = "Let's start!";
                rightBottomButton.Icon = FontAwesome.Solid.Rocket;
                rightBottomButton.ResizeWidthTo(275, 200, Easing.OutQuart);
            }
            else if (e.OldValue == SongSelectionScreenState.LastSetting && e.NewValue == SongSelectionScreenState.BeatmapSelection)
            {
                beatmapSelectionContainer.MoveToX(0, 500, Easing.OutQuart);
                beatmapSelectionContainer.FadeIn(500, Easing.OutQuart);

                songTitle.HideTexture();

                rightBottomButton.Text = "Go!";
                rightBottomButton.Icon = FontAwesome.Solid.ArrowRight;
                rightBottomButton.ResizeWidthTo(200, 200, Easing.OutQuart);
            }
        }, true);

        songTitle.HideTexture();
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();

        beatmapChangeTimer.Start();
    }

    protected override void Update()
    {
        if (lastBeatmapChangeTime + 200 < beatmapChangeTimer.CurrentTime && !isBeatmapChanged)
        {
            if (currentScreenState.Value == SongSelectionScreenState.SongList)
                workingBeatmap.BeatmapSet = beatmapSetSwiper.CurrentItem.Value;
            else if (currentScreenState.Value == SongSelectionScreenState.BeatmapSelection)
                workingBeatmap.Beatmap = beatmapSwiper.CurrentItem.Value;
            lastBeatmapChangeTime = beatmapChangeTimer.CurrentTime;
            isBeatmapChanged = true;
        }

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
        beatmapChangeTimer.Stop();
        beatmapChangeTimer.Reset();
        this.FadeOut(500, Easing.OutQuart);
        songTitleContainer.MoveToX(-600, 500, Easing.OutQuart);
        songListContainer.MoveToY(600, 750, Easing.OutQuart);

        return base.OnExiting(e);
    }

    private void toggleNextButton()
    {
        if (currentScreenState.Value == SongSelectionScreenState.SongList)
            beatmapSetSwiper.Next();
        else if (currentScreenState.Value == SongSelectionScreenState.BeatmapSelection)
            beatmapSwiper.Next();
        leftClickSample?.Play();
    }

    private void togglePreviousButton()
    {
        if (currentScreenState.Value == SongSelectionScreenState.SongList)
            beatmapSetSwiper.Previous();
        else if (currentScreenState.Value == SongSelectionScreenState.BeatmapSelection)
            beatmapSwiper.Previous();
        rightClickSample?.Play();
    }

    private void toggleBackButton()
    {
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

    private void toggleGoButton()
    {
        switch (currentScreenState.Value)
        {
            case SongSelectionScreenState.SongList:
                currentScreenState.Value = SongSelectionScreenState.BeatmapSelection;
                break;

            case SongSelectionScreenState.BeatmapSelection:
                currentScreenState.Value = SongSelectionScreenState.LastSetting;
                break;

            case SongSelectionScreenState.LastSetting:
                // TODO: Go to gameplay loading screen
                break;

            default:
                currentScreenState.Value = SongSelectionScreenState.BeatmapSelection;
                break;
        }

        goClickSample?.Play();
    }

    protected override bool OnKeyDown(KeyDownEvent e)
    {
        if (e.Key == Key.Right)
        {
            toggleNextButton();
        }
        else if (e.Key == Key.Left)
        {
            togglePreviousButton();
        }
        else if (e.Key == Key.Escape)
        {
            toggleBackButton();
        }
        else if (e.Key == Key.Enter)
        {
            toggleGoButton();
        }

        return base.OnKeyDown(e);
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

        return base.OnScroll(e);
    }
}

internal enum SongSelectionScreenState
{
    SongList,
    BeatmapSelection,
    LastSetting
}

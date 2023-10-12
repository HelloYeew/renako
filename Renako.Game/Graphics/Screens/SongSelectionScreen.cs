using System.Collections.Generic;
using System.Globalization;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osu.Framework.Timing;
using osuTK;
using osuTK.Input;
using Renako.Game.Beatmaps;
using Renako.Game.Configurations;
using Renako.Game.Graphics.Drawables;
using Renako.Game.Graphics.UserInterface;
using Renako.Game.Stores;
using Renako.Game.Utilities;

namespace Renako.Game.Graphics.Screens;

public partial class SongSelectionScreen : RenakoScreen
{
    private FillFlowContainer songTitleContainer;
    private Container songListContainer;
    private Button chevronLeftButton;
    private Button chevronRightButton;

    [Resolved]
    private BeatmapsCollection beatmapsCollection { get; set; }

    [Resolved]
    private WorkingBeatmap workingBeatmap { get; set; }

    private HorizontalTextureSwiper<BeatmapSet> beatmapSetSwiper;
    private List<TextureSwiperItem<BeatmapSet>> beatmapSetSwiperItemList;
    private MenuTitle songTitle;
    private SpriteText sourceText;
    private SpriteText totalBeatmapSetDifficultyText;
    private SpriteText bpmText;
    private SpriteText creatorText;
    private SpriteText lengthText;
    private StopwatchClock beatmapChangeTimer = new StopwatchClock();
    private double lastBeatmapChangeTime = 0;
    private bool isBeatmapChanged = false;

    private Bindable<bool> useUnicodeInfo;

    private const int icon_size = 13;
    private const int song_description_font_size = 15;

    private const int default_beatmapset_id = 0;
    private const int default_beatmap_id = 0;

    [BackgroundDependencyLoader]
    private void load(TextureStore textureStore, RenakoConfigManager config)
    {
        beatmapChangeTimer.Start();

        beatmapSetSwiperItemList = new List<TextureSwiperItem<BeatmapSet>>();
        beatmapSetSwiper = new HorizontalTextureSwiper<BeatmapSet>()
        {
            Items = beatmapSetSwiperItemList,
            Position = new Vector2(0, 0)
        };

        workingBeatmap.BeatmapSet = config.Get<int>(RenakoSetting.LatestBeatmapSetID) == 0 ? beatmapsCollection.GetBeatmapSetByID(default_beatmapset_id) : beatmapsCollection.GetBeatmapSetByID(config.Get<int>(RenakoSetting.LatestBeatmapSetID));
        workingBeatmap.Beatmap = config.Get<int>(RenakoSetting.LatestBeatmapID) == 0 ? beatmapsCollection.GetBeatmapByID(default_beatmap_id) : beatmapsCollection.GetBeatmapByID(config.Get<int>(RenakoSetting.LatestBeatmapID));

        foreach (BeatmapSet beatmapSet in beatmapsCollection.BeatmapSets)
        {
            beatmapSetSwiperItemList.Add(new TextureSwiperItem<BeatmapSet>()
            {
                Item = beatmapSet,
                Texture = textureStore.Get(beatmapSet.CoverPath)
            });
        }

        if (workingBeatmap.BeatmapSet != null)
            beatmapSetSwiper.CurrentIndex = beatmapSetSwiperItemList.FindIndex(x => x.Item == workingBeatmap.BeatmapSet);

        Alpha = 0;
        InternalChildren = new Drawable[]
        {
            new BackButton()
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Action = this.Exit
            },
            new RightBottomButton()
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Icon = FontAwesome.Solid.ArrowRight,
                Text = "Go!"
            },
            songTitleContainer = new FillFlowContainer()
            {
                Anchor = Anchor.TopLeft,
                Origin = Anchor.TopLeft,
                RelativeSizeAxes = Axes.Both,
                Direction = FillDirection.Vertical,
                Spacing = new Vector2(0, 14),
                Position = new Vector2(-600, 0),
                Size = new Vector2(1f, 0.4f),
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
                    songTitle = new MenuTitle()
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
                    chevronLeftButton = new BasicButton()
                    {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        Size = new Vector2(30, 20),
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
                    chevronRightButton = new BasicButton()
                    {
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.CentreRight,
                        Size = new Vector2(30, 20),
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

        beatmapSetSwiper.CurrentItem.BindValueChanged((item) =>
        {
            isBeatmapChanged = false;
            lastBeatmapChangeTime = beatmapChangeTimer.CurrentTime;
        });
        workingBeatmap.BindableWorkingBeatmapSet.BindValueChanged((item) =>
        {
            if (item.NewValue == null) return;

            songTitle.Title = useUnicodeInfo.Value ? item.NewValue.TitleUnicode : item.NewValue.Title;
            songTitle.Description = useUnicodeInfo.Value ? item.NewValue.ArtistUnicode : item.NewValue.Artist;
            creatorText.Text = item.NewValue.Creator;
            lengthText.Text = BeatmapSetUtility.GetFormattedTime(item.NewValue);
            sourceText.Text = useUnicodeInfo.Value ? item.NewValue.SourceUnicode : item.NewValue.Source;
            Dictionary<string, int> calculatedMinMix = beatmapsCollection.GetMixMaxDifficultyLevel(item.NewValue);
            totalBeatmapSetDifficultyText.Text = $"{calculatedMinMix["min"]} - {calculatedMinMix["max"]}";
            bpmText.Text = item.NewValue.BPM.ToString(CultureInfo.InvariantCulture);

            Scheduler.Add(() => config.SetValue(RenakoSetting.LatestBeatmapSetID, item.NewValue.ID));
            isBeatmapChanged = true;
        }, true);

        workingBeatmap.BindableWorkingBeatmap.BindValueChanged((item) =>
        {
            if (item.NewValue == null) return;

            Scheduler.Add(() => config.SetValue(RenakoSetting.LatestBeatmapID, item.NewValue.ID));
        });
    }

    protected override void Update()
    {
        if (lastBeatmapChangeTime + 200 < beatmapChangeTimer.CurrentTime && !isBeatmapChanged)
        {
            workingBeatmap.BeatmapSet = beatmapSetSwiper.CurrentItem.Value;
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
        beatmapSetSwiper.Next();
    }

    private void togglePreviousButton()
    {
        beatmapSetSwiper.Previous();
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

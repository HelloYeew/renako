using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Screens;
using osuTK;
using Renako.Game.Graphics.Drawables;

namespace Renako.Game.Graphics.Screens;

public partial class SongSelectionScreen : RenakoScreen
{
    private FillFlowContainer songTitleContainer;
    private Container songListContainer;
    private Container chevronLeftContainer;
    private Container chevronRightContainer;

    private const int icon_size = 13;
    private const int song_description_font_size = 15;

    [BackgroundDependencyLoader]
    private void load(TextureStore textureStore)
    {
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
                Size = new Vector2(1f, 0.3f),
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
                    new MenuTitle()
                    {
                        ButtonWidth = 0.35f,
                        BackgroundColor = Color4Extensions.FromHex("F2DFE9"),
                        TitleColor = Color4Extensions.FromHex("67344D"),
                        DescriptionColor = Color4Extensions.FromHex("251319"),
                        AutoUpperCaseTitle = false,
                        Title = "Innocence",
                        Description = "Eir Aoi"
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
                                                Colour = Color4Extensions.FromHex("67344D")
                                            },
                                            new SpriteText()
                                            {
                                                Anchor = Anchor.CentreLeft,
                                                Origin = Anchor.CentreLeft,
                                                Font = RenakoFont.GetFont(RenakoFont.Typeface.MPlus1P, song_description_font_size),
                                                Text = "HelloYeew",
                                                Colour = Color4Extensions.FromHex("251319")
                                            },
                                            new SpriteIcon()
                                            {
                                                Anchor = Anchor.CentreLeft,
                                                Origin = Anchor.CentreLeft,
                                                Size = new Vector2(icon_size),
                                                Icon = FontAwesome.Solid.Rocket,
                                                Colour = Color4Extensions.FromHex("67344D")
                                            },
                                            new SpriteText()
                                            {
                                                Anchor = Anchor.CentreLeft,
                                                Origin = Anchor.CentreLeft,
                                                Font = RenakoFont.GetFont(RenakoFont.Typeface.MPlus1P, song_description_font_size),
                                                Text = "2-12",
                                                Colour = Color4Extensions.FromHex("251319")
                                            },
                                            new SpriteIcon()
                                            {
                                                Anchor = Anchor.CentreLeft,
                                                Origin = Anchor.CentreLeft,
                                                Size = new Vector2(icon_size),
                                                Icon = FontAwesome.Solid.Clock,
                                                Colour = Color4Extensions.FromHex("67344D")
                                            },
                                            new SpriteText()
                                            {
                                                Anchor = Anchor.CentreLeft,
                                                Origin = Anchor.CentreLeft,
                                                Font = RenakoFont.GetFont(RenakoFont.Typeface.MPlus1P, song_description_font_size),
                                                Text = "03:33",
                                                Colour = Color4Extensions.FromHex("251319")
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    },
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
                    chevronLeftContainer = new Container()
                    {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        Size = new Vector2(30, 20),
                        Position = new Vector2(15, 0),
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
                    chevronRightContainer = new Container()
                    {
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.CentreRight,
                        Size = new Vector2(30, 20),
                        Position = new Vector2(-15, 0),
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
                    new Container()
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Size = new Vector2(220, 220),
                        Masking = true,
                        CornerRadius = 10,
                        FillMode = FillMode.Fill,
                        Rotation = 10,
                        Child = new Sprite()
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            RelativeSizeAxes = Axes.Both,
                            Texture = textureStore.Get("Beatmaps/Album/innocence.jpg")
                        }
                    }
                }
            }
        };
    }

    public override void OnEntering(ScreenTransitionEvent e)
    {
        this.FadeIn(500, Easing.OutQuart);
        songTitleContainer.MoveToX(-MenuButton.CONTAINER_PADDING, 500, Easing.OutQuart);
        songListContainer.MoveToY(-115, 750, Easing.OutBack);

        base.OnEntering(e);
    }
}

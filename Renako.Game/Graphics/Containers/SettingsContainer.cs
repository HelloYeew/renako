using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;
using Renako.Game.Graphics.Drawables;

namespace Renako.Game.Graphics.Containers;

/// <summary>
/// A settings overlay container that's block input to drawables behind it.
/// </summary>
public partial class SettingsContainer : FocusedOverlayContainer
{
    private Container menuTitleContainer;
    private FillFlowContainer tipsContainer;

    protected override bool BlockNonPositionalInput => false;
    protected override bool BlockScrollInput => false;

    [BackgroundDependencyLoader]
    private void load()
    {
        Alpha = 0;
        InternalChildren = new Drawable[]
        {
            new Box()
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                Colour = Color4Extensions.FromHex("19191D"),
                Alpha = 0.85f
            },
            menuTitleContainer = new Container()
            {
                Anchor = Anchor.TopLeft,
                Origin = Anchor.TopLeft,
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(1f, 0.225f),
                Position = new Vector2(-600, 0),
                Child = new MenuTitle()
                {
                    ButtonWidth = 0.45f,
                    BackgroundColor = Color4Extensions.FromHex("E0E1F0"),
                    TitleColor = Color4Extensions.FromHex("403F75"),
                    DescriptionColor = Color4Extensions.FromHex("171A2D"),
                    Title = "Settings".ToUpper(),
                    Description = "Get ready for fight!"
                }
            },
            tipsContainer = new FillFlowContainer()
            {
                Anchor = Anchor.TopRight,
                Origin = Anchor.TopRight,
                RelativeSizeAxes = Axes.X,
                Direction = FillDirection.Vertical,
                Spacing = new Vector2(0, 16),
                Position = new Vector2(600, 0),
                Margin = new MarginPadding()
                {
                    Top = 90,
                    Right = 24,
                    Bottom = 0,
                    Left = 0
                },
                Children = new Drawable[]
                {
                    new FillFlowContainer()
                    {
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.CentreRight,
                        RelativeSizeAxes = Axes.X,
                        Direction = FillDirection.Horizontal,
                        Spacing = new Vector2(4, 0),
                        Children = new Drawable[]
                        {
                            new SpriteText()
                            {
                                Anchor = Anchor.CentreRight,
                                Origin = Anchor.CentreRight,
                                Text = "Tips".ToUpper(),
                                Font = RenakoFont.GetFont(RenakoFont.Typeface.JosefinSans, 28f, RenakoFont.FontWeight.Bold),
                                Colour = Color4Extensions.FromHex("C4C6EA")
                            },
                            new SpriteIcon()
                            {
                                Anchor = Anchor.CentreRight,
                                Origin = Anchor.CentreRight,
                                Icon = FontAwesome.Solid.Lightbulb,
                                Size = new Vector2(24),
                                Colour = Color4Extensions.FromHex("A7ABE1")
                            }
                        }
                    },
                    new SpriteText()
                    {
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.CentreRight,
                        Text = "Use Ctrl + O to toggle settings anywhere!",
                        Font = RenakoFont.GetFont(RenakoFont.Typeface.MPlus1P, 20f),
                        Colour = Color4Extensions.FromHex("E0E1F0")
                    }
                }
            },
            new BackButton()
            {
                Action = ToggleVisibility
            }
        };
    }

    /// <summary>
    /// Play screen enter animation.
    /// </summary>
    public void Enter()
    {
        this.FadeIn(500, Easing.OutQuart);
        menuTitleContainer.MoveToX(-MenuButton.CONTAINER_PADDING, 500, Easing.OutQuart);
        tipsContainer.MoveToX(0, 750, Easing.OutQuart);
    }

    /// <summary>
    /// Play screen exit animation.
    /// </summary>
    public void Exit()
    {
        this.FadeOut(500, Easing.OutQuart);
        menuTitleContainer.MoveToX(-600, 500, Easing.OutQuart);
        tipsContainer.MoveToX(600, 750, Easing.OutQuart);
    }

    protected override void PopIn()
    {
        Enter();
    }

    protected override void PopOut()
    {
        Exit();
    }
}

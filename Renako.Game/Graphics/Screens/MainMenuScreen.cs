using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK;
using Renako.Game.Graphics.Drawables;

namespace Renako.Game.Graphics.Screens;

public partial class MainMenuScreen : Screen
{
    private FillFlowContainer menuContainer;
    private FillFlowContainer reverseMenuContainer;

    [BackgroundDependencyLoader]
    private void load()
    {
        InternalChildren = new Drawable[]
        {
            menuContainer = new FillFlowContainer()
            {
                Anchor = Anchor.TopLeft,
                Origin = Anchor.TopLeft,
                RelativeSizeAxes = Axes.Both,
                Direction = FillDirection.Vertical,
                Spacing = new Vector2(0, 28),
                Position = new Vector2(-MenuButton.CONTAINER_PADDING - 600, 0),
                Children = new Drawable[]
                {
                    // Play
                    new MenuButton()
                    {
                        ButtonWidth = 0.45f,
                        BackgroundColor = Color4Extensions.FromHex("F2DFE9"),
                        IconColor = Color4Extensions.FromHex("4B2839"),
                        TitleColor = Color4Extensions.FromHex("67344D"),
                        DescriptionColor = Color4Extensions.FromHex("251319"),
                        Icon = FontAwesome.Solid.Play,
                        Title = "Play",
                        Description = "Let's have some fun!"
                    },
                    // Editor
                    new MenuButton()
                    {
                        ButtonWidth = 0.38f,
                        BackgroundColor = Color4Extensions.FromHex("FDE798"),
                        IconColor = Color4Extensions.FromHex("4B4A28"),
                        TitleColor = Color4Extensions.FromHex("666734"),
                        DescriptionColor = Color4Extensions.FromHex("212110"),
                        Icon = FontAwesome.Solid.PencilAlt,
                        Title = "Editor",
                        Description = "Create a new strategy"
                    },
                    // List
                    new MenuButton()
                    {
                        ButtonWidth = 0.31f,
                        BackgroundColor = Color4Extensions.FromHex("C3DCC2"),
                        IconColor = Color4Extensions.FromHex("284B2E"),
                        TitleColor = Color4Extensions.FromHex("407642"),
                        DescriptionColor = Color4Extensions.FromHex("1A321A"),
                        Icon = FontAwesome.Solid.List,
                        Title = "List",
                        Description = "Find more enemies"
                    },
                    // Exit
                    new MenuButton()
                    {
                        ButtonWidth = 0.24f,
                        BackgroundColor = Color4Extensions.FromHex("ECC1C1"),
                        IconColor = Color4Extensions.FromHex("4B2828"),
                        TitleColor = Color4Extensions.FromHex("753F3F"),
                        DescriptionColor = Color4Extensions.FromHex("2D1717"),
                        Icon = FontAwesome.Solid.SignOutAlt,
                        Title = "Exit",
                        Description = "See you soon!"
                    },
                }
            },
            reverseMenuContainer = new FillFlowContainer()
            {
                Anchor = Anchor.TopRight,
                Origin = Anchor.TopRight,
                RelativeSizeAxes = Axes.Both,
                Direction = FillDirection.Vertical,
                RelativePositionAxes = Axes.Y,
                Spacing = new Vector2(0, 28),
                Position = new Vector2(600, 0.23f),
                Child = new ReverseMenuButton()
                {
                    ButtonWidth = 0.2f,
                    BackgroundColor = Color4Extensions.FromHex("E0E1F0"),
                    IconColor = Color4Extensions.FromHex("2D284B"),
                    TitleColor = Color4Extensions.FromHex("403F75"),
                    DescriptionColor = Color4Extensions.FromHex("171A2D"),
                    Icon = FontAwesome.Solid.Cog,
                    Title = "Settings",
                    Description = "Get ready for fight!"
                }
            }
        };
    }

    public override void OnEntering(ScreenTransitionEvent e)
    {
        menuContainer.MoveToX(-MenuButton.CONTAINER_PADDING, 750, Easing.OutQuart);
        reverseMenuContainer.MoveToX(MenuButton.CONTAINER_PADDING, 1000, Easing.OutQuart);

        base.OnEntering(e);
    }
}

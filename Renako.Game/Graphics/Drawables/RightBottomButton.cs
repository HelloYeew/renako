using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osuTK;

namespace Renako.Game.Graphics.Drawables;

public partial class RightBottomButton : Button
{
    private Box backgroundBox;
    public ColourInfo BackgroundColor { get; set; } = Color4Extensions.FromHex("ECC1C1");
    public ColourInfo IconColor { get; set; } = Color4Extensions.FromHex("4B2828");
    public ColourInfo TextColor { get; set; } = Color4Extensions.FromHex("753F3F");
    public IconUsage Icon { get; set; } = FontAwesome.Solid.ArrowLeft;
    public string Text { get; set; } = "Back";

    [BackgroundDependencyLoader]
    private void load()
    {
        Anchor = Anchor.BottomRight;
        Origin = Anchor.BottomRight;
        Size = new Vector2(200, 60);
        Colour = Colour4.White;
        Position = new Vector2(20, -40);
        Children = new Drawable[]
        {
            backgroundBox = new Box()
            {
                Colour = BackgroundColor,
                RelativeSizeAxes = Axes.Both,
                Shear = new Vector2(0.45f, 0f),
                Position = new Vector2(20, 0)
            },
            new FillFlowContainer()
            {
                Anchor = Anchor.CentreRight,
                Origin = Anchor.CentreRight,
                RelativeSizeAxes = Axes.Both,
                Padding = new MarginPadding()
                {
                    Left = 20,
                    Right = 40,
                    Top = 20,
                    Bottom = 20
                },
                Spacing = new Vector2(20, 0),
                Direction = FillDirection.Horizontal,
                Children = new Drawable[]
                {
                    new SpriteIcon()
                    {
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.CentreRight,
                        Size = new Vector2(25),
                        Icon = Icon,
                        Colour = IconColor
                    },
                    new SpriteText()
                    {
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.CentreRight,
                        Font = RenakoFont.GetFont(RenakoFont.Typeface.JosefinSans, 35f, RenakoFont.FontWeight.Bold),
                        Text = Text.ToUpper(),
                        Colour = TextColor
                    }
                }
            }
        };
    }

    protected override bool OnHover(HoverEvent e)
    {
        backgroundBox.FlashColour(Color4Extensions.Lighten(BackgroundColor, 0.8f), 500, Easing.OutBounce);

        return base.OnHover(e);
    }
}

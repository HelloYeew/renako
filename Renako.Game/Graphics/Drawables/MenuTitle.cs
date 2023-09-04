using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace Renako.Game.Graphics.Drawables;

public partial class MenuTitle : CompositeDrawable
{
    public float ButtonWidth { get; set; } = 0.45f;
    public ColourInfo BackgroundColor { get; set; } = Color4Extensions.FromHex("F2DFE9");
    public ColourInfo TitleColor { get; set; } = Color4Extensions.FromHex("67344D");
    public ColourInfo DescriptionColor { get; set; } = Color4Extensions.FromHex("251319");
    public string Title { get; set; } = "Play";
    public string Description { get; set; } = "Let's have some fun!";
    public bool AutoUpperCaseTitle { get; set; } = true;

    public const float CONTAINER_PADDING = 20;

    private Box backgroundBox;

    [BackgroundDependencyLoader]
    private void load()
    {
        Anchor = Anchor.CentreLeft;
        Origin = Anchor.CentreLeft;
        RelativeSizeAxes = Axes.X;
        Size = new Vector2(ButtonWidth, 80);
        Masking = true;
        CornerRadius = 15;
        Colour = Colour4.White;
        InternalChildren = new Drawable[]
        {
            backgroundBox = new Box()
            {
                Colour = BackgroundColor,
                RelativeSizeAxes = Axes.Both,
                Shear = new Vector2(0.45f, 0f),
            },
            new FillFlowContainer()
            {
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                RelativeSizeAxes = Axes.Both,
                Padding = new MarginPadding()
                {
                    Left = CONTAINER_PADDING * 2,
                    Right = CONTAINER_PADDING,
                    Top = CONTAINER_PADDING,
                    Bottom = CONTAINER_PADDING
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
                        Direction = FillDirection.Vertical,
                        Children = new Drawable[]
                        {
                            new SpriteText()
                            {
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                Font = RenakoFont.GetFont(RenakoFont.Typeface.JosefinSans, 35f, RenakoFont.FontWeight.Bold),
                                Text = AutoUpperCaseTitle ? Title.ToUpper() : Title,
                                Colour = TitleColor
                            },
                            new SpriteText()
                            {
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                Font = RenakoFont.GetFont(RenakoFont.Typeface.MPlus1P, 23f),
                                Text = Description,
                                Colour = DescriptionColor
                            }
                        }
                    }
                }
            }
        };
    }
}

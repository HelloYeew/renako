using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osuTK;

namespace Renako.Game.Graphics.UserInterface;

public partial class ReverseMenuButton : Button
{
    public float ButtonWidth { get; set; } = 0.45f;
    public ColourInfo BackgroundColor { get; set; } = Color4Extensions.FromHex("F2DFE9");
    public ColourInfo IconColor { get; set; } = Color4Extensions.FromHex("4B2839");
    public ColourInfo TitleColor { get; set; } = Color4Extensions.FromHex("67344D");
    public ColourInfo DescriptionColor { get; set; } = Color4Extensions.FromHex("251319");
    public IconUsage Icon { get; set; } = FontAwesome.Solid.Play;
    public string Title { get; set; } = "Play";
    public string Description { get; set; } = "Let's have some fun!";
    public MenuButtonClickSample ClickSample { get; set; } = MenuButtonClickSample.Enter1;

    public const float CONTAINER_PADDING = 20;

    private Box backgroundBox;
    private Sample hoverSample;
    private Sample clickSample;

    [BackgroundDependencyLoader]
    private void load(AudioManager audioManager)
    {
        Anchor = Anchor.CentreRight;
        Origin = Anchor.CentreRight;
        RelativeSizeAxes = Axes.X;
        Size = new Vector2(ButtonWidth, 80);
        Colour = Colour4.White;
        Children = new Drawable[]
        {
            backgroundBox = new Box()
            {
                Colour = BackgroundColor,
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.CentreRight,
                Origin = Anchor.CentreRight,
                Shear = new Vector2(0.45f, 0f)
            },
            new FillFlowContainer()
            {
                Anchor = Anchor.CentreRight,
                Origin = Anchor.CentreRight,
                RelativeSizeAxes = Axes.Both,
                Padding = new MarginPadding()
                {
                    Left = CONTAINER_PADDING,
                    Right = CONTAINER_PADDING * 2,
                    Top = CONTAINER_PADDING,
                    Bottom = CONTAINER_PADDING
                },
                Spacing = new Vector2(20, 0),
                Direction = FillDirection.Horizontal,
                Children = new Drawable[]
                {
                    new SpriteIcon()
                    {
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.CentreRight,
                        Size = new Vector2(30),
                        Icon = Icon,
                        Colour = IconColor
                    },
                    new FillFlowContainer()
                    {
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.CentreRight,
                        RelativeSizeAxes = Axes.X,
                        Direction = FillDirection.Vertical,
                        Children = new Drawable[]
                        {
                            new SpriteText()
                            {
                                Anchor = Anchor.CentreRight,
                                Origin = Anchor.CentreRight,
                                Font = RenakoFont.GetFont(RenakoFont.Typeface.JosefinSans, 35f, RenakoFont.FontWeight.Bold),
                                Text = Title.ToUpper(),
                                Colour = TitleColor
                            },
                            new SpriteText()
                            {
                                Anchor = Anchor.CentreRight,
                                Origin = Anchor.CentreRight,
                                Font = RenakoFont.GetFont(RenakoFont.Typeface.MPlus1P, 23f),
                                Text = Description,
                                Colour = DescriptionColor
                            }
                        }
                    }
                }
            }
        };

        hoverSample = audioManager.Samples.Get("UI/hover");

        switch (ClickSample)
        {
            case MenuButtonClickSample.Enter1:
                clickSample = audioManager.Samples.Get("UI/click-enter1");
                break;

            case MenuButtonClickSample.Enter2:
                clickSample = audioManager.Samples.Get("UI/click-enter2");
                break;
        }
    }

    protected override bool OnHover(HoverEvent e)
    {
        backgroundBox.FlashColour(Color4Extensions.Lighten(BackgroundColor, 0.8f), 500, Easing.OutBounce);
        hoverSample?.Play();

        return base.OnHover(e);
    }

    protected override bool OnClick(ClickEvent e)
    {
        clickSample?.Play();

        return base.OnClick(e);
    }
}

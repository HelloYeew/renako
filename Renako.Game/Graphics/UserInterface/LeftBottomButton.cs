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

public partial class LeftBottomButton : Button
{
    private Box backgroundBox;
    public ColourInfo BackgroundColor { get; set; } = Color4Extensions.FromHex("ECC1C1");
    public ColourInfo IconColor { get; set; } = Color4Extensions.FromHex("4B2828");
    public ColourInfo TextColor { get; set; } = Color4Extensions.FromHex("753F3F");
    public IconUsage Icon { get; set; } = FontAwesome.Solid.ArrowLeft;
    public string Text { get; set; } = "Back";

    private Sample hoverSample;
    private Sample clickSample;

    [BackgroundDependencyLoader]
    private void load(AudioManager audioManager)
    {
        Anchor = Anchor.BottomLeft;
        Origin = Anchor.BottomLeft;
        Size = new Vector2(200, 60);
        Masking = true;
        CornerRadius = 15;
        Colour = Colour4.White;
        Position = new Vector2(-20, -40);
        Children = new Drawable[]
        {
            backgroundBox = new Box()
            {
                Colour = BackgroundColor,
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
                    new SpriteIcon()
                    {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        Size = new Vector2(25),
                        Icon = Icon,
                        Colour = IconColor
                    },
                    new SpriteText()
                    {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        Font = RenakoFont.GetFont(RenakoFont.Typeface.JosefinSans, 35f, RenakoFont.FontWeight.Bold),
                        Text = Text.ToUpper(),
                        Colour = TextColor
                    }
                }
            }
        };

        hoverSample = audioManager.Samples.Get("UI/hover");
        clickSample = audioManager.Samples.Get("UI/click-back");

        Enabled.BindValueChanged(enabled =>
        {
            if (!enabled.NewValue)
            {
                backgroundBox.FadeColour(Color4Extensions.Darken(BackgroundColor, 0.8f), 500, Easing.OutQuint);
            }
            else
            {
                backgroundBox.FadeColour(BackgroundColor, 500, Easing.OutQuint);
            }
        }, true);
    }

    protected override bool OnHover(HoverEvent e)
    {
        if (Enabled.Value)
        {
            backgroundBox.FlashColour(Color4Extensions.Lighten(BackgroundColor, 0.8f), 500, Easing.OutBounce);
            hoverSample?.Play();
        }

        return base.OnHover(e);
    }

    protected override bool OnClick(ClickEvent e)
    {
        if (Enabled.Value)
            clickSample?.Play();

        return base.OnClick(e);
    }
}

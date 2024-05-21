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

public partial class RightBottomButton : Button
{
    private Box backgroundBox;
    public ColourInfo BackgroundColor { get; set; } = Color4Extensions.FromHex("ECC1C1");
    public ColourInfo IconColor { get; set; } = Color4Extensions.FromHex("4B2828");
    public ColourInfo TextColor { get; set; } = Color4Extensions.FromHex("753F3F");
    private IconUsage icon = FontAwesome.Solid.ArrowLeft;
    private string text = "Back";
    private readonly SpriteIcon iconSprite;
    private readonly SpriteText textSprite;

    public const int HOVER_MOVE_DISTANCE = 10;
    public const int DEFAULT_X_POSITION = 20;

    private Sample hoverSample;

    public IconUsage Icon
    {
        get => icon;
        set
        {
            icon = value;
            iconSprite.Icon = value;
        }
    }

    public string Text
    {
        get => text;
        set
        {
            text = value;
            textSprite.Text = value.ToUpper();
        }
    }

    public RightBottomButton()
    {
        iconSprite = new SpriteIcon()
        {
            Anchor = Anchor.CentreRight,
            Origin = Anchor.CentreRight,
            Size = new Vector2(25),
            Icon = Icon,
            Colour = IconColor
        };
        textSprite = new RenakoSpriteText()
        {
            Anchor = Anchor.CentreRight,
            Origin = Anchor.CentreRight,
            Font = RenakoFont.GetFont(RenakoFont.Typeface.JosefinSans, 35f, RenakoFont.FontWeight.Bold),
            Text = Text.ToUpper(),
            Colour = TextColor
        };
    }

    [BackgroundDependencyLoader]
    private void load(AudioManager audioManager)
    {
        Anchor = Anchor.BottomRight;
        Origin = Anchor.BottomRight;
        Size = new Vector2(200, 60);
        Masking = true;
        CornerRadius = 15;
        Colour = Colour4.White;
        Position = new Vector2(DEFAULT_X_POSITION, -40);
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
                    iconSprite,
                    textSprite
                }
            }
        };

        hoverSample = audioManager.Samples.Get("UI/hover");

        Enabled.BindValueChanged(enabled =>
        {
            if (!enabled.NewValue)
            {
                Scheduler.Add(() => backgroundBox.FadeColour(Color4Extensions.Darken(BackgroundColor, 0.8f), 500, Easing.OutQuint));
            }
            else
            {
                Scheduler.Add(() => backgroundBox.FadeColour(BackgroundColor, 500, Easing.OutQuint));
            }
        }, true);
    }

    protected override bool OnHover(HoverEvent e)
    {
        if (Enabled.Value)
            hoverSample?.Play();
        this.MoveToX(DEFAULT_X_POSITION - HOVER_MOVE_DISTANCE, 250, Easing.OutCirc);

        return base.OnHover(e);
    }

    protected override void OnHoverLost(HoverLostEvent e)
    {
        base.OnHoverLost(e);
        this.MoveToX(DEFAULT_X_POSITION, 250, Easing.OutCirc);
    }

    /// <summary>
    /// Flash the background of the button
    /// </summary>
    /// <param name="duration">Flash duration</param>
    /// <param name="loop">Loop the flash animation</param>
    public void FlashBackground(double duration, bool loop = false)
    {
        Scheduler.Add(() =>
        {
            backgroundBox.ClearTransforms();
            backgroundBox.Colour = BackgroundColor;
            if (loop)
                backgroundBox.FlashColour(Color4Extensions.FromHex("F3E7EE"), duration, Easing.OutCubic).Loop();
            else
                backgroundBox.FlashColour(Color4Extensions.FromHex("F3E7EE"), duration, Easing.OutCubic);
        });
    }
}

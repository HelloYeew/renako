using osu.Framework.Allocation;
using osu.Framework.Development;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK;
using Renako.Game.Audio;
using Renako.Game.Graphics.ScreenStacks;

namespace Renako.Game.Graphics.Screens;


public partial class StartScreen : RenakoScreen
{
    private SpriteText pressAnyKeyText;
    private SpriteText buildText;
    private SpriteText rightBottomText;

    [Resolved]
    private RenakoBackgroundScreenStack backgroundScreenStack { get; set; }

    [Resolved]
    private RenakoScreenStack mainScreenStack { get; set; }

    [Resolved]
    private LogoScreenStack logoScreenStack { get; set; }

    [Resolved]
    private RenakoAudioManager audioManager { get; set; }

    [BackgroundDependencyLoader]
    private void load()
    {
        InternalChildren = new Drawable[]
        {
            pressAnyKeyText = new SpriteText()
            {
                Anchor = Anchor.BottomCentre,
                Origin = Anchor.BottomCentre,
                RelativePositionAxes = Axes.Y,
                Position = new Vector2(0, 0.15f),
                Text = "Press any key to start".ToUpper(),
                Font = RenakoFont.GetFont(RenakoFont.Typeface.JosefinSans, 42f, RenakoFont.FontWeight.Bold)
            },
            buildText = new SpriteText()
            {
                Anchor = Anchor.BottomLeft,
                Origin = Anchor.BottomLeft,
                Text = DebugUtils.IsDebugBuild ? "Development build".ToUpper() : $"Version {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}".ToUpper(),
                Font = RenakoFont.GetFont(RenakoFont.Typeface.JosefinSans, 24f),
                Colour = DebugUtils.IsDebugBuild ? Colour4.Red : Color4Extensions.FromHex("82767E")
            },
            rightBottomText = new SpriteText()
            {
                Anchor = Anchor.BottomRight,
                Origin = Anchor.BottomRight,
                Text = "HelloYeew".ToUpper(),
                Font = RenakoFont.GetFont(RenakoFont.Typeface.JosefinSans, 24f),
                Colour = Color4Extensions.FromHex("82767E")
            }
        };
    }

    public override void OnEntering(ScreenTransitionEvent e)
    {
        base.OnEntering(e);

        buildText.Delay(500).FadeTo(1, 750, Easing.OutCubic);
        rightBottomText.Delay(750).FadeTo(1, 750, Easing.OutCubic);

        pressAnyKeyText.Delay(500).MoveToY(-0.15f, 750, Easing.OutCubic);
        Scheduler.AddDelayed(() => pressAnyKeyText.Loop(b => b.FadeTo(0.25f).FadeTo(1, 1000)).Loop(), 1250);

        audioManager.Track?.Start();
    }

    protected override bool OnMouseDown(MouseDownEvent e)
    {
        goToMainMenu();
        return base.OnMouseDown(e);
    }

    protected override bool OnKeyDown(KeyDownEvent e)
    {
        goToMainMenu();
        return base.OnKeyDown(e);
    }

    protected override bool OnJoystickPress(JoystickPressEvent e)
    {
        goToMainMenu();
        return base.OnJoystickPress(e);
    }

    private void goToMainMenu()
    {
        if (Clock.CurrentTime < 5000) return;

        this.Exit();
        mainScreenStack.Push(new MainMenuScreen());
    }
}

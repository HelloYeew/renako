using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK;
using Renako.Game.Audio;
using Renako.Game.Graphics.ScreenStacks;

namespace Renako.Game.Graphics.Screens;

public partial class StartScreen : Screen
{
    private SpriteText pressAnyKeyText;

    [Resolved]
    private RenakoBackgroundScreenStack backgroundScreenStack { get; set; }

    [Resolved]
    private RenakoScreenStack mainScreenStack { get; set; }

    [Resolved]
    private LogoScreenStack logoScreenStack { get; set; }

    [Resolved]
    private RenakoAudioManager audioManager { get; set; }

    [BackgroundDependencyLoader]
    private void load(TextureStore textureStore)
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
            new SpriteText()
            {
                Anchor = Anchor.BottomLeft,
                Origin = Anchor.BottomLeft,
                Text = "Development Build".ToUpper(),
                Font = RenakoFont.GetFont(RenakoFont.Typeface.JosefinSans, 24f),
                Colour = Color4Extensions.FromHex("82767E")
            }
        };
    }

    public override void OnEntering(ScreenTransitionEvent e)
    {
        base.OnEntering(e);

        backgroundScreenStack.ImageSprite.Delay(250).FadeTo(1, 750, Easing.OutCubic);

        pressAnyKeyText.Delay(500).MoveToY(-0.15f, 750, Easing.OutCubic);
        Scheduler.AddDelayed(() => pressAnyKeyText.Loop(b => b.FadeTo(0.25f).FadeTo(1, 1000)).Loop(), 1250);
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

    private void goToMainMenu()
    {
        if (Clock.CurrentTime < 2000) return;

        this.Exit();
        mainScreenStack.Push(new MainMenuScreen());
    }
}

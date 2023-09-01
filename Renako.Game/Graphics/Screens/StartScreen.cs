using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK;
using Renako.Game.Graphics.Drawables;
using Renako.Game.Graphics.ScreenStacks;

namespace Renako.Game.Graphics.Screens;

public partial class StartScreen : Screen
{
    private RenakoLogo logo;
    private SpriteText pressAnyKeyText;

    [Resolved]
    private RenakoBackgroundScreenStack backgroundScreenStack { get; set; }

    [Resolved]
    private RenakoScreenStack mainScreenStack { get; set; }

    [BackgroundDependencyLoader]
    private void load(TextureStore textureStore)
    {
        InternalChildren = new Drawable[]
        {
            logo = new RenakoLogo()
            {
                Anchor = Anchor.TopCentre,
                Origin = Anchor.TopCentre,
                RelativePositionAxes = Axes.Y,
                Position = new Vector2(0, -0.15f)
            },
            pressAnyKeyText = new SpriteText()
            {
                Anchor = Anchor.BottomCentre,
                Origin = Anchor.BottomCentre,
                RelativePositionAxes = Axes.Y,
                Position = new Vector2(0, 0.15f),
                Text = "Press any key to start".ToUpper(),
                Font = RenakoFont.GetFont(RenakoFont.Typeface.JosefinSans, 42f, RenakoFont.FontWeight.Bold),
                Alpha = 0
            }
        };

        Scheduler.AddDelayed(() =>
        {
            backgroundScreenStack.ImageSprite.Texture = textureStore.Get("main-background");
            backgroundScreenStack.ImageSprite.FadeTo(1, 250, Easing.OutCubic);
        }, 250);
    }

    public override void OnEntering(ScreenTransitionEvent e)
    {
        base.OnEntering(e);

        logo.MoveToY(0.15f, 750, Easing.OutCubic);
        pressAnyKeyText.Delay(500).MoveToY(-0.15f, 750, Easing.OutCubic);
        pressAnyKeyText.FadeTo(0, 500)
                       .FadeTo(1, 500)
                       .Loop();
    }

    protected override bool OnKeyDown(KeyDownEvent e)
    {
        return base.OnKeyDown(e);
    }

    private void goToMainMenu()
    {
        this.Exit();
        mainScreenStack.Push(new MainMenuScreen());
    }
}

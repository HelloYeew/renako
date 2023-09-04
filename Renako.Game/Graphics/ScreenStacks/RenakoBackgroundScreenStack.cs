using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Logging;
using osu.Framework.Screens;
using Renako.Game.Graphics.Screens;

namespace Renako.Game.Graphics.ScreenStacks;

public partial class RenakoBackgroundScreenStack : ScreenStack
{
    public Sprite ImageSprite;

    private Texture mainBackgroundTexture;
    private Texture playMenuBackgroundTexture;

    [Resolved]
    private RenakoScreenStack mainScreenStack { get; set; }

    [BackgroundDependencyLoader]
    private void load(TextureStore textureStore)
    {
        mainBackgroundTexture = textureStore.Get("Screen/main-background.jpeg");
        playMenuBackgroundTexture = textureStore.Get("Screen/play-background.jpg");
        Logger.Log(mainBackgroundTexture.Size.ToString());

        AddInternal(ImageSprite = new Sprite()
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            RelativeSizeAxes = Axes.Both,
            FillMode = FillMode.Fill,
            Alpha = 0,
            Texture = mainBackgroundTexture
        });

        mainScreenStack.BindableCurrentScreen.BindValueChanged((e) => changeBackgroundByMainScreen(e.OldValue, e.NewValue));
    }

    /// <summary>
    /// Change background by main screen.
    /// </summary>
    /// <param name="oldScreen">The old screen before change.</param>
    /// <param name="newScreen">The new screen that will be changed.</param>
    private void changeBackgroundByMainScreen(IScreen oldScreen, IScreen newScreen)
    {
        // Start game event
        if (oldScreen is WarningScreen && newScreen is StartScreen)
        {
            ImageSprite.Delay(250).FadeTo(1, 750, Easing.OutCubic);
        }

        switch (newScreen)
        {
            case (MainMenuScreen):
                ImageSprite.Texture = mainBackgroundTexture;
                break;

            case (StartScreen):
                ImageSprite.Texture = mainBackgroundTexture;
                break;

            case (PlayMenuScreen):
                ImageSprite.Texture = playMenuBackgroundTexture;
                break;
        }
    }
}

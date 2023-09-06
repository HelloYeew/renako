using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Screens;
using Renako.Game.Graphics.Screens;

namespace Renako.Game.Graphics.ScreenStacks;

public partial class RenakoBackgroundScreenStack : ScreenStack
{
    public Sprite ImageSpriteUp;
    public Sprite ImageSpriteDown;

    private Texture mainBackgroundTexture;
    private Texture playMenuBackgroundTexture;
    private Texture songSelectionBackgroundTexture;

    [Resolved]
    private RenakoScreenStack mainScreenStack { get; set; }

    [BackgroundDependencyLoader]
    private void load(TextureStore textureStore)
    {
        mainBackgroundTexture = textureStore.Get("Screen/main-background.jpeg");
        playMenuBackgroundTexture = textureStore.Get("Screen/play-background.jpg");
        // TODO: This need to be depend on current working beatmap background
        songSelectionBackgroundTexture = textureStore.Get("Beatmaps/Background/innocence-tv-size.jpg");

        AddInternal(ImageSpriteDown = new Sprite()
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            RelativeSizeAxes = Axes.Both,
            FillMode = FillMode.Fill,
            Alpha = 0
        });
        AddInternal(ImageSpriteUp = new Sprite()
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            RelativeSizeAxes = Axes.Both,
            FillMode = FillMode.Fill,
            Alpha = 0
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
            ImageSpriteUp.Delay(250).FadeTo(1, 750, Easing.OutCubic);
        }

        switch (newScreen)
        {
            case (MainMenuScreen):
                // Don't do background transition again between MainMenuScreen and StartScreen
                if (oldScreen is StartScreen) break;

                ChangeBackground(mainBackgroundTexture);
                break;

            case (StartScreen):
                ChangeBackground(mainBackgroundTexture);
                break;

            case (PlayMenuScreen):
                ChangeBackground(playMenuBackgroundTexture);
                break;

            case (SongSelectionScreen):
                ChangeBackground(songSelectionBackgroundTexture);
                break;
        }
    }

    /// <summary>
    /// Smoothly change background using fade out and fade in between two image <see cref="Sprite"/>.
    /// </summary>
    /// <param name="texture">New <see cref="Texture"/> that will be changed.</param>
    /// <param name="duration">Duration of the fade out and fade in.</param>
    public void ChangeBackground(Texture texture, int duration = 500)
    {
        ImageSpriteDown.Texture = ImageSpriteUp.Texture;
        ImageSpriteDown.Alpha = ImageSpriteUp.Alpha;
        ImageSpriteUp.Texture = texture;
        ImageSpriteUp.Alpha = 0;
        ImageSpriteUp.FadeIn(duration, Easing.OutCubic);
        ImageSpriteDown.FadeOut(duration, Easing.OutCubic);
    }
}

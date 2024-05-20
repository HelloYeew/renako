using System.IO;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osuTK.Graphics;
using Renako.Game.Beatmaps;
using Renako.Game.Graphics.Screens;
using Renako.Game.Utilities;

namespace Renako.Game.Graphics.ScreenStacks;

public partial class RenakoBackgroundScreenStack : ScreenStack
{
    private ITextureStore textureStore;

    public Sprite ImageSpriteUp;
    public Sprite ImageSpriteDown;

    private Texture mainBackgroundTexture;
    private Texture playMenuBackgroundTexture;
    private Texture fallbackBeatmapBackground;
    private Container backgroundContainer;
    private Box maskBox;

    [Resolved]
    private RenakoScreenStack mainScreenStack { get; set; }

    [Resolved]
    private WorkingBeatmap workingBeatmap { get; set; }

    [Resolved]
    private GameHost host { get; set; }

    [BackgroundDependencyLoader]
    private void load(TextureStore textureStore)
    {
        this.textureStore = textureStore;

        mainBackgroundTexture = textureStore.Get("Screen/main-background.jpeg");
        playMenuBackgroundTexture = textureStore.Get("Screen/play-background.jpg");
        // TODO: Change this to a real fallback background and delete the fallback-beatmap-background.jpg
        fallbackBeatmapBackground = textureStore.Get("Screen/main-background.jpeg");

        AddInternal(backgroundContainer = new Container()
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            RelativeSizeAxes = Axes.Both,
            Children = new Drawable[]
            {
                ImageSpriteDown = new Sprite()
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    FillMode = FillMode.Fill,
                    Alpha = 0
                },
                ImageSpriteUp = new Sprite()
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    FillMode = FillMode.Fill,
                    Alpha = 0
                }
            }
        });

        AddInternal(maskBox = new Box()
        {
            RelativeSizeAxes = Axes.Both,
            Colour = Color4.Black,
            Alpha = 0
        });

        mainScreenStack.BindableCurrentScreen.BindValueChanged((e) => changeBackgroundByMainScreen(e.OldValue, e.NewValue));
        workingBeatmap.BindableWorkingBeatmapSet.BindValueChanged((e) => changeBackgroundByBeatmapSet(e.OldValue, e.NewValue));
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
                changeBackgroundByBeatmapSet(null, workingBeatmap.BeatmapSet);
                break;
        }
    }

    /// <summary>
    /// Change background by beatmap set.
    /// </summary>
    /// <param name="oldBeatmapSet">The old beatmap set before change.</param>
    /// <param name="newBeatmapSet">The new beatmap set that will be changed.</param>
    private void changeBackgroundByBeatmapSet(BeatmapSet oldBeatmapSet, BeatmapSet newBeatmapSet)
    {
        if (newBeatmapSet == null)
        {
            ChangeBackground(fallbackBeatmapBackground);
            return;
        }

        if (Equals(oldBeatmapSet, newBeatmapSet)) return;

        if (mainScreenStack.CurrentScreen is not SongSelectionScreen) return;

        Texture newBackgroundTexture;

        if (newBeatmapSet.UseLocalSource)
            newBackgroundTexture = textureStore.Get(newBeatmapSet.BackgroundPath);
        else
        {
            string coverPath = BeatmapSetUtility.GetBackgroundPath(newBeatmapSet);
            newBackgroundTexture = textureStore.Get(coverPath);

            if (newBackgroundTexture == null)
            {
                Stream backgroundTextureStream = host.Storage.GetStream(coverPath);
                newBackgroundTexture = Texture.FromStream(host.Renderer, backgroundTextureStream);
                backgroundTextureStream?.Close();
            }
        }

        ChangeBackground(newBackgroundTexture ?? fallbackBeatmapBackground);
    }

    /// <summary>
    /// Smoothly change background using fade out and fade in between two image <see cref="Sprite"/>.
    /// </summary>
    /// <param name="texture">New <see cref="Texture"/> that will be changed.</param>
    /// <param name="duration">Duration of the fade out and fade in.</param>
    public void ChangeBackground(Texture texture, int duration = 500)
    {
        Scheduler.Add(() =>
        {
            ImageSpriteDown.Texture = ImageSpriteUp.Texture;
            ImageSpriteDown.Alpha = ImageSpriteUp.Alpha;
            ImageSpriteUp.Texture = texture;
            ImageSpriteUp.Alpha = 0;
            ImageSpriteUp.FadeIn(duration, Easing.OutCubic);
            ImageSpriteDown.FadeOut(duration, Easing.OutCubic);
        });
    }

    /// <summary>
    /// Adjust the alpha of the mask box.
    /// </summary>
    /// <param name="alpha">The new alpha value.</param>
    /// <param name="duration">Duration of the fade in and fade out.</param>
    /// <param name="easing">Easing function of the fade in and fade out.</param>
    public void AdjustMaskAlpha(float alpha, int duration = 500, Easing easing = Easing.OutQuart)
    {
        Scheduler.Add(() => maskBox.FadeTo(alpha, duration, easing));
    }
}

using System;
using System.IO;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osuTK.Graphics;
using Renako.Game.Beatmaps;
using Renako.Game.Configurations;
using Renako.Game.Graphics.Drawables;
using Renako.Game.Graphics.Screens;
using Renako.Game.Utilities;

namespace Renako.Game.Graphics.ScreenStacks;

public partial class RenakoBackgroundScreenStack : ScreenStack
{
    private ITextureStore textureStore;

    public Sprite ImageSpriteUp;
    public Sprite ImageSpriteDown;

    private Texture fallbackBeatmapBackground;
    private Container backgroundContainer;
    private Box maskBox;
    private Container videoContainer;

    private AbLoopVideo video;

    [Resolved]
    private RenakoScreenStack mainScreenStack { get; set; }

    [Resolved]
    private WorkingBeatmap workingBeatmap { get; set; }

    [Resolved]
    private GameHost host { get; set; }

    [Resolved]
    private RenakoConfigManager config { get; set; }

    [BackgroundDependencyLoader]
    private void load(TextureStore textureStore)
    {
        this.textureStore = textureStore;

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
                },
                videoContainer = new Container()
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
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

        workingBeatmap.BindableWorkingBeatmapSet.BindValueChanged((e) => changeBackgroundByBeatmapSet(e.OldValue, e.NewValue), true);

        var disableVideoBackground = config.GetBindable<bool>(RenakoSetting.DisableVideoPreview);
        disableVideoBackground.BindValueChanged((e) =>
        {
            if (e.NewValue)
                HideBackgroundVideo(true);
            else
                changeBackgroundByBeatmapSet(null, workingBeatmap.BeatmapSet);
        }, true);

        mainScreenStack.BindableCurrentScreen.BindValueChanged((e) =>
        {
            if (e.NewValue is SongSelectionScreen)
            {
                if (video != null)
                {
                    video.StartTime = workingBeatmap.BeatmapSet.PreviewTime;
                    video.LoopToStartTime = true;
                }
            }
            else
            {
                if (video != null)
                {
                    video.StartTime = 0;
                    video.LoopToStartTime = false;
                }
            }
        }, true);
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

        if (Equals(oldBeatmapSet, newBeatmapSet) && ImageSpriteDown.Texture != null)
            return;

        // if (mainScreenStack.CurrentScreen is not SongSelectionScreen) return;

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

        if (newBeatmapSet.HasVideo && newBeatmapSet.VideoPath != null && !config.Get<bool>(RenakoSetting.DisableVideoPreview))
        {
            ChangeBackgroundVideo(BeatmapSetUtility.GetVideoPath(newBeatmapSet), mainScreenStack.CurrentScreen is SongSelectionScreen ? newBeatmapSet.PreviewTime : 0, newBeatmapSet.TotalLength);
        }
        else
        {
            HideBackgroundVideo(true);
        }
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

    /// <summary>
    /// Reset the alpha of the mask box to 0.
    /// </summary>
    /// <param name="duration">Duration of the fade in and fade out.</param>
    /// <param name="easing">Easing function of the fade in and fade out.</param>
    public void ResetMaskAlpha(int duration = 500, Easing easing = Easing.OutQuart)
    {
        AdjustMaskAlpha(0, duration, easing);
    }

    /// <summary>
    /// Change the background video.
    /// </summary>
    /// <param name="videoPath">The path of the video file in game storage.</param>
    /// <param name="startTime">The start time of the video.</param>
    /// <param name="endTime">The end time of the video and will perform loop back to the start time.</param>
    /// <param name="fadeIn">Whether to fade in the video container when changing.</param>
    public void ChangeBackgroundVideo(string videoPath, double startTime = 0, double endTime = 0, bool fadeIn = true)
    {
        Scheduler.Add(() =>
        {
            videoContainer.FadeOut(500, Easing.OutCubic);
            videoContainer.Clear();

            try
            {
                videoContainer.Add(video = new AbLoopVideo(host.Storage.GetFullPath(videoPath))
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    FillMode = FillMode.Fill
                });
            }
            catch (Exception e)
            {
                Logger.Log($"Failed to load video: {e.Message}", LoggingTarget.Runtime, LogLevel.Error);
                return;
            }

            video.EndTime = video.Duration < endTime ? video.Duration : endTime;
            video.StartTime = startTime;
            video.Seek(startTime);
            video.LoopToStartTime = true;
            if (fadeIn)
                videoContainer.FadeIn(500, Easing.OutCubic);
        });
    }

    /// <summary>
    /// Seek the background video to the specified time.
    /// </summary>
    /// <param name="time">The time to seek to.</param>
    public void SeekBackgroundVideo(double time)
    {
        Scheduler.Add(() => video?.Seek(time));
    }

    /// <summary>
    /// Hide the background video.
    /// </summary>
    /// <param name="dispose">Whether to dispose the video on hide.</param>
    public void HideBackgroundVideo(bool dispose = false)
    {
        Scheduler.Add(() =>
        {
            if (dispose)
                video = null;
            videoContainer.FadeOut(500, Easing.OutCubic);
        });
    }

    /// <summary>
    /// Show the background video
    /// </summary>
    public void ShowBackgroundVideo()
    {
        Scheduler.Add(() => videoContainer.FadeIn(500, Easing.OutCubic));
    }

    /// <summary>
    /// Whether the background video is available.
    /// </summary>
    /// <returns>The availability of the background video.</returns>
    public bool HaveBackgroundVideo()
    {
        return video != null;
    }
}

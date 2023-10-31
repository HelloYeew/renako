using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics.Containers;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osu.Framework.Screens;
using Renako.Game.Beatmaps;
using Renako.Game.Graphics.Screens;
using Renako.Game.Graphics.ScreenStacks;
using Renako.Game.Utilities;

namespace Renako.Game.Audio;

/// <summary>
/// The audio manager that will play every track in Renako.
/// </summary>
public partial class RenakoAudioManager : CompositeDrawable
{
    public Track Track;
    private ITrackStore trackStore;
    private AudioManager audioManager;

    private double mainThemeDuration;

    [Resolved]
    private RenakoScreenStack mainScreenStack { get; set; }

    [Resolved]
    private WorkingBeatmap workingBeatmap { get; set; }

    [Resolved]
    private GameHost host { get; set; }

    [BackgroundDependencyLoader]
    private void load(AudioManager audioManagerSource)
    {
        mainScreenStack.BindableCurrentScreen.BindValueChanged(e => changeTrackOnScreenChanged(e.OldValue, e.NewValue));
        workingBeatmap.BindableWorkingBeatmapSet.BindValueChanged(e => changeTrackOnBeatmapSetChanged(e.OldValue, e.NewValue));

        trackStore = audioManagerSource.Tracks;
        Track = trackStore.Get("theme/main-theme.mp3");
        audioManager = audioManagerSource;

        foreach (var source in trackStore.GetAvailableResources())
        {
            Logger.Log("track :" + source, LoggingTarget.Runtime, LogLevel.Debug);
        }
    }

    /// <summary>
    /// Change the track when the screen changed.
    /// </summary>
    /// <param name="oldScreen">The old <see cref="IScreen"/> before change.</param>
    /// <param name="newScreen">The new <see cref="IScreen"/> that will be changed.</param>
    private void changeTrackOnScreenChanged(IScreen oldScreen, IScreen newScreen)
    {
        // Case specifically for starting the game.
        if (oldScreen is WarningScreen && newScreen is StartScreen)
        {
            Track?.Dispose();
            Track = trackStore.Get("theme/main-theme.mp3");
            Track.Looping = true;
            Track.Start();
        }
        else if (oldScreen is StartScreen && newScreen is MainMenuScreen)
        {
            return;
        }

        // Record the duration of the main theme if old screen is StartScreen or MainMenuScreen.
        if (oldScreen is MainMenuScreen or StartScreen)
        {
            mainThemeDuration = Track.CurrentTime;
        }

        if (newScreen is MainMenuScreen)
        {
            Track?.Stop();
            Track?.Dispose();
            Track = trackStore.Get("theme/main-theme.mp3");
            Track.Looping = true;
            Track.Seek(mainThemeDuration);
            Track.Start();
        }
        else if (newScreen is PlayMenuScreen)
        {
            Track?.Stop();
            Track?.Dispose();
            Track = trackStore.Get("theme/play-theme.mp3");
            Track.Looping = true;
            Track.Start();
        }
        else if (newScreen is SongSelectionScreen)
        {
            changeTrackOnBeatmapSetChanged(null, workingBeatmap.BeatmapSet);
        }
    }

    /// <summary>
    /// Change the track when the beatmap set changed.
    /// </summary>
    /// <param name="oldBeatmapSet">The old <see cref="BeatmapSet"/> before change.</param>
    /// <param name="newBeatmapSet">The new <see cref="BeatmapSet"/> that will be changed.</param>
    private void changeTrackOnBeatmapSetChanged(BeatmapSet oldBeatmapSet, BeatmapSet newBeatmapSet)
    {
        if (newBeatmapSet == null || Equals(oldBeatmapSet, newBeatmapSet))
            return;

        // We don't want to change the track if the screen is not SongSelectionScreen.
        if (mainScreenStack.CurrentScreen is not SongSelectionScreen)
            return;

        Track?.Stop();
        Track?.Dispose();

        if (newBeatmapSet.UseLocalSource)
            Track = trackStore.Get(newBeatmapSet.TrackPath);
        else
        {
            string path = "beatmaps/" + BeatmapSetUtility.GetFolderName(newBeatmapSet) + "/" + newBeatmapSet.TrackPath;
            Track = trackStore.Get(path);
        }

        Track.Looping = true;
        Track.Seek(newBeatmapSet.PreviewTime);
        Track.RestartPoint = newBeatmapSet.PreviewTime;
        Track.Start();
    }

    /// <summary>
    /// Mute the track.
    /// </summary>
    public void Mute()
    {
        Track.Volume.Value = 0;
    }
}

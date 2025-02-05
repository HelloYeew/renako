using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics.Containers;
using osu.Framework.Platform;
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

    [Resolved]
    private RenakoScreenStack mainScreenStack { get; set; }

    [Resolved]
    private WorkingBeatmap workingBeatmap { get; set; }

    [Resolved]
    private GameHost host { get; set; }

    [Resolved]
    private BeatmapManager beatmapManager { get; set; }

    [BackgroundDependencyLoader]
    private void load(AudioManager audioManagerSource)
    {
        workingBeatmap.BindableWorkingBeatmapSet.BindValueChanged(e => changeTrackOnBeatmapSetChanged(e.OldValue, e.NewValue), true);

        mainScreenStack.BindableCurrentScreen.BindValueChanged(e =>
        {
            if (e.NewValue is SongSelectionScreen)
            {
                if (Track != null)
                {
                    Track.Looping = true;

                    if (workingBeatmap.BeatmapSet != null)
                    {
                        if (Track != null) Track.RestartPoint = workingBeatmap.BeatmapSet.PreviewTime;
                    }
                }
            }
            else
            {
                if (Track != null)
                {
                    Track.Looping = false;
                    Track.RestartPoint = 0;
                }
            }
        }, true);

        trackStore = audioManagerSource.Tracks;
        audioManager = audioManagerSource;
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

        Track?.Stop();
        Track?.Dispose();

        Track = trackStore.Get(newBeatmapSet.UseLocalSource ? newBeatmapSet.TrackPath : BeatmapSetUtility.GetTrackPath(newBeatmapSet));

        if (Track == null)
            return;

        if (mainScreenStack.CurrentScreen is SongSelectionScreen)
        {
            Track.Seek(newBeatmapSet.PreviewTime);
            Track.RestartPoint = newBeatmapSet.PreviewTime;
            Track.Looping = true;
        }

        Track.Start();

        Track.Completed += () =>
        {
            beatmapManager.NextBeatmapSet();
        };
    }

    /// <summary>
    /// Mute the track.
    /// </summary>
    public void Mute()
    {
        Track.Volume.Value = 0;
    }
}

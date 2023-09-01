using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics.Containers;

namespace Renako.Game.Audio;

/// <summary>
/// The audio manager that will play every track in Renako.
/// </summary>
public partial class RenakoAudioManager : CompositeDrawable
{
    public Track Track;
    private ITrackStore trackStore;

    [BackgroundDependencyLoader]
    private void load(AudioManager audioManagerSource)
    {
        trackStore = audioManagerSource.Tracks;
        Track = trackStore.Get("main-theme.mp3");
        Track.Looping = true;
    }
}

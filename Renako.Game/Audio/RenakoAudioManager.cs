using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics.Containers;
using osu.Framework.Screens;
using Renako.Game.Graphics.Screens;
using Renako.Game.Graphics.ScreenStacks;

namespace Renako.Game.Audio;

/// <summary>
/// The audio manager that will play every track in Renako.
/// </summary>
public partial class RenakoAudioManager : CompositeDrawable
{
    public Track Track;
    private ITrackStore trackStore;

    [Resolved]
    private RenakoScreenStack mainScreenStack { get; set; }

    [BackgroundDependencyLoader]
    private void load(AudioManager audioManagerSource)
    {
        mainScreenStack.BindableCurrentScreen.BindValueChanged((e) => changeTrackOnScreenChanged(e.OldValue, e.NewValue));

        trackStore = audioManagerSource.Tracks;
    }

    /// <summary>
    /// Change the track when the screen changed.
    /// </summary>
    /// <param name="oldScreen">The old <see cref="IScreen"/> before change.</param>
    /// <param name="newScreen">The new <see cref="IScreen"/> that will be changed.</param>
    private void changeTrackOnScreenChanged(IScreen oldScreen, IScreen newScreen)
    {
        if (oldScreen is WarningScreen && newScreen is StartScreen)
        {
            Track?.Stop();
            Track = trackStore.Get("theme/main-theme.mp3");
            Track.Looping = true;
            Track.Start();
        }
    }
}

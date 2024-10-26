using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics;
using Renako.Game.Audio;
using Renako.Game.Graphics.Drawables;

namespace Renako.Game.Tests.Visual.Drawables;

[TestFixture]
public partial class TestSceneAudioVisualizer : RenakoGameDrawableTestScene
{
    [Resolved]
    private RenakoAudioManager audioManager { get; set; }

    public TestSceneAudioVisualizer()
    {
        Add(new AudioVisualizer()
        {
            Anchor = Anchor.BottomLeft,
            Origin = Anchor.BottomLeft,
            RelativeSizeAxes = Axes.Both
        });
    }

    [BackgroundDependencyLoader]
    private void load(AudioManager audioManagerSource)
    {
        audioManager.Track = audioManagerSource.Tracks.Get("theme/main-theme.mp3");
        audioManager.Track.Looping = true;
        audioManager.Track.Start();
    }
}

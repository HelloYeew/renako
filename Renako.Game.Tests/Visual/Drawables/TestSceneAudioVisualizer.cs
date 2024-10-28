using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Textures;
using osuTK;
using Renako.Game.Audio;
using Renako.Game.Graphics.Drawables;

namespace Renako.Game.Tests.Visual.Drawables;

[TestFixture]
public partial class TestSceneAudioVisualizer : RenakoGameDrawableTestScene
{
    [Resolved]
    private RenakoAudioManager audioManager { get; set; }

    [Resolved]
    private TextureStore textureStore { get; set; }

    private AudioVisualizer audioVisualizer;

    [BackgroundDependencyLoader]
    private void load(AudioManager audioManagerSource)
    {
        audioManager.Track = audioManagerSource.Tracks.Get("theme/main-theme.mp3");
        audioManager.Track.Looping = true;
        audioManager.Track.Start();
        Add(audioVisualizer = new AudioVisualizer()
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            RelativeSizeAxes = Axes.Both,
            Alpha = 1f,
            Size = new Vector2(1)
        });
        AddStep("Add source", () => audioVisualizer.AddAmplitudeSource(audioManager.Track));
    }
}

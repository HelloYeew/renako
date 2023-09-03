using System;
using System.Collections.Generic;
using System.Globalization;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using Renako.Game.Audio;

namespace Renako.Game.Tests.Visual.Miscellaneous;

[TestFixture]
public partial class TestSceneAudioVisualizerConcept : RenakoTestScene
{
    [Cached]
    private RenakoAudioManager audioManager = new RenakoAudioManager();

    private List<SpriteText> frequencyAmplitudes = new List<SpriteText>();
    private List<Box> frequencyAmplitudesBox = new List<Box>();

    [BackgroundDependencyLoader]
    private void load(ITrackStore trackStore)
    {
        Dependencies.CacheAs(audioManager);
        audioManager.Track = trackStore.Get("main-theme.mp3");
        audioManager.Track.Looping = true;
        audioManager.Track.Start();
    }

    public TestSceneAudioVisualizerConcept()
    {
        for (int i = 0; i < 256; i++)
        {
            frequencyAmplitudes.Add(new SpriteText()
            {
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                Text = "0",
                Colour = Colour4.Black
            });
        }

        for (int i = 0; i < 256; i++)
        {
            frequencyAmplitudesBox.Add(new Box()
            {
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                Size = new osuTK.Vector2(0, 10),
                Colour = Colour4.White
            });
        }

        Add(new FillFlowContainer()
        {
            Anchor = Anchor.CentreRight,
            Origin = Anchor.CentreRight,
            Direction = FillDirection.Vertical,
            Spacing = new osuTK.Vector2(0, 0),
            Children = frequencyAmplitudes
        });

        Add(new FillFlowContainer()
        {
            Anchor = Anchor.CentreLeft,
            Origin = Anchor.CentreLeft,
            Direction = FillDirection.Vertical,
            Spacing = new osuTK.Vector2(0, 0),
            Children = frequencyAmplitudesBox
        });
    }

    protected override void Update()
    {
        base.Update();
        ReadOnlyMemory<float> readOnlyMemory = audioManager.Track.CurrentAmplitudes.FrequencyAmplitudes;

        // Log the frequency amplitudes.
        for (int i = 0; i < 256; i++)
        {
            frequencyAmplitudes[i].Text = readOnlyMemory.Span[i].ToString(CultureInfo.InvariantCulture);
            frequencyAmplitudesBox[i].Size = new osuTK.Vector2(readOnlyMemory.Span[i] * 1000, 5);
        }
    }

    // On exit, the track will be stopped.
    [TearDown]
    public void TearDown()
    {
        audioManager.Track.Stop();
    }
}

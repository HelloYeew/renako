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
[Ignore("Test scene is not supported in headless mode.")]
public partial class TestSceneAudioVisualizerConcept : RenakoTestScene
{
    [Cached]
    private RenakoAudioManager audioManager = new RenakoAudioManager();

    private List<Box> frequencyAmplitudesBox = new List<Box>();
    private SpriteText trackName;
    private SpriteText trackPosition;
    private SpriteText trackAverageAmplitude;
    private SpriteText trackFrequency;

    [BackgroundDependencyLoader]
    private void load(ITrackStore trackStore)
    {
        Dependencies.CacheAs(audioManager);
        audioManager.Track = trackStore.Get("theme/main-theme.mp3");
        audioManager.Track.Looping = true;
        audioManager.Track.Start();

        AddSliderStep<float>("Volume", 0, 1, 1, (value) => audioManager.Track.Volume.Value = value);
        AddSliderStep("Duration", 0, audioManager.Track.Length, 0, (value) => audioManager.Track.SeekAsync(value));
        AddSliderStep<float>("Frequency", 0, 3, 1, (value) => audioManager.Track.Frequency.Value = value);
        AddToggleStep("Resume", (_) => audioManager.Track.Start());
        AddToggleStep("Pause", (_) => audioManager.Track.Stop());
        AddToggleStep("Change track to Innocence", (_) =>
        {
            audioManager.Track.Stop();
            audioManager.Track = trackStore.Get("beatmaps/innocence.mp3");
            audioManager.Track.Start();
            audioManager.Track.Looping = true;
        });
        AddToggleStep("Change track to Main Theme", (_) =>
        {
            audioManager.Track.Stop();
            audioManager.Track = trackStore.Get("theme/main-theme.mp3");
            audioManager.Track.Start();
            audioManager.Track.Looping = true;
        });
        AddToggleStep("Change track to Play Theme", (_) =>
        {
            audioManager.Track.Stop();
            audioManager.Track = trackStore.Get("theme/play-theme.mp3");
            audioManager.Track.Start();
            audioManager.Track.Looping = true;
        });
    }

    public TestSceneAudioVisualizerConcept()
    {
        for (int i = 0; i < 256; i++)
        {
            frequencyAmplitudesBox.Add(new Box()
            {
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                Size = new osuTK.Vector2(0, 10),
                Colour = Colour4.White,
                Name = $"FrequencyAmplitudesBox{i}"
            });
        }

        Add(new FillFlowContainer()
        {
            Anchor = Anchor.BottomLeft,
            Origin = Anchor.BottomLeft,
            Direction = FillDirection.Horizontal,
            Spacing = new osuTK.Vector2(2, 0),
            Children = frequencyAmplitudesBox
        });
        Add(trackName = new SpriteText()
        {
            Anchor = Anchor.TopLeft,
            Origin = Anchor.TopLeft,
            Text = "Name : ",
            Colour = Colour4.White
        });
        Add(trackPosition = new SpriteText()
        {
            Anchor = Anchor.TopLeft,
            Origin = Anchor.TopLeft,
            Text = "Position: 0 / 0",
            Position = new osuTK.Vector2(0, 20),
            Colour = Colour4.White
        });
        Add(trackAverageAmplitude = new SpriteText()
        {
            Anchor = Anchor.TopLeft,
            Origin = Anchor.TopLeft,
            Text = "Average Amplitude: 0",
            Colour = Colour4.White,
            Position = new osuTK.Vector2(0, 40)
        });
        Add(trackFrequency = new SpriteText()
        {
            Anchor = Anchor.TopLeft,
            Origin = Anchor.TopLeft,
            Text = "Frequency: 1",
            Colour = Colour4.White,
            Position = new osuTK.Vector2(0, 60)
        });
    }

    protected override void Update()
    {
        base.Update();
        ReadOnlyMemory<float> readOnlyMemory = audioManager.Track.CurrentAmplitudes.FrequencyAmplitudes;

        if (audioManager.Track.IsRunning)
        {
            for (int i = 0; i < 256; i++)
            {
                frequencyAmplitudesBox[i].Size = new osuTK.Vector2(3.5f, readOnlyMemory.Span[i] * 1000);
            }
        }

        trackName.Text = $"Name : {audioManager.Track.Name}";
        trackPosition.Text = $"Position: {audioManager.Track.CurrentTime.ToString(CultureInfo.InvariantCulture)} / {audioManager.Track.Length.ToString(CultureInfo.InvariantCulture)}";
        trackAverageAmplitude.Text = $"Average Amplitude: {audioManager.Track.CurrentAmplitudes.Average.ToString(CultureInfo.InvariantCulture)}";
        trackFrequency.Text = $"Frequency: {audioManager.Track.Frequency.Value.ToString(CultureInfo.InvariantCulture)}";
    }
}

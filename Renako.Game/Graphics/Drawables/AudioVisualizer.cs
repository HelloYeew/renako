using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;
using Renako.Game.Audio;

namespace Renako.Game.Graphics.Drawables;

/// <summary>
/// The audio visualizer that will visualize the audio from the track.
/// </summary>
public partial class AudioVisualizer : CompositeDrawable
{
    [Resolved]
    private RenakoAudioManager audioManager { get; set; }

    private Colour4 barColour = Colour4.White;
    private float barAlpha = 1;

    public Colour4 BarColour
    {
        get => barColour;
        set
        {
            foreach (Box box in frequencyAmplitudesBox)
            {
                box.Colour = value;
            }
        }
    }

    public float BarAlpha
    {
        get => barAlpha;
        set
        {
            foreach (Box box in frequencyAmplitudesBox)
            {
                box.Alpha = value;
            }
        }
    }

    private readonly List<Box> frequencyAmplitudesBox = new List<Box>();

    [BackgroundDependencyLoader]
    private void load()
    {
        for (int i = 0; i < 150; i++)
        {
            frequencyAmplitudesBox.Add(new Box()
            {
                Anchor = Anchor.BottomLeft,
                Origin = Anchor.BottomLeft,
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(10, 0),
                Colour = barColour,
                Alpha = barAlpha
            });
        }

        InternalChild = new FillFlowContainer()
        {
            Anchor = Anchor.BottomLeft,
            Origin = Anchor.BottomLeft,
            Direction = FillDirection.Horizontal,
            Spacing = new Vector2(2, 0),
            RelativeSizeAxes = Axes.Both,
            Children = frequencyAmplitudesBox
        };
    }

    protected override void Update()
    {
        base.Update();

        if (audioManager.Track == null)
            return;

        ReadOnlyMemory<float> readOnlyMemory = audioManager.Track.CurrentAmplitudes.FrequencyAmplitudes;

        if (audioManager.Track.IsRunning)
        {
            for (int i = 0; i < 150; i++)
            {
                frequencyAmplitudesBox[i].ResizeTo(new Vector2(0.0065f, readOnlyMemory.Span[i] / 2), 50, Easing.OutQuint);
            }
        }
    }
}

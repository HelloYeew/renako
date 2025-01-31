using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Rendering.Vertices;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Textures;
using osu.Framework.Threading;
using osuTK;
using osuTK.Graphics;
using Renako.Game.Audio;

namespace Renako.Game.Graphics.Drawables;

/// <summary>
/// The audio visualizer that will visualize the audio from the track.
/// </summary>
public partial class AudioVisualizer : Drawable
{
    [Resolved]
    private RenakoAudioManager audioManager { get; set; }

    /// <summary>
    /// The number of bars to jump each update iteration.
    /// </summary>
    private const int index_offset = 5;

    /// <summary>
    /// The maximum length of each bar in the visualiser.
    /// </summary>
    private const float bar_length = 500;

    /// <summary>
    /// The number of bars in one rotation of the visualiser.
    /// </summary>
    private const int bars_per_visualiser = 150;

    /// <summary>
    /// How much should each bar go down each millisecond (based on a full bar).
    /// </summary>
    private const float decay_per_millisecond = 0.0024f;

    /// <summary>
    /// Number of milliseconds between each amplitude update.
    /// </summary>
    private float timeBetweenUpdates = 50;

    /// <summary>
    /// The minimum amplitude to show a bar.
    /// </summary>
    private const float amplitude_dead_zone = 1f / bar_length;

    /// <summary>
    /// The relative movement of bars based on input amplification. Defaults to 1.
    /// </summary>
    public float Magnitude { get; set; } = 1;

    private readonly float[] frequencyAmplitudes = new float[256];

    private IShader shader = null!;
    private Texture texture = null!;

    private ScheduledDelegate updateDelegate;

    public AudioVisualizer()
    {
        Blending = BlendingParameters.Additive;
    }

    private readonly List<IHasAmplitudes> amplitudeSources = new List<IHasAmplitudes>();

    /// <summary>
    /// Add a new amplitude source to this visualiser.
    /// </summary>
    /// <param name="amplitudeSource">The amplitude source (i.e. a <see cref="Track"/>).</param>
    public void AddAmplitudeSource(IHasAmplitudes amplitudeSource)
    {
        amplitudeSources.Add(amplitudeSource);
    }

    /// <summary>
    /// Clear the source of amplitude data.
    /// </summary>
    public void ClearAmplitudeSources()
    {
        amplitudeSources.Clear();
    }

    /// <summary>
    /// Change the track of the visualizer.
    /// </summary>
    /// <param name="track">The new track to visualize.</param>
    public void ChangeTrack(Track track)
    {
        amplitudeSources.Clear();
        AddAmplitudeSource(track);
    }

    public void ChangeSpeedByBpm(float bpm)
    {
        timeBetweenUpdates = 60000 / bpm / 30;

        if (updateDelegate != null)
        {
            updateDelegate.Cancel();
            updateDelegate = Scheduler.AddDelayed(updateAmplitudes, timeBetweenUpdates, true);
            updateDelegate.PerformRepeatCatchUpExecutions = false;
        }
    }

    [BackgroundDependencyLoader]
    private void load(IRenderer renderer, ShaderManager shaders)
    {
        texture = renderer.WhitePixel;
        shader = shaders.Load(VertexShaderDescriptor.TEXTURE_2, FragmentShaderDescriptor.TEXTURE);
    }

    private readonly float[] temporalAmplitudes = new float[ChannelAmplitudes.AMPLITUDES_SIZE];

    private void updateAmplitudes()
    {
        for (int i = 0; i < temporalAmplitudes.Length; i++)
            temporalAmplitudes[i] = 0;

        foreach (var source in amplitudeSources)
            addAmplitudesFromSource(source);

        for (int i = 0; i < bars_per_visualiser; i++)
        {
            float targetAmplitude = temporalAmplitudes[i % bars_per_visualiser];
            if (targetAmplitude > frequencyAmplitudes[i])
                frequencyAmplitudes[i] = targetAmplitude;
        }

        // indexOffset = (indexOffset + index_offset) % bars_per_visualiser;
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();

        updateDelegate = Scheduler.AddDelayed(updateAmplitudes, timeBetweenUpdates, true);
        updateDelegate.PerformRepeatCatchUpExecutions = false;
    }

    protected override void Update()
    {
        base.Update();

        float decayFactor = (float)Time.Elapsed * decay_per_millisecond;

        for (int i = 0; i < bars_per_visualiser; i++)
        {
            // 3% of extra bar length to make it a little faster when bar is almost at it's minimum
            frequencyAmplitudes[i] -= decayFactor * (frequencyAmplitudes[i] + 0.03f);
            if (frequencyAmplitudes[i] < 0)
                frequencyAmplitudes[i] = 0;
        }

        Invalidate(Invalidation.DrawNode);
    }

    protected override DrawNode CreateDrawNode() => new VisualizationDrawNode(this);

    private void addAmplitudesFromSource(IHasAmplitudes source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        var amplitudes = source.CurrentAmplitudes.FrequencyAmplitudes.Span;

        for (int i = 0; i < amplitudes.Length; i++)
        {
            if (i < temporalAmplitudes.Length)
                temporalAmplitudes[i] += amplitudes[i];
        }
    }

    private class VisualizationDrawNode : DrawNode
{
    protected new AudioVisualizer Source => (AudioVisualizer)base.Source;

    private IShader shader;
    private Texture texture;

    private float size;

    private static readonly Color4 bar_color4 = Color4Extensions.FromHex("f7f0f4").Opacity(0.2f);

    private readonly float[] audioData = new float[256];

    private IVertexBatch<TexturedVertex2D> vertexBatch;

    public VisualizationDrawNode(AudioVisualizer source) : base(source)
    {
    }

    public override void ApplyState()
    {
        base.ApplyState();

        shader = Source.shader;
        texture = Source.texture;
        size = Source.DrawSize.X;

        Source.frequencyAmplitudes.AsSpan().CopyTo(audioData);
    }

    protected override void Draw(IRenderer renderer)
    {
        base.Draw(renderer);

        vertexBatch ??= renderer.CreateQuadBatch<TexturedVertex2D>(100, 10);

        shader.Bind();

        Vector2 inflation = DrawInfo.MatrixInverse.ExtractScale().Xy;

        ColourInfo colourInfo = DrawColourInfo.Colour;
        colourInfo.ApplyChild(bar_color4);

        float barWidth = size / bars_per_visualiser;

        for (int j = 0; j < bars_per_visualiser; j++)
        {
            if (audioData[j] < amplitude_dead_zone)
                continue;

            float barHeight = bar_length * audioData[j];
            Vector2 barPosition = new Vector2(j * barWidth, Source.DrawSize.Y); // Set Y-coordinate to the bottom

            Quad rectangle = new Quad(
                Vector2Extensions.Transform(barPosition, DrawInfo.Matrix),
                Vector2Extensions.Transform(barPosition + new Vector2(0, -barHeight), DrawInfo.Matrix),
                Vector2Extensions.Transform(barPosition + new Vector2(barWidth, 0), DrawInfo.Matrix),
                Vector2Extensions.Transform(barPosition + new Vector2(barWidth, -barHeight), DrawInfo.Matrix)
            );

            renderer.DrawQuad(
                texture,
                rectangle,
                colourInfo,
                null,
                vertexBatch.AddAction,
                Vector2.Divide(inflation, new Vector2(barWidth, barHeight))
            );
        }

        shader.Unbind();
    }

    protected override void Dispose(bool isDisposing)
    {
        base.Dispose(isDisposing);
        vertexBatch?.Dispose();
    }
}
}

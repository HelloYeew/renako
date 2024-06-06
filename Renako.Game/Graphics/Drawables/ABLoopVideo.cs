using System.IO;
using JetBrains.Annotations;
using osu.Framework.Graphics.Video;

namespace Renako.Game.Graphics.Drawables;

/// <inheritdoc />
/// <summary>
/// The extension of <see cref="T:osu.Framework.Graphics.Video.Video" /> that allows the video to perform AB loop.
/// </summary>
public partial class AbLoopVideo : Video
{
    public AbLoopVideo(string filename, bool startAtCurrentTime = true)
        : base(filename, startAtCurrentTime)
    {
    }

    public AbLoopVideo([NotNull] Stream stream, bool startAtCurrentTime = true)
        : base(stream, startAtCurrentTime)
    {
    }

    /// <summary>
    /// Override the original loop property to always return false to prevent the video from looping like original <see cref="Video"/>.
    /// </summary>
    public override bool Loop => false;

    /// <summary>
    /// The time video start.
    /// </summary>
    public double StartTime { get; set; }

    /// <summary>
    /// The time video end and will loop back to the start time.
    /// To make it loop at end, set this value to video duration.
    /// </summary>
    public double EndTime { get; set; }

    /// <summary>
    /// Whether to loop to the restart time when the video reaches the end.
    /// </summary>
    public bool LoopToStartTime { get; set; }

    protected override void Update()
    {
        base.Update();

        // If current end time is more than the video duration, set it to the video duration.
        // Or if it's not set, set it to the video duration.
        if (EndTime > Duration || EndTime == 0)
        {
            EndTime = Duration;
        }

        if (PlaybackPosition >= EndTime && LoopToStartTime)
        {
            Seek(StartTime);
        }
    }
}

using System.IO;
using JetBrains.Annotations;
using osu.Framework.Graphics.Video;

namespace Renako.Game.Graphics.Drawables;

/// <summary>
/// The extension of <see cref="Video"/> that allows the video to loop back to the restart time when the video reaches the end.
/// </summary>
public partial class LoopableVideo : Video
{
    public LoopableVideo(string filename, bool startAtCurrentTime = true)
        : base(filename, startAtCurrentTime)
    {
    }

    public LoopableVideo([NotNull] Stream stream, bool startAtCurrentTime = true)
        : base(stream, startAtCurrentTime)
    {
    }

    /// <summary>
    /// Override the original loop property to always return false to prevent the video from looping like original <see cref="Video"/>.
    /// </summary>
    public override bool Loop => false;

    /// <summary>
    /// The time to loop the video back to.
    /// </summary>
    public double RestartTime { get; set; }

    /// <summary>
    /// Whether to loop to the restart time when the video reaches the end.
    /// </summary>
    public bool LoopToRestartTime { get; set; }

    protected override void Update()
    {
        base.Update();

        if (PlaybackPosition >= Duration && LoopToRestartTime)
        {
            Seek(RestartTime);
        }
    }
}

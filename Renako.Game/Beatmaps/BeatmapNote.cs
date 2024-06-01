namespace Renako.Game.Beatmaps;

public class BeatmapNote
{
    public NoteLane Lane { get; set; }
    public NoteType Type { get; set; }
    public double StartTime { get; set; }
    public double EndTime { get; set; }
}

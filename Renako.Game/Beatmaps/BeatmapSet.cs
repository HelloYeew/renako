namespace Renako.Game.Beatmaps;

/// <summary>
/// The beatmap set
/// </summary>
public class BeatmapSet
{
    public int ID { get; set; }

    /// <summary>
    /// The title of the beatmap set that's English.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// The localised title of the beatmap set.
    /// </summary>
    public string TitleUnicode { get; set; }

    /// <summary>
    /// Artist of the beatmap set that's English.
    /// </summary>
    public string Artist { get; set; }

    /// <summary>
    /// The localised artist of the beatmap set.
    /// </summary>
    public string ArtistUnicode { get; set; }

    /// <summary>
    /// The source of the track in this beatmap set in English.
    /// </summary>
    public string Source { get; set; }

    /// <summary>
    /// The localised source of the track in this beatmap set.
    /// </summary>
    public string SourceUnicode { get; set; }

    /// <summary>
    /// Total length of the beatmap set in milliseconds.
    /// </summary>
    public int TotalLength { get; set; }

    /// <summary>
    /// The start time in millisecond of the preview of the beatmap set.
    /// </summary>
    public int PreviewTime { get; set; }

    /// <summary>
    /// Beat per minute of the track in the beatmap set.
    /// </summary>
    public double BPM { get; set; }

    /// <summary>
    /// Main creator of the beatmap set. If more than one creator in different beatmap, use creator in beatmap instead.
    /// </summary>
    public string Creator { get; set; }

    /// <summary>
    /// Whether the beatmap set has video or not.
    /// </summary>
    public bool HasVideo { get; set; }

    /// <summary>
    /// Whether this beatmap get the file from local source (use game's storage) or not.
    /// </summary>
    public bool UseLocalSource { get; set; }

    /// <summary>
    /// The path of the album cover of the beatmap set.
    /// </summary>
    public string CoverPath { get; set; }

    /// <summary>
    /// The path of the track file that will play in the beatmap set.
    /// </summary>
    public string TrackPath { get; set; }

    /// <summary>
    /// The path of the background image of the beatmap set.
    /// </summary>
    public string BackgroundPath { get; set; }

    /// <summary>
    /// The path of the video of the beatmap set. If the beatmap set doesn't have a video, set this as blank.
    /// </summary>
    public string VideoPath { get; set; } = "";

    public override string ToString()
    {
        return $"{Title} - {Artist}";
    }

    public override bool Equals(object obj)
    {
        return obj is BeatmapSet beatmapSet &&
               ID == beatmapSet.ID &&
               Title == beatmapSet.Title &&
               TitleUnicode == beatmapSet.TitleUnicode &&
               Artist == beatmapSet.Artist &&
               ArtistUnicode == beatmapSet.ArtistUnicode &&
               Source == beatmapSet.Source &&
               SourceUnicode == beatmapSet.SourceUnicode &&
               TotalLength == beatmapSet.TotalLength &&
               PreviewTime == beatmapSet.PreviewTime &&
               BPM == beatmapSet.BPM &&
               Creator == beatmapSet.Creator &&
               HasVideo == beatmapSet.HasVideo &&
               UseLocalSource == beatmapSet.UseLocalSource &&
               CoverPath == beatmapSet.CoverPath &&
               TrackPath == beatmapSet.TrackPath &&
               BackgroundPath == beatmapSet.BackgroundPath &&
               VideoPath == beatmapSet.VideoPath;
    }
}

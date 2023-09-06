namespace Renako.Game.Beatmaps;

/// <summary>
/// The beatmap or difficulty in <see cref="BeatmapSet"/>
/// </summary>
public class Beatmap
{
    public int ID { get; set; }

    /// <summary>
    /// The object of <see cref="BeatmapSet"/> that this beatmap belongs to.
    /// </summary>
    public BeatmapSet BeatmapSet { get; set; }

    /// <summary>
    /// Author of the beatmap.
    /// </summary>
    public string Creator { get; set; }

    /// <summary>
    /// The difficulty name of the beatmap.
    /// </summary>
    public string DifficultyName { get; set; }

    /// <summary>
    /// The difficulty level of the beatmap that's roughly based from rating.
    /// </summary>
    public DifficultyLevel DifficultyLevel { get; set; }

    /// <summary>
    /// The calculated difficulty rating of the beatmap.
    /// </summary>
    public double DifficultyRating { get; set; }

    /// <summary>
    /// The path of the background file.
    /// </summary>
    public string BackgroundPath { get; set; }
}

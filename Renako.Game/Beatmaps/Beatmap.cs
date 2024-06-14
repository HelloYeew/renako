using System;
using System.Collections.Generic;

namespace Renako.Game.Beatmaps;

/// <summary>
/// The beatmap or difficulty in <see cref="BeatmapSet"/>
/// </summary>
public class Beatmap
{
    /// <summary>
    /// Version of beatmap file, will increase if there's a change in the file structure.
    /// </summary>
    public int Version { get; set; } = 1;

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
    /// The calculated difficulty rating of the beatmap.
    /// </summary>
    public double DifficultyRating { get; set; }

    /// <summary>
    /// The path of the background file.
    /// </summary>
    public string BackgroundPath { get; set; }

    /// <summary>
    /// The list of notes in the beatmap.
    /// </summary>
    public BeatmapNote[] Notes { get; set; }

    public override string ToString()
    {
        return $"{BeatmapSet.Title} [{DifficultyName}] by {Creator} ({DifficultyRating:0.00})";
    }

    public override bool Equals(object obj)
    {
        return obj is Beatmap beatmap &&
               ID == beatmap.ID &&
               EqualityComparer<BeatmapSet>.Default.Equals(BeatmapSet, beatmap.BeatmapSet) &&
               Creator == beatmap.Creator &&
               DifficultyName == beatmap.DifficultyName &&
               DifficultyRating == beatmap.DifficultyRating &&
               BackgroundPath == beatmap.BackgroundPath;
    }

    protected bool Equals(Beatmap other)
    {
        return ID == other.ID && Equals(BeatmapSet, other.BeatmapSet) && Creator == other.Creator && DifficultyName == other.DifficultyName && DifficultyRating.Equals(other.DifficultyRating) && BackgroundPath == other.BackgroundPath;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(ID, BeatmapSet, Creator, DifficultyName, DifficultyRating, BackgroundPath);
    }

    /// <summary>
    /// Create a deep copy of the beatmap
    /// </summary>
    /// <returns>A deep copy of the beatmap</returns>
    public Beatmap Clone()
    {
        return (Beatmap)MemberwiseClone();
    }
}

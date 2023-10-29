using Renako.Game.Beatmaps;

namespace Renako.Game.Utilities;

/// <summary>
/// Set of utilities for <see cref="Beatmap"/>
/// </summary>
public class BeatmapUtility
{
    /// <summary>
    /// Returns the <see cref="DifficultyLevel"/> based on the difficulty rating.
    /// </summary>
    /// <param name="difficultyRating">The difficulty rating to calculate</param>
    /// <returns>The <see cref="DifficultyLevel"/> based on the difficulty rating</returns>
    public static DifficultyLevel CalculateDifficultyLevel(double difficultyRating)
    {
        return difficultyRating switch
        {
            < 4 => DifficultyLevel.Basic,
            < 8 => DifficultyLevel.Advance,
            < 12 => DifficultyLevel.Expert,
            < 16 => DifficultyLevel.Master,
            _ => DifficultyLevel.Virtuoso
        };
    }

    /// <summary>
    /// Returns the <see cref="DifficultyLevel"/> based on the difficulty rating.
    /// </summary>
    /// <param name="beatmap">The <see cref="Beatmap"/> to calculate the difficulty level from</param>
    /// <returns>The <see cref="DifficultyLevel"/> based on the difficulty rating</returns>
    public static DifficultyLevel CalculateDifficultyLevel(Beatmap beatmap)
    {
        return CalculateDifficultyLevel(beatmap.DifficultyRating);
    }

    /// <summary>
    /// Returns the folder name of the beatmap set in game storage (Format: {ID} {Title} - {Artist})
    /// </summary>
    /// <param name="beatmapSet"></param>
    /// <returns></returns>
    public static string GetFolderName(BeatmapSet beatmapSet)
    {
        return $"{beatmapSet.ID} {beatmapSet.Title} - {beatmapSet.Artist}";
    }
}

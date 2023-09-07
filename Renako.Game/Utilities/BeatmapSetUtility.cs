using osu.Framework.Allocation;
using Renako.Game.Beatmaps;
using Renako.Game.Stores;

namespace Renako.Game.Utilities;

/// <summary>
/// A helper class for <see cref="BeatmapSet"/>
/// </summary>
public class BeatmapSetUtility
{
    [Resolved]
    private BeatmapsCollection beatmapsCollection { get; set; }

    /// <summary>
    /// Get the formatted time using in UI from <see cref="BeatmapSet"/> in the format of mm:ss
    /// </summary>
    /// <param name="beatmapSet">The <see cref="BeatmapSet"/> to get the formatted time from</param>
    /// <returns>The formatted time</returns>
    public static string GetFormattedTime(BeatmapSet beatmapSet)
    {
        int totalMilliseconds = beatmapSet.TotalLength;
        return $"{totalMilliseconds / 60000:00}:{totalMilliseconds / 1000 % 60:00}";
    }
}

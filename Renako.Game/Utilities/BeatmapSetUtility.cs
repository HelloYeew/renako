using System.IO;
using System.Text.Json;
using Renako.Game.Beatmaps;

namespace Renako.Game.Utilities;

/// <summary>
/// A helper class for <see cref="BeatmapSet"/>
/// </summary>
public class BeatmapSetUtility
{
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

    /// <summary>
    /// Deserialize a <see cref="BeatmapSet"/> from a <see cref="string"/>
    /// </summary>
    /// <param name="json">The <see cref="string"/> to deserialize from</param>
    /// <returns>The deserialized <see cref="BeatmapSet"/></returns>
    public static BeatmapSet Deserialize(string json)
    {
        return JsonSerializer.Deserialize<BeatmapSet>(json);
    }

    /// <summary>
    /// Deserialize a <see cref="BeatmapSet"/> from a beatmapset file <see cref="Stream"/>
    /// </summary>
    /// <param name="stream">The <see cref="Stream"/> to deserialize from</param>
    /// <returns>The deserialized <see cref="BeatmapSet"/></returns>
    public static BeatmapSet Deserialize(Stream stream)
    {
        return JsonSerializer.Deserialize<BeatmapSet>(new StreamReader(stream).ReadToEnd());
    }
}

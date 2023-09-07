using System.Collections.Generic;
using System.Linq;
using Renako.Game.Beatmaps;
using Renako.Game.Utilities;

namespace Renako.Game.Stores;

/// <summary>
/// A class that stores all beatmap and the operations related to it.
/// </summary>
public class BeatmapsCollection
{
    public List<BeatmapSet> BeatmapSets { get; set; }
    public List<Beatmap> Beatmaps { get; set; }

    /// <summary>
    /// Set value to <see cref="BeatmapSets"/> and <see cref="Beatmaps"/> by generating random beatmaps.
    /// </summary>
    public void GenerateTestCollection()
    {
        BeatmapTestUtility beatmapTestUtility = new BeatmapTestUtility();
        BeatmapSets = beatmapTestUtility.BeatmapSets;
        Beatmaps = beatmapTestUtility.Beatmaps;
    }

    /// <summary>
    /// Get array of <see cref="Beatmap"/> from collection that's in <see cref="BeatmapSet"/> object.
    /// Compare using the ID of <see cref="BeatmapSet"/> and the <see cref="Beatmap.BeatmapSet"/> property.
    /// </summary>
    /// <param name="beatmapSet">The <see cref="BeatmapSet"/> to get the <see cref="Beatmap"/> from</param>
    /// <returns>Array of <see cref="Beatmap"/></returns>
    public Beatmap[] GetBeatmapsFromBeatmapSet(BeatmapSet beatmapSet)
    {
        return Beatmaps.FindAll((e) => e.BeatmapSet.ID == beatmapSet.ID).ToArray();
    }

    /// <summary>
    /// Get the min and max difficulty rating from <see cref="Beatmap"/> in the <see cref="BeatmapSet"/>
    /// </summary>
    /// <param name="beatmapSet">The <see cref="BeatmapSet"/> to get the difficulty rating from</param>
    /// <returns>The formatted difficulty rating</returns>
    public Dictionary<string, int> GetMixMaxDifficultyLevel(BeatmapSet beatmapSet)
    {
        Beatmap[] beatmaps = GetBeatmapsFromBeatmapSet(beatmapSet);
        double min = beatmaps.Min((e) => e.DifficultyRating);
        double max = beatmaps.Max((e) => e.DifficultyRating);
        return new Dictionary<string, int>()
        {
            { "min", (int)min },
            { "max", (int)max }
        };
    }
}

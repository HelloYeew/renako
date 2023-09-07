using System.Collections.Generic;
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
}

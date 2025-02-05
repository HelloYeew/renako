using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Framework.Utils;

namespace Renako.Game.Beatmaps;

/// <summary>
/// Class that contain all function related to beatmap and collection of beatmaps.
/// </summary>
public partial class BeatmapManager : CompositeDrawable
{
    [Resolved]
    private BeatmapsCollection beatmapsCollection { get; set; }

    [Resolved]
    private WorkingBeatmap workingBeatmap { get; set; }

    // [BackgroundDependencyLoader]
    // private void load(BeatmapsCollection beatmapsCollection, WorkingBeatmap workingBeatmap)
    // {
    //     this.beatmapsCollection = beatmapsCollection;
    //     this.workingBeatmap = workingBeatmap;
    // }

    /// <summary>
    /// Set the working beatmap to the next beatmap in the collection.
    /// </summary>
    /// <param name="random">Whether to set the next beatmap randomly.</param>
    public void NextBeatmapSet(bool random = false)
    {
        if (random)
        {
            workingBeatmap.BeatmapSet = beatmapsCollection.BeatmapSets[RNG.Next(beatmapsCollection.BeatmapSets.Count)];
        }
        else
        {
            // TODO: Avoid theme song
            int currentBeatmapSetIndex = beatmapsCollection.BeatmapSets.IndexOf(workingBeatmap.BeatmapSet);

            workingBeatmap.BeatmapSet = currentBeatmapSetIndex == beatmapsCollection.BeatmapSets.Count - 1 ? beatmapsCollection.BeatmapSets[0] : beatmapsCollection.BeatmapSets[currentBeatmapSetIndex + 1];

            workingBeatmap.Beatmap = beatmapsCollection.GetFirstBeatmapFromBeatmapSet(workingBeatmap.BeatmapSet);
        }
    }

    /// <summary>
    /// Set the working beatmap to the previous beatmap in the collection.
    /// </summary>
    public void PreviousBeatmapSet()
    {
        int currentBeatmapSetIndex = beatmapsCollection.BeatmapSets.IndexOf(workingBeatmap.BeatmapSet);

        workingBeatmap.BeatmapSet = currentBeatmapSetIndex == 0 ? beatmapsCollection.BeatmapSets[beatmapsCollection.BeatmapSets.Count - 1] : beatmapsCollection.BeatmapSets[currentBeatmapSetIndex - 1];

        workingBeatmap.Beatmap = beatmapsCollection.GetFirstBeatmapFromBeatmapSet(workingBeatmap.BeatmapSet);
    }
}

using System.Collections.Generic;
using System.Linq;
using Renako.Game.Utilities;

namespace Renako.Game.Beatmaps;

/// <summary>
/// A class that stores all beatmap and the operations related to it.
/// </summary>
public class BeatmapsCollection
{
    public List<BeatmapSet> BeatmapSets { get; set; }
    public List<Beatmap> Beatmaps { get; set; }

    public BeatmapsCollection()
    {
        BeatmapSets = new List<BeatmapSet>();
        Beatmaps = new List<Beatmap>();
        addThemeSongBeatmapSet();
    }

    /// <summary>
    /// Set value to <see cref="BeatmapSets"/> and <see cref="Beatmaps"/> by generating random beatmaps.
    /// </summary>
    public void GenerateTestCollection()
    {
        BeatmapTestUtility beatmapTestUtility = new BeatmapTestUtility();
        BeatmapSets = beatmapTestUtility.BeatmapSets;
        Beatmaps = beatmapTestUtility.Beatmaps;
    }

    private void addThemeSongBeatmapSet()
    {
        // TODO: Get proper Title, Artist and Source translated from source
        // TODO: Also change background
        BeatmapSets.Add(new BeatmapSet()
        {
            ID = -1,
            Title = "Night Beauty",
            TitleUnicode = "月下美人",
            Artist = "Manbou 2nd class",
            ArtistUnicode = "まんぼう二等兵",
            Source = "Manbou 2nd class (Renako Main Menu Theme Song)",
            SourceUnicode = "まんぼう二等兵 (Renako Main Menu Theme Song)",
            TotalLength = 525000,
            PreviewTime = 171970,
            BPM = 180,
            Creator = "Renako",
            HasVideo = false,
            UseLocalSource = true,
            CoverPath = "Screen/main-background.jpeg",
            TrackPath = "theme/main-theme.mp3",
            BackgroundPath = "Screen/main-background.jpeg",
            Hide = true
        });
        BeatmapSets.Add(new BeatmapSet()
        {
            ID = -2,
            Title = "Emotional catharsis",
            TitleUnicode = "情動カタルシス",
            Artist = "Manbou 2nd class",
            ArtistUnicode = "まんぼう二等兵",
            Source = "Manbou 2nd class (Renako Play Menu Theme Song)",
            SourceUnicode = "まんぼう二等兵 (Renako Play Menu Theme Song)",
            TotalLength = 311000,
            PreviewTime = 0,
            BPM = 148,
            Creator = "Renako",
            HasVideo = false,
            UseLocalSource = true,
            CoverPath = "Screen/play-background.jpg",
            TrackPath = "theme/play-theme.mp3",
            BackgroundPath = "Screen/play-background.jpg",
            Hide = true
        });
    }

    /// <summary>
    /// Get array of <see cref="Beatmap"/> from collection that's in <see cref="BeatmapSet"/> object.
    /// Compare using the ID of <see cref="BeatmapSet"/> and the <see cref="Beatmap.BeatmapSet"/> property.
    /// </summary>
    /// <param name="beatmapSet">The <see cref="BeatmapSet"/> to get the <see cref="Beatmap"/> from</param>
    /// <param name="sortByDifficulty">Sort the <see cref="Beatmap"/> by difficulty rating or not</param>
    /// <returns>Array of <see cref="Beatmap"/></returns>
    public Beatmap[] GetBeatmapsFromBeatmapSet(BeatmapSet beatmapSet, bool sortByDifficulty = true)
    {
        Beatmap[] beatmaps = Beatmaps.FindAll(e => e.BeatmapSet.ID == beatmapSet.ID).ToArray();

        if (sortByDifficulty)
        {
            beatmaps = beatmaps.OrderBy(e => e.DifficultyRating).ToArray();
        }

        return beatmaps;
    }

    /// <summary>
    /// Get the min and max difficulty rating from <see cref="Beatmap"/> in the <see cref="BeatmapSet"/>
    /// </summary>
    /// <param name="beatmapSet">The <see cref="BeatmapSet"/> to get the difficulty rating from</param>
    /// <returns>The formatted difficulty rating</returns>
    public Dictionary<string, int> GetMixMaxDifficultyLevel(BeatmapSet beatmapSet)
    {
        Beatmap[] beatmaps = GetBeatmapsFromBeatmapSet(beatmapSet);

        if (beatmaps.Length == 0)
        {
            return new Dictionary<string, int>()
            {
                { "min", 0 },
                { "max", 0 }
            };
        }

        double min = beatmaps.Min(e => e.DifficultyRating);
        double max = beatmaps.Max(e => e.DifficultyRating);
        return new Dictionary<string, int>()
        {
            { "min", (int)min },
            { "max", (int)max }
        };
    }

    /// <summary>
    /// Get the <see cref="BeatmapSet"/> by ID.
    /// </summary>
    /// <param name="id">The ID of the <see cref="BeatmapSet"/></param>
    /// <returns>The <see cref="BeatmapSet"/></returns>
    public BeatmapSet GetBeatmapSetByID(int id)
    {
        return BeatmapSets.Find(e => e.ID == id);
    }

    /// <summary>
    /// Get the <see cref="Beatmap"/> by ID.
    /// </summary>
    /// <param name="id">The ID of the <see cref="Beatmap"/></param>
    /// <returns>The <see cref="Beatmap"/></returns>
    public Beatmap GetBeatmapByID(int id)
    {
        return Beatmaps.Find(e => e.ID == id);
    }

    /// <summary>
    /// Clear <see cref="BeatmapSets"/> and <see cref="Beatmaps"/> list.
    /// </summary>
    public void Clear()
    {
        BeatmapSets.Clear();
        Beatmaps.Clear();
    }
}

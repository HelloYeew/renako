using System;
using System.Collections.Generic;
using System.Linq;
using Renako.Game.Beatmaps;

namespace Renako.Game.Utilities;

/// <summary>
/// A utility class for generating beatmap for testing.
/// </summary>
public class BeatmapTestUtility
{
    private readonly Random random = new Random();

    public List<BeatmapSet> BeatmapSets { get; set; }

    public List<Beatmap> Beatmaps { get; set; }

    public DifficultyLevel[] AllDifficultyLevels = Enum.GetValues(typeof(DifficultyLevel)).Cast<DifficultyLevel>().ToArray();

    public string[] RandomNameList = new string[]
    {
        "HelloYeew",
        "Renako"
    };

    public BeatmapTestUtility()
    {
        BeatmapSets = GetLocalBeatmapSets();
        Beatmaps = GenerateRandomBeatmaps();
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public List<BeatmapSet> GetLocalBeatmapSets()
    {
        return new List<BeatmapSet>()
        {
            new BeatmapSet()
            {
                ID = 1,
                Title = "Innocence (TV Size)",
                TitleUnicode = "Innocence (TV Size)",
                Artist = "Eir Aoi",
                ArtistUnicode = "藍井エイル",
                Source = "Sword Art Online",
                SourceUnicode = "ソードアート・オンライン",
                TotalLength = 89000,
                PreviewTime = 54300,
                BPM = 183,
                Creator = GetRandomCreatorName(),
                HasVideo = false,
                UseLocalSource = true,
                CoverPath = "Beatmaps/Album/innocence-tv-size.jpg",
                TrackPath = "beatmaps/innocence-tv-size.mp3",
                BackgroundPath = "Beatmaps/Background/innocence-tv-size.jpg"
            },
            new BeatmapSet()
            {
                ID = 2,
                Title = "ideal white",
                TitleUnicode = "ideal white",
                Artist = "Mashiro Ayano",
                ArtistUnicode = "綾野ましろ",
                Source = "Fate/stay night: Unlimited Blade Works",
                SourceUnicode = "Fate/stay night: Unlimited Blade Works",
                TotalLength = 267000,
                PreviewTime = 47900,
                BPM = 180,
                Creator = GetRandomCreatorName(),
                HasVideo = false,
                UseLocalSource = true,
                CoverPath = "Beatmaps/Album/ideal-white.jpg",
                TrackPath = "beatmaps/ideal-white.mp3",
                BackgroundPath = "Beatmaps/Background/ideal-white.jpg"
            },
            new BeatmapSet()
            {
                ID = 3,
                Title = "Ano Yume wo Nazotte",
                TitleUnicode = "あの夢をなぞって",
                Artist = "YOASOBI",
                ArtistUnicode = "YOASOBI",
                Source = "Ano Yume wo Nazotte - Single",
                SourceUnicode = "あの夢をなぞって - Single",
                TotalLength = 242000,
                PreviewTime = 78500,
                BPM = 180,
                Creator = GetRandomCreatorName(),
                HasVideo = false,
                UseLocalSource = true,
                CoverPath = "Beatmaps/Album/ano-yume-wo-nazotte.jpg",
                TrackPath = "beatmaps/ano-yume-wo-nazotte.mp3",
                BackgroundPath = "Beatmaps/Background/ano-yume-wo-nazotte.jpg"
            },
            new BeatmapSet()
            {
                ID = 4,
                Title = "SnowMix",
                TitleUnicode = "SnowMix",
                Artist = "marasy & Hatsune Miku",
                ArtistUnicode = "marasy & 初音ミク",
                Source = "Winter Sky Fantasia",
                SourceUnicode = "幽天のファンタジア",
                TotalLength = 246000,
                PreviewTime = 85000,
                BPM = 185,
                Creator = GetRandomCreatorName(),
                HasVideo = false,
                UseLocalSource = true,
                CoverPath = "Beatmaps/Album/snowmix.jpg",
                TrackPath = "beatmaps/snowmix.mp3",
                BackgroundPath = "Beatmaps/Background/snowmix.jpg"
            },
            new BeatmapSet()
            {
                ID = 5,
                Title = "Moonlightspeed",
                TitleUnicode = "Moonlightspeed",
                Artist = "Midnight Grand Orchestra",
                ArtistUnicode = "Midnight Grand Orchestra",
                Source = "Moonlightspeed - Single",
                SourceUnicode = "Moonlightspeed - Single",
                TotalLength = 189000,
                PreviewTime = 42000,
                BPM = 176,
                Creator = GetRandomCreatorName(),
                HasVideo = false,
                UseLocalSource = true,
                CoverPath = "Beatmaps/Album/moonlightspeed.jpg",
                TrackPath = "beatmaps/moonlightspeed.m4a",
                BackgroundPath = "Beatmaps/Background/moonlightspeed.jpg"
            },
            new BeatmapSet()
            {
                ID = 6,
                Title = "Kani*Do-Luck!",
                TitleUnicode = "カニ*Do-Luck!",
                Artist = "Aiuraba",
                ArtistUnicode = "あいうらば",
                Source = "Aiura",
                SourceUnicode = "あいうら",
                TotalLength = 226000,
                PreviewTime = 58750,
                BPM = 210,
                Creator = GetRandomCreatorName(),
                HasVideo = false,
                UseLocalSource = true,
                CoverPath = "Beatmaps/Album/kani-do-luck.jpg",
                TrackPath = "beatmaps/kani-do-luck.mp3",
                BackgroundPath = "Beatmaps/Background/kani-do-luck.jpg"
            }
        };
    }

    /// <summary>
    /// Generate
    /// </summary>
    /// <returns></returns>
    public List<Beatmap> GenerateRandomBeatmaps()
    {
        int incrementID = 1;
        List<Beatmap> beatmaps = new List<Beatmap>();

        foreach (BeatmapSet beatmapSet in GetLocalBeatmapSets())
        {
            foreach (DifficultyLevel difficultyLevel in AllDifficultyLevels)
            {
                beatmaps.Add(new Beatmap()
                {
                    ID = incrementID,
                    BeatmapSet = beatmapSet,
                    Creator = GetRandomCreatorName(),
                    DifficultyLevel = difficultyLevel,
                    DifficultyName = difficultyLevel.ToString(),
                    DifficultyRating = random.NextDouble() * 10,
                    BackgroundPath = beatmapSet.BackgroundPath
                });
                incrementID++;
            }
        }

        return beatmaps;
    }

    /// <summary>
    /// Return a random name from the list.
    /// </summary>
    /// <returns>A random name</returns>
    public string GetRandomCreatorName()
    {
        return RandomNameList[random.Next(0, RandomNameList.Length)];
    }
}

public enum BeatmapSetInformation
{
    InnocenceTVSize,
    IdealWhite,
    AnoYumeWoNazotte,
    SnowMix,
    Moonlightspeed,
    KaniDoLuck
}

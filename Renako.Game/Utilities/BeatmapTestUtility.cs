using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Utils;
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

    public string[] RandomNameList =
    {
        "HelloYeew",
        "Renako",
        "Yuuki",
        "Mai"
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
                Source = "Sword Art Online II",
                SourceUnicode = "ソードアート・オンライン II",
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
                Title = "Courage",
                TitleUnicode = "courage",
                Artist = "Haruka Tomatsu",
                ArtistUnicode = "戸松遥",
                Source = "Sword Art Online II",
                SourceUnicode = "ソードアート・オンライン II",
                TotalLength = 254000,
                PreviewTime = 49700,
                BPM = 98,
                Creator = GetRandomCreatorName(),
                HasVideo = false,
                UseLocalSource = true,
                CoverPath = "Beatmaps/Album/courage.jpg",
                TrackPath = "beatmaps/courage.mp3",
                BackgroundPath = "Beatmaps/Background/courage.jpg"
            },
            new BeatmapSet()
            {
                ID = 3,
                Title = "Passionate Starmine",
                TitleUnicode = "熱色スターマイン",
                Artist = "Roselia",
                ArtistUnicode = "Roselia",
                Source = "Passionate Starmine - EP",
                SourceUnicode = "熱色スターマイン - EP",
                TotalLength = 267000,
                PreviewTime = 242000,
                BPM = 187,
                Creator = GetRandomCreatorName(),
                HasVideo = false,
                UseLocalSource = true,
                CoverPath = "Beatmaps/Album/passionate-starmine.jpg",
                TrackPath = "beatmaps/passionate-starmine.mp3",
                BackgroundPath = "Beatmaps/Background/passionate-starmine.jpg"
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
                PreviewTime = 189430,
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
                Title = "Mirai No Museum",
                TitleUnicode = "未来のミュージアム",
                Artist = "Perfume",
                ArtistUnicode = "Perfume",
                Source = "Mirai No Museum - EP",
                SourceUnicode = "未来のミュージアム - EP",
                TotalLength = 202000,
                PreviewTime = 158600,
                BPM = 170,
                Creator = GetRandomCreatorName(),
                HasVideo = false,
                UseLocalSource = true,
                CoverPath = "Beatmaps/Album/mirai-no-museum.jpg",
                TrackPath = "beatmaps/mirai-no-museum.mp3",
                BackgroundPath = "Beatmaps/Background/mirai-no-museum.jpg"
            },
            new BeatmapSet()
            {
                ID = 6,
                Title = "ONESELF",
                TitleUnicode = "ONESELF",
                Artist = "Twinfield feat. Hatsune Miku",
                ArtistUnicode = "Twinfield feat. 初音ミク",
                Source = "ONESELF - Single",
                SourceUnicode = "ONESELF - Single",
                TotalLength = 241000,
                PreviewTime = 190875,
                BPM = 220,
                Creator = GetRandomCreatorName(),
                HasVideo = false,
                UseLocalSource = true,
                CoverPath = "Beatmaps/Album/oneself.jpg",
                TrackPath = "beatmaps/oneself.mp3",
                BackgroundPath = "Beatmaps/Background/oneself.jpg"
            },
            // Added for edge case for very long title and dark background
            new BeatmapSet()
            {
                ID = 7,
                Title = "I'm Glad You're Evil Too",
                TitleUnicode = "きみも悪い人でよかった",
                Artist = "PinocchioP feat. Hatsune Miku",
                ArtistUnicode = "ピノキオピー feat. 初音ミク",
                Source = "HUMAN",
                SourceUnicode = "HUMAN",
                TotalLength = 391000,
                PreviewTime = 268000,
                BPM = 120,
                Creator = GetRandomCreatorName(),
                HasVideo = false,
                UseLocalSource = true,
                CoverPath = "Beatmaps/Album/im-glad-youre-evil-too.jpg",
                TrackPath = "beatmaps/im-glad-youre-evil-too.mp3",
                BackgroundPath = "Beatmaps/Background/im-glad-youre-evil-too.jpg"
            }
        };
    }

    /// <summary>
    /// Generate random beatmaps for testing.
    /// </summary>
    /// <returns></returns>
    public List<Beatmap> GenerateRandomBeatmaps()
    {
        int incrementID = 0;
        List<Beatmap> beatmaps = new List<Beatmap>();

        foreach (BeatmapSet beatmapSet in GetLocalBeatmapSets())
        {
            foreach (DifficultyLevel difficultyLevel in AllDifficultyLevels)
            {
                double difficultyRating = random.NextDouble() * 18;

                List<BeatmapNote> notes = new List<BeatmapNote>();

                for (int i = 0; i < beatmapSet.TotalLength / 1000; i++)
                {
                    notes.Add(new BeatmapNote()
                    {
                        Lane = (NoteLane)RNG.Next(0, 4),
                        Type = NoteType.BasicNote,
                        StartTime = (i + 1) * 1000,
                        EndTime = (i + 1) * 1000
                    });
                }

                beatmaps.Add(new Beatmap()
                {
                    ID = incrementID,
                    BeatmapSet = beatmapSet,
                    Creator = GetRandomCreatorName(),
                    DifficultyName = BeatmapUtility.CalculateDifficultyLevel(difficultyRating) + " " + difficultyLevel,
                    DifficultyRating = difficultyRating,
                    BackgroundPath = beatmapSet.BackgroundPath,
                    Notes = notes.ToArray()
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

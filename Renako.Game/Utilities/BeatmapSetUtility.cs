﻿using System.IO;
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

    /// <summary>
    /// Returns the folder name of the beatmap set in game storage (Format: {ID} {Title} - {Artist})
    /// </summary>
    /// <param name="beatmapSet"></param>
    /// <returns></returns>
    public static string GetFolderName(BeatmapSet beatmapSet)
    {
        return $"{beatmapSet.ID} {StringUtility.CleanFileName(beatmapSet.Title)} - {StringUtility.CleanFileName(beatmapSet.Artist)}";
    }

    /// <summary>
    /// Return the path of the cover image of <see cref="BeatmapSet"/>.
    /// </summary>
    /// <param name="beatmapSet">The <see cref="BeatmapSet"/> to get the cover path from</param>
    /// <returns>The path of the cover image</returns>
    public static string GetCoverPath(BeatmapSet beatmapSet)
    {
        return "beatmaps/" + GetFolderName(beatmapSet) + "/" + StringUtility.CleanFileName(beatmapSet.CoverPath);
    }

    /// <summary>
    /// Return the path of the background image of <see cref="BeatmapSet"/>.
    /// </summary>
    /// <param name="beatmapSet">The <see cref="BeatmapSet"/> to get the background path from</param>
    /// <returns>The path of the background image</returns>
    public static string GetBackgroundPath(BeatmapSet beatmapSet)
    {
        return "beatmaps/" + GetFolderName(beatmapSet) + "/" + StringUtility.CleanFileName(beatmapSet.BackgroundPath);
    }

    /// <summary>
    /// Return the path of the track of <see cref="BeatmapSet"/>.
    /// </summary>
    /// <param name="beatmapSet">The <see cref="BeatmapSet"/> to get the track path from</param>
    /// <returns>The path of the track</returns>
    public static string GetTrackPath(BeatmapSet beatmapSet)
    {
        return "beatmaps/" + GetFolderName(beatmapSet) + "/" + StringUtility.CleanFileName(beatmapSet.TrackPath);
    }

    /// <summary>
    /// Returns the file name of the beatmap set in game storage (Format: {ID} {Title} - {Artist})
    /// </summary>
    /// <param name="beatmapSet"></param>
    /// <returns></returns>
    public static string GetBeatmapSetFileName(BeatmapSet beatmapSet)
    {
        return $"{beatmapSet.ID} {StringUtility.CleanFileName(beatmapSet.Title)} - {StringUtility.CleanFileName(beatmapSet.Artist)}";
    }

    /// <summary>
    /// Return the path of the video of <see cref="BeatmapSet"/>, will return empty string if the beatmap set doesn't have a video.
    /// </summary>
    /// <param name="beatmapSet">The <see cref="BeatmapSet"/> to get the video path from</param>
    /// <returns></returns>
    public static string GetVideoPath(BeatmapSet beatmapSet)
    {
        if (!beatmapSet.HasVideo)
            return "";

        return "beatmaps/" + GetFolderName(beatmapSet) + "/" + beatmapSet.VideoPath;
    }
}

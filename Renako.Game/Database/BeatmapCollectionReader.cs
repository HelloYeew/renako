using System.Collections.Generic;
using System.IO;
using osu.Framework.Logging;
using osu.Framework.Platform;
using Renako.Game.Beatmaps;
using Renako.Game.Utilities;

namespace Renako.Game.Database;

/// <summary>
/// Read beatmap file from storage and add it to the game's beatmap collection
/// </summary>
public class BeatmapCollectionReader
{
    private readonly Storage gameStorage;
    private readonly BeatmapsCollection beatmapsCollection;

    public const string BEATMAP_FOLDER_NAME = "beatmaps";

    public BeatmapCollectionReader(Storage storage, BeatmapsCollection beatmapsCollection)
    {
        gameStorage = storage.GetStorageForDirectory(BEATMAP_FOLDER_NAME);
        this.beatmapsCollection = beatmapsCollection;
    }

    public void Read(bool clear = true, bool sortOnFinish = true)
    {
        if (clear)
        {
            beatmapsCollection.Beatmaps.Clear();
            beatmapsCollection.BeatmapSets.Clear();
        }

        // Get list of folder in beatmap folder
        List<string> beatmapSetFolders = new List<string>(gameStorage.GetDirectories(string.Empty));

        foreach (string beatmapSetFolder in beatmapSetFolders)
        {
            Logger.Log("Reading beatmap set folder: " + beatmapSetFolder, LoggingTarget.Database);
        }

        foreach (string beatmapSetFolder in beatmapSetFolders)
        {
            // Get list of beatmap file in beatmap set folder
            List<string> beatmapFiles = new List<string>(gameStorage.GetFiles(beatmapSetFolder));

            foreach (string beatmapFile in beatmapFiles)
            {
                if (beatmapFile.EndsWith(".rks"))
                {
                    Logger.Log("Reading beatmap file: " + beatmapFile, LoggingTarget.Database);
                    // Deserialize rks file using JSON deserializer
                    Stream stream = gameStorage.GetStream(beatmapFile);
                    BeatmapSet beatmapSet = BeatmapSetUtility.Deserialize(stream);
                    beatmapSet.UseLocalSource = false;
                    stream.Close();

                    // Find beatmapset in beatmap collection using ID
                    if (beatmapsCollection.BeatmapSets.FindAll(e => e.ID == beatmapSet.ID).Count == 0)
                    {
                        // Add beatmapset to beatmap collection if not found
                        beatmapsCollection.BeatmapSets.Add(beatmapSet);
                        Logger.Log("Beatmap set added to collection: " + beatmapSet.ID, LoggingTarget.Database);
                    }
                    else
                    {
                        Logger.Log("Beatmap set already exist in collection: " + beatmapSet.ID, LoggingTarget.Database);
                    }
                }
                else if (beatmapFile.EndsWith(".rkb"))
                {
                    Logger.Log("Reading beatmap file: " + beatmapFile, LoggingTarget.Database);
                    // Deserialize rkb file using JSON deserializer
                    Stream stream = gameStorage.GetStream(beatmapFile);
                    Beatmap beatmap = BeatmapUtility.Deserialize(stream);
                    stream.Close();

                    // Find beatmap in beatmap collection using ID
                    if (beatmapsCollection.Beatmaps.FindAll(e => e.ID == beatmap.ID).Count == 0)
                    {
                        // Add beatmap to beatmap collection if not found
                        beatmapsCollection.Beatmaps.Add(beatmap);
                        Logger.Log("Beatmap added to collection: " + beatmap.ID, LoggingTarget.Database);
                    }
                    else
                    {
                        Logger.Log("Beatmap already exist in collection: " + beatmap.ID, LoggingTarget.Database);
                    }
                }
            }
        }

        if (sortOnFinish)
        {
            beatmapsCollection.SortBeatmapSetsByID();
        }
    }
}

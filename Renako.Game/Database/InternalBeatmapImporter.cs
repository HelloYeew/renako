using System.Collections.Generic;
using System.IO;
using osu.Framework.Audio;
using osu.Framework.Logging;
using osu.Framework.Platform;
using Renako.Game.Beatmaps;
using Renako.Game.Utilities;

namespace Renako.Game.Database;

/// <summary>
/// Import beatmap from game resource to game storage
/// </summary>
public class InternalBeatmapImporter
{
    private readonly Storage gameStorage;
    private readonly AudioManager audioManager;
    private readonly BeatmapTestUtility beatmapTestUtility = new BeatmapTestUtility();

    public string BeatmapFolderName = "beatmaps";

    public InternalBeatmapImporter(AudioManager audioManager, GameHost host)
    {
        gameStorage = host.Storage.GetStorageForDirectory(BeatmapFolderName);
        this.audioManager = audioManager;
    }

    public void Import()
    {
        List<BeatmapSet> beatmapSets = beatmapTestUtility.BeatmapSets;
        List<Beatmap> beatmaps = beatmapTestUtility.Beatmaps;

        foreach (BeatmapSet beatmapSet in beatmapSets)
        {
            string folderName = BeatmapUtility.GetFolderName(beatmapSet);

            // List all file in track store
            foreach (var file in audioManager.GetTrackStore().GetAvailableResources())
            {
                Logger.Log($"File: {file}");
            }

            // Export track to folder
            // Get stream from track
            Stream track = audioManager.GetTrackStore().GetStream($"{beatmapSet.TrackPath}");
            // Create file in game storage
            Stream writeStream = gameStorage.CreateFileSafely($"{folderName}/{beatmapSet.TrackPath.Split("/")[1]}");
            // Copy stream to file
            track.CopyTo(writeStream);
            // Close stream
            track.Close();
            writeStream.Close();
        }
    }
}

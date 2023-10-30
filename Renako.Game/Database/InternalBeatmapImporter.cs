using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
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
            string beatmapSetFileName = BeatmapUtility.GetBeatmapSetFileName(beatmapSet);

            Logger.Log("Importing beatmap set: " + folderName, LoggingTarget.Database);
            // track
            Stream trackStream = audioManager.GetTrackStore().GetStream($"{beatmapSet.TrackPath}");
            Stream writeStream = gameStorage.CreateFileSafely($"{folderName}/{beatmapSet.TrackPath.Split("/")[1]}");
            trackStream.CopyTo(writeStream);
            trackStream.Close();
            writeStream.Close();

            // album cover and background

            // beatmapset info
            writeStream = gameStorage.CreateFileSafely($"{folderName}/{beatmapSetFileName}.rks");
            string beatmapSetJsonSeting = JsonSerializer.Serialize(beatmapSet);
            byte[] beatmapSetJsonBytes = Encoding.UTF8.GetBytes(beatmapSetJsonSeting);
            writeStream.Write(beatmapSetJsonBytes);
            writeStream.Close();
        }
    }
}

using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using osu.Framework.Audio;
using osu.Framework.Graphics.Textures;
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
    private readonly TextureStore textureStore;
    private readonly BeatmapTestUtility beatmapTestUtility = new BeatmapTestUtility();

    public const string BEATMAP_FOLDER_NAME = "beatmaps";

    public InternalBeatmapImporter(AudioManager audioManager, TextureStore textureStore, GameHost host)
    {
        gameStorage = host.Storage.GetStorageForDirectory(BEATMAP_FOLDER_NAME);
        this.audioManager = audioManager;
        this.textureStore = textureStore;
    }

    public void Import()
    {
        List<BeatmapSet> beatmapSets = beatmapTestUtility.BeatmapSets;
        List<Beatmap> beatmaps = beatmapTestUtility.Beatmaps;

        foreach (BeatmapSet beatmapSet in beatmapSets)
        {
            string folderName = BeatmapSetUtility.GetFolderName(beatmapSet);
            string beatmapSetFileName = BeatmapUtility.GetBeatmapSetFileName(beatmapSet);

            Logger.Log("Importing beatmap set: " + folderName, LoggingTarget.Database);
            // track
            Stream sourceStream = audioManager.GetTrackStore().GetStream($"{beatmapSet.TrackPath}");
            Stream writeStream = gameStorage.CreateFileSafely($"{folderName}/{beatmapSet.TrackPath.Split("/")[1]}");
            sourceStream.CopyTo(writeStream);
            sourceStream.Close();
            writeStream.Close();

            // album cover and background
            sourceStream = textureStore.GetStream($"{beatmapSet.CoverPath}");
            writeStream = gameStorage.CreateFileSafely($"{folderName}/cover.jpg");
            sourceStream.CopyTo(writeStream);
            sourceStream.Close();
            writeStream.Close();
            sourceStream = textureStore.GetStream($"{beatmapSet.BackgroundPath}");
            writeStream = gameStorage.CreateFileSafely($"{folderName}/background.jpg");
            sourceStream.CopyTo(writeStream);
            sourceStream.Close();
            writeStream.Close();

            // beatmapset info
            BeatmapSet copyBeatmapSet = beatmapSet.Clone();
            copyBeatmapSet.BackgroundPath = "background.jpg";
            copyBeatmapSet.CoverPath = "cover.jpg";
            copyBeatmapSet.TrackPath = beatmapSet.TrackPath.Split("/")[1];
            writeStream = gameStorage.CreateFileSafely($"{folderName}/{beatmapSetFileName}.rks");
            string beatmapSetJsonSeting = JsonSerializer.Serialize(copyBeatmapSet, new JsonSerializerOptions()
            {
                WriteIndented = true
            });
            byte[] beatmapSetJsonBytes = Encoding.UTF8.GetBytes(beatmapSetJsonSeting);
            writeStream.Write(beatmapSetJsonBytes);
            writeStream.Close();

            // beatmap info
            foreach (Beatmap beatmap in beatmaps)
            {
                if (beatmap.BeatmapSet.ID == beatmapSet.ID)
                {
                    string beatmapFileName = BeatmapUtility.GetBeatmapFileName(beatmap);
                    Beatmap copyBeatmap = beatmap.Clone();
                    copyBeatmap.BackgroundPath = "background.jpg";
                    copyBeatmap.BeatmapSet = copyBeatmapSet;
                    writeStream = gameStorage.CreateFileSafely($"{folderName}/{beatmapFileName}.rkb");
                    string beatmapJsonSetting = JsonSerializer.Serialize(copyBeatmap, new JsonSerializerOptions()
                    {
                        WriteIndented = true
                    });
                    byte[] beatmapJsonBytes = Encoding.UTF8.GetBytes(beatmapJsonSetting);
                    writeStream.Write(beatmapJsonBytes);
                    writeStream.Close();
                }
            }
        }
    }
}

using System.Linq;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Screens;
using osuTK.Input;
using Renako.Game.Beatmaps;
using Renako.Game.Configurations;
using Renako.Game.Graphics.Screens;
using Renako.Game.Utilities;

namespace Renako.Game.Tests.Visual.Screens;

public partial class TestSceneSongSelectionScreen : RenakoGameDrawableManualnputManagerTestScene
{
    [Cached]
    private BeatmapsCollection beatmapsCollection = new BeatmapsCollection();

    [Cached]
    private WorkingBeatmap workingBeatmap = new WorkingBeatmap();

    [Resolved]
    private RenakoConfigManager configManager { get; set; }

    [SetUp]
    protected new void SetUp()
    {
        beatmapsCollection.GenerateTestCollection();
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();
        configManager.SetValue(RenakoSetting.DisableIdleMode, true);
    }

    [Test]
    public void TestSongSelectionScreen()
    {
        AddStep("set working beatmap", () => workingBeatmap.BeatmapSet = beatmapsCollection.BeatmapSets.First());
        AddStep("add song selection screen", () => MainScreenStack.Push(new SongSelectionScreen()));
        WaitForScreen();
        AddAssert("screen loaded", () => MainScreenStack.CurrentScreen is SongSelectionScreen);
        AddStep("rerun", rerunScreen);
    }

    [Test]
    public void TestSongSelectionScreenBasicInteraction()
    {
        AddStep("set working beatmap", () => workingBeatmap.BeatmapSet = beatmapsCollection.BeatmapSets.First());
        AddStep("add song selection screen", () => MainScreenStack.Push(new SongSelectionScreen()));
        WaitForScreen();
        AddAssert("screen loaded", () => MainScreenStack.CurrentScreen is SongSelectionScreen);
        AddStep("rerun", rerunScreen);
        Beatmap lastBeatmap = workingBeatmap.Beatmap;
        AddStep("try click go button", () => PressKeyOnce(Key.Enter));
        AddStep("try select beatmap", () => PressKeyOnce(Key.Right));
        AddAssert("beatmap changed", () => !Equals(workingBeatmap.Beatmap, lastBeatmap));
    }

    [Test]
    public void TestSongSelectionScreenWithNoBeatmapSets()
    {
        AddStep("set working beatmap", () => workingBeatmap.BeatmapSet = beatmapsCollection.BeatmapSets.First());
        AddStep("clear beatmap sets", () => beatmapsCollection.BeatmapSets.Clear());
        AddStep("add song selection screen", rerunScreen);
        AddAssert("screen loaded", () => MainScreenStack.CurrentScreen is SongSelectionScreen);
    }

    [Test]
    public void TestSongSelectionScreenBeatmapSetWithNoBeatmaps()
    {
        AddStep("set working beatmap", () => workingBeatmap.BeatmapSet = beatmapsCollection.BeatmapSets.First());
        AddStep("clear collection", () => beatmapsCollection.Clear());
        AddStep("add dummy blank beatmapset", () => beatmapsCollection.BeatmapSets.Add(new BeatmapTestUtility().GetLocalBeatmapSets().First()));
        AddAssert("beatmapset added", () => beatmapsCollection.BeatmapSets.Count > 0);
        AddAssert("beatmaps empty", () => beatmapsCollection.Beatmaps.Count == 0);
        AddStep("add song selection screen", rerunScreen);
        WaitForScreen();
        AddAssert("screen loaded", () => MainScreenStack.CurrentScreen is SongSelectionScreen);
        AddStep("try click go button", () => PressKeyOnce(Key.Enter));
        AddAssert("working beatmap still null", () => workingBeatmap.Beatmap == null);
    }

    [Test]
    public void TestSongSelectionScreenHaveOldWorkingBeatmapSet()
    {
        AddStep("set working beatmap", () => workingBeatmap.BeatmapSet = beatmapsCollection.BeatmapSets.First());
        AddStep("set old working beatmap in config", () => configManager.SetValue(RenakoSetting.LatestBeatmapSetID, 3));
        AddStep("add song selection screen", rerunScreen);
        WaitForScreen();
        AddAssert("screen loaded", () => MainScreenStack.CurrentScreen is SongSelectionScreen);
        AddAssert("check working beatmap set", () => workingBeatmap.BeatmapSet.ID == 3);
    }

    private void rerunScreen()
    {
        Scheduler.Add(() =>
        {
            MainScreenStack.CurrentScreen?.Exit();
            MainScreenStack.Push(new SongSelectionScreen());
        });
    }
}

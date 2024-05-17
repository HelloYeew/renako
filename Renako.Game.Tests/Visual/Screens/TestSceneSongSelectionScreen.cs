using System.Linq;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Screens;
using osuTK.Input;
using Renako.Game.Beatmaps;
using Renako.Game.Graphics.Screens;
using Renako.Game.Utilities;

namespace Renako.Game.Tests.Visual.Screens;

public partial class TestSceneSongSelectionScreen : RenakoGameDrawableManualnputManagerTestScene
{
    [Cached]
    private BeatmapsCollection beatmapsCollection = new BeatmapsCollection();

    [Cached]
    private WorkingBeatmap workingBeatmap = new WorkingBeatmap();

    [SetUp]
    protected new void SetUp()
    {
        beatmapsCollection.GenerateTestCollection();
    }

    [Test]
    public void TestSongSelectionScreen()
    {
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
        AddStep("clear beatmap sets", () => beatmapsCollection.BeatmapSets.Clear());
        AddStep("add song selection screen", rerunScreen);
        AddAssert("screen loaded", () => MainScreenStack.CurrentScreen is SongSelectionScreen);
    }

    [Test]
    public void TestSongSelectionScreenBeatmapSetWithNoBeatmaps()
    {
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

    private void rerunScreen()
    {
        Scheduler.Add(() =>
        {
            MainScreenStack.CurrentScreen?.Exit();
            MainScreenStack.Push(new SongSelectionScreen());
        });
    }
}

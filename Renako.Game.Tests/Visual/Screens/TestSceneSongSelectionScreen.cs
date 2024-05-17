using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Screens;
using osuTK.Input;
using Renako.Game.Beatmaps;
using Renako.Game.Graphics.Screens;

namespace Renako.Game.Tests.Visual.Screens;

public partial class TestSceneSongSelectionScreen : RenakoGameDrawableManualnputManagerTestScene
{
    [Cached]
    private BeatmapsCollection beatmapsCollection = new BeatmapsCollection();

    [Cached]
    private WorkingBeatmap workingBeatmap = new WorkingBeatmap();

    [SetUp]
    public new void SetUp() => beatmapsCollection.GenerateTestCollection();

    [Test]
    public void TestSongSelectionScreen()
    {
        AddStep("add song selection screen", () => MainScreenStack.Push(new SongSelectionScreen()));
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
    }

    private void rerunScreen()
    {
        MainScreenStack.CurrentScreen?.Exit();
        MainScreenStack.Push(new SongSelectionScreen());
    }
}

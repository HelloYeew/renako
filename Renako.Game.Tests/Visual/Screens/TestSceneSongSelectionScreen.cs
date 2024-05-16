using System.Linq;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osu.Framework.Screens;
using osu.Framework.Utils;
using osuTK;
using osuTK.Input;
using Renako.Game.Beatmaps;
using Renako.Game.Graphics.Screens;

namespace Renako.Game.Tests.Visual.Screens;

public partial class TestSceneSongSelectionScreen : ManualInputTestScene
{
    [Resolved]
    private BeatmapsCollection beatmapsCollection { get; set; }

    [Resolved]
    private WorkingBeatmap workingBeatmap { get; set; }

    [BackgroundDependencyLoader]
    private void load()
    {
        beatmapsCollection.GenerateTestCollection();
    }

    [SetUp]
    public new void SetUp() => beatmapsCollection.GenerateTestCollection();

    [Test]
    public void TestSongSelectionScreen()
    {
        AddStep("add song selection screen", () => MainScreenStack.Push(new SongSelectionScreen()));
        AddAssert("screen loaded", () => MainScreenStack.CurrentScreen is SongSelectionScreen);
        AddStep("rerun", rerunScreen);
        var lastBeatmap = workingBeatmap.Beatmap;
        AddStep("try click go button", () =>
        {
            InputManager.PressKey(Key.Enter);
            InputManager.ReleaseKey(Key.Enter);
        });
        AddStep("try select beatmap", () =>
        {
            InputManager.PressKey(Key.Right);
            InputManager.ReleaseKey(Key.Right);
        });
        AddAssert("beatmap changed", () => !Equals(workingBeatmap.Beatmap, lastBeatmap));
    }

    [Test]
    public void TestSongSelectionScreenWithNoBeatmapSets()
    {
        AddStep("clear beatmap sets", () => beatmapsCollection.BeatmapSets.Clear());
        AddStep("add song selection screen", rerunScreen);
    }

    [Test]
    public void TestSongSelectionScreenWithNoBeatmaps()
    {
        AddStep("add beatmapset with no beatmaps", () =>
        {
            BeatmapSet firstBeatmapSet = beatmapsCollection.BeatmapSets.First().Clone();
            firstBeatmapSet.ID = RNG.Next(10, 1000);
            firstBeatmapSet.Title = "No Beatmaps";
            beatmapsCollection.BeatmapSets.Add(firstBeatmapSet);
            workingBeatmap.BeatmapSet = firstBeatmapSet;
        });
        AddStep("add song selection screen", rerunScreen);
        AddStep("try click go button", () => InputManager.PressKey(Key.Enter));
    }

    private void rerunScreen()
    {
        MainScreenStack.CurrentScreen?.Exit();
        MainScreenStack.Push(new SongSelectionScreen());
    }
}

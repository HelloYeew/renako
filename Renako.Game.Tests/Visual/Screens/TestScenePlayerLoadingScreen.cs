using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Textures;
using osu.Framework.Screens;
using Renako.Game.Beatmaps;
using Renako.Game.Graphics.Screens;

namespace Renako.Game.Tests.Visual.Screens;

public partial class TestScenePlayerLoadingScreen : GameDrawableTestScene
{
    [Resolved]
    private WorkingBeatmap workingBeatmap { get; set; }

    [Cached]
    private BeatmapsCollection beatmapsCollection = new BeatmapsCollection();

    [Resolved]
    private TextureStore textureStore { get; set; }

    [Test]
    public void TestPlayerLoadingScreen()
    {
        beatmapsCollection.GenerateTestCollection();
        AddStep("set current working beatmap", () =>
        {
            workingBeatmap.BeatmapSet = beatmapsCollection.BeatmapSets[0];
            workingBeatmap.Beatmap = beatmapsCollection.GetBeatmapsFromBeatmapSet(workingBeatmap.BeatmapSet)[0];
            BackgroundScreenStack.ChangeBackground(textureStore.Get(workingBeatmap.BeatmapSet.BackgroundPath));
        });
        AddStep("add player loading screen", () => MainScreenStack.Push(new PlayerLoadingScreen()));
        AddAssert("screen loaded", () => MainScreenStack.CurrentScreen is PlayerLoadingScreen);
        AddStep("rerun", () =>
        {
            MainScreenStack.CurrentScreen?.Exit();
            MainScreenStack.Push(new PlayerLoadingScreen());
        });
    }
}

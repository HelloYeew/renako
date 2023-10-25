using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Screens;
using Renako.Game.Beatmaps;
using Renako.Game.Graphics.Screens;

namespace Renako.Game.Tests.Visual.Screens;

public partial class TestSceneSongSelectionScreen : GameDrawableTestScene
{
    [BackgroundDependencyLoader]
    private void load(BeatmapsCollection beatmapsCollection)
    {
        beatmapsCollection.GenerateTestCollection();
    }

    [Test]
    public void TestSongSelectionScreen()
    {
        AddStep("add song selection screen", () => MainScreenStack.Push(new SongSelectionScreen()));
        AddAssert("screen loaded", () => MainScreenStack.CurrentScreen is SongSelectionScreen);
        AddStep("rerun", () => {
            MainScreenStack.CurrentScreen?.Exit();
            MainScreenStack.Push(new SongSelectionScreen());
        });
    }
}

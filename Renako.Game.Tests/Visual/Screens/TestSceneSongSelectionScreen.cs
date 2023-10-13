using NUnit.Framework;
using osu.Framework.Screens;
using Renako.Game.Graphics.Screens;

namespace Renako.Game.Tests.Visual.Screens;

public partial class TestSceneSongSelectionScreen : GameDrawableTestScene
{
    [Test]
    public void TestSongSelectionScreen()
    {
        AddStep("load beatmap test collection", () => beatmapsCollection.GenerateTestCollection());
        AddStep("add song selection screen", () => MainScreenStack.Push(new SongSelectionScreen()));
        AddAssert("screen loaded", () => MainScreenStack.CurrentScreen is SongSelectionScreen);
        AddStep("rerun", () => {
            MainScreenStack.CurrentScreen?.Exit();
            MainScreenStack.Push(new SongSelectionScreen());
            audioManager.Mute();
        });
    }
}

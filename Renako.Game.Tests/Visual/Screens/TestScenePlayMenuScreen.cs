using NUnit.Framework;
using osu.Framework.Screens;
using Renako.Game.Graphics.Screens;

namespace Renako.Game.Tests.Visual.Screens;

public partial class TestScenePlayMenuScreen : GameDrawableTestScene
{
    [Test]
    public void TestPlayMenuScreen()
    {
        AddStep("add play menu screen", () => MainScreenStack.Push(new PlayMenuScreen()));
        AddAssert("screen loaded", () => MainScreenStack.CurrentScreen is PlayMenuScreen);
        AddStep("rerun", () => {
            MainScreenStack.CurrentScreen?.Exit();
            MainScreenStack.Push(new PlayMenuScreen());
        });
    }
}

using NUnit.Framework;
using osu.Framework.Screens;
using Renako.Game.Graphics.Screens;

namespace Renako.Game.Tests.Visual.Screens;

public partial class TestSceneStartScreen : GameDrawableTestScene
{
    [Test]
    public void TestStartScreen()
    {
        AddStep("add start screen", () => MainScreenStack.Push(new StartScreen()));
        AddAssert("screen loaded", () => MainScreenStack.CurrentScreen is StartScreen);
        AddStep("rerun", () => {
            MainScreenStack.CurrentScreen.Exit();
            MainScreenStack.Push(new StartScreen());
        });
    }
}

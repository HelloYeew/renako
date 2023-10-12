using NUnit.Framework;
using osu.Framework.Screens;
using Renako.Game.Graphics.Screens;

namespace Renako.Game.Tests.Visual.Screens;

public partial class TestSceneStartScreen : GameDrawableTestScene
{
    [Test]
    public void TestStartScreen()
    {
        AddStep("add start screen", () => mainScreenStack.Push(new StartScreen()));
        AddAssert("screen loaded", () => mainScreenStack.CurrentScreen is StartScreen);
        AddStep("rerun", () => {
            mainScreenStack.CurrentScreen.Exit();
            mainScreenStack.Push(new StartScreen());
        });
    }
}

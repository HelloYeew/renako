using NUnit.Framework;
using osu.Framework.Screens;
using Renako.Game.Graphics.Screens;

namespace Renako.Game.Tests.Visual.Screens;

public partial class TestSceneWarningScreen : GameDrawableTestScene
{
    [Test]
    public void TestWarningScreen()
    {
        AddStep("add warning screen", () => mainScreenStack.Push(new WarningScreen()));
        AddAssert("screen loaded", () => mainScreenStack.CurrentScreen is WarningScreen);
        AddStep("rerun", () => {
            mainScreenStack.CurrentScreen.Exit();
            mainScreenStack.Push(new WarningScreen());
        });
    }
}

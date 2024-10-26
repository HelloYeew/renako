using NUnit.Framework;
using osu.Framework.Screens;
using Renako.Game.Graphics.Screens;

namespace Renako.Game.Tests.Visual.Screens;

public partial class TestSceneWarningScreen : RenakoGameDrawableTestScene
{
    [Test]
    public void TestWarningScreen()
    {
        AddStep("add warning screen", () => MainScreenStack.Push(new WarningScreen()));
        AddAssert("screen loaded", () => MainScreenStack.CurrentScreen is WarningScreen);
        AddStep("rerun", () => {
            MainScreenStack.CurrentScreen.Exit();
            MainScreenStack.Push(new WarningScreen());
        });
    }
}

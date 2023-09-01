using NUnit.Framework;
using osu.Framework.Screens;
using Renako.Game.Graphics.Screens;

namespace Renako.Game.Tests.Visual.Screens;

[TestFixture]
public partial class TestSceneStartScreen : RenakoTestScene
{
    public TestSceneStartScreen()
    {
        Add(MainScreenStack);
        MainScreenStack.Push(new StartScreen());
        AddAssert("screen loaded", () => MainScreenStack.CurrentScreen is StartScreen);
        AddStep("rerun", () => {
            MainScreenStack.CurrentScreen.Exit();
            MainScreenStack.Push(new StartScreen());
        });
    }
}

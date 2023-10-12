using NUnit.Framework;
using osu.Framework.Screens;
using Renako.Game.Graphics.Screens;

namespace Renako.Game.Tests.Visual.Screens;

public partial class TestScenePlayMenuScreen : GameDrawableTestScene
{
    [Test]
    public void TestPlayMenuScreen()
    {
        AddStep("add play menu screen", () => mainScreenStack.Push(new PlayMenuScreen()));
        AddAssert("screen loaded", () => mainScreenStack.CurrentScreen is PlayMenuScreen);
        AddStep("rerun", () => {
            mainScreenStack.CurrentScreen?.Exit();
            mainScreenStack.Push(new PlayMenuScreen());
        });
    }
}

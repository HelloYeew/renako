using NUnit.Framework;
using osu.Framework.Screens;
using Renako.Game.Graphics;
using Renako.Game.Graphics.Screens;

namespace Renako.Game.Tests.Visual;

[TestFixture]
public partial class TestSceneWarningScreen : RenakoTestScene
{
    private RenakoMainScreenStack mainScreenStack = new RenakoMainScreenStack();

    public TestSceneWarningScreen()
    {
        Add(mainScreenStack);
        mainScreenStack.Push(new WarningScreen());
        AddStep("rerun", () => {
            mainScreenStack.CurrentScreen.Exit();
            mainScreenStack.Push(new WarningScreen());
        });
    }
}

using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Screens;
using Renako.Game.Graphics.Screens;
using Renako.Game.Graphics.ScreenStacks;

namespace Renako.Game.Tests.Visual.Screens;

[TestFixture]
public partial class TestSceneWarningScreen : RenakoTestScene
{
    private ScreenStack mainScreenStack = new ScreenStack();

    [Resolved]
    private RenakoBackgroundScreenStack backgroundScreenStack { get; set; }

    public TestSceneWarningScreen()
    {

        Add(backgroundScreenStack = new RenakoBackgroundScreenStack());
        Add(mainScreenStack);
        mainScreenStack.Push(new WarningScreen());
        AddAssert("screen loaded", () => mainScreenStack.CurrentScreen is WarningScreen);
        AddStep("rerun", () => {
            mainScreenStack.CurrentScreen.Exit();
            mainScreenStack.Push(new WarningScreen());
        });
    }
}

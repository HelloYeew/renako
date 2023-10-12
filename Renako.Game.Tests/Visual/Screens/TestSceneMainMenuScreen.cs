using NUnit.Framework;
using osu.Framework.Screens;
using Renako.Game.Graphics.Screens;

namespace Renako.Game.Tests.Visual.Screens;

public partial class TestSceneMainMenuScreen : GameDrawableTestScene
{
    [Test]
    public void TestMainMenuScreen()
    {
        AddStep("move logo to main menu position", () => logoScreenStack.LogoScreenObject.MoveToMainMenu());
        AddStep("add main menu screen", () => mainScreenStack.Push(new MainMenuScreen()));
        AddAssert("screen loaded", () => mainScreenStack.CurrentScreen is MainMenuScreen);
        AddStep("rerun", () => {
            mainScreenStack.CurrentScreen.Exit();
            mainScreenStack.Push(new MainMenuScreen());
        });
    }
}

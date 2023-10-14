﻿using NUnit.Framework;
using osu.Framework.Screens;
using Renako.Game.Graphics.Screens;

namespace Renako.Game.Tests.Visual.Screens;

public partial class TestSceneMainMenuScreen : GameDrawableTestScene
{
    [Test]
    public void TestMainMenuScreen()
    {
        AddStep("move logo to main menu position", () => LogoScreenStack.LogoScreenObject.MoveToMainMenu());
        AddStep("add main menu screen", () => MainScreenStack.Push(new MainMenuScreen()));
        AddAssert("screen loaded", () => MainScreenStack.CurrentScreen is MainMenuScreen);
        AddStep("rerun", () => {
            MainScreenStack.CurrentScreen.Exit();
            MainScreenStack.Push(new MainMenuScreen());
        });
    }
}

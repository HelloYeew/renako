using NUnit.Framework;
using osuTK.Input;
using Renako.Game.Graphics.Screens;

namespace Renako.Game.Tests.Visual.InGame;

public partial class TestSceneSongSelectionScreenInGame : RenakoGameManualInputManagerTestScene
{
    [Test]
    public void TestSongSelectionScreenAccessFromGame()
    {
        AddUntilStep("wait for game to load", () => TestGame.IsLoaded);
        AddUntilStep("wait until start screen", () => TestGame.GetCurrentScreenType() == typeof(StartScreen));
        AddStep("go to main menu", () => PressKeyOnce(Key.Enter));
        AddAssert("current screen is main menu", () => TestGame.GetCurrentScreenType() == typeof(MainMenuScreen));
        AddStep("go to play menu", () => PressKeyOnce(Key.P));
        AddAssert("current screen is play menu", () => TestGame.GetCurrentScreenType() == typeof(PlayMenuScreen));
        AddStep("go to song selection screen", () => PressKeyOnce(Key.P));
        AddAssert("current screen is song selection screen", () => TestGame.GetCurrentScreenType() == typeof(SongSelectionScreen));
    }
}

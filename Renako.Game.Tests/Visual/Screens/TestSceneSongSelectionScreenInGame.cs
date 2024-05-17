using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Platform;
using osuTK.Input;
using Renako.Game.Beatmaps;
using Renako.Game.Graphics.Screens;

namespace Renako.Game.Tests.Visual.Screens;

public partial class TestSceneSongSelectionScreenInGame : ManualInputTestScene
{
    [Resolved]
    private BeatmapsCollection beatmapsCollection { get; set; }

    [Resolved]
    private GameHost host { get; set; }

    [BackgroundDependencyLoader]
    private void load(GameHost host)
    {
        beatmapsCollection.GenerateTestCollection();
    }

    [Test]
    public void TestSongSelectionScreenAccessFromGame()
    {
        RenakoGame game = new RenakoGame();
        game.SetHost(host);
        AddGame(game);
        AddUntilStep("wait for game to load", () => game.IsLoaded);
        AddUntilStep("wait until start screen", () => game.GetCurrentScreenType() == typeof(StartScreen));
        AddStep("go to main menu", () => PressKeyOnce(Key.Enter));
        AddAssert("current screen is main menu", () => game.GetCurrentScreenType() == typeof(MainMenuScreen));
        AddStep("go to play menu", () => PressKeyOnce(Key.P));
        AddAssert("current screen is play menu", () => game.GetCurrentScreenType() == typeof(PlayMenuScreen));
        AddStep("go to song selection screen", () => PressKeyOnce(Key.P));
        AddAssert("current screen is song selection screen", () => game.GetCurrentScreenType() == typeof(SongSelectionScreen));
    }
}

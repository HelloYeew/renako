using osu.Framework.Allocation;
using osu.Framework.Platform;
using osu.Framework.Testing;
using Renako.Game.Graphics.Screens;

namespace Renako.Game.Tests.Visual;

public partial class GameInputTestScene : RenakoTestScene
{
    protected RenakoGame game;

    private GameHost host;

    [BackgroundDependencyLoader]
    private void load(GameHost host)
    {
        this.host = host;
    }

    [SetUpSteps]
    public void SetUpSteps()
    {
        AddStep("add game instance", () =>
        {
            Clear();
            game = new RenakoGame();
            game.SetHost(host);
            AddGame(game);
        });
        AddAssert("game loaded", () => game.IsLoaded);
        AddUntilStep("wait until start screen", () => game.MainScreenStack.CurrentScreen is StartScreen);
    }
}

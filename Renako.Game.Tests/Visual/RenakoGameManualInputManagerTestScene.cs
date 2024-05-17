using osu.Framework.Allocation;
using osu.Framework.Platform;
using Renako.Game.Beatmaps;

namespace Renako.Game.Tests.Visual;

public partial class RenakoGameManualInputManagerTestScene : RenakoManualInputManagerTestScene
{
    [Resolved]
    private BeatmapsCollection beatmapsCollection { get; set; }

    public RenakoGame TestGame = new RenakoGame();

    public void AddTestBeatmapSet()
    {
        beatmapsCollection.GenerateTestCollection();
    }

    [BackgroundDependencyLoader]
    private void load(GameHost host)
    {
        TestGame.SetHost(host);
        AddGame(TestGame);
    }
}

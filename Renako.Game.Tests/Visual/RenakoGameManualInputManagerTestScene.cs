using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Platform;
using Renako.Game.Beatmaps;

namespace Renako.Game.Tests.Visual;

public partial class RenakoGameManualInputManagerTestScene : RenakoManualInputManagerTestScene
{
    [Resolved]
    private BeatmapsCollection beatmapsCollection { get; set; }

    [Resolved]
    private GameHost testGameHost { get; set; }

    public RenakoGame TestGame = new RenakoGame();

    public void AddTestBeatmapSet()
    {
        beatmapsCollection.GenerateTestCollection();
    }

    [SetUp]
    public new void SetUp()
    {
        TestGame.SetHost(testGameHost);
        AddGame(TestGame);
    }
}

using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using Renako.Game.Beatmaps;
using Renako.Game.Configurations;
using Renako.Game.Graphics.Drawables;

namespace Renako.Game.Tests.Visual.Drawables;

[TestFixture]
public partial class TestSceneCurrentTrackText : RenakoGameDrawableTestScene
{
    [Resolved]
    private RenakoConfigManager configManager { get; set; }

    [Resolved]
    private WorkingBeatmap workingBeatmap { get; set; }

    [Resolved]
    private BeatmapManager beatmapManager { get; set; }

    [Cached]
    private BeatmapsCollection beatmapsCollection = new BeatmapsCollection();

    [BackgroundDependencyLoader]
    private void load()
    {
        beatmapsCollection.GenerateTestCollection();
        AddStep("set current working beatmap", () =>
        {
            workingBeatmap.BeatmapSet = beatmapsCollection.BeatmapSets[0];
            workingBeatmap.Beatmap = beatmapsCollection.GetFirstBeatmapFromBeatmapSet(workingBeatmap.BeatmapSet);
        });
        Add(new CurrentTrackText
        {
            Anchor = Anchor.TopRight,
            Origin = Anchor.TopRight,
            Margin = new MarginPadding(20)
        });
    }

    [Test]
    public void TestChangeBeatmapSet()
    {
        AddStep("set next beatmap set", () => beatmapManager.NextBeatmapSet(true));
    }

    [Test]
    public void TestUseUnicodeSetting()
    {
        AddStep("toggle unicode setting", () => configManager.SetValue(RenakoSetting.UseUnicodeInfo, !configManager.Get<bool>(RenakoSetting.UseUnicodeInfo)));
    }
}

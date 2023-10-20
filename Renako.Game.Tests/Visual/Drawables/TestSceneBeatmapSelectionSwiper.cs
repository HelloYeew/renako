using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;
using Renako.Game.Beatmaps;
using Renako.Game.Graphics;
using Renako.Game.Graphics.Drawables;
using Renako.Game.Utilities;

namespace Renako.Game.Tests.Visual.Drawables;

[TestFixture]
public partial class TestSceneBeatmapSelectionSwiper : GameDrawableTestScene
{
    [Resolved]
    private WorkingBeatmap workingBeatmap { get; set; }

    private BeatmapTestUtility beatmapTestUtility = new BeatmapTestUtility();
    private BeatmapSelectionSwiper swiper;
    private SpriteText indexText;

    [BackgroundDependencyLoader]
    private void load(TextureStore textureStore)
    {
        workingBeatmap.BeatmapSet = beatmapTestUtility.BeatmapSets[0];
        Add(indexText = new RenakoSpriteText()
        {
            Anchor = Anchor.TopLeft,
            Origin = Anchor.TopLeft,
            Font = RenakoFont.GetFont(),
            Text = "Index: 0"
        });
        Add(swiper = new BeatmapSelectionSwiper()
        {
            Position = new Vector2(0, -115)
        });
        swiper.BeatmapList = beatmapTestUtility.Beatmaps.FindAll(e => e.BeatmapSet.ID == workingBeatmap.BeatmapSet.ID);
        swiper.SetTexture(textureStore.Get(workingBeatmap.BeatmapSet.CoverPath));
        swiper.BindIndexChangeAction((index) => indexText.Text = $"Index: {index.NewValue}");
    }

    [Test]
    public void TestBasicSwiperHandling()
    {
        AddStep("toggle next", () => swiper.Next());
        AddStep("toggle previous", () => swiper.Previous());
    }

    [Test]
    public void TestSwiperItemHandling()
    {
        AddStep("scroll to first", () => swiper.SetIndex(0));
        AddAssert("index is 0", () => swiper.CurrentIndex == 0);
        AddStep("swipe to next", () => swiper.Next());
        AddAssert("index is 1", () => swiper.CurrentIndex == 1);
        AddStep("swipe to previous", () => swiper.Previous());
        AddAssert("index is 0", () => swiper.CurrentIndex == 0);
        AddRepeatStep("swipe to last", () => swiper.Next(), swiper.BeatmapList.Count - 1);
        AddAssert($"index is {swiper.BeatmapList.Count - 1}", () => swiper.CurrentIndex == swiper.BeatmapList.Count - 1);
        AddStep("swipe next again", () => swiper.Next());
    }
}

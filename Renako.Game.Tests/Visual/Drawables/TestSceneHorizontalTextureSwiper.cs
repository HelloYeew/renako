using System.Collections.Generic;
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
public partial class TestSceneHorizontalTextureSwiper : RenakoTestScene
{
    private List<TextureSwiperItem<BeatmapSet>> swiperItemList = new List<TextureSwiperItem<BeatmapSet>>();
    private HorizontalTextureSwiper<BeatmapSet> swiper;
    private SpriteText indexText;

    private BeatmapTestUtility beatmapTestUtility = new BeatmapTestUtility();

    [BackgroundDependencyLoader]
    private void load(TextureStore textureStore)
    {
        foreach (BeatmapSet beatmapSet in beatmapTestUtility.BeatmapSets)
        {
            swiperItemList.Add(new TextureSwiperItem<BeatmapSet>()
            {
                Item = beatmapSet,
                Texture = textureStore.Get(beatmapSet.CoverPath)
            });
        }

        Add(swiper = new HorizontalTextureSwiper<BeatmapSet>
        {
            Items = swiperItemList,
            Position = new Vector2(0, -115)
        });
        Add(indexText = new SpriteText()
        {
            Anchor = Anchor.TopLeft,
            Origin = Anchor.TopLeft,
            Font = RenakoFont.GetFont(),
            Text = "Index: 0"
        });
        swiper.BindIndexChangeAction((index) => indexText.Text = $"Index: {index.NewValue}");
        AddAssert("Index is 0", () => swiper.CurrentIndex == 0);
        AddStep("Swipe to next", () => swiper.Next());
        AddAssert("Index is 1", () => swiper.CurrentIndex == 1);
        AddStep("Swipe to previous", () => swiper.Previous());
        AddAssert("Index is 0", () => swiper.CurrentIndex == 0);
        AddRepeatStep("Swipe to last", () => swiper.Next(), beatmapTestUtility.BeatmapSets.Count - 1);
        AddAssert($"Index is {beatmapTestUtility.BeatmapSets.Count - 1}", () => swiper.CurrentIndex == beatmapTestUtility.BeatmapSets.Count - 1);
        AddStep("Swipe next again", () => swiper.Next());
    }

    [Test]
    public void TestBasicSwiperHandling()
    {
        AddStep("Toggle next", () => swiper.Next());
        AddStep("Toggle previous", () => swiper.Previous());
    }
}

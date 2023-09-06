using System.Collections.Generic;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using Renako.Game.Graphics;
using Renako.Game.Graphics.Drawables;

namespace Renako.Game.Tests.Visual.Drawables;

[TestFixture]
public partial class TestSceneHorizontalTextureSwiper : RenakoTestScene
{
    private List<TextureSwiperItem<int>> swiperItemList = new List<TextureSwiperItem<int>>();
    private HorizontalTextureSwiper<int> swiper;
    private SpriteText indexText;

    [BackgroundDependencyLoader]
    private void load(TextureStore textureStore)
    {
        for (int i = 0; i < 10; i++)
        {
            swiperItemList.Add(new TextureSwiperItem<int>()
            {
                Item = i,
                Texture = textureStore.Get("Beatmaps/Album/innocence-tv-size.jpg")
            });
        }

        Add(swiper = new HorizontalTextureSwiper<int>()
        {
            Items = swiperItemList
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
        AddRepeatStep("Swipe to last", () => swiper.Next(), 9);
        AddAssert("Index is 9", () => swiper.CurrentIndex == 9);
        AddStep("Swipe next again", () => swiper.Next());
        AddAssert("Index is back to 0", () => swiper.CurrentIndex == 0);

        // TODO: Add more tests
    }
}

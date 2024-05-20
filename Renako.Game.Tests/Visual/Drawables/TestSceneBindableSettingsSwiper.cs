using System.Collections.Generic;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osuTK;
using Renako.Game.Configurations;
using Renako.Game.Graphics;
using Renako.Game.Graphics.Drawables;

namespace Renako.Game.Tests.Visual.Drawables;

[TestFixture]
public partial class TestSceneBindableSettingsSwiper : GameDrawableTestScene
{
    [Resolved]
    private RenakoConfigManager configManager { get; set; }

    private BindableSettingsSwiper swiper;
    private SpriteText indexText;

    private readonly BindableFloat testFloat1 = new BindableFloat(0);
    private readonly BindableFloat testFloat2 = new BindableFloat(0);
    private readonly BindableFloat testFloat3 = new BindableFloat(0);
    private readonly BindableFloat testFloat4 = new BindableFloat(0);

    [BackgroundDependencyLoader]
    private void load()
    {
        Add(swiper = new BindableSettingsSwiper()
        {
            Position = new Vector2(0, -115)
        });
        Add(indexText = new RenakoSpriteText()
        {
            Anchor = Anchor.TopLeft,
            Origin = Anchor.TopLeft,
            Font = RenakoFont.GetFont(),
            Text = "Index: 0"
        });
        swiper.BindIndexChangeAction(index => indexText.Text = $"Index: {index.NewValue}");
        swiper.Items = new List<BindableSettingsSwiperItem>()
        {
            new BindableSettingsSwiperItem("Test 1", testFloat1),
            new BindableSettingsSwiperItem("Test 2", testFloat2),
            new BindableSettingsSwiperItem("Test 3", testFloat3),
            new BindableSettingsSwiperItem("Test 4", testFloat4)
        };
        swiper.Items[1].IncrementStep = 1.5f;
    }

    [Test]
    public void TestBasicSwiperHandling()
    {
        AddStep("toggle next", () => swiper.Next());
        AddStep("toggle previous", () => swiper.Previous());
    }

    [Test]
    public void TestChangeBindableValues()
    {
        AddStep("increment test 1", () => testFloat1.Value++);
        AddAssert("test 1 value is 1", () => swiper.Items[0].BindableFloat.Value == 1);
        AddStep("increment test 2 using step", () => swiper.Items[1].Increment());
        AddAssert("test 2 value is 1", () => swiper.Items[1].BindableFloat.Value == 1.5f);
        AddStep("decrement test 3", () => testFloat3.Value--);
        AddAssert("test 3 value is -1", () => swiper.Items[2].BindableFloat.Value == -1);
        AddStep("decrement test 4 using step", () => swiper.Items[3].Decrement());
        AddAssert("test 4 value is -1", () => swiper.Items[3].BindableFloat.Value == -0.1f);
    }
}

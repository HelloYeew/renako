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
public partial class TestSceneBindableSettingsSwiper : RenakoGameDrawableTestScene
{
    [Resolved]
    private RenakoConfigManager configManager { get; set; }

    private BindableSettingsSwiper swiper;
    private SpriteText indexText;

    private readonly Bindable<int> testInt1 = new Bindable<int>();
    private readonly Bindable<int> testInt2 = new Bindable<int>();
    private readonly Bindable<int> testInt3 = new Bindable<int>();
    private readonly Bindable<int> testInt4 = new Bindable<int>();

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
            new BindableSettingsSwiperItem("Test 1", testInt1),
            new BindableSettingsSwiperItem("Test 2", testInt2),
            new BindableSettingsSwiperItem("Test 3", testInt3),
            new BindableSettingsSwiperItem("Test 4", testInt4)
        };
        swiper.Items[1].IncrementStep = 3;
        swiper.Items[3].IncrementStep = 3;
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
        AddStep("increment test 1", () => testInt1.Value++);
        AddAssert("test 1 value is 1", () => swiper.Items[0].BindableInt.Value == 1);
        AddStep("increment test 2 using step", () => swiper.Items[1].Increment());
        AddAssert("test 2 value is 1", () => swiper.Items[1].BindableInt.Value == 3);
        AddStep("decrement test 3", () => testInt3.Value--);
        AddAssert("test 3 value is -1", () => swiper.Items[2].BindableInt.Value == -1);
        AddStep("decrement test 4 using step", () => swiper.Items[3].Decrement());
        AddAssert("test 4 value is -1", () => swiper.Items[3].BindableInt.Value == -3);
    }
}

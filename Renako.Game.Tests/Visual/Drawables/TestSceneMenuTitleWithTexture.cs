using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Textures;
using Renako.Game.Graphics.UserInterface;

namespace Renako.Game.Tests.Visual.Drawables;

[TestFixture]
public partial class TestSceneMenuTitleWithTexture : GameDrawableTestScene
{
    private MenuTitleWithTexture menuTitleWithTexture;

    [BackgroundDependencyLoader]
    private void load(TextureStore textures)
    {
        Add(menuTitleWithTexture = new MenuTitleWithTexture()
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre
        });
        menuTitleWithTexture.Texture = textures.Get("Beatmaps/Album/courage.jpg");
    }

    [Test]
    public void TestMenuTitleWithTexture()
    {
        AddStep("show texture", () => menuTitleWithTexture.ShowTexture());
        AddStep("hide texture", () => menuTitleWithTexture.HideTexture());
    }
}

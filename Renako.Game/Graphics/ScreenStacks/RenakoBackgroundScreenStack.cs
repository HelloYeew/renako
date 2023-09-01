using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Screens;

namespace Renako.Game.Graphics.ScreenStacks;

public partial class RenakoBackgroundScreenStack : ScreenStack
{
    public Sprite ImageSprite;

    [BackgroundDependencyLoader]
    private void load(TextureStore textureStore)
    {
        AddInternal(ImageSprite = new Sprite()
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            RelativeSizeAxes = Axes.Both,
            FillMode = FillMode.Fill,
            Alpha = 0,
            Texture = textureStore.Get("main-background.jpeg")
        });
    }
}

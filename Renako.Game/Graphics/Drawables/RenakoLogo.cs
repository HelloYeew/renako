using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace Renako.Game.Graphics.Drawables;

public partial class RenakoLogo : CompositeDrawable
{
    [BackgroundDependencyLoader]
    private void load(TextureStore textureStore)
    {
        Size = new Vector2(259, 84);
        InternalChild = new FillFlowContainer()
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Direction = FillDirection.Horizontal,
            Spacing = new Vector2(16),
            Children = new Drawable[]
            {
                new Sprite()
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(84),
                    Texture = textureStore.Get("Logo/renako-logo-white-pink")
                },
                new SpriteText()
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Font = RenakoFont.GetFont(RenakoFont.Typeface.Offside, 48),
                    Colour = Color4Extensions.FromHex("F0C3DB"),
                    Size = new Vector2(159, 53),
                    Text = "Renako"
                }
            }
        };
    }
}

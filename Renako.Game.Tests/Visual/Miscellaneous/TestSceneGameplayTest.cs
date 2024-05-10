using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using Renako.Game.Graphics;

namespace Renako.Game.Tests.Visual.Miscellaneous;

public partial class TestSceneGameplayTest : GameDrawableTestScene
{
    [BackgroundDependencyLoader]
    private void load(TextureStore textureStore)
    {
        BackgroundScreenStack.ChangeBackground(textureStore.Get("Screen/fallback-beatmap-background.jpg"));
        BackgroundScreenStack.AdjustMaskAlpha(0.5f);

        Add(new FillFlowContainer()
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            RelativeSizeAxes = Axes.Both,
            Direction = FillDirection.Horizontal,
            Spacing = new osuTK.Vector2(125, 0),
            Children = new Drawable[]
            {
                new Box()
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Width = 10,
                    Height = 0.6f,
                    RelativeSizeAxes = Axes.Y,
                    Colour = Color4Extensions.FromHex("E8DEEE")
                },
                new Box()
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Width = 10,
                    Height = 0.6f,
                    RelativeSizeAxes = Axes.Y,
                    Colour = Color4Extensions.FromHex("E8DEEE")
                },
                new Box()
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Width = 10,
                    Height = 0.6f,
                    RelativeSizeAxes = Axes.Y,
                    Colour = Color4Extensions.FromHex("E8DEEE")
                },
                new Box()
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Width = 10,
                    Height = 0.6f,
                    RelativeSizeAxes = Axes.Y,
                    Colour = Color4Extensions.FromHex("E8DEEE")
                }
            }
        });
    }
}

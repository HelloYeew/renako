using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Input;

namespace Renako.Game.Tests.Visual.Miscellaneous;

public partial class TestSceneGameplayTest : GameDrawableTestScene
{
    private Container playfield;
    private Circle player;

    [BackgroundDependencyLoader]
    private void load(TextureStore textureStore)
    {
        BackgroundScreenStack.ChangeBackground(textureStore.Get("Screen/fallback-beatmap-background.jpg"));
        BackgroundScreenStack.AdjustMaskAlpha(0.5f);

        Add(playfield = new Container()
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Size = new Vector2(800, 500),
            Children = new Drawable[]
            {
                new FillFlowContainer()
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Direction = FillDirection.Horizontal,
                    Spacing = new Vector2(75, 0),
                    Children = new Drawable[]
                    {
                        createLane(),
                        createLane(),
                        createLane(),
                        createLane()
                    }
                },
                new Box()
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Colour = Color4Extensions.FromHex("E8DEEE").Opacity(0.5f),
                    Size = new Vector2(800, 20),
                    Position = new Vector2(0, 200)
                },
                player = new Circle()
                {
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,
                    Size = new Vector2(20),
                    Colour = Color4Extensions.FromHex("DAB4E7"),
                    Position = new Vector2(125, 0)
                }
            }
        });
    }

    protected override bool OnKeyDown(KeyDownEvent e)
    {
        switch (e.Key)
        {
            case Key.D:
                player.MoveTo(new Vector2(-125, 0), 100, Easing.Out);
                break;

            case Key.F:
                player.MoveTo(new Vector2(-40, 0), 100, Easing.Out);
                break;

            case Key.J:
                player.MoveTo(new Vector2(40, 0), 100, Easing.Out);
                break;

            case Key.K:
                player.MoveTo(new Vector2(125, 0), 100, Easing.Out);
                break;
        }

        return base.OnKeyDown(e);
    }

    private Container createLane()
    {
        return new Container()
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Width = 10,
            Height = 400,
            Children = new Drawable[]
            {
                new Box()
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Width = 10,
                    Height = 1,
                    RelativeSizeAxes = Axes.Y,
                    Colour = Color4Extensions.FromHex("E8DEEE")
                },
                new Circle()
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Width = 20,
                    Height = 20,
                    Position = new Vector2(0, -0.5f),
                    RelativePositionAxes = Axes.Y,
                    Colour = Color4Extensions.FromHex("E8DEEE")
                }
            }
        };
    }
}

using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
using osu.Framework.Utils;
using osuTK;
using osuTK.Input;

namespace Renako.Game.Tests.Visual.Miscellaneous;

public partial class TestSceneGameplayTest : GameDrawableTestScene
{
    private Container playfield;
    private Container drawablePlayfield;
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

        Add(drawablePlayfield = new Container()
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Size = new Vector2(800, 500)
        });

        AddStep("create note on lane 1", () => addNote(Lane.Lane1));
        AddStep("create note on lane 2", () => addNote(Lane.Lane2));
        AddStep("create note on lane 3", () => addNote(Lane.Lane3));
        AddStep("create note on lane 4", () => addNote(Lane.Lane4));
        AddStep("create random note", () => addNote((Lane)RNG.Next(0, 4)));
        AddSliderStep("scale", 0, 1, 1f, scale =>
        {
            playfield.Scale = new Vector2(scale);
            drawablePlayfield.Scale = new Vector2(scale);
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

    private void addNote(Lane lane)
    {
        Circle note;
        float x = getLaneX(lane);

        drawablePlayfield.Add(note = new Circle()
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Size = new Vector2(50),
            Colour = Color4Extensions.FromHex("D9D9D9"),
            Position = new Vector2(x, -200),
            Scale = Vector2.Zero,
            Alpha = 0
        });

        note.ScaleTo(new Vector2(1), 500, Easing.Out);
        note.FadeIn(500)
            .Then()
            .MoveTo(new Vector2(x, 200), 750)
            .Then()
            .MoveTo(new Vector2(x, 400), 250)
            .FadeOut(250);
    }

    private enum Lane
    {
        Lane1,
        Lane2,
        Lane3,
        Lane4
    }

    private float getLaneX(Lane lane)
    {
        return lane switch
        {
            Lane.Lane1 => -127.5f,
            Lane.Lane2 => -42.5f,
            Lane.Lane3 => 42.5f,
            Lane.Lane4 => 127.5f,
            _ => 0
        };
    }
}

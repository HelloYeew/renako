using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Input;

namespace Renako.Game.Tests.Visual.Miscellaneous;


[TestFixture]
[Ignore("This is just for testing purposes.")]
public partial class TestSceneGameplayNormalPhaseProofOfConcept : RenakoTestScene
{
    private Circle character;

    public TestSceneGameplayNormalPhaseProofOfConcept()
    {
        Add(new Container()
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            RelativeSizeAxes = Axes.Both,
            Size = new Vector2(0.8f),
            Children = new Drawable[]
            {
                new Box()
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Colour = Colour4.White,
                    Alpha = 0.1f
                },
                new Container()
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    Size = new Vector2(0.8f, 0.7f),
                    Children = new Drawable[]
                    {
                        new Box()
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            RelativeSizeAxes = Axes.Both,
                            Colour = Colour4.White,
                            Alpha = 0.25f
                        },
                        character = new Circle()
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Size = new Vector2(40),
                            Position = new Vector2(0, 200),
                            Colour = Colour4.Cyan
                        }
                    }
                },
                new Circle()
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Size = new Vector2(80),
                    Position = new Vector2(0, 20)
                }
            }
        });
    }

    protected override bool OnKeyDown(KeyDownEvent e)
    {
        if (e.Key == Key.Left || e.Key == Key.Right)
        {
            character.MoveToX(character.X + (e.Key == Key.Left ? -20 : 20), 100, Easing.OutQuint);
        }

        return base.OnKeyDown(e);
    }
}

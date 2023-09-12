using System.Collections.Generic;
using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Framework.Utils;
using osuTK;
using osuTK.Input;

namespace Renako.Game.Tests.Visual.Miscellaneous;


[TestFixture]
[Ignore("This is just for testing purposes.")]
public partial class TestSceneGameplayNormalPhaseProofOfConcept : RenakoTestScene
{
    private Circle character;
    private Triangle note;
    private List<Triangle> notes = new List<Triangle>();

    private bool followMouse = false;

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

        // Randomly drop note from top to bottom.
        Scheduler.AddDelayed(() =>
        {
            Triangle note;
            Add(note = new Triangle()
            {
                Anchor = Anchor.TopCentre,
                Origin = Anchor.TopCentre,
                Size = new Vector2(40),
                Position = new Vector2(RNG.Next(-300, 300), -200),
                Colour = Colour4.Red
            });
            note.MoveToY(2000, 5000).Expire();
        }, 500, true);

        AddToggleStep("Follow mouse", b =>
        {
            followMouse = b;
        });
    }

    protected override bool OnKeyDown(KeyDownEvent e)
    {
        if (e.Key == Key.Left || e.Key == Key.Right)
        {
            character.MoveToX(character.X + (e.Key == Key.Left ? -30 : 30), 50, Easing.OutQuint);
        }

        return base.OnKeyDown(e);
    }

    protected override void Update()
    {
        // Remove expired notes.
        foreach (var note in notes)
        {
            if (note.IsAlive)
                continue;

            notes.Remove(note);
        }

        base.Update();
    }

    protected override bool OnMouseMove(MouseMoveEvent e)
    {
        if (followMouse)
        {
            character.MoveToX(e.MousePosition.X - 200, 50, Easing.OutQuint);
        }

        return base.OnMouseMove(e);
    }
}

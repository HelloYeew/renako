using System.Collections.Generic;
using NUnit.Framework;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Utils;
using osuTK;
using osuTK.Input;
using Renako.Game.Graphics;
using Box = osu.Framework.Graphics.Shapes.Box;

namespace Renako.Game.Tests.Visual.Miscellaneous;

[TestFixture]
[Ignore("This is just for testing purposes.")]
public partial class TestSceneGameplayTest : RenakoTestScene
{
    private Container character;
    private Circle boss;
    private Box playfield;
    private Circle fruit;
    private KeyDownEvent lastKeyDownEvent;
    private Circle bullet;

    private int score;
    private SpriteText scoreText;
    private SpriteText characterPositionText;
    private SpriteText fruitPositionText;
    private SpriteText canGetScoreText;

    private List<Key> controlKey = new List<Key>()
    {
        Key.W,
        Key.A,
        Key.S,
        Key.D,
        Key.Left,
        Key.Right,
        Key.Down,
        Key.Up
    };

    private readonly int distancePerKey = 20;
    private readonly int hitWindow = 50;

    public TestSceneGameplayTest()
    {
        character = new Container()
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Size = new Vector2(40, 40),
            Position = new Vector2(0, -120),
            Children = new Drawable[]
            {
                // Arrow for rotation of character.
                new Triangle()
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    Size = new Vector2(0.25f, 0.25f),
                    Colour = Colour4.Aqua,
                    Position = new Vector2(0, -0.5f)
                },
                // Character body.
                new Circle()
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.5f, 0.5f),
                    Colour = Colour4.Aqua
                }
            }
        };

        Add(playfield = new Box()
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            RelativeSizeAxes = Axes.Both,
            Colour = Color4Extensions.FromHex("D9D9D9"),
            Size = new Vector2(0.8f, 0.65f),
            Position = new Vector2(0, 40),
            Alpha = 0.25f
        });
        Add(boss = new Circle()
        {
            Anchor = Anchor.TopCentre,
            Origin = Anchor.TopCentre,
            Size = new Vector2(80),
            Position = new Vector2(0, 50),
        });
        Add(character);
        Add(scoreText = new SpriteText()
        {
            Anchor = Anchor.TopRight,
            Origin = Anchor.TopRight,
            Text = "Score: 0",
            Font = RenakoFont.GetFont()
        });
        Add(characterPositionText = new SpriteText()
        {
            Anchor = Anchor.TopRight,
            Origin = Anchor.TopRight,
            Text = "Character Position: 0, 0",
            Font = RenakoFont.GetFont(),
            Position = new Vector2(0, 20)
        });
        Add(fruitPositionText = new SpriteText()
        {
            Anchor = Anchor.TopRight,
            Origin = Anchor.TopRight,
            Text = "Fruit Position: 0, 0",
            Font = RenakoFont.GetFont(),
            Position = new Vector2(0, 40)
        });
        Add(canGetScoreText = new SpriteText()
        {
            Anchor = Anchor.TopRight,
            Origin = Anchor.TopRight,
            Text = "Can get score: false",
            Font = RenakoFont.GetFont(),
            Position = new Vector2(0, 60)
        });

        // randomly generate fruit in playfield in every 2 seconds in separate thread.
        Scheduler.AddDelayed(() =>
        {
            Add(fruit = new Circle()
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(20),
                Position = new Vector2(RNG.NextSingle(-400, 400), RNG.NextSingle(-200, 200)),
                Colour = Color4Extensions.FromHex("FF0000")
            });
            fruitPositionText.Text = $"Fruit Position: {fruit.Position.X}, {fruit.Position.Y}";
            fruit.FadeTo(0, 5000).Expire();
        }, 5000, true);

        // TODO: We need more boolets
    }

    protected override bool OnKeyDown(KeyDownEvent e)
    {
        if (controlKey.Contains(e.Key))
        {
            if (lastKeyDownEvent != null)
            {
                if (e.Key == Key.W || e.Key == Key.Up)
                {
                    if (lastKeyDownEvent.Key == Key.W || lastKeyDownEvent.Key == Key.Up)
                    {
                        character.Y -= distancePerKey;
                    }
                    else
                    {
                        character.Rotation = 0;
                    }
                }
                else if (e.Key == Key.S || e.Key == Key.Down)
                {
                    if (lastKeyDownEvent.Key == Key.S || lastKeyDownEvent.Key == Key.Down)
                    {
                        character.Y += distancePerKey;
                    }
                    else
                    {
                        character.Rotation = 180;
                    }
                }
                else if (e.Key == Key.A || e.Key == Key.Left)
                {
                    if (lastKeyDownEvent.Key == Key.A || lastKeyDownEvent.Key == Key.Left)
                    {
                        character.X -= distancePerKey;
                    }
                    else
                    {
                        character.Rotation = 270;
                    }
                }
                else if (e.Key == Key.D || e.Key == Key.Right)
                {
                    if (lastKeyDownEvent.Key == Key.D || lastKeyDownEvent.Key == Key.Right)
                    {
                        character.X += distancePerKey;
                    }
                    else
                    {
                        character.Rotation = 90;
                    }
                }
            }

            characterPositionText.Text = $"Character Position: {character.Position.X}, {character.Position.Y}";

            lastKeyDownEvent = e;
        }

        if (e.Key == Key.Enter)
        {
            // Check if in front of character is fruit.
            if (character.Position.X - fruit.Position.X <= hitWindow && character.Position.X - fruit.Position.X >= -hitWindow && character.Position.Y - fruit.Position.Y <= hitWindow && character.Position.Y - fruit.Position.Y >= -hitWindow)
            {
                if (fruit.IsAlive)
                    score++;
                scoreText.Text = $"Score: {score}";
                fruit.ClearTransforms();
            }
        }

        return base.OnKeyDown(e);
    }

    protected override void Update()
    {
        if (fruit != null)
        {
            if (character.Position.X - fruit.Position.X <= hitWindow && character.Position.X - fruit.Position.X >= -hitWindow && character.Position.Y - fruit.Position.Y <= hitWindow && character.Position.Y - fruit.Position.Y >= -hitWindow)
            {
                canGetScoreText.Text = "Can get score: true";
            }
            else
            {
                canGetScoreText.Text = "Can get score: false";
            }
        }

        base.Update();
    }
}

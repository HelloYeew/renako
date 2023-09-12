using System.Collections.Generic;
using System.Globalization;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osu.Framework.Timing;
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
    private SpriteText clockText;

    private StopwatchClock clock;

    private Track track;

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

    private List<double> fruitNoteTime = new List<double>()
    {
        2233.37,
        3608.80,
        4896.85,
        6245.69,
        7511.45,
        8785.51,
        10086.30,
        11386.62,
        23230.23,
        25850.09,
        28818.81,
        29466.33,
        31084.44,
        32240.49,
        33736.96,
        36257.68,
        39289.20,
        41551.06,
        44185.23,
        46162.40,
        46585.87,
        48809.80,
        50028.89,
        51066.81,
        51780.56,
        53327.07,
        55059.79,
        56513.02,
        57752.61,
        59016.78,
        60251.11,
        61615.46,
        62813.71,
        64145.21,
        65469.90,
        66938.52,
        68354.84,
        69558.45,
        70912.52,
        72124.32,
        3641.72,
        74983.59,
        76013.18,
        77643.21,
        79049.58,
        80820.57,
        82459.36,
        84037.55,
        85610.56
    };

    private readonly int distancePerKey = 20;
    private readonly int hitWindow = 50;

    [BackgroundDependencyLoader]
    private void load(ITrackStore trackStore)
    {
        track = trackStore.Get("beatmaps/innocence-tv-size.mp3");
        clock = new StopwatchClock();
    }

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
        Add(clockText = new SpriteText()
        {
            Anchor = Anchor.TopRight,
            Origin = Anchor.TopRight,
            Text = "Clock: 0",
            Font = RenakoFont.GetFont(),
            Position = new Vector2(0, 80)
        });

        // randomly generate fruit in playfield in every 2 seconds in separate thread.
        // Scheduler.AddDelayed(() =>
        // {
        //     Add(fruit = new Circle()
        //     {
        //         Anchor = Anchor.Centre,
        //         Origin = Anchor.Centre,
        //         Size = new Vector2(20),
        //         Position = new Vector2(RNG.NextSingle(-400, 400), RNG.NextSingle(-200, 200)),
        //         Colour = Color4Extensions.FromHex("FF0000")
        //     });
        //     fruitPositionText.Text = $"Fruit Position: {fruit.Position.X}, {fruit.Position.Y}";
        //     fruit.FadeTo(0, 5000).Expire();
        // }, 5000, true);

        AddStep("Toggle pause or play", () =>
        {
            if (clock.IsRunning)
                clock.Stop();
            else
                clock.Start();
            if (track.IsRunning)
                track.Stop();
            else
                track.Start();
        });

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

        if (e.Key == Key.BackSpace)
        {
            Logger.Log(clock.CurrentTime.ToString("F2", CultureInfo.InvariantCulture));
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

        clockText.Text = $"Clock: {clock.CurrentTime.ToString(CultureInfo.InvariantCulture)}";

        // If clock is running, check if there is any note need to spawn (spawn note 3 second before it should be hit).
        if (clock.IsRunning)
        {
            for (int i = 0; i < fruitNoteTime.Count; i++)
            {
                if (clock.CurrentTime >= fruitNoteTime[i] - 3000 && clock.CurrentTime <= fruitNoteTime[i] + 3000)
                {
                    Add(fruit = new Circle()
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Size = new Vector2(25),
                        Position = new Vector2(RNG.NextSingle(-400, 400), RNG.NextSingle(-200, 200)),
                        Colour = Color4Extensions.FromHex("FF0000")
                    });
                    fruitPositionText.Text = $"Fruit Position: {fruit.Position.X}, {fruit.Position.Y}";
                    fruit.FadeTo(0, 3000).Expire();
                    fruitNoteTime.RemoveAt(i);
                }
            }
        }

        base.Update();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Pooling;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
using osu.Framework.Timing;
using osu.Framework.Utils;
using osuTK;
using osuTK.Input;
using Renako.Game.Graphics;

namespace Renako.Game.Tests.Visual.Miscellaneous;

public partial class TestSceneGameplayTest : GameDrawableTestScene
{
    private Container playfield;
    private Container drawablePlayfield;
    private Circle player;
    private statistics stats = new statistics();
    private RenakoSpriteText scoreText;
    private RenakoSpriteText criticalText;
    private RenakoSpriteText breakText;
    private RenakoSpriteText hitText;
    private RenakoSpriteText missText;
    private RenakoSpriteText clockText;

    private DrawablePool<note> notePool;

    private const int fade_in_time = 500;
    private const int move_time = 750;

    private const int playerY = 75;

    private readonly StopwatchClock stopwatchClock = new StopwatchClock();

    private static BeatmapNote[] beatmapNotes;
    private static PlayfieldNote[] playfieldNotes;

    [BackgroundDependencyLoader]
    private void load(TextureStore textureStore)
    {
        Add(notePool = new DrawablePool<note>(50));

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
                    Size = new Vector2(60),
                    Colour = Color4Extensions.FromHex("DAB4E7"),
                    Position = new Vector2(125, playerY)
                }
            }
        });

        Add(drawablePlayfield = new Container()
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Size = new Vector2(800, 500),
            Name = "drawablePlayfield"
        });

        Add(new FillFlowContainer()
        {
            Anchor = Anchor.TopRight,
            Origin = Anchor.TopRight,
            Size = new Vector2(100, 0),
            AutoSizeAxes = Axes.Y,
            Direction = FillDirection.Vertical,
            Spacing = new Vector2(0, 10),
            Margin = new MarginPadding(10),
            Children = new Drawable[]
            {
                scoreText = new RenakoSpriteText()
                {
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    Text = "Score: 0"
                },
                criticalText = new RenakoSpriteText()
                {
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    Text = "Critical: 0"
                },
                breakText = new RenakoSpriteText()
                {
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    Text = "Break: 0"
                },
                hitText = new RenakoSpriteText()
                {
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    Text = "Hit: 0"
                },
                missText = new RenakoSpriteText()
                {
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    Text = "Miss: 0"
                },
                clockText = new RenakoSpriteText()
                {
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    Text = "0"
                }
            }
        });

        AddSliderStep("scale", 0, 1, 1f, scale =>
        {
            playfield.Scale = new Vector2(scale);
            drawablePlayfield.Scale = new Vector2(scale);
        });

        List<BeatmapNote> beatmapNotesTemp = new List<BeatmapNote>();

        for (int i = 0; i < 25; i++)
        {
            beatmapNotesTemp.Add(new BeatmapNote
            {
                Lane = (Lane)RNG.Next(0, 4),
                Time = (i + 1) * 1000
            });
        }

        beatmapNotes = beatmapNotesTemp.ToArray();
        playfieldNotes = beatmapNotes.Select(PlayfieldNote.FromBeatmapNote).ToArray();
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();
        stopwatchClock.Start();
    }

    protected override bool OnKeyDown(KeyDownEvent e)
    {
        switch (e.Key)
        {
            case Key.D:
                player.MoveTo(new Vector2(getLaneX(Lane.Lane1), playerY), 100, Easing.Out);
                processHit(Lane.Lane1);
                break;

            case Key.F:
                player.MoveTo(new Vector2(getLaneX(Lane.Lane2), playerY), 100, Easing.Out);
                processHit(Lane.Lane2);
                break;

            case Key.J:
                player.MoveTo(new Vector2(getLaneX(Lane.Lane3), playerY), 100, Easing.Out);
                processHit(Lane.Lane3);
                break;

            case Key.K:
                player.MoveTo(new Vector2(getLaneX(Lane.Lane4), playerY), 100, Easing.Out);
                processHit(Lane.Lane4);
                break;
        }

        return base.OnKeyDown(e);
    }

    protected override void Update()
    {
        base.Update();

        foreach (var note in playfieldNotes)
        {
            // Count missed note
            if (!note.IsHit && stopwatchClock.CurrentTime - note.Time > 200)
            {
                note.IsHit = true;
                stats.Miss++;
                updateScoreText();
            }

            if (note.IsDrawn)
                continue;

            if (stopwatchClock.CurrentTime >= note.Time - move_time - fade_in_time)
            {
                addNote(note);
                note.IsDrawn = true;
            }
        }

        clockText.Text = stopwatchClock.CurrentTime.ToString("0.00");
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

    private void addNote(PlayfieldNote playfieldNote)
    {
        if (!notePool.IsLoaded)
            return;

        float x = getLaneX(playfieldNote.Lane);
        note n = notePool.Get(noteObject =>
        {
            noteObject.Position = new Vector2(x, -200);
            noteObject.LifetimeEnd = Clock.CurrentTime + 2000;
        });

        drawablePlayfield.Add(n);

        n.ScaleTo(new Vector2(1), fade_in_time, Easing.OutCubic)
         .FadeIn(500)
         .Then()
         .MoveTo(new Vector2(x, 200), move_time)
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

    private static float getLaneX(Lane lane)
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

    private class BeatmapNote
    {
        public Lane Lane { get; set; }
        public double Time { get; set; }
    }

    private class PlayfieldNote
    {
        public Lane Lane { get; set; }
        public double Time { get; set; }
        public bool IsDrawn { get; set; }

        public bool IsHit { get; set; }

        public static PlayfieldNote FromBeatmapNote(BeatmapNote beatmapNote) =>
            new PlayfieldNote { Lane = beatmapNote.Lane, Time = beatmapNote.Time };
    }

    private void processHit(Lane lane)
    {
        PlayfieldNote[] notes = playfieldNotes.Where(n => !n.IsHit && n.Lane == lane && Math.Abs(stopwatchClock.CurrentTime - n.Time) <= 200).ToArray();

        if (notes.Length == 0)
            return;

        foreach (var note in notes)
        {
            note.IsHit = true;

            double diff = Math.Abs(stopwatchClock.CurrentTime - note.Time);

            if (diff < 50)
                stats.Critical++;
            else if (diff < 100)
                stats.Break++;
            else if (diff < 200)
                stats.Hit++;
            else
                stats.Miss++;

            stats.Score = stats.Score + 1000 - diff;
            updateScoreText();
        }
    }

    private void updateScoreText()
    {
        scoreText.Text = $"Score: {stats.Score}";
        criticalText.Text = $"Critical: {stats.Critical}";
        breakText.Text = $"Break: {stats.Break}";
        hitText.Text = $"Hit: {stats.Hit}";
        missText.Text = $"Miss: {stats.Miss}";
    }

    private class statistics
    {
        public double Score { get; set; }

        public int Critical { get; set; } // Less than 50ms
        public int Break { get; set; } // More than 50ms but less than 100ms
        public int Hit { get; set; } // More than 100ms but less than 200ms
        public int Miss { get; set; } // More than 200ms
    }

    private partial class note : PoolableDrawable
    {
        public note()
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            Size = new Vector2(50);
            Position = new Vector2(0, -200);
            Scale = Vector2.Zero;
            Alpha = 0;
            InternalChild = new Circle()
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = Vector2.One,
                RelativeSizeAxes = Axes.Both,
                Colour = Color4Extensions.FromHex("D9D9D9")
            };
        }

        public PlayfieldNote PlayfieldNote { get; set; }
    }
}

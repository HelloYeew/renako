using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Pooling;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Textures;
using osu.Framework.Logging;
using osu.Framework.Timing;
using osuTK;
using Renako.Game.Beatmaps;

namespace Renako.Game.Graphics.Containers;

public partial class PlayfieldContainer : Container
{
    private DrawablePool<Note> notePool;
    private DrawablePool<Indicator> indicatorPool;
    private DrawablePool<DrawableHitResult> hitResultPool;

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

    private const int fade_in_time = move_time / 2;
    private const int move_time = 750; // TODO: Change by scroll speed

    private const int player_y = 50;

    private Container lane1;
    private Container lane2;
    private Container lane3;
    private Container lane4;

    private static PlayfieldNote[] playfieldNotes;

    private StopwatchClock stopwatchClock;

    public PlayfieldContainer(StopwatchClock playfieldClock)
    {
        stopwatchClock = playfieldClock;
    }

    [BackgroundDependencyLoader]
    private void load(TextureStore textureStore, WorkingBeatmap workingBeatmap)
    {
        Add(notePool = new DrawablePool<Note>(50));
        Add(indicatorPool = new DrawablePool<Indicator>(20));
        Add(hitResultPool = new DrawablePool<DrawableHitResult>(20));

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
                        lane1 = createLane(),
                        lane2 = createLane(),
                        lane3 = createLane(),
                        lane4 = createLane()
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
                    Position = new Vector2(125, player_y)
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

        playfieldNotes = workingBeatmap.Beatmap.Notes.Select(PlayfieldNote.FromBeatmapNote).ToArray();
    }

    protected override void Update()
    {
        base.Update();

        foreach (var note in playfieldNotes)
        {
            // Count missed note
            if (!note.IsHit && stopwatchClock.CurrentTime - note.EndTime > 200 + move_time)
            {
                Logger.Log($"Missed note at {note.EndTime}", LoggingTarget.Runtime, LogLevel.Debug);
                note.IsHit = true;
                stats.Miss++;
                addHitResultAnimation(HitResult.Miss);
                updateScoreText();
            }

            if (note.IsDrawn)
                continue;

            if (stopwatchClock.CurrentTime >= note.StartTime - move_time - fade_in_time)
            {
                Logger.Log($"Drawing note at {note.StartTime}", LoggingTarget.Runtime, LogLevel.Debug);
                note.DrawableNote = addNote(note);
                note.IsDrawn = true;
            }
        }

        clockText.Text = stopwatchClock.CurrentTime.ToString("0.00");
    }

    /// <summary>
    /// Play animation and process input from upper layer.
    /// </summary>
    /// <param name="lane"></param>
    internal void ReceiveLaneInput(NoteLane lane)
    {
        player.MoveTo(new Vector2(getLaneX(lane), player_y), 100, Easing.Out);
        processHit(lane);
        addHitAnimation(lane);
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

    private Note addNote(PlayfieldNote playfieldNote)
    {
        if (!notePool.IsLoaded)
            return null;

        float x = getLaneX(playfieldNote.Lane);
        Note n = notePool.Get(noteObject =>
        {
            noteObject.Position = new Vector2(x, -200);
            noteObject.LifetimeEnd = Clock.CurrentTime + fade_in_time * 2 + move_time + 250;
        });

        drawablePlayfield.Add(n);

        n.ClearTransforms();
        n.ScaleTo(new Vector2(1), fade_in_time, Easing.OutCubic)
         .FadeIn(fade_in_time)
         .Then()
         .MoveTo(new Vector2(x, 200), move_time)
         .Then()
         .MoveTo(new Vector2(x, 400), fade_in_time, Easing.OutCubic)
         .FadeOut(fade_in_time)
         .Then()
         .Delay(250);

        return n;
    }

    /// <summary>
    /// Add hit animation to the lane on every user click.
    /// </summary>
    /// <param name="lane">The lane to add hit animation.</param>
    internal void addHitAnimation(NoteLane lane)
    {
        if (!indicatorPool.IsLoaded)
            return;

        Indicator indicatorDrawable = indicatorPool.Get(indicatorObject =>
        {
            indicatorObject.Position = new Vector2(getLaneX(lane), 200);
            indicatorObject.LifetimeEnd = Clock.CurrentTime + 500;
            indicatorObject.Size = new Vector2(25);
        });

        drawablePlayfield.Add(indicatorDrawable);
        indicatorDrawable.FadeIn(50).Then().ResizeTo(new Vector2(75), 150, Easing.OutCubic).FadeOut(150);

        switch (lane)
        {
            case NoteLane.Lane1:
                lane1.FlashColour(Color4Extensions.FromHex("8D90D0"), 200, Easing.OutCubic);
                break;

            case NoteLane.Lane2:
                lane2.FlashColour(Color4Extensions.FromHex("8D90D0"), 200, Easing.OutCubic);
                break;

            case NoteLane.Lane3:
                lane3.FlashColour(Color4Extensions.FromHex("8D90D0"), 200, Easing.OutCubic);
                break;

            case NoteLane.Lane4:
                lane4.FlashColour(Color4Extensions.FromHex("8D90D0"), 200, Easing.OutCubic);
                break;
        }
    }

    private void addHitResultAnimation(HitResult result)
    {
        if (!hitResultPool.IsLoaded)
            return;

        DrawableHitResult drawableHitResult = hitResultPool.Get(hitResultObject =>
        {
            hitResultObject.Scale = new Vector2(0.5f);
            hitResultObject.LifetimeEnd = Clock.CurrentTime + 1000;

            switch (result)
            {
                case HitResult.Critical:
                    hitResultObject.SetCritical();
                    break;

                case HitResult.Break:
                    hitResultObject.SetBreak();
                    break;

                case HitResult.Hit:
                    hitResultObject.SetHit();
                    break;

                case HitResult.Miss:
                    hitResultObject.SetMiss();
                    break;
            }
        });

        drawableHitResult.FadeIn(50).Then().FadeOut(500, Easing.OutCirc);
        drawableHitResult.HitText.FadeIn(50).Then().ScaleTo(new Vector2(1.5f), 500, Easing.OutCirc).FadeOut(500, Easing.OutCirc);

        drawablePlayfield.Add(drawableHitResult);
    }

    private void processHit(NoteLane lane)
    {
        PlayfieldNote[] notes = playfieldNotes.Where(n => !n.IsHit && n.Lane == lane && Math.Abs(stopwatchClock.CurrentTime - n.EndTime) <= 200 + move_time).ToArray();

        if (notes.Length == 0)
            return;

        foreach (var note in notes)
        {
            note.IsHit = true;

            double diff = Math.Abs(stopwatchClock.CurrentTime - note.EndTime);

            bool playHitAnimation = true;

            if (diff < 50)
            {
                stats.Critical++;
                addHitResultAnimation(HitResult.Critical);
            }
            else if (diff < 100)
            {
                stats.Break++;
                addHitResultAnimation(HitResult.Break);
            }
            else if (diff < 200)
            {
                stats.Hit++;
                addHitResultAnimation(HitResult.Hit);
            }
            else
            {
                stats.Miss++;
                addHitResultAnimation(HitResult.Miss);
                playHitAnimation = false;
            }

            stats.Score = stats.Score + 1000 - diff;
            updateScoreText();

            if (playHitAnimation && note.DrawableNote != null)
            {
                note.DrawableNote.ClearTransforms();
                note.DrawableNote.FadeOut(fade_in_time, Easing.OutCubic);
            }
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

    private enum HitResult
    {
        Critical,
        Break,
        Hit,
        Miss
    }

    private static float getLaneX(NoteLane lane)
    {
        return lane switch
        {
            NoteLane.Lane1 => -127.5f,
            NoteLane.Lane2 => -42.5f,
            NoteLane.Lane3 => 42.5f,
            NoteLane.Lane4 => 127.5f,
            _ => 0
        };
    }

    private class statistics
    {
        public double Score { get; set; }

        public int Critical { get; set; } // Less than 50ms
        public int Break { get; set; } // More than 50ms but less than 100ms
        public int Hit { get; set; } // More than 100ms but less than 200ms
        public int Miss { get; set; } // More than 200ms
    }

    private class PlayfieldNote
    {
        public NoteLane Lane { get; set; }

        public Note DrawableNote { get; set; }

        public double StartTime { get; set; }
        public double EndTime { get; set; }
        public bool IsDrawn { get; set; }

        public bool IsHit { get; set; }

        public static PlayfieldNote FromBeatmapNote(BeatmapNote beatmapNote) =>
            new PlayfieldNote { Lane = beatmapNote.Lane, StartTime = beatmapNote.StartTime, EndTime = beatmapNote.EndTime };
    }

    private partial class Note : PoolableDrawable
    {
        public Note()
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
    }

    private partial class Indicator : PoolableDrawable
    {
        public Indicator()
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            Size = new Vector2(25);
            Alpha = 0;
            InternalChild = new Circle()
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = Vector2.One,
                RelativeSizeAxes = Axes.Both,
                Colour = Color4Extensions.FromHex("8D90D0")
            };
        }
    }

    private partial class DrawableHitResult : PoolableDrawable
    {
        internal RenakoSpriteText HitText;

        public DrawableHitResult()
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            Size = new Vector2(50);
            Position = new Vector2(0, -100);
            InternalChild = HitText = new RenakoSpriteText()
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Spacing = new Vector2(5),
                Font = RenakoFont.GetFont(RenakoFont.Typeface.JosefinSans, 75, RenakoFont.FontWeight.Bold)
            };
        }

        public void SetCritical()
        {
            HitText.Text = "Critical".ToUpper();
            HitText.Colour = Color4Extensions.FromHex("C8F1D4");
        }

        public void SetBreak()
        {
            HitText.Text = "Break".ToUpper();
            HitText.Colour = Color4Extensions.FromHex("FDE798");
        }

        public void SetHit()
        {
            HitText.Text = "Hit".ToUpper();
            HitText.Colour = Color4Extensions.FromHex("F0E0E0");
        }

        public void SetMiss()
        {
            HitText.Text = "Miss".ToUpper();
            HitText.Colour = Color4Extensions.FromHex("F6F3E6");
        }
    }
}

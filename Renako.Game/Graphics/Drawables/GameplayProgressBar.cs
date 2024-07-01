using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;

namespace Renako.Game.Graphics.Drawables;

public partial class GameplayProgressBar : CompositeDrawable
{
    private RenakoSpriteText currentTime;
    private RenakoSpriteText totalTime;
    private double totalTimeValue;
    private Container progressBar;

    [BackgroundDependencyLoader]
    private void load()
    {
        Anchor = Anchor.BottomCentre;
        Origin = Anchor.BottomCentre;
        RelativeSizeAxes = Axes.X;
        Width = 0.85f;
        Height = 15;
        Y = -20;
        InternalChildren = new Drawable[]
        {
            new Container()
            {
                Anchor = Anchor.BottomCentre,
                Origin = Anchor.BottomCentre,
                RelativeSizeAxes = Axes.Both,
                Width = 0.9f,
                Children = new Drawable[]
                {
                    new Container()
                    {
                        Masking = true,
                        CornerRadius = 10,
                        RelativeSizeAxes = Axes.Both,
                        Children = new Drawable[]
                        {
                            new Box
                            {
                                RelativeSizeAxes = Axes.Both,
                                Colour = Colour4.Gray
                            }
                        }
                    },
                    progressBar = new Container()
                    {
                        Masking = true,
                        CornerRadius = 10,
                        RelativeSizeAxes = Axes.Both,
                        Width = 0f,
                        Children = new Drawable[]
                        {
                            new Box
                            {
                                RelativeSizeAxes = Axes.Both,
                                Colour = Colour4.White
                            }
                        }
                    },
                }
            },
            currentTime = new RenakoSpriteText()
            {
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                Font = RenakoFont.GetFont(RenakoFont.Typeface.JosefinSans, 25),
                Text = "0:00"
            },
            totalTime = new RenakoSpriteText()
            {
                Anchor = Anchor.CentreRight,
                Origin = Anchor.CentreRight,
                Font = RenakoFont.GetFont(RenakoFont.Typeface.JosefinSans, 25),
                Text = "0:00"
            }
        };
    }

    /// <summary>
    /// Set the total time of the progress bar in milliseconds.
    /// </summary>
    /// <param name="time">The total time in milliseconds.</param>
    public void SetTotalTime(double time)
    {
        totalTime.Text = TimeSpan.FromMilliseconds(time).ToString(@"m\:ss");
        totalTimeValue = time;
    }

    /// <summary>
    /// Set the current time of the progress bar in milliseconds.
    /// </summary>
    /// <param name="time">The current time in milliseconds.</param>
    public void SetCurrentTime(double time)
    {
        currentTime.Text = TimeSpan.FromMilliseconds(time).ToString(@"m\:ss");
        if (totalTimeValue == 0)
            return;

        progressBar.ResizeWidthTo((float)(time / totalTimeValue), 500, Easing.OutQuint);
    }
}

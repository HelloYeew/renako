using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osu.Framework.Timing;
using osuTK;
using osuTK.Input;
using Renako.Game.Audio;
using Renako.Game.Beatmaps;
using Renako.Game.Configurations;
using Renako.Game.Graphics.Containers;
using Renako.Game.Graphics.Drawables;
using Renako.Game.Graphics.ScreenStacks;

namespace Renako.Game.Graphics.Screens;

/// <summary>
/// The playable playfield that receives input from the player.
/// </summary>
public partial class PlayablePlayfieldScreen : PlayfieldScreen
{
    private PlayfieldContainer playfieldContainer;
    private GameplayProgressBar progressBar;

    private readonly StopwatchClock playfieldClock = new StopwatchClock();

    [Resolved]
    private RenakoAudioManager audioManager { get; set; }

    [Resolved]
    private RenakoBackgroundScreenStack backgroundScreenStack { get; set; }

    [Resolved]
    private WorkingBeatmap workingBeatmap { get; set; }

    [BackgroundDependencyLoader]
    private void load(RenakoConfigManager configManager, RenakoBackgroundScreenStack backgroundScreenStack)
    {
        AddInternal(playfieldContainer = new PlayfieldContainer(playfieldClock)
        {
            RelativeSizeAxes = Axes.Both
        });

        // Dummy button for testing on touch screen
        if (RuntimeInfo.IsMobile)
        {
            AddInternal(new BasicButton()
            {
                Text = "1",
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Action = () => playfieldContainer.ReceiveLaneInput(NoteLane.Lane1),
                Size = new Vector2(100),
                BackgroundColour = Color4Extensions.FromHex("F0E0E0"),
                Alpha = 0.25f,
                Masking = true,
                CornerRadius = 25,
                Position = new Vector2(PlayfieldContainer.GetLaneX(NoteLane.Lane1), 200)
            });
            AddInternal(new BasicButton()
            {
                Text = "2",
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Action = () => playfieldContainer.ReceiveLaneInput(NoteLane.Lane2),
                Size = new Vector2(100),
                BackgroundColour = Color4Extensions.FromHex("F0E0E0"),
                Alpha = 0.25f,
                Masking = true,
                CornerRadius = 25,
                Position = new Vector2(PlayfieldContainer.GetLaneX(NoteLane.Lane2), 200)
            });
            AddInternal(new BasicButton()
            {
                Text = "3",
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Action = () => playfieldContainer.ReceiveLaneInput(NoteLane.Lane3),
                Size = new Vector2(100),
                BackgroundColour = Color4Extensions.FromHex("F0E0E0"),
                Alpha = 0.25f,
                Masking = true,
                CornerRadius = 25,
                Position = new Vector2(PlayfieldContainer.GetLaneX(NoteLane.Lane3), 200)
            });
            AddInternal(new BasicButton()
            {
                Text = "4",
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Action = () => playfieldContainer.ReceiveLaneInput(NoteLane.Lane4),
                Size = new Vector2(100),
                BackgroundColour = Color4Extensions.FromHex("F0E0E0"),
                Alpha = 0.25f,
                Masking = true,
                CornerRadius = 25,
                Position = new Vector2(PlayfieldContainer.GetLaneX(NoteLane.Lane4), 200)
            });
        }

        AddInternal(new BasicButton()
        {
            Text = "Exit",
            Anchor = Anchor.BottomRight,
            Origin = Anchor.BottomRight,
            Action = this.Exit,
            Size = new Vector2(100, 50),
            Margin = new MarginPadding(20)
        });

        AddInternal(progressBar = new GameplayProgressBar());
        progressBar.SetTotalTime(workingBeatmap.BeatmapSet.TotalLength);

        backgroundScreenStack.AdjustMaskAlpha(configManager.Get<int>(RenakoSetting.PlayfieldBackgroundDim) / 100f);
    }

    protected override void LoadComplete()
    {
        Scheduler.AddDelayed(() =>
        {
            audioManager.Track?.Stop();
            audioManager.Track?.Restart();
            audioManager.Track?.Seek(0);
            audioManager.Track?.Start();
            playfieldClock.Start();

            if (workingBeatmap.BeatmapSet.HasVideo)
            {
                backgroundScreenStack.SeekBackgroundVideo(0f);
                backgroundScreenStack.ShowBackgroundVideo();
            }
        }, 2000);

        if (audioManager.Track != null)
        {
            audioManager.Track.Looping = false;
            audioManager.Track.RestartPoint = 0;
            audioManager.Track.Completed += this.Exit;
        }

        base.LoadComplete();
    }

    protected override void Update()
    {
        base.Update();
        progressBar.SetCurrentTime(playfieldClock.CurrentTime);
    }

    protected override bool OnKeyDown(KeyDownEvent e)
    {
        switch (e.Key)
        {
            case Key.D:
                playfieldContainer.ReceiveLaneInput(NoteLane.Lane1);
                break;

            case Key.F:
                playfieldContainer.ReceiveLaneInput(NoteLane.Lane2);
                break;

            case Key.J:
                playfieldContainer.ReceiveLaneInput(NoteLane.Lane3);
                break;

            case Key.K:
                playfieldContainer.ReceiveLaneInput(NoteLane.Lane4);
                break;

            case Key.Escape:
                // TODO: This need to be paused instead of exit
                this.Exit();
                break;
        }

        return base.OnKeyDown(e);
    }
}

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osu.Framework.Timing;
using osuTK.Input;
using Renako.Game.Audio;
using Renako.Game.Beatmaps;
using Renako.Game.Configurations;
using Renako.Game.Graphics.Containers;
using Renako.Game.Graphics.ScreenStacks;

namespace Renako.Game.Graphics.Screens;

/// <summary>
/// The playable playfield that receives input from the player.
/// </summary>
public partial class PlayablePlayfieldScreen : PlayfieldScreen
{
    private PlayfieldContainer playfieldContainer;

    private readonly StopwatchClock stopwatchClock = new StopwatchClock();

    [Resolved]
    private RenakoAudioManager audioManager { get; set; }

    [BackgroundDependencyLoader]
    private void load(RenakoConfigManager configManager, RenakoBackgroundScreenStack backgroundScreenStack)
    {
        AddInternal(playfieldContainer = new PlayfieldContainer(stopwatchClock)
        {
            RelativeSizeAxes = Axes.Both
        });
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
            stopwatchClock.Start();
        }, 3000);

        base.LoadComplete();
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

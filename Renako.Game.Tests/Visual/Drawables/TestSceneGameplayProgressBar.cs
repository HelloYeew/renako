using NUnit.Framework;
using osu.Framework.Allocation;
using Renako.Game.Beatmaps;
using Renako.Game.Graphics.Drawables;

namespace Renako.Game.Tests.Visual.Drawables;

public partial class TestSceneGameplayProgressBar : RenakoGameDrawableTestScene
{
    [Cached]
    private BeatmapsCollection beatmapsCollection = new BeatmapsCollection();

    [Cached]
    private WorkingBeatmap workingBeatmap = new WorkingBeatmap();

    private GameplayProgressBar gameplayProgressBar;

    protected override void LoadComplete()
    {
        base.LoadComplete();
        beatmapsCollection.GenerateTestCollection();
        workingBeatmap.BeatmapSet = beatmapsCollection.BeatmapSets.Find(set => set.ID == 1);
        workingBeatmap.Beatmap = beatmapsCollection.Beatmaps.Find(beatmap => beatmap.ID == 1);
    }

    [Test]
    public void TestGameplayProgressBar()
    {
        AddStep("add gameplay progress bar", () => Add(gameplayProgressBar = new GameplayProgressBar()));
        AddStep("set total time", () => gameplayProgressBar.SetTotalTime(workingBeatmap.BeatmapSet.TotalLength));
        AddStep("start track", () => AudioManager.Track.Start());
        AddStep("set current time", () => gameplayProgressBar.SetCurrentTime(AudioManager.Track.CurrentTime));
        AddStep("seek to 100s", () => AudioManager.Track.Seek(100000));
    }

    protected override void Update()
    {
        base.Update();

        if (AudioManager.Track.IsRunning)
        {
            gameplayProgressBar.SetCurrentTime(AudioManager.Track.CurrentTime);
        }
    }
}

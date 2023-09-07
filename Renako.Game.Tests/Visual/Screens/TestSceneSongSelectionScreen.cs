using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Screens;
using Renako.Game.Audio;
using Renako.Game.Beatmaps;
using Renako.Game.Graphics.Screens;
using Renako.Game.Graphics.ScreenStacks;
using Renako.Game.Stores;

namespace Renako.Game.Tests.Visual.Screens;

[TestFixture]
public partial class TestSceneSongSelectionScreen : RenakoTestScene
{
    [Cached]
    private RenakoBackgroundScreenStack backgroundScreenStack = new RenakoBackgroundScreenStack();

    [Cached]
    private RenakoScreenStack mainScreenStack = new RenakoScreenStack();

    [Cached]
    private LogoScreenStack logoScreenStack = new LogoScreenStack();

    [Cached]
    private RenakoAudioManager audioManager = new RenakoAudioManager();

    [Cached]
    private BeatmapsCollection beatmapsCollection = new BeatmapsCollection();

    [Cached]
    private WorkingBeatmap workingBeatmap = new WorkingBeatmap();

    [BackgroundDependencyLoader]
    private void load()
    {
        beatmapsCollection.GenerateTestCollection();
        Dependencies.CacheAs(mainScreenStack);
        Dependencies.CacheAs(backgroundScreenStack);
        Dependencies.CacheAs(logoScreenStack);
        Dependencies.CacheAs(audioManager);
        Dependencies.CacheAs(beatmapsCollection);
        Dependencies.CacheAs(workingBeatmap);
    }

    [SetUp]
    public void SetUp()
    {
        Add(backgroundScreenStack);
        Add(mainScreenStack);
        Add(logoScreenStack);
        Add(audioManager);
        mainScreenStack.Push(new SongSelectionScreen());
        Scheduler.Add(() => audioManager.Mute());
        AddAssert("screen loaded", () => mainScreenStack.CurrentScreen is SongSelectionScreen);
        AddStep("rerun", () => {
            mainScreenStack.CurrentScreen?.Exit();
            mainScreenStack.Push(new SongSelectionScreen());
            audioManager.Mute();
        });
    }
}

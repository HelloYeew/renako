using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Screens;
using Renako.Game.Audio;
using Renako.Game.Beatmaps;
using Renako.Game.Graphics.Screens;
using Renako.Game.Graphics.ScreenStacks;

namespace Renako.Game.Tests.Visual.Screens;

[TestFixture]
public partial class TestSceneStartScreen : RenakoTestScene
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
    private WorkingBeatmap workingBeatmap = new WorkingBeatmap();

    [BackgroundDependencyLoader]
    private void load()
    {
        Dependencies.CacheAs(mainScreenStack);
        Dependencies.CacheAs(logoScreenStack);
        Dependencies.CacheAs(backgroundScreenStack);
        Dependencies.CacheAs(audioManager);
        Dependencies.CacheAs(workingBeatmap);
    }

    [SetUp]
    public void SetUp()
    {
        Add(backgroundScreenStack);
        Add(mainScreenStack);
        Add(logoScreenStack);
        Add(audioManager);
        mainScreenStack.Push(new StartScreen());
        AddAssert("screen loaded", () => mainScreenStack.CurrentScreen is StartScreen);
        AddStep("rerun", () => {
            mainScreenStack.CurrentScreen.Exit();
            mainScreenStack.Push(new StartScreen());
        });
    }
}

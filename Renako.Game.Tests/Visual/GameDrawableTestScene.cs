using osu.Framework.Allocation;
using osu.Framework.Testing;
using Renako.Game.Audio;
using Renako.Game.Beatmaps;
using Renako.Game.Graphics.ScreenStacks;
using Renako.Game.Stores;

namespace Renako.Game.Tests.Visual;

/// <summary>
/// The extend class of <see cref="RenakoTestScene"/> that's already have all the dependencies needed for testing.
/// </summary>
public partial class GameDrawableTestScene : RenakoTestScene
{
    /// <summary>
    /// Load the beatmap test collection using <see cref="BeatmapsCollection.GenerateTestCollection"/>
    /// </summary>
    public bool LoadBeatmapTestCollection { get; set; } = false;

    /// <summary>
    /// Load all screen stacks using in game on setup.
    /// </summary>
    public bool LoadAllScreenStacks { get; set; } = false;

    [Cached]
    public readonly RenakoBackgroundScreenStack backgroundScreenStack = new RenakoBackgroundScreenStack();

    [Cached]
    public readonly RenakoScreenStack mainScreenStack = new RenakoScreenStack();

    [Cached]
    public readonly LogoScreenStack logoScreenStack = new LogoScreenStack();

    [Cached]
    public readonly SettingsScreenStack settingsScreenStack = new SettingsScreenStack();

    [Cached]
    public readonly RenakoAudioManager audioManager = new RenakoAudioManager();

    [Cached]
    public readonly BeatmapsCollection beatmapsCollection = new BeatmapsCollection();

    [Cached]
    public readonly WorkingBeatmap workingBeatmap = new WorkingBeatmap();

    [BackgroundDependencyLoader]
    private void load()
    {
        Dependencies.CacheAs(mainScreenStack);
        Dependencies.CacheAs(logoScreenStack);
        Dependencies.CacheAs(backgroundScreenStack);
        Dependencies.CacheAs(settingsScreenStack);
        Dependencies.CacheAs(audioManager);
        Dependencies.CacheAs(workingBeatmap);
        Dependencies.CacheAs(beatmapsCollection);

        Add(audioManager);

        if (LoadBeatmapTestCollection) beatmapsCollection.GenerateTestCollection();
    }

    [SetUpSteps]
    public void setupTest()
    {
        AddStep("clear all children", () =>
        {
            Clear();
        });

        if (LoadAllScreenStacks)
        {
            Scheduler.Add(() => loadAllScreenStacks());
        }
    }

    public void loadAllScreenStacks()
    {
        Add(backgroundScreenStack);
        Add(mainScreenStack);
        Add(logoScreenStack);
        Add(settingsScreenStack);
    }
}

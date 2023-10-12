using osu.Framework.Allocation;
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
        Add(backgroundScreenStack);
        Add(mainScreenStack);
        Add(logoScreenStack);
        Add(settingsScreenStack);
    }
}

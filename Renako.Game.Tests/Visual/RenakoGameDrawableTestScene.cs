using osu.Framework.Allocation;
using osu.Framework.Platform;
using Renako.Game.Audio;
using Renako.Game.Beatmaps;
using Renako.Game.Database;
using Renako.Game.Graphics.ScreenStacks;

namespace Renako.Game.Tests.Visual;

/// <summary>
/// The extend class of <see cref="RenakoTestScene"/> that's already have all the dependencies needed for testing the game drawable.
/// </summary>
public partial class RenakoGameDrawableTestScene : RenakoTestScene
{
    [Resolved]
    private GameHost host { get; set; }

    [Cached]
    public readonly RenakoBackgroundScreenStack BackgroundScreenStack = new RenakoBackgroundScreenStack();

    [Cached]
    public readonly RenakoScreenStack MainScreenStack = new RenakoScreenStack();

    [Cached]
    public readonly LogoScreenStack LogoScreenStack = new LogoScreenStack();

    [Cached]
    public readonly SettingsScreenStack SettingsScreenStack = new SettingsScreenStack();

    [Cached]
    public readonly RenakoAudioManager AudioManager = new RenakoAudioManager();

    [Cached]
    public readonly BeatmapsCollection BeatmapsCollection = new BeatmapsCollection();

    [BackgroundDependencyLoader]
    private void load()
    {
        Dependencies.CacheAs(new BeatmapCollectionReader(host.Storage, BeatmapsCollection));
        Add(BackgroundScreenStack);
        Add(MainScreenStack);
        Add(LogoScreenStack);
        Add(SettingsScreenStack);
        Add(AudioManager);
    }
}

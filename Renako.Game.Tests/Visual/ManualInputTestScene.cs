using osu.Framework.Allocation;
using osu.Framework.Testing;
using osuTK.Input;
using Renako.Game.Graphics.ScreenStacks;

namespace Renako.Game.Tests.Visual;

/// <summary>
/// The test scene for testing in-game manual input, already prepared with all the dependencies needed.
/// </summary>
public partial class ManualInputTestScene : ManualInputManagerTestScene
{
    [Cached]
    public readonly RenakoBackgroundScreenStack BackgroundScreenStack = new RenakoBackgroundScreenStack();

    [Cached]
    public readonly RenakoScreenStack MainScreenStack = new RenakoScreenStack();

    [Cached]
    public readonly LogoScreenStack LogoScreenStack = new LogoScreenStack();

    [Cached]
    public readonly SettingsScreenStack SettingsScreenStack = new SettingsScreenStack();

    [BackgroundDependencyLoader]
    private void load()
    {
        Add(BackgroundScreenStack);
        Add(MainScreenStack);
        Add(LogoScreenStack);
        Add(SettingsScreenStack);
    }

    /// <summary>
    /// Press and release a key once.
    /// </summary>
    /// <param name="key">The key to press and release.</param>
    public void PressKeyOnce(Key key)
    {
        InputManager.PressKey(key);
        InputManager.ReleaseKey(key);
    }
}

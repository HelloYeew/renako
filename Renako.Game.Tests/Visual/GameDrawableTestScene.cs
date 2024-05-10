using osu.Framework.Allocation;
using Renako.Game.Graphics.ScreenStacks;

namespace Renako.Game.Tests.Visual;

/// <summary>
/// The extend class of <see cref="RenakoTestScene"/> that's already have all the dependencies needed for testing the game drawable.
/// </summary>
public partial class GameDrawableTestScene : RenakoTestScene
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
}

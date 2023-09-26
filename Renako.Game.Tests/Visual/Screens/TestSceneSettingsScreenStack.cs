using NUnit.Framework;
using osu.Framework.Allocation;
using Renako.Game.Graphics.ScreenStacks;

namespace Renako.Game.Tests.Visual.Screens;

[TestFixture]
public partial class TestSceneSettingsScreenStack : RenakoTestScene
{
    [Cached]
    private RenakoScreenStack mainScreenStack = new RenakoScreenStack();

    private readonly SettingsScreenStack settingsScreenStack = new SettingsScreenStack();

    [BackgroundDependencyLoader]
    private void load()
    {
        Dependencies.CacheAs(mainScreenStack);
    }

    [SetUp]
    public void SetUp()
    {
        Add(mainScreenStack);
        Add(settingsScreenStack);
        AddStep("toggle visibility", () => settingsScreenStack.ToggleVisibility());
    }
}

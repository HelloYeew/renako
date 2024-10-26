using NUnit.Framework;

namespace Renako.Game.Tests.Visual.Screens;

public partial class TestSceneSettingsScreenStack : RenakoGameDrawableTestScene
{
    [Test]
    public void TestSettingsScreenStack()
    {
        AddStep("toggle visibility", () => SettingsScreenStack.ToggleVisibility());
    }
}

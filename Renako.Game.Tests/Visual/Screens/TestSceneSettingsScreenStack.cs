using NUnit.Framework;

namespace Renako.Game.Tests.Visual.Screens;

public partial class TestSceneSettingsScreenStack : GameDrawableTestScene
{
    [Test]
    public void TestSettingsScreenStack()
    {
        AddStep("toggle visibility", () => settingsScreenStack.ToggleVisibility());
    }
}

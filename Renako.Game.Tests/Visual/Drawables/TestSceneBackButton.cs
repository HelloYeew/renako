using NUnit.Framework;
using osu.Framework.Graphics;
using Renako.Game.Graphics.UserInterface;

namespace Renako.Game.Tests.Visual.Drawables;

[TestFixture]
public partial class TestSceneBackButton : GameDrawableTestScene
{
    public TestSceneBackButton()
    {
        BackButton backButton;
        Add(backButton = new BackButton()
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre
        });

        AddStep("Enable or disable button", () => backButton.Enabled.Value = !backButton.Enabled.Value);
    }
}


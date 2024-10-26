using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using Renako.Game.Graphics.UserInterface;

namespace Renako.Game.Tests.Visual.Drawables;

[TestFixture]
public partial class TestSceneRightBottomButton : RenakoGameDrawableTestScene
{
    private LeftBottomButton leftBottomButton;
    private RightBottomButton rightBottomButton;

    public TestSceneRightBottomButton()
    {
        Add(rightBottomButton = new RightBottomButton()
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Icon = FontAwesome.Solid.ArrowRight
        });
        Add(leftBottomButton = new LeftBottomButton()
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre
        });
        AddStep("Show or hide LeftBottomButton", () => leftBottomButton.Alpha = leftBottomButton.Alpha == 1 ? 0 : 1);
        AddStep("Enable or disable button", () => rightBottomButton.Enabled.Value = !rightBottomButton.Enabled.Value);
    }
}

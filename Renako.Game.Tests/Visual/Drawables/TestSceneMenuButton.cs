using NUnit.Framework;
using osu.Framework.Graphics;
using Renako.Game.Graphics.UserInterface;

namespace Renako.Game.Tests.Visual.Drawables;

[TestFixture]
public partial class TestSceneMenuButton : GameDrawableTestScene
{
    public TestSceneMenuButton()
    {
        Add(new MenuButton()
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre
        });
    }
}

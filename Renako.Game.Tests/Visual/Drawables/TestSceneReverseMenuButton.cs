using NUnit.Framework;
using osu.Framework.Graphics;
using Renako.Game.Graphics.UserInterface;

namespace Renako.Game.Tests.Visual.Drawables;

[TestFixture]
public partial class TestSceneReverseMenuButton : RenakoTestScene
{
    public TestSceneReverseMenuButton()
    {
        Add(new ReverseMenuButton()
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre
        });
    }
}

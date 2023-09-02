using NUnit.Framework;
using osu.Framework.Graphics;
using Renako.Game.Graphics.Drawables;

namespace Renako.Game.Tests.Visual.Drawables;

[TestFixture]
public partial class TestSceneMenuButton : RenakoTestScene
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

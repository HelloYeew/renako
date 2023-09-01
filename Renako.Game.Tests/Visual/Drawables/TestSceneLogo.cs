using NUnit.Framework;
using osu.Framework.Graphics;
using osuTK;
using Renako.Game.Graphics.Drawables;

namespace Renako.Game.Tests.Visual.Drawables;

[TestFixture]
public partial class TestSceneLogo : RenakoTestScene
{
    public TestSceneLogo()
    {
        Add(new RenakoLogo()
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Size = new Vector2(249, 84)
        });
    }
}

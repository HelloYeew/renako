using NUnit.Framework;
using osu.Framework.Graphics;
using Renako.Game.Graphics.Drawables;

namespace Renako.Game.Tests.Visual.Drawables;

[TestFixture]
public partial class TestSceneLogo : RenakoGameDrawableTestScene
{
    public TestSceneLogo()
    {
        Add(new RenakoLogo()
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre
        });
    }
}

using NUnit.Framework;
using osu.Framework.Graphics;
using Renako.Game.Graphics.Drawables;

namespace Renako.Game.Tests.Visual.Drawables;

[TestFixture]
public partial class TestSceneLeftBottomButton : RenakoTestScene
{
    public TestSceneLeftBottomButton()
    {
        Add(new LeftBottomButton()
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre
        });
    }
}

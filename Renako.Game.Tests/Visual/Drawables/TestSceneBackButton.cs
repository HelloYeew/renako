using NUnit.Framework;
using osu.Framework.Graphics;
using Renako.Game.Graphics.Drawables;

namespace Renako.Game.Tests.Visual.Drawables;

[TestFixture]
public partial class TestSceneBackButton : RenakoTestScene
{
    public TestSceneBackButton()
    {
        Add(new BackButton()
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre
        });
    }
}

using NUnit.Framework;
using osu.Framework.Graphics;
using Renako.Game.Graphics.UserInterface;

namespace Renako.Game.Tests.Visual.Drawables;

[TestFixture]
public partial class TestSceneMenuTitle : RenakoGameDrawableTestScene
{
    public TestSceneMenuTitle()
    {
        Add(new MenuTitle()
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre
        });
    }
}

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Platform;
using osu.Framework.Testing;

namespace Renako.Game.Tests
{
    public partial class RenakoTestBrowser : RenakoGameBase
    {
        public new DependencyContainer Dependencies { get; private set; }

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
        {
            IReadOnlyDependencyContainer baseDependencies = base.CreateChildDependencies(parent);
            Dependencies = new DependencyContainer(baseDependencies);
            return Dependencies;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            AddRange(new Drawable[]
            {
                new TestBrowser("Renako"),
                new CursorContainer()
            });
        }

        public override void SetHost(GameHost host)
        {
            base.SetHost(host);
            host.Window.CursorState |= CursorState.Hidden;
        }
    }
}

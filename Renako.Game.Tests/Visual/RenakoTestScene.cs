using osu.Framework.Allocation;
using osu.Framework.Testing;
using Renako.Game.Graphics.ScreenStacks;

namespace Renako.Game.Tests.Visual
{
    public partial class RenakoTestScene : TestScene
    {
        protected override ITestSceneTestRunner CreateRunner() => new RenakoTestSceneTestRunner();

        public new DependencyContainer Dependencies { get; set; }

        public new RenakoBackgroundScreenStack BackgroundScreenStack { get; set; }

        public new RenakoScreenStack MainScreenStack { get; set; }

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
        {
            IReadOnlyDependencyContainer baseDependencies = base.CreateChildDependencies(parent);
            Dependencies = new DependencyContainer(baseDependencies);
            Dependencies.CacheAs(BackgroundScreenStack = new RenakoBackgroundScreenStack());
            Dependencies.CacheAs(MainScreenStack = new RenakoScreenStack());
            return Dependencies;
        }

        private partial class RenakoTestSceneTestRunner : RenakoGameBase, ITestSceneTestRunner
        {
            private TestSceneTestRunner.TestRunner runner;

            protected override void LoadAsyncComplete()
            {
                base.LoadAsyncComplete();
                Add(runner = new TestSceneTestRunner.TestRunner());
            }

            public void RunTestBlocking(TestScene test) => runner.RunTestBlocking(test);
        }
    }
}

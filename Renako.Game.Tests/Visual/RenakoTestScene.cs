using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Testing;

namespace Renako.Game.Tests.Visual
{
    public partial class RenakoTestScene : TestScene
    {
        [Resolved]
        private AudioManager frameworkAudioManager { get; set; }

        protected override ITestSceneTestRunner CreateRunner() => new RenakoTestSceneTestRunner();

        public new DependencyContainer Dependencies { get; set; }

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
        {
            IReadOnlyDependencyContainer baseDependencies = base.CreateChildDependencies(parent);
            Dependencies = new DependencyContainer(baseDependencies);
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

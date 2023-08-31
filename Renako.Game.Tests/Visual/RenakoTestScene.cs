using osu.Framework.Testing;

namespace Renako.Game.Tests.Visual
{
    public partial class RenakoTestScene : TestScene
    {
        protected override ITestSceneTestRunner CreateRunner() => new RenakoTestSceneTestRunner();

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

using osu.Framework.Testing;

namespace OFModTest.Game.Tests.Visual
{
    public abstract partial class OFModTestTestScene : TestScene
    {
        protected override ITestSceneTestRunner CreateRunner() => new OFModTestTestSceneTestRunner();

        private partial class OFModTestTestSceneTestRunner : OFModTestGameBase, ITestSceneTestRunner
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

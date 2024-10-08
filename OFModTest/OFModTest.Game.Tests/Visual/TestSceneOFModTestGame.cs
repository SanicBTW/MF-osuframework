using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Platform;

namespace OFModTest.Game.Tests.Visual
{
    [TestFixture]
    public partial class TestSceneOFModTestGame : OFModTestTestScene
    {
        // Add visual tests to ensure correct behaviour of your game: https://github.com/ppy/osu-framework/wiki/Development-and-Testing
        // You can make changes to classes associated with the tests and they will recompile and update immediately.

        private OFModTestGame game;

        [BackgroundDependencyLoader]
        private void load(GameHost host)
        {
            game = new OFModTestGame();
            game.SetHost(host);

            AddGame(game);
        }
    }
}

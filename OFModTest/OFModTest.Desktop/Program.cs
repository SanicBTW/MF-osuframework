using OFModTest.Game;
using osu.Framework;
using osu.Framework.Platform;

namespace OFModTest.Desktop
{
    public static class Program
    {
        public static void Main()
        {
            using (GameHost host = Host.GetSuitableDesktopHost(@"OFModTest"))
            using (osu.Framework.Game game = new OFModTestGame())
                host.Run(game);
        }
    }
}

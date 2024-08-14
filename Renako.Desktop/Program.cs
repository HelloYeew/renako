using osu.Framework.Platform;
using osu.Framework;
using Velopack;

namespace Renako.Desktop
{
    public static class Program
    {
        public static void Main()
        {
            using (GameHost host = Host.GetSuitableDesktopHost(@"Renako"))
            using (osu.Framework.Game game = new RenakoGameDesktop())
            {
                VelopackApp.Build().Run();
                host.Run(game);
            }
        }
    }
}

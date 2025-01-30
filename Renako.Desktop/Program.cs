using osu.Framework.Platform;
using osu.Framework;
using Velopack;

namespace Renako.Desktop
{
    public static class Program
    {
#if DEBUG
        private const string base_game_name = "RenakoDevelopment";
#else
        private const string base_game_name = "Renako";
#endif

        public static void Main()
        {
            using (GameHost host = Host.GetSuitableDesktopHost(base_game_name))
            using (osu.Framework.Game game = new RenakoGameDesktop())
            {
                VelopackApp.Build().Run();
                host.Run(game);
            }
        }
    }
}

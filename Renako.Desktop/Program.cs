using osu.Framework.Platform;
using osu.Framework;
using Renako.Game;

namespace Renako.Desktop
{
    public static class Program
    {
        public static void Main()
        {
            using (GameHost host = Host.GetSuitableDesktopHost(@"Renako"))
            using (osu.Framework.Game game = new RenakoGame())
                host.Run(game);
        }
    }
}

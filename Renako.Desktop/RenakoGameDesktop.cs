using osu.Framework.Development;
using osu.Framework.Platform;
using Renako.Game;

namespace Renako.Desktop;

// ReSharper disable once InconsistentNaming
public partial class RenakoGameDesktop : RenakoGame
{
    public override void SetHost(GameHost host)
    {
        base.SetHost(host);
        var desktopWindow = host.Window;

        desktopWindow.Title = "Renako";
        if (DebugUtils.IsDebugBuild)
            desktopWindow.Title += " development";
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();

        // Load component on desktop here
    }
}

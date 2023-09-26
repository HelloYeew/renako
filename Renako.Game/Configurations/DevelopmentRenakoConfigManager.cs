using osu.Framework.Platform;

namespace Renako.Game.Configurations;

public class DevelopmentRenakoConfigManager : RenakoConfigManager
{
    protected override string Filename => base.Filename.Replace(".ini", ".dev.ini");

    public DevelopmentRenakoConfigManager(Storage storage)
        : base(storage)
    {
    }
}

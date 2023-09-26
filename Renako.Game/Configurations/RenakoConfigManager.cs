using osu.Framework.Configuration;
using osu.Framework.Platform;

namespace Renako.Game.Configurations;

public class RenakoConfigManager : IniConfigManager<RenakoSetting>
{
    public RenakoConfigManager(Storage storage)
        : base(storage)
    {
    }

    protected override void InitialiseDefaults()
    {
        SetDefault(RenakoSetting.UseUnicodeInfo, true);
    }
}

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
        SetDefault(RenakoSetting.UseUnicodeInfo, false);
        SetDefault(RenakoSetting.LatestBeatmapSetID, 0);
        SetDefault(RenakoSetting.LatestBeatmapID, 0);
    }
}

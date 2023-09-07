using osu.Framework.Bindables;

namespace Renako.Game.Beatmaps;

/// <summary>
/// A class that's store the current playing or selected <see cref="Beatmap"/>
/// </summary>
public class WorkingBeatmap
{
    public Bindable<Beatmap> BindableWorkingBeatmap = new Bindable<Beatmap>();

    public Bindable<BeatmapSet> BindableWorkingBeatmapSet = new Bindable<BeatmapSet>();

    public Beatmap Beatmap
    {
        get => BindableWorkingBeatmap.Value;
        set => BindableWorkingBeatmap.Value = value;
    }

    public BeatmapSet BeatmapSet
    {
        get => BindableWorkingBeatmapSet.Value;
        set => BindableWorkingBeatmapSet.Value = value;
    }
}

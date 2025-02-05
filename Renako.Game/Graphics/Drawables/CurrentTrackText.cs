using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;
using Renako.Game.Beatmaps;
using Renako.Game.Configurations;

namespace Renako.Game.Graphics.Drawables;

public partial class CurrentTrackText : CompositeDrawable
{
    private RenakoSpriteText nameText;
    private RenakoSpriteText artistText;
    private Bindable<bool> useUnicodeInfoSettings;

    [BackgroundDependencyLoader]
    private void load(WorkingBeatmap workingBeatmap, RenakoConfigManager configManager)
    {
        InternalChild = new FillFlowContainer()
        {
            Anchor = Anchor.TopRight,
            Origin = Anchor.TopRight,
            AutoSizeAxes = Axes.Both,
            Direction = FillDirection.Vertical,
            RelativePositionAxes = Axes.X,
            Spacing = new Vector2(0, 5),
            Children = new Drawable[]
            {
                nameText = new RenakoSpriteText
                {
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                    Font = RenakoFont.GetFont(RenakoFont.Typeface.JosefinSans, 30, RenakoFont.FontWeight.Bold)
                },
                artistText = new RenakoSpriteText
                {
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                    Font = RenakoFont.GetFont(RenakoFont.Typeface.MPlus1P, 20)
                }
            }
        };

        workingBeatmap.BindableWorkingBeatmapSet.BindValueChanged(e =>
        {
            if (e.NewValue == null) return;

            // Clear all scheduled transforms
            Scheduler.CancelDelayedTasks();
            nameText.ClearTransforms();
            artistText.ClearTransforms();

            nameText.Alpha = 1;
            artistText.Alpha = 1;

            bool useUnicode = configManager.Get<bool>(RenakoSetting.UseUnicodeInfo);
            nameText.Text = useUnicode ? e.NewValue.TitleUnicode : e.NewValue.Title;
            artistText.Text = useUnicode ? e.NewValue.ArtistUnicode : e.NewValue.Artist;

            Scheduler.AddDelayed(() =>
            {
                nameText.FadeOut(500);
                artistText.FadeOut(500);
            }, 3000);
        }, true);

        useUnicodeInfoSettings = configManager.GetBindable<bool>(RenakoSetting.UseUnicodeInfo);

        useUnicodeInfoSettings.ValueChanged += e =>
        {
            if (workingBeatmap.BeatmapSet == null) return;

            nameText.Text = e.NewValue ? workingBeatmap.BeatmapSet.TitleUnicode : workingBeatmap.BeatmapSet.Title;
            artistText.Text = e.NewValue ? workingBeatmap.BeatmapSet.ArtistUnicode : workingBeatmap.BeatmapSet.Artist;
        };
    }
}

using System.IO;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osuTK;
using osuTK.Input;
using Renako.Game.Beatmaps;
using Renako.Game.Configurations;
using Renako.Game.Graphics.Drawables;
using Renako.Game.Graphics.ScreenStacks;
using Renako.Game.Utilities;

namespace Renako.Game.Graphics.Screens;

public partial class PlayerLoadingScreen : RenakoScreen
{
    private bool loadPlayer;
    private bool isScreenLoaded;

    private RightBottomBeatmapSetDetailContainer beatmapSetDetailContainer;

    [Resolved]
    private WorkingBeatmap workingBeatmap { get; set; }

    [Resolved]
    private TextureStore textureStore { get; set; }

    [Resolved]
    private GameHost host { get; set; }

    [Resolved]
    private RenakoScreenStack mainScreenStack { get; set; }

    public PlayerLoadingScreen(bool loadPlayer = false)
    {
        this.loadPlayer = loadPlayer;
    }

    [BackgroundDependencyLoader]
    private void load(RenakoConfigManager configManager)
    {
        InternalChildren = new Drawable[]
        {
            new LoadingSpinner()
            {
                Anchor = Anchor.BottomLeft,
                Origin = Anchor.BottomLeft,
                Size = new Vector2(50),
                Margin = new MarginPadding()
                {
                    Left = 20,
                    Bottom = 20
                }
            },
            beatmapSetDetailContainer = new RightBottomBeatmapSetDetailContainer()
        };

        beatmapSetDetailContainer.Title = configManager.Get<bool>(RenakoSetting.UseUnicodeInfo) ? workingBeatmap.BeatmapSet.TitleUnicode : workingBeatmap.BeatmapSet.Title;
        beatmapSetDetailContainer.Artist = configManager.Get<bool>(RenakoSetting.UseUnicodeInfo) ? workingBeatmap.BeatmapSet.ArtistUnicode : workingBeatmap.BeatmapSet.Artist;
        beatmapSetDetailContainer.Source = configManager.Get<bool>(RenakoSetting.UseUnicodeInfo) ? workingBeatmap.BeatmapSet.SourceUnicode : workingBeatmap.BeatmapSet.Source;
        Texture coverTexture = textureStore.Get(workingBeatmap.BeatmapSet.CoverPath);

        if (coverTexture == null)
        {
            Stream coverTextureStream = host.Storage.GetStream(BeatmapSetUtility.GetCoverPath(workingBeatmap.BeatmapSet));
            coverTexture = Texture.FromStream(host.Renderer, coverTextureStream);
            coverTextureStream?.Close();
        }

        beatmapSetDetailContainer.CoverImage = coverTexture;
    }

    protected override void LoadComplete()
    {
        Scheduler.AddDelayed(() =>
        {
            this.Exit();
            mainScreenStack.Push(new PlayablePlayfieldScreen());
        }, 1000);
    }

    protected override bool OnKeyDown(KeyDownEvent e)
    {
        switch (e.Key)
        {
            case Key.Escape:
                if (!isScreenLoaded)
                {
                    this.Exit();
                    return true;
                }

                break;
        }

        return base.OnKeyDown(e);
    }
}

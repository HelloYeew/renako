using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Bindables;
using osu.Framework.Development;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Performance;
using osu.Framework.IO.Stores;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osuTK;
using Renako.Game.Audio;
using Renako.Game.Beatmaps;
using Renako.Game.Configurations;
using Renako.Game.Database;
using Renako.Game.Stores;
using Renako.Resources;

namespace Renako.Game
{
    public partial class RenakoGameBase : osu.Framework.Game
    {
        // Anything in this class is shared between the test browser and the game implementation.
        // It allows for caching global dependencies that should be accessible to tests, or changing
        // the screen scaling for all components including the test browser and framework overlays.

        protected override Container<Drawable> Content { get; }

        protected Storage Storage { get; set; }

        protected RenakoConfigManager LocalConfig { get; private set; }

        private DependencyContainer dependencies;

        private RenakoTextureStore textureStore;

        private AudioManager audioManager;

        protected RenakoAudioManager RenakoAudioManager;

        private Bindable<bool> fpsDisplayVisible;

        private InternalBeatmapImporter internalBeatmapImporter;

        private BeatmapCollectionReader beatmapCollectionReader;

        protected BeatmapsCollection BeatmapsCollection;

        protected WorkingBeatmap WorkingBeatmap;

        protected RenakoGameBase()
        {
            // Ensure game and tests scale with window size and screen DPI.
            base.Content.Add(Content = new DrawSizePreservingFillContainer
            {
                // You may want to change TargetDrawSize to your "default" resolution, which will decide how things scale and position when using absolute coordinates.
                TargetDrawSize = new Vector2(1366, 768)
            });
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Resources.AddStore(new DllResourceStore(typeof(RenakoResources).Assembly));

            AddFont(Resources, @"Fonts/MPlus1p/MPlus1p-Regular");
            AddFont(Resources, @"Fonts/MPlus1p/MPlus1p-Bold");
            AddFont(Resources, @"Fonts/MPlus1p/MPlus1p-BoldItalic");
            AddFont(Resources, @"Fonts/MPlus1p/MPlus1p-Italic");
            AddFont(Resources, @"Fonts/MPlus1p/MPlus1p-Light");
            AddFont(Resources, @"Fonts/MPlus1p/MPlus1p-Medium");

            AddFont(Resources, @"Fonts/JosefinSans/JosefinSans");
            AddFont(Resources, @"Fonts/JosefinSans/JosefinSans-Bold");
            AddFont(Resources, @"Fonts/JosefinSans/JosefinSans-Italic");
            AddFont(Resources, @"Fonts/JosefinSans/JosefinSans-BoldItalic");

            AddFont(Resources, @"Fonts/YuGothic/YuGothic");
            AddFont(Resources, @"Fonts/YuGothic/YuGothic-Bold");
            AddFont(Resources, @"Fonts/YuGothic/YuGothic-Italic");
            AddFont(Resources, @"Fonts/YuGothic/YuGothic-BoldItalic");

            AddFont(Resources, @"Fonts/Offside/Offside");

            AddFont(Resources, @"Fonts/Noto/Noto-Basic");
            AddFont(Resources, @"Fonts/Noto/Noto-Hangul");
            AddFont(Resources, @"Fonts/Noto/Noto-CJK-Basic");
            AddFont(Resources, @"Fonts/Noto/Noto-CJK-Compatibility");

            ResourceStore<byte[]> trackResourceStore = new ResourceStore<byte[]>();
            trackResourceStore.AddStore(new NamespacedResourceStore<byte[]>(Resources, "Tracks"));
            trackResourceStore.AddStore(new ResourceStore<byte[]>(new RenakoStore(Host.Storage)));

            foreach (var track in trackResourceStore.GetAvailableResources())
            {
                Logger.Log($"Available track : {track}");
            }

            ResourceStore<byte[]> textureResourceStore = new ResourceStore<byte[]>();
            textureResourceStore.AddStore(new NamespacedResourceStore<byte[]>(Resources, "Textures"));
            textureResourceStore.AddStore(new ResourceStore<byte[]>(new RenakoStore(Host.Storage)));

            foreach (var texture in textureResourceStore.GetAvailableResources())
            {
                Logger.Log($"Available texture : {texture}");
            }

            dependencies.Cache(textureStore = new RenakoTextureStore(Host.Renderer, Host.CreateTextureLoaderStore(textureResourceStore)));
            dependencies.Cache(audioManager = new AudioManager(Host.AudioThread, trackResourceStore, new NamespacedResourceStore<byte[]>(Resources, "Samples")));
            dependencies.CacheAs(RenakoAudioManager = new RenakoAudioManager());
            dependencies.CacheAs(LocalConfig);
            dependencies.CacheAs(BeatmapsCollection = new BeatmapsCollection());
            dependencies.CacheAs(WorkingBeatmap = new WorkingBeatmap());
            dependencies.CacheAs(this);

            internalBeatmapImporter = new InternalBeatmapImporter(audioManager, textureStore, Host);

            if (!LocalConfig.Get<bool>(RenakoSetting.FirstImport))
            {
                internalBeatmapImporter.Import();
                LocalConfig.SetValue(RenakoSetting.FirstImport, true);
            }
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            beatmapCollectionReader = new BeatmapCollectionReader(Host.Storage, BeatmapsCollection);
            beatmapCollectionReader.Read();

            fpsDisplayVisible = LocalConfig.GetBindable<bool>(RenakoSetting.ShowFPSCounter);
            fpsDisplayVisible.ValueChanged += visible => { FrameStatistics.Value = visible.NewValue ? FrameStatisticsMode.Minimal : FrameStatisticsMode.None; };
            fpsDisplayVisible.TriggerChange();
        }

        public override void SetHost(GameHost host)
        {
            base.SetHost(host);
            Storage = host.Storage;
            LocalConfig ??= DebugUtils.IsDebugBuild
                ? new DevelopmentRenakoConfigManager(Storage)
                : new RenakoConfigManager(Storage);
        }

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) => dependencies = new DependencyContainer(base.CreateChildDependencies(parent));
    }
}

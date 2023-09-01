using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.IO.Stores;
using osuTK;
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

        private DependencyContainer dependencies;

        private RenakoTextureStore textureStore;

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

            // Host.Storage.PresentExternally();

            dependencies.Cache(textureStore = new RenakoTextureStore(Host.Renderer, Host.CreateTextureLoaderStore(new NamespacedResourceStore<byte[]>(Resources, "Textures"))));
            dependencies.CacheAs(this);
        }

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) => dependencies = new DependencyContainer(base.CreateChildDependencies(parent));
    }
}

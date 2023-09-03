using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Screens;
using Renako.Game.Graphics.Drawables;

namespace Renako.Game.Graphics.Screens;

public partial class PlayMenuScreen : Screen
{
    [BackgroundDependencyLoader]
    private void load()
    {
        InternalChildren = new Drawable[]
        {
            new BackButton()
            {
                Action = this.Exit
            }
        };
    }
}

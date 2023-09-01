using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Screens;
using osuTK;
using Renako.Game.Graphics.Drawables;

namespace Renako.Game.Graphics.Screens;

/// <summary>
/// Screen that contain only Renako logo for flexible transition in logo.
/// </summary>
public partial class LogoScreen : Screen
{
    public RenakoLogo Logo;

    [BackgroundDependencyLoader]
    private void load()
    {
        InternalChild = Logo = new RenakoLogo()
        {
            Anchor = Anchor.TopLeft,
            Origin = Anchor.Centre,
            RelativePositionAxes = Axes.Both,
            RelativeSizeAxes = Axes.Both,
            Alpha = 0,
            Size = new Vector2(0.1f, 0.1f)
        };
    }
}

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;

namespace Renako.Game.Graphics.Drawables;

public partial class LoadingSpinner : CompositeDrawable
{
    private SpriteIcon icon;

    [BackgroundDependencyLoader]
    private void load()
    {
        InternalChild = new Container()
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            RelativeSizeAxes = Axes.Both,
            Children = new Drawable[]
            {
                icon = new SpriteIcon()
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Colour = Colour4.FromHex("6F5E67"),
                    RelativeSizeAxes = Axes.Both,
                    Icon = FontAwesome.Solid.CircleNotch
                }
            }
        };
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();
        animate();
    }

    private void animate()
    {
        icon.RotateTo(0).Then()
            .RotateTo(360, 1000, Easing.InOutSine)
            .Loop();
    }
}

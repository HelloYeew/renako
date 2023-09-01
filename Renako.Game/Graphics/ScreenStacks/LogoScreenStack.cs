using osu.Framework.Graphics;
using osu.Framework.Screens;
using Renako.Game.Graphics.Screens;

namespace Renako.Game.Graphics.ScreenStacks;

public partial class LogoScreenStack : ScreenStack
{
    public LogoScreen LogoScreenObject;

    public LogoScreenStack()
    {
        RelativeSizeAxes = Axes.Both;
        Push(LogoScreenObject = new LogoScreen());
    }
}

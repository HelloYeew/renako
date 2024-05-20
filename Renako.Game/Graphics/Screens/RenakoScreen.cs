using osu.Framework.Allocation;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK.Input;
using Renako.Game.Graphics.ScreenStacks;

namespace Renako.Game.Graphics.Screens;

/// <summary>
/// An extension of <see cref="Screen"/> that will be used in Renako.
/// </summary>
public partial class RenakoScreen : Screen
{
    [Resolved]
    private SettingsScreenStack settingsScreenStack { get; set; }

    protected override bool OnKeyDown(KeyDownEvent e)
    {
        // TODO: This is temporary, this would be better in SettingsContainer
        if (e.Key == Key.Escape && settingsScreenStack.IsSettingsVisible)
        {
            settingsScreenStack.ToggleVisibility();
            return true;
        }

        return base.OnKeyDown(e);
    }
}

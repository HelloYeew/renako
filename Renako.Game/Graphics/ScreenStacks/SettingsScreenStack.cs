using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK.Input;
using Renako.Game.Graphics.Containers;
using Renako.Game.Graphics.Screens;

namespace Renako.Game.Graphics.ScreenStacks;

/// <summary>
/// Screenstack for add settings container.
/// </summary>
public partial class SettingsScreenStack : ScreenStack
{
    private SettingsContainer settingsContainer;

    [Resolved]
    private RenakoScreenStack mainScreenStack { get; set; }

    [BackgroundDependencyLoader]
    private void load()
    {
        InternalChild = settingsContainer = new SettingsContainer
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            RelativeSizeAxes = Axes.Both,
        };
    }

    protected override bool OnKeyDown(KeyDownEvent e)
    {
        if (mainScreenStack.CurrentScreen is WarningScreen || mainScreenStack.CurrentScreen is StartScreen) return base.OnKeyDown(e);

        // ctrl + o to open settings.
        if (e.ControlPressed && e.Key == Key.O)
        {
            settingsContainer.ToggleVisibility();
        }

        return base.OnKeyDown(e);
    }
}

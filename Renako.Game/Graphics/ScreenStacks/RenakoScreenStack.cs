using osu.Framework.Bindables;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK.Input;
using Renako.Game.Graphics.Screens;

namespace Renako.Game.Graphics.ScreenStacks;

public partial class RenakoScreenStack : ScreenStack
{
    public Bindable<IScreen> BindableCurrentScreen { get; } = new Bindable<IScreen>();

    protected override bool OnKeyDown(KeyDownEvent e)
    {
        if (e.Key == Key.Escape && CurrentScreen is not MainMenuScreen && CurrentScreen is not WarningScreen && CurrentScreen is not SongSelectionScreen)
            CurrentScreen?.Exit();

        return base.OnKeyDown(e);
    }

    protected override void Update()
    {
        base.Update();
        BindableCurrentScreen.Value = CurrentScreen;
    }
}

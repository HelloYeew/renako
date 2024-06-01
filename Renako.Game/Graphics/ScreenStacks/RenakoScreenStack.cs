using osu.Framework.Bindables;
using osu.Framework.Logging;
using osu.Framework.Screens;

namespace Renako.Game.Graphics.ScreenStacks;

public partial class RenakoScreenStack : ScreenStack
{
    public Bindable<IScreen> BindableCurrentScreen { get; } = new Bindable<IScreen>();

    protected override void Update()
    {
        base.Update();
        BindableCurrentScreen.Value = CurrentScreen;
    }

    protected override void LoadComplete()
    {
        BindableCurrentScreen.BindValueChanged(e =>
        {
            Logger.Log("🖥️ Current screen changed from " + e.OldValue?.GetType().Name + " to " + e.NewValue?.GetType().Name);
        });
        base.LoadComplete();
    }
}

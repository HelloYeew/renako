using osu.Framework.Bindables;
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
}

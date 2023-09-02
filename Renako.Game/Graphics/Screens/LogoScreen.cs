﻿using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Screens;
using osuTK;
using Renako.Game.Graphics.Drawables;
using Renako.Game.Graphics.ScreenStacks;

namespace Renako.Game.Graphics.Screens;

/// <summary>
/// Screen that contain only Renako logo for flexible transition in logo.
/// </summary>
public partial class LogoScreen : Screen
{
    public RenakoLogo Logo;

    [Resolved]
    private RenakoScreenStack mainScreenStack { get; set; }

    public LogoScreen()
    {
        InternalChild = Logo = new RenakoLogo()
        {
            Anchor = Anchor.TopLeft,
            Origin = Anchor.Centre,
            RelativePositionAxes = Axes.Both,
            Alpha = 0
        };
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        mainScreenStack.BindableCurrentScreen.BindValueChanged((e) => changeLogoLocationOnScreenChanged(e.OldValue, e.NewValue));
    }

    /// <summary>
    /// Change logo location on screen changed.
    /// </summary>
    /// <param name="oldScreen">The old <see cref="IScreen"/> before change.</param>
    /// <param name="newScreen">The new <see cref="IScreen"/> that will be changed.</param>
    private void changeLogoLocationOnScreenChanged(IScreen oldScreen, IScreen newScreen)
    {
        // WarningScreen -> StartScreen : Move logo from top.
        if (oldScreen is WarningScreen && newScreen is StartScreen)
        {
            Logo.Position = new Vector2(0.5f, -0.15f);
            Logo.Alpha = 1;
            Logo.MoveTo(new Vector2(0.5f, 0.15f), 750, Easing.OutCubic);
        }
        else if (oldScreen is StartScreen && newScreen is MainMenuScreen)
        {
            MoveToMainMenu();
        }
    }

    /// <summary>
    /// Run the logo animation to move to main menu.
    /// </summary>
    public void MoveToMainMenu()
    {
        Logo.Alpha = 1;
        Logo.MoveTo(new Vector2(0.1f, 0.075f), 500, Easing.InOutCirc);
    }
}

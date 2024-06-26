﻿using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK;
using osuTK.Input;
using Renako.Game.Graphics.ScreenStacks;
using Renako.Game.Graphics.UserInterface;

namespace Renako.Game.Graphics.Screens;

public partial class PlayMenuScreen : Screen
{
    private FillFlowContainer menuContainer;
    private Sample clickSample;

    [Resolved]
    private RenakoScreenStack mainScreenStack { get; set; }

    [BackgroundDependencyLoader]
    private void load(AudioManager audioManager)
    {
        Alpha = 0;
        InternalChildren = new Drawable[]
        {
            menuContainer = new FillFlowContainer()
            {
                Anchor = Anchor.TopLeft,
                Origin = Anchor.TopLeft,
                RelativeSizeAxes = Axes.Both,
                Direction = FillDirection.Vertical,
                Spacing = new Vector2(0, 28),
                Position = new Vector2(-600, 0),
                Size = new Vector2(1f, 0.65f),
                Children = new Drawable[]
                {
                    // Play
                    new MenuTitle()
                    {
                        ButtonWidth = 0.45f,
                        BackgroundColor = Color4Extensions.FromHex("F2DFE9"),
                        TitleColor = Color4Extensions.FromHex("67344D"),
                        DescriptionColor = Color4Extensions.FromHex("251319"),
                        Title = "Play".ToUpper(),
                        Description = "Let's have some fun!"
                    },
                    // Single player
                    new MenuButton()
                    {
                        ButtonWidth = 0.4f,
                        BackgroundColor = Color4Extensions.FromHex("E0BCD5"),
                        IconColor = Color4Extensions.FromHex("512945"),
                        TitleColor = Color4Extensions.FromHex("6C375C"),
                        DescriptionColor = Color4Extensions.FromHex("261321"),
                        Icon = FontAwesome.Solid.User,
                        Title = "Single Player".ToUpper(),
                        Description = "Quickly recap the story with the beat",
                        Action = toggleSinglePlayerButton
                    },
                    // Multiplayer
                    new MenuButton()
                    {
                        ButtonWidth = 0.35f,
                        BackgroundColor = Color4Extensions.FromHex("E5DBEE"),
                        IconColor = Color4Extensions.FromHex("3D2951"),
                        TitleColor = Color4Extensions.FromHex("563973"),
                        DescriptionColor = Color4Extensions.FromHex("1E1528"),
                        Icon = FontAwesome.Solid.Users,
                        Title = "Multiplayer".ToUpper(),
                        Description = "Compete or co-op in story"
                    },
                    // Story
                    new MenuButton()
                    {
                        ButtonWidth = 0.30f,
                        BackgroundColor = Color4Extensions.FromHex("DBE4EE"),
                        IconColor = Color4Extensions.FromHex("293951"),
                        TitleColor = Color4Extensions.FromHex("394D73"),
                        DescriptionColor = Color4Extensions.FromHex("151F28"),
                        Icon = FontAwesome.Solid.BookOpen,
                        Title = "Story".ToUpper(),
                        Description = "Full story experience"
                    }
                }
            },
            new BackButton()
            {
                Action = this.Exit
            }
        };

        clickSample = audioManager.Samples.Get("UI/click-enter2");
    }

    /// <summary>
    /// Toggle single player button.
    /// </summary>
    private void toggleSinglePlayerButton()
    {
        clickSample?.Play();
        mainScreenStack.Push(new SongSelectionScreen());
    }

    public override void OnEntering(ScreenTransitionEvent e)
    {
        this.FadeIn(500, Easing.OutQuart);
        menuContainer.MoveToX(-MenuButton.CONTAINER_PADDING, 500, Easing.OutQuart);

        base.OnEntering(e);
    }

    public override void OnSuspending(ScreenTransitionEvent e)
    {
        this.FadeOut(500, Easing.OutQuart);
        menuContainer.MoveToX(-600, 500, Easing.OutQuart);

        base.OnSuspending(e);
    }

    public override void OnResuming(ScreenTransitionEvent e)
    {
        this.FadeIn(500, Easing.OutQuart);
        menuContainer.MoveToX(-MenuButton.CONTAINER_PADDING, 500, Easing.OutQuart);

        base.OnResuming(e);
    }

    public override bool OnExiting(ScreenExitEvent e)
    {
        this.FadeOut(500, Easing.OutQuart);
        menuContainer.MoveToX(-600, 500, Easing.OutQuart);
        return base.OnExiting(e);
    }

    protected override bool OnKeyDown(KeyDownEvent e)
    {
        if (e.Key == Key.P)
            toggleSinglePlayerButton();
        else if (e.Key == Key.Escape)
            this.Exit();

        return base.OnKeyDown(e);
    }
}

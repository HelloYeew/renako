using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Bindables;
using osu.Framework.Configuration;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Localisation;
using osu.Framework.Platform;
using osuTK;
using Renako.Game.Configurations;
using Renako.Game.Graphics.UserInterface;

namespace Renako.Game.Graphics.Containers;

/// <summary>
/// A settings overlay container that's block input to drawables behind it.
/// </summary>
public partial class SettingsContainer : FocusedOverlayContainer
{
    private Container menuTitleContainer;
    private FillFlowContainer timeContainer;
    private SpriteText currentTimeText;
    private SpriteText runningTimeText;

    private readonly Bindable<Display> currentDisplay = new Bindable<Display>();

    private BasicDropdown<Display> displayDropdown;

    private readonly DateTime startGameTime = DateTime.Now;

    protected override bool BlockNonPositionalInput => false;
    protected override bool BlockScrollInput => false;

    [BackgroundDependencyLoader]
    private void load(RenakoConfigManager renakoConfigManager, FrameworkConfigManager frameworkConfigManager, Storage storage, GameHost host, AudioManager audioManager)
    {
        IWindow window = host.Window;

        if (window != null)
        {
            currentDisplay.BindTo(window.CurrentDisplayBindable);
            window.DisplaysChanged += onDisplaysChanged;
        }

        Alpha = 0;
        InternalChildren = new Drawable[]
        {
            new Box()
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                Colour = Color4Extensions.FromHex("19191D"),
                Alpha = 0.85f
            },
            menuTitleContainer = new Container()
            {
                Anchor = Anchor.TopLeft,
                Origin = Anchor.TopLeft,
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(1f, 0.225f),
                Position = new Vector2(-600, 0),
                Child = new MenuTitle()
                {
                    ButtonWidth = 0.45f,
                    BackgroundColor = Color4Extensions.FromHex("E0E1F0"),
                    TitleColor = Color4Extensions.FromHex("403F75"),
                    DescriptionColor = Color4Extensions.FromHex("171A2D"),
                    Title = "Settings".ToUpper(),
                    Description = "Get ready for fight!"
                }
            },
            timeContainer = new FillFlowContainer()
            {
                Anchor = Anchor.TopRight,
                Origin = Anchor.TopRight,
                RelativeSizeAxes = Axes.X,
                Direction = FillDirection.Vertical,
                Spacing = new Vector2(0, 16),
                Position = new Vector2(600, 0),
                Margin = new MarginPadding()
                {
                    Top = 90,
                    Right = 24,
                    Bottom = 0,
                    Left = 0
                },
                Children = new Drawable[]
                {
                    new FillFlowContainer()
                    {
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.CentreRight,
                        RelativeSizeAxes = Axes.X,
                        Direction = FillDirection.Horizontal,
                        Spacing = new Vector2(4, 0),
                        Children = new Drawable[]
                        {
                            currentTimeText = new SpriteText()
                            {
                                Anchor = Anchor.CentreRight,
                                Origin = Anchor.CentreRight,
                                Text = DateTime.Now.ToString("HH:mm:ss tt").ToUpper(),
                                Font = RenakoFont.GetFont(RenakoFont.Typeface.JosefinSans, 28f, RenakoFont.FontWeight.Bold),
                                Colour = Color4Extensions.FromHex("C4C6EA")
                            },
                            new SpriteIcon()
                            {
                                Anchor = Anchor.CentreRight,
                                Origin = Anchor.CentreRight,
                                Icon = FontAwesome.Solid.Clock,
                                Size = new Vector2(24),
                                Colour = Color4Extensions.FromHex("A7ABE1")
                            }
                        }
                    },
                    runningTimeText = new SpriteText()
                    {
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.CentreRight,
                        Text = "You are fighting the boss for 00:00:00 !",
                        Font = RenakoFont.GetFont(RenakoFont.Typeface.MPlus1P, 20f),
                        Colour = Color4Extensions.FromHex("E0E1F0")
                    }
                }
            },
            new BackButton()
            {
                Action = ToggleVisibility
            },
            new Container()
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(0.7f, 0.5f),
                Masking = true,
                CornerRadius = 16,
                Children = new Drawable[]
                {
                    new Box()
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        RelativeSizeAxes = Axes.Both,
                        Colour = Color4Extensions.FromHex("5F5F66"),
                        Alpha = 0.8f
                    },
                    new BasicScrollContainer()
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        RelativeSizeAxes = Axes.Both,
                        Child = new FillFlowContainer()
                        {
                            Anchor = Anchor.TopLeft,
                            Origin = Anchor.TopLeft,
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y,
                            Direction = FillDirection.Vertical,
                            Spacing = new Vector2(0, 16),
                            Padding = new MarginPadding(15),
                            Children = new Drawable[]
                            {
                                new SpriteText()
                                {
                                    Text = "General".ToUpper(),
                                    Font = RenakoFont.GetFont(RenakoFont.Typeface.JosefinSans, 32f, RenakoFont.FontWeight.Bold),
                                },
                                new SpriteText()
                                {
                                    Text = "Use unicode info"
                                },
                                new BasicCheckbox
                                {
                                    Current = renakoConfigManager.GetBindable<bool>(RenakoSetting.UseUnicodeInfo)
                                },
                                new SpriteText()
                                {
                                    Text = "Display".ToUpper(),
                                    Font = RenakoFont.GetFont(RenakoFont.Typeface.JosefinSans, 32f, RenakoFont.FontWeight.Bold),
                                },
                                new SpriteText()
                                {
                                    Text = "Show FPS"
                                },
                                new BasicCheckbox
                                {
                                    Current = renakoConfigManager.GetBindable<bool>(RenakoSetting.ShowFPSCounter)
                                },
                                new SpriteText()
                                {
                                    Text = "FPS Limit"
                                },
                                new BasicDropdown<FrameSync>
                                {
                                    Width = 300,
                                    Items = Enum.GetValues(typeof(FrameSync)).Cast<FrameSync>(),
                                    Current = frameworkConfigManager.GetBindable<FrameSync>(FrameworkSetting.FrameSync)
                                },
                                new SpriteText()
                                {
                                    Text = "Execution mode"
                                },
                                new BasicDropdown<ExecutionMode>
                                {
                                    Width = 300,
                                    Items = Enum.GetValues(typeof(ExecutionMode)).Cast<ExecutionMode>(),
                                    Current = frameworkConfigManager.GetBindable<ExecutionMode>(FrameworkSetting.ExecutionMode)
                                },
                                new SpriteText()
                                {
                                    Text = "Renderer"
                                },
                                new BasicDropdown<RendererType>
                                {
                                    Width = 300,
                                    Items = host.GetPreferredRenderersForCurrentPlatform().OrderBy(t => t).Where(t => t != RendererType.Vulkan),
                                    Current = frameworkConfigManager.GetBindable<RendererType>(FrameworkSetting.Renderer)
                                },
                                new SpriteText()
                                {
                                    Text = "Window mode",
                                    Alpha = host.Window?.SupportedWindowModes.Count() > 1 ? 1f : 0.5f
                                },
                                new BasicDropdown<WindowMode>()
                                {
                                    Width = 300,
                                    Items = window?.SupportedWindowModes,
                                    Alpha = window?.SupportedWindowModes.Count() > 1 ? 1f : 0.5f,
                                    Current = frameworkConfigManager.GetBindable<WindowMode>(FrameworkSetting.WindowMode)
                                },
                                new SpriteText()
                                {
                                    Text = "Display"
                                },
                                displayDropdown = new DisplaySettingsDropdown()
                                {
                                    Width = 300,
                                    Items = window?.Displays,
                                    Current = currentDisplay
                                },
                                new SpriteText()
                                {
                                    Text = "Audio".ToUpper(),
                                    Font = RenakoFont.GetFont(RenakoFont.Typeface.JosefinSans, 32f, RenakoFont.FontWeight.Bold),
                                },
                                new SpriteText()
                                {
                                    Text = "Master"
                                },
                                new BasicSliderBar<double>()
                                {
                                    Width = 300,
                                    Height = 30,
                                    Current = audioManager.Volume,
                                    KeyboardStep = 0.01f,
                                },
                                new SpriteText()
                                {
                                    Text = "Effect"
                                },
                                new BasicSliderBar<double>()
                                {
                                    Width = 300,
                                    Height = 30,
                                    Current = audioManager.VolumeSample,
                                    KeyboardStep = 0.01f,
                                },
                                new SpriteText()
                                {
                                    Text = "Music"
                                },
                                new BasicSliderBar<double>()
                                {
                                    Width = 300,
                                    Height = 30,
                                    Current = audioManager.VolumeTrack,
                                    KeyboardStep = 0.01f
                                },
                                new SpriteText()
                                {
                                    Text = "Maintenance".ToUpper(),
                                    Font = RenakoFont.GetFont(RenakoFont.Typeface.JosefinSans, 32f, RenakoFont.FontWeight.Bold),
                                },
                                new BasicButton()
                                {
                                    Action = () => storage.PresentExternally(),
                                    Text = "Open Renako folder",
                                    Width = 300,
                                    Height = 30
                                }
                            }
                        }
                    }
                }
            }
        };
    }

    /// <summary>
    /// Play screen enter animation.
    /// </summary>
    public void Enter()
    {
        this.FadeIn(500, Easing.OutQuart);
        menuTitleContainer.MoveToX(-MenuButton.CONTAINER_PADDING, 500, Easing.OutQuart);
        timeContainer.MoveToX(0, 750, Easing.OutQuart);
    }

    /// <summary>
    /// Play screen exit animation.
    /// </summary>
    public void Exit()
    {
        this.FadeOut(500, Easing.OutQuart);
        menuTitleContainer.MoveToX(-600, 500, Easing.OutQuart);
        timeContainer.MoveToX(600, 750, Easing.OutQuart);
    }

    protected override void Update()
    {
        base.Update();

        currentTimeText.Text = DateTime.Now.ToString("HH:mm:ss tt").ToUpper();
        runningTimeText.Text = $"You are fighting the boss for {DateTime.Now - startGameTime:hh\\:mm\\:ss} !";
    }

    protected override void PopIn()
    {
        Enter();
    }

    protected override void PopOut()
    {
        Exit();
    }

    private void onDisplaysChanged(IEnumerable<Display> displays)
    {
        Scheduler.AddOnce(d =>
        {
            if (!displayDropdown.Items.SequenceEqual(d, DisplayListComparer.DEFAULT))
                displayDropdown.Items = d;
        }, displays);
    }

    /// <summary>
    /// The <see cref="Dropdown{T}"/> that extend to show the display name.
    /// </summary>
    private partial class DisplaySettingsDropdown : BasicDropdown<Display>
    {
        protected override LocalisableString GenerateItemText(Display item)
        {
            return $"{item.Index}: {item.Name} ({item.Bounds.Width}x{item.Bounds.Height})";
        }
    }

    /// <summary>
    /// Contrary to <see cref="Display.Equals(osu.Framework.Platform.Display?)"/>, this comparer disregards the value of <see cref="Display.Bounds"/>.
    /// We want to just show a list of displays, and for the purposes of settings we don't care about their bounds when it comes to the list.
    /// However, <see cref="IWindow.DisplaysChanged"/> fires even if only the resolution of the current display was changed
    /// (because it causes the bounds of all displays to also change).
    /// We're not interested in those changes, so compare only the rest that we actually care about.
    /// This helps to avoid a bindable/event feedback loop, in which a resolution change
    /// would trigger a display "change", which would in turn reset resolution again.
    /// </summary>
    private class DisplayListComparer : IEqualityComparer<Display>
    {
        public static readonly DisplayListComparer DEFAULT = new DisplayListComparer();

        public bool Equals(Display x, Display y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;

            return x.Index == y.Index
                   && x.Name == y.Name
                   && x.DisplayModes.SequenceEqual(y.DisplayModes);
        }

        public int GetHashCode(Display obj)
        {
            var hashCode = new HashCode();

            hashCode.Add(obj.Index);
            hashCode.Add(obj.Name);
            hashCode.Add(obj.DisplayModes.Length);
            foreach (var displayMode in obj.DisplayModes)
                hashCode.Add(displayMode);

            return hashCode.ToHashCode();
        }
    }
}

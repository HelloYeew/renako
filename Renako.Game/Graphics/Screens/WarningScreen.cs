using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Screens;
using osuTK;
using Renako.Game.Graphics.ScreenStacks;

namespace Renako.Game.Graphics.Screens;

public partial class WarningScreen : Screen
{
    private Container logoContainer;
    private Container warningTextContainer;
    private SpriteText descriptionText;
    private SpriteText descriptionText2;

    [Resolved]
    private RenakoScreenStack mainScreenStack { get; set; }

    [BackgroundDependencyLoader]
    private void load(TextureStore textureStore)
    {
        InternalChildren = new Drawable[]
        {
            new FillFlowContainer()
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                Direction = FillDirection.Vertical,
                Spacing = new Vector2(0, 10),
                Children = new Drawable[]
                {
                    logoContainer = new Container()
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Size = new Vector2(84),
                        Scale = new Vector2(0),
                        Padding = new MarginPadding()
                        {
                            Bottom = 38
                        },
                        Child = new Sprite()
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Texture = textureStore.Get("renako-logo"),
                            Size = new Vector2(84)
                        }
                    },
                    warningTextContainer = new Container()
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Size = new Vector2(84),
                        Scale = new Vector2(0),
                        Padding = new MarginPadding()
                        {
                            Bottom = 18
                        },
                        Child = new SpriteText()
                        {
                            Font = RenakoFont.GetFont(RenakoFont.Typeface.JosefinSans, size: 48f),
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Colour = Color4Extensions.FromHex("EDDAE4"),
                            Text = "WARNING!"
                        }
                    },
                    descriptionText = new SpriteText()
                    {
                        Font = RenakoFont.GetFont(size: 32f),
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Scale = new Vector2(0),
                        Colour = Color4Extensions.FromHex("E0BCD5"),
                        Text = "This game is under development"
                    },
                    descriptionText2 = new SpriteText()
                    {
                        Font = RenakoFont.GetFont(size: 32f),
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Scale = new Vector2(0),
                        Colour = Color4Extensions.FromHex("E0BCD5"),
                        Text = "If you see any issues please report on GitHub"
                    }
                }
            }
        };
    }

    public override void OnEntering(ScreenTransitionEvent e)
    {
        base.OnEntering(e);

        logoContainer.ScaleTo(1, 500, Easing.OutCubic)
                     .Delay(2000)
                     .RotateTo(-45, 250, Easing.OutCubic)
                     .Then()
                     .RotateTo(0, 250, Easing.OutCubic)
                     .Then()
                     .ScaleTo(0, 150, Easing.OutExpo);
        warningTextContainer.ScaleTo(1, 1000, Easing.OutCubic)
                            .Delay(2500)
                            .ScaleTo(0, 150, Easing.OutExpo);
        descriptionText.ScaleTo(1, 1250, Easing.OutCubic)
                       .Delay(2500)
                       .ScaleTo(0, 150, Easing.OutExpo);
        descriptionText2.ScaleTo(1, 1500, Easing.OutCubic)
                        .Delay(2500)
                        .ScaleTo(0, 150, Easing.OutExpo);

        Scheduler.AddDelayed(() => mainScreenStack.Push(new StartScreen()), 3500);
    }
}

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace Renako.Game.Graphics.UserInterface;

public partial class MenuTitleWithTexture : CompositeDrawable
{
    public float ButtonWidth { get; set; } = 0.45f;
    public ColourInfo BackgroundColor { get; set; } = Color4Extensions.FromHex("F2DFE9");
    public ColourInfo TitleColor { get; set; } = Color4Extensions.FromHex("67344D");
    public ColourInfo DescriptionColor { get; set; } = Color4Extensions.FromHex("251319");

    private readonly Bindable<string> bindableTitle = new Bindable<string>("Play");
    private readonly Bindable<string> bindableDescription = new Bindable<string>("Let's have some fun!");
    private Texture texture;
    public bool AutoUpperCaseTitle { get; set; } = true;

    public const float CONTAINER_PADDING = 20;

    private Box backgroundBox;
    private SpriteText titleSpriteText;
    private SpriteText descriptionSpriteText;
    private Container textureContainer;
    private Sprite textureSprite;

    public string Title
    {
        get => bindableTitle.Value;
        set => bindableTitle.Value = value;
    }

    public string Description
    {
        get => bindableDescription.Value;
        set => bindableDescription.Value = value;
    }

    public Texture Texture
    {
        get => texture;
        set
        {
            texture = value;
            textureSprite.Texture = value;
        }
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        Anchor = Anchor.CentreLeft;
        Origin = Anchor.CentreLeft;
        RelativeSizeAxes = Axes.X;
        Size = new Vector2(ButtonWidth, 80);
        Masking = true;
        CornerRadius = 15;
        Colour = Colour4.White;
        InternalChildren = new Drawable[]
        {
            backgroundBox = new Box()
            {
                Colour = BackgroundColor,
                RelativeSizeAxes = Axes.Both,
                Shear = new Vector2(0.45f, 0f),
            },
            new FillFlowContainer()
            {
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                RelativeSizeAxes = Axes.Both,
                Padding = new MarginPadding()
                {
                    Left = CONTAINER_PADDING * 2,
                    Right = CONTAINER_PADDING,
                    Top = CONTAINER_PADDING,
                    Bottom = CONTAINER_PADDING
                },
                Direction = FillDirection.Horizontal,
                Children = new Drawable[]
                {
                    textureContainer = new Container()
                    {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        Size = new Vector2(60),
                        Masking = true,
                        CornerRadius = 10,
                        Margin = new MarginPadding()
                        {
                            Right = 20
                        },
                        Child = textureSprite = new Sprite()
                        {
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                            Size = new Vector2(60),
                            Texture = Texture
                        }
                    },
                    new FillFlowContainer()
                    {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        RelativeSizeAxes = Axes.X,
                        Direction = FillDirection.Vertical,
                        Children = new Drawable[]
                        {
                            titleSpriteText = new SpriteText()
                            {
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                Font = RenakoFont.GetFont(RenakoFont.Typeface.JosefinSans, 35f, RenakoFont.FontWeight.Bold),
                                Text = AutoUpperCaseTitle ? Title.ToUpper() : Title,
                                Colour = TitleColor
                            },
                            descriptionSpriteText = new SpriteText()
                            {
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                Font = RenakoFont.GetFont(RenakoFont.Typeface.MPlus1P, 23f),
                                Text = Description,
                                Colour = DescriptionColor
                            }
                        }
                    }
                }
            }
        };
        bindableTitle.BindValueChanged((value) =>
        {
            titleSpriteText.Text = AutoUpperCaseTitle ? value.NewValue.ToUpper() : value.NewValue;
        }, true);
        bindableDescription.BindValueChanged((value) =>
        {
            descriptionSpriteText.Text = value.NewValue;
        }, true);
    }

    /// <summary>
    /// Hide the texture container.
    /// </summary>
    public void HideTexture()
    {
        textureContainer.ScaleTo(0, 200, Easing.OutQuint);
    }

    /// <summary>
    /// Show the texture container.
    /// </summary>
    public void ShowTexture()
    {
        textureContainer.ScaleTo(1, 200, Easing.OutQuint);
    }
}

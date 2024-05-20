using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace Renako.Game.Graphics.Drawables;

/// <inheritdoc />
/// <summary>
/// Right-bottom container that displays the beatmap set details when the screen is idle.
/// </summary>
public partial class IdleBeatmapSetDetailContainer : Container
{
    private readonly Sprite idleBeatmapSetCover;
    private readonly SpriteText idleTitleText;
    private readonly SpriteText idleArtistText;
    private readonly SpriteText idleSourceText;

    public IdleBeatmapSetDetailContainer()
    {
        Anchor = Anchor.BottomRight;
        Origin = Anchor.BottomRight;
        Margin = new MarginPadding()
        {
            Right = 20,
            Bottom = 20
        };
        Size = new Vector2(540, 140);
        Alpha = 0;
        Masking = true;
        CornerRadius = 15;
        Children = new Drawable[]
        {
            new Box()
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Color4Extensions.FromHex("F2DFE9"),
                Alpha = 0.75f
            },
            new FillFlowContainer()
            {
                Anchor = Anchor.BottomRight,
                Origin = Anchor.BottomRight,
                Padding = new MarginPadding(20),
                Direction = FillDirection.Horizontal,
                Spacing = new Vector2(20, 0),
                RelativeSizeAxes = Axes.Both,
                Children = new Drawable[]
                {
                    new Container()
                    {
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.CentreRight,
                        Size = new Vector2(100),
                        Masking = true,
                        CornerRadius = 15,
                        Child = idleBeatmapSetCover = new Sprite()
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            RelativeSizeAxes = Axes.Both
                        }
                    },
                    new FillFlowContainer()
                    {
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.CentreRight,
                        Direction = FillDirection.Vertical,
                        Size = new Vector2(400, 100),
                        Spacing = new Vector2(5, 0),
                        Children = new Drawable[]
                        {
                            idleTitleText = new RenakoSpriteText()
                            {
                                Anchor = Anchor.CentreRight,
                                Origin = Anchor.CentreRight,
                                Font = RenakoFont.GetFont(RenakoFont.Typeface.JosefinSans, 35, RenakoFont.FontWeight.Bold),
                                Colour = Color4Extensions.FromHex("67344D")
                            },
                            idleArtistText = new RenakoSpriteText()
                            {
                                Anchor = Anchor.CentreRight,
                                Origin = Anchor.CentreRight,
                                Font = RenakoFont.GetFont(RenakoFont.Typeface.MPlus1P, 23),
                                Colour = Color4Extensions.FromHex("251319")
                            },
                            idleSourceText = new RenakoSpriteText()
                            {
                                Anchor = Anchor.CentreRight,
                                Origin = Anchor.CentreRight,
                                Font = RenakoFont.GetFont(RenakoFont.Typeface.MPlus1P, 15),
                                Colour = Color4Extensions.FromHex("251319")
                            }
                        }
                    }
                }
            }
        };
    }

    /// <summary>
    /// The cover image of the beatmap set.
    /// </summary>
    public Texture CoverImage
    {
        get => idleBeatmapSetCover.Texture;
        set => idleBeatmapSetCover.Texture = value;
    }

    /// <summary>
    /// The title of the beatmap set.
    /// </summary>
    public string Title
    {
        get => idleTitleText.Text.ToString();
        set => idleTitleText.Text = value;
    }

    /// <summary>
    /// The artist of the beatmap set.
    /// </summary>
    public string Artist
    {
        get => idleArtistText.Text.ToString();
        set => idleArtistText.Text = value;
    }

    /// <summary>
    /// The source of the beatmap set.
    /// </summary>
    public string Source
    {
        get => idleSourceText.Text.ToString();
        set => idleSourceText.Text = value;
    }
}

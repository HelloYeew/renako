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
/// Right-bottom container that displays the beatmap set details.
/// </summary>
public partial class RightBottomBeatmapSetDetailContainer : Container
{
    private readonly Sprite beatmapSetCover;
    private readonly SpriteText titleText;
    private readonly SpriteText artistText;
    private readonly SpriteText sourceText;

    public RightBottomBeatmapSetDetailContainer()
    {
        Anchor = Anchor.BottomRight;
        Origin = Anchor.BottomRight;
        Margin = new MarginPadding()
        {
            Right = 20,
            Bottom = 20
        };
        Size = new Vector2(540, 140);
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
                        Child = beatmapSetCover = new Sprite()
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
                            titleText = new RenakoSpriteText()
                            {
                                Anchor = Anchor.CentreRight,
                                Origin = Anchor.CentreRight,
                                Font = RenakoFont.GetFont(RenakoFont.Typeface.JosefinSans, 35, RenakoFont.FontWeight.Bold),
                                Colour = Color4Extensions.FromHex("67344D")
                            },
                            artistText = new RenakoSpriteText()
                            {
                                Anchor = Anchor.CentreRight,
                                Origin = Anchor.CentreRight,
                                Font = RenakoFont.GetFont(RenakoFont.Typeface.MPlus1P, 23),
                                Colour = Color4Extensions.FromHex("251319")
                            },
                            sourceText = new RenakoSpriteText()
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
        get => beatmapSetCover.Texture;
        set => beatmapSetCover.Texture = value;
    }

    /// <summary>
    /// The title of the beatmap set.
    /// </summary>
    public string Title
    {
        get => titleText.Text.ToString();
        set => titleText.Text = value;
    }

    /// <summary>
    /// The artist of the beatmap set.
    /// </summary>
    public string Artist
    {
        get => artistText.Text.ToString();
        set => artistText.Text = value;
    }

    /// <summary>
    /// The source of the beatmap set.
    /// </summary>
    public string Source
    {
        get => sourceText.Text.ToString();
        set => sourceText.Text = value;
    }
}

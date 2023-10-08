using osu.Framework.Graphics.Sprites;

namespace Renako.Game.Graphics;

/// <summary>
/// The <see cref="SpriteText"/> that initialized with default font.
/// </summary>
public partial class RenakoSpriteText : SpriteText
{
    public RenakoSpriteText()
    {
        Font = RenakoFont.GetFont();
    }
}

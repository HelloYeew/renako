using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics.Sprites;

namespace Renako.Game.Graphics.UserInterface;

public partial class BackButton : LeftBottomButton
{
    public BackButton()
    {
        BackgroundColor = Color4Extensions.FromHex("ECC1C1");
        IconColor = Color4Extensions.FromHex("4B2828");
        TextColor = Color4Extensions.FromHex("753F3F");
        Icon = FontAwesome.Solid.ArrowLeft;
        Text = "Back";
    }
}

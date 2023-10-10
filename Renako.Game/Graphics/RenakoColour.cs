using System;
using osu.Framework.Extensions.Color4Extensions;
using osuTK.Graphics;
using Renako.Game.Utilities;

namespace Renako.Game.Graphics;

public class RenakoColour
{
    public static Color4 ForDifficultyLevel(double difficultyLevel) => ColourUtility.SampleFromLinearGradient(new[]
    {
        (0f, Color4Extensions.FromHex("8EE5C8")),
        (4f, Color4Extensions.FromHex("E3E58E")),
        (8f, Color4Extensions.FromHex("E38C8C")),
        (12f, Color4Extensions.FromHex("CE8DE4")),
        (16f, Color4Extensions.FromHex("8D90E4")),
        (16f, Color4Extensions.FromHex("595C9D")),
    }, (float)Math.Round(difficultyLevel, 2, MidpointRounding.AwayFromZero));
}

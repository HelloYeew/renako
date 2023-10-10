using System;
using osu.Framework.Extensions.Color4Extensions;
using osuTK.Graphics;
using Renako.Game.Utilities;

namespace Renako.Game.Graphics;

public class RenakoColour
{
    /// <summary>
    /// Get the colour for a given difficulty level from gradient.
    /// </summary>
    /// <param name="difficultyLevel">The difficulty level to get the colour for.</param>
    /// <returns>A <see cref="Color4"/> for the given difficulty level.</returns>
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

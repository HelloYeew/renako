using System;
using JetBrains.Annotations;
using osu.Framework.Graphics.Sprites;

namespace Renako.Game.Graphics;

public class RenakoFont
{
    /// <summary>
    /// The default font size.
    /// </summary>
    public const float DEFAULT_SIZE = 16;

    /// <summary>
    /// The default font.
    /// </summary>
    public static FontUsage Default => GetFont();

    public static FontUsage MPlus1P => GetFont(Typeface.MPlus1P, weight: FontWeight.Regular);
    public static FontUsage JosefinSans => GetFont(Typeface.JosefinSans, weight: FontWeight.Regular);
    public static FontUsage YuGothic => GetFont(Typeface.YuGothic, weight: FontWeight.Regular);
    public static FontUsage Offside => GetFont(Typeface.Offside, weight: FontWeight.Regular);

    /// <summary>
    /// Retrieve a <see cref="FontUsage"/> with some more specified requirements.
    /// </summary>
    /// <param name="typeface">The font typeface.</param>
    /// <param name="size">The size of the font.</param>
    /// <param name="weight">The font weight.</param>
    /// <param name="italics">Whether the font is italic.</param>
    /// <param name="fixedWidth">Whether all characters should be spaced the same distance apart.</param>
    /// <returns>The <see cref="FontUsage"/></returns>
    public static FontUsage GetFont(Typeface typeface = Typeface.MPlus1P, float size = DEFAULT_SIZE,
                                    FontWeight weight = FontWeight.Regular, bool italics = false, bool fixedWidth = false)
        => new FontUsage(GetFamilyString(typeface), size, GetWeightString(typeface, weight), getItalics(typeface, italics),
            fixedWidth);

    /// <summary>
    /// Return the availability of italics for the specified typeface.
    ///
    /// Will return false if the typeface is not supported, else will return as specified.
    /// </summary>
    /// <param name="typeface">The target typeface of the font.</param>
    /// <param name="italics">Whether want italic or not.</param>
    /// <returns>The boolean value that can use in <see cref="FontUsage"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private static bool getItalics(Typeface typeface, bool italics)
    {
        // This can add some exception when some of our font are not support italics
        switch (typeface)
        {
            case Typeface.MPlus1P:
                return italics;

            case Typeface.JosefinSans:
                return italics;

            case Typeface.YuGothic:
                return italics;

            case Typeface.Offside:
                return italics;

            default:
                throw new ArgumentOutOfRangeException(nameof(typeface), typeface, null);
        }
    }

    /// <summary>
    /// Retrieve the string representation of a <see cref="Typeface"/>
    /// </summary>
    /// <param name="typeface">The <see cref="Typeface"/></param>
    /// <returns>A string representation.</returns>
    public static string GetFamilyString(Typeface typeface)
    {
        switch (typeface)
        {
            case Typeface.MPlus1P:
                return "MPlus1p";

            case Typeface.JosefinSans:
                return "JosefinSans";

            case Typeface.YuGothic:
                return "YuGothic";

            case Typeface.Offside:
                return "Offside";

            default:
                return null;
        }
    }

    /// <summary>
    /// Retrieve the string representation of a <see cref="FontWeight"/>
    /// </summary>
    /// <param name="typeface">The <see cref="Typeface"/>.</param>
    /// <param name="weight">The <see cref="FontWeight"/>.</param>
    /// <returns>The string representation of <paramref name="weight"/> in the specified <paramref name="typeface"/>.</returns>
    [CanBeNull]
    public static string GetWeightString(Typeface typeface, FontWeight weight)
    {
        // Offside font doesn't have any weight
        if (typeface == Typeface.Offside || (typeface == Typeface.JosefinSans && weight == FontWeight.Regular))
            return null;

        return GetWeightString(GetFamilyString(typeface), weight);
    }

    /// <summary>
    /// Retrieve the string representation of a <see cref="FontWeight"/>.
    /// </summary>
    /// <param name="family">The family string.</param>
    /// <param name="weight">The <see cref="FontWeight"/>.</param>
    /// <returns>The string representation of <paramref name="weight"/> in the specified <paramref name="family"/>.</returns>
    public static string GetWeightString(string family, FontWeight weight) => weight.ToString();

    public enum Typeface
    {
        MPlus1P,
        JosefinSans,
        YuGothic,
        Offside
    }

    public enum FontWeight
    {
        Light = 300,
        Regular = 400,
        Medium = 500,
        SemiBold = 600,
        Bold = 700,
        Black = 900
    }
}

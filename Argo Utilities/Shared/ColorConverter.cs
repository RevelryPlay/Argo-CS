using System.Drawing;

namespace Argo_Utilities.Shared;

public struct ColorConverter
{
    // Normalized colors are mapped on a range of 0.0 to 1.0, with 0.0 representing black,
    // and 1.0 representing the largest possible value for that channel.
    /// <summary>
    /// Convert a color from a hex value to something that can be used with OpenGL
    /// </summary>
    /// <param name="hex">String of the hex value as either `AARRGGBB` `RRGGBB` or `RGB`</param>
    public static NormalizedColor HexToNormalizedColor(string hex)
    {
        Color color = ColorTranslator.FromHtml(hex);
        return new(
            (float)color.R / 255.0f,
            (float)color.G / 255.0f,
            (float)color.B / 255.0f,
            (float)color.A / 255.0f
        );
    }
}
using System.Collections;
using System.Drawing;
using Argo_Core.Shared.Core.System.Graphics.UIText;

namespace Argo_Core.Shared.Core.System.Graphics.Font_Rendering;

public class BitmapFont : IEnumerable<Character>
{

    #region Public Member Declarations

    public const int NoMaxWidth = -1;

    #endregion Public Member Declarations

    #region Private Methods

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    #endregion Private Methods

    #region Public Properties

    public int AlphaChannel { get; set; }

    public int BaseHeight { get; set; }

    public int BlueChannel { get; set; }

    public bool Bold { get; set; }

    public IDictionary<char, Character> Characters { get; set; }

    public string? Charset { get; set; }

    public string? FamilyName { get; set; }

    public int FontSize { get; set; }

    public int GreenChannel { get; set; }

    public bool Italic { get; set; }

    public IDictionary<Kerning, int> Kernings { get; set; }

    public int LineHeight { get; set; }

    public int OutlineSize { get; set; }

    public bool Packed { get; set; }

    public Padding Padding { get; set; }

    public Page[] Pages { get; set; }

    public int RedChannel { get; set; }

    public bool Smoothed { get; set; }

    public Point Spacing { get; set; }

    public int StretchedHeight { get; set; }

    public int SuperSampling { get; set; }

    public Size TextureSize { get; set; }

    public Character this[char character]
    {
        get { return Characters[character]; }
    }

    public bool Unicode { get; set; }

    #endregion Public Properties

    #region Public Methods

    public IEnumerator<Character> GetEnumerator()
    {
        foreach (KeyValuePair<char, Character> pair in Characters)
        {
            yield return pair.Value;
        }
    }

    public int GetKerning(char previous, char current)
    {

        Kerning key = new(previous, current, 0);
        if (!Kernings.TryGetValue(key, out int result))
            result = 0;

        return result;
    }

    public Size MeasureFont(string text, double maxWidth = NoMaxWidth)
    {

        char previousCharacter = ' ';
        string normalizedText = NormalizeLineBreaks(text);
        int currentLineWidth = 0;
        int currentLineHeight = LineHeight;
        int blockWidth = 0;
        int blockHeight = 0;
        List<int> lineHeights = new();

        foreach (char character in normalizedText)
        {
            switch (character)
            {
                case '\n':
                    lineHeights.Add(currentLineHeight);
                    blockWidth = Math.Max(blockWidth, currentLineWidth);
                    currentLineWidth = 0;
                    currentLineHeight = LineHeight;
                    break;
                default:

                    Character data = this[character];
                    int width = data.XAdvance + GetKerning(previousCharacter, character);

                    if (maxWidth != NoMaxWidth && currentLineWidth + width >= maxWidth)
                    {
                        lineHeights.Add(currentLineHeight);
                        blockWidth = Math.Max(blockWidth, currentLineWidth);
                        currentLineWidth = 0;
                        currentLineHeight = LineHeight;
                    }

                    currentLineWidth += width;
                    currentLineHeight = Math.Max(currentLineHeight, data.Bounds.Height + data.Offset.Y);
                    previousCharacter = character;
                    break;
            }
        }

        // finish off the current line if required
        if (currentLineHeight != 0)
            lineHeights.Add(currentLineHeight);

        // reduce any lines other than the last back to the base
        for (int i = 0; i < lineHeights.Count - 1; i++)
            lineHeights[i] = LineHeight;

        // calculate the final block height
        foreach (int lineHeight in lineHeights)
            blockHeight += lineHeight;

        return new(Math.Max(currentLineWidth, blockWidth), blockHeight);
    }

    public string NormalizeLineBreaks(string s)
    {
        return s.Replace("\r\n", "\n").Replace("\r", "\n");
    }

    #endregion Public Methods

}
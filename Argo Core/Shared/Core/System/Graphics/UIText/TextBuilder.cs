using System.Drawing;
using Argo_Core.Shared.Core.System.Graphics.Font_Rendering;

namespace Argo_Core.Shared.Core.System.Graphics.UIText;

public class TextBuilder
{
    readonly Dictionary<string, BitmapFont> _loadedFonts = new ();
    readonly Dictionary<string, TextProperties> _addedStrings = new();

    public TextBuilder()
    {
    }

    public string AddText(string text, string fontName, float fontSize, Point position, double maxWidth = -1)
    {
        LoadFontFiles(fontName);

        BitmapFont font = _loadedFonts[fontName];
        string id = Guid.NewGuid().ToString();

        TextProperties properties = new()
        {
            Text = text,
            FontName = fontName,
            FontSize = fontSize,
            Position = position,
            Size = font.MeasureFont(text, maxWidth),
            MaxWidth = maxWidth
        };

        _addedStrings.Add(id, properties);

        return id;
    }

    public void RemoveText(string id)
    {
        _addedStrings.Remove(id);
    }

    public Dictionary<string, TextProperties> GetAddedText()
    {
        return _addedStrings;
    }

    private void LoadFontFiles(string fontName)
    {
        if (_loadedFonts.ContainsKey(fontName))
            return;

        // Load the font definition
        BitmapFont font = BitmapFontLoader.LoadFontFromTextFile($"Shared/Fonts/{fontName}.fnt") ?? throw new InvalidOperationException();
        _loadedFonts.Add(fontName, font);

        // Load the font textures

    }
}

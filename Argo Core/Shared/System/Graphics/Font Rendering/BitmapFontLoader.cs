using System.Diagnostics;
using System.Drawing;
using System.Xml;
using Argo_Core.Shared.System.Graphics.UIText;

namespace Argo_Core.Shared.System.Graphics.Font_Rendering;

public class BitmapFontLoader
{

    #region Public Class Methods

    /// <summary>
    ///     Loads a bitmap font from a file, attempting to auto detect the file type
    /// </summary>
    /// <param name="fileName">Name of the file to load.</param>
    /// <returns></returns>
    public static BitmapFont? LoadFontFromFile(string fileName)
    {
        BitmapFont? result;

        if (string.IsNullOrEmpty(fileName))
            throw new ArgumentNullException(nameof(fileName), "File name not specified");
        if (!File.Exists(fileName))
            throw new FileNotFoundException($"Cannot find file '{fileName}'", fileName);

        using (FileStream file = File.OpenRead(fileName))
        {
            using (TextReader reader = new StreamReader(file))
            {

                string? line = reader.ReadLine();

                if (line != null && line.StartsWith("info "))
                    result = LoadFontFromTextFile(fileName);
                else if (line != null && line.StartsWith("<?xml"))
                    result = LoadFontFromXmlFile(fileName);
                else
                    throw new InvalidDataException("Unknown file format.");
            }
        }

        return result;
    }

    /// <summary>
    ///     Loads a bitmap font from a text file.
    /// </summary>
    /// <param name="fileName">Name of the file to load.</param>
    /// <returns></returns>
    public static BitmapFont? LoadFontFromTextFile(string fileName)
    {

        if (string.IsNullOrEmpty(fileName))
            throw new ArgumentNullException(nameof(fileName), "File name not specified");
        if (!File.Exists(fileName))
            throw new FileNotFoundException($"Cannot find file '{fileName}'", fileName);

        IDictionary<int, Page> pageData = new SortedDictionary<int, Page>();
        IDictionary<Kerning, int> kerningDictionary = new Dictionary<Kerning, int>();
        IDictionary<char, Character> charDictionary = new Dictionary<char, Character>();
        BitmapFont? font = new();

        string? resourcePath = Path.GetDirectoryName(fileName);
        string[] lines = File.ReadAllLines(fileName);

        foreach (string line in lines)
        {

            string[] parts = Split(line, ' ');

            if (parts.Length != 0)
            {
                switch (parts[0])
                {
                    case "info":
                        font.FamilyName = GetNamedString(parts, "face");
                        font.FontSize = GetNamedInt(parts, "size");
                        font.Bold = GetNamedBool(parts, "bold");
                        font.Italic = GetNamedBool(parts, "italic");
                        font.Charset = GetNamedString(parts, "charset");
                        font.Unicode = GetNamedBool(parts, "unicode");
                        font.StretchedHeight = GetNamedInt(parts, "stretchH");
                        font.Smoothed = GetNamedBool(parts, "smooth");
                        font.SuperSampling = GetNamedInt(parts, "aa");
                        font.Padding = ParsePadding(GetNamedString(parts, "padding"));
                        font.Spacing = ParsePoint(GetNamedString(parts, "spacing"));
                        font.OutlineSize = GetNamedInt(parts, "outline");
                        break;
                    case "common":
                        font.LineHeight = GetNamedInt(parts, "lineHeight");
                        font.BaseHeight = GetNamedInt(parts, "base");
                        font.TextureSize = new(
                            GetNamedInt(parts, "scaleW"),
                            GetNamedInt(parts, "scaleH")
                        );
                        font.Packed = GetNamedBool(parts, "packed");
                        font.AlphaChannel = GetNamedInt(parts, "alphaChnl");
                        font.RedChannel = GetNamedInt(parts, "redChnl");
                        font.GreenChannel = GetNamedInt(parts, "greenChnl");
                        font.BlueChannel = GetNamedInt(parts, "blueChnl");
                        break;

                    case "page":

                        int id = GetNamedInt(parts, "id");
                        string? name = GetNamedString(parts, "file");
                        string textureId = Path.GetFileNameWithoutExtension(name) ?? string.Empty;

                        if (resourcePath != null && name != null)
                            pageData.Add(id, new(id, Path.Combine(resourcePath, name)));
                        break;

                    case "char":

                        Character charData = new()
                        {
                            Char = (char)GetNamedInt(parts, "id"),
                            Bounds = new(
                                GetNamedInt(parts, "x"),
                                GetNamedInt(parts, "y"),
                                GetNamedInt(parts, "width"),
                                GetNamedInt(parts, "height")
                            ),
                            Offset = new(
                                GetNamedInt(parts, "xoffset"),
                                GetNamedInt(parts, "yoffset")
                            ),
                            XAdvance = GetNamedInt(parts, "xadvance"),
                            TexturePage = GetNamedInt(parts, "page"),
                            Channel = GetNamedInt(parts, "chnl")
                        };
                        charDictionary.Add(charData.Char, charData);
                        break;
                    case "kerning":

                        Kerning key = new((char)GetNamedInt(parts, "first"), (char)GetNamedInt(parts, "second"), GetNamedInt(parts, "amount"));

                        if (!kerningDictionary.ContainsKey(key))
                            kerningDictionary.Add(key, key.Amount);
                        break;
                }
            }
        }

        font.Pages = ToArray(pageData.Values);
        font.Characters = charDictionary;
        font.Kernings = kerningDictionary;

        return font;
    }

    /// <summary>
    ///     Loads a bitmap font from an XML file.
    /// </summary>
    /// <param name="fileName">Name of the file to load.</param>
    /// <returns></returns>
    public static BitmapFont? LoadFontFromXmlFile(string fileName)
    {

        if (string.IsNullOrEmpty(fileName))
            throw new ArgumentNullException(nameof(fileName), "File name not specified");
        if (!File.Exists(fileName))
            throw new FileNotFoundException($"Cannot find file '{fileName}'", fileName);

        XmlDocument document = new();
        IDictionary<int, Page> pageData = new SortedDictionary<int, Page>();
        IDictionary<Kerning, int> kerningDictionary = new Dictionary<Kerning, int>();
        IDictionary<char, Character> charDictionary = new Dictionary<char, Character>();
        BitmapFont? font = new();

        string? resourcePath = Path.GetDirectoryName(fileName);
        document.Load(fileName);
        document.Load(fileName);
        if (document.DocumentElement != null)
        {
            XmlNode root = document.DocumentElement;

            // load the basic attributes
            XmlNode? properties = root.SelectSingleNode("info");
            if (properties is { Attributes: { } })
            {
                font.FamilyName = properties.Attributes["face"]?.Value;
                font.FontSize = Convert.ToInt32(properties.Attributes["size"]?.Value);
                font.Bold = Convert.ToInt32(properties.Attributes["bold"]?.Value) != 0;
                font.Italic = Convert.ToInt32(properties.Attributes["italic"]?.Value) != 0;
                font.Unicode = Convert.ToInt32(properties.Attributes["unicode"]?.Value) != 0;
                font.StretchedHeight = Convert.ToInt32(properties.Attributes["stretchH"]?.Value);
                font.Charset = properties.Attributes["charset"]?.Value;
                font.Smoothed = Convert.ToInt32(properties.Attributes["smooth"]?.Value) != 0;
                font.SuperSampling = Convert.ToInt32(properties.Attributes["aa"]?.Value);
                font.Padding = ParsePadding(properties.Attributes["padding"]?.Value);
                font.Spacing = ParsePoint(properties.Attributes["spacing"]?.Value);
                font.OutlineSize = Convert.ToInt32(properties.Attributes["outline"]?.Value);
            }

            // common attributes
            properties = root.SelectSingleNode("common");
            if (properties is { Attributes: { } })
            {
                font.BaseHeight = Convert.ToInt32(properties.Attributes["lineHeight"]?.Value);
                font.LineHeight = Convert.ToInt32(properties.Attributes["base"]?.Value);
                font.TextureSize = new(
                    Convert.ToInt32(properties.Attributes["scaleW"]?.Value),
                    Convert.ToInt32(properties.Attributes["scaleH"]?.Value)
                );
                font.Packed = Convert.ToInt32(properties.Attributes["packed"]?.Value) != 0;
                font.AlphaChannel = Convert.ToInt32(properties.Attributes["alphaChnl"]?.Value);
                font.RedChannel = Convert.ToInt32(properties.Attributes["redChnl"]?.Value);
                font.GreenChannel = Convert.ToInt32(properties.Attributes["greenChnl"]?.Value);
                font.BlueChannel = Convert.ToInt32(properties.Attributes["blueChnl"]?.Value);
            }

            // load texture information
            XmlNodeList? list = root.SelectNodes("pages/page")!;
            for (int index = 0; index < list.Count; index++)
            {
                XmlNode? node = list[index];
                if (node is not { Attributes: { } })
                    continue;

                Page page = new(
                    Convert.ToInt32(node.Attributes["id"]?.Value),
                    Path.Combine(resourcePath!, node.Attributes["file"]?.Value!)
                );

                pageData.Add(page.Id, page);
            }
            font.Pages = ToArray(pageData.Values);

            // load character information
            foreach (XmlNode node in root.SelectNodes("chars/char")!)
            {

                Character character = new();
                if (node.Attributes != null)
                {
                    character.Char = (char)Convert.ToInt32(node.Attributes["id"]?.Value);
                    character.Bounds = new(
                        Convert.ToInt32(node.Attributes["x"]?.Value),
                        Convert.ToInt32(node.Attributes["y"]?.Value),
                        Convert.ToInt32(node.Attributes["width"]?.Value),
                        Convert.ToInt32(node.Attributes["height"]?.Value)
                    );
                    character.Offset = new(
                        Convert.ToInt32(node.Attributes["xoffset"]?.Value),
                        Convert.ToInt32(node.Attributes["yoffset"]?.Value)
                    );
                    character.XAdvance = Convert.ToInt32(node.Attributes["xadvance"]?.Value);
                    character.TexturePage = Convert.ToInt32(node.Attributes["page"]?.Value);
                    character.Channel = Convert.ToInt32(node.Attributes["chnl"]?.Value);
                }

                charDictionary.Add(character.Char, character);
            }
            font.Characters = charDictionary;

            // loading kerning information
            foreach (XmlNode node in root.SelectNodes("kernings/kerning")!)
            {

                Kerning key = new((char)Convert.ToInt32(node.Attributes?["first"]?.Value),
                    (char)Convert.ToInt32(node.Attributes?["second"]?.Value),
                    Convert.ToInt32(node.Attributes?["amount"]?.Value));

                if (!kerningDictionary.ContainsKey(key))
                    kerningDictionary.Add(key, key.Amount);
            }
        }
        font.Kernings = kerningDictionary;

        return font;
    }

    #endregion Public Class Methods

    #region Private Class Methods

    /// <summary>
    ///     Returns a boolean from an array of name/value pairs.
    /// </summary>
    /// <param name="parts">The array of parts.</param>
    /// <param name="name">The name of the value to return.</param>
    /// <returns></returns>
    static bool GetNamedBool(IEnumerable<string> parts, string name)
    {
        return GetNamedInt(parts, name) != 0;
    }

    /// <summary>
    ///     Returns an integer from an array of name/value pairs.
    /// </summary>
    /// <param name="parts">The array of parts.</param>
    /// <param name="name">The name of the value to return.</param>
    /// <returns></returns>
    static int GetNamedInt(IEnumerable<string> parts, string name)
    {
        try
        {
            return Convert.ToInt32(GetNamedString(parts, name));
        }
        catch (Exception ex)
        {
            // ignored
            Debugger.Log(1, "", ex.ToString());
        }

        return 0;
    }

    /// <summary>
    ///     Returns a string from an array of name/value pairs.
    /// </summary>
    /// <param name="parts">The array of parts.</param>
    /// <param name="name">The name of the value to return.</param>
    /// <returns></returns>
    static string? GetNamedString(IEnumerable<string> parts, string name)
    {

        string? result = string.Empty;
        name = name.ToLowerInvariant();

        foreach (string part in parts)
        {

            int nameEndIndex = part.IndexOf("=");
            if (nameEndIndex == -1)
                continue;

            string namePart = part[..nameEndIndex].ToLowerInvariant();
            string? valuePart = part[(nameEndIndex + 1)..];

            if (namePart != name)
                continue;

            if (valuePart.StartsWith("\"") && valuePart.EndsWith("\""))
                valuePart = valuePart.Substring(1, valuePart.Length - 2);

            result = valuePart;
            break;
        }

        return result;
    }

    /// <summary>
    ///     Creates a Padding object from a string representation
    /// </summary>
    /// <param name="s">The string.</param>
    /// <returns></returns>
    static Padding ParsePadding(string? s)
    {
        string[] parts = s.Split(',');

        return new(
            Convert.ToInt32(parts[3].Trim()),
            Convert.ToInt32(parts[0].Trim()),
            Convert.ToInt32(parts[1].Trim()),
            Convert.ToInt32(parts[2].Trim())
        );
    }

    /// <summary>
    ///     Creates a Point object from a string representation
    /// </summary>
    /// <param name="s">The string.</param>
    /// <returns></returns>
    static Point ParsePoint(string? s)
    {

        string[] parts = s.Split(',');

        return new()
        {
            X = Convert.ToInt32(parts[0].Trim()), Y = Convert.ToInt32(parts[1].Trim())
        };
    }

    /// <summary>
    ///     Splits the specified string using a given delimiter, ignoring any instances of the delimiter as part of a quoted
    ///     string.
    /// </summary>
    /// <param name="s">The string to split.</param>
    /// <param name="delimiter">The delimiter.</param>
    /// <returns></returns>
    static string[] Split(string s, char delimiter)
    {
        string[] results;

        if (s.Contains("\""))
        {

            int partStart = -1;
            List<string> parts = new();

            do
            {

                int quoteStart = s.IndexOf("\"", partStart + 1);
                int quoteEnd = s.IndexOf("\"", quoteStart + 1);
                int partEnd = s.IndexOf(delimiter, partStart + 1);

                if (partEnd == -1)
                    partEnd = s.Length;

                bool hasQuotes = quoteStart != -1 && partEnd > quoteStart && partEnd < quoteEnd;
                if (hasQuotes)
                    partEnd = s.IndexOf(delimiter, quoteEnd + 1);

                parts.Add(s.Substring(partStart + 1, partEnd - partStart - 1));

                if (hasQuotes)
                    partStart = partEnd - 1;

                partStart = s.IndexOf(delimiter, partStart + 1);
            } while (partStart != -1);

            results = parts.ToArray();
        }
        else
            results = s.Split(new[]
            {
                delimiter
            }, StringSplitOptions.RemoveEmptyEntries);

        return results;
    }

    /// <summary>
    ///     Converts the given collection into an array
    /// </summary>
    /// <typeparam name="T">Type of the items in the array</typeparam>
    /// <param name="values">The values.</param>
    /// <returns></returns>
    static T[] ToArray<T>(ICollection<T> values)
    {

        T[] result =
            // avoid a forced .NET 3 dependency just for one call to Linq
            new T[values.Count];
        values.CopyTo(result, 0);

        return result;
    }

    #endregion Private Class Methods

}

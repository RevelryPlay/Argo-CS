using System.Drawing;

namespace Argo_Core.Shared.Core.System.Graphics.UIText;

public class Character
{

    #region Public Properties

    public int Channel { get; set; }
    public Rectangle Bounds { get; set; }
    public Point Offset { get; set; }
    public char Char { get; set; }
    public int TexturePage { get; set; }
    public int XAdvance { get; set; }

    #endregion Public Properties

    #region Public Methods

    public Character() { }

    Character(int channel, Rectangle bounds, Point offset, char c, int texturePage, int xAdvance)
    {
        Channel = channel;
        Bounds = bounds;
        Offset = offset;
        Char = c;
        TexturePage = texturePage;
        XAdvance = xAdvance;
    }

    public override string ToString()
    {
        return Char.ToString();
    }

    #endregion Public Methods

}
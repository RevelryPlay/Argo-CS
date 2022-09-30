using System.Drawing;

namespace Argo_Core.Shared.Core.System.Graphics.Font_Rendering;

public class Character
{

    #region Public Methods

    public override string ToString()
    {
        return Char.ToString();
    }

    #endregion Public Methods

    #region Public Properties

    public int Channel { get; set; }

    public Rectangle Bounds { get; set; }

    public Point Offset { get; set; }

    public char Char { get; set; }

    public int TexturePage { get; set; }

    public int XAdvance { get; set; }

    #endregion Public Properties

}
namespace Argo_Core.Shared.System.Graphics.UIText;

public class Padding
{

    #region Public Constructors

    public Padding(int left, int top, int right, int bottom)
    {
        Top = top;
        Left = left;
        Right = right;
        Bottom = bottom;
    }

    #endregion Public Constructors

    #region Public Methods

    public override string ToString()
    {
        return $"{Left}, {Top}, {Right}, {Bottom}";
    }

    #endregion Public Methods

    #region Public Properties

    public int Top { get; set; }

    public int Left { get; set; }

    public int Right { get; set; }

    public int Bottom { get; set; }

    #endregion Public Properties

}

using System.Drawing;
using Argo_Utilities.Shared.Graphics;

namespace Argo_Core.Shared.Core.System.Graphics.UIText;

public class TextProperties
{
    public string? Text;
    public string? FontName;
    public float FontSize;
    public Point Position;
    public Size Size;
    public double MaxWidth = -1;
    public NormalizedColor Color = new (0, 0, 0);

    public TextProperties() {}
}

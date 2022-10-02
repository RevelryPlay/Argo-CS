using System.Drawing;
using Argo_Utilities.Shared.Graphics;

namespace Argo_Core.Shared.Core.System.Graphics.UIText;

public class TextProperties
{
    public NormalizedColor Color = new(0, 0, 0);
    public string? FontName;
    public float FontSize;
    public double MaxWidth = -1;
    public Point Position;
    public Size Size;
    public string? Text;

}
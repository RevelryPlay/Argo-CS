namespace Argo_Utilities.Shared.Graphics;

public struct NormalizedColor
{
    public readonly float Red;
    public readonly float Green;
    public readonly float Blue;
    public readonly float Alpha;

    public NormalizedColor(float red, float green, float blue, float alpha = 1.0f)
    {
        Red = red;
        Green = green;
        Blue = blue;
        Alpha = alpha;
    }
}
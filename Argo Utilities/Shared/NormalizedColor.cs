namespace Argo_Utilities.Shared;

public struct NormalizedColor
{
    public readonly float Red;
    public readonly float Green;
    public readonly float Blue;
    public readonly float Alpha;

    public NormalizedColor(float red, float green, float blue, float alpha)
    {
        this.Red = red;
        this.Green = green;
        this.Blue = blue;
        this.Alpha = alpha;
    }
}
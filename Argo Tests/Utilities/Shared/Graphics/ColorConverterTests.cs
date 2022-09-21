using System;
using Argo_Utilities.Shared.Graphics;
using Xunit;

namespace Argo_Tests.Utilities.Shared.Graphics;

public class ColorConverterTests
{
    [Fact]
    public void TestNormalizeColorSegments()
    {
        // RRGGBB
        NormalizedColor color = ColorConverter.HexToNormalizedColor("#ff0000");
        Assert.Equal(1.0f, color.Red);
        Assert.Equal(0.0f, color.Green);
        Assert.Equal(0.0f, color.Blue);
        Assert.Equal(1.0f, color.Alpha);
        
        color = ColorConverter.HexToNormalizedColor("#00ff00");
        Assert.Equal(0.0f, color.Red);
        Assert.Equal(1.0f, color.Green);
        Assert.Equal(0.0f, color.Blue);
        Assert.Equal(1.0f, color.Alpha);
        
        color = ColorConverter.HexToNormalizedColor("#0000ff");
        Assert.Equal(0.0f, color.Red);
        Assert.Equal(0.0f, color.Green);
        Assert.Equal(1.0f, color.Blue);
        Assert.Equal(1.0f, color.Alpha);
        
        // AARRGGBB
        color = ColorConverter.HexToNormalizedColor("#00ff0000");
        Assert.Equal(1.0f, color.Red);
        Assert.Equal(0.0f, color.Green);
        Assert.Equal(0.0f, color.Blue);
        Assert.Equal(0.0f, color.Alpha);
    }

    [Fact]
    public void TestNormalizeColorNames()
    {
        NormalizedColor color = ColorConverter.HexToNormalizedColor("white");
        Assert.Equal(1.0f, color.Red);
        Assert.Equal(1.0f, color.Green);
        Assert.Equal(1.0f, color.Blue);
        Assert.Equal(1.0f, color.Alpha);
        
        color = ColorConverter.HexToNormalizedColor("black");
        Assert.Equal(0.0f, color.Red);
        Assert.Equal(0.0f, color.Green);
        Assert.Equal(0.0f, color.Blue);
        Assert.Equal(1.0f, color.Alpha);
    }

    [Fact]
    public void TestNormalizeColorInvalidFormats()
    {
        Assert.Throws<ArgumentException>(() => ColorConverter.HexToNormalizedColor("fish"));
    }
}
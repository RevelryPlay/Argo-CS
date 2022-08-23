using Xunit;
using Argo_Core;

namespace Argo_Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        Class1 cls = new Class1();
        
        Assert.True(cls.testCase());
    }
}
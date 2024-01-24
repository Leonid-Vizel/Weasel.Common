using Weasel.Tools.Extensions.Common;

namespace Weasel.Common.Tests;

[TestClass]
public class StringExtensionsTest
{
    private static readonly string TestingString = "123-123-123-123-123-123-123-123-123-123-123-123-123-123-123";

    [TestMethod]
    public void ShortenLowLength()
    {
        var changed = TestingString.Shorten(10);
        if (changed?.Length > 10)
        {
            throw new Exception("WRONG LENGTH");
        }
    }

    [TestMethod]
    public void ShortenEqualLength()
    {
        var changed = TestingString.Shorten(TestingString.Length);
        if (changed?.Length > TestingString.Length)
        {
            throw new Exception("WRONG LENGTH");
        }
    }

    [TestMethod]
    public void ShortenHighLength()
    {
        var changed = TestingString.Shorten(100000);
        if (changed?.Length > 100000)
        {
            throw new Exception("WRONG LENGTH");
        }
    }
}

using System.ComponentModel.DataAnnotations;
using Weasel.Tools.Extensions.Common;

namespace Weasel.Common.Tests;

public enum StandardEnum
{
    [Display(Name = "CHECK 1")]
    One,
    [Display(Name = "CHECK 2")]
    Two,
    [Display(Name = "CHECK 3")]
    Three,
    Nothing
}

[Flags]
public enum FlagEnum
{
    [Display(Name = "CHECK 1")]
    One,
    [Display(Name = "CHECK 2")]
    Two,
    [Display(Name = "CHECK 3")]
    Three
}

[TestClass]
public class EnumDisplayNameTest
{
    [TestMethod]
    public void DisplayForNothingEnum()
    {
        var test = StandardEnum.Nothing;
        var result = test.GetDisplayName();
        if (result != "Nothing")
        {
            throw new Exception($"WRONG! ({result})");
        }
    }

    [TestMethod]
    public void DisplayForStandardEnum()
    {
        var test = StandardEnum.One;
        var result = test.GetDisplayName();
        if (result != "CHECK 1")
        {
            throw new Exception($"WRONG! ({result})");
        }
    }

    [TestMethod]
    public void DisplayForFlagEnum()
    {
        var test = FlagEnum.One | FlagEnum.Two | FlagEnum.Three;
        var result = test.GetDisplayName();
        if (result != "CHECK 1, CHECK 2, CHECK 3")
        {
            throw new Exception($"WRONG! ({result})");
        }
    }

    [TestMethod]
    public void DisplayForFlagEnumWithCustomSeparator()
    {
        var test = FlagEnum.One | FlagEnum.Two | FlagEnum.Three;
        var result = test.GetDisplayName(" ABOBA ");
        if (result != "CHECK 1 ABOBA CHECK 2 ABOBA CHECK 3")
        {
            throw new Exception($"WRONG! ({result})");
        }
    }
}

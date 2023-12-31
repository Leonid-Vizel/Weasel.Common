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
    Nothing,
}

[Flags]
public enum FlagEnum
{
    Nothing = 0,
    [Display(Name = "CHECK 1")]
    One,
    [Display(Name = "CHECK 2")]
    Two,
    [Display(Name = "CHECK 3")]
    Three
}

[Flags]
public enum LongFlagEnum : long
{
    Nothing = 0,
    [Display(Name = "CHECK 1")]
    One,
    [Display(Name = "CHECK 2")]
    Two,
    [Display(Name = "CHECK 3")]
    Three,
    [Display(Name = "CHECK 4")]
    Four
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
    public void DisplayForFlagEnumTest()
    {
        var test = FlagEnum.One | FlagEnum.Two | FlagEnum.Three;
        var result = test.GetDisplayName();
        if (result != "CHECK 1, CHECK 2, CHECK 3")
        {
            throw new Exception($"WRONG! ({result})");
        }
    }

    [TestMethod]
    public void DisplayForFlagEnumDefaultIncludeTest()
    {
        var test = FlagEnum.One | FlagEnum.Two | FlagEnum.Three;
        var result = test.GetDisplayName(includeDefault: true);
        if (result != "Nothing, CHECK 1, CHECK 2, CHECK 3")
        {
            throw new Exception($"WRONG! ({result})");
        }
    }

    [TestMethod]
    public void FirstDisplayForStandardEnum_1()
    {
        var test = StandardEnum.One;
        var result = test.GetFirstDisplayName();
        if (result != "CHECK 1")
        {
            throw new Exception($"WRONG! ({result})");
        }
    }

    [TestMethod]
    public void FirstDisplayForStandardEnum_2()
    {
        var test = StandardEnum.One;
        var result = test.GetFirstDisplayName(typeof(StandardEnum));
        if (result != "CHECK 1")
        {
            throw new Exception($"WRONG! ({result})");
        }
    }

    [TestMethod]
    public void EnumerationTest_T()
    {
        var test = FlagEnum.One | FlagEnum.Two | FlagEnum.Three;
        var result = test.EnumerateFlags().ToArray();
        if (result[0] != FlagEnum.One || result[1] != FlagEnum.Two || result[2] != FlagEnum.Three)
        {
            throw new Exception($"WRONG! ({result})");
        }
    }

    [TestMethod]
    public void LongTest()
    {
        var test = LongFlagEnum.One | LongFlagEnum.Two | LongFlagEnum.Three;
        var result = test.EnumerateFlags().ToArray();
        if (result[0] != LongFlagEnum.One || result[1] != LongFlagEnum.Two || result[2] != LongFlagEnum.Three)
        {
            throw new Exception($"WRONG! ({result})");
        }
    }

    [TestMethod]
    public void EnumerationIncludeDefaultTest_T()
    {
        var test = FlagEnum.One | FlagEnum.Two | FlagEnum.Three;
        var result = test.EnumerateFlags(includeDefault: true).ToArray();
        if (result[0] != FlagEnum.Nothing || result[1] != FlagEnum.One || result[2] != FlagEnum.Two || result[3] != FlagEnum.Three)
        {
            throw new Exception($"WRONG! ({result})");
        }
    }

    [TestMethod]
    public void EnumerationTest()
    {
        var test = FlagEnum.One | FlagEnum.Two | FlagEnum.Three;

        var result = test.EnumerateFlags(test.GetType()).Cast<FlagEnum>().ToArray();
        if (result[0] != FlagEnum.One || result[1] != FlagEnum.Two || result[2] != FlagEnum.Three)
        {
            throw new Exception($"WRONG! ({result})");
        }
    }

    [TestMethod]
    public void EnumerationIncludeDefaultTest()
    {
        var test = FlagEnum.One | FlagEnum.Two | FlagEnum.Three;
        var result = test.EnumerateFlags(test.GetType(), true).Cast<FlagEnum>().ToArray();
        if (result[0] != FlagEnum.Nothing || result[1] != FlagEnum.One || result[2] != FlagEnum.Two || result[3] != FlagEnum.Three)
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

    [TestMethod]
    public void DisplayForFlagEnumWithCustomSeparatorIncludeDefault()
    {
        var test = FlagEnum.One | FlagEnum.Two | FlagEnum.Three;
        var result = test.GetDisplayName(" ABOBA ", true);
        if (result != "Nothing ABOBA CHECK 1 ABOBA CHECK 2 ABOBA CHECK 3")
        {
            throw new Exception($"WRONG! ({result})");
        }
    }

    [TestMethod]
    public void EnumIsFlagTest1_1()
    {
        if (EnumExtensions.CheckEnumIsFlag(typeof(StandardEnum)))
        {
            throw new Exception("WRONG!");
        }
    }

    [TestMethod]
    public void EnumIsFlagTest1_2()
    {
        if (!EnumExtensions.CheckEnumIsFlag(typeof(FlagEnum)))
        {
            throw new Exception("WRONG!");
        }
    }

    [TestMethod]
    public void EnumIsFlagTest2_1()
    {
        if (EnumExtensions.CheckEnumIsFlag<StandardEnum>())
        {
            throw new Exception("WRONG!");
        }
    }

    [TestMethod]
    public void EnumIsFlagTest2_2()
    {
        if (!EnumExtensions.CheckEnumIsFlag<FlagEnum>())
        {
            throw new Exception("WRONG!");
        }
    }

    [TestMethod]
    public void EnumIsFlagTest3_1()
    {
        if (StandardEnum.One.CheckEnumIsFlag())
        {
            throw new Exception("WRONG!");
        }
    }

    [TestMethod]
    public void EnumIsFlagTest3_2()
    {
        if (!FlagEnum.One.CheckEnumIsFlag())
        {
            throw new Exception("WRONG!");
        }
    }

    [TestMethod]
    public void EnumIsFlagTest3_3()
    {
        if (!(FlagEnum.Two | FlagEnum.Three).CheckEnumIsFlag())
        {
            throw new Exception("WRONG!");
        }
    }
}

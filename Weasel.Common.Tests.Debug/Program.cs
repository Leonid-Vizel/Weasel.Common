using System.ComponentModel.DataAnnotations;

var enumValue = FlagEnum.One | FlagEnum.Two | FlagEnum.Three;

var type = typeof(FlagEnum);
var a = Enum.GetValues(type).Cast<Enum>().Where(enumValue.HasFlag).Select(x => (int)(object)x != 0);
var b = 123;
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
using System.Collections.Concurrent;
using System.Reflection;

namespace Weasel.Tools.Extensions.Common;

public static class EnumExtensions
{
    private static readonly ConcurrentDictionary<Type, bool> _enumFlagData = [];
    private static readonly ConcurrentDictionary<Enum, string[]?> _enumGroupingsData = [];

    #region CheckEnumIsFlag
    public static bool CheckEnumIsFlag(Type type)
        => _enumFlagData.GetOrAdd(type, CheckEnumIsFlagInnerFunction);
    private static bool CheckEnumIsFlagInnerFunction(Type type)
        => type.GetCustomAttribute<FlagsAttribute>() != null;
    public static bool CheckEnumIsFlag<TEnum>()
        => CheckEnumIsFlag(typeof(TEnum));
    public static bool CheckEnumIsFlag(this Enum enumValue)
        => CheckEnumIsFlag(enumValue.GetType());
    #endregion

    #region EnumerateFlags
    public static IEnumerable<TEnum> EnumerateFlags<TEnum>(this TEnum enumValue, bool includeDefault = false) where TEnum : Enum
        => enumValue.EnumerateFlags(typeof(TEnum), includeDefault).Cast<TEnum>();
    public static IEnumerable<Enum> EnumerateFlags(this Enum enumValue, Type type, bool includeDefault = false)
    {
        if (!CheckEnumIsFlag(type))
        {
            return [enumValue];
        }
        var flags = Enum.GetValues(type).Cast<Enum>().Where(enumValue.HasFlag);
        if (!includeDefault)
        {
            flags = flags.Where(x => Convert.ToInt64(x) != 0);
        }
        return flags;
    }
    #endregion

    #region GetGroupings
    public static string[]? GetGroupings(this Enum enumValue)
    {
        if (!_enumGroupingsData.TryGetValue(enumValue, out var result))
        {
            result = enumValue.GetType()
                        .GetMember(enumValue.ToString())
                        .FirstOrDefault()?
                        .GetCustomAttribute<EnumGroupingAttribute>()?
                        .Grouping;
            _enumGroupingsData.TryAdd(enumValue, result);
        }
        return result;
    }
    #endregion
}

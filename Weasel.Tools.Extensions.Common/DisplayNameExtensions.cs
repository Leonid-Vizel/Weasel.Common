using System.Collections.Concurrent;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace Weasel.Tools.Extensions.Common;

[AttributeUsage(AttributeTargets.Field)]
public sealed class EnumGroupingAttribute : Attribute
{
    public string[] Grouping { get; set; }
    public EnumGroupingAttribute(params string[] grouping)
    {
        Grouping = grouping;
    }
}

public static class DisplayNameExtensions
{
    private static readonly ConcurrentDictionary<Enum, string?> _enumNamesData;
    private static readonly ConcurrentDictionary<Enum, string[]?> _enumGroupingsData;
    private static readonly ConcurrentDictionary<Type, bool> _enumFlagData;
    private static readonly ConcurrentDictionary<(Type, string), string?> _propertyNamesData;
    private static readonly ConcurrentDictionary<Type, string?> _typeNamesData;
    static DisplayNameExtensions()
    {
        _enumNamesData = new ConcurrentDictionary<Enum, string?>();
        _enumGroupingsData = new ConcurrentDictionary<Enum, string[]?>();
        _enumFlagData = new ConcurrentDictionary<Type, bool>();
        _propertyNamesData = new ConcurrentDictionary<(Type, string), string?>();
        _typeNamesData = new ConcurrentDictionary<Type, string?>();
    }

    #region Enums
    public static string? GetDisplayName(this Enum enumValue, string separator = ", ", bool includeDefault = false)
    {
        var type = enumValue.GetType();
        var enums = enumValue
            .EnumerateFlags(type, includeDefault)
            .Select(x => x.GetFirstDisplayName(type));
        return string.Join(separator, enums);
    }

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

    public static string? GetFirstDisplayName(this Enum enumValue)
        => enumValue.GetFirstDisplayName(enumValue.GetType());

    public static string? GetFirstDisplayName(this Enum enumValue, Type type)
    {
        if (!_enumNamesData.TryGetValue(enumValue, out var result))
        {
            var preResult = type.GetMember(enumValue.ToString()).FirstOrDefault(x => x.DeclaringType?.IsEnum ?? false);
            result = preResult?.GetCustomAttribute<DisplayAttribute>()?.GetName() ?? preResult?.Name;
            _enumNamesData.TryAdd(enumValue, result);
        }
        return result;
    }

    public static bool CheckEnumIsFlag(Type type)
    {
        if (!_enumFlagData.TryGetValue(type, out bool isFlag))
        {
            isFlag = type.GetCustomAttribute<FlagsAttribute>() != null;
            _enumFlagData.TryAdd(type, isFlag);
        }
        return isFlag;
    }

    public static bool CheckEnumIsFlag<TEnum>()
        => CheckEnumIsFlag(typeof(TEnum));

    public static bool CheckEnumIsFlag(this Enum enumValue)
        => CheckEnumIsFlag(enumValue.GetType());

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
    public static string GetDisplayNameNonNull(this Enum enumValue, string nullValue = "")
        => enumValue.GetDisplayName() ?? nullValue;
    #endregion

    #region Properties
    public static string? GetDisplayName(this object obj, string propertyName)
        => obj.GetType().GetDisplayName(propertyName);

    public static string GetDisplayNameNonNull(this object obj, string propertyName, string nullValue = "")
        => obj.GetType().GetDisplayName(propertyName) ?? nullValue;

    public static string? GetDisplayName<T>(string propertyName)
        => typeof(T).GetDisplayName(propertyName);

    public static string? GetDisplayName<T>(Expression<Func<T, object?>> lambda)
        => lambda.GetPropertyInfo()?.GetDisplayName();

    public static string GetDisplayNameNonNull<T>(string propertyName, string nullValue = "")
        => typeof(T).GetDisplayName(propertyName) ?? nullValue;

    public static string GetDisplayNameNonNull<T>(Expression<Func<T, object?>> lambda, string nullValue = "")
        => lambda.GetPropertyInfo()?.GetDisplayName() ?? nullValue;

    public static string? GetDisplayName(this Type objType, string propertyName)
    {
        var key = (objType, propertyName);
        if (!_propertyNamesData.TryGetValue(key, out string? name))
        {
            var propInfo = objType.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            if (propInfo == null)
            {
                propInfo = objType.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);
                if (propInfo == null)
                {
                    throw new Exception($"Property named {propertyName} not found in {objType.Name}!");
                }
            }
            name = propInfo.GetDisplayName();
            _propertyNamesData.TryAdd(key, name);
        }
        return name;
    }

    public static PropertyInfo? GetPropertyInfo<T, TValue>(this Expression<Func<T, TValue>> lambda)
    {
        MemberExpression? memberExpression;
        if (lambda.Body is UnaryExpression unaryExpression)
        {
            memberExpression = unaryExpression.Operand as MemberExpression;
        }
        else
        {
            memberExpression = lambda.Body as MemberExpression;
        }
        return memberExpression?.Member as PropertyInfo;
    }

    public static string? GetDisplayName(this PropertyInfo propInfo)
        => propInfo.GetCustomAttributes<DisplayNameAttribute>()?.FirstOrDefault()?.DisplayName;

    public static string GetDisplayNameNonNull(this PropertyInfo propInfo, string nullValue = "")
        => propInfo.GetDisplayName() ?? nullValue;
    #endregion

    #region Types
    public static string? GetDisplayName(this object obj)
        => obj.GetType().GetDisplayName();

    public static string? GetDisplayName<T>()
        => typeof(T).GetDisplayName();

    public static string? GetDisplayName(this Type objType)
    {
        if (!_typeNamesData.TryGetValue(objType, out string? name))
        {
            name = objType.GetCustomAttributes<DisplayNameAttribute>()?.FirstOrDefault()?.DisplayName;
            _typeNamesData.TryAdd(objType, name);
        }
        return name;
    }
    #endregion
}

using System.Collections.Concurrent;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
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
    private static readonly ConcurrentDictionary<Enum, string?> _enumNamesData = [];
    private static readonly ConcurrentDictionary<(Type, string), string?> _propertyNamesData = [];
    private static readonly ConcurrentDictionary<Type, string?> _typeNamesData = [];

    #region Enums
    public static string? GetDisplayName(this Enum enumValue, string separator = ", ", bool includeDefault = false)
    {
        var type = enumValue.GetType();
        var enums = enumValue
            .EnumerateFlags(type, includeDefault)
            .Select(x => x.GetFirstDisplayName(type));
        return string.Join(separator, enums);
    }

    public static string? GetFirstDisplayName(this Enum enumValue)
        => enumValue.GetFirstDisplayName(enumValue.GetType());

    public static string? GetFirstDisplayName(this Enum enumValue, Type type)
        => _enumNamesData.GetOrAdd(enumValue, GetFirstDisplayNameInnerFunction, type);
    private static string? GetFirstDisplayNameInnerFunction(Enum enumValue, Type type)
    {
        var preResult = type
            .GetMember(enumValue.ToString())
            .FirstOrDefault(x => x.DeclaringType?.IsEnum ?? false);
        return preResult?
            .GetCustomAttribute<DisplayAttribute>()?
            .GetName() ?? preResult?.Name;
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
        => _propertyNamesData.GetOrAdd((objType, propertyName), GetDisplayNameInnerFunction);
    private static string? GetDisplayNameInnerFunction((Type,string) key)
    {
        var objType = key.Item1;
        var propertyName = key.Item2;
        var propInfo = objType.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
        if (propInfo == null)
        {
            propInfo = objType.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);
            if (propInfo == null)
            {
                throw new Exception($"Property named {propertyName} not found in {objType.Name}!");
            }
        }
        return propInfo.GetDisplayName();
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
        => _typeNamesData.GetOrAdd(objType, GetDisplayNameInnerFunction);
    private static string? GetDisplayNameInnerFunction(Type type)
        => type.GetCustomAttributes<DisplayNameAttribute>()?.FirstOrDefault()?.DisplayName;
    #endregion
}

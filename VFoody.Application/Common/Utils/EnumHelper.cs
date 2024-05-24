using System.ComponentModel;
using System.Reflection;

namespace VFoody.Application.Common.Utils;

public static class EnumHelper
{
    public static string GetEnumDescription<TEnum>(int value) where TEnum : Enum
    {
        var type = typeof(TEnum);
        var name = Enum.GetName(type, value);
        if (name == null)
        {
            return null;
        }

        var field = type.GetField(name);
        var attribute = field.GetCustomAttribute<DescriptionAttribute>();
        return attribute?.Description ?? name;
    }
}
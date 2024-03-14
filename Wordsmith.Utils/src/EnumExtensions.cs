using System.ComponentModel;

namespace Wordsmith.Utils;

public static class EnumExtensions
{
    public static string GetDescription<TEnum>(this TEnum value) where TEnum : Enum
    {
        var memberInfo = typeof(TEnum).GetMember(value.ToString());

        if (memberInfo.Length <= 0) return value.ToString();

        var attributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

        return attributes.Length > 0 ? ((DescriptionAttribute)attributes[0]).Description : value.ToString();
    }
}
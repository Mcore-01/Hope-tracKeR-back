using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Hope_tracKeR_back.Enums;

public static class EnumExtentions
{
    public static string GetDisplayName(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var displayAttr = field?.GetCustomAttribute<DisplayAttribute>();
        return displayAttr?.Name ?? value.ToString();
    }
}
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace SharpDeploy.Constants
{
    public static class EnumExtentions
    {
        public static string ToNameValue(this Enum enumValue)
        {
            var displayAttribute = enumValue.GetType()
                                    .GetMember(enumValue.ToString())
                                    .First()
                                    .GetCustomAttribute<DisplayAttribute>();

            return displayAttribute?.Name ?? enumValue.ToString();
        }
    }

}

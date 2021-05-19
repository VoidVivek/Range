using System;
using System.ComponentModel;
using System.Linq;

namespace Aveva.CounterRange
{
    /// <summary>
    ///     Class EnumExtensions.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        ///     Descriptions the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        public static string Description(this Enum value)
        {
            var attributes = value.GetType().GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Any())
                return (attributes.First() as DescriptionAttribute).Description;

            // return ToString() if description is not found
            return value.ToString();
        }
    }
}
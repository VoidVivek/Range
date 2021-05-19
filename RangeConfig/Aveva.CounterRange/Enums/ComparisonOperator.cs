using System.ComponentModel;

namespace Aveva.CounterRange.Enums
{
    /// <summary>
    ///     Comparison operators for a condition.
    /// </summary>
    public enum ComparisonOperator
    {
        /// <summary>
        ///     The equality comparison.
        /// </summary>
        [Description("=")] Equal = 0,

        /// <summary>
        ///     The inequality comparison.
        /// </summary>
        [Description("<>")] NotEqual = 1,

        /// <summary>
        ///     The "less than" numeric comparison.
        /// </summary>
        [Description("<")] LessThan = 2,

        /// <summary>
        ///     The "less than or equal" numeric comparison.
        /// </summary>
        [Description("<=")] LessThanOrEqual = 3,

        /// <summary>
        ///     The "greater than" numeric comparison.
        /// </summary>
        [Description(">")] GreaterThan = 4,

        /// <summary>
        ///     The "greater than or equal" numeric comparison.
        /// </summary>
        [Description(">=")] GreaterThanOrEqual = 5,

        /// <summary>
        ///     The "starts with" comparison.
        /// </summary>
        [Description("Starts With")] StartsWith = 6,

        /// <summary>
        ///     The "ends with" comparison.
        /// </summary>
        [Description("Ends With")] EndsWith = 7,

        /// <summary>
        ///     The "contains" comparison.
        /// </summary>
        Contains = 8,

        /// <summary>
        ///     The "contains" comparison applied on the value of an expression.
        /// </summary>
        ContainsOnValue = 9
    }
}
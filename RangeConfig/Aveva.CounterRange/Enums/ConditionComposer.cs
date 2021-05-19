namespace Aveva.CounterRange.Enums
{
    /// <summary>
    ///     Condition composition operators for joining multiple conditions.
    /// </summary>
    public enum ConditionComposer
    {
        /// <summary>
        ///     The "and" composition.
        /// </summary>
        None = 0,

        /// <summary>
        ///     The "and" composition.
        /// </summary>
        And = 1,

        /// <summary>
        ///     The "or" composition.
        /// </summary>
        Or = 2
    }
}
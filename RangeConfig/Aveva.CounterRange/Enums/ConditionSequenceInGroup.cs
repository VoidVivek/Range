namespace Aveva.CounterRange.Enums
{
    /// <summary>
    ///     Sequence of the condition in a group of conditions inside a rule.
    /// </summary>
    public enum ConditionSequenceInGroup
    {
        /// <summary>
        ///     Condition is not in a group
        /// </summary>
        None = 0,

        /// <summary>
        ///     First condition in the group
        /// </summary>
        Start = 1,

        /// <summary>
        ///     Somewhere in between First and Last condition in the group
        /// </summary>
        Inner = 2,

        /// <summary>
        ///     Last condition in the group
        /// </summary>
        End = 3,
    }
}
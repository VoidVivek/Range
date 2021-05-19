using System;
using System.Linq;
using System.Linq.Expressions;
using Aveva.CounterRange.Enums;

namespace Aveva.CounterRange.Models
{
    /// <summary>
    ///     Class Condition.
    /// </summary>
    public class Condition
    {
        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; set; }

        /// <summary>
        ///     Gets or sets the sequence.
        /// </summary>
        /// <value>The sequence.</value>
        public int Sequence { get; set; }

        /// <summary>
        ///     Gets or sets the and or.
        /// </summary>
        /// <value>The and or.</value>
        public ConditionComposer AndOr { get; set; }

        /// <summary>
        ///     Gets or sets the attribute URI.
        /// </summary>
        /// <value>The attribute URI.</value>
        public string AttributeUri { get; set; }

        /// <summary>
        ///     Gets or sets the comparison operator.
        /// </summary>
        /// <value>The comparison operator.</value>
        public ComparisonOperator ComparisonOperator { get; set; }

        /// <summary>
        ///     Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is grouped.
        /// </summary>
        /// <value><c>true</c> if this instance is grouped; otherwise, <c>false</c>.</value>
        public bool IsGrouped { get; set; }

        /// <summary>
        ///     Gets or sets the status in group.
        /// </summary>
        /// <value>The status in group.</value>
        public ConditionSequenceInGroup StatusInGroup { get; set; }

        /// <summary>
        ///     Gets the expression for this condition.
        /// </summary>
        /// <returns>BinaryExpression.</returns>
        /// <exception cref="ArgumentOutOfRangeException">ComparisonOperator - null</exception>
        public BinaryExpression GetExpression()
        {
            // TODO: find data type of the attribute to see if param and Const datatype will be string or int
            var t = IsNumeric(Value) ? typeof(int) : typeof(string);
            var paramExpression = Expression.Parameter(t);
            var constantExpression = Expression.Constant(Convert.ChangeType(Value, t), t);
            BinaryExpression binaryExpression = null;

            switch (ComparisonOperator)
            {
                case ComparisonOperator.Equal:
                    binaryExpression = Expression.Equal(paramExpression, constantExpression);
                    break;
                case ComparisonOperator.NotEqual:
                    binaryExpression = Expression.NotEqual(paramExpression, constantExpression);
                    break;
                case ComparisonOperator.LessThan:
                    binaryExpression = Expression.LessThan(paramExpression, constantExpression);
                    break;
                case ComparisonOperator.LessThanOrEqual:
                    binaryExpression = Expression.LessThanOrEqual(paramExpression, constantExpression);
                    break;
                case ComparisonOperator.GreaterThan:
                    binaryExpression = Expression.GreaterThan(paramExpression, constantExpression);
                    break;
                case ComparisonOperator.GreaterThanOrEqual:
                    binaryExpression = Expression.GreaterThanOrEqual(paramExpression, constantExpression);
                    break;
                case ComparisonOperator.StartsWith:
                    break;
                case ComparisonOperator.EndsWith:
                    break;
                case ComparisonOperator.Contains:
                    break;
                case ComparisonOperator.ContainsOnValue:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(ComparisonOperator), ComparisonOperator, null);
            }

            return binaryExpression;
        }

        /// <summary>
        ///     Determines whether the specified value is numeric.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if the specified value is numeric; otherwise, <c>false</c>.</returns>
        private bool IsNumeric(string value)
        {
            return value.All(char.IsNumber);
        }
    }
}
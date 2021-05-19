using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Aveva.CounterRange.Enums;

namespace Aveva.CounterRange.Models
{
    /// <summary>
    ///     Class Rule.
    /// </summary>
    public class Rule
    {
        /// <summary>
        ///     The compiled delegate for the rule.
        /// </summary>
        private Delegate compiled;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Rule" /> class.
        /// </summary>
        public Rule()
        {
            Conditions = new List<Condition>();
        }

        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; set; }

        /// <summary>
        ///     Gets or sets the minimum value.
        /// </summary>
        /// <value>The minimum value.</value>
        public int MinValue { get; set; }

        /// <summary>
        ///     Gets or sets the maximum value.
        /// </summary>
        /// <value>The maximum value.</value>
        public int MaxValue { get; set; }

        /// <summary>
        ///     Gets or sets the sequence.
        /// </summary>
        /// <value>The sequence.</value>
        public int Sequence { get; set; }

        /// <summary>
        ///     Gets or sets the conditions.
        /// </summary>
        /// <value>The conditions.</value>
        public List<Condition> Conditions { get; set; }

        /// <summary>
        ///     Adds a new condition to the rule.
        /// </summary>
        /// <returns>Condition.</returns>
        internal Condition AddNewCondition()
        {
            var retVal = new Condition
            {
                Sequence = Conditions.Count,
                AndOr = ConditionComposer.And
            };
            Conditions.Add(retVal);
            return retVal;
        }

        /// <summary>
        ///     Compiles this rule into a delegate.
        /// </summary>
        public void Compile()
        {
            if (compiled != null) // compile only once
                return;
            if (Conditions.Count == 0)
                return;

            BinaryExpression mainExpression = null;
            var parameterExpressions = new List<ParameterExpression>();

            foreach (var condition in Conditions)
            {
                var current = condition.GetExpression();
                if (mainExpression != null)
                {
                    if (condition.AndOr == ConditionComposer.And)
                        mainExpression = Expression.AndAlso(mainExpression, current);
                    else if (condition.AndOr == ConditionComposer.Or)
                        mainExpression = Expression.OrElse(mainExpression, current);
                }
                else
                {
                    mainExpression = current;
                }

                parameterExpressions.Add((ParameterExpression) current.Left);
            }

            var lambda = Expression.Lambda(mainExpression, parameterExpressions);

            compiled = lambda.Compile(true);
        }

        /// <summary>
        ///     Executes the compiled delegate with specified arguments.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="InvalidOperationException">Rule needs to be compiled before execution</exception>
        /// <exception cref="ArgumentException">Invalid number of arguments passed - arguments</exception>
        public bool Execute(object[] arguments)
        {
            if (compiled == null) throw new InvalidOperationException("Rule needs to be compiled before execution");

            if (Conditions.Count != arguments.Length)
                throw new ArgumentException("Invalid number of arguments passed", nameof(arguments));

            var retVal = compiled.DynamicInvoke(arguments);
            return (bool) retVal;
        }
    }
}
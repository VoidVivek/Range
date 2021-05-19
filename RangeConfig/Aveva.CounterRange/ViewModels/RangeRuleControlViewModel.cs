using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Aveva.CounterRange.Enums;
using Aveva.CounterRange.Models;

namespace Aveva.CounterRange.ViewModels
{
    /// <summary>
    ///     Class RangeRuleControlViewModel.
    /// </summary>
    /// <seealso cref="System.ComponentModel.IDataErrorInfo" />
    public class RangeRuleControlViewModel : IDataErrorInfo
    {
        /// <summary>
        ///     The add command
        /// </summary>
        private ICommand addCommand;

        /// <summary>
        ///     The group conditions command
        /// </summary>
        private ICommand groupConditionsCommand;

        /// <summary>
        ///     The remove command
        /// </summary>
        private ICommand removeCommand;

        /// <summary>
        ///     The rule
        /// </summary>
        private readonly Rule rule;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RangeRuleControlViewModel" /> class.
        /// </summary>
        /// <param name="rule">The rule.</param>
        public RangeRuleControlViewModel(Rule rule)
        {
            this.rule = rule;
            Conditions = GetConditions();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="RangeRuleControlViewModel" /> class.
        /// </summary>
        /// <param name="sequence">The sequence.</param>
        public RangeRuleControlViewModel(int sequence)
        {
            rule = new Rule();
            Sequence = sequence;

            var condition = rule.AddNewCondition();
            Conditions = new ObservableCollection<ConditionViewModel>
            {
                new ConditionViewModel(condition) {Sequence = sequence, AndOr = ConditionComposer.None}
            };
        }

        /// <summary>
        ///     Gets or sets the conditions.
        /// </summary>
        /// <value>The conditions.</value>
        public ObservableCollection<ConditionViewModel> Conditions { get; set; }

        /// <summary>
        ///     Gets the add command.
        /// </summary>
        /// <value>The add command.</value>
        public ICommand AddCommand =>
            addCommand ?? (addCommand = new CommandHandler<ConditionViewModel>(AddCondition, null));

        /// <summary>
        ///     Gets the remove command.
        /// </summary>
        /// <value>The remove command.</value>
        public ICommand RemoveCommand => removeCommand ??
                                         (removeCommand =
                                             new CommandHandler<ConditionViewModel>(RemoveCondition, null));

        /// <summary>
        ///     Gets the group conditions command.
        /// </summary>
        /// <value>The group conditions command.</value>
        public ICommand GroupConditionsCommand => groupConditionsCommand ??
                                                  (groupConditionsCommand = new CommandHandler(GroupConditions,
                                                      GroupConditionsCanExecute));

        /// <summary>
        ///     Gets or sets the minimum value.
        /// </summary>
        /// <value>The minimum value.</value>
        public int MinValue
        {
            get => rule.MinValue;
            set => rule.MinValue = value;
        }

        /// <summary>
        ///     Gets or sets the maximum value.
        /// </summary>
        /// <value>The maximum value.</value>
        public int MaxValue
        {
            get => rule.MaxValue;
            set => rule.MaxValue = value;
        }

        /// <summary>
        ///     Gets or sets the sequence.
        /// </summary>
        /// <value>The sequence.</value>
        public int Sequence
        {
            get => rule.Sequence;
            set => rule.Sequence = value;
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is selected.
        /// </summary>
        /// <value><c>true</c> if this instance is selected; otherwise, <c>false</c>.</value>
        public bool IsSelected { get; set; }

        /// <summary>
        ///     Gets the <see cref="System.String" /> with the specified property name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>System.String.</returns>
        public string this[string propertyName]
        {
            get
            {
                var error = string.Empty;

                switch (propertyName)
                {
                    case nameof(MinValue):
                        if (MinValue < 0)
                            error = "MinValue has to be greater than 0";
                        if (MinValue >= MaxValue)
                            error = "MinValue has to be lesser than MaxValue";
                        ;
                        break;

                    case nameof(MaxValue):
                        if (MaxValue < 0)
                            error = "MaxValue has to be greater than 0";
                        if (MinValue >= MaxValue)
                            error = "MinValue has to be lesser than MaxValue";
                        ;
                        break;
                }

                return error;
            }
        }

        /// <summary>
        ///     Gets an error message indicating what is wrong with this object.
        /// </summary>
        /// <value>The error.</value>
        public string Error => string.Empty;

        /// <summary>
        ///     Adds the condition.
        /// </summary>
        /// <param name="Condition">The condition.</param>
        public void AddCondition(ConditionViewModel Condition)
        {
            var index = Conditions.IndexOf(Condition);
            var condition = rule.AddNewCondition();
            Conditions.Insert(index + 1,
                new ConditionViewModel(condition) {Sequence = index, AndOr = ConditionComposer.And});
        }

        /// <summary>
        ///     Removes the condition.
        /// </summary>
        /// <param name="Condition">The condition.</param>
        public void RemoveCondition(ConditionViewModel Condition)
        {
            if (Conditions.Count > 1) Conditions.Remove(Condition);
        }

        /// <summary>
        ///     Groups the conditions.
        /// </summary>
        public void GroupConditions()
        {
            var toBeGrouped = new List<ConditionViewModel>();

            for (var i = 0; i < Conditions.Count; i++)
            {
                var Condition = Conditions[i];
                if (Condition.IsSelected && !Condition.IsGrouped)
                {
                    // Validations required
                    Condition.IsGrouped = true;
                    Condition.StatusInGroup = ConditionSequenceInGroup.Inner;
                    toBeGrouped.Add(Condition);
                }
            }

            if (toBeGrouped.Count > 0)
            {
                toBeGrouped[0].StatusInGroup = ConditionSequenceInGroup.Start;
                toBeGrouped[toBeGrouped.Count - 1].StatusInGroup = ConditionSequenceInGroup.End;
            }
        }

        /// <summary>
        ///     Copies this instance.
        /// </summary>
        /// <returns>RangeRuleControlViewModel.</returns>
        internal RangeRuleControlViewModel Copy()
        {
            var copy = new RangeRuleControlViewModel(Sequence);
            copy.Conditions.Clear();
            copy.MinValue = MinValue;
            copy.MaxValue = MaxValue;

            foreach (var Condition in Conditions)
            {
                var conditionCopy = Condition.Copy();
                copy.Conditions.Add(conditionCopy);
            }

            return copy;
        }

        /// <summary>
        ///     Groups the conditions can execute.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool GroupConditionsCanExecute(object obj)
        {
            var sequences = new List<int>();
            for (var i = 0; i < Conditions.Count; i++)
            {
                var Condition = Conditions[i];
                if (Condition.IsSelected && !Condition.IsGrouped) sequences.Add(Condition.Sequence);
            }

            var count = 0;
            for (var i = 0; i < sequences.Count; i++)
                if (i >= 1)
                    if (sequences[i] - 1 == sequences[i - 1]) count++;

            var retVal = count > 0 && count == sequences.Count - 1;
            return retVal;
        }

        /// <summary>
        ///     Gets the conditions.
        /// </summary>
        /// <returns>ObservableCollection&lt;ConditionViewModel&gt;.</returns>
        private ObservableCollection<ConditionViewModel> GetConditions()
        {
            var retVal = new ObservableCollection<ConditionViewModel>();

            foreach (var condition in rule.Conditions)
            {
                var conditionViewModel = new ConditionViewModel(condition);
                retVal.Add(conditionViewModel);
            }

            return retVal;
        }

        /// <summary>
        ///     Gets the rule.
        /// </summary>
        /// <returns>Rule.</returns>
        internal Rule GetRule()
        {
            return rule;
        }
    }
}
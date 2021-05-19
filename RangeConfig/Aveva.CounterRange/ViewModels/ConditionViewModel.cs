using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Aveva.CounterRange.Enums;
using Aveva.CounterRange.Models;

namespace Aveva.CounterRange.ViewModels
{
    /// <summary>
    ///     Class ConditionViewModel.
    /// </summary>
    /// <seealso cref="System.ComponentModel.IDataErrorInfo" />
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public class ConditionViewModel : IDataErrorInfo, INotifyPropertyChanged
    {
        /// <summary>
        ///     The condition
        /// </summary>
        private readonly Condition condition;

        /// <summary>
        ///     The status in group
        /// </summary>
        private ConditionSequenceInGroup statusInGroup;

        ///// <summary>
        /////     Initializes a new instance of the <see cref="ConditionViewModel" /> class.
        ///// </summary>
        //public ConditionViewModel()
        //{
        //    condition = new Condition();
        //}

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConditionViewModel" /> class.
        /// </summary>
        /// <param name="condition">The condition.</param>
        public ConditionViewModel(Condition condition)
        {
            this.condition = condition;
            Sequence = condition.Sequence;
            AndOr = condition.AndOr;
            Attribute = condition.AttributeUri;
            ComparisonOperator = condition.ComparisonOperator;
            Value = condition.Value;
            IsGrouped = condition.IsGrouped;
            StatusInGroup = condition.StatusInGroup;
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is selected.
        /// </summary>
        /// <value><c>true</c> if this instance is selected; otherwise, <c>false</c>.</value>
        public bool IsSelected { get; set; }

        /// <summary>
        ///     Gets or sets the sequence.
        /// </summary>
        /// <value>The sequence.</value>
        public int Sequence
        {
            get => condition.Sequence;
            set => condition.Sequence = value;
        }

        /// <summary>
        ///     Gets or sets the and or.
        /// </summary>
        /// <value>The and or.</value>
        public ConditionComposer AndOr
        {
            get => condition.AndOr;
            set => condition.AndOr = value;
        }

        /// <summary>
        ///     Gets or sets the attribute.
        /// </summary>
        /// <value>The attribute.</value>
        public string Attribute
        {
            get => condition.AttributeUri;
            set => condition.AttributeUri = value;
        }

        /// <summary>
        ///     Gets or sets the comparison operator.
        /// </summary>
        /// <value>The comparison operator.</value>
        public ComparisonOperator ComparisonOperator
        {
            get => condition.ComparisonOperator;
            set => condition.ComparisonOperator = value;
        }

        /// <summary>
        ///     Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value
        {
            get => condition.Value;
            set => condition.Value = value;
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is grouped.
        /// </summary>
        /// <value><c>true</c> if this instance is grouped; otherwise, <c>false</c>.</value>
        public bool IsGrouped
        {
            get => condition.IsGrouped;
            set => condition.IsGrouped = value;
        }

        /// <summary>
        ///     Gets or sets the status in group.
        /// </summary>
        /// <value>The status in group.</value>
        public ConditionSequenceInGroup StatusInGroup
        {
            get => condition.StatusInGroup;
            set
            {
                SetField(ref statusInGroup, value);
                condition.StatusInGroup = statusInGroup;
            }
        }

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
                    case nameof(Attribute):
                        if (string.IsNullOrWhiteSpace(Value))
                            error = "Attribute cannot be empty.";
                        break;
                    case nameof(Value):
                        if (string.IsNullOrWhiteSpace(Value))
                            error = "Value cannot be empty.";
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
        ///     Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        ///     Sets the field.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        ///     Copies this instance.
        /// </summary>
        /// <returns>ConditionViewModel.</returns>
        public ConditionViewModel Copy()
        {
            return (ConditionViewModel) MemberwiseClone();
        }
    }
}
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Aveva.CounterRange.Models;
using Aveva.CounterRange.Repositories;

namespace Aveva.CounterRange.ViewModels
{
    /// <summary>
    ///     Class RangeConfigControlViewModel.
    /// </summary>
    public class RangeConfigControlViewModel
    {
        /// <summary>
        ///     The add command
        /// </summary>
        private ICommand addCommand;

        /// <summary>
        ///     The copy command
        /// </summary>
        private ICommand copyCommand;

        /// <summary>
        ///     The move down command
        /// </summary>
        private ICommand moveDownCommand;

        /// <summary>
        ///     The move up command
        /// </summary>
        private ICommand moveUpCommand;

        /// <summary>
        ///     The range configuration
        /// </summary>
        private readonly RangeConfig rangeConfig;

        /// <summary>
        ///     The remove command
        /// </summary>
        private ICommand removeCommand;

        /// <summary>
        ///     The repository
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        ///     The selected
        /// </summary>
        private bool selected;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RangeConfigControlViewModel" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="rangeConfig">The range configuration.</param>
        public RangeConfigControlViewModel(IRepository repository, RangeConfig rangeConfig)
        {
            this.repository = repository;
            this.rangeConfig = rangeConfig;
            RangeRuleControlViewModels = new ObservableCollection<RangeRuleControlViewModel>();
        }

        /// <summary>
        ///     Gets the range rule control view models.
        /// </summary>
        /// <value>The range rule control view models.</value>
        public ObservableCollection<RangeRuleControlViewModel> RangeRuleControlViewModels { get; private set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get => rangeConfig.DisplayName;
            set => rangeConfig.DisplayName = value;
        }

        /// <summary>
        ///     Gets or sets the class.
        /// </summary>
        /// <value>The class.</value>
        public string Class
        {
            //TODO: some extra logic required to conver URI into class name and vice versa
            get => rangeConfig.ClassUri;
            set => rangeConfig.ClassUri = value;
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is selected.
        /// </summary>
        /// <value><c>true</c> if this instance is selected; otherwise, <c>false</c>.</value>
        public bool IsSelected
        {
            get => selected;
            set
            {
                selected = value;
                if (value)
                {
                    if (string.IsNullOrWhiteSpace(rangeConfig.Id))
                        RangeRuleControlViewModels = new ObservableCollection<RangeRuleControlViewModel>();
                    else
                        RangeRuleControlViewModels = GetRangeRules();
                }
            }
        }

        /// <summary>
        ///     Gets the add command.
        /// </summary>
        /// <value>The add command.</value>
        public ICommand AddCommand => addCommand ?? (addCommand = new CommandHandler(AddRangeRule, null));

        /// <summary>
        ///     Gets the remove command.
        /// </summary>
        /// <value>The remove command.</value>
        public ICommand RemoveCommand => removeCommand ?? (removeCommand = new CommandHandler(RemoveRangeRule, null));

        /// <summary>
        ///     Gets the copy command.
        /// </summary>
        /// <value>The copy command.</value>
        public ICommand CopyCommand => copyCommand ?? (copyCommand = new CommandHandler(CopyRangeRule, null));

        /// <summary>
        ///     Gets the move up command.
        /// </summary>
        /// <value>The move up command.</value>
        public ICommand MoveUpCommand => moveUpCommand ?? (moveUpCommand = new CommandHandler(MoveRangeRuleUp, null));

        /// <summary>
        ///     Gets the move down command.
        /// </summary>
        /// <value>The move down command.</value>
        public ICommand MoveDownCommand =>
            moveDownCommand ?? (moveDownCommand = new CommandHandler(MoveRangeRuleDown, null));

        /// <summary>
        ///     Adds the range rule.
        /// </summary>
        internal void AddRangeRule()
        {
            RangeRuleControlViewModels.Add(new RangeRuleControlViewModel(RangeRuleControlViewModels.Count));
        }

        /// <summary>
        ///     Removes the range rule.
        /// </summary>
        internal void RemoveRangeRule()
        {
            for (var i = RangeRuleControlViewModels.Count - 1; i >= 0; i--)
            {
                var rangeControlViewModel = RangeRuleControlViewModels[i];
                if (rangeControlViewModel.IsSelected)
                {
                    RangeRuleControlViewModels.Remove(rangeControlViewModel);
                    break;
                }
            }
        }

        /// <summary>
        ///     Copies the range rule.
        /// </summary>
        internal void CopyRangeRule()
        {
            for (var i = RangeRuleControlViewModels.Count - 1; i >= 0; i--)
            {
                var rangeControlViewModel = RangeRuleControlViewModels[i];
                if (rangeControlViewModel.IsSelected)
                {
                    var copy = rangeControlViewModel.Copy();
                    RangeRuleControlViewModels.Add(copy);
                    break;
                }
            }
        }

        /// <summary>
        ///     Copies this instance.
        /// </summary>
        /// <returns>RangeConfigControlViewModel.</returns>
        internal RangeConfigControlViewModel Copy()
        {
            var rangeConfig = new RangeConfig();
            var copy = new RangeConfigControlViewModel(repository, rangeConfig);
            copy.Name = "Copy-" + Name;
            copy.Class = Class;

            for (var i = 0; i < RangeRuleControlViewModels.Count; i++)
                copy.RangeRuleControlViewModels.Add(RangeRuleControlViewModels[i].Copy());

            return copy;
        }

        /// <summary>
        ///     Saves this instance.
        /// </summary>
        internal void Save()
        {
            var rules = ExtractRules();
            repository.Save(rangeConfig, rules);
        }

        /// <summary>
        ///     Moves the range rule up.
        /// </summary>
        private void MoveRangeRuleUp()
        {
            for (var i = RangeRuleControlViewModels.Count - 1; i >= 0; i--)
            {
                var rangeControlViewModel = RangeRuleControlViewModels[i];
                if (rangeControlViewModel.IsSelected)
                {
                    var currentIndex = RangeRuleControlViewModels.IndexOf(rangeControlViewModel);
                    if (currentIndex > 0)
                    {
                        var previous = RangeRuleControlViewModels[currentIndex - 1];
                        var previousIndex = RangeRuleControlViewModels.IndexOf(previous);

                        RangeRuleControlViewModels.Remove(rangeControlViewModel);
                        RangeRuleControlViewModels.Remove(previous);

                        RangeRuleControlViewModels.Insert(previousIndex, rangeControlViewModel);
                        rangeControlViewModel.Sequence = previousIndex;

                        RangeRuleControlViewModels.Insert(currentIndex, previous);
                        previous.Sequence = currentIndex;
                    }

                    break;
                }
            }
        }

        /// <summary>
        ///     Moves the range rule down.
        /// </summary>
        private void MoveRangeRuleDown()
        {
            for (var i = RangeRuleControlViewModels.Count - 1; i >= 0; i--)
            {
                var rangeControlViewModel = RangeRuleControlViewModels[i];
                if (rangeControlViewModel.IsSelected)
                {
                    var currentIndex = RangeRuleControlViewModels.IndexOf(rangeControlViewModel);
                    if (currentIndex < RangeRuleControlViewModels.Count - 1)
                    {
                        var next = RangeRuleControlViewModels[currentIndex + 1];
                        var nextIndex = RangeRuleControlViewModels.IndexOf(next);

                        RangeRuleControlViewModels.Remove(rangeControlViewModel);
                        RangeRuleControlViewModels.Remove(next);

                        RangeRuleControlViewModels.Insert(currentIndex, next);
                        next.Sequence = currentIndex;

                        if (nextIndex > RangeRuleControlViewModels.Count)
                            RangeRuleControlViewModels.Add(rangeControlViewModel);
                        else
                            RangeRuleControlViewModels.Insert(nextIndex, rangeControlViewModel);

                        rangeControlViewModel.Sequence = nextIndex;
                    }

                    break;
                }
            }
        }

        /// <summary>
        ///     Gets the range rules.
        /// </summary>
        /// <returns>ObservableCollection&lt;RangeRuleControlViewModel&gt;.</returns>
        private ObservableCollection<RangeRuleControlViewModel> GetRangeRules()
        {
            var retVal = new ObservableCollection<RangeRuleControlViewModel>();
            var rules = repository.GetRulesForConfig(rangeConfig.Id);

            foreach (var rule in rules)
            {
                var rangeRuleControlViewModel = new RangeRuleControlViewModel(rule);
                retVal.Add(rangeRuleControlViewModel);
            }

            return retVal;
        }

        /// <summary>
        ///     Extracts the rules.
        /// </summary>
        /// <returns>IEnumerable&lt;Rule&gt;.</returns>
        private IEnumerable<Rule> ExtractRules()
        {
            var rules = new List<Rule>();

            foreach (var rangeRuleControlViewModel in RangeRuleControlViewModels)
                rules.Add(rangeRuleControlViewModel.GetRule());

            return rules;
        }
    }
}
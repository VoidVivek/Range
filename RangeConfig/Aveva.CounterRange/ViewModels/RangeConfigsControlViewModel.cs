using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Serialization;
using Aveva.CounterRange.Models;
using Aveva.CounterRange.Repositories;

namespace Aveva.CounterRange.ViewModels
{
    /// <summary>
    ///     Class RangeConfigsControlViewModel.
    /// </summary>
    public class RangeConfigsControlViewModel
    {
        /// <summary>
        ///     The add command
        /// </summary>
        private ICommand addCommand;

        /// <summary>
        ///     The collection view
        /// </summary>
        private readonly ICollectionView collectionView;

        /// <summary>
        ///     The copy command
        /// </summary>
        private ICommand copyCommand;

        /// <summary>
        ///     The remove command
        /// </summary>
        private ICommand removeCommand;

        /// <summary>
        ///     The repository
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RangeConfigsControlViewModel" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public RangeConfigsControlViewModel(IRepository repository)
        {
            this.repository = repository;
            var rangeConfigs = repository.GetRangeConfigs();
            RangeConfigControlViewModels = new ObservableCollection<RangeConfigControlViewModel>();
            collectionView = CollectionViewSource.GetDefaultView(RangeConfigControlViewModels);
            collectionView.CurrentChanging += CollectionView_CurrentChanging;

            var i = 0;
            foreach (var rangeConfig in rangeConfigs)
            {
                var rangeConfigControlViewModel = new RangeConfigControlViewModel(repository, rangeConfig);
                if (i == 0) rangeConfigControlViewModel.IsSelected = true; // Select the first one so it is loaded
                i++;
                RangeConfigControlViewModels.Add(rangeConfigControlViewModel);
            }
        }

        /// <summary>
        ///     Gets the range configuration control view models.
        /// </summary>
        /// <value>The range configuration control view models.</value>
        public ObservableCollection<RangeConfigControlViewModel> RangeConfigControlViewModels { get; }

        /// <summary>
        ///     Gets the add command.
        /// </summary>
        /// <value>The add command.</value>
        public ICommand AddCommand => addCommand ?? (addCommand = new CommandHandler(AddRangeConfig, null));

        /// <summary>
        ///     Gets the remove command.
        /// </summary>
        /// <value>The remove command.</value>
        public ICommand RemoveCommand => removeCommand ?? (removeCommand = new CommandHandler(RemoveRangeConfig, null));

        /// <summary>
        ///     Gets the copy command.
        /// </summary>
        /// <value>The copy command.</value>
        public ICommand CopyCommand => copyCommand ?? (copyCommand = new CommandHandler(CopyRangeConfig, null));

        /// <summary>
        ///     Handles the CurrentChanging event of the CollectionView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CurrentChangingEventArgs" /> instance containing the event data.</param>
        private void CollectionView_CurrentChanging(object sender, CurrentChangingEventArgs e)
        {
            if (collectionView.CurrentItem != null) ((RangeConfigControlViewModel) collectionView.CurrentItem).Save();
        }

        /// <summary>
        ///     Adds the range configuration.
        /// </summary>
        public void AddRangeConfig()
        {
            var rangeConfig = new RangeConfig();
            var rangeConfigControlViewModel = new RangeConfigControlViewModel(repository, rangeConfig)
                {Name = "Range Config" + RangeConfigControlViewModels.Count + 1, IsSelected = true};
            RangeConfigControlViewModels.Add(rangeConfigControlViewModel);
        }

        /// <summary>
        ///     Removes the range configuration.
        /// </summary>
        public void RemoveRangeConfig()
        {
            for (var i = RangeConfigControlViewModels.Count - 1; i >= 0; i--)
            {
                var rangeConfigControlViewModel = RangeConfigControlViewModels[i];
                if (rangeConfigControlViewModel.IsSelected)
                {
                    RangeConfigControlViewModels.Remove(rangeConfigControlViewModel);
                    break;
                }
            }
        }

        /// <summary>
        ///     Copies the range configuration.
        /// </summary>
        private void CopyRangeConfig()
        {
            var writer =
                new XmlSerializer(typeof(ObservableCollection<RangeConfigControlViewModel>));

            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                       "//SerializedRangeConfigs.xml";
            var file = File.Create(path);

            writer.Serialize(file, RangeConfigControlViewModels);
            file.Close();
            //for (int i = RangeConfigControlViewModels.Count - 1; i >= 0; i--)
            //{
            //    var rangeConfigControlViewModel = RangeConfigControlViewModels[i];
            //    if (rangeConfigControlViewModel.IsSelected)
            //    {
            //        RangeConfigControlViewModels.Add(rangeConfigControlViewModel.Copy());
            //        break;
            //    }
            //}
        }
    }
}
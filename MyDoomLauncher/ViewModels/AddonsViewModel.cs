using Microsoft.Expression.Interactivity.Core;
using MyDoomLauncher.Models;
using MyDoomLauncher.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MyDoomLauncher.ViewModels
{
    public sealed class AddonsViewModel : INotifyPropertyChanged
    {
        public async void LoadAddons()
        {
            _history = new HistoryProvider();
            _allAddons = await WadsSearch.GetAddons();
            Addons = new ObservableCollection<AddOn>(_allAddons);
            OnPropertyChanged("Addons");
        }

        public void OnStartAddon()
        {
            if (SelectedItem == null)
            {
                if (!TrySelectFirstPossibleItem())
                    return;
            }

            string parameters = ParametersBuilder.BuildStartParameter(SelectedItem, _allAddons);

            // Change date for single selected item.
            SelectedItem.ChangeLastUseDateToNow();

            // Change date for all selected items.
            foreach (var item in _allAddons)
            {
                if (item != SelectedItem && item.Selected)
                    item.ChangeLastUseDateToNow();
            }

#if !DEBUG
            ProcessStart.StartProcess(parameters);
#endif

            _history.UpdateHistoryFromList(Addons);
        }

        public string SearchInput
        {
            get
            {
                return _searchInput;
            }

            set
            {
                _searchInput = value;
                OnPropertyChanged();
                FilterAddons(_searchInput);
            }
        }

        public ObservableCollection<AddOn> Addons {
            get
            {
                return _addons;
            }

            set
            {
                _addons = value;
                OnPropertyChanged();
            }
        }

        public ICommand ClearCommand
        {
            get
            {
                if (clearCommand == null)
                {
                    clearCommand = new ActionCommand(Clear);
                }

                return clearCommand;
            }
        }

        private void FilterAddons(string searchInput)
        {
            if (string.IsNullOrWhiteSpace(searchInput) || searchInput.Length < 1)
            {
                Addons = new ObservableCollection<AddOn>(_allAddons);
            }
            else
            {
                searchInput = searchInput.ToLower();
                Addons = new ObservableCollection<AddOn>(_allAddons.Where(a => a.Name.ToLower().Contains(searchInput)));
            }
        }

        private bool TrySelectFirstPossibleItem()
        {
            var firstElement = Addons.FirstOrDefault();
            if (firstElement == null)
                return false;
            SelectedItem = firstElement;
            return true;
        }

        private void Clear() => SearchInput = string.Empty;

        private void OnPropertyChanged([CallerMemberName]string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public event PropertyChangedEventHandler PropertyChanged;

        public AddOn SelectedItem { get; set; }

        private string _searchInput;
        private ObservableCollection<AddOn> _addons;
        private IHistoryProvider _history;
        private List<AddOn> _allAddons;
        private ActionCommand clearCommand;
    }
}

using MyDoomLauncher.Models;
using MyDoomLauncher.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

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

            ParametersBuilder parametersBuilder = new ParametersBuilder();
            string parameters = ParametersBuilder.BuildStartParameter(SelectedItem, _allAddons);

            SelectedItem.RefreshLastUseDate();
            foreach (var item in _allAddons)
            {
                if (item != SelectedItem && item.Selected)
                    item.RefreshLastUseDate();
            }

            ProcessStart.StartProcess(parameters);
            _history.UpdateHistoryFromList(Addons);
        }

        private bool TrySelectFirstPossibleItem()
        {
            var firstElement = Addons.FirstOrDefault();
            if (firstElement == null)
                return false;
            SelectedItem = firstElement;
            return true;
        }

        private string _searchInput;
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

        private ObservableCollection<AddOn> _addons;
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

        private void OnPropertyChanged([CallerMemberName]string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public event PropertyChangedEventHandler PropertyChanged;

        public AddOn SelectedItem { get; set; }

        private HistoryProvider _history;
        private List<AddOn> _allAddons;
    }
}

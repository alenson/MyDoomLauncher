using MyDoomLauncher.Models;
using MyDoomLauncher.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyDoomLauncher.ViewModels
{
    public sealed class AddonsViewModel : INotifyPropertyChanged
    {
        public async void LoadAddons()
        {
            _allAddons = await WadsSearch.GetAddons();
            Addons = new ObservableCollection<AddOn>(_allAddons);
            OnPropertyChanged("Addons");
        }

        public void ViewUnload()
        {
            History history = new History();
            history.DeleteDataFileIfNotUsed(Addons.Count);
        }

        private bool CanStartAddon()
        {
            return SelectedItem != null;
        }

        public void OnStartAddon()
        {
            string wadFileName = SelectedItem.FileName;

            SelectedItem.LastUseDate = DateTime.Now;
            SelectedItem.TimesUsed++;

            ProcessStart.StartProcess(wadFileName);

            History history = new History();
            history.UpdateHistoryFromList(Addons);

        }

        public AddOn SelectedItem { get; set; }

        private string _searchInput;
        public string SearchInput {
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

        private void OnPropertyChanged([CallerMemberName]string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private List<AddOn> _allAddons;
    }
}

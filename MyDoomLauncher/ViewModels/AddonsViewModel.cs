using MyDoomLauncher.Models;
using MyDoomLauncher.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyDoomLauncher.ViewModels
{
    public sealed class AddonsViewModel : INotifyPropertyChanged
    {
        public AddonsViewModel()
        {
            StartAddonCommand = new RelayCommand(OnStartAddon, CanStartAddon);
        }

        public async void LoadAddons()
        {
            Collection = new ObservableCollection<AddOn>(await WadsSearch.GetAddons());
            OnPropertyChanged("Collection");
            StartAddonCommand.RaiseCanExecuteChanged();
        }

        public void ViewUnload()
        {
            History history = new History();
            history.DeleteDataFileIfNotUsed(Collection.Count);
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
            history.UpdateHistoryFromList(Collection);

        }

        private AddOn _selectedItem;
        public AddOn SelectedItem
        {
            get
            {
                return _selectedItem;
            }

            set
            {
                StartAddonCommand.RaiseCanExecuteChanged();
                _selectedItem = value;
            }
        }

        private void OnPropertyChanged([CallerMemberName]string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public ObservableCollection<AddOn> Collection { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;


        public RelayCommand StartAddonCommand { get; private set; }

    }
}

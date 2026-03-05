using Microsoft.Xaml.Behaviors.Core;
using MyDoomLauncher.Models;
using MyDoomLauncher.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
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

            // Set default sort by Last Played (descending)
            _lastSortColumn = "LastUseDateFormatted";
            _sortAscending = false;
            var sortedAddons = SortAddonsDescending(Addons, "LastUseDateFormatted").ToList();
            Addons = new ObservableCollection<AddOn>(sortedAddons);

            OnPropertyChanged("Addons");
        }

        public void OnStartAddon()
        {
            if (SelectedItem == null && !_allAddons.Any(a => a.Selected))
            {
                if (!TrySelectFirstPossibleItem())
                    return;
            }

            // Ignore currently selected if any checkbox is checked.
            if (_allAddons.Any(a => a.Selected))
            {
                SelectedItem = null;
            }

            string parameters = ParametersBuilder.BuildStartParameter(SelectedItem, _allAddons.Where(a => a.Selected).ToList());

            // Change date for single selected item.
            SelectedItem?.ChangeLastUseDateToNow();

            // Change date for all selected items.
            foreach (var item in _allAddons)
            {
                if (item != SelectedItem && item.Selected)
                    item.ChangeLastUseDateToNow();
            }

            ProcessStart.StartProcess(parameters);
            _history.UpdateHistoryFromList(_allAddons);
        }

        public void OnColumnHeaderClick()
        {
            var element = System.Windows.Input.Mouse.DirectlyOver;
            if (element == null)
                return;

            // Try to find GridViewColumnHeader in the visual tree
            DependencyObject current = element as DependencyObject;
            while (current != null)
            {
                GridViewColumnHeader header = current as GridViewColumnHeader;
                if (header != null)
                {
                    string displayName = header.Content as string;
                    if (!string.IsNullOrEmpty(displayName))
                    {
                        // Map display names to property names
                        string propertyName = MapDisplayNameToPropertyName(displayName);
                        if (!string.IsNullOrEmpty(propertyName))
                        {
                            SortByColumn(propertyName);
                        }
                    }
                    return;
                }

                // Stop if we hit a ListView item (means we clicked on content, not header)
                ListViewItem item = current as ListViewItem;
                if (item != null)
                    return;

                current = System.Windows.Media.VisualTreeHelper.GetParent(current);
            }
        }

        private string MapDisplayNameToPropertyName(string displayName)
        {
            if (displayName == "Name")
                return "Name";
            else if (displayName == "Last played")
                return "LastUseDateFormatted";
            else if (displayName == "Times played")
                return "TimesUsed";
            else
                return null;
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

        public ObservableCollection<AddOn> Addons
        {
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

        public ICommand RunCommand
        {
            get
            {
                if (runCommand == null)
                {
                    runCommand = new ActionCommand(OnStartAddon);
                }

                return runCommand;
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

        public AddOn SelectedItem { get; set; }

        public ICommand SortCommand
        {
            get
            {
                if (sortCommand == null)
                {
                    sortCommand = new SortCommandImpl(this);
                }

                return sortCommand;
            }
        }

        public void SortByColumn(string columnName)
        {
            if (_allAddons == null || _allAddons.Count == 0)
                return;

            // Toggle sort direction or start new sort
            if (_lastSortColumn == columnName)
            {
                _sortAscending = !_sortAscending;
            }
            else
            {
                _lastSortColumn = columnName;
                _sortAscending = true;
            }

            // Apply sort to filtered collection
            var sortedAddons = _sortAscending 
                ? SortAddonsAscending(Addons, columnName).ToList()
                : SortAddonsDescending(Addons, columnName).ToList();

            Addons = new ObservableCollection<AddOn>(sortedAddons);
        }

        private IEnumerable<AddOn> SortAddonsAscending(IEnumerable<AddOn> addons, string columnName)
        {
            if (columnName == "Name")
                return addons.OrderBy(a => a.Name);
            else if (columnName == "LastUseDateFormatted")
                return addons.OrderBy(a => a.LastUseDate);
            else if (columnName == "TimesUsed")
                return addons.OrderBy(a => a.TimesUsed);
            else
                return addons;
        }

        private IEnumerable<AddOn> SortAddonsDescending(IEnumerable<AddOn> addons, string columnName)
        {
            if (columnName == "Name")
                return addons.OrderByDescending(a => a.Name);
            else if (columnName == "LastUseDateFormatted")
                return addons.OrderByDescending(a => a.LastUseDate);
            else if (columnName == "TimesUsed")
                return addons.OrderByDescending(a => a.TimesUsed);
            else
                return addons;
        }

        private void Clear() => SearchInput = string.Empty;

        private void OnPropertyChanged([CallerMemberName] string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public event PropertyChangedEventHandler PropertyChanged;

        private string _searchInput;
        private ObservableCollection<AddOn> _addons;
        private IHistoryProvider _history;
        private List<AddOn> _allAddons;
        private ActionCommand clearCommand;
        private ActionCommand runCommand;
        private SortCommandImpl sortCommand;
        private string _lastSortColumn;
        private bool _sortAscending = true;
    }

    internal class SortCommandImpl : ICommand
    {
        private readonly AddonsViewModel _viewModel;

        public SortCommandImpl(AddonsViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var columnName = parameter as string;
            if (!string.IsNullOrEmpty(columnName))
            {
                _viewModel.SortByColumn(columnName);
            }
        }
    }
}

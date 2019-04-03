using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MyDoomLauncher.Models
{
    [Serializable]
    public sealed class AddOn : INotifyPropertyChanged
    {
        public bool Selected { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }

        private int _timesUsed;
        private DateTime _lastUseDate;

        public DateTime LastUseDate
        {
            get
            {
                return _lastUseDate;
            }
            set
            {
                if (value != _lastUseDate)
                {
                    _lastUseDate = value;
                    OnPropertyChanged();
                }
            }
        }
        public int TimesUsed
        {
            get
            {
                return _timesUsed;
            }
            set
            {
                if (value != _timesUsed)
                {
                    _timesUsed = value;
                    OnPropertyChanged();
                }
            }
        }

        private void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string LastUseDateFormatted
        {
            get
            {
                if (LastUseDate == DateTime.MinValue)
                    return "Unknown";

                return LastUseDate.ToString("dd.MM.yyyy");
            }
        }

        [field:NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        internal void RefreshLastUseDate()
        {
            LastUseDate = DateTime.Now;
            TimesUsed++;
        }
    }
}

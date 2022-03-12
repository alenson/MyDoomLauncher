using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace MyDoomLauncher.Models
{
    [Serializable]
    public sealed class AddOn : INotifyPropertyChanged
    {
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
                    OnPropertyChanged(nameof(LastUseDateFormatted));
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

        public void ChangeLastUseDateToNow()
        {
            LastUseDate = DateTime.Now;
            TimesUsed++;
        }

        public string LastUseDateFormatted
        {
            get
            {
                if (LastUseDate == DateTime.MinValue)
                    return "Unknown";

                return LastUseDate.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [field:NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        public bool Selected { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }

        private int _timesUsed;
        private DateTime _lastUseDate;
    }
}

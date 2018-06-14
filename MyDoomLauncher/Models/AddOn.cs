using System;

namespace MyDoomLauncher.Models
{
    [Serializable]
    public sealed class AddOn
    {
        public string Name { get; set; }
        public string FileName { get; set; }
        public DateTime LastUseDate { get; set; }
        public int TimesUsed { get; set; }

        public string LastUseDateFormatted
        {
            get
            {
                if (LastUseDate == DateTime.MinValue)
                    return "Unknown";

                return LastUseDate.ToString("dd.MM.yyyy");
            }
        }
    }
}

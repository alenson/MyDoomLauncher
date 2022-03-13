using MyDoomLauncher.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;

namespace MyDoomLauncher.Services
{
    sealed class HistoryProvider : IHistoryProvider
    {
        public void UpdateListFromHistory(IEnumerable<AddOn> list)
        {
            if (string.IsNullOrEmpty(_filePath))
                SetFilePath();

            if (_history == null)
                LoadHistory();

            foreach (var item in list)
            {
                if (_history.ContainsKey(item.Name))
                    CopyDetails(item, _history[item.Name]);
            }
        }

        public void UpdateHistoryFromList(IEnumerable<AddOn> list)
        {
            CreateHistoryDictionary(list);

            if (string.IsNullOrEmpty(_filePath))
                SetFilePath();

            if (!Directory.Exists(Path.GetDirectoryName(_filePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(_filePath));

            using (FileStream stream = File.Create(_filePath))
            {
                using (GZipStream compressedStream = new GZipStream(stream, CompressionLevel.Optimal, false))
                {
                    BinaryFormatter serializer = new BinaryFormatter();
                    serializer.Serialize(compressedStream, _history);
                }
            }
        }

        private void CopyDetails(AddOn target, AddOn source)
        {
            target.LastUseDate = source.LastUseDate;
            target.TimesUsed = source.TimesUsed;
        }

        private void LoadHistory()
        {
            if (!File.Exists(_filePath))
            {
                _history = new Dictionary<string, AddOn>();
                return;
            }
            try
            {
                using (FileStream stream = File.OpenRead(_filePath))
                {
                    using (GZipStream compressedStream = new GZipStream(stream, CompressionMode.Decompress))
                    {
                        BinaryFormatter serializer = new BinaryFormatter();
                        _history = serializer.Deserialize(compressedStream) as Dictionary<string, AddOn>;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error while accessing history file.\n" + e.Message, "Error", MessageBoxButton.OK);
                _history = new Dictionary<string, AddOn>();
            }
        }

        private static void SetFilePath()
        {
#if DEBUG
            _filePath = Path.GetTempPath();
#else
            _filePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
#endif
            _filePath = Path.Combine(_filePath, "MyDoomLauncher", "history.bin");
        }

        private static void CreateHistoryDictionary(IEnumerable<AddOn> list)
        {
            _history = new Dictionary<string, AddOn>();

            foreach (var item in list)
            {
                _history.Add(item.Name, item);
            }
        }

        private static Dictionary<string, AddOn> _history;
        private static string _filePath;
    }
}

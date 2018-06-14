using MyDoomLauncher.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;

namespace MyDoomLauncher.Services
{
    sealed class History
    {
        public void UpdateListFromHistory(IEnumerable<AddOn> list)
        {
            if (string.IsNullOrEmpty(m_filePath))
                SetFilePath();

            if (m_history == null)
                LoadHistory();

            foreach (var item in list)
            {
                if (m_history.ContainsKey(item.Name))
                    CopyDetails(item, m_history[item.Name]);
            }
        }

        private void CopyDetails(AddOn target, AddOn source)
        {
            target.LastUseDate = source.LastUseDate;
            target.TimesUsed = source.TimesUsed;
        }

        private void LoadHistory()
        {
            if (!File.Exists(m_filePath))
            {
                m_history = new Dictionary<string, AddOn>();
                return;
            }
            try
            {
                using (FileStream stream = File.OpenRead(m_filePath))
                {
                    BinaryFormatter serializer = new BinaryFormatter();
                    m_history = serializer.Deserialize(stream) as Dictionary<string, AddOn>;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error while accessing history file.\n" + e.Message, "Error", MessageBoxButton.OK);
                m_history = new Dictionary<string, AddOn>();
            }
        }

        private static void SetFilePath()
        {
            m_filePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            m_filePath = Path.Combine(m_filePath, "MyDoomLauncher", "history.bin");
        }

        public void UpdateHistoryFromList(IEnumerable<AddOn> list)
        {
            CreateHistoryDictionary(list);

            if (string.IsNullOrEmpty(m_filePath))
                SetFilePath();

            if (!Directory.Exists(Path.GetDirectoryName(m_filePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(m_filePath));

            using (FileStream stream = File.Create(m_filePath))
            {
                BinaryFormatter serializer = new BinaryFormatter();
                serializer.Serialize(stream, m_history);
            }
        }

        private static void CreateHistoryDictionary(IEnumerable<AddOn> list)
        {
            m_history = new Dictionary<string, AddOn>();

            foreach(var item in list)
            {
                m_history.Add(item.Name, item);
            }
        }

        public void DeleteDataFileIfNotUsed(int itemsCount)
        {
            if (string.IsNullOrEmpty(m_filePath))
                SetFilePath();

            if (itemsCount == 0 && File.Exists(m_filePath))
                File.Delete(m_filePath);
        }

        private static Dictionary<string, AddOn> m_history;
        private static string m_filePath;
    }
}

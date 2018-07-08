using MyDoomLauncher.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyDoomLauncher.Services
{
    public static class WadsSearch
    {
        public async static Task<List<AddOn>> GetAddons()
        {
            string searchPattern = ConfigurationManager.AppSettings.Get("SearchPattern");
            List<string> allFiles = new List<string>();

            return await Task.Factory.StartNew(() =>
             {
                 foreach (var item in searchPattern.Split(','))
                 {
                     string[] files = Directory.GetFiles(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), $"*.{item}", SearchOption.TopDirectoryOnly);
                     allFiles.AddRange(files);
                 }

                 List<AddOn> list = new List<AddOn>();
                 foreach (var item in allFiles)
                 {
                     list.Add(new AddOn()
                     {
                         FileName = item,
                         Name = Path.GetFileNameWithoutExtension(item),
                         TimesUsed = 0,
                         LastUseDate = DateTime.MinValue,
                     });
                 }

                 FillDataFromHistory(list);
                 return list.OrderByDescending(a => a.LastUseDate).ToList();
             });
        }

        private static void FillDataFromHistory(List<AddOn> list)
        {
            HistoryProvider history = new HistoryProvider();
            history.UpdateListFromHistory(list);
        }
    }
}

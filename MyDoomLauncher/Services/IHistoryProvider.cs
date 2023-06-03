using MyDoomLauncher.Models;
using System.Collections.Generic;

namespace MyDoomLauncher.Services
{
    internal interface IHistoryProvider
    {
        void UpdateHistoryFromList(IEnumerable<AddOn> list);
        void UpdateListFromHistory(IEnumerable<AddOn> list);
    }
}
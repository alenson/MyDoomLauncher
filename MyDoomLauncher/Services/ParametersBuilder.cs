using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyDoomLauncher.Models;

namespace MyDoomLauncher.Services
{
    class ParametersBuilder
    {
        internal static string BuildStartParameter(AddOn clickedAddon, List<AddOn> allAddons)
        {
            StringBuilder builder = new StringBuilder();
            if (clickedAddon != null && !clickedAddon.Selected)
                builder.AppendFormat(ParameterPartFormat, clickedAddon.FileName);

            foreach (var item in allAddons.Where(a => a.Selected))
                builder.AppendFormat(ParameterPartFormat, item.FileName);

            builder.Length--;

            return builder.ToString();
        }

        private static string ParameterPartFormat = "{0} ";
    }
}

﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyDoomLauncher.Models;

namespace MyDoomLauncher.Services
{
    class ParametersBuilder
    {
        internal static string BuildStartParameter(AddOn clickedAddon, List<AddOn> selectedWads)
        {
            // Create list of all selected WADs.
            if (clickedAddon != null)
            {
                selectedWads.Add(clickedAddon);
            }

            if (!selectedWads.Any())
            {
                return string.Empty;
            }

            AddOn iwad = GetInternalWad(selectedWads);
            List<AddOn> distinct = selectedWads.Distinct().ToList();

            // Remove IWAD as it should be presented differently.
            if (iwad != null)
            {
                distinct.Remove(iwad);
            }

            // Build parameters starting from IWAD and then adding rest of selected WADs.
            return CreateRunParameters(distinct, iwad).ToString();
        }

        private static StringBuilder CreateRunParameters(List<AddOn> selectedWads, AddOn iwad)
        {
            string iWadParameterFormat = ConfigurationProvider.GetValueForCurrentConfiguration("IwadParameterFormat");
            string parameterPartFormat = ConfigurationProvider.GetValueForCurrentConfiguration("ParameterFormat");
            StringBuilder builder = new StringBuilder();

            if (iwad != null)
            {
                builder.AppendFormat(iWadParameterFormat, iwad.CommandLineFileName);
            }

            foreach (AddOn wad in selectedWads)
            {
                builder.AppendFormat(parameterPartFormat, wad.CommandLineFileName);
            }

            builder.Length--;
            return builder;
        }

        private static AddOn GetInternalWad(List<AddOn> selectedWads)
        {
            AddOn iwad = default;
            foreach (string name in ConfigurationProvider.GetKnownInternalWads())
            {
                iwad = selectedWads.FirstOrDefault(a => a.FileName.Equals(name, System.StringComparison.OrdinalIgnoreCase));
                if (iwad != null)
                {
                    break;
                }
            }

            return iwad;
        }
    }
}

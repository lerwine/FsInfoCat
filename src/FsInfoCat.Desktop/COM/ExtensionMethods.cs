using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;

namespace FsInfoCat.Desktop.COM
{
    public static class ExtensionMethods
    {
        public static readonly ReadOnlyCollection<string> IgnoreExtendedProperties = new(new string[] { "Size", "Name",
            "Date modified", "Date accessed", "Total size", "Computer", "Filename", "Space free", "Shared", "Folder name",
            "Folder path", "Folder", "Path", "Space used", "Sharing status", "Availability status", "Type" });

        private static readonly ReadOnlyDictionary<string, ExtendedPropertyName> _propertyMap;

        static ExtensionMethods()
        {
            Dictionary<string, ExtendedPropertyName> propertyMap = new(StringComparer.InvariantCultureIgnoreCase);
            foreach (ExtendedPropertyName value in Enum.GetValues<ExtendedPropertyName>())
                propertyMap.Add((string)typeof(ExtendedPropertyName).GetField(value.ToString("F")).GetCustomAttribute<AmbientValueAttribute>().Value, value);
            _propertyMap = new(propertyMap);
        }
    }
}

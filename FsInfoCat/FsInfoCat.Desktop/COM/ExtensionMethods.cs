using Shell32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop.COM
{
    public static class ExtensionMethods
    {
        public static readonly ReadOnlyCollection<string> IgnoreExtendedProperties = new ReadOnlyCollection<string>(new string[] { "Size", "Name",
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

        public static async Task<Dictionary<int, string>> GetExtendedPropertyMapAsync(this Folder folder) => await Task.Run(() => Enumerable.Range(0, 0xffff).Select(index =>
        {
            string name = folder.GetDetailsOf(null, index);
            return (index, name);
        }).Where(a => !(string.IsNullOrWhiteSpace(a.name) || IgnoreExtendedProperties.Contains(a.name))).ToDictionary(a => a.index, a => a.name));

        public static async Task<Dictionary<ExtendedPropertyName, int[]>> GetExtendedPropertyDictionaryAsync(this Folder folder) => await Task.Run(() => Enumerable.Range(0, 0xffff).Select(index =>
        {
            string name = folder.GetDetailsOf(null, index);
            ExtendedPropertyName? value = (string.IsNullOrWhiteSpace(name) || !_propertyMap.ContainsKey(name)) ? null : _propertyMap[name];
            return (index, value);
        }).Where(a => a.value.HasValue).GroupBy(a => a.value.Value).ToDictionary(a => a.Key, a => a.Select(v => v.index).ToArray()));

        public static async Task<IEnumerable<(string Name, Dictionary<ExtendedPropertyName, string> properties)>> GetExtendedPropertiesAsync(this Folder folder, Func<(string Name, bool IsFolder), bool> filter = null)
        {
            Dictionary<ExtendedPropertyName, int[]> propertyDefinitions = await folder.GetExtendedPropertyDictionaryAsync();
            return ((filter is null) ? folder.Items().OfType<FolderItem>() : folder.Items().OfType<FolderItem>().Where(f => filter((f.Name, f.IsFolder)))).Select(item =>
                (item.Name, propertyDefinitions.Keys.Select(n => (n, propertyDefinitions[n].Select(i => folder.GetDetailsOf(item, i))
                .FirstOrDefault(s => !string.IsNullOrWhiteSpace(s)))).Where(a => a.Item2 is not null).ToDictionary(a => a.n, a => a.Item2)));
        }

        public static async Task<IEnumerable<(string Name, Dictionary<string, string> properties)>> GetAllExtendedPropertiesAsync(this Folder folder, Func<(string Name, bool IsFolder), bool> filter = null)
        {
            Dictionary<int, string> propertyDefinitions = await folder.GetExtendedPropertyMapAsync();
            return ((filter is null) ? folder.Items().OfType<FolderItem>() : folder.Items().OfType<FolderItem>().Where(f => filter((f.Name, f.IsFolder)))).Select(item =>
                (item.Name, propertyDefinitions.Keys.Select(i => (i, folder.GetDetailsOf(item, i))).Where(a => !string.IsNullOrWhiteSpace(a.Item2))
                .GroupBy(a => propertyDefinitions[a.i]).ToDictionary(a => a.Key, a => a.First().Item2)));
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Management;
using System.Text.RegularExpressions;

namespace FsInfoCat.Desktop.Model
{
    public static class Extensions
    {
        public static T? ToEnumPropertyValue<T>(this ManagementObject managementObject, string propertyName)
            where T : struct, Enum
        {
            object obj = managementObject[propertyName];
            if (obj is null)
                return null;
            return (T)Enum.ToObject(typeof(T), obj);
        }

        public static IEnumerable<T> ToEnumPropertyValues<T>(this ManagementObject managementObject, string propertyName)
            where T : struct, Enum
        {
            object obj = managementObject[propertyName];
            if (obj is null)
                yield break;
            foreach (object o in (IEnumerable)obj)
                yield return (T)Enum.ToObject(typeof(T), o);
        }

        public static Win32_LocalTime ToWin32LocalTime(this ManagementObject managementObject, string propertyName)
        {
            object obj = managementObject[propertyName];
            if (obj is null)
                return null;
            return new Win32_LocalTime((ManagementObject)obj);
        }

        internal static string GetRootPathName(this IVolume volume)
        {
#warning GetRootPathName not implemented
            throw new NotImplementedException();
        }

        internal static string GetDriveFormat(this IVolume volume)
        {
#warning GetDriveFormat not implemented
            throw new NotImplementedException();
        }

        internal static long GetEffectiveMaxNameLength(this IVolume volume)
        {
#warning GetEffectiveMaxNameLength not implemented
            throw new NotImplementedException();
        }

        internal static bool GetEffectiveCaseSensitiveSearch(this IVolume volume)
        {
#warning GetEffectiveCaseSensitiveSearch not implemented
            throw new NotImplementedException();
        }
    }
}

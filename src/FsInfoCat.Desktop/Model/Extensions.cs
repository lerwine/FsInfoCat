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
        //public static int FindPrimeNumber(int startValue)
        //{
        //    try
        //    {
        //        if ((Math.Abs(startValue) & 1) == 0)
        //            startValue++;
        //        while (!IsPrimeNumber(startValue))
        //            startValue += 2;
        //    }
        //    catch (OverflowException) { return 1; }
        //    return startValue;
        //}

        //public static bool IsPrimeNumber(int n)
        //{   
        //    if (((n = Math.Abs(n)) & 1) == 0)
        //        return false;
        //    for (int i = n >> 1; i > 1; i--)
        //    {
        //        if (n % i == 0)
        //            return false;
        //    }
        //    return true;
        //}

        //public static T[] CoerceAsArray<T>(this IEnumerable<T> source) => (source is null) ? Array.Empty<T>() : (source is T[] a) ? a : source.ToArray();

        //public static int ToAggregateHashCode(this IEnumerable<int> hashCodes)
        //{
        //    int[] arr = hashCodes.CoerceAsArray();
        //    int prime = arr.Length;
        //    if (prime == 0)
        //        return 0;
        //    if (arr.Length == 1)
        //        return arr[0];
        //    int seed = FindPrimeNumber(prime);
        //    for (int n = 1; n < prime; n++)
        //        seed = FindPrimeNumber(seed + 1);
        //    prime = FindPrimeNumber(seed + 1);
        //    return arr.Aggregate(seed, (a, i) =>
        //    {
        //        unchecked { return (a * prime) ^ i; }
        //    });
        //}

        //public static int GetAggregateHashCode<T>(this IEnumerable<T> source, IEqualityComparer<T> comparer = null)
        //{
        //    if (source is null || !source.Any())
        //        return 0;
        //    if (comparer is null)
        //        comparer = EqualityComparer<T>.Default;
        //    return source.Select(obj => comparer.GetHashCode(obj)).ToAggregateHashCode();
        //}

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

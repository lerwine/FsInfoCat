using FsInfoCat.Models.Volumes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace FsInfoCat.Util
{
    public static class ExtensionMethods
    {
        public const string MESSAGE_NO_PATH_PROVIDED = "No path was provied";
        public const string MESSAGE_ERROR_GETTING_MACHINE_IDENTIFIER = "Error getting machine identifier.";
        public const string MESSAGE_INVALID_PATH = "Path is invalid.";
        public const string MESSAGE_INVALID_ABSOLUTE_FILE_URI = "Invalid URI or not an absolute file URI.";
        public const string MESSAGE_PATH_NOT_FOUND = "Path not found.";
        public const string MESSAGE_FILESYSTEM_INFO_PROPERTY_ACCESS_ERROR = "Unable to obtain name or length.";
        public const string MESSAGE_CREATION_TIME_ACCESS_ERROR = "Unable to obtain creation time.";
        public const string MESSAGE_LAST_WRITE_TIME_ACCESS_ERROR = "Unable to obtain last write time.";
        public const string MESSAGE_ATTRIBUTES_ACCESS_ERROR = "Unable to obtain file system attributes.";
        public const string MESSAGE_DIRECTORY_FILES_ACCESS_ERROR = "Unable to enmerate files.";
        public const string MESSAGE_SUBDIRECTORIES_ACCESS_ERROR = "Unable to enmerate subdirectories.";
        public const string MESSAGE_CRAWL_OPERATION_STOPPED = "Crawl operation stopped.";
        public const string MESSAGE_MAX_DEPTH_REACHED = "Maximum crawl depth has been reached.";
        public const string MESSAGE_MAX_ITEMS_REACHED = "Maximum crawl item count has been reached.";
        public const string MESSAGE_UNEXPECTED_ERROR = "An unexpected error has occurred.";

        public static T FindByChildItem<T>(this IEnumerable<T> source, FileUri fileUri)
            where T : class, IVolumeInfo
        {
            if (source is null || fileUri is null)
                return null;

            // Compare name. If matches, go up to parent volume and compare there
            throw new NotImplementedException();
        }

        public static IEnumerable<T> FindByIdentifier<T>(this IEnumerable<T> source, VolumeIdentifier volumeIdentifier)
            where T : class, IVolumeInfo
        {
            if (source is null)
                return new T[0];
            return source.Where(v => !(v is null) && v.Identifier.Equals(volumeIdentifier));
        }

        public static IEnumerable<T> FindByRootUri<T>(this IEnumerable<T> source, FileUri fileUri)
            where T : class, IVolumeInfo
        {
            if (source is null || fileUri is null)
                return new T[0];
            return source.Where(v => !(v is null) && fileUri.Equals(v.RootUri, v.SegmentNameComparer));
        }

        public static IEnumerable<T> FindByVolumeName<T>(this IEnumerable<T> source, string volumeName)
            where T : class, IVolumeInfo
        {
            if (source is null)
                return new T[0];
            if (string.IsNullOrEmpty(volumeName))
                return source.Where(v => !(v is null) && string.IsNullOrEmpty(v.VolumeName));
            StringComparer comparer = StringComparer.InvariantCultureIgnoreCase;
            return source.Where(v => !(v is null) && comparer.Equals(volumeName, v.VolumeName));
        }

        public static bool IsEqualTo(this string s, char c) => null != s && s.Length == 1 && s[0] == c;

        public static bool IsEqualTo(this string target, string other, int length)
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length));
            if (target is null)
                return other is null;
            if (other is null)
                return false;
            if (length == 0)
                return true;
            if (target.Length == 0)
                return other.Length == 0;
            if (length > target.Length)
            {
                if (length < other.Length)
                    return false;
            }
            else if (length == target.Length)
            {
                if (length > other.Length)
                    return false;
                if (length == other.Length)
                    return target.Equals(other);
            }
            else if (other.Length < length)
                return false;
            return target.Take(length).SequenceEqual(other.Take(length));
        }

        public static bool IsSubstringEqualTo(this string target, int startIndex, string other, int otherStartIndex, int length)
        {
            if (startIndex < 1 && otherStartIndex < 1)
                return IsEqualTo(target, other, length);
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (otherStartIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(otherStartIndex));
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length));
            if (target is null)
                return other is null;
            if (other is null)
                return false;
            if (startIndex >= target.Length)
                return otherStartIndex >= other.Length;
            if (otherStartIndex >= other.Length)
                return false;
            if (length == 0)
                return true;
            int targetEndIndex = startIndex + length;
            int otherEndIndex = otherStartIndex + length;
            int shortage = targetEndIndex - target.Length;
            if ((shortage > 0) ? (otherEndIndex - other.Length != shortage) : (other.Length < otherEndIndex))
                return false;
            return target.Skip(startIndex).Take(length).SequenceEqual(other.Skip(startIndex).Take(length));
        }

        public static bool IsSubstringEqualTo(this string target, int startIndex, string other, int otherStartIndex)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (otherStartIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(otherStartIndex));
            if (target is null)
                return other is null;
            if (startIndex == 0 && otherStartIndex == 0)
                return target.Equals(other);
            if (other is null)
                return false;
            int tLen = target.Length - startIndex;
            int oLen = other.Length - otherStartIndex;
            if (tLen < 1)
                return oLen < 1;
            return oLen == tLen && target.Skip(startIndex).SequenceEqual(other.Skip(otherStartIndex));
        }

        public static bool EndsWith(this string s, char c) => !string.IsNullOrEmpty(s) && s[s.Length - 1] == c;

        public static bool StartsWith(this string s, char c) => !string.IsNullOrEmpty(s) && s[0] == c;

        public static string[] Split(this string s, char c) => (s is null) ? new string[0] : s.Split(new char[] { c });

        public static string[] Split(this string s, char c, int count) => (s is null) ? new string[0] : s.Split(new char[] { c }, count);

        public static bool TryDequeue<T>(this Queue<T> source, out T result)
        {
            if (source is null || source.Count == 0)
            {
                result = default;
                return false;
            }
            try
            {
                result = source.Dequeue();
            }
            catch (InvalidOperationException)
            {
                result = default;
                return false;
            }
            return true;
        }

        public static bool TryGetDescription(this MemberInfo memberInfo, out string description)
        {
            description = null;

            foreach (string d in memberInfo.GetCustomAttributes<DescriptionAttribute>().Select(a => a.Description).Where(a => null != a))
            {
                if ((description = d).Any(c => !Char.IsWhiteSpace(c)))
                    return true;
            }
            return false;
        }

        public static bool TryGetDescription<T>(this T value, out string description, out string name)
            where T : struct, Enum
        {
            Type t = value.GetType();
            name = value.ToString("F");
            return t.GetField(name).TryGetDescription(out description);
        }

        public static bool TryGetDescription<T>(this T value, out string description) where T : struct, Enum => value.GetType().GetField(value.ToString("F")).TryGetDescription(out description);

        public static string GetDescription(this MemberInfo memberInfo) => memberInfo?.GetCustomAttributes<DescriptionAttribute>()
            .Select(a => a.Description).Where(d => !string.IsNullOrWhiteSpace(d)).FirstOrDefault();

        public static string GetDescription<T>(this T value, Func<string, string> getDefaultDescription = null)
            where T : struct, Enum
        {
            if (value.TryGetDescription(out string description, out string name))
                return description;
            return (getDefaultDescription is null) ? $"ID: {name}" : getDefaultDescription(name);
        }

        public static string GetDescription<T>(this T value, out string name) where T : struct, Enum => (value.TryGetDescription(out string description, out name)) ? description : $"ID: {name}";

        public static string GetDescription<T>(this T value, Func<string, string> getDefaultDescription, out string name)
            where T : struct, Enum
        {
            if (value.TryGetDescription(out string description, out name))
                return description;
            return (getDefaultDescription is null) ? $"ID: {name}" : getDefaultDescription(name);
        }

        public static string GetDescription<T>(this T value, Func<T, string> getDefaultDescription)
            where T : struct, Enum
        {
            if (value.TryGetDescription(out string description, out string name))
                return description;
            return (getDefaultDescription is null) ? $"ID: {name}" : getDefaultDescription(value);
        }

        public static string GetDescription<T>(this T value, Func<T, string> getDefaultDescription, out string name)
            where T : struct, Enum
        {
            if (value.TryGetDescription(out string description, out name))
                return description;
            return (getDefaultDescription is null) ? $"ID: {name}" : getDefaultDescription(value);
        }
    }
}

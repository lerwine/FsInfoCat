using System;
using System.Management;

namespace FsInfoCat.Desktop.WMI
{
    public static class WmiExtensionMethods
    {
        public static ushort GetSInt16(this ManagementObject obj, string propertyName) =>
            GetSInt16((obj ?? throw new ArgumentNullException(nameof(obj))).Properties, propertyName);

        public static ushort GetSInt16(this PropertyDataCollection properties, string propertyName)
        {
            if (properties is null)
                throw new ArgumentNullException(nameof(properties));
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or empty.", nameof(propertyName));
            PropertyData propertyData = properties[propertyName];
            if (propertyData is not null && propertyData.Type == CimType.SInt16 && !propertyData.IsArray)
                return (ushort)propertyData.Value;
            throw new ArgumentOutOfRangeException(nameof(propertyName));
        }

        public static ushort? GetSInt16Opt(this ManagementObject obj, string propertyName) =>
            GetSInt16Opt((obj ?? throw new ArgumentNullException(nameof(obj))).Properties, propertyName);

        public static ushort? GetSInt16Opt(this PropertyDataCollection properties, string propertyName)
        {
            if (properties is null)
                throw new ArgumentNullException(nameof(properties));
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or empty.", nameof(propertyName));
            PropertyData propertyData = properties[propertyName];
            if (propertyData is not null && propertyData.Type == CimType.SInt16 && !propertyData.IsArray)
                return (ushort?)propertyData.Value;
            throw new ArgumentOutOfRangeException(nameof(propertyName));
        }

        public static int GetSInt32(this ManagementObject obj, string propertyName) =>
            GetSInt32((obj ?? throw new ArgumentNullException(nameof(obj))).Properties, propertyName);

        public static int GetSInt32(this PropertyDataCollection properties, string propertyName)
        {
            if (properties is null)
                throw new ArgumentNullException(nameof(properties));
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or empty.", nameof(propertyName));
            PropertyData propertyData = properties[propertyName];
            if (propertyData is not null && propertyData.Type == CimType.SInt32 && !propertyData.IsArray)
                return (int)propertyData.Value;
            throw new ArgumentOutOfRangeException(nameof(propertyName));
        }

        public static int? GetSInt32Opt(this ManagementObject obj, string propertyName) =>
            GetSInt32Opt((obj ?? throw new ArgumentNullException(nameof(obj))).Properties, propertyName);

        public static int? GetSInt32Opt(this PropertyDataCollection properties, string propertyName)
        {
            if (properties is null)
                throw new ArgumentNullException(nameof(properties));
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or empty.", nameof(propertyName));
            PropertyData propertyData = properties[propertyName];
            if (propertyData is not null && propertyData.Type == CimType.SInt32 && !propertyData.IsArray)
                return (int?)propertyData.Value;
            throw new ArgumentOutOfRangeException(nameof(propertyName));
        }

        public static float GetReal32(this ManagementObject obj, string propertyName) =>
            GetReal32((obj ?? throw new ArgumentNullException(nameof(obj))).Properties, propertyName);

        public static float GetReal32(this PropertyDataCollection properties, string propertyName)
        {
            if (properties is null)
                throw new ArgumentNullException(nameof(properties));
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or empty.", nameof(propertyName));
            PropertyData propertyData = properties[propertyName];
            if (propertyData is not null && propertyData.Type == CimType.Real32 && !propertyData.IsArray)
                return (float)propertyData.Value;
            throw new ArgumentOutOfRangeException(nameof(propertyName));
        }

        public static float? GetReal32Opt(this ManagementObject obj, string propertyName) =>
            GetReal32Opt((obj ?? throw new ArgumentNullException(nameof(obj))).Properties, propertyName);

        public static float? GetReal32Opt(this PropertyDataCollection properties, string propertyName)
        {
            if (properties is null)
                throw new ArgumentNullException(nameof(properties));
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or empty.", nameof(propertyName));
            PropertyData propertyData = properties[propertyName];
            if (propertyData is not null && propertyData.Type == CimType.Real32 && !propertyData.IsArray)
                return (float?)propertyData.Value;
            throw new ArgumentOutOfRangeException(nameof(propertyName));
        }

        public static double GetReal64(this ManagementObject obj, string propertyName) =>
            GetReal64((obj ?? throw new ArgumentNullException(nameof(obj))).Properties, propertyName);

        public static double GetReal64(this PropertyDataCollection properties, string propertyName)
        {
            if (properties is null)
                throw new ArgumentNullException(nameof(properties));
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or empty.", nameof(propertyName));
            PropertyData propertyData = properties[propertyName];
            if (propertyData is not null && propertyData.Type == CimType.Real64 && !propertyData.IsArray)
                return (double)propertyData.Value;
            throw new ArgumentOutOfRangeException(nameof(propertyName));
        }

        public static double? GetReal64Opt(this ManagementObject obj, string propertyName) =>
            GetReal64Opt((obj ?? throw new ArgumentNullException(nameof(obj))).Properties, propertyName);

        public static double? GetReal64Opt(this PropertyDataCollection properties, string propertyName)
        {
            if (properties is null)
                throw new ArgumentNullException(nameof(properties));
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or empty.", nameof(propertyName));
            PropertyData propertyData = properties[propertyName];
            if (propertyData is not null && propertyData.Type == CimType.Real64 && !propertyData.IsArray)
                return (double?)propertyData.Value;
            throw new ArgumentOutOfRangeException(nameof(propertyName));
        }

        internal static string GetString(this ManagementObject obj, string propertyName) =>
            GetString((obj ?? throw new ArgumentNullException(nameof(obj))).Properties, propertyName);

        internal static string GetString(this PropertyDataCollection properties, string propertyName)
        {
            if (properties is null)
                throw new ArgumentNullException(nameof(properties));
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or empty.", nameof(propertyName));
            PropertyData propertyData = properties[propertyName];
            if (propertyData is not null && propertyData.Type == CimType.String && !propertyData.IsArray)
                return (string)propertyData.Value;
            throw new ArgumentOutOfRangeException(nameof(propertyName));
        }

        public static bool GetBoolean(this ManagementObject obj, string propertyName) =>
            GetBoolean((obj ?? throw new ArgumentNullException(nameof(obj))).Properties, propertyName);

        public static bool GetBoolean(this PropertyDataCollection properties, string propertyName)
        {
            if (properties is null)
                throw new ArgumentNullException(nameof(properties));
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or empty.", nameof(propertyName));
            PropertyData propertyData = properties[propertyName];
            if (propertyData is not null && propertyData.Type == CimType.Boolean && !propertyData.IsArray)
                return (bool)propertyData.Value;
            throw new ArgumentOutOfRangeException(nameof(propertyName));
        }

        public static bool? GetBooleanOpt(this ManagementObject obj, string propertyName) =>
            GetBooleanOpt((obj ?? throw new ArgumentNullException(nameof(obj))).Properties, propertyName);

        public static bool? GetBooleanOpt(this PropertyDataCollection properties, string propertyName)
        {
            if (properties is null)
                throw new ArgumentNullException(nameof(properties));
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or empty.", nameof(propertyName));
            PropertyData propertyData = properties[propertyName];
            if (propertyData is not null && propertyData.Type == CimType.Boolean && !propertyData.IsArray)
                return (bool?)propertyData.Value;
            throw new ArgumentOutOfRangeException(nameof(propertyName));
        }

        public static sbyte GetSInt8(this ManagementObject obj, string propertyName) =>
            GetSInt8((obj ?? throw new ArgumentNullException(nameof(obj))).Properties, propertyName);

        public static sbyte GetSInt8(this PropertyDataCollection properties, string propertyName)
        {
            if (properties is null)
                throw new ArgumentNullException(nameof(properties));
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or empty.", nameof(propertyName));
            PropertyData propertyData = properties[propertyName];
            if (propertyData is not null && propertyData.Type == CimType.SInt8 && !propertyData.IsArray)
                return (sbyte)propertyData.Value;
            throw new ArgumentOutOfRangeException(nameof(propertyName));
        }

        public static sbyte? GetSInt8Opt(this ManagementObject obj, string propertyName) =>
            GetSInt8Opt((obj ?? throw new ArgumentNullException(nameof(obj))).Properties, propertyName);

        public static sbyte? GetSInt8Opt(this PropertyDataCollection properties, string propertyName)
        {
            if (properties is null)
                throw new ArgumentNullException(nameof(properties));
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or empty.", nameof(propertyName));
            PropertyData propertyData = properties[propertyName];
            if (propertyData is not null && propertyData.Type == CimType.SInt8 && !propertyData.IsArray)
                return (sbyte?)propertyData.Value;
            throw new ArgumentOutOfRangeException(nameof(propertyName));
        }

        public static byte GetUInt8(this ManagementObject obj, string propertyName) =>
            GetUInt8((obj ?? throw new ArgumentNullException(nameof(obj))).Properties, propertyName);

        public static byte GetUInt8(this PropertyDataCollection properties, string propertyName)
        {
            if (properties is null)
                throw new ArgumentNullException(nameof(properties));
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or empty.", nameof(propertyName));
            PropertyData propertyData = properties[propertyName];
            if (propertyData is not null && propertyData.Type == CimType.UInt8 && !propertyData.IsArray)
                return (byte)propertyData.Value;
            throw new ArgumentOutOfRangeException(nameof(propertyName));
        }

        public static byte? GetUInt8Opt(this ManagementObject obj, string propertyName) =>
            GetUInt8Opt((obj ?? throw new ArgumentNullException(nameof(obj))).Properties, propertyName);

        public static byte? GetUInt8Opt(this PropertyDataCollection properties, string propertyName)
        {
            if (properties is null)
                throw new ArgumentNullException(nameof(properties));
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or empty.", nameof(propertyName));
            PropertyData propertyData = properties[propertyName];
            if (propertyData is not null && propertyData.Type == CimType.UInt8 && !propertyData.IsArray)
                return (byte?)propertyData.Value;
            throw new ArgumentOutOfRangeException(nameof(propertyName));
        }

        public static ushort GetUInt16(this ManagementObject obj, string propertyName) =>
            GetUInt16((obj ?? throw new ArgumentNullException(nameof(obj))).Properties, propertyName);

        public static ushort GetUInt16(this PropertyDataCollection properties, string propertyName)
        {
            if (properties is null)
                throw new ArgumentNullException(nameof(properties));
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or empty.", nameof(propertyName));
            PropertyData propertyData = properties[propertyName];
            if (propertyData is not null && propertyData.Type == CimType.UInt16 && !propertyData.IsArray)
                return (ushort)propertyData.Value;
            throw new ArgumentOutOfRangeException(nameof(propertyName));
        }

        public static ushort? GetUInt16Opt(this ManagementObject obj, string propertyName) =>
            GetUInt16Opt((obj ?? throw new ArgumentNullException(nameof(obj))).Properties, propertyName);

        public static ushort? GetUInt16Opt(this PropertyDataCollection properties, string propertyName)
        {
            if (properties is null)
                throw new ArgumentNullException(nameof(properties));
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or empty.", nameof(propertyName));
            PropertyData propertyData = properties[propertyName];
            if (propertyData is not null && propertyData.Type == CimType.UInt16 && !propertyData.IsArray)
                return (ushort?)propertyData.Value;
            throw new ArgumentOutOfRangeException(nameof(propertyName));
        }

        public static uint GetUInt32(this ManagementObject obj, string propertyName) =>
            GetUInt32((obj ?? throw new ArgumentNullException(nameof(obj))).Properties, propertyName);

        public static uint GetUInt32(this PropertyDataCollection properties, string propertyName)
        {
            if (properties is null)
                throw new ArgumentNullException(nameof(properties));
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or empty.", nameof(propertyName));
            PropertyData propertyData = properties[propertyName];
            if (propertyData is not null && propertyData.Type == CimType.UInt32 && !propertyData.IsArray)
                return (uint)propertyData.Value;
            throw new ArgumentOutOfRangeException(nameof(propertyName));
        }

        public static uint? GetUInt32Opt(this ManagementObject obj, string propertyName) =>
            GetUInt32Opt((obj ?? throw new ArgumentNullException(nameof(obj))).Properties, propertyName);

        public static uint? GetUInt32Opt(this PropertyDataCollection properties, string propertyName)
        {
            if (properties is null)
                throw new ArgumentNullException(nameof(properties));
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or empty.", nameof(propertyName));
            PropertyData propertyData = properties[propertyName];
            if (propertyData is not null && propertyData.Type == CimType.UInt32 && !propertyData.IsArray)
                return (uint?)propertyData.Value;
            throw new ArgumentOutOfRangeException(nameof(propertyName));
        }

        public static long GetSInt64(this ManagementObject obj, string propertyName) =>
            GetSInt64((obj ?? throw new ArgumentNullException(nameof(obj))).Properties, propertyName);

        public static long GetSInt64(this PropertyDataCollection properties, string propertyName)
        {
            if (properties is null)
                throw new ArgumentNullException(nameof(properties));
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or empty.", nameof(propertyName));
            PropertyData propertyData = properties[propertyName];
            if (propertyData is not null && propertyData.Type == CimType.SInt64 && !propertyData.IsArray)
                return (long)propertyData.Value;
            throw new ArgumentOutOfRangeException(nameof(propertyName));
        }

        public static long? GetSInt64Opt(this ManagementObject obj, string propertyName) =>
            GetSInt64Opt((obj ?? throw new ArgumentNullException(nameof(obj))).Properties, propertyName);

        public static long? GetSInt64Opt(this PropertyDataCollection properties, string propertyName)
        {
            if (properties is null)
                throw new ArgumentNullException(nameof(properties));
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or empty.", nameof(propertyName));
            PropertyData propertyData = properties[propertyName];
            if (propertyData is not null && propertyData.Type == CimType.SInt64 && !propertyData.IsArray)
                return (long?)propertyData.Value;
            throw new ArgumentOutOfRangeException(nameof(propertyName));
        }

        public static ulong GetUInt64(this ManagementObject obj, string propertyName) =>
            GetUInt64((obj ?? throw new ArgumentNullException(nameof(obj))).Properties, propertyName);

        public static ulong GetUInt64(this PropertyDataCollection properties, string propertyName)
        {
            if (properties is null)
                throw new ArgumentNullException(nameof(properties));
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or empty.", nameof(propertyName));
            PropertyData propertyData = properties[propertyName];
            if (propertyData is not null && propertyData.Type == CimType.UInt64 && !propertyData.IsArray)
                return (ulong)propertyData.Value;
            throw new ArgumentOutOfRangeException(nameof(propertyName));
        }

        public static ulong? GetUInt64Opt(this ManagementObject obj, string propertyName) =>
            GetUInt64Opt((obj ?? throw new ArgumentNullException(nameof(obj))).Properties, propertyName);

        public static ulong? GetUInt64Opt(this PropertyDataCollection properties, string propertyName)
        {
            if (properties is null)
                throw new ArgumentNullException(nameof(properties));
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or empty.", nameof(propertyName));
            PropertyData propertyData = properties[propertyName];
            if (propertyData is not null && propertyData.Type == CimType.UInt64 && !propertyData.IsArray)
                return (ulong?)propertyData.Value;
            throw new ArgumentOutOfRangeException(nameof(propertyName));
        }

        public static DateTime GetDateTime(this ManagementObject obj, string propertyName) =>
            GetDateTime((obj ?? throw new ArgumentNullException(nameof(obj))).Properties, propertyName);

        public static DateTime GetDateTime(this PropertyDataCollection properties, string propertyName)
        {
            if (properties is null)
                throw new ArgumentNullException(nameof(properties));
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or empty.", nameof(propertyName));
            PropertyData propertyData = properties[propertyName];
            if (propertyData is not null && propertyData.Type == CimType.DateTime && !propertyData.IsArray)
            {
                if (propertyData.Value is DateTime dateTime)
                    return dateTime;
                return DateTime.Parse((string)propertyData.Value);
            }
            throw new ArgumentOutOfRangeException(nameof(propertyName));
        }

        public static DateTime? GetDateTimeOpt(this ManagementObject obj, string propertyName) =>
            GetDateTimeOpt((obj ?? throw new ArgumentNullException(nameof(obj))).Properties, propertyName);

        public static DateTime? GetDateTimeOpt(this PropertyDataCollection properties, string propertyName)
        {
            if (properties is null)
                throw new ArgumentNullException(nameof(properties));
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or empty.", nameof(propertyName));
            PropertyData propertyData = properties[propertyName];
            if (propertyData is not null && propertyData.Type == CimType.DateTime && !propertyData.IsArray)
            {
                if (propertyData.Value is DateTime dateTime)
                    return dateTime;
                string s = (string)propertyData.Value;
                return string.IsNullOrEmpty(s) ? null : DateTime.Parse((string)propertyData.Value);
            }
            throw new ArgumentOutOfRangeException(nameof(propertyName));
        }

        public static char GetChar16(this ManagementObject obj, string propertyName) =>
            GetChar16((obj ?? throw new ArgumentNullException(nameof(obj))).Properties, propertyName);

        public static char GetChar16(this PropertyDataCollection properties, string propertyName)
        {
            if (properties is null)
                throw new ArgumentNullException(nameof(properties));
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or empty.", nameof(propertyName));
            PropertyData propertyData = properties[propertyName];
            if (propertyData is not null && propertyData.Type == CimType.Char16 && !propertyData.IsArray)
                return (char)propertyData.Value;
            throw new ArgumentOutOfRangeException(nameof(propertyName));
        }

        public static char? GetChar16Opt(this ManagementObject obj, string propertyName) =>
            GetChar16Opt((obj ?? throw new ArgumentNullException(nameof(obj))).Properties, propertyName);

        public static char? GetChar16Opt(this PropertyDataCollection properties, string propertyName)
        {
            if (properties is null)
                throw new ArgumentNullException(nameof(properties));
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or empty.", nameof(propertyName));
            PropertyData propertyData = properties[propertyName];
            if (propertyData is not null && propertyData.Type == CimType.Char16 && !propertyData.IsArray)
                return (char?)propertyData.Value;
            throw new ArgumentOutOfRangeException(nameof(propertyName));
        }
    }
}

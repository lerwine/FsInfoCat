using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml;

namespace FsInfoCat.PS
{
    public static class ValueHelperExtensions
    {
        public static string ToDateTimeXml(this DateTime value) => value.ToString("yyyy-MM-dd HH:mm:ss");

        public static string ToDateTimeXml(this DateTime? value) => value?.ToDateTimeXml();

        public static DateTime FromXmlDateTime(this string value, DateTime defaultValue)
        {
            if (value is null || (value = value.Trim()).Length == 0 || !DateTime.TryParse(value, out DateTime dateTime))
                return defaultValue;
            return dateTime;
        }

        public static DateTime? FromXmlDateTime(this string value)
        {
            if (value is null || (value = value.Trim()).Length == 0 || !DateTime.TryParse(value, out DateTime dateTime))
                return null;
            return dateTime;
        }

        public static string ToGuidXml(this Guid value) => XmlConvert.ToString(value);

        public static string ToGuidXml(this Guid? value) => value?.ToGuidXml();

        public static Guid? FromXmlGuid(this string value)
        {
            if (value is not null && (value = value.Trim()).Length > 0)
                try { return XmlConvert.ToGuid(value); } catch { }
            return null;
        }

        public static Guid FromXmlGuid(this string value, Guid defaultValue) => value.FromXmlGuid() ?? defaultValue;

        public static string ToDriveTypeXml(this DriveType value) => (value == default) ? null : XmlConvert.ToString((int)value);

        public static string ToDriveTypeXml(this DriveType? value) => value.HasValue ? XmlConvert.ToString((int)value.Value) : null;

        public static DriveType FromXmlDriveType(this string value, DriveType defaultValue) => (DriveType)FromXmlInt32(value, (int)defaultValue);

        public static DriveType? FromXmlDriveType(this string value) => (DriveType?)value.FromXmlInt32();

        public static string ToFileCrawlOptionsXml(this FileCrawlOptions value) => (value == default) ? null : XmlConvert.ToString((byte)value);

        public static string ToFileCrawlOptionsXml(this FileCrawlOptions? value) => value.HasValue ? XmlConvert.ToString((byte)value.Value) : null;

        public static FileCrawlOptions FromXmlFileCrawlOptions(this string value, FileCrawlOptions defaultValue) => (FileCrawlOptions)FromXmlByte(value, (byte)defaultValue);

        public static FileCrawlOptions? FromXmlFileCrawlOptions(this string value) => (FileCrawlOptions?)value.FromXmlByte();

        public static string ToDirectoryCrawlOptionsXml(this DirectoryCrawlOptions value) => (value == default) ? null : XmlConvert.ToString((byte)value);

        public static string ToDirectoryCrawlOptionsXml(this DirectoryCrawlOptions? value) => value.HasValue ? XmlConvert.ToString((byte)value.Value) : null;

        public static DirectoryCrawlOptions FromXmlDirectoryCrawlOptions(this string value, DirectoryCrawlOptions defaultValue) => (DirectoryCrawlOptions)FromXmlByte(value, (byte)defaultValue);

        public static DirectoryCrawlOptions? FromXmlDirectoryCrawlOptions(this string value) => (DirectoryCrawlOptions?)value.FromXmlByte();

        public static string ToDirectoryStatusXml(this DirectoryStatus value) => (value == default) ? null : XmlConvert.ToString((byte)value);

        public static string ToDirectoryStatusXml(this DirectoryStatus? value) => value.HasValue ? XmlConvert.ToString((byte)value.Value) : null;

        public static DirectoryStatus FromXmlDirectoryStatus(this string value, DirectoryStatus defaultValue) => (DirectoryStatus)FromXmlByte(value, (byte)defaultValue);

        public static DirectoryStatus? FromXmlDirectoryStatus(this string value) => (DirectoryStatus?)value.FromXmlByte();

        public static string ToVolumeStatusXml(this VolumeStatus value) => (value == default) ? null : XmlConvert.ToString((byte)value);

        public static string ToVolumeStatusXml(this VolumeStatus? value) => value.HasValue ? XmlConvert.ToString((byte)value.Value) : null;

        public static VolumeStatus FromXmlVolumeStatus(this string value, VolumeStatus defaultValue) => (VolumeStatus)FromXmlByte(value, (byte)defaultValue);

        public static VolumeStatus? FromXmlVolumeStatus(this string value) => (VolumeStatus?)value.FromXmlByte();

        public static string ToAccessErrorCodeXml(this AccessErrorCode value) => (value == default) ? null : XmlConvert.ToString((int)value);

        public static string ToAccessErrorCodeXml(this AccessErrorCode? value) => value.HasValue ? XmlConvert.ToString((int)value.Value) : null;

        public static AccessErrorCode FromAccessErrorCode(this string value, AccessErrorCode defaultValue) => (AccessErrorCode)FromXmlInt32(value, (int)defaultValue);

        public static AccessErrorCode? FromAccessErrorCode(this string value) => (AccessErrorCode?)value.FromXmlInt32();

        public static string ToBooleanXml(this bool value, bool defaultValue = false) => (value == defaultValue) ? null : XmlConvert.ToString(value);

        public static string ToBooleanXml(this bool? value) => value.HasValue ? XmlConvert.ToString(value.Value) : null;

        public static bool? FromXmlBoolean(this string value)
        {
            if (value is not null && (value = value.Trim()).Length > 0)
                try { return XmlConvert.ToBoolean(value); } catch { }
            return null;
        }

        public static bool FromXmlBoolean(this string value, bool defaultValue) => value.FromXmlBoolean() ?? defaultValue;

        public static string ToByteXml(this byte value, byte defaultValue) => (value == defaultValue) ? null : XmlConvert.ToString(value);

        public static string ToByteXml(this byte? value) => value.HasValue ? XmlConvert.ToString(value.Value) : null;

        public static byte? FromXmlByte(this string value)
        {
            if (value is not null && (value = value.Trim()).Length > 0)
                try { return XmlConvert.ToByte(value); } catch { }
            return null;
        }

        public static byte FromXmlByte(this string value, byte defaultValue) => value.FromXmlByte() ?? defaultValue;

        public static string ToUInt16Xml(this ushort value, ushort defaultValue) => (value == defaultValue) ? null : XmlConvert.ToString(value);

        public static string ToUInt16Xml(this ushort? value) => value.HasValue ? XmlConvert.ToString(value.Value) : null;

        public static ushort? FromXmlUInt16(this string value)
        {
            if (value is not null && (value = value.Trim()).Length > 0)
                try { return XmlConvert.ToUInt16(value); } catch { }
            return null;
        }

        public static ushort FromXmlUInt16(this string value, ushort defaultValue) => value.FromXmlUInt16() ?? defaultValue;

        public static string ToInt32Xml(this int value, int defaultValue) => (value == defaultValue) ? null : XmlConvert.ToString(value);

        public static string ToInt32Xml(this int? value) => value.HasValue ? XmlConvert.ToString(value.Value) : null;

        public static int? FromXmlInt32(this string value)
        {
            if (value is not null && (value = value.Trim()).Length > 0)
                try { return XmlConvert.ToInt32(value); } catch { }
            return null;
        }

        public static int FromXmlInt32(this string value, int defaultValue) => value.FromXmlInt32() ?? defaultValue;

        public static string ToUInt32Xml(this uint value, uint defaultValue) => (value == defaultValue) ? null : XmlConvert.ToString(value);

        public static string ToUInt32Xml(this uint? value) => value.HasValue ? XmlConvert.ToString(value.Value) : null;

        public static uint? FromXmlUInt32(this string value)
        {
            if (value is not null && (value = value.Trim()).Length > 0)
                try { return XmlConvert.ToUInt32(value); } catch { }
            return null;
        }

        public static uint FromXmlUInt32(this string value, uint defaultValue) => value.FromXmlUInt32() ?? defaultValue;

        public static string ToUInt64Xml(this ulong value, ulong defaultValue) => (value == defaultValue) ? null : XmlConvert.ToString(value);

        public static string ToUInt64Xml(this ulong? value) => value.HasValue ? XmlConvert.ToString(value.Value) : null;

        public static ulong? FromXmlUInt64(this string value)
        {
            if (value is not null && (value = value.Trim()).Length > 0)
                try { return XmlConvert.ToUInt64(value); } catch { }
            return null;
        }

        public static ulong FromXmlUInt64(this string value, ulong defaultValue) => value.FromXmlUInt64() ?? defaultValue;

        public static string EmptyIfNull(this string value) => (value is null) ? "" : value;

        public static Collection<T> EmptyIfNull<T>(this Collection<T> value) => (value is null) ? new Collection<T>() : value;

        public static string NullIfWhitespace(this string value) => string.IsNullOrWhiteSpace(value) ? null : value;

        public static string TrimmedNotNull(this string value) => (value is null) ? "" : value.Trim();

        public static string ToAbsoluteUri(this Uri uri) => (uri is null || !uri.IsAbsoluteUri) ? null : uri.AbsoluteUri;

        public static Uri ToAbsoluteUri(this string value) => (string.IsNullOrWhiteSpace(value) || !Uri.TryCreate(value, UriKind.Absolute, out Uri uri)) ? null : uri;
    }
}

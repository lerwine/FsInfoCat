using FsInfoCat.Models.DB;
using FsInfoCat.Models.Volumes;
using FsInfoCat.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace FsInfoCat.Models
{
    public static class ModelHelper
    {
        public const string Role_Name_Viewer = "viewer";
        public const string Role_Name_User = "user";
        public const string Role_Name_App_Contrib = "app-contrib";
        public const string Role_Name_Admin = "admin";
        public const string PATTERN_DOTTED_NAME = @"(?i)^\s*([a-z][a-z\d_]*(\.[a-z][a-z\d_]*)*)\s*$";
        public const string PATTERN_MACHINE_NAME = @"^\s*(?=.{1,255}$)[0-9A-Za-z](?:(?:[0-9A-Za-z]|-){0,61}[0-9A-Za-z])?(?:\.[0-9A-Za-z](?:(?:[0-9A-Za-z]|-){0,61}[0-9A-Za-z])?)*\.?\s*$";
        public const string PATTERN_PATH_OR_URL = @"(?i)^([a-z]:[\\/]$|file:///[a-z]:/$|([a-z]:|[\\/]{2}([a-z]+(-[a-z\d]+)*(\.[a-z]+(-[a-z\d]+)*)*|(([01]\d?|[3-9])\d?|2(5[0-5]?|[0-4]?\d?)?)(\.(([01]\d?|[3-9])\d?|2(5[0-5]?|[0-4]?\d?)?)){3}))([\\/][^\\/:""<>|*?\x00-\x19]+)+[\\/]?$|file://(/[a-z]:|[a-z]+(-[a-z\d]+)*(\.[a-z]+(-[a-z\d]+)*)*|(([01]\d?|[3-9])\d?|2(5[0-5]?|[0-4]?\d?)?)(\.(([01]\d?|[3-9])\d?|2(5[0-5]?|[0-4]?\d?)?)){3})?([\\/][^\\/:""<>|*?\x00-\x19]+)+/?$)";
        public const string PATTERN_BASE64 = @"^\s*(([A-Za-z\d+/])\s*)?$";
        public static readonly Regex DottedNameRegex = new Regex(PATTERN_DOTTED_NAME, RegexOptions.Compiled);
        public static readonly Regex MachineNameRegex = new Regex(PATTERN_MACHINE_NAME, RegexOptions.Compiled);
        public static readonly Regex PathOrUrlRegex = new Regex(PATTERN_PATH_OR_URL, RegexOptions.Compiled);
        public static readonly Regex NonNormalWsRegex = new Regex(@" \s+|(?! )\s+", RegexOptions.Compiled);
        public static readonly Regex Base64Regex = new Regex(PATTERN_BASE64, RegexOptions.Compiled);

        public static string CoerceAsString(object baseValue) => (baseValue is null) ? "" : ((baseValue is string) ? (string)baseValue : baseValue.ToString());

        public static string CoerceAsTrimmedString(object baseValue) => (baseValue is null) ? "" : ((baseValue is string) ? (string)baseValue : baseValue.ToString()).Trim();

        public static string CoerceAsWsNormalizedString(object baseValue) => CoerceAsWsNormalized((baseValue is null || baseValue is string) ? baseValue as string : baseValue.ToString());

        public static Guid CoerceAsGuid(object baseValue) => (null != baseValue && baseValue is Guid) ? (Guid)baseValue : Guid.Empty;

        public static bool CoerceAsBoolean(object baseValue) => (null != baseValue && baseValue is bool) ? (bool)baseValue : false;

        public static string CoerceAsNonNull(this string value) => (value is null) ? "" : value;

        public static string CoerceAsTrimmed(this string value) => (value is null) ? "" : value.Trim();

        public static string CoerceAsWsNormalized(this string value) => ((value = CoerceAsTrimmed(value)).Length > 0) ? NonNormalWsRegex.Replace(value, " ") : value;

        public static DateTime CoerceAsLocalTime(this DateTime value)
        {
            switch (value.Kind)
            {
                case DateTimeKind.Unspecified:
                    return new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second, value.Millisecond, DateTimeKind.Local);
                case DateTimeKind.Local:
                    return value;
                default:
                    return value.ToLocalTime();
            }
        }

        public static DateTime CoerceAsLocalTimeOrNow(this DateTime? value) => (value.HasValue) ? CoerceAsLocalTime(value.Value) : DateTime.Now;

        public static DateTime CoerceAsLocalTimeOrDefault(this DateTime? value, DateTime defaultValue) => CoerceAsLocalTime(value ?? defaultValue);

        public static DateTime CoerceAsUniversalTime(this DateTime value)
        {
            switch (value.Kind)
            {
                case DateTimeKind.Unspecified:
                    return new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second, value.Millisecond, DateTimeKind.Utc);
                case DateTimeKind.Utc:
                    return value;
                default:
                    return value.ToUniversalTime();
            }
        }

        public static DateTime CoerceAsUniversalTimeOrNow(this DateTime? value) => (value.HasValue) ? CoerceAsUniversalTime(value.Value) : DateTime.UtcNow;

        public static DateTime CoerceAsUniversalTimeOrDefault(this DateTime? value, DateTime defaultValue) => CoerceAsUniversalTime(value ?? defaultValue);

        public static IList<ValidationResult> ValidateForSave(this IModficationAuditable target, Account modifiedBy, bool isCreate)
        {
            if (target is null)
                throw new ArgumentNullException("target");
            if (modifiedBy is null)
                throw new ArgumentNullException("modifiedBy");
            target.ModifiedOn = DateTime.Now;
            target.ModifiedBy = modifiedBy.AccountID;
            if (isCreate)
            {
                target.CreatedOn = target.ModifiedOn;
                target.CreatedBy = target.ModifiedBy;
            }
            else
            {
                if (null == (target.CreatedOn = (target.CreatedOn)))
                    target.CreatedOn = CoerceAsLocalTimeOrDefault(target.CreatedOn, target.ModifiedOn);
            }
            return target.ValidateAll();
        }

        public static bool TryFindVolume<T>(this IEnumerable<T> volumes, DirectoryInfo directoryInfo, out T result)
            where T : class, IVolumeInfo
        {
            if (volumes is null || directoryInfo is null)
            {
                result = null;
                return false;
            }
            FileUri fileUri = new FileUri(directoryInfo);
            result = volumes.FirstOrDefault(v => v.RootUri.Equals(fileUri));
            if (result is null)
                return TryFindVolume(volumes, directoryInfo.Parent, out result);
            return true;
        }

        public static Accounts.UserRole ToUserRole(byte value)
        {
            return Enum.GetValues(typeof(Accounts.UserRole)).Cast<Accounts.UserRole>().Where(t => t.Equals(value)).FirstOrDefault();
        }

        public static HostDevices.PlatformType ToPlatformType(byte value)
        {
            return Enum.GetValues(typeof(HostDevices.PlatformType)).Cast<HostDevices.PlatformType>().Where(t => t.Equals(value)).FirstOrDefault();
        }
    }
}

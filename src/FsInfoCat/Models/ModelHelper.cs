using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace FsInfoCat.Models
{
    public class ModelHelper
    {
        public const string PATTERN_DOTTED_NAME = @"^\s*([a-z][a-z\d_]*(\.[a-z][a-z\d_]*))\s*$";
        public const string PATTERN_MACHINE_NAME = @"^\s*(?=.{1,255}$)[0-9A-Za-z](?:(?:[0-9A-Za-z]|-){0,61}[0-9A-Za-z])?(?:\.[0-9A-Za-z](?:(?:[0-9A-Za-z]|-){0,61}[0-9A-Za-z])?)*\.?\s*$";
        public const string PATTERN_PATH_OR_URL = @"(?i)^([a-z]:[\\/]$|file:///[a-z]:/$|([a-z]:|[\\/]{2}([a-z]+(-[a-z\d]+)*(\.[a-z]+(-[a-z\d]+)*)*|(([01]\d?|[3-9])\d?|2(5[0-5]?|[0-4]?\d?)?)(\.(([01]\d?|[3-9])\d?|2(5[0-5]?|[0-4]?\d?)?)){3}))([\\/][^\\/:""<>|*?\x00-\x19]+)+[\\/]?$|file://(/[a-z]:|[a-z]+(-[a-z\d]+)*(\.[a-z]+(-[a-z\d]+)*)*|(([01]\d?|[3-9])\d?|2(5[0-5]?|[0-4]?\d?)?)(\.(([01]\d?|[3-9])\d?|2(5[0-5]?|[0-4]?\d?)?)){3})?([\\/][^\\/:""<>|*?\x00-\x19]+)+/?$)";
        public const string PATTERN_BASE64 = @"^\s*([A-Za-z\d+/])\s*)?$";
        public static readonly Regex DottedNameRegex = new Regex(PATTERN_DOTTED_NAME, RegexOptions.Compiled);
        public static readonly Regex MachineNameRegex = new Regex(PATTERN_MACHINE_NAME, RegexOptions.Compiled);
        public static readonly Regex PathOrUrlRegex = new Regex(PATTERN_PATH_OR_URL, RegexOptions.Compiled);
        public static readonly Regex NonNormalWsRegex = new Regex(@" \s+|(?! )\s+", RegexOptions.Compiled);
        public static readonly Regex Base64Regex = new Regex(PATTERN_BASE64, RegexOptions.Compiled);
        public static string CoerceAsString(object baseValue) => (null == baseValue) ? "" : ((baseValue is string) ? (string)baseValue : baseValue.ToString());
        public static string CoerceAsTrimmedString(object baseValue) => (null == baseValue) ? "" : ((baseValue is string) ? (string)baseValue : baseValue.ToString()).Trim();
        public static string CoerceAsWsNormalizedString(object baseValue) => CoerceAsWsNormalized((null == baseValue || baseValue is string) ? baseValue as string : baseValue.ToString());
        public static Guid CoerceAsGuid(object baseValue) => (null != baseValue && baseValue is Guid) ? (Guid)baseValue : Guid.Empty;
        public static bool CoerceAsBoolean(object baseValue) => (null != baseValue && baseValue is bool) ? (bool)baseValue : false;
        public static string CoerceAsNonNull(string value) => (null == value) ? "" : value;
        public static string CoerceAsTrimmed(string value) => (null == value) ? "" : value.Trim();
        public static string CoerceAsWsNormalized(string value) => ((value = CoerceAsTrimmed(value)).Length > 0) ? NonNormalWsRegex.Replace(value, " ") : value;
        public static DateTime CoerceAsLocalTime(DateTime value) => (null != value && value.Kind == DateTimeKind.Local) ? value : value.ToLocalTime();
        public static DateTime CoerceAsLocalTimeOrNow(DateTime value) => (null != (value = CoerceAsLocalTime(value))) ? value : DateTime.Now;
        public static DateTime CoerceAsLocalTimeOrDefault(DateTime value, DateTime defaultValue) => (null != (value = CoerceAsLocalTime(value))) ? value : CoerceAsLocalTime(default);
        public static DateTime CoerceAsUniversalTime(DateTime value) => (null != value && value.Kind == DateTimeKind.Utc) ? value : value.ToUniversalTime();
        public static DateTime CoerceAsUniversalTimeOrNow(DateTime value) => (null != (value = CoerceAsUniversalTime(value))) ? value : DateTime.UtcNow;
        public static DateTime CoerceAsUniversalTimeOrDefault(DateTime value, DateTime defaultValue) => (null != (value = CoerceAsUniversalTime(value))) ? value : CoerceAsUniversalTime(defaultValue);
        public static IList<ValidationResult> ValidateForSave(IModficationAuditable target, Account modifiedBy, bool isCreate)
        {
            if (null == target)
                throw new ArgumentNullException("target");
            if (null == modifiedBy)
                throw new ArgumentNullException("modifiedBy");
            target.ModifiedOn = DateTime.Now;
            if (isCreate)
            {
                target.CreatedOn = target.ModifiedOn;
                if (target is IModificationAccountAuditable)
                {
                    IModificationAccountAuditable a = (IModificationAccountAuditable)target;
                    a.CreatedBy = a.ModifiedBy = modifiedBy;
                }
                target.CreatedBy = target.ModifiedBy = modifiedBy.AccountID;
            }
            else
            {
                if (null == (target.CreatedOn = (target.CreatedOn)))
                    target.CreatedOn = CoerceAsLocalTimeOrDefault(target.CreatedOn, target.ModifiedOn);
                if (target is IModificationAccountAuditable)
                {
                    IModificationAccountAuditable a = (IModificationAccountAuditable)target;
                    a.ModifiedBy = modifiedBy;
                    if (null == a.CreatedBy)
                    {
                        a.CreatedBy = modifiedBy;
                        target.CreatedBy = modifiedBy.AccountID;
                    }
                }
                target.ModifiedBy = modifiedBy.AccountID;
            }
            return target.ValidateAll();
        }
    }
}

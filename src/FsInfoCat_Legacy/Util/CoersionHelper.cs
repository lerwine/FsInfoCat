using System;
using System.Text.RegularExpressions;

namespace FsInfoCat.Util
{
    public static class CoersionHelper
    {
        public static readonly Regex NonNormalWsRegex = new Regex(@" \s+|(?! )\s+", RegexOptions.Compiled);

        public static string CoerceAsString(object baseValue) => (baseValue is null) ? "" : ((baseValue is string s) ? s : baseValue.ToString());

        public static string CoerceAsTrimmedString(object baseValue) => (baseValue is null) ? "" : ((baseValue is string t) ? t : baseValue.ToString()).Trim();

        public static string CoerceAsWsNormalizedString(object baseValue) => CoerceAsWsNormalized((baseValue is null || baseValue is string) ? baseValue as string : baseValue.ToString());

        public static Guid CoerceAsGuid(object baseValue) => (!(baseValue is null) && baseValue is Guid g) ? g : Guid.Empty;

        public static bool CoerceAsBoolean(object baseValue) => !(baseValue is null) && baseValue is bool b && b;

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

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FsInfoCat
{
    public static class StringExtensionMethods
    {
        public static readonly Regex NewLineRegex = new(@"\r\n?|\n", RegexOptions.Compiled);

        public static readonly Regex AbnormalWsRegex = new(@" [\s\p{Z}\p{C}]+|(?! )[\s\p{Z}\p{C}]+", RegexOptions.Compiled);

        public static readonly Regex OuterWsRegex = new(@"^[\s\p{Z}\p{C}]+|[\s\p{Z}\p{C}]+$", RegexOptions.Compiled);

        public static string AsWsNormalizedOrEmpty(this string text) => (text is null || text.Length == 0 || (text = OuterWsRegex.Replace(text, "")).Length == 0) ? "" :
            (AbnormalWsRegex.IsMatch(text) ? AbnormalWsRegex.Replace(text, " ") : text);

        public static string NullIfWhiteSpaceOrNormalized(this string text) => (text is null || text.Length == 0 || (text = OuterWsRegex.Replace(text, "")).Length == 0) ? null :
            (AbnormalWsRegex.IsMatch(text) ? AbnormalWsRegex.Replace(text, " ") : text);

        public static string DefaultIfNullOrEmpty(this string text, string defaultValue) => ExtensionMethods.DefaultIf(text, string.IsNullOrEmpty, defaultValue);

        public static string DefaultIfNullOrWhiteSpace(this string text, string defaultValue) => ExtensionMethods.DefaultIf(text, string.IsNullOrWhiteSpace, defaultValue);

        public static string GetDefaultIfNullOrEmpty(this string text, Func<string> getDefaultValue) => ExtensionMethods.GetDefaultIf(text, string.IsNullOrEmpty, getDefaultValue);

        public static string GetDefaultIfNullOrWhiteSpace(this string text, Func<string> getDefaultValue) =>
            ExtensionMethods.GetDefaultIf(text, string.IsNullOrWhiteSpace, getDefaultValue);

        public static string NullIfEmpty(this string source) => string.IsNullOrEmpty(source) ? null : source;

        public static string NullIfWhiteSpace(this string source) => string.IsNullOrWhiteSpace(source) ? null : source;

        public static string AsNonNullTrimmed(this string text) => string.IsNullOrWhiteSpace(text) ? "" : text.Trim();

        public static string TrimmedOrNullIfEmpty(this string text) => string.IsNullOrEmpty(text) ? null : text.Trim();

        public static string TrimmedOrNullIfWhiteSpace(this string text) => (text is null || (text = text.Trim()).Length == 0) ? null : text;

        public static string[] SplitLines(this string text) => (text is null) ? Array.Empty<string>() : NewLineRegex.Split(text);

        public static string JoinWithNewLines(this IEnumerable<string> text) => (text is null || !text.Any()) ? null : string.Join(Environment.NewLine, text);

        public static IEnumerable<string> ElementsNotNullOrEmpty(this IEnumerable<string> text) => text?.Where(s => !string.IsNullOrEmpty(s));

        public static IEnumerable<string> ElementsNotNullOrWhiteSpace(this IEnumerable<string> text) => text.Where(s => !string.IsNullOrWhiteSpace(s));

        public static bool AllNullOrWhiteSpace(this IEnumerable<string> text) => text is null || !text.Any() || text.All(string.IsNullOrWhiteSpace);

        public static IEnumerable<string> NonEmptyTrimmedElements(this IEnumerable<string> text) => text?.Where(s => s is not null).Select(s => s.Trim())
            .Where(s => s.Length > 0);

        public static IEnumerable<string> AsNonNullTrimmedValues(this IEnumerable<string> text) => text?.Select(AsNonNullTrimmed);

        public static IEnumerable<string> AsWsNormalizedOrEmptyValues(this IEnumerable<string> text) => text?.Select(AsWsNormalizedOrEmpty);

        public static IEnumerable<string> AsNonNullValues(this IEnumerable<string> text) => text?.Select(t => t ?? "");

        public static IEnumerable<string> AsOrderedDistinct(this IEnumerable<string> text) => text?.Select(t => t ?? "").Distinct().OrderBy(t => t);

        public static string EmptyIfNullOrWhiteSpace(this string source) => string.IsNullOrWhiteSpace(source) ? "" : source;

        public static IEnumerable<string> ValuesEmptyIfNullOrWhiteSpace(this IEnumerable<string> source) => source?.Select(EmptyIfNullOrWhiteSpace);
    }
}

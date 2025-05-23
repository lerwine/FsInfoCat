using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FsInfoCat
{
    // TODO: Document StringExtensionMethods class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static partial class StringExtensionMethods
    {
        public static readonly Regex NewLineRegex = GetNewLineRegex();

        public static readonly Regex AbnormalWsRegex = GetAbnormalRegex();

        public static readonly Regex OuterWsRegex = GetOuterWsRegex();

        public static readonly Regex SafeBreakRegex = GetSafeBreakRegex();

        public static string ToKeyValueListString(this IEnumerable<ValueTuple<string, string>> source, bool includeEmpty = false)
        {
            if (source is null)
                return null;
            return string.Join("; ", (includeEmpty ? source : source.Where(t => !string.IsNullOrWhiteSpace(t.Item2))).Select(t => $"{t.Item1}: {t.Item2}"));
        }

        public static string TruncateWithElipses(this string text, int maxLength)
        {
            if (maxLength < 2)
                throw new ArgumentOutOfRangeException(nameof(maxLength));
            if (text is null || text.Length <= maxLength)
                return text;
            Match match = SafeBreakRegex.Match(text, 0, maxLength);
            if (match.Success)
                return $"{match.Value}…";
            return $"{text[0..(maxLength-1)]}…";
        }

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

        public static string[] SplitLines(this string text) => (text is null) ? [] : NewLineRegex.Split(text);

        public static string JoinWithNewLines(this IEnumerable<string> text) => (text is null || !text.Any()) ? null : string.Join(Environment.NewLine, text);

        public static IEnumerable<string> ElementsNotNullOrEmpty(this IEnumerable<string> text) => text?.Where(s => !string.IsNullOrEmpty(s));

        public static IEnumerable<string> ElementsNotNullOrWhiteSpace(this IEnumerable<string> text) => text.Where(s => !string.IsNullOrWhiteSpace(s));

        public static bool AllNullOrWhiteSpace(this IEnumerable<string> text) => text is null || !text.Any() || text.All(string.IsNullOrWhiteSpace);

        public static IEnumerable<string> NonEmptyTrimmedElements(this IEnumerable<string> text) => text?.Where(s => s is not null).Select(s => s.Trim())
            .Where(s => s.Length > 0);

        public static IEnumerable<string> AsNonNullTrimmedValues(this IEnumerable<string> text) => text?.Select(AsNonNullTrimmed);

        public static IEnumerable<string> AsWsNormalizedOrEmptyValues(this IEnumerable<string> text) => text?.Select(AsWsNormalizedOrEmpty);

        public static string ToNormalizedDelimitedText(this IEnumerable<string> source, bool includeEmpty = false, string delimiter = "; ")
        {
            if (source is null || !(source = includeEmpty ? source.AsWsNormalizedOrEmptyValues(): source.AsWsNormalizedOrEmptyValues().Where(s => s.Length > 0)).Any())
                return "";
            return string.Join(delimiter ?? "", source);
        }

        public static string ToNormalizedDelimitedText(this IEnumerable<string> source, string delimiter) => ToNormalizedDelimitedText(source, false, delimiter);

        public static IEnumerable<string> AsNonNullValues(this IEnumerable<string> text) => text?.Select(t => t ?? "");

        public static IEnumerable<string> AsOrderedDistinct(this IEnumerable<string> text) => text?.Select(t => t ?? "").Distinct().OrderBy(t => t);

        public static string EmptyIfNullOrWhiteSpace(this string source) => string.IsNullOrWhiteSpace(source) ? "" : source;

        public static IEnumerable<string> ValuesEmptyIfNullOrWhiteSpace(this IEnumerable<string> source) => source?.Select(EmptyIfNullOrWhiteSpace);
        
        [GeneratedRegex(@"\r\n?|\n", RegexOptions.Compiled)]
        private static partial Regex GetNewLineRegex();

        [GeneratedRegex(@" [\s\p{Z}\p{C}]+|(?! )[\s\p{Z}\p{C}]+", RegexOptions.Compiled)]
        private static partial Regex GetAbnormalRegex();

        [GeneratedRegex(@"^[\s\p{Z}\p{C}]+|[\s\p{Z}\p{C}]+$", RegexOptions.Compiled)]
        private static partial Regex GetOuterWsRegex();

        [GeneratedRegex(@"^[\s\p{Z}\p{C}]*[^\s\p{Z}\p{C}]+(?=[\s\p{Z}\p{C}])([\s\p{Z}\p{C}]+[^\s\p{Z}\p{C}]+(?=[\s\p{Z}\p{C}]))*|^[\s\p{Z}\p{C}\p{P}\p{M}]*[^\s\p{Z}\p{C}\p{P}\p{M}]+(?=[\p{P}\p{M}])([\p{P}\p{M}]+[^\p{P}\p{M}]+(?=\p{P}))*[\p{P}\p{M}](?=.)|[\s\p{Z}\p{C}\p{P}\p{M}\p{S}]*[^\s\p{Z}\p{C}\p{S}]+(?=\p{S})(\p{S}+\P{S}+(?=\p{S}))*", RegexOptions.Compiled)]
        private static partial Regex GetSafeBreakRegex();
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}

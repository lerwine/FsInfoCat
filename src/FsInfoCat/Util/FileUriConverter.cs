using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using FsInfoCat.Models;

namespace FsInfoCat.Util
{
    public abstract class FileUriConverter
    {
        public const string URI_SCHEME_URN = UriHelper.URI_SCHEME_URN;
        public const string URN_NAMESPACE_UUID = UriHelper.URN_NAMESPACE_UUID;
        public const string URN_NAMESPACE_VOLUME = UriHelper.URN_NAMESPACE_VOLUME;
        public const string URN_NAMESPACE_ID = UriHelper.URN_NAMESPACE_ID;
        public const char URI_QUERY_DELIMITER_CHAR = UriHelper.URI_QUERY_DELIMITER_CHAR;
        public const string URI_QUERY_DELIMITER_STRING = UriHelper.URI_QUERY_DELIMITER_STRING;
        public const string URI_QUERY_DELIMITER_ESCAPED = UriHelper.URI_QUERY_DELIMITER_ESCAPED;
        public const char URI_FRAGMENT_DELIMITER_CHAR = UriHelper.URI_FRAGMENT_DELIMITER_CHAR;
        public const string URI_FRAGMENT_DELIMITER_STRING = UriHelper.URI_FRAGMENT_DELIMITER_STRING;
        public const string URI_FRAGMENT_DELIMITER_ESCAPED = UriHelper.URI_FRAGMENT_DELIMITER_ESCAPED;
        public const char URI_SCHEME_SEPARATOR_CHAR = UriHelper.URI_SCHEME_SEPARATOR_CHAR;
        public const string URI_SCHEME_SEPARATOR_STRING = UriHelper.URI_SCHEME_SEPARATOR_STRING;
        public const string URI_SCHEME_DELIMITER_ESCAPED = UriHelper.URI_SCHEME_DELIMITER_ESCAPED;
        public const char URI_USERINFO_SEPARATOR_CHAR = UriHelper.URI_USERINFO_SEPARATOR_CHAR;
        public const string URI_USERINFO_SEPARATOR_STRING = UriHelper.URI_USERINFO_SEPARATOR_STRING;
        public const string URI_USERINFO_DELIMITER_ESCAPED = UriHelper.URI_USERINFO_DELIMITER_ESCAPED;
        public const char URI_PATH_SEPARATOR_CHAR = UriHelper.URI_PATH_SEPARATOR_CHAR;
        public const string URI_PATH_SEPARATOR_STRING = UriHelper.URI_PATH_SEPARATOR_STRING;
        public const string URI_PATH_SEPARATOR_ESCAPED = UriHelper.URI_PATH_SEPARATOR_ESCAPED;
        public const string MATCH_GROUP_NAME_FILE = "file";
        public const string MATCH_GROUP_NAME_HOST = "host";
        public const string MATCH_GROUP_NAME_PATH = "path";
        public const string MATCH_GROUP_NAME_FILE_NAME = "fileName";
        public const string MATCH_GROUP_NAME_IPV2 = "ipv2";
        public const string MATCH_GROUP_NAME_IPV6 = "ipv6";
        public const string MATCH_GROUP_NAME_UNC = "unc";
        public const string MATCH_GROUP_NAME_DNS = "dns";
        public const string MATCH_GROUP_NAME_SCHEME = "scheme";

        /*
         * Patterns
         * Extended Non-Printable Ranges: [\u007f-\u00a0\u1680\u2028\u2029\ud800-\udfff]
         * IPV2: (?=(\d+\.){3}\d+)((2(5[0-5|[0-4]?\d?)?|[01]?\d\d?)(\.|(?![.\d]))){4}
         * IPV6 with host: (?=\[[^:]*(:[^:]*){2,7}\]|[^:]*(:[^:]*){2,7}|[^-]*(-[^-]*){2,7}\.ipv6-literal\.net)\[?(?<ipv6>[a-f\d]{1,4}([:-][a-f\d]{1,4}){7}|(([a-f\d]{1,4}[:-])+|[:-])([:-][a-f\d]{1,4})+)(?<d>\.ipv6-literal\.net)?\]?
         * IPV6: (?=\[[^:]*(:[^:]*){2,7}\]|[^:]*(:[^:]*){2,7})\[?(?<ipv6>[a-f\d]{1,4}(:[a-f\d]{1,4}){7}|(([a-f\d]{1,4}:)+|:)(:[a-f\d]{1,4})+)\]?
         * DNS: (?=[^\s/]{1,255}(![\w-]))[a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?
         */

        /// <summary>
        /// Matches a path segment including optional trailing slash.
        /// </summary>
        public static readonly Regex URI_PATH_SEGMENT_REGEX = new Regex(@"(?:^|\G)(?:/|([^/]+)(?:/|$))", RegexOptions.Compiled);

        public const string PATTERN_HOST_NAME = @"(?i)^\s*((?=(\d+\.){3}\d+)((2(5[0-5|[0-4]?\d?)?|[01]?\d\d?)(\.|(?![.\d]))){4}|(?=\[[^:]*(:[^:]*){2,7}\]|[^:]*(:[^:]*){2,7})\[?([a-f\d]{1,4}(:[a-f\d]{1,4}){7}|(([a-f\d]{1,4}:)+|:)(:[a-f\d]{1,4})+)\]?|(?=[^\s/]{1,255}(![\w-]))[a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?)\s*$";

        /// <summary>
        /// Matches a host name.
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        /// <item><term>ipv2</term> Matches IPV2 address (<see cref="MATCH_GROUP_NAME_IPV2"/>).</item>
        /// <item><term>ipv6</term> Matches an IPV6 address without any surrounding brackets or the UNC domain text (<see cref="MATCH_GROUP_NAME_IPV6"/>).</item>
        /// <item><term>unc</term> Matches the <c>.ipv6-literal.net</c> domain name for IPV6 UNC notation (<see cref="MATCH_GROUP_NAME_UNC"/>).
        /// If this group matches, then hexidecimal groups for the IPV6 address are separated by dashes rather than colons.</item>
        /// <item><term>dns</term> Matches a DNS host name (<see cref="MATCH_GROUP_NAME_DNS"/>).</item>
        /// </list></remarks>
        public static readonly Regex HOST_NAME_W_UNC_REGEX = new Regex(@"^
(
    (?=(\d+\.){3}\d+)
    (?<ipv2>((2(5[0-5|[0-4]?\d?)?|[01]?\d\d?)(\.|(?![.\d]))){4})
|
    (?=\[[^:]*(:[^:]*){2,7}\]|[^:]*(:[^:]*){2,7}|[^-]*(-[^-]*){2,7}\.ipv6-literal\.net)
    \[?
    (?<ipv6>[a-f\d]{1,4}([:-][a-f\d]{1,4}){7}|(([a-f\d]{1,4}[:-])+|[:-])([:-][a-f\d]{1,4})+)
    (?<d>\.ipv6-literal\.net)?
    \]?
|
    (?=[^\s/]{1,255})
    (?<dns>[a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?)
)$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

        /// <summary>
        /// Matches a host name.
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        /// <item><term>ipv2</term> Matches IPV2 address.</item>
        /// <item><term>ipv6</term> Matches an IPV6 address without any surrounding brackets or the UNC domain text.</item>
        /// If this group matches, then hexidecimal groups for the IPV6 address are separated by dashes rather than colons.</item>
        /// <item><term>dns</term> Matches a DNS host name.</item>
        /// </list></remarks>
        public static readonly Regex HOST_NAME_REGEX = new Regex(@"^
(
    (?=(\d+\.){3}\d+)
    (?<ipv2>((2(5[0-5|[0-4]?\d?)?|[01]?\d\d?)(\.|(?![.\d]))){4})
|
    (?=\[[^:]*(:[^:]*){2,7}\]|[^:]*(:[^:]*){2,7})
    \[?
    (?<ipv6>[a-f\d]{1,4}([:-][a-f\d]{1,4}){7}|(([a-f\d]{1,4}[:-])+|[:-])([:-][a-f\d]{1,4})+)
    \]?
|
    (?=[^\s/]{1,255})
    (?<dns>[a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?)
)$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

        public const string PATTERN_DNS_NAME = @"(?i)^\s*(?=\S{1,255})[a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?\s*$";

        /// <summary>
        /// Matches an IPV6 address
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        /// <item><term>ipv6</term> Matches the actual address without any surrounding brackets.</item>
        /// </list></remarks>
        public static readonly Regex IPV6_ADDRESS_REGEX = new Regex(@"^(?=\[[a-f\d]*(:[a-f\d]*){2,7}\]|[a-f\d]*(:[a-f\d]*){2,7})\[?(?<ipv6>[a-f\d]{1,4}(:[a-f\d]{1,4}){7}|(([a-f\d]{1,4}[:-])+|:)(:[a-f\d]{1,4})+)\]?$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public const string PATTERN_ANY_ABS_FILE_URI_OR_LOCAL_LAX = @"(?i)^\s*(((?<file>file)://|[\\/]{2})(?<host>(?=(\d+\.){3}\d+)(?<ipv2>((2(5[0-5|[0-4]?\d?)?|[01]?\d\d?)(\.|(?![.\d]))){4})|(?=\[[^:]*(:[^:]*){2,7}\]|[^:]*(:[^:]*){2,7}|[^-]*(-[^-]*){2,7}\.ipv6-literal\.net)\[?(?<ipv6>[a-f\d]{1,4}([:-][a-f\d]{1,4}){7}|(([a-f\d]{1,4}[:-])+|[:-])([:-][a-f\d]{1,4})+)(?<d>\.ipv6-literal\.net)?\]?|(?=[^\s/]{1,255}(![\w-]))(?<dns>[a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?))|((?<file>file):///)?(?=[a-z]:|/))(?<path>([a-z]:)?([\\/](?=\s*$)|([\\/]([^\u0000-\u0019\u007f-\u00a0\u1680\u2028\u2029\ud800-\udfff\\/%]+|%((?![a-f\d]{2})|0[1-9a-f]|[1-9a-f]))+)*))[\\/]?\s*$";

        public static readonly Regex FILE_URI_COMPONENTS_LAX_REGEX = new Regex(@"^(?<file>file://(?<host>[^/?#]+)?)?(?<path>.+?)((?<fileName>[/](?=$))|(?<fileName>[^/]*)/?)$");

        private static readonly char[] _INVALID_FILENAME_CHARS;

        public static readonly FileUriConverter CURRENT_FACTORY;
        public static readonly FileUriConverter ALT_FACTORY;

        ///// <summary>
        ///// Regular expression which can be used to detect the input string type.
        ///// </summary>
        ///// <remarks>Group names are as follows:
        ///// <list type="bullet">
        ///// <item><see cref="FORMAT_DETECT_MATCH_GROUP_FILE_URL">f</see> - Matches an absolute <seealso cref="Uri.UriSchemeFile">file</seealso> URI string.</item>
        ///// <item><see cref="FORMAT_DETECT_MATCH_GROUP_UNC">u</see> - Matches a UNC path string.</item>
        ///// <item><see cref="PATH_MATCH_GROUP_NAME_HOST">h</see> - Matches the host name.</item>
        ///// <item><see cref="PATH_MATCH_GROUP_NAME_PATH">p</see> - Matches the rooted path string.</item>
        ///// <item><see cref="FORMAT_DETECT_MATCH_GROUP_NON_FILE">x</see> - Matches a URI string that is not a <seealso cref="Uri.UriSchemeFile">file</seealso> URI string.</item>
        ///// </list></remarks>
        //public abstract Regex FormatDetectionRegex { get; }

        ///// <summary>
        ///// Regular expression that matches a valid host name.
        ///// </summary>
        //public abstract Regex HostNameRegex { get; }

        ///// <summary>
        ///// Matches a well-formed URI that can be converted to a valid absolute or relative local path on the target filesystem type.
        ///// </summary>
        ///// <remarks>Group names are as follows:
        ///// <list type="bullet">
        ///// <item><term><seealso cref="PATH_MATCH_GROUP_NAME_ABSOLUTE">a</seealso></term> Text is an absolute file URL. If this group is not matched, then the text is a relative URL that can be used as the path of a file URL.</item>
        ///// <item><term><seealso cref="PATH_MATCH_GROUP_NAME_HOST">h</seealso></term> Matches the host name. This will never match when group <c>a</c> is not matched.</item>1:2:3:4:5:6:7:8
        ///// <item><term><seealso cref="PATH_MATCH_GROUP_NAME_PATH">p</seealso></term> Matches the path. This match will always succeed when entire expression succeeds, even if the path is an empty string.</item>
        ///// </list></remarks>
        //public abstract Regex FileUriStrictRegex { get; }

        ///// <summary>
        ///// Matches a well-formed local path on the target filesystem type.
        ///// </summary>
        ///// <remarks>Group names are as follows:
        ///// <list type="bullet">
        ///// <item><term><seealso cref="PATH_MATCH_GROUP_NAME_ABSOLUTE">a</seealso></term> Text is an absolute local path. If this group is not matched, then the text is a relative local path.</item>
        ///// <item><term><seealso cref="PATH_MATCH_GROUP_NAME_HOST">h</seealso></term> Matches the host name. This implies that the text is a URN.</item>
        ///// <item><term><seealso cref="PATH_MATCH_GROUP_NAME_PATH">p</seealso></term> Matches the path minus the host name. This match will always succeed when entire expression succeeds, even if the path is an empty string.</item>
        ///// </list></remarks>
        //public abstract Regex LocalFsPathRegex { get; }

        /// <summary>
        /// Character which separates path segments on the target filesystem.
        /// </summary>
        public abstract char LocalDirectorySeparatorChar { get;  }

        /// <summary>
        /// Display name for the target filesystem type.
        /// </summary>
        public abstract string FsDisplayName { get;  }

        /// <summary>
        /// The target filesystem platform type.
        /// </summary>
        public abstract PlatformType FsPlatform { get; }

        //public static bool IsAbsoluteGroupMatch(Match match) => match.Success && match.Groups[PATH_MATCH_GROUP_NAME_ABSOLUTE].Success;

        //public static string GetPathGroupMatchValue(Match match)
        //{
        //    if (match.Success)
        //    {
        //        Group g = match.Groups[PATH_MATCH_GROUP_NAME_PATH];
        //        if (g.Success)
        //            return g.Value;
        //    }
        //    return "";
        //}

        //public static string GetHostGroupMatchValue(Match match)
        //{
        //    if (match.Success)
        //    {
        //        Group g = match.Groups[PATH_MATCH_GROUP_NAME_HOST];
        //        if (g.Success)
        //            return g.Value;
        //    }
        //    return "";
        //}

        /// <summary>
        /// Determines whether a string is valid for use as a file name on the local file system.
        /// </summary>
        /// <param name="name">The file name to test.</param>
        /// <returns><see langword="true"/> if <paramref name="name"/> is not <see langword="null"/> or empty and contains only valid local file system characters.</returns>
        public static bool IsValidLocalFileName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return false;
            if (name.Length < _INVALID_FILENAME_CHARS.Length)
                return !name.ToCharArray().Any(c => _INVALID_FILENAME_CHARS.Contains(c));
            return !_INVALID_FILENAME_CHARS.Any(c => name.Contains(c));
        }

        static FileUriConverter()
        {
            char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
            _INVALID_FILENAME_CHARS = invalidFileNameChars.Contains(Path.PathSeparator) ? invalidFileNameChars : invalidFileNameChars.Concat(new char[] { Path.PathSeparator }).ToArray();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                CURRENT_FACTORY = WindowsFileUriConverter.INSTANCE;
                ALT_FACTORY = LinuxFileUriConverter.INSTANCE;
            }
            else
            {
                CURRENT_FACTORY = LinuxFileUriConverter.INSTANCE;
                ALT_FACTORY = WindowsFileUriConverter.INSTANCE;
            }
        }

        /// <summary>
        /// Encodes path characters are valid for the target OS type, are not encoded by <seealso cref="Uri.EscapeUriString(string)"/>, and must be encoded before attempting to parse a <seealso cref="Uri.UriSchemeFile">file</seealso> URI string.
        /// </summary>
        /// <param name="path">The URI-encoded path string.</param>
        /// <returns>The URI-encoded path string with special path characters encoded as well.</returns>
        protected abstract string EscapeSpecialPathChars(string path);

        /// <summary>
        /// Indicates whether a URI-encoded path string contains characters which are acceptable for the current OS type, but have not yet been URI-encoded.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected abstract bool ContainsSpecialPathChars(string path);

        /// <summary>
        /// Converts a URI-compatible host name and URI-encoded absolute path string to a filesystem path string.
        /// </summary>
        /// <param name="host">The URI-compatible host name.</param>
        /// <param name="path">The URI-encoded path string.</param>
        /// <param name="platform">The target platform type that determines the format of the filesystem path string.</param>
        /// <param name="allowAlt">If <see langword="true"/>, allows a valid filesystem path of the platform type which is alternate to the specified <seealso cref="PlatformType"/> to be used if <paramref name="path"/>
        /// is not a valid path according to the <paramref name="platform"/>.</param>
        /// <returns>A filesystem path string.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="host"/> and/or <paramref name="path"/> is invalid.</exception>
        public static string ToFileSystemPath(string host, string path, PlatformType platform, bool allowAlt = false)
        {
            if (allowAlt)
            {
                FileUriConverter currentFactory = GetFactory(platform, out FileUriConverter altFactory);
                if (!currentFactory.IsWellFormedFileSystemPath(path, true) && altFactory.IsWellFormedFileSystemPath(path, true))
                    return altFactory.ToFileSystemPath(host, path);
                return currentFactory.ToFileSystemPath(host, path);
            }
            return GetFactory(platform).ToFileSystemPath(host, path);
        }

        /// <summary>
        /// Converts a URI-compatible host name and URI-encoded path string to a filesystem path string.
        /// </summary>
        /// <param name="host">The URI-compatible host name.</param>
        /// <param name="path">The URI-encoded path string.</param>
        /// <returns>A filesystem path string.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="host"/> and/or <paramref name="path"/> is invalid.</exception>
        public abstract string ToFileSystemPath(string hostName, string path);

        /// <summary>
        /// Converts a local filesystem path string to an URI encoded file name, relative directory path string, and host name.
        /// </summary>
        /// <param name="path">The local filesystem path string to convert.</param>
        /// <param name="hostName">Returns the URI-compatible host name that was referenced within the file system <paramref name="path"/> or <seealso cref="string.Empty"/> if it did not reference a host name.</param>
        /// <param name="directoryName">Returns the URI-encoded relative directory path string. This will never end with a path separator (<c>/</c>) unless it represents the root subdirectory.</param>
        /// <returns>The URI-encoded file name (leaf) portion of the path string or <seealso cref="string.Empty"/> if the file system <paramref name="path"/> referenced the root subdirectory.</returns>
        public abstract string FromFileSystemPath(string path, out string hostName, out string directoryName);

        /// <summary>
        /// Converts a local filesystem path string to an URI encoded relative path string and host name.
        /// </summary>
        /// <param name="path">The local filesystem path string to convert.</param>
        /// <param name="hostName">Returns the URI-compatible host name that was referenced within the file system <paramref name="path"/> or <seealso cref="string.Empty"/> if it did not reference a host name.</param>
        /// <returns>The URI-encoded relative path string. This will never end with a path separator (<c>/</c>) unless it represents the root subdirectory.</returns>
        public abstract string FromFileSystemPath(string path, out string hostName);

        ///// <summary>
        ///// Parses the components of a URI-encoded file path or a file system path string.
        ///// </summary>
        ///// <param name="input">The input string to parse.</param>
        ///// <param name="preferFsPath">If <see langword="true"/>, attempts to parse the <paramref name="input"/> string as a URI-encoded string, first. Otherwise, this will parse the <paramref name="input"/> string as
        ///// a URI string only if it is not a well-formed file system path string.</param>
        ///// <param name="platform"></param>
        ///// <param name="allowAlt"></param>
        ///// <param name="hostName">Returns the host name component contained within the <paramref name="input"/> string or <seealso cref="string.Empty"/> if it did not reference a host name.</param>
        ///// <param name="path">Returns the path component contained within the <paramref name="input"/> string or <seealso cref="string.Empty"/> if only contained the host name or if it was null or empty.</param>
        ///// <returns>A <seealso cref="FileStringFormat"/> indicating the format that was used to parse the <paramref name="input"/> string.</returns>
        //public static FileStringFormat ParseUriOrPath(string input, bool preferFsPath, PlatformType platform, bool allowAlt, out string hostName, out string path)
        //{
        //    if (input is null)
        //    {
        //        hostName = path = "";
        //        if (platform.IsUnixLike() == CURRENT_FACTORY.FsPlatform.IsUnixLike())
        //        {
        //            if (preferFsPath)
        //                return FileStringFormat.RelativeLocalPath;
        //            return FileStringFormat.RelativeLocalUri;
        //        }
        //        if (preferFsPath)
        //            return FileStringFormat.RelativeAltPath;
        //        return FileStringFormat.RelativeAltUri;
        //    }
        //    if (input.Length == 1)
        //    {
        //        hostName = path = "";
        //        if (platform.IsUnixLike() == CURRENT_FACTORY.FsPlatform.IsUnixLike())
        //        {
        //            if (preferFsPath)
        //                return FileStringFormat.WellFormedRelativeLocalPath;
        //            return FileStringFormat.WellformedRelativeLocalUri;
        //        }
        //        if (preferFsPath)
        //            return FileStringFormat.WellFormedRelativeAltPath;
        //        return FileStringFormat.WellformedRelativeAltUri;
        //    }

        //    FileUriConverter currentFactory;
        //    if (allowAlt)
        //    {
        //        currentFactory = GetFactory(platform, out FileUriConverter altFactory);
        //        if (preferFsPath)
        //        {
        //            #region Try to match well-formed strings

        //            if (currentFactory.TrySplitFileSystemPath(input, true, out hostName, out path, out bool isAbsolute))
        //            {
        //                if (isAbsolute)
        //                    return (platform.IsUnixLike() == CURRENT_FACTORY.FsPlatform.IsUnixLike()) ? FileStringFormat.WellFormedAbsoluteLocalPath : FileStringFormat.WellFormedAbsoluteAltPath;
        //                if (altFactory.TrySplitFileSystemPath(input, true, out hostName, out path, out isAbsolute) && isAbsolute)
        //                    return (platform.IsUnixLike() == CURRENT_FACTORY.FsPlatform.IsUnixLike()) ? FileStringFormat.WellFormedAbsoluteAltPath : FileStringFormat.WellFormedAbsoluteLocalPath;
        //                if (currentFactory.TrySplitFileUriString(input, true, out hostName, out path))
        //                    return (platform.IsUnixLike() == CURRENT_FACTORY.FsPlatform.IsUnixLike()) ? FileStringFormat.WellformedAbsoluteLocalUrl : FileStringFormat.WellformedAbsoluteAltUrl;
        //                if (altFactory.TrySplitFileUriString(input, true, out hostName, out path))
        //                    return (platform.IsUnixLike() == CURRENT_FACTORY.FsPlatform.IsUnixLike()) ? FileStringFormat.WellformedAbsoluteAltUrl : FileStringFormat.WellformedAbsoluteLocalUrl;
        //                return (platform.IsUnixLike() == CURRENT_FACTORY.FsPlatform.IsUnixLike()) ? FileStringFormat.WellFormedRelativeLocalPath : FileStringFormat.WellFormedRelativeAltPath;
        //            }
        //            if (altFactory.TrySplitFileSystemPath(input, true, out hostName, out path, out isAbsolute))
        //            {
        //                if (isAbsolute)
        //                    return (platform.IsUnixLike() == CURRENT_FACTORY.FsPlatform.IsUnixLike()) ? FileStringFormat.WellFormedAbsoluteAltPath : FileStringFormat.WellFormedAbsoluteLocalPath;
        //                if (currentFactory.TrySplitFileUriString(input, true, out hostName, out path))
        //                    return (platform.IsUnixLike() == CURRENT_FACTORY.FsPlatform.IsUnixLike()) ? FileStringFormat.WellformedAbsoluteLocalUrl : FileStringFormat.WellformedAbsoluteAltUrl;
        //                if (altFactory.TrySplitFileUriString(input, true, out hostName, out path))
        //                    return (platform.IsUnixLike() == CURRENT_FACTORY.FsPlatform.IsUnixLike()) ? FileStringFormat.WellformedAbsoluteAltUrl : FileStringFormat.WellformedAbsoluteLocalUrl;
        //                return (platform.IsUnixLike() == CURRENT_FACTORY.FsPlatform.IsUnixLike()) ? FileStringFormat.WellFormedRelativeAltPath : FileStringFormat.WellFormedRelativeLocalPath;
        //            }
        //            if (currentFactory.TrySplitFileUriString(input, true, out hostName, out path))
        //                return (platform.IsUnixLike() == CURRENT_FACTORY.FsPlatform.IsUnixLike()) ? FileStringFormat.WellformedAbsoluteLocalUrl : FileStringFormat.WellformedAbsoluteAltUrl;
        //            if (altFactory.TrySplitFileUriString(input, true, out hostName, out path))
        //                return (platform.IsUnixLike() == CURRENT_FACTORY.FsPlatform.IsUnixLike()) ? FileStringFormat.WellformedAbsoluteAltUrl : FileStringFormat.WellformedAbsoluteLocalUrl;

        //            #endregion

        //            if (currentFactory.TrySplitFileSystemPath(input, false, out hostName, out path, out isAbsolute))
        //            {
        //                if (isAbsolute)
        //                    return (platform.IsUnixLike() == CURRENT_FACTORY.FsPlatform.IsUnixLike()) ? FileStringFormat.AbsoluteLocalPath : FileStringFormat.AbsoluteAltPath;
        //                if (altFactory.TrySplitFileSystemPath(input, false, out hostName, out path, out isAbsolute) && isAbsolute)
        //                    return (platform.IsUnixLike() == CURRENT_FACTORY.FsPlatform.IsUnixLike()) ? FileStringFormat.AbsoluteAltPath : FileStringFormat.AbsoluteLocalPath;
        //                if (currentFactory.TrySplitFileUriString(input, false, out hostName, out path))
        //                    return (platform.IsUnixLike() == CURRENT_FACTORY.FsPlatform.IsUnixLike()) ? FileStringFormat.WellformedAbsoluteLocalUrl : FileStringFormat.WellformedAbsoluteAltUrl;
        //                if (altFactory.TrySplitFileUriString(input, false, out hostName, out path))
        //                    return (platform.IsUnixLike() == CURRENT_FACTORY.FsPlatform.IsUnixLike()) ? FileStringFormat.WellformedAbsoluteAltUrl : FileStringFormat.WellformedAbsoluteLocalUrl;
        //                return (platform.IsUnixLike() == CURRENT_FACTORY.FsPlatform.IsUnixLike()) ? FileStringFormat.RelativeLocalPath : FileStringFormat.RelativeAltPath;
        //            }
        //            if (altFactory.TrySplitFileSystemPath(input, true, out hostName, out path, out isAbsolute))
        //            {
        //                if (isAbsolute)
        //                    return (platform.IsUnixLike() == CURRENT_FACTORY.FsPlatform.IsUnixLike()) ? FileStringFormat.AbsoluteAltPath : FileStringFormat.AbsoluteLocalPath;
        //                if (currentFactory.TrySplitFileUriString(input, true, out hostName, out path))
        //                    return (platform.IsUnixLike() == CURRENT_FACTORY.FsPlatform.IsUnixLike()) ? FileStringFormat.WellformedAbsoluteLocalUrl : FileStringFormat.WellformedAbsoluteAltUrl;
        //                if (altFactory.TrySplitFileUriString(input, true, out hostName, out path))
        //                    return (platform.IsUnixLike() == CURRENT_FACTORY.FsPlatform.IsUnixLike()) ? FileStringFormat.WellformedAbsoluteAltUrl : FileStringFormat.WellformedAbsoluteLocalUrl;
        //                return (platform.IsUnixLike() == CURRENT_FACTORY.FsPlatform.IsUnixLike()) ? FileStringFormat.RelativeAltPath : FileStringFormat.RelativeLocalPath;
        //            }
        //            if (currentFactory.TrySplitFileUriString(input, true, out hostName, out path))
        //                return (platform.IsUnixLike() == CURRENT_FACTORY.FsPlatform.IsUnixLike()) ? FileStringFormat.WellformedAbsoluteLocalUrl : FileStringFormat.WellformedAbsoluteAltUrl;
        //            if (altFactory.TrySplitFileUriString(input, true, out hostName, out path))
        //                return (platform.IsUnixLike() == CURRENT_FACTORY.FsPlatform.IsUnixLike()) ? FileStringFormat.WellformedAbsoluteAltUrl : FileStringFormat.WellformedAbsoluteLocalUrl;
        //            // TODO: Use detection regex
        //        }
        //        else
        //        {
        //        }
        //    }
        //    throw new NotImplementedException();
        //}

        /// <summary>
        /// Ensures that a string is a well-formed file URI.
        /// </summary>
        /// <param name="uriString">The input URI string.</param>
        /// <param name="kind">The expected URI type.</param>
        /// <returns>A well-formed file URI.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="uriString"/> type does not match the specified <paramref name="kind"/>.</exception>
        public abstract string EnsureWellFormedUriString(string uriString, UriKind kind);

        /// <summary>
        /// Determines if a <seealso cref="Uri.UriSchemeFile">file</seealso> URL is well-formed and compatible with the target filesystem type.
        /// </summary>
        /// <param name="uriString">The URI string to test.</param>
        /// <param name="kind">The URI type to test for.</param>
        /// <returns><see langword="true"/> if the <paramref name="uriString"/> is well-formed and the decoded string is compatible with the target filesystem type;
        /// otherwise, <see langword="false"/>.</returns>
        public abstract bool IsWellFormedUriString(string uriString, UriKind kind);

        /// <summary>
        /// Determines if a string can be used as a file name for the target file system type.
        /// </summary>
        /// <param name="fileName">The file name to test.</param>
        /// <returns><see langword="true"/> if the <paramref name="fileName"/> is well-formed and compatible with the target filesystem type;
        /// otherwise, <see langword="false"/>.</returns>
        public bool IsValidFileSystemName(string fileName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determines if a path string is well-formed and can be used as a path for the target filesystem type.
        /// </summary>
        /// <param name="fileSystemPath">The filesystem path to test.</param>
        /// <param name="absoluteOnly"><see langword="true"/> if the <paramref name="fileSystemPath"/> must be an absolute path; otherwise <see langword="false"/> to
        /// permit relative paths.</param>
        /// <returns><see langword="true"/> if the <paramref name="fileSystemPath"/> is well-formed and compatible with the target filesystem type;
        /// otherwise, <see langword="false"/>.</returns>
        public abstract bool IsWellFormedFileSystemPath(string fileSystemPath, bool absoluteOnly);

        public bool TrySplitFileSystemPath(string absolutePath, bool requireWellFormed, out string hostName, out string rootedPath, out bool isAbsolute)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Attempt to create a <seealso cref="FileUri"/> object from an absolute <seealso cref="Uri.UriSchemeFile">file</seealso> URL or a filesystem path string.
        /// </summary>
        /// <param name="inputString">The source path string or URL.</param>
        /// <param name="platform">The target platform type.</param>
        /// <param name="preferUri">If <see langword="true"/>, try to parse a URI first; otherwise, try to parse a path string, first.</param>
        /// <param name="fileUri">Returns the constructed <seealso cref="FileUri"/> object or <see langword="null"/> if the <paramref name="inputString"/>
        /// could not be parsed.</param>
        /// <returns><see langword="true"/> if the <paramref name="inputString"/> could be used to construct a <seealso cref="FileUri"/> object;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool TryFromLocalPathOrUri(string inputString, PlatformType platform, bool preferUri, out FileUri fileUri)
        {
            FileUriConverter currentFactory = GetFactory(platform, out FileUriConverter altFactory);
            if (preferUri)
                return currentFactory.TryParseUriString(inputString, out fileUri) || altFactory.TryParseUriString(inputString, out fileUri) ||
                    currentFactory.TryParseFsPath(inputString, out fileUri) || altFactory.TryParseFsPath(inputString, out fileUri);
            return currentFactory.TryParseFsPath(inputString, out fileUri) || altFactory.TryParseFsPath(inputString, out fileUri) ||
                currentFactory.TryParseUriString(inputString, out fileUri) || altFactory.TryParseUriString(inputString, out fileUri);
        }

        /// <summary>
        /// Attempt to create a <seealso cref="FileUri"/> object from an absolute filesystem path string.
        /// </summary>
        /// <param name="path">The abolute filesystem path string.</param>
        /// <param name="fileUri">Returns the constructed <seealso cref="FileUri"/> object or <see langword="null"/> if the <paramref name="inputString"/>
        /// could not be parsed.</param>
        /// <returns><see langword="true"/> if the <paramref name="path"/> could be parsed as a filesystem path string;
        /// otherwise, <see langword="false"/>.</returns>
        protected abstract bool TryParseFsPath(string path, out FileUri fileUri);

        /// <summary>
        /// Attempt to create a <seealso cref="FileUri"/> object from an absolute <seealso cref="Uri.UriSchemeFile">file</seealso> URL string.
        /// </summary>
        /// <param name="uriString">The absolute file URI string.</param>
        /// <param name="fileUri">Returns the constructed <seealso cref="FileUri"/> object or <see langword="null"/> if the <paramref name="inputString"/>
        /// could not be parsed.</param>
        /// <returns><see langword="true"/> if the <paramref name="uriString"/> could be parsed as an absolute <seealso cref="Uri.UriSchemeFile">file</seealso> URL;
        /// otherwise, <see langword="false"/>.</returns>
        protected abstract bool TryParseUriString(string uriString, out FileUri fileUri);

        public static bool TrySplitFileUriString(string uriString, out string hostName, out string path, out string fileName, out bool isAbsolute)
        {
            Match match = FILE_URI_COMPONENTS_LAX_REGEX.Match(uriString);
            if (match.Success)
            {
                isAbsolute = match.Groups[MATCH_GROUP_NAME_FILE].Success;
                if (isAbsolute)
                    hostName = match.GetGroupValue(MATCH_GROUP_NAME_HOST, "");
                else
                    hostName = "";
                path = match.GetGroupValue(MATCH_GROUP_NAME_PATH, "");
                fileName = match.GetGroupValue(MATCH_GROUP_NAME_FILE_NAME, "");
                return true;
            }
            hostName = fileName = "";
            path = uriString;
            isAbsolute = false;
            return false;
        }

        public static int GetComponentIndexes(string fileUriString, out bool isAbsolute, out int hostIndex, out int leafIndex)
        {
            if (string.IsNullOrEmpty(fileUriString))
            {
                isAbsolute = false;
                hostIndex = leafIndex = 0;
                return 0;
            }
            Match match = FILE_URI_COMPONENTS_LAX_REGEX.Match(fileUriString);
            isAbsolute = match.Groups["a"].Success;
            leafIndex = (match.Groups["n"].Success) ? match.Groups["n"].Index : match.Groups["d"].Index;
            if (isAbsolute)
                hostIndex = (match.Groups["h"].Success) ? match.Groups["h"].Index : match.Groups["a"].Length;
            else
                hostIndex = 0;
            return match.Groups["d"].Index;
        }

        public static string EnsureWellFormedUri(string value, UriKind kind, PlatformType platform, bool allowAlt = false)
        {
            if (allowAlt)
            {
                FileUriConverter currentFactory = GetFactory(platform, out FileUriConverter altFactory);
                if (currentFactory.IsWellFormedUriString(value, kind) || altFactory.IsWellFormedUriString(value, kind))
                    return value;
                return currentFactory.EnsureWellFormedUriString(value, kind);
            }
            return GetFactory(platform).EnsureWellFormedUriString(value, kind);
        }

        public static string SplitUriPathFileName(string uriString, out string fileName)
        {
            if (string.IsNullOrEmpty(uriString))
            {
                fileName = "";
                return "";
            }
            int endIndex = uriString.Length - 1;
            int dirLen = uriString.LastIndexOf(URI_PATH_SEPARATOR_CHAR);
            if (dirLen == endIndex)
            {
                endIndex--;
                dirLen = uriString.LastIndexOf(URI_PATH_SEPARATOR_CHAR, dirLen - 1);
            }
            if (dirLen < 0)
            {
                fileName = uriString;
                return "";
            }
            fileName = uriString.Substring(dirLen + 1, endIndex - dirLen);
            return uriString.Substring(0, dirLen);
        }
        public static FileUriConverter GetFactory(PlatformType platform, out FileUriConverter altFactory)
        {
            switch (platform)
            {
                case PlatformType.Windows:
                case PlatformType.Xbox:
                    altFactory = LinuxFileUriConverter.INSTANCE;
                    return WindowsFileUriConverter.INSTANCE;
                default:
                    altFactory = WindowsFileUriConverter.INSTANCE;
                    return LinuxFileUriConverter.INSTANCE;
            }
        }

        public static FileUriConverter GetFactory(PlatformType platform)
        {
            switch (platform)
            {
                case PlatformType.Windows:
                case PlatformType.Xbox:
                    return WindowsFileUriConverter.INSTANCE;
                default:
                    return LinuxFileUriConverter.INSTANCE;
            }
        }

        public static FileUriConverter GetAltFactory(PlatformType platform)
        {
            switch (platform)
            {
                case PlatformType.Windows:
                case PlatformType.Xbox:
                    return LinuxFileUriConverter.INSTANCE;
                default:
                    return WindowsFileUriConverter.INSTANCE;
            }
        }
    }
}

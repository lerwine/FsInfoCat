using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FsInfoCat.Util
{
    public abstract class FileUriFactory
    {
        public const char URI_PATH_SEPARATOR_CHAR = '/';
        public const string URI_PATH_SEPARATOR_STRING = "/";

        /// <summary>
        /// Matches a path segment including optional trailing slash.
        /// </summary>
        public static readonly Regex URI_PATH_SEGMENT_REGEX = new Regex(@"(?:^|\G)(?:/|([^/]+)(?:/|$))", RegexOptions.Compiled);

        public const string PATTERN_HOST_NAME = @"(?i)^\s*((?=\d+(\.\d+){3})(?<ipv2>((2(5[0-5]?|[0-4]?\d?)?|[01]?\d\d?)(\.|(?![.\d]))){4})|(?=\[[a-f\d]*(:[a-f\d]*){2,7}\]|[a-f\d]*(:[a-f\d]*){2,7})\[?(?<ipv6>[a-f\d]{1,4}(:[a-f\d]{1,4}){7}|(([a-f\d]{1,4}:)+|:)(:[a-f\d]{1,4})+)\]?|(?=\S{1,255}\s*$)(?<dns>[a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?))\s*$";

        /// <summary>
        /// Matches a host name.
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        /// <item><term><seealso cref="HOST_NAME_MATCH_GROUP_NAME_IPV2">ipv2</seealso></term> Matches IPV2 address.</item>
        /// <item><term><seealso cref="HOST_NAME_MATCH_GROUP_NAME_IPV6">ipv6</seealso></term> Matches an IPV6 address without any surrounding brackets or the UNC domain text.</item>
        /// If this group matches, then hexidecimal groups for the IPV6 address are separated by dashes rather than colons.</item>
        /// <item><term><seealso cref="HOST_NAME_MATCH_GROUP_NAME_DNS">dns</seealso></term> Matches a DNS host name.</item>
        /// </list></remarks>
        public static readonly Regex HOST_NAME_REGEX = new Regex(@"^((?=\d+(\.\d+){3})(?<ipv2>((2(5[0-5]?|[0-4]?\d?)?|[01]?\d\d?)(\.|(?![.\d]))){4})|(?=\[[a-f\d]*(:[a-f\d]*){2,7}\]|[a-f\d]*(:[a-f\d]*){2,7})\[?(?<ipv6>[a-f\d]{1,4}(:[a-f\d]{1,4}){7}|(([a-f\d]{1,4}:)+|:)(:[a-f\d]{1,4})+)\]?|(?=\S{1,255}\s*$)(?<dns>[a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?))$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public const string PATTERN_DNS_NAME = @"(?i)^\s*(?=\S{1,255}\s*$)[a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?\s*$";

        public static readonly Regex DNS_NAME_REGEX = new Regex(@"^(?=\S{1,255}\s*$)[a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public const string HOST_NAME_MATCH_GROUP_NAME_IPV2 = "ipv2";

        public const string HOST_NAME_MATCH_GROUP_NAME_IPV6 = "ipv6";

        public const string HOST_NAME_MATCH_GROUP_NAME_DNS = "dns";

        public static readonly Regex IPV2_ADDRESS_REGEX = new Regex(@"^(?=\d+(\.\d+){3})((2(5[0-5]?|[0-4]?\d?)?|[01]?\d\d?)(\.|(?![.\d]))){4}$", RegexOptions.Compiled);

        /// <summary>
        /// Matches an IPV6 address
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        /// <item><term><seealso cref="HOST_NAME_MATCH_GROUP_NAME_IPV6">ipv6</seealso></term> Matches the actual address without any surrounding brackets.</item>
        /// </list></remarks>
        public static readonly Regex IPV6_ADDRESS_REGEX = new Regex(@"^(?=\[[a-f\d]*(:[a-f\d]*){2,7}\]|[a-f\d]*(:[a-f\d]*){2,7})\[?(?<ipv6>[a-f\d]{1,4}(:[a-f\d]{1,4}){7}|(([a-f\d]{1,4}[:-])+|:)(:[a-f\d]{1,4})+)\]?$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public const string PATTERN_ANY_ABS_FILE_URI_OR_LOCAL_LAX = @"(?i)^\s*(?=\S)(file://([a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?|\[?(:(:[a-f\d]{1,4}){1,7}|(?=[a-f\d]{0,4}(:[a-f\d]{0,4}){2,7})([a-f\d]+:)+(:|(:[a-f\d]+)+))\]?|/[a-f]:)?|[\\/]{2}([a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?|\[?(:(:[a-f\d]{1,4}){1,7}|(?=[a-f\d]{0,4}(:[a-f\d]{0,4}){2,7})([a-f\d]+:)+(:|(:[a-f\d]+)+))\]?)|[a-f]:)?(([\\/]([^\u0000-\u0019\u007f-\u00a0\u1680\u2028\u2029\ud800-\udfff\\/]+|%([^025%]|0[^0]|2[^f]|5[\dabde](?=%|$)))+)*/?)\s*$";
        public static readonly Regex ANY_ABS_FILE_URI_OR_LOCAL_LAX_REGEX = new Regex(PATTERN_ANY_ABS_FILE_URI_OR_LOCAL_LAX, RegexOptions.Compiled);

        /// <summary>
        /// Match group name for <see cref="FORMAT_GUESS_LOCAL_REGEX"/>, <see cref="FORMAT_GUESS_ALT_REGEX"/>, <see cref="Windows.FORMAT_GUESS_REGEX"/>
        /// and <see cref="Linux.FORMAT_GUESS_REGEX"/> for absolute file URL match.
        /// </summary>
        public const string FORMAT_DETECT_MATCH_GROUP_FILE_URL = "f";

        /// <summary>
        /// Match group name for <see cref="FORMAT_GUESS_LOCAL_REGEX"/>, <see cref="FORMAT_GUESS_ALT_REGEX"/>, <see cref="Windows.FORMAT_GUESS_REGEX"/>
        /// and <see cref="Linux.FORMAT_GUESS_REGEX"/> for URN path match.
        /// </summary>
        public const string FORMAT_DETECT_MATCH_GROUP_URN = "u";

        /// <summary>
        /// Match group name for <see cref="FORMAT_GUESS_LOCAL_REGEX"/>, <see cref="FORMAT_GUESS_ALT_REGEX"/>, <see cref="Windows.FORMAT_GUESS_REGEX"/>
        /// and <see cref="Linux.FORMAT_GUESS_REGEX"/> for non-file URI match.
        /// </summary>
        public const string FORMAT_DETECT_MATCH_GROUP_NON_FILE = "x";

        /// <summary>
        /// Matches a well-formed URI that can be converted to a valid absolute or relative filesystem-local path.
        /// </summary>
        /// <remarks>Named group match meanings:
        /// <list type="bullet">
        /// <item><term><seealso cref="PATH_MATCH_GROUP_NAME_ABSOLUTE">a</seealso></term> Text is an absolute file URL. If this group is not matched, then the text is a relative URL that can be used as the path of a file URL.</item>
        /// <item><term><seealso cref="PATH_MATCH_GROUP_NAME_HOST">h</seealso></term> Matches the host name. This will never match when group <c>a</c> is not matched.</item>
        /// <item><term><seealso cref="PATH_MATCH_GROUP_NAME_PATH">p</seealso></term> Matches the path. This match will always succeed when entire expression succeeds, even if the path is an empty string.</item>
        /// </list></remarks>
        public static readonly Regex ANY_FILE_URI_STRICT_REGEX = new Regex(@"^((?<a>file://(?i)^(?<h>[a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?|\[?(:(:[a-f\d]{1,4}){1,7}|(?=[a-f\d]{0,4}(:[a-f\d]{0,4}){2,7})([a-f\d]+:)+(:|(:[a-f\d]+)+))\]?)?(?<p>(/([!$&-.:;=@[\]()\w]+|%([13-9a-f][\da-f]|0[1-9a-f]|2[\da-e]))+)*/?))|(?<p>(?i)(/(?!/))?(([!$&-.:;=@[\]()\w]+|%([13-9a-f][\da-f]|0[1-9a-f]|2[\da-e]))+(/|(?=$)))*))$", RegexOptions.Compiled);

        /// <summary>
        /// Match group name for <see cref="ANY_FILE_URI_STRICT_REGEX"/>, <see cref="LOCAL_FILE_URI_STRICT_REGEX"/>, <see cref="ALT_FILE_URI_STRICT_REGEX"/>,
        /// <see cref="FORMAT_GUESS_LOCAL_REGEX"/>, <see cref="FORMAT_GUESS_ALT_REGEX"/>, <see cref="LOCAL_FS_PATH_REGEX"/>, <see cref="ALT_FS_PATH_REGEX"/>,
        /// <see cref="Windows.FILE_URI_STRICT_REGEX"/>, <see cref="Windows.FORMAT_GUESS_REGEX"/>, <see cref="Linux.FORMAT_GUESS_REGEX"/>,
        /// <see cref="Linux.FILE_URI_STRICT_REGEX"/>, <see cref="Windows.FS_PATH_REGEX"/> and <see cref="Linux.FS_PATH_REGEX"/> for absolute path match.
        /// </summary>
        public const string PATH_MATCH_GROUP_NAME_ABSOLUTE = "a";

        /// <summary>
        /// Match group name for <see cref="LOCAL_FS_PATH_REGEX"/>, <see cref="ALT_FS_PATH_REGEX"/>, <see cref="ANY_FILE_URI_STRICT_REGEX"/>,
        /// <see cref="LOCAL_FILE_URI_STRICT_REGEX"/>, <see cref="ALT_FILE_URI_STRICT_REGEX"/>, <see cref="Windows.FS_PATH_REGEX"/>, <see cref="Linux.FS_PATH_REGEX"/>,
        /// <see cref="Windows.FILE_URI_STRICT_REGEX"/> and <see cref="Linux.FILE_URI_STRICT_REGEX"/> for path match.
        /// </summary>
        public const string PATH_MATCH_GROUP_NAME_PATH = "p";

        /// <summary>
        /// Match group name for <see cref="FORMAT_GUESS_LOCAL_REGEX"/>, <see cref="FORMAT_GUESS_ALT_REGEX"/>, <see cref="LOCAL_FS_PATH_REGEX"/>, <see cref="ALT_FS_PATH_REGEX"/>,
        /// <see cref="ANY_FILE_URI_STRICT_REGEX"/>, <see cref="LOCAL_FILE_URI_STRICT_REGEX"/>, <see cref="ALT_FILE_URI_STRICT_REGEX"/>, <see cref="Windows.FORMAT_GUESS_REGEX"/>
        /// and <see cref="Linux.FORMAT_GUESS_REGEX"/>, <see cref="Windows.FS_PATH_REGEX"/>, <see cref="Linux.FS_PATH_REGEX"/>, <see cref="Windows.FILE_URI_STRICT_REGEX"/>
        /// and <see cref="Linux.FILE_URI_STRICT_REGEX"/> for host name match.
        /// </summary>
        public const string PATH_MATCH_GROUP_NAME_HOST = "h";

        private static readonly char[] _INVALID_FILENAME_CHARS;

        public abstract Regex FormatDetectionRegex { get; }

        public abstract Regex HostNameRegex { get; }

        public abstract Regex FileUriStrictRegex { get; }

        public abstract Regex LocalFsPathRegex { get; }

        public abstract char LocalDirectorySeparatorChar { get;  }

        public abstract string FsDisplayName { get;  }

        public abstract PlatformType FsPlatform { get; }

        public static bool IsAbsoluteGroupMatch(Match match) => match.Success && match.Groups[PATH_MATCH_GROUP_NAME_ABSOLUTE].Success;

        public static string GetPathGroupMatchValue(Match match)
        {
            if (match.Success)
            {
                Group g = match.Groups[PATH_MATCH_GROUP_NAME_PATH];
                if (g.Success)
                    return g.Value;
            }
            return "";
        }
        public static string GetHostGroupMatchValue(Match match)
        {
            if (match.Success)
            {
                Group g = match.Groups[PATH_MATCH_GROUP_NAME_HOST];
                if (g.Success)
                    return g.Value;
            }
            return "";
        }

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

        static FileUriFactory()
        {
            char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
            _INVALID_FILENAME_CHARS = invalidFileNameChars.Contains(Path.PathSeparator) ? invalidFileNameChars : invalidFileNameChars.Concat(new char[] { Path.PathSeparator }).ToArray();
        }

        protected abstract string EscapeSpecialPathChars(string path);

        protected abstract bool ContainsSpecialPathChars(string path);

        public abstract string ToLocalFsPath(string hostName, string uriPath);

        public abstract string FromLocalFsPath(string path, out string hostName);

        public FileStringFormat ParseUriOrPath(string source, bool preferFsPath, out string hostName, out string path)
        {
            throw new NotImplementedException();
        }

        public bool TryCreateUri(string uriString, UriKind uriKind, out Uri result)
        {
            throw new NotImplementedException();
        }

        public static bool TryParseFileUriString(string uriString, out string hostName, out string absolutePath, out int leafIndex)
        {
            throw new NotImplementedException();
        }

        public bool TryCreateFileUriFromUriString(string uriString, out Uri uri)
        {
            throw new NotImplementedException();
        }

        public bool TryCreateFileUriFromLocalPath(string localPath, out Uri uri)
        {
            throw new NotImplementedException();
        }

        public bool IsWellFormedFileUriString(string uri, UriKind kind)
        {
            throw new NotImplementedException();
        }

        public virtual string EnsureWellFormedRelativeUriPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return "";
            if (Uri.IsWellFormedUriString(path, UriKind.Relative))
                return path;
            if (ContainsSpecialPathChars(path))
            {
                string p = EscapeSpecialPathChars(path);
                if (Uri.IsWellFormedUriString(path, UriKind.Relative))
                    return p;
                return EscapeSpecialPathChars(Uri.EscapeUriString(path));
            }
            return Uri.EscapeUriString(path);
        }
    }
}

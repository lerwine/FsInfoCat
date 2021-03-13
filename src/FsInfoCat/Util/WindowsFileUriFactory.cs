using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FsInfoCat.Util
{
    public class WindowsFileUriFactory : FileUriFactory
    {
        public static readonly WindowsFileUriFactory INSTANCE = new WindowsFileUriFactory();

        public const string PATTERN_LOCAL_FS_NAME = @"^[^\u0000-\u0019""<>|:*?\\/]+$";
        public static readonly Regex LOCAL_FS_NAME_REGEX = new Regex(PATTERN_LOCAL_FS_NAME, RegexOptions.Compiled);

        public const string PATTERN_ABS_FS_PATH = @"(?i)^((//|\\\\)([a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?|\[?(:(:[a-f\d]{1,4}){1,7}|(?=[a-f\d]{0,4}(:[a-f\d]{0,4}){2,7})([a-f\d]+:)+(([a-f\d]+|:)(?!:[\da-f])|(:[a-f\d]+)+(?!:[\da-f])))\]?)|[a-z]:)([/\\]([^\u0000-\u0019""<>|:;*?\\/]+([\\/](?![\\/]))?)*)?$";
        public static readonly Regex ABS_FS_PATH_REGEX = new Regex(@"(?i)^((//|\\\\)([a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?|\[?(:(:[a-f\d]{1,4}){1,7}|(?=[a-f\d]{0,4}(:[a-f\d]{0,4}){2,7})([a-f\d]+:)+(([a-f\d]+|:)(?!:[\da-f])|(:[a-f\d]+)+(?!:[\da-f])))\]?)|[a-z]:)([/\\]([^\u0000-\u0019""<>|:;*?\\/]+([\\/](?![\\/]))?)*)?$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Matches a well-formed local path on the typical Windows filesystem.
        /// </summary>
        /// <remarks>Named group match meanings:
        /// <list type="bullet">
        /// <item><term><seealso cref="PATH_MATCH_GROUP_NAME_ABSOLUTE">a</seealso></term> Text is an absolute local path. If this group is not matched, then the text is either a URN or a relative local path.</item>
        /// <item><term><seealso cref="PATH_MATCH_GROUP_NAME_HOST">h</seealso></term> Matches the host name. This implies that the text is a URN.</item>
        /// <item><term><seealso cref="PATH_MATCH_GROUP_NAME_PATH">p</seealso></term> Matches the path minus the host name. This match will always succeed when entire expression succeeds, even if the path is an empty string.</item>
        /// </list></remarks>
        public static readonly Regex FS_PATH_REGEX = new Regex(@"^((//|\\\\)(?<h>[a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?|\[?(:(:[a-f\d]{1,4}){1,7}|(?=[a-f\d]{0,4}(:[a-f\d]{0,4}){2,7})([a-f\d]+:)+(([a-f\d]+|:)(?!:[\da-f])|(:[a-f\d]+)+(?!:[\da-f])))\]?)(?=[/\\]|$))?(?<p>(?<a>[a-z]:(?=[/\\]|$))?[/\\]?([^\u0000-\u0019""<>|:;*?\\/]+([\\/](?![\\/]))?)*)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Matches an IPV6 address
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        /// <item><term><seealso cref="HOST_NAME_MATCH_GROUP_NAME_IPV6">ipv6</seealso></term> Matches the actual address without any surrounding brackets or the UNC domain text.</item>
        /// <item><term><seealso cref="HOST_NAME_MATCH_GROUP_NAME_UNC">unc</seealso></term> Matches the <c>.ipv6-literal.net</c> domain name for UNC notation. If this group matches, then hexidecimal groups are separated by dashes rather than colons.</item>
        /// </list></remarks>
        public static readonly Regex LOCAL_IPV6_ADDRESS_REGEX = new Regex(@"^(?=\[[a-f\d]*(:[a-f\d]*){2,7}\]|[a-f\d]*(:[a-f\d]*){2,7}|[a-f\d]*(-[a-f\d]*){2,7}\.ipv6-literal\.net)\[?(?<ipv6>[a-f\d]{1,4}([:-][a-f\d]{1,4}){7}|(([a-f\d]{1,4}[:-])+|[:-])([:-][a-f\d]{1,4})+)(?<unc>\.ipv6-literal\.net)?\]?$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public const string HOST_NAME_MATCH_GROUP_NAME_UNC = "unc";

        public const string PATTERN_FS_URI_NAME = @"(?i)^([^\u0000-\u0019""<>|:;*?\\/%]+|%([^012357%]|2[^2af]|3[\dd]|[57][^c]|(?=%|$)))+$";
        public static readonly Regex FS_URI_NAME_REGEX = new Regex(PATTERN_FS_URI_NAME, RegexOptions.Compiled);

        public const string PATTERN_ABS_FILE_URI_OR_LOCAL_LAX = @"(?i)^\s*(?=\S)(file://([a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?|\[?(:(:[a-f\d]{1,4}){1,7}|(?=[a-f\d]{0,4}(:[a-f\d]{0,4}){2,7})([a-f\d]+:)+(:|(:[a-f\d]+)+))\]?|/[a-f]:)|[\\/]{2}([a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?|\[?(:(:[a-f\d]{1,4}){1,7}|(?=[a-f\d]{0,4}(:[a-f\d]{0,4}){2,7})([a-f\d]+:)+(:|(:[a-f\d]+)+))\]?)|[a-f]:)([\\/]([^\u0000-\u0019\u007f-\u00a0\u1680\u2028\u2029\ud800-\udfff/"" <>\\/|]+|%([^012357%]|2[^2af]|3[^acef]|[57][^c]|(?=%|$)))+)*[\\/]?\s*$";
        public static readonly Regex ABS_FILE_URI_OR_LOCAL_LAX_REGEX = new Regex(PATTERN_ABS_FILE_URI_OR_LOCAL_LAX, RegexOptions.Compiled);

        /// <summary>
        /// Matches a host name.
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        /// <item><term><seealso cref="HOST_NAME_MATCH_GROUP_NAME_IPV2">ipv2</seealso></term> Matches IPV2 address.</item>
        /// <item><term><seealso cref="HOST_NAME_MATCH_GROUP_NAME_IPV6">ipv6</seealso></term> Matches an IPV6 address without any surrounding brackets or the UNC domain text.</item>
        /// <item><term><seealso cref="HOST_NAME_MATCH_GROUP_NAME_UNC">unc</seealso></term> Matches the <c>.ipv6-literal.net</c> domain name for IPV6 UNC notation.
        /// If this group matches, then hexidecimal groups for the IPV6 address are separated by dashes rather than colons.</item>
        /// <item><term><seealso cref="HOST_NAME_MATCH_GROUP_NAME_DNS">dns</seealso></term> Matches a DNS host name.</item>
        /// </list></remarks>
        public static readonly Regex HOST_NAME_W_UNC_REGEX = new Regex(@"^((?=\d+(\.\d+){3})(?<ipv2>((2(5[0-5]?|[0-4]?\d?)?|[01]?\d\d?)(\.|(?![.\d]))){4})|(?=\[[a-f\d]*(:[a-f\d]*){2,7}\]|[a-f\d]*(:[a-f\d]*){2,7}|[a-f\d]*(-[a-f\d]*){2,7}\.ipv6-literal\.net)\[?(?<ipv6>[a-f\d]{1,4}([:-][a-f\d]{1,4}){7}|(([a-f\d]{1,4}[:-])+|[:-])([:-][a-f\d]{1,4})+)(?<unc>\.ipv6-literal\.net)?\]?|(?=\S{1,255}\s*$)(?<dns>[a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?))$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// <seealso cref="Regex"/> that can be used to guess the format of a string from the perspective of a typical Windows file system.
        /// </summary>
        /// <remarks>Named group match meanings:
        /// <list type="bullet">
        /// <item><term><see cref="FORMAT_GUESS_MATCH_GROUP_FILE_URL">f</see></term> Text appears to be a file URL.</item>
        /// <item><term><see cref="FORMAT_GUESS_MATCH_GROUP_URN">u</see></term> Text appears to be URN notation.</item>
        /// <item><term><see cref="PATH_MATCH_GROUP_NAME_HOST">h</see></term> Host name.</item>
        /// <item><term><see cref="PATH_MATCH_GROUP_NAME_PATH">p</see></term> Absolute path.</item>
        /// <item><term><see cref="FORMAT_GUESS_MATCH_GROUP_NON_FILE">x</see></term> Text appears to be a non-file URI.</item>
        /// <item>Any group match may succeeed independently of other groups.</item>
        /// <item>At least one group will match if the entire expression matches.</item>
        /// <item>Groups <c>f</c>, <c>u</c> and <c>x</c> are exclusive of each other.</item>
        /// <item>If the expression matches, but groups <c>f</c> and <c>u</c> do not, then text appears to be an absolute local path.</item>
        /// <item>If the expression does not match at all, then the text is assumed to be a non-rooted relative path.</item>
        /// </list></remarks>
        public static readonly Regex FORMAT_DETECTION_REGEX = new Regex(@"^((?<f>file:[\\/]{2}((?<h>[^\\/]+)(?<p>[\\/].*)?|(?<p>[\\/][a-z]:([\\/].*)?)))|(?<u>[\\/]{2}(?<h>[^\\/]+)(?<p>([\\/](?![\\/]).*)?))|(?<p>[a-z]:([\\/].*)?)|(?<x>(?!file:)[a-z][\w-]+:([\\/][\\/]?)?(?<h>([^?#/@:]*(:[^?#/@:]*)?@)?[^?#/@:]+(:\d+)?)?(?<p>([\\/:].*)?)))$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Matches a well-formed URI that can be converted to a valid absolute or relative local path on a typical Windows filesystem.
        /// </summary>
        /// <remarks>Named group match meanings:
        /// <list type="bullet">
        /// <item><term><seealso cref="PATH_MATCH_GROUP_NAME_ABSOLUTE">a</seealso></term> Text is an absolute file URL. If this group is not matched, then the text is a relative URL that can be used as the path of a file URL.</item>
        /// <item><term><seealso cref="PATH_MATCH_GROUP_NAME_HOST">h</seealso></term> Matches the host name. This will never match when group <c>a</c> is not matched.</item>
        /// <item><term><seealso cref="PATH_MATCH_GROUP_NAME_PATH">p</seealso></term> Matches the path. This match will always succeed when entire expression succeeds, even if the path is an empty string.</item>
        /// </list></remarks>
        public static readonly Regex FILE_URI_STRICT_REGEX = new Regex(@"^((?<a>file://(?i)(?=[^/]+|/[a-z]:)(?<h>[a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?|\[?(:(:[a-f\d]{1,4}){1,7}|(?=[a-f\d]{0,4}(:[a-f\d]{0,4}){2,7})([a-f\d]+:)+(([a-f\d]+|:)(?!:[\da-f])|(:[a-f\d]+)+(?!:[\da-f])))\]?)?(?<p>(/[a-z]:)?(/([!$&-)+-.=@[\]\w]+|%([46][\da-f]|2[^2]|3[\dd]|[57][\dabde]))+)*/?))|(?<p>(?i)(/(?!/))?(([!$&-)+-.=@[\]\w]+|%([46][\da-f]|2[^2]|3[\dbd]|[57][\dabde]))+(/|(?=$)))*))$", RegexOptions.Compiled);

        private WindowsFileUriFactory() { }

        public override Regex FormatDetectionRegex => FORMAT_DETECTION_REGEX;

        public override Regex HostNameRegex => HOST_NAME_W_UNC_REGEX;

        public override Regex FileUriStrictRegex => FILE_URI_STRICT_REGEX;

        public override Regex LocalFsPathRegex => FS_PATH_REGEX;

        public override char LocalDirectorySeparatorChar => '\\';

        public override string FsDisplayName => "Windows";

        public override PlatformType FsPlatform => PlatformType.Windows;

        protected override string EscapeSpecialPathChars(string path) => path.Replace(UriHelper.URI_FRAGMENT_DELIMITER_STRING, UriHelper.URI_FRAGMENT_DELIMITER_ESCAPED);

        protected override bool ContainsSpecialPathChars(string path) => path.Contains(UriHelper.URI_FRAGMENT_DELIMITER_CHAR);

        public override string EnsureWellFormedRelativeUriPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return "";
            return base.EnsureWellFormedRelativeUriPath(path.Replace(LocalDirectorySeparatorChar, URI_PATH_SEPARATOR_CHAR));
        }

        public override string ToLocalFsPath(string hostName, string uriPath)
        {
            if (string.IsNullOrEmpty(hostName))
                return (string.IsNullOrEmpty(uriPath)) ? uriPath : Uri.UnescapeDataString(uriPath).Replace(URI_PATH_SEPARATOR_CHAR, LocalDirectorySeparatorChar);
            Match match = IPV6_ADDRESS_REGEX.Match(hostName);
            if (match.Success)
                hostName = $"{hostName.Replace(':', '-')}.ipv6-literal.net";
            if (string.IsNullOrEmpty(uriPath))
                return $"\\\\{hostName}";

            if (uriPath[0] != URI_PATH_SEPARATOR_CHAR)
                return $"\\\\{hostName}\\{Uri.UnescapeDataString(uriPath).Replace(URI_PATH_SEPARATOR_CHAR, LocalDirectorySeparatorChar)}";
            return $"\\\\{hostName}{Uri.UnescapeDataString(uriPath).Replace(URI_PATH_SEPARATOR_CHAR, LocalDirectorySeparatorChar)}";
        }

        public override string FromLocalFsPath(string path, out string hostName)
        {
            if (string.IsNullOrEmpty(path))
            {
                hostName = "";
                return "";
            }
            Match match = FILE_URI_STRICT_REGEX.Match(path);
            if (match.Success)
            {
                hostName = GetHostGroupMatchValue(match);
                path = GetPathGroupMatchValue(match);
                match = LOCAL_IPV6_ADDRESS_REGEX.Match(hostName);
                if (match.Success && match.Groups[HOST_NAME_MATCH_GROUP_NAME_UNC].Success)
                    hostName = match.Groups[HOST_NAME_MATCH_GROUP_NAME_IPV6].Value.Replace('-', ':');
            }
            else
                hostName = "";
            return EscapeSpecialPathChars(Uri.EscapeUriString(path.Replace(LocalDirectorySeparatorChar, URI_PATH_SEPARATOR_CHAR)));
        }
    }
}

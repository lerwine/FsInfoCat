using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FsInfoCat.Util
{
    public class LinuxFileUriFactory : FileUriFactory
    {
        public static readonly LinuxFileUriFactory INSTANCE = new LinuxFileUriFactory();

        public const string PATTERN_LOCAL_FS_NAME = @"^[^\u0000/]+$";
        public static readonly Regex LOCAL_FS_NAME_REGEX = new Regex(PATTERN_LOCAL_FS_NAME, RegexOptions.Compiled);

        public const string PATTERN_ABS_FS_PATH = @"(?i)^(\s+(?=//))?(//([a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?|\[?(:(:[a-f\d]{1,4}){1,7}|(?=[a-f\d]{0,4}(:[a-f\d]{0,4}){2,7})([a-f\d]+:)+(([a-f\d]+|:)(?!:[\da-f])|(:[a-f\d]+)+(?!:[\da-f])))\]?)(?=/|$)|(?=/))(/(?!/))?([^\u0000/]+(/(?!/))?)*$";

        /// <summary>
        /// Matches a well-formed local path on the typical Linux filesystem.
        /// </summary>
        /// <remarks>Named group match meanings:
        /// <list type="bullet">
        /// <item><term><seealso cref="PATH_MATCH_GROUP_NAME_ABSOLUTE">a</seealso></term> Text is an absolute local path. If this group is not matched, then the text is a relative local path.</item>
        /// <item><term><seealso cref="PATH_MATCH_GROUP_NAME_HOST">h</seealso></term> Matches the host name. This implies that the text is a URN.</item>
        /// <item><term><seealso cref="PATH_MATCH_GROUP_NAME_PATH">p</seealso></term> Matches the path minus the host name. This match will always succeed when entire expression succeeds, even if the path is an empty string.</item>
        /// </list></remarks>
        public static readonly Regex FS_PATH_REGEX = new Regex(@"^(//(?<h>[a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?|\[?(:(:[a-f\d]{1,4}){1,7}|(?=[a-f\d]{0,4}(:[a-f\d]{0,4}){2,7})([a-f\d]+:)+(([a-f\d]+|:)(?!:[\da-f])|(:[a-f\d]+)+(?!:[\da-f])))\]?)(?=/|$))?(?<p>(?<a>/(?!/))?([^\u0000/]+(/(?!/))?)*)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        //public static readonly Regex FS_PATH_REGEz = new Regex(@"(?i)^(\s+(?=//))?(//([a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?|\[?(:(:[a-f\d]{1,4}){1,7}|(?=[a-f\d]{0,4}(:[a-f\d]{0,4}){2,7})([a-f\d]+:)+(([a-f\d]+|:)(?!:[\da-f])|(:[a-f\d]+)+(?!:[\da-f])))\]?)(?=/|$)|(?=/))(/(?!/))?([^\u0000/]+(/(?!/))?)*$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public const string PATTERN_FS_URI_NAME = @"(?i)^([^\u0000/%]+|%([^02%]|0[^0]|2[^f]|(?=%|$)))+$";
        public static readonly Regex FS_URI_NAME_REGEX = new Regex(PATTERN_FS_URI_NAME, RegexOptions.Compiled);

        public const string PATTERN_ABS_FILE_URI_OR_LOCAL_LAX = @"(?i)^\s*(?=\S)(file://([a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?|\[?(:(:[a-f\d]{1,4}){1,7}|(?=[a-f\d]{0,4}(:[a-f\d]{0,4}){2,7})([a-f\d]+:)+(:|(:[a-f\d]+)+))\]?)?|//([a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?|\[?(:(:[a-f\d]{1,4}){1,7}|(?=[a-f\d]{0,4}(:[a-f\d]{0,4}){2,7})([a-f\d]+:)+(:|(:[a-f\d]+)+))\]?))?((/([^\u0000-\u0019\u007f-\u00a0\u1680\u2028\u2029\ud800-\udfff/]+|%([^02%]|0[^0]|2[^f]|(?=%|$)))+)*/?)\s*$";
        public static readonly Regex ABS_FILE_URI_OR_LOCAL_LAX_REGEX = new Regex(PATTERN_ABS_FILE_URI_OR_LOCAL_LAX, RegexOptions.Compiled);

        /// <summary>
        /// <seealso cref="Regex"/> that can be used to guess the format of a string from the perspective of a typical Linux file system.
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
        public static readonly Regex FORMAT_DETECTION_REGEX = new Regex(@"^((?<f>file://(?<h>[^/]+)?(?<p>/.*)?)|(?<u>//(?<h>[^/]+)(?<p>(/.*)?))|(?<p>/(?!/).*)|(?<x>(?!file:)[a-z][\w-]*:(//?)?(?<h>([^?#/@:]*(:[^?#/@:]*)?@)?[^?#/@:]+(:\d+)?)?(?<p>([/:].*)?)))$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Matches a well-formed URI that can be converted to a valid absolute or relative local path on a typical Linux filesystem.
        /// </summary>
        /// <remarks>Named group match meanings:
        /// <list type="bullet">
        /// <item><term><seealso cref="PATH_MATCH_GROUP_NAME_ABSOLUTE">a</seealso></term> Text is an absolute file URL. If this group is not matched, then the text is a relative URL that can be used as the path of a file URL.</item>
        /// <item><term><seealso cref="PATH_MATCH_GROUP_NAME_HOST">h</seealso></term> Matches the host name. This will never match when group <c>a</c> is not matched.</item>1:2:3:4:5:6:7:8
        /// <item><term><seealso cref="PATH_MATCH_GROUP_NAME_PATH">p</seealso></term> Matches the path. This match will always succeed when entire expression succeeds, even if the path is an empty string.</item>
        /// </list></remarks>
        public static readonly Regex FILE_URI_STRICT_REGEX = new Regex(@"^((?<a>file://(?i)(?<h>[a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?|\[?(:(:[a-f\d]{1,4}){1,7}|(?=[a-f\d]{0,4}(:[a-f\d]{0,4}){2,7})([a-f\d]+:)+(([a-f\d]+|:)(?!:[\da-f])|(:[a-f\d]+)+(?!:[\da-f])))\]?)?(?i)(?<p>((/([!$&-.:;=@[\]()\w]+|%([13-9a-f][\da-f]|0[1-9a-f]|2[\da-e]))+)*/?|/(?=$))))|(?i)(?<p>(/(?!/))?(([!$&-.:;=@[\]()\w]+|%([13-9a-f][\da-f]|0[1-9a-f]|2[\da-e]))+(/|(?=$)))*))$", RegexOptions.Compiled);

        private LinuxFileUriFactory() { }

        public override Regex FormatDetectionRegex => FORMAT_DETECTION_REGEX;

        public override Regex HostNameRegex => HOST_NAME_REGEX;

        public override Regex FileUriStrictRegex => FILE_URI_STRICT_REGEX;

        public override Regex LocalFsPathRegex => FS_PATH_REGEX;

        public override char LocalDirectorySeparatorChar => '/';

        public override string FsDisplayName => "Linux";

        public override PlatformType FsPlatform => PlatformType.Linux;

        protected override string EscapeSpecialPathChars(string path) => path.Replace(UriHelper.URI_QUERY_DELIMITER_STRING, UriHelper.URI_QUERY_DELIMITER_ESCAPED)
            .Replace(UriHelper.URI_FRAGMENT_DELIMITER_STRING, UriHelper.URI_FRAGMENT_DELIMITER_ESCAPED);

        protected override bool ContainsSpecialPathChars(string path) => path.Contains(UriHelper.URI_QUERY_DELIMITER_CHAR) || path.Contains(UriHelper.URI_FRAGMENT_DELIMITER_CHAR);

        public override string ToLocalFsPath(string hostName, string uriPath)
        {
            if (string.IsNullOrEmpty(hostName))
                return (string.IsNullOrEmpty(uriPath)) ? uriPath : Uri.UnescapeDataString(uriPath);
            return (string.IsNullOrEmpty(uriPath)) ? $"//{hostName}" : Uri.UnescapeDataString(uriPath);
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
            }
            else
                hostName = "";
            return EscapeSpecialPathChars(Uri.EscapeUriString(path.Replace(LocalDirectorySeparatorChar, URI_PATH_SEPARATOR_CHAR)));
        }
    }
}

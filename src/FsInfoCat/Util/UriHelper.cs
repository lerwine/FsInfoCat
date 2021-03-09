using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace FsInfoCat.Util
{
    public static class UriHelper
    {
        public const string URI_SCHEME_URN = "urn";
        public const string URN_NAMESPACE_UUID = "uuid";
        public const string URN_NAMESPACE_VOLUME = "volume";
        public const string URN_NAMESPACE_ID = "id";

        public const UriComponents BEFORE_USERINFO_COMPONENTS = UriComponents.Scheme | UriComponents.KeepDelimiter;

        public const UriComponents AFTER_USERINFO_COMPONENTS = UriComponents.HostAndPort | UriComponents.PathAndQuery |
            UriComponents.Fragment | UriComponents.KeepDelimiter;

        public const UriComponents BEFORE_HOST_COMPONENTS = UriComponents.Scheme | UriComponents.UserInfo | UriComponents.KeepDelimiter;

        public const UriComponents AFTER_PORT_COMPONENTS = UriComponents.PathAndQuery | UriComponents.Fragment | UriComponents.KeepDelimiter;

        public const UriComponents BEFORE_PATH_COMPONENTS = UriComponents.Scheme | UriComponents.UserInfo | UriComponents.HostAndPort |
            UriComponents.KeepDelimiter;

        public const UriComponents AFTER_PATH_COMPONENTS = UriComponents.Query | UriComponents.Fragment | UriComponents.KeepDelimiter;

        public const UriComponents BEFORE_QUERY_COMPONENTS = UriComponents.Scheme | UriComponents.UserInfo | UriComponents.HostAndPort |
            UriComponents.Path | UriComponents.KeepDelimiter;

        public const UriComponents AFTER_QUERY_COMPONENTS = UriComponents.Fragment | UriComponents.KeepDelimiter;

        public const UriComponents BEFORE_FRAGMENT_COMPONENTS = UriComponents.Scheme | UriComponents.UserInfo | UriComponents.HostAndPort |
            UriComponents.PathAndQuery | UriComponents.KeepDelimiter;

        /// <summary>
        /// Matches a path segment including optional trailing slash.
        /// </summary>
        public static readonly Regex URI_PATH_SEGMENT_REGEX = new Regex(@"(?:^|\G)(?:/|([^/]+)(?:/|$))", RegexOptions.Compiled);

        public const char URI_QUERY_DELIMITER_CHAR = '?';
        public const char URI_FRAGMENT_DELIMITER_CHAR = '#';
        public const char URI_PATH_SEPARATOR_CHAR = '/';
        public const string URI_PATH_SEPARATOR_STRING = "/";
        public const char URI_SCHEME_SEPARATOR_CHAR = ':';

        public const string PATTERN_DNS_NAME = @"(?i)^\s*(?=\S{1,255}\s*$)(?=[\d.]*[a-z_-])[a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?\s*$";
        public static readonly Regex DNS_NAME_REGEX = new Regex(PATTERN_DNS_NAME, RegexOptions.Compiled);

        public const string PATTERN_IPV6_ADDRESS = @"(?i)^\s*(?=\[[a-f\d:]+\]|[a-f\d]*:)\[?(:(:[a-f\d]{1,4}){1,7}|(?=([a-f\d]+:)+((:[a-f\d]+)+|:|[a-f\d]+))([a-f\d]{0,4}:){1,7}[a-f\d]{0,4})\]?\s*$";
        public static readonly Regex IPV6_ADDRESS_REGEX = new Regex(PATTERN_IPV6_ADDRESS, RegexOptions.Compiled);

        public const string PATTERN_IPV2_ADDRESS = @"^\s*(?=((2(5[0-5]?|[0-4]?\d?)?|[01]?\d\d?)(\.|(?![.\d]))){4})\d+(\.\d+){3}\s*$";
        public static readonly Regex IPV2_ADDRESS_REGEX = new Regex(PATTERN_IPV2_ADDRESS, RegexOptions.Compiled);

        public static class Windows
        {
            public const string PATTERN_LOCAL_FS_NAME = @"^[^\u0000-\u0019""<>|:*?\\/]+$";
            public static readonly Regex LOCAL_FS_NAME_REGEX = new Regex(PATTERN_LOCAL_FS_NAME, RegexOptions.Compiled);

            public const string PATTERN_ABS_FS_PATH = @"(?i)^(?:\s+(?=//|\\\\|[a-z]:))?((//|\\\\)([a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?|\[?(:(:[a-f\d]{1,4}){1,7}|(?=[a-f\d]{0,4}(:[a-f\d]{0,4}){2,7})([a-f\d]+:)+(([a-f\d]+|:)(?!:[\da-f])|(:[a-f\d]+)+(?!:[\da-f])))\]?)(?=[/\\]|$))?([a-z]:(?=[/\\]|$))?[/\\]?([^\u0000-\u0019""<>|:;*?\\/]+([\\/](?![\\/]))?)*$";

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

            public const string PATTERN_FS_URI_NAME = @"(?i)^([^\u0000-\u0019""<>|:;*?\\/%]+|%([^012357%]|2[^2af]|3[\dd]|[57][^c]|(?=%|$)))+$";
            public static readonly Regex FS_URI_NAME_REGEX = new Regex(PATTERN_FS_URI_NAME, RegexOptions.Compiled);

            public const string PATTERN_ABS_FILE_URI_OR_LOCAL_LAX = @"(?i)^\s*(?=\S)(file://([a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?|\[?(:(:[a-f\d]{1,4}){1,7}|(?=[a-f\d]{0,4}(:[a-f\d]{0,4}){2,7})([a-f\d]+:)+(:|(:[a-f\d]+)+))\]?|/[a-f]:)|[\\/]{2}([a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?|\[?(:(:[a-f\d]{1,4}){1,7}|(?=[a-f\d]{0,4}(:[a-f\d]{0,4}){2,7})([a-f\d]+:)+(:|(:[a-f\d]+)+))\]?)|[a-f]:)([\\/]([^\u0000-\u0019\u007f-\u00a0\u1680\u2028\u2029\ud800-\udfff/"" <>\\/|]+|%([^012357%]|2[^2af]|3[^acef]|[57][^c]|(?=%|$)))+)*[\\/]?\s*$";
            public static readonly Regex ABS_FILE_URI_OR_LOCAL_LAX_REGEX = new Regex(PATTERN_ABS_FILE_URI_OR_LOCAL_LAX, RegexOptions.Compiled);

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
            public static readonly Regex FORMAT_GUESS_REGEX = new Regex(@"^((?<f>file:[\\/]{2}((?<h>[^\\/]+)(?<p>[\\/].*)?|(?<p>[\\/][a-z]:([\\/].*)?)))|(?<u>[\\/]{2}(?<h>[^\\/]+)(?<p>([\\/](?![\\/]).*)?))|(?<p>[a-z]:([\\/].*)?)|(?<x>[a-z][\w-]+:([\\/][\\/]?)?(?<h>([^?#/@:]*(:[^?#/@:]*)?@)?[^?#/@:]+(:\d+)?)?(?<p>([\\/:].*)?)))$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

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
        }

        public static class Linux
        {
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
            public static readonly Regex FORMAT_GUESS_REGEX = new Regex(@"^((?<f>file://(?<h>[^/]+)?(?<p>/.*)?)|(?<u>//(?<h>[^/]+)(?<p>(/.*)?))|(?<p>/(?!/).*)|(?<x>[a-z][\w-]*:(//?)?(?<h>([^?#/@:]*(:[^?#/@:]*)?@)?[^?#/@:]+(:\d+)?)?(?<p>([/:].*)?)))$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            /// <summary>
            /// Matches a well-formed URI that can be converted to a valid absolute or relative local path on a typical Linux filesystem.
            /// </summary>
            /// <remarks>Named group match meanings:
            /// <list type="bullet">
            /// <item><term><seealso cref="PATH_MATCH_GROUP_NAME_ABSOLUTE">a</seealso></term> Text is an absolute file URL. If this group is not matched, then the text is a relative URL that can be used as the path of a file URL.</item>
            /// <item><term><seealso cref="PATH_MATCH_GROUP_NAME_HOST">h</seealso></term> Matches the host name. This will never match when group <c>a</c> is not matched.</item>1:2:3:4:5:6:7:8
            /// <item><term><seealso cref="PATH_MATCH_GROUP_NAME_PATH">p</seealso></term> Matches the path. This match will always succeed when entire expression succeeds, even if the path is an empty string.</item>
            /// </list></remarks>
            public static readonly Regex FILE_URI_STRICT_REGEX = new Regex(@"^((?<a>file://(?i)(?<h>[a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?|\[?(:(:[a-f\d]{1,4}){1,7}|(?=[a-f\d]{0,4}(:[a-f\d]{0,4}){2,7})([a-f\d]+:)+(([a-f\d]+|:)(?!:[\da-f])|(:[a-f\d]+)+(?!:[\da-f])))\]?)?(?i)(?<p>((/([!$&-.:;=@[\]\w]+|%([13-9a-f][\da-f]|0[1-9a-f]|2[\da-e]))+)*/?|/(?=$))))|(?i)(?<p>(/(?!/))?(([!$&-.:;=@[\]\w]+|%([13-9a-f][\da-f]|0[1-9a-f]|2[\da-e]))+(/|(?=$)))*))$", RegexOptions.Compiled);
        }

        public static readonly string LOCAL_FILE_SYSTEM_DISPLAY_NAME;
        public static readonly string ALT_FILE_SYSTEM_DISPLAY_NAME;

        /// <summary>
        /// Matches a well-formed local path on the typical filesystem for the current host.
        /// </summary>
        /// <remarks>Named group match meanings:
        /// <list type="bullet">
        /// <item><term><seealso cref="PATH_MATCH_GROUP_NAME_ABSOLUTE">a</seealso></term> Text is an absolute local path. If this group is not matched, then the text is either a URN or a relative local path.</item>
        /// <item><term><seealso cref="PATH_MATCH_GROUP_NAME_HOST">h</seealso></term> Matches the host name. This implies that the text is a URN.</item>
        /// <item><term><seealso cref="PATH_MATCH_GROUP_NAME_PATH">p</seealso></term> Matches the path minus the host name. This match will always succeed when entire expression succeeds, even if the path is an empty string.</item>
        /// </list></remarks>
        public static readonly Regex LOCAL_FS_PATH_REGEX;

        /// <summary>
        /// Matches a well-formed filesystem-local path for a filesystem alternative to the current host.
        /// </summary>
        /// <remarks>Named group match meanings:
        /// <list type="bullet">
        /// <item><term><seealso cref="PATH_MATCH_GROUP_NAME_ABSOLUTE">a</seealso></term> Text is an absolute filesystem-local path. If this group is not matched, then the text is either a URN or a relative path.</item>
        /// <item><term><seealso cref="PATH_MATCH_GROUP_NAME_HOST">h</seealso></term> Matches the host name. This implies that the text is a URN.</item>
        /// <item><term><seealso cref="PATH_MATCH_GROUP_NAME_PATH">p</seealso></term> Matches the path minus the host name. This match will always succeed when entire expression succeeds, even if the path is an empty string.</item>
        /// </list></remarks>
        public static readonly Regex ALT_FS_PATH_REGEX;

        public const string PATTERN_ANY_ABS_FILE_URI_OR_LOCAL_LAX = @"(?i)^\s*(?=\S)(file://([a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?|\[?(:(:[a-f\d]{1,4}){1,7}|(?=[a-f\d]{0,4}(:[a-f\d]{0,4}){2,7})([a-f\d]+:)+(:|(:[a-f\d]+)+))\]?|/[a-f]:)?|[\\/]{2}([a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?|\[?(:(:[a-f\d]{1,4}){1,7}|(?=[a-f\d]{0,4}(:[a-f\d]{0,4}){2,7})([a-f\d]+:)+(:|(:[a-f\d]+)+))\]?)|[a-f]:)?(([\\/]([^\u0000-\u0019\u007f-\u00a0\u1680\u2028\u2029\ud800-\udfff\\/]+|%([^025%]|0[^0]|2[^f]|5[\dabde](?=%|$)))+)*/?)\s*$";
        public static readonly Regex ANY_ABS_FILE_URI_OR_LOCAL_LAX_REGEX = new Regex(PATTERN_ANY_ABS_FILE_URI_OR_LOCAL_LAX, RegexOptions.Compiled);

        /// <summary>
        /// Match group name for <see cref="FORMAT_GUESS_LOCAL_REGEX"/>, <see cref="FORMAT_GUESS_ALT_REGEX"/>, <see cref="Windows.FORMAT_GUESS_REGEX"/>
        /// and <see cref="Linux.FORMAT_GUESS_REGEX"/> for absolute file URL match.
        /// </summary>
        public const string FORMAT_GUESS_MATCH_GROUP_FILE_URL = "f";

        /// <summary>
        /// Match group name for <see cref="FORMAT_GUESS_LOCAL_REGEX"/>, <see cref="FORMAT_GUESS_ALT_REGEX"/>, <see cref="Windows.FORMAT_GUESS_REGEX"/>
        /// and <see cref="Linux.FORMAT_GUESS_REGEX"/> for URN path match.
        /// </summary>
        public const string FORMAT_GUESS_MATCH_GROUP_URN = "u";

        /// <summary>
        /// Match group name for <see cref="FORMAT_GUESS_LOCAL_REGEX"/>, <see cref="FORMAT_GUESS_ALT_REGEX"/>, <see cref="Windows.FORMAT_GUESS_REGEX"/>
        /// and <see cref="Linux.FORMAT_GUESS_REGEX"/> for non-file URI match.
        /// </summary>
        public const string FORMAT_GUESS_MATCH_GROUP_NON_FILE = "x";

        /// <summary>
        /// <seealso cref="Regex"/> that can be used to guess the format of a string from the perspective of the current host file system.
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
        /// <item>If the expression matches, but groups <c>f</c> and <c>u</c> do not, then text appears to be an absolute path.</item>
        /// <item>If the expression does not match at all, then the text is assumed to be a non-rooted relative path.</item>
        /// </list></remarks>
        public static readonly Regex FORMAT_GUESS_LOCAL_REGEX;

        /// <summary>
        /// <seealso cref="Regex"/> that can be used to guess the format of a string from the perspective alternate to the current host file system.
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
        /// <item>If the expression matches, but groups <c>f</c> and <c>u</c> do not, then text appears to be an absolute path.</item>
        /// <item>If the expression does not match at all, then the text is assumed to be a non-rooted relative path.</item>
        /// </list></remarks>
        public static readonly Regex FORMAT_GUESS_ALT_REGEX;

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

        /// <summary>
        /// Matches a well-formed URI that can be converted to a valid absolute or relative local path on the typical filesystem for the current host.
        /// </summary>
        /// <remarks>Named group match meanings:
        /// <list type="bullet">
        /// <item><term><seealso cref="PATH_MATCH_GROUP_NAME_ABSOLUTE">a</seealso></term> Text is an absolute file URL. If this group is not matched, then the text is a relative URL that can be used as the path of a file URL.</item>
        /// <item><term><seealso cref="PATH_MATCH_GROUP_NAME_HOST">h</seealso></term> Matches the host name. This will never match when group <c>a</c> is not matched.</item>
        /// <item><term><seealso cref="PATH_MATCH_GROUP_NAME_PATH">p</seealso></term> Matches the path. This match will always succeed when entire expression succeeds, even if the path is an empty string.</item>
        /// </list></remarks>
        public static readonly Regex LOCAL_FILE_URI_STRICT_REGEX;

        /// <summary>
        /// Matches a well-formed URI that can be converted to a valid absolute or relative filesystem-local path on the filesystem alternative to the current host.
        /// </summary>
        /// <remarks>Named group match meanings:
        /// <list type="bullet">
        /// <item><term><seealso cref="PATH_MATCH_GROUP_NAME_ABSOLUTE">a</seealso></term> Text is an absolute file URL. If this group is not matched, then the text is a relative URL that can be used as the path of a file URL.</item>
        /// <item><term><seealso cref="PATH_MATCH_GROUP_NAME_HOST">h</seealso></term> Matches the host name. This will never match when group <c>a</c> is not matched.</item>
        /// <item><term><seealso cref="PATH_MATCH_GROUP_NAME_PATH">p</seealso></term> Matches the path. This match will always succeed when entire expression succeeds, even if the path is an empty string.</item>
        /// </list></remarks>
        public static readonly Regex ALT_FILE_URI_STRICT_REGEX;

        /// <summary>
        /// Matches a well-formed URI that can be converted to a valid absolute or relative filesystem-local path.
        /// </summary>
        /// <remarks>Named group match meanings:
        /// <list type="bullet">
        /// <item><term><seealso cref="PATH_MATCH_GROUP_NAME_ABSOLUTE">a</seealso></term> Text is an absolute file URL. If this group is not matched, then the text is a relative URL that can be used as the path of a file URL.</item>
        /// <item><term><seealso cref="PATH_MATCH_GROUP_NAME_HOST">h</seealso></term> Matches the host name. This will never match when group <c>a</c> is not matched.</item>
        /// <item><term><seealso cref="PATH_MATCH_GROUP_NAME_PATH">p</seealso></term> Matches the path. This match will always succeed when entire expression succeeds, even if the path is an empty string.</item>
        /// </list></remarks>
        public static readonly Regex ANY_FILE_URI_STRICT_REGEX = new Regex(@"^((?<a>file://(?i)^(?<h>[a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?|\[?(:(:[a-f\d]{1,4}){1,7}|(?=[a-f\d]{0,4}(:[a-f\d]{0,4}){2,7})([a-f\d]+:)+(:|(:[a-f\d]+)+))\]?)?(?<p>(/([!$&-.:;=@[\]\w]+|%([13-9a-f][\da-f]|0[1-9a-f]|2[\da-e]))+)*/?))|(?<p>(?i)(/(?!/))?(([!$&-.:;=@[\]\w]+|%([13-9a-f][\da-f]|0[1-9a-f]|2[\da-e]))+(/|(?=$)))*))$", RegexOptions.Compiled);

        public static PlatformType PLATFORM_TYPE { get; }

        private static readonly char[] _QUERY_OR_FRAGMENT_DELIMITER = new char[] { URI_QUERY_DELIMITER_CHAR, URI_FRAGMENT_DELIMITER_CHAR };
        private static readonly char[] _INVALID_FILENAME_CHARS;
        private static readonly Func<string, string> _FROM_LOCAL_PATH_PRE_ENCODE;

        static UriHelper()
        {
            char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
            _INVALID_FILENAME_CHARS = invalidFileNameChars.Contains(Path.PathSeparator) ? invalidFileNameChars : invalidFileNameChars.Concat(new char[] { Path.PathSeparator }).ToArray();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                PLATFORM_TYPE = PlatformType.Windows;
                LOCAL_FILE_URI_STRICT_REGEX = Windows.FILE_URI_STRICT_REGEX;
                ALT_FILE_URI_STRICT_REGEX = Linux.FILE_URI_STRICT_REGEX;
                FORMAT_GUESS_LOCAL_REGEX = Windows.FORMAT_GUESS_REGEX;
                FORMAT_GUESS_ALT_REGEX = Linux.FORMAT_GUESS_REGEX;
                _FROM_LOCAL_PATH_PRE_ENCODE = s => s.Replace("#", "%23").Replace("/", "%5C");
                LOCAL_FILE_SYSTEM_DISPLAY_NAME = "Windows";
                ALT_FILE_SYSTEM_DISPLAY_NAME = "Linux";
            }
            else
            {
                PLATFORM_TYPE = PlatformType.Linux;
                LOCAL_FILE_URI_STRICT_REGEX = Linux.FILE_URI_STRICT_REGEX;
                ALT_FILE_URI_STRICT_REGEX = Windows.FILE_URI_STRICT_REGEX;
                FORMAT_GUESS_LOCAL_REGEX = Linux.FORMAT_GUESS_REGEX;
                FORMAT_GUESS_ALT_REGEX = Windows.FORMAT_GUESS_REGEX;
                _FROM_LOCAL_PATH_PRE_ENCODE = s => s.Replace("#", "%23").Replace("?", "%3F");
                LOCAL_FILE_SYSTEM_DISPLAY_NAME = "Windows";
                ALT_FILE_SYSTEM_DISPLAY_NAME = "Linux";
            }
        }

        public static FileStringFormat ParseUriOrPath(string source, bool preferFsPath, out string hostName, out string path)
        {
            if (string.IsNullOrEmpty(source))
            {
                hostName = "";
                path = "";
                return (preferFsPath) ? FileStringFormat.RelativeLocalPath : FileStringFormat.RelativeLocalUri;
            }

            Match guessMatch = FORMAT_GUESS_LOCAL_REGEX.Match(source);
            Match uriMatch, fsMatch;
            if (guessMatch.Success)
            {
                if (guessMatch.Groups[FORMAT_GUESS_MATCH_GROUP_NON_FILE].Success)
                {
                    hostName = GetHostGroupMatchValue(guessMatch);
                    path = GetPathGroupMatchValue(guessMatch);
                    if (!(guessMatch = FORMAT_GUESS_ALT_REGEX.Match(source)).Success || guessMatch.Groups[FORMAT_GUESS_MATCH_GROUP_NON_FILE].Success)
                        return (Uri.IsWellFormedUriString(source, UriKind.Absolute)) ? FileStringFormat.WellFormedNonFileUrl : FileStringFormat.NonFileUrl;
                }
                else
                {
                    if ((uriMatch = LOCAL_FILE_URI_STRICT_REGEX.Match(source)).Success && IsAbsoluteGroupMatch(uriMatch))
                    {
                        hostName = GetHostGroupMatchValue(uriMatch);
                        path = GetPathGroupMatchValue(uriMatch);
                        return FileStringFormat.WellformedAbsoluteLocalUrl;
                    }
                    Match altUriMatch = ALT_FILE_URI_STRICT_REGEX.Match(source);
                    if (altUriMatch.Success && IsAbsoluteGroupMatch(altUriMatch))
                    {
                        hostName = GetHostGroupMatchValue(altUriMatch);
                        path = GetPathGroupMatchValue(altUriMatch);
                        return FileStringFormat.WellformedAbsoluteAltUrl;
                    }
                    if (guessMatch.Groups[FORMAT_GUESS_MATCH_GROUP_FILE_URL].Success)
                    {
                        path = GetPathGroupMatchValue(guessMatch);
                        hostName = GetHostGroupMatchValue(guessMatch);
                        return (uriMatch.Success || !altUriMatch.Success) ? FileStringFormat.AbsoluteLocalUrl : FileStringFormat.AbsoluteAltUrl;
                    }
                    if ((fsMatch = ALT_FS_PATH_REGEX.Match(source)).Success && IsAbsoluteGroupMatch(fsMatch))
                    {
                        path = GetPathGroupMatchValue(fsMatch);
                        return ((hostName = GetHostGroupMatchValue(fsMatch)).Length > 0) ? FileStringFormat.WellFormedLocalUrn : FileStringFormat.WellFormedAbsoluteLocalPath;
                    }
                    Match altFsMatch = ALT_FS_PATH_REGEX.Match(source);
                    if (altFsMatch.Success && IsAbsoluteGroupMatch(altFsMatch))
                    {
                        path = GetPathGroupMatchValue(fsMatch);
                        return ((hostName = GetHostGroupMatchValue(fsMatch)).Length > 0) ? FileStringFormat.WellFormedAltUrn : FileStringFormat.WellFormedAbsoluteAltPath;
                    }
                    hostName = GetHostGroupMatchValue(guessMatch);
                    if (guessMatch.Groups[FORMAT_GUESS_MATCH_GROUP_URN].Success)
                    {
                        path = GetPathGroupMatchValue(guessMatch);
                        return (fsMatch.Success || !altFsMatch.Success) ? FileStringFormat.LocalUrn : FileStringFormat.AltUrn;
                    }
                    if (preferFsPath)
                    {
                        if (fsMatch.Success)
                        {
                            path = GetPathGroupMatchValue(fsMatch);
                            return FileStringFormat.WellFormedRelativeLocalPath;
                        }
                        if (altFsMatch.Success)
                        {
                            path = GetPathGroupMatchValue(altFsMatch);
                            return FileStringFormat.WellFormedRelativeAltPath;
                        }
                        if (uriMatch.Success)
                        {
                            path = GetPathGroupMatchValue(uriMatch);
                            return FileStringFormat.WellformedRelativeLocalUri;
                        }
                        if (altUriMatch.Success)
                        {
                            path = GetPathGroupMatchValue(altUriMatch);
                            return FileStringFormat.WellformedRelativeAltUri;
                        }
                        path = GetPathGroupMatchValue(guessMatch);
                        return FileStringFormat.RelativeAltPath;
                    }
                    if (uriMatch.Success)
                    {
                        path = GetPathGroupMatchValue(uriMatch);
                        return FileStringFormat.WellformedRelativeLocalUri;
                    }
                    if (altUriMatch.Success)
                    {
                        path = GetPathGroupMatchValue(altUriMatch);
                        return FileStringFormat.WellformedRelativeAltUri;
                    }
                    if (fsMatch.Success)
                    {
                        path = GetPathGroupMatchValue(fsMatch);
                        return FileStringFormat.WellFormedRelativeLocalPath;
                    }
                    if (altFsMatch.Success)
                    {
                        path = GetPathGroupMatchValue(altFsMatch);
                        return FileStringFormat.WellFormedRelativeAltPath;
                    }
                    path = GetPathGroupMatchValue(guessMatch);
                    return FileStringFormat.RelativeAltUri;
                }
            }
            else
            {
                if ((guessMatch = FORMAT_GUESS_ALT_REGEX.Match(source)).Success)
                {
                    if (guessMatch.Groups[FORMAT_GUESS_MATCH_GROUP_NON_FILE].Success)
                    {
                        hostName = GetHostGroupMatchValue(guessMatch);
                        path = GetPathGroupMatchValue(guessMatch);
                        return (Uri.IsWellFormedUriString(source, UriKind.Absolute)) ? FileStringFormat.WellFormedNonFileUrl : FileStringFormat.NonFileUrl;
                    }
                }
                else
                {
                    hostName = "";
                    path = source;
                    return FileStringFormat.Invalid;
                }
            }

            if ((uriMatch = ALT_FILE_URI_STRICT_REGEX.Match(source)).Success && IsAbsoluteGroupMatch(uriMatch))
            {
                hostName = GetHostGroupMatchValue(uriMatch);
                path = GetPathGroupMatchValue(uriMatch);
                return FileStringFormat.WellformedAbsoluteAltUrl;
            }

            if (guessMatch.Groups[FORMAT_GUESS_MATCH_GROUP_FILE_URL].Success)
            {
                hostName = GetHostGroupMatchValue(guessMatch);
                path = GetPathGroupMatchValue(guessMatch);
                return FileStringFormat.AbsoluteAltUrl;
            }

            if ((fsMatch = ALT_FS_PATH_REGEX.Match(source)).Success && IsAbsoluteGroupMatch(fsMatch))
            {
                path = GetPathGroupMatchValue(fsMatch);
                return ((hostName = GetHostGroupMatchValue(fsMatch)).Length > 0) ? FileStringFormat.WellFormedAltUrn : FileStringFormat.WellFormedAbsoluteAltPath;
            }
            hostName = GetHostGroupMatchValue(guessMatch);
            if (guessMatch.Groups[FORMAT_GUESS_MATCH_GROUP_URN].Success)
            {
                path = GetPathGroupMatchValue(guessMatch);
                return FileStringFormat.AltUrn;
            }

            if (preferFsPath)
            {
                if (fsMatch.Success)
                {
                    path = GetPathGroupMatchValue(fsMatch);
                    return FileStringFormat.WellFormedRelativeAltPath;
                }
                if (uriMatch.Success)
                {
                    path = GetPathGroupMatchValue(uriMatch);
                    return FileStringFormat.WellformedRelativeAltUri;
                }
                path = GetPathGroupMatchValue(guessMatch);
                return FileStringFormat.RelativeAltPath;
            }
            if (uriMatch.Success)
            {
                path = GetPathGroupMatchValue(uriMatch);
                return FileStringFormat.WellformedRelativeAltUri;
            }
            if (fsMatch.Success)
            {
                path = GetPathGroupMatchValue(fsMatch);
                return FileStringFormat.WellFormedRelativeAltPath;
            }
            path = GetPathGroupMatchValue(guessMatch);
            return FileStringFormat.RelativeAltUri;
        }

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

        private static int _UriGetPartIndexes(string text, out int schemeLength, out int hostNameLength)
        {
            using (CharEnumerator enumerator = text.GetEnumerator())
            {
                schemeLength = hostNameLength = 0;
                if (!enumerator.MoveNext())
                    return -1;
                switch (enumerator.Current)
                {
                    case URI_PATH_SEPARATOR_CHAR:
                    case URI_FRAGMENT_DELIMITER_CHAR:
                    case URI_QUERY_DELIMITER_CHAR:
                    case URI_SCHEME_SEPARATOR_CHAR:
                        return -1;
                }
                while (enumerator.MoveNext())
                {
                    schemeLength++;
                    switch (enumerator.Current)
                    {
                        case URI_SCHEME_SEPARATOR_CHAR:
                            if (enumerator.MoveNext() && enumerator.Current == URI_PATH_SEPARATOR_CHAR && enumerator.MoveNext() && enumerator.Current == URI_PATH_SEPARATOR_CHAR)
                            {
                                while (enumerator.MoveNext())
                                {
                                    switch (enumerator.Current)
                                    {
                                        case URI_PATH_SEPARATOR_CHAR:
                                        case URI_FRAGMENT_DELIMITER_CHAR:
                                        case URI_QUERY_DELIMITER_CHAR:
                                            return hostNameLength + schemeLength + 2;
                                    }
                                    hostNameLength++;
                                }
                                return hostNameLength + schemeLength + 2;
                            }
                            schemeLength = hostNameLength = 0;
                            return -1;
                        case URI_PATH_SEPARATOR_CHAR:
                        case URI_FRAGMENT_DELIMITER_CHAR:
                        case URI_QUERY_DELIMITER_CHAR:
                            schemeLength = hostNameLength = 0;
                            return -1;
                    }
                }
            }
            schemeLength = hostNameLength = 0;
            return -1;
        }

        public static bool TryParseFileUriString(string uriString, out string hostName, out string absolutePath, out int leafIndex)
        {
            if (string.IsNullOrEmpty(uriString))
            {
                hostName = "";
                absolutePath = "";
                leafIndex = 0;
                return true;
            }


            uriString = _FROM_LOCAL_PATH_PRE_ENCODE((uriString[0] == URI_PATH_SEPARATOR_CHAR && (uriString.Length == 1 || uriString[1] != URI_PATH_SEPARATOR_CHAR)) ?
                $"file://{uriString}" : uriString);

            if (Uri.TryCreate(uriString, UriKind.Absolute, out Uri uri) && uri.Scheme.Equals(Uri.UriSchemeFile) && string.IsNullOrEmpty(uri.Query) && string.IsNullOrEmpty(uri.Fragment))
            {
                hostName = uri.Host;
                absolutePath = uri.AbsolutePath;
                int i = absolutePath.LastIndexOf(URI_PATH_SEPARATOR_CHAR);
                if (i > 0 && absolutePath[i] == URI_PATH_SEPARATOR_CHAR)
                {
                    absolutePath = absolutePath.Substring(0, i);
                    i = absolutePath.LastIndexOf(URI_PATH_SEPARATOR_CHAR);
                }
                leafIndex = (i < 0) ? 0 : i + 1;
                return true;
            }
            hostName = absolutePath = null;
            leafIndex = 0;
            return false;
        }

        public static bool TryCreateFileUriFromUriString(string uriString, out Uri uri)
        {
            if (string.IsNullOrEmpty(uriString))
            {
                uri = new Uri("", UriKind.Relative);
                return true;
            }
            uriString = _FROM_LOCAL_PATH_PRE_ENCODE(uriString);
            if (Uri.TryCreate(uriString, UriKind.Absolute, out uri))
                return uri.Scheme.Equals(Uri.UriSchemeFile) && string.IsNullOrEmpty(uri.Query) && string.IsNullOrEmpty(uri.Fragment);
            return Uri.TryCreate((Uri.IsWellFormedUriString(uriString, UriKind.Relative)) ? uriString : Uri.EscapeUriString(uriString), UriKind.Relative, out uri) &&
                uri.OriginalString.IndexOfAny(_QUERY_OR_FRAGMENT_DELIMITER) < 0;
        }

        public static bool TryCreateFileUriFromLocalPath(string localPath, out Uri uri)
        {
            if (string.IsNullOrEmpty(localPath))
            {
                uri = new Uri("", UriKind.Relative);
                return true;
            }
            localPath = _FROM_LOCAL_PATH_PRE_ENCODE(localPath);
            if (Uri.TryCreate(localPath, UriKind.Absolute, out uri))
                return uri.Scheme.Equals(Uri.UriSchemeFile) && string.IsNullOrEmpty(uri.Query) && string.IsNullOrEmpty(uri.Fragment);
            return Uri.TryCreate(localPath, UriKind.Relative, out uri) && uri.OriginalString.IndexOfAny(_QUERY_OR_FRAGMENT_DELIMITER) < 0;
        }

        public static Uri ToUri(this FileSystemInfo fileSystemInfo)
        {
            if (fileSystemInfo is null)
                return new Uri("", UriKind.Relative);
            return new Uri(_FROM_LOCAL_PATH_PRE_ENCODE(fileSystemInfo.FullName), UriKind.Absolute);
        }

        /// <summary>
        /// Determines whether a URI string is well-formed and compatible with the <seealso cref="Uri.UriSchemeFile">file</seealso> URI scheme.
        /// </summary>
        /// <param name="uri">The URI string to test</param>
        /// <param name="kind">The type of URI string.</param>
        /// <returns><see langword="true"/> if <paramref name="uri"/> well-formed and compatible with the <seealso cref="Uri.UriSchemeFile">file</seealso> URI scheme; otherwise, <see langword="false"/>.</returns>
        /// <remarks>For relative URI strings, this will also check to see if the URI string utilizes any special local file system path characters that would otherwise be considered part of a valid relative URI string.</remarks>
        public static bool IsWellFormedFileUriString(string uri, UriKind kind)
        {
            if (string.IsNullOrEmpty(uri))
                return kind != UriKind.Absolute;
            if (!Uri.IsWellFormedUriString(uri, kind))
                return false;
            int startIndex = _UriGetPartIndexes(uri, out int schemeLength, out _);
            if (startIndex < 0)
                return false;
            if (startIndex > 0)
                return uri.Substring(0, schemeLength).Equals(Uri.UriSchemeFile) && (startIndex == uri.Length || ANY_FILE_URI_STRICT_REGEX.IsMatch(uri.Substring(startIndex)));
            return ANY_FILE_URI_STRICT_REGEX.IsMatch(uri);
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

        /// <summary>
        /// Gets the length of the path contained within a relative URI string.
        /// </summary>
        /// <param name="relativeUriString">The relative URI string</param>
        /// <param name="nextIsQuery">Returns <see langword="true"/> if the next character is the start of the query, <see langword="false"/> if it starts the fragment or <see langword="null"/> if the <paramref name="relativeUriString"/>
        /// contains no query or fragment.</param>
        /// <returns>The length of the path contained within the <paramref name="relativeUriString"/>.</returns>
        public static int GetUriPathLength(string relativeUriString, out bool? nextIsQuery)
        {
            if (relativeUriString is null)
            {
                nextIsQuery = null;
                return 0;
            }

            int len = relativeUriString.Length;
            if (len > 0)
            {
                int i = relativeUriString.IndexOfAny(_QUERY_OR_FRAGMENT_DELIMITER);
                if (i > -1)
                {
                    nextIsQuery = relativeUriString[i] == URI_QUERY_DELIMITER_CHAR;
                    return i;
                }
            }

            nextIsQuery = null;
            return len;
        }

        /// <summary>
        /// Normalizes a URL so it is compatible for searches and comparison.
        /// </summary>
        /// <remarks><seealso cref="Uri.Host"/> name and <seealso cref="Uri.Scheme"/> is converted to lower case. Default <seealso cref="Uri.Port"/> numbers
        /// and empty <seealso cref="Uri.Query"/> strings and <seealso cref="Uri.Fragment"/>s are removed. If the <seealso cref="Uri.OriginalString"/> is not a well-formed URI,
        /// a new <seealso cref="Uri"/> is returned where the <seealso cref="Uri.OriginalString"/> property is well-formed.
        /// Both absolute and relative URIs are supported by this extension method.</remarks>
        public static Uri AsNormalized(this Uri uri)
        {
            if (null == uri)
                return uri;
            string originalString;
            if (uri.IsAbsoluteUri)
            {
                if (!(uri.Scheme.ToLower().Equals(uri.Scheme) && uri.Host.ToLower().Equals(uri.Host)))
                {
                    UriBuilder uriBuilder = new UriBuilder(uri)
                    {
                        Scheme = uri.Scheme.ToLower(),
                        Host = uri.Host.ToLower()
                    };
                    if (uri.Query == "?")
                        uriBuilder.Query = "";
                    if (uri.Fragment == "#")
                        uriBuilder.Fragment = "";
                    uri = uriBuilder.Uri;
                }
                else if (!uri.Host.ToLower().Equals(uri.Host))
                {
                    if (uri.IsDefaultPort)
                        uri.TrySetHostComponent(uri.Host.ToLower(), null, out uri);
                    uri.TrySetHostComponent(uri.Host.ToLower(), uri.Port, out uri);
                }
                if (uri.Query == "?")
                    uri.TrySetQueryComponent(null, out uri);
                if (uri.Fragment == "#")
                    uri.TrySetFragmentComponent(null, out uri);
                originalString = uri.OriginalString;
                if (Uri.IsWellFormedUriString(originalString, UriKind.Absolute))
                    return uri;
                if (Uri.IsWellFormedUriString(uri.AbsoluteUri, UriKind.Absolute))
                    return new Uri(uri.AbsoluteUri, UriKind.Absolute);
                return new Uri(Uri.EscapeUriString(uri.OriginalString.Replace('\\', '/')), UriKind.Absolute);
            }

            string s = uri.GetFragmentComponent();
            if (null != s && s.Length == 0)
                uri.TrySetFragmentComponent(null, out uri);
            s = uri.GetQueryComponent();
            if (null != s && s.Length == 0)
                uri.TrySetQueryComponent(null, out uri);
            if ((originalString = uri.OriginalString).Length == 0 || Uri.IsWellFormedUriString(originalString, UriKind.Relative))
                return uri;
            originalString = EnsureWellFormedUriPath(originalString);
            if (Uri.IsWellFormedUriString(originalString, UriKind.Relative))
                return new Uri(originalString, UriKind.Relative);
            return new Uri(Uri.EscapeDataString(uri.OriginalString), UriKind.Relative);
        }

        /// <summary>
        /// Splits the <seealso cref="Uri.Query"/> strings and <seealso cref="Uri.Fragment"/>, returning a new <seealso cref="Uri"/> without these components.
        /// </summary>
        /// <param name="uri">The <seealso cref="Uri"/> to split</param>
        /// <param name="query">The query string from the original <paramref name="uri"/> or <c>null</c> if it contained no query.
        /// This will not include the leading separator character (<c>?</c>) of the query string.</param>
        /// <param name="fragment">The fragment string from the original <paramref name="uri"/> or <c>null</c> if it contained no fragment.
        /// This will not include the leading separator character (<c>#</c>) of the fragment.</param>
        /// <returns>A <seealso cref="Uri"/> without the <seealso cref="Uri.Query"/> or <seealso cref="Uri.Fragment"/> components.</returns>
        public static Uri SplitQueryAndFragment(this Uri uri, out string query, out string fragment)
        {
            if (uri is null)
            {
                query = fragment = null;
                return null;
            }
            if (uri.IsAbsoluteUri)
            {
                fragment = (uri.Fragment.Length > 0) ? uri.Fragment.Substring(1) : null;
                query = (uri.Fragment.Length > 0) ? uri.Fragment.Substring(1) : null;
                if (null == query && null == fragment)
                    return uri;
                UriBuilder uriBuilder = new UriBuilder(uri);
                uriBuilder.Query = uriBuilder.Fragment = "";
                return uriBuilder.Uri;
            }

            string originalString = uri.OriginalString;
            int index = originalString.IndexOfAny(_QUERY_OR_FRAGMENT_DELIMITER);
            if (index < 0)
            {
                query = fragment = null;
                return uri;
            }
            if (originalString[index] == '#')
            {
                query = null;
                fragment = originalString.Substring(index + 1);
                originalString = originalString.Substring(0, index);
            }
            else
            {
                query = originalString.Substring(index + 1);
                originalString = originalString.Substring(0, index);
                index = query.IndexOf('#');
                if (index < 0)
                    fragment = null;
                else
                {
                    fragment = query.Substring(index + 1);
                    query = query.Substring(0, index);
                }
            }
            return new Uri(originalString, UriKind.Relative);
        }

        /// <summary>
        /// Test whether a <seealso cref="Uri"/> is equal to another <seealso cref="Uri"/> with authority component comparison being case-insensitive.
        /// </summary>
        /// <param name="uri">The target <seealso cref="Uri"/> of the comparison.</param>
        /// <param name="other">The <seealso cref="Uri"/> to compare to.</param>
        /// <returns><c>true</c> if the authority components both <seealso cref="Uri"/>s are equivalent; otherwise, <c>false</c>.</returns>
        /// <remarks>The <seealso cref="UriComponents.Scheme"/>, <seealso cref="UriComponents.Scheme"/> and <seealso cref="UriComponents.Scheme"/> components
        /// are tested using the <seealso cref="StringComparison.InvariantCultureIgnoreCase"/> comparison option. All other components will use exact comparison.</remarks>
        public static bool AuthorityCaseInsensitiveEquals(this Uri uri, Uri other)
        {
            if (null == uri)
                return null == other;
            if (null == other)
                return false;
            if (ReferenceEquals(uri, other))
                return true;
            if (uri.IsAbsoluteUri)
            {
                if (!other.IsAbsoluteUri)
                    return false;
                UriComponents c = UriComponents.Scheme | UriComponents.UserInfo | UriComponents.HostAndPort | UriComponents.KeepDelimiter;
                return uri.GetComponents(c, UriFormat.UriEscaped).Equals(other.GetComponents(c, UriFormat.UriEscaped),
                    StringComparison.InvariantCultureIgnoreCase) &&
                    uri.PathAndQuery.Equals(other.PathAndQuery) && uri.Fragment.Equals(other.Fragment);
            }
            return !other.IsAbsoluteUri && uri.OriginalString.Equals(other.OriginalString);
        }

        /// <summary>
        /// Returns a well-formed relative URI string from the current string value.
        /// </summary>
        /// <param name="value">The string value to be converted to a well-formed relative URI string.</param>
        /// <returns>A string value containing a well-formed relative URI string. Empty and <c>null</c> strings will be returned as-is.</returns>
        /// <remarks>This will do &quot;smart&quot; URI escaping, whereby <seealso cref="Uri.EscapeUriString"/> will not be applied to the string if it already
        /// represents a well-formed URI string.</remarks>
        public static string AsRelativeUriString(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;
            if (Uri.TryCreate(value, UriKind.Absolute, out Uri uri))
                return uri.GetComponents(UriComponents.PathAndQuery | UriComponents.Fragment | UriComponents.KeepDelimiter, UriFormat.UriEscaped);
            if (!Uri.IsWellFormedUriString(value, UriKind.RelativeOrAbsolute))
            {
                int n = value.IndexOf('\\');
                if (n < 0)
                    value = Uri.EscapeUriString(value);
                else
                {
                    int i = value.IndexOfAny(_QUERY_OR_FRAGMENT_DELIMITER);
                    if (i < 0)
                    {
                        if (!Uri.IsWellFormedUriString(value = value.Replace('\\', '/'), UriKind.RelativeOrAbsolute))
                            value = Uri.EscapeUriString(value);
                    }
                    else if (n > i || !Uri.IsWellFormedUriString(value = value.Substring(0, i).Replace('\\', '/') + value.Substring(i), UriKind.RelativeOrAbsolute))
                        value = Uri.EscapeUriString(value);
                }
            }
            return value;
        }

        /// <summary>
        /// Ensures a string value is escaped for inclusion in a URI string as the username portion of the <seealso cref="UriComponents.UserInfo"/> component.
        /// </summary>
        /// <param name="value">The user name to escape.</param>
        /// <returns>A string value that is propertly escaped for inclusion in a URI string as the username portion of the <seealso cref="UriComponents.UserInfo"/> component.</returns>
        public static string AsUserNameComponentEncoded(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return (Uri.IsWellFormedUriString(value, UriKind.Relative) ? value : Uri.EscapeUriString(value))
                .Replace("/", "%2F").Replace(":", "%3A").Replace("?", "%3F").Replace("#", "%23").Replace("@", "%40");
        }

        /// <summary>
        /// Ensures a string value is escaped for inclusion in a URI string as the password portion of the <seealso cref="UriComponents.UserInfo"/> component.
        /// </summary>
        /// <param name="value">The password to escape.</param>
        /// <returns>A string value that is propertly escaped for inclusion in a URI string as the password portion of the <seealso cref="UriComponents.UserInfo"/> component.</returns>
        public static string AsPasswordComponentEncoded(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return (Uri.IsWellFormedUriString(value, UriKind.Relative) ? value : Uri.EscapeUriString(value))
                .Replace("/", "%2F").Replace("?", "%3F").Replace("#", "%23").Replace("@", "%40");
        }

        /// <summary>
        /// Gets the username and password from the <seealso cref="UriComponents.UserInfo"/> component of a <seealso cref="Uri"/>.
        /// </summary>
        /// <param name="uri">The source <seealso cref="Uri"/>.</param>
        /// <param name="password">The password portion from the <seealso cref="UriComponents.UserInfo"/> component of the source <paramref name="uri"/> or <c>null</c> if it contained no password.</param>
        /// <returns>The user name portion from the <seealso cref="UriComponents.UserInfo"/> component of the source <paramref name="uri"/> or <c>null</c> if it contained no <seealso cref="UriComponents.UserInfo"/> component.</returns>
        public static string GetUserNameAndPassword(this Uri uri, out string password)
        {
            if (null != uri && uri.IsAbsoluteUri)
            {
                string userInfo = uri.GetComponents(UriComponents.UserInfo | UriComponents.KeepDelimiter, UriFormat.UriEscaped);
                if (userInfo.Length > 0)
                {
                    string[] upw = uri.GetComponents(UriComponents.UserInfo, UriFormat.UriEscaped).Split(':', 2);
                    password = (upw.Length > 1) ? upw[1] : null;
                    return upw[0];
                }
            }
            password = null;
            return null;
        }

        /// <summary>
        /// Attempts to create a variant of a <seealso cref="Uri"/> with the specified <seealso cref="UriComponents.UserInfo"/> components applied.
        /// </summary>
        /// <param name="uri">The original <seealso cref="Uri"/>.</param>
        /// <param name="userName">The username portion of the <seealso cref="UriComponents.UserInfo"/> component for the new <seealso cref="Uri"/>.
        /// This can be <c>null</c> if you wish to create a <seealso cref="Uri"/> with the <seealso cref="UriComponents.UserInfo"/> omitted.</param>
        /// <param name="password">The password portion of the <seealso cref="UriComponents.UserInfo"/> component for the new <seealso cref="Uri"/>.
        /// This can be <c>null</c> if you wish to create a <seealso cref="Uri"/> with only the username portion of the <seealso cref="UriComponents.UserInfo"/>.</param>
        /// <param name="result">The new <seealso cref="Uri"/> with the specified <seealso cref="UriComponents.UserInfo"/> components applied.</param>
        /// <returns><c>true</c> if the <paramref name="result"/> <seealso cref="Uri"/> was successfully created; otherwise, <c>false</c>.</returns>
        /// <remarks>This can return <c>false</c> if the source <paramref name="uri"/> is <c>null</c>, if it not an absolute URI,
        /// and in cases where the <seealso cref="Uri.Scheme"/> does not support the <seealso cref="UriComponents.UserInfo"/> component.</remarks>
        public static bool TrySetUserInfoComponent(this Uri uri, string userName, string password, out Uri result)
        {
            if (!uri.IsAbsoluteUri || uri.Host.Length == 0)
            {
                result = uri;
                return false;
            }

            string oldUserInfo = uri.GetComponents(UriComponents.UserInfo, UriFormat.UriEscaped);
            if (null != userName)
            {
                string newUserInfo = userName.AsUserNameComponentEncoded();
                if (null != password && (newUserInfo.Length > 0 || password.Length > 0))
                    newUserInfo = $"{newUserInfo}:{password.AsPasswordComponentEncoded()}";
                if (newUserInfo.Length > 0)
                {
                    if (oldUserInfo != newUserInfo)
                        try
                        {
                            string preceding = uri.GetComponents(BEFORE_USERINFO_COMPONENTS, UriFormat.UriEscaped);
                            string following = uri.GetComponents(AFTER_USERINFO_COMPONENTS, UriFormat.UriEscaped);
                            result = new Uri($"{preceding}{newUserInfo}@{following}", UriKind.Absolute);
                            if (preceding != uri.GetComponents(BEFORE_USERINFO_COMPONENTS, UriFormat.UriEscaped) ||
                                following != uri.GetComponents(AFTER_USERINFO_COMPONENTS, UriFormat.UriEscaped))
                            {
                                result = uri;
                                return false;
                            }
                        }
                        catch
                        {
                            result = uri;
                            return false;
                        }
                    else
                        result = uri;
                    return true;
                }
            }

            if (string.IsNullOrEmpty(oldUserInfo))
                result = uri;
            else
                try
                {
                    result = new Uri(uri.GetComponents(UriComponents.Scheme | AFTER_USERINFO_COMPONENTS, UriFormat.UriEscaped), UriKind.Absolute);
                }
                catch
                {
                    result = uri;
                    return false;
                }
            return true;
        }

        /// <summary>
        /// Attempts to create a variant of a <seealso cref="Uri"/> with the specified <seealso cref="UriComponents.HostAndPort"/> components applied.
        /// </summary>
        /// <param name="uri">The original <seealso cref="Uri"/>.</param>
        /// <param name="hostName">The new <seealso cref="UriComponents.Host"/> component or <c>null</c> to create a <seealso cref="Uri"/> without this component.</param>
        /// <param name="port">The new <seealso cref="UriComponents.Port"/> component or <c>null</c> to create a <seealso cref="Uri"/> without this component.</param>
        /// <param name="result">The new <seealso cref="Uri"/> with the specified <seealso cref="UriComponents.HostAndPort"/> components applied.</param>
        /// <remarks>This can return <c>false</c> if the source <paramref name="uri"/> is <c>null</c>, if it not an absolute URI,
        /// and in cases where the <seealso cref="Uri.Scheme"/> does not support the inclusion or omission of the <seealso cref="UriComponents.HostAndPort"/> components.</remarks>
        public static bool TrySetHostComponent(this Uri uri, string hostName, int? port, out Uri result)
        {
            if (!uri.IsAbsoluteUri)
            {
                result = uri;
                return false;
            }

            string oldHostAndPort = uri.GetComponents(UriComponents.HostAndPort, UriFormat.UriEscaped);
            if (!string.IsNullOrEmpty(hostName))
            {
                hostName = hostName.ToLower();
                if (Uri.CheckHostName(hostName) == UriHostNameType.Unknown)
                {
                    result = uri;
                    return false;
                }
                string hostAndPort = hostName;
                if (port.HasValue && port.Value > -1)
                {
                    if (port.Value > 65535)
                    {
                        result = uri;
                        return false;
                    }
                    hostAndPort = $"{hostAndPort}:{port.Value}";
                }
                if (oldHostAndPort != hostAndPort)
                    try
                    {
                        string preceding = uri.GetComponents(BEFORE_HOST_COMPONENTS, UriFormat.UriEscaped);
                        string following = uri.GetComponents(AFTER_PORT_COMPONENTS, UriFormat.UriEscaped);
                        result = new Uri($"{preceding}{hostAndPort}{following}", UriKind.Absolute);
                        if (preceding != uri.GetComponents(BEFORE_HOST_COMPONENTS, UriFormat.UriEscaped) ||
                            following != uri.GetComponents(AFTER_PORT_COMPONENTS, UriFormat.UriEscaped))
                        {
                            result = uri;
                            return false;
                        }
                    }
                    catch
                    {
                        result = uri;
                        return false;
                    }
                else
                    result = uri;
                return true;
            }

            if (string.IsNullOrEmpty(oldHostAndPort))
                result = uri;
            else
                try
                {
                    result = new Uri(uri.GetComponents(BEFORE_HOST_COMPONENTS | AFTER_PORT_COMPONENTS, UriFormat.UriEscaped), UriKind.Absolute);
                }
                catch
                {
                    result = uri;
                    return false;
                }
            return true;
        }

        /// <summary>
        /// Gets the <seealso cref="UriComponents.Path"/> component of a <seealso cref="Uri"/>.
        /// </summary>
        /// <param name="uri">The source <seealso cref="Uri"/>.</param>
        /// <returns>The <seealso cref="UriComponents.Path"/> component of the source <paramref name="uri"/>.
        /// This will return <c>null</c> if the source <paramref name="uri"/> was <c>null</c>.</returns>
        public static string GetPathComponent(this Uri uri)
        {
            if (uri is null)
                return null;
            if (uri.IsAbsoluteUri)
                return uri.GetComponents(UriComponents.Path | UriComponents.KeepDelimiter, UriFormat.UriEscaped);
            string originalString = uri.OriginalString;
            if (originalString.Length == 0)
                return "";
            int index = originalString.IndexOfAny(_QUERY_OR_FRAGMENT_DELIMITER);
            if (index == 0)
                return "";
            if (index > 0)
                originalString = originalString.Substring(0, index);
            return EnsureWellFormedUriPath(originalString);
        }

        public static string SplitQueryComponents(this Uri uri, out string schemeAndAuthority, out string queryAndFragment)
        {
            if (uri is null)
            {
                schemeAndAuthority = queryAndFragment = null;
                return null;
            }

            if (uri.IsAbsoluteUri)
            {
                schemeAndAuthority = uri.GetComponents(UriComponents.Scheme | UriComponents.UserInfo | UriComponents.Host | UriComponents.Port | UriComponents.KeepDelimiter, UriFormat.UriEscaped);
                queryAndFragment = uri.GetComponents(UriComponents.Fragment | UriComponents.Query | UriComponents.KeepDelimiter, UriFormat.UriEscaped);
                return uri.GetComponents(UriComponents.Path | UriComponents.KeepDelimiter, UriFormat.UriEscaped);
            }
            schemeAndAuthority = "";
            string originalString = uri.OriginalString;
            if (originalString.Length == 0)
            {
                queryAndFragment = "";
                return "";
            }
            originalString = EnsureWellFormedUriPath(originalString);

            int pathLen = originalString.IndexOfAny(_QUERY_OR_FRAGMENT_DELIMITER);
            if (pathLen < 0)
            {
                queryAndFragment = "";
                return originalString;
            }
            queryAndFragment = originalString.Substring(pathLen);
            return originalString.Substring(0, pathLen);
        }

        public static string SplitQueryComponents(this Uri uri, out string schemeAndAuthority, out string query, out string fragment)
        {
            if (uri is null)
            {
                schemeAndAuthority = query = fragment = null;
                return null;
            }

            if (uri.IsAbsoluteUri)
            {
                schemeAndAuthority = uri.GetComponents(UriComponents.Scheme | UriComponents.UserInfo | UriComponents.Host | UriComponents.Port | UriComponents.KeepDelimiter, UriFormat.UriEscaped);
                query = uri.GetComponents(UriComponents.Query | UriComponents.KeepDelimiter, UriFormat.UriEscaped);
                fragment = uri.GetComponents(UriComponents.Fragment | UriComponents.KeepDelimiter, UriFormat.UriEscaped);
                return uri.GetComponents(UriComponents.Path | UriComponents.KeepDelimiter, UriFormat.UriEscaped);
            }
            schemeAndAuthority = "";
            string originalString = uri.OriginalString;
            if (originalString.Length == 0)
            {
                query = fragment = "";
                return "";
            }
            originalString = EnsureWellFormedUriPath(originalString);

            int pathLen = originalString.IndexOfAny(_QUERY_OR_FRAGMENT_DELIMITER);
            if (pathLen < 0)
            {
                query = fragment = "";
                return originalString;
            }
            else if (originalString[pathLen] == '#')
            {
                query = "";
                fragment = originalString.Substring(pathLen);
            }
            else
            {
                int i = originalString.IndexOf('#', pathLen);
                if (i < 0)
                {
                    fragment = "";
                    query = originalString.Substring(pathLen);
                }
                else
                {
                    query = originalString.Substring(pathLen, i - pathLen);
                    fragment = originalString.Substring(i);
                }
            }

            return originalString.Substring(0, pathLen);
        }

        public static string SplitQueryComponents(this Uri uri, out string scheme, out string authority, out string query, out string fragment)
        {
            if (uri is null)
            {
                scheme = authority = query = fragment = null;
                return null;
            }

            if (uri.IsAbsoluteUri)
            {
                scheme = uri.GetComponents(UriComponents.Scheme | UriComponents.KeepDelimiter, UriFormat.UriEscaped);
                authority = uri.GetComponents(UriComponents.UserInfo | UriComponents.Host | UriComponents.Port | UriComponents.KeepDelimiter, UriFormat.UriEscaped);
                query = uri.GetComponents(UriComponents.Query | UriComponents.KeepDelimiter, UriFormat.UriEscaped);
                fragment = uri.GetComponents(UriComponents.Fragment | UriComponents.KeepDelimiter, UriFormat.UriEscaped);
                return uri.GetComponents(UriComponents.Path | UriComponents.KeepDelimiter, UriFormat.UriEscaped);
            }
            scheme = authority = "";
            string originalString = uri.OriginalString;
            if (originalString.Length == 0)
            {
                query = fragment = "";
                return "";
            }
            originalString = EnsureWellFormedUriPath(originalString);

            int pathLen = originalString.IndexOfAny(_QUERY_OR_FRAGMENT_DELIMITER);
            if (pathLen < 0)
            {
                query = fragment = "";
                return originalString;
            }
            else if (originalString[pathLen] == '#')
            {
                query = "";
                fragment = originalString.Substring(pathLen);
            }
            else
            {
                int i = originalString.IndexOf('#', pathLen);
                if (i < 0)
                {
                    fragment = "";
                    query = originalString.Substring(pathLen);
                }
                else
                {
                    query = originalString.Substring(pathLen, i - pathLen);
                    fragment = originalString.Substring(i);
                }
            }

            return originalString.Substring(0, pathLen);
        }

        public static string SplitQueryComponents(this Uri uri, out string scheme, out string userInfo, out string hostAndPort, out string query, out string fragment)
        {
            if (uri is null)
            {
                scheme = userInfo = hostAndPort = query = fragment = null;
                return null;
            }

            if (uri.IsAbsoluteUri)
            {
                scheme = uri.GetComponents(UriComponents.Scheme | UriComponents.KeepDelimiter, UriFormat.UriEscaped);
                userInfo = uri.GetComponents(UriComponents.UserInfo | UriComponents.KeepDelimiter, UriFormat.UriEscaped);
                hostAndPort = uri.GetComponents(UriComponents.Host | UriComponents.Port | UriComponents.KeepDelimiter, UriFormat.UriEscaped);
                query = uri.GetComponents(UriComponents.Query | UriComponents.KeepDelimiter, UriFormat.UriEscaped);
                fragment = uri.GetComponents(UriComponents.Fragment | UriComponents.KeepDelimiter, UriFormat.UriEscaped);
                return uri.GetComponents(UriComponents.Path | UriComponents.KeepDelimiter, UriFormat.UriEscaped);
            }
            scheme = userInfo = hostAndPort = "";
            string originalString = uri.OriginalString;
            if (originalString.Length == 0)
            {
                query = fragment = "";
                return "";
            }
            originalString = EnsureWellFormedUriPath(originalString);

            int pathLen = originalString.IndexOfAny(_QUERY_OR_FRAGMENT_DELIMITER);
            if (pathLen < 0)
            {
                query = fragment = "";
                return originalString;
            }
            else if (originalString[pathLen] == '#')
            {
                query = "";
                fragment = originalString.Substring(pathLen);
            }
            else
            {
                int i = originalString.IndexOf('#', pathLen);
                if (i < 0)
                {
                    fragment = "";
                    query = originalString.Substring(pathLen);
                }
                else
                {
                    query = originalString.Substring(pathLen, i - pathLen);
                    fragment = originalString.Substring(i);
                }
            }

            return originalString.Substring(0, pathLen);
        }

        public static string EnsureWellFormedUriPath(this string path)
        {
            if (string.IsNullOrEmpty(path))
                return "";
            if (Uri.IsWellFormedUriString(path, UriKind.Relative))
                return path;
            if (path.Contains("\\"))
            {
                path = path.Replace("\\", "/");
                if (Uri.IsWellFormedUriString(path, UriKind.Relative))
                    return path;
            }
            return Uri.EscapeUriString(path);
        }

        /// <summary>
        /// Attempts to create a variant of a <seealso cref="Uri"/> with the specified <seealso cref="UriComponents.Path"/> component applied.
        /// </summary>
        /// <param name="uri">The original <seealso cref="Uri"/>.</param>
        /// <param name="newPath">The new <seealso cref="UriComponents.Path"/> component. This can be <c>null</c> to create a <seealso cref="Uri"/> without this component.</param>
        /// <param name="result">The new <seealso cref="Uri"/> with the specified <seealso cref="UriComponents.Path"/> component applied.</param>
        /// <returns><c>true</c> if the new <paramref name="result"/> <seealso cref="Uri"/> was successfully created; otherwise, <c>false</c>.</returns>
        /// <remarks>This can return <c>false</c> in circumstances where the change would create an invalid URI string as it pertains to the current <seealso cref="Uri.Scheme"/>.
        /// If the specified <paramref name="newPath"/> contains a query (<c>?</c>) and/or fragment (<c>#</c>) separator, those components will be applied
        /// to the <paramref name="result"/> <seealso cref="Uri"/> as well.</remarks>
        public static bool TrySetPathComponent(this Uri uri, string newPath, out Uri result)
        {
            if (uri is null)
            {
                result = uri;
                return false;
            }

            string newQuery, newFragment;
            if (string.IsNullOrEmpty(newPath))
                newPath = newQuery = newFragment = "";
            else
            {
                newPath = EnsureWellFormedUriPath(newPath);
                int idx = newPath.IndexOfAny(_QUERY_OR_FRAGMENT_DELIMITER);
                if (idx < 0)
                    newQuery = newFragment = "";
                else
                {
                    if (newPath[idx] == '#')
                    {
                        newQuery = "";
                        newFragment = newPath.Substring(idx);
                    }
                    else
                    {
                        int i = newPath.IndexOf('#', idx);
                        if (i < 0)
                        {
                            newQuery = newPath.Substring(idx);
                            newFragment = "";
                        }
                        else
                        {
                            newQuery = newPath.Substring(idx, i - idx);
                            newFragment = newPath.Substring(i);
                        }
                    }
                    newPath = newPath.Substring(0, idx);
                }
            }
            string oldPath = uri.SplitQueryComponents(out string scheme, out string authority, out string oldQuery, out string oldFragment);
            if (uri.IsAbsoluteUri && newPath.Length > 0 && newPath[0] != '/')
            {
                if (scheme.StartsWith($"{URI_SCHEME_URN}:") && !(scheme.Contains("/") || oldPath.Contains("/")))
                {
                    if (authority.Length > 0)
                        newPath = $":{newPath}";
                    else
                        newPath = $"::{newPath}";
                }
                else if (authority.Length > 0)
                    newPath = $"/{newPath}";
                else
                    newPath = $"//{newPath}";
            }
            if (oldPath.Equals(newPath))
            {
                bool areEqual;
                if (newQuery.Length == 0)
                {
                    if (newFragment.Length > 0 && !oldFragment.Equals(newFragment))
                    {
                        areEqual = false;
                        newQuery = oldQuery;
                    }
                    else
                        areEqual = true;
                }
                else if (oldQuery.Equals(newQuery))
                    areEqual = newFragment.Length == 0 || oldFragment.Equals(newFragment);
                else
                {
                    areEqual = false;
                    if (newFragment.Length == 0)
                        newFragment = oldFragment;
                }

                if (areEqual)
                {
                    result = uri;
                    return true;
                }
            }
            else
            {
                if (newQuery.Length == 0)
                    newQuery = oldQuery;
                if (newFragment.Length == 0)
                    newFragment = oldFragment;
            }
            try
            {
                result = (string.IsNullOrEmpty(scheme)) ? new Uri($"{newPath}{newQuery}{newFragment}", UriKind.Relative)
                    : new Uri($"{scheme}{authority}{newPath}{newQuery}{newFragment}", UriKind.Absolute);
            }
            catch
            {
                result = uri;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Gets the <seealso cref="UriComponents.Query"/> component of a <seealso cref="Uri"/>.
        /// </summary>
        /// <param name="uri">The source <seealso cref="Uri"/>.</param>
        /// <returns>The <seealso cref="UriComponents.Query"/> component of the source <paramref name="uri"/> or <c>null</c> if it contained no query component.
        /// This will also return <c>null</c> if the source <paramref name="uri"/> was <c>null</c>.</returns>
        public static string GetQueryComponent(this Uri uri)
        {
            if (uri is null)
                return null;
            if (uri.IsAbsoluteUri)
                return (uri.Query.Length == 0) ? null : uri.Query.Substring(1);
            string originalString = uri.OriginalString.AsRelativeUriString();
            int index = originalString.IndexOfAny(_QUERY_OR_FRAGMENT_DELIMITER);
            if (index < 0 || originalString[index] == '#')
                return null;
            int fragmentIndex = index + 1;
            if (fragmentIndex < originalString.Length && (fragmentIndex = originalString.IndexOf('#', fragmentIndex)) > index)
                return originalString.Substring(index + 1, fragmentIndex - index);
            return originalString.Substring(index + 1);
        }

        /// <summary>
        /// Attempts to create a variant of a <seealso cref="Uri"/> with the specified <seealso cref="UriComponents.Query"/> component applied.
        /// </summary>
        /// <param name="uri">The original <seealso cref="Uri"/>.</param>
        /// <param name="query">The new <seealso cref="UriComponents.Query"/> component. This can be <c>null</c> to create a <seealso cref="Uri"/> without this component.</param>
        /// <param name="result">The new <seealso cref="Uri"/> with the specified <seealso cref="UriComponents.Query"/> component applied.</param>
        /// <returns><c>true</c> if the new <paramref name="result"/> <seealso cref="Uri"/> was successfully created; otherwise, <c>false</c>.</returns>
        /// <remarks>This can return <c>false</c> in circumstances where the change would create an invalid URI string as it pertains to the current <seealso cref="Uri.Scheme"/>.
        /// If the specified <paramref name="query"/> contains a fragment (<c>#</c>) separator, that component will be applied to the <paramref name="result"/>
        /// <seealso cref="Uri"/> as well.</remarks>
        public static bool TrySetQueryComponent(this Uri uri, string query, out Uri result)
        {
            if (uri is null)
            {
                result = uri;
                return false;
            }

            int index;
            if (uri.IsAbsoluteUri)
            {
                if (query is null)
                {
                    if (uri.Query.Length == 0)
                        result = uri;
                    else
                        try
                        {
                            result = new Uri(uri.GetComponents(BEFORE_QUERY_COMPONENTS | AFTER_QUERY_COMPONENTS, UriFormat.UriEscaped), UriKind.Absolute);
                        }
                        catch
                        {
                            result = uri;
                            return false;
                        }
                    return true;
                }

                query = query.AsRelativeUriString();
                if ((index = query.IndexOf('#')) > -1 && !uri.TrySetFragmentComponent(query.Substring(index + 1), out _))
                {
                    result = uri;
                    return false;
                }
                query = "?" + query.Replace("?", Uri.HexEscape('?'));
                if (uri.Query != query)
                    try
                    {
                        string preceding = uri.GetComponents(BEFORE_QUERY_COMPONENTS, UriFormat.UriEscaped);
                        string following = uri.GetComponents(AFTER_QUERY_COMPONENTS, UriFormat.UriEscaped);
                        result = new Uri($"{preceding}{query}{following}", UriKind.Relative);
                        if (preceding != uri.GetComponents(BEFORE_QUERY_COMPONENTS, UriFormat.UriEscaped) ||
                            following != uri.GetComponents(AFTER_QUERY_COMPONENTS, UriFormat.UriEscaped))
                        {
                            result = uri;
                            return false;
                        }
                    }
                    catch
                    {
                        result = uri;
                        return false;
                    }
                else
                    result = uri;
                return true;
            }

            string originalString = uri.OriginalString;
            int fragmentIndex;
            index = originalString.IndexOfAny(_QUERY_OR_FRAGMENT_DELIMITER);
            if (index < 0)
                index = originalString.Length;
            if (originalString[index] == '#')
                fragmentIndex = index;
            else if (index == originalString.Length - 1 || (fragmentIndex = originalString.IndexOf('#', index + 1)) < 0)
                fragmentIndex = originalString.Length;

            if (query is null)
            {
                if (index == fragmentIndex)
                    result = uri;
                else
                    try
                    {
                        if (index == 0)
                            result = new Uri((fragmentIndex < originalString.Length) ? originalString.Substring(fragmentIndex) : "", UriKind.Relative);
                        else if (fragmentIndex < originalString.Length)
                            result = new Uri(originalString.Substring(0, index) + originalString.Substring(fragmentIndex), UriKind.Relative);
                        else
                            result = new Uri(originalString.Substring(0, index), UriKind.Relative);
                    }
                    catch
                    {
                        result = uri;
                        return false;
                    }
                return true;
            }

            query = $"?{query.AsRelativeUriString()}";
            if (query.Contains("#") || fragmentIndex == originalString.Length)
            {
                if (index == 0)
                {
                    if (query.Equals(originalString))
                        result = uri;
                    else
                        try
                        {
                            result = new Uri(query, UriKind.Relative);
                        }
                        catch
                        {
                            result = uri;
                            return false;
                        }
                }
                else
                {
                    if (query.Length == originalString.Length - index && originalString.Substring(index).Equals(query))
                        result = uri;
                    else
                        try
                        {
                            result = new Uri($"{originalString.Substring(0, index)}{query}", UriKind.Relative);
                        }
                        catch
                        {
                            result = uri;
                            return false;
                        }
                }
            }
            else if (index == 0)
            {
                if (index == query.Length && query.Equals(originalString.Substring(0, index)))
                    result = uri;
                else
                    try
                    {
                        result = new Uri($"{query}{originalString.Substring(fragmentIndex)}", UriKind.Relative);
                    }
                    catch
                    {
                        result = uri;
                        return false;
                    }
            }
            else
            {
                int replaceLen = fragmentIndex - index;
                if (replaceLen == query.Length && query.Equals(originalString.Substring(index, replaceLen)))
                    result = uri;
                else
                    try
                    {
                        result = new Uri($"{originalString.Substring(0, index)}{query}{originalString.Substring(fragmentIndex)}", UriKind.Relative);
                    }
                    catch
                    {
                        result = uri;
                        return false;
                    }
            }
            return true;
        }

        /// <summary>
        /// Gets the <seealso cref="UriComponents.Fragment"/> component of a <seealso cref="Uri"/>.
        /// </summary>
        /// <param name="uri">The source <seealso cref="Uri"/>.</param>
        /// <returns>The <seealso cref="UriComponents.Fragment"/> component of the source <paramref name="uri"/> or <c>null</c> if it contained no fragment component.
        /// This will also return <c>null</c> if the source <paramref name="uri"/> was <c>null</c>.</returns>
        public static string GetFragmentComponent(this Uri uri)
        {
            if (uri is null)
                return null;
            if (uri.IsAbsoluteUri)
                return (uri.Fragment.Length == 0) ? null : uri.Fragment.Substring(1);
            string originalString = uri.OriginalString.AsRelativeUriString();
            int index = originalString.IndexOf('#');
            return (index < 0) ? null : originalString.Substring(index + 1);
        }

        /// <summary>
        /// Attempts to create a variant of a <seealso cref="Uri"/> with the specified <seealso cref="UriComponents.Fragment"/> component applied.
        /// </summary>
        /// <param name="uri">The original <seealso cref="Uri"/>.</param>
        /// <param name="fragment">The new <seealso cref="UriComponents.Fragment"/> component. This can be <c>null</c> to create a <seealso cref="Uri"/> without this component.</param>
        /// <param name="result">The new <seealso cref="Uri"/> with the specified <seealso cref="UriComponents.Fragment"/> component applied.</param>
        /// <returns><c>true</c> if the new <paramref name="result"/> <seealso cref="Uri"/> was successfully created; otherwise, <c>false</c>.</returns>
        /// <remarks>This can return <c>false</c> in circumstances where the change would create an invalid URI string as it pertains to the current <seealso cref="Uri.Scheme"/>.</remarks>
        public static bool TrySetFragmentComponent(this Uri uri, string fragment, out Uri result)
        {
            if (uri is null)
            {
                result = uri;
                return false;
            }

            if (uri.IsAbsoluteUri)
            {
                if (fragment is null)
                {
                    if (uri.Fragment.Length == 0)
                        result = uri;
                    else
                        try
                        {
                            result = new Uri(uri.GetComponents(BEFORE_FRAGMENT_COMPONENTS, UriFormat.UriEscaped), UriKind.Absolute);
                        }
                        catch
                        {
                            result = uri;
                            return false;
                        }
                    return true;
                }

                fragment = fragment.AsRelativeUriString();
                fragment = $"#{fragment}";
                if (uri.Fragment != fragment)
                    try
                    {
                        string preceding = uri.GetComponents(BEFORE_FRAGMENT_COMPONENTS, UriFormat.UriEscaped);
                        result = new Uri($"{preceding}{fragment}", UriKind.Relative);
                        if (preceding != uri.GetComponents(BEFORE_QUERY_COMPONENTS, UriFormat.UriEscaped))
                        {
                            result = uri;
                            return false;
                        }
                    }
                    catch
                    {
                        result = uri;
                        return false;
                    }
                else
                    result = uri;
                return true;
            }


            string originalString = uri.OriginalString;
            int index = originalString.IndexOf('#');
            if (fragment is null)
            {
                if (index < 0)
                    result = uri;
                else
                    try
                    {
                        result = new Uri(originalString.Substring(index + 1), UriKind.Relative);
                    }
                    catch
                    {
                        result = uri;
                        return false;
                    }
                return true;
            }
            if (index < 0)
                try
                {
                    result = new Uri($"{originalString}#{fragment}", UriKind.Relative);
                }
                catch
                {
                    result = uri;
                    return false;
                }
            else if (index == 0)
            {
                if (originalString.Equals(fragment))
                    result = uri;
                else
                    try
                    {
                        result = new Uri($"#{fragment}", UriKind.Relative);
                    }
                    catch
                    {
                        result = uri;
                        return false;
                    }
            }
            else
            {
                if (fragment.Length == originalString.Length - index && fragment.Equals(originalString.Substring(index + 1)))
                    result = uri;
                else
                    try
                    {
                        result = new Uri($"{originalString.Substring(0, index)}#{fragment}", UriKind.Relative);
                    }
                    catch
                    {
                        result = uri;
                        return false;
                    }
            }
            return true;
        }

        /// <summary>
        /// Gets the path segments of a relative or absolute <seealso cref="Uri"/>;
        /// </summary>
        /// <param name="uri">The target <seealso cref="Uri"/>.</param>
        /// <returns>A System.String array that contains the path segments that make up the specified <seealso cref="Uri"/>.</returns>
        public static string[] GetPathSegments(this Uri uri)
        {
            if (uri is null)
                return new string[0];
            if (uri.IsAbsoluteUri)
                return uri.Segments;
            string u = uri.OriginalString;
            if (u.Length == 0)
                return new string[] { u };
            u = EnsureWellFormedUriPath(u);
            return URI_PATH_SEGMENT_REGEX.Matches(u).Cast<Match>().Select(m => m.Value).ToArray();
        }

        /// <summary>
        /// Adds or removes a trailing slash (empty segment) to the <seealso cref="UriComponents.Path"/> component of a <seealso cref="Uri"/>.
        /// </summary>
        /// <param name="uri">The target <seealso cref="Uri"/>.</param>
        /// <param name="shouldHaveTrailingSlash"><c>true</c> if the <seealso cref="UriComponents.Path"/> component of the <paramref name="result"/>
        /// <seealso cref="Uri"/> should have a trailing slash; otherwise <c>false</c> if the trailing slash should be omitted.</param>
        /// <param name="result">A <seealso cref="Uri"/> with the trailing slash included or omitted.</param>
        /// <returns><c>true</c> if the trailing slash was successfully included or omitted; otherwise, <c>false</c>.</returns>
        /// <remarks>This can return <c>false</c> in cases where the change would create an invalid URI as per the current <seealso cref="Uri.Scheme"/>
        /// or if the <seealso cref="Uri.Scheme"/> automatically appends a trailing slash with at it's root path.</remarks>
        public static bool TrySetTrailingEmptyPathSegment(this Uri uri, bool shouldHaveTrailingSlash, out Uri result)
        {
            string path = uri.GetPathComponent();

            if (string.IsNullOrEmpty(path))
            {
                if (shouldHaveTrailingSlash)
                    return uri.TrySetPathComponent("/", out result) && result.GetPathComponent().EndsWith('/');
            }
            else if (path.EndsWith('/') != shouldHaveTrailingSlash)
            {
                if (shouldHaveTrailingSlash)
                    return uri.TrySetPathComponent($"{path}/", out result) && result.GetPathComponent().EndsWith('/');
                int i = path.Length - 1;
                while (i > 0 && path[i - 1] == '/')
                    i--;
                return uri.TrySetPathComponent(path.Substring(0, i), out result) && !result.GetPathComponent().EndsWith('/');
            }
            if (!(uri.IsAbsoluteUri || Uri.IsWellFormedUriString(uri.OriginalString, UriKind.Relative)))
                uri = new Uri(EnsureWellFormedUriPath(uri.OriginalString), UriKind.Relative);
            result = uri;
            return true;
        }
    }
}

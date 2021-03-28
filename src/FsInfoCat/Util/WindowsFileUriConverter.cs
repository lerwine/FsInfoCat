using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace FsInfoCat.Util
{
    /// <summary>
    /// Utility class for converting windows filesystem path strings to and from file URI strings.
    /// </summary>
    /// <remarks>
    /// Templates for regular expressions used by this class:
    /// <list type="bullet">
    /// <item><term>Any Name</term>
    ///     <description>Matches a character sequence which contains only literal and URI-encoded characters which are compatible with typical
    ///         Windows OS file names.</description>
    ///     <code>([^\u0000-\u0019"&lt;&gt;|:;*?\\/%]+|%((?![A-F\d]{2})|2[013-9B-E]|3[\dD]|[57][\dABDEF]|[4689]))+</code></item>
    /// <item><term>Filesystem Name</term>
    ///     <description>Matches a character sequence which contains only literal characters which are compatible with typical Windows OS file names.</description>
    ///     <code>[^\u0000-\u0019"&lt;&gt;|:;*?\\/]+</code></item>
    /// <item><term>Encoded Name</term>
    ///     <description>Matches a character sequence which contains only well-formed URI character sequences which are compatible with typical
    ///         Windows OS file names.</description>
    ///     <code>([!$&amp;-)+-.=@[\]\w]+|%(2[013-9B-E]|3[\dD]|[57][\dABDEF]|[4689][\dA-F]))+</code></item>
    /// </list></remarks>
    public class WindowsFileUriConverter : FileUriConverter
    {
        public const char DIRECTORY_SEPARATOR_CHAR = '\\';
        public const string DIRECTORY_SEPARATOR_STRING = "\\";
        public const string URI_ENCODED_DIRECTORY_SEPARATOR_STRING = "%5C";
        private static readonly char[] FS_SEPARATORS = new char[] { DIRECTORY_SEPARATOR_CHAR, URI_PATH_SEPARATOR_CHAR };

        public static readonly WindowsFileUriConverter INSTANCE = new WindowsFileUriConverter();

        #region Regular Expressions

        /// <summary>
        /// Matches a well-formed relative or absolute local path on the typical Windows filesystem.
        /// </summary>
        /// <remarks>Named group definitions:
        /// <list type="bullet">
        ///     <item><term>host</term> Matches the host name. This implies that the input text is an absolute UNC path
        ///             (<see cref="FileUriConverter.MATCH_GROUP_NAME_HOST"/>).
        ///         <list type="bullet">
        ///             <item><term>ipv4</term> Matches an <seealso cref="UriHostNameType.IPv4"/> host name. This implies that the input text is an absolute UNC path
        ///                  (<see cref="FileUriConverter.MATCH_GROUP_NAME_IPV4"/>).</item>
        ///             <item><term>ipv6</term> Matches an IPV6 host name. This implies that the input text is an absolute UNC path
        ///                     (<see cref="FileUriConverter.MATCH_GROUP_NAME_IPV6"/>).
        ///                 <list type="bullet">
        ///                     <item><term>unc</term> Matches the domain of an IPV6 address.
        ///                         This implies that the input text is an absolute UNC path (<see cref="FileUriConverter.MATCH_GROUP_NAME_UNC"/>).</item>
        ///                 </list>
        ///             </item>
        ///             <item><term>dns</term> Matches a <seealso cref="UriHostNameType.Dns"/> or <seealso cref="UriHostNameType.Basic"/> host name.
        ///                 This implies that the input text is an absolute UNC path (<see cref="FileUriConverter.MATCH_GROUP_NAME_DNS"/>).</item>
        ///         </list>
        ///     </item>
        ///     <item><term>path</term> Matches the path string (<see cref="FileUriConverter.MATCH_GROUP_NAME_PATH"/>).
        ///         <list type="bullet">
        ///             <item><term>root</term> Matches the drive letter. This implies that the input text is an absolute local path
        ///                 (<see cref="FileUriConverter.MATCH_GROUP_NAME_ROOT"/>).</item>
        ///         </list>
        ///     </item>
        /// </list></remarks>
        public static readonly Regex FS_HOST_AND_PATH_REGEX = new Regex(@"
^
(
    (//|\\\\)
    (?<host>
        (?<ipv4>((?<!\d)(0(?=\d))*(?!25[6-9]|([3-9]\d|1\d\d)\d)\d{1,3}\.?){4}(?<!\.))
    |
        (?=\[[a-f\d]*(:[a-f\d]*){2,7}\]|[a-f\d]*(:[a-f\d]*){2,7}$(?=[/:?#]|$)(?=[/:?#]|$)|[a-f\d]*(-[a-f\d]*){2,7}\.ipv6-literal\.net$)
        \[?(?<ipv6>[a-f\d]{1,4}([:-][a-f\d]{1,4}){7}|(([a-f\d]{1,4}[:-])+|[:-])([:-][a-f\d]{1,4})+|::)(\]|(?<unc>\.ipv6-literal\.net))?
    |
        (?=[\w-.]{1,255}(?![\w-.]))
        (?<dns>[a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?)
    )
    (?<path>
        [/\\]?(?=$)
    |
        (
            [/\\]
            [^\u0000-\u0019""<>|:;*?\\/]+
        )*
    )
|
    (?<path>
        (
            (?<root>[a-z]):
        |
            [^\u0000-\u0019""<>|:;*?\\/]+
        )?
        (
            [/\\]
            [^\u0000-\u0019""<>|:;*?\\/]+
        )*
    )
)
[\\/]?$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

        /// <summary>
        /// Matches a well-formed relative or absolute local path on the typical Windows filesystem.
        /// </summary>
        /// <remarks>Named group definitions:
        /// <list type="bullet">
        ///     <item><term>root</term> Matches the drive letter. This implies that the input text is an absolute local path
        ///         (<see cref="FileUriConverter.MATCH_GROUP_NAME_ROOT"/>).</item>
        /// </list></remarks>
        public static readonly Regex FS_LOCAL_PATH_REGEX = new Regex(@"
^
(
    (?<root>[a-z]):
    (
        (
            [/\\]
            [^\u0000-\u0019""<>|:;*?\\/]+
        )+
    |
        [/\\]?(?=$)
    )
|
    [^\u0000-\u0019""<>|:;*?\\/]*
    (
        [/\\]
        [^\u0000-\u0019""<>|:;*?\\/]+
    )*
)
(?=[\\/]?$)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

        /// <summary>
        /// Matches consecutive and trailing filesystem path separators, allowing up to 2 consecutive path separator characters at the beginning of the string.
        /// </summary>
        /// <remarks>This will also match surrounding whitespace and relative self-reference sequences (<c>\.\<c>). This does not match parent segment
        /// references (<c>\..\</c>) if they are not at the beginning of the string.</remarks>
        public static readonly Regex FS_FULL_PATH_NORMALIZE_REGEX = new Regex(@"
^
(\s*(\.\.?[/\\]+)+|\s+)
|
(?<=.[\\/])[/\\]+
|
[/\\]\.(?=[/\\]|$)
|
([/\\]+\s*|\s+)
$", RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

        /// <summary>
        /// Matches consecutive URI path separators as well as leading and trailing whitespace and path separators.
        /// </summary>
        /// <remarks>This will also match relative self-reference sequences (<c>\.\<c>). This does not match parent segment references (<c>\..\</c>)
        /// if they are not at the beginning of the string.</remarks>
        public static readonly Regex FS_RELATIVE_PATH_NORMALIZE_REGEX = new Regex(@"
^(\s*(\.\.?[/\\]+)+|\s+)
|
(?<!^\s*[/\\])[/\\](?=[/\\])
|
[/\\]\.(?=[/\\]|$)
|
((?<!^\s*)[/\\]+\s*|\s+)$", RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

        /// <summary>
        /// Matches character sequences which need to be encoded within a URI string.
        /// </summary>
        /// <remarks>This is intended to be used on a string that has not been URI-encoded.</remarks>
        public static readonly Regex URI_ENCODING_REQUIRED_REGEX = new Regex(@"[^!$=&-.:;=@[\]\w/]+", RegexOptions.Compiled);

        /// <summary>
        /// Regular expression pattern that matches a string that can be parsed as an absolute <seealso cref="Uri.UriSchemeFile">file</seealso> URI or an absolute
        /// filesystem path, and does not contain any characters which are not compatible with a typical Windows filesystem.
        /// </summary>
        /// <remarks>Only the host name needs to be well-formed to match this expression.</remarks>
        public const string PATTERN_ABSOLUTE_FILESYSTEM_OR_LAX_FILE_URI = @"^\s*((file:)?//((?i)^\s*((file:)?//((?i)((?<!\d)(0(?=\d))*(?!25[6-9]|([3-9]\d|1\d\d)\d)\d{1,3}\.?){4}(?<!\.)" +
            @"|(?=\[[a-f\d]*(:[a-f\d]*){2,7}\]|[a-f\d]*(:[a-f\d]*){2,7}(?=[/:?#]|$)|[a-f\d]*(-[a-f\d]*){2,7}\.ipv6-literal\.net$)" +
            @"\[?([a-f\d]{1,4}([:-][a-f\d]{1,4}){7}|(([a-f\d]{1,4}[:-])+|[:-])([:-][a-f\d]{1,4})+|::)(\]|\.ipv6-literal\.net)?" +
            @"|(?=[\w-.]{1,255}(?![\w-.]))[a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?)(?=/|\s*$)|(file://)?(?=/))" +
            @"([\\/]([^\u0000-\u0019\u007f-\u00a0\u1680\u2028\u2029\ud800-\udfff/"" <>\\/|]+|%([4689A-F][\dA-F]|2[^2AF]|3[^ACEF]|[57][^C]|(?![\dA-F]{2})))+)*[\\/]?\s*$";

        /// <summary>
        /// Matches a string that can be parsed as an absolute <seealso cref="Uri.UriSchemeFile">file</seealso> URI or an absolute filesystem path, and does not contain
        /// any characters which are not compatible with the Linux filesystem.
        /// </summary>
        /// <remarks>Only the host name needs to be well-formed to match this expression.
        /// <para>Named group definitions:
        /// <list type="bullet">
        ///     <item><term>file</term> Matches the <seealso cref="Uri.UriSchemeFile">file</seealso> URI scheme name
        ///         (<see cref="FileUriConverter.MATCH_GROUP_NAME_FILE"/>).</item>
        ///     <item><term>host</term> Matches the host name. This implies that the input text is an absolute UNC path
        ///             (<see cref="FileUriConverter.MATCH_GROUP_NAME_HOST"/>).
        ///         <list type="bullet">
        ///             <item><term>ipv4</term> Matches an <seealso cref="UriHostNameType.IPv4"/> host name. This implies that the input text is an absolute UNC path
        ///                  (<see cref="FileUriConverter.MATCH_GROUP_NAME_IPV4"/>).</item>
        ///             <item><term>ipv6</term> Matches an IPV6 host name. This implies that the input text is an absolute UNC path
        ///                     (<see cref="FileUriConverter.MATCH_GROUP_NAME_IPV6"/>).
        ///                 <list type="bullet">
        ///                     <item><term>unc</term> Matches the domain of an IPV6 address.
        ///                         This implies that the input text is an absolute UNC path (<see cref="FileUriConverter.MATCH_GROUP_NAME_UNC"/>).</item>
        ///                 </list>
        ///             </item>
        ///             <item><term>dns</term> Matches a <seealso cref="UriHostNameType.Dns"/> or <seealso cref="UriHostNameType.Basic"/> host name.
        ///                 This implies that the input text is an absolute UNC path (<see cref="FileUriConverter.MATCH_GROUP_NAME_DNS"/>).</item>
        ///         </list>
        ///     </item>
        ///     <item><term>path</term> Matches the path string (<see cref="FileUriConverter.MATCH_GROUP_NAME_PATH"/>).
        ///         <list type="bullet">
        ///             <item><term>dir</term> Matches the parent directory path. This group will always succeed when the expression succeeds,
        ///                 even if it is empty. The trailing slash will be omitted unless it is the root path
        ///                 (<see cref="FileUriConverter.MATCH_GROUP_NAME_DIR"/>).</item>
        ///             <item><term>root</term> Matches leading directory separator character. This implies that the input text is an absolute local path
        ///                 (<see cref="FileUriConverter.MATCH_GROUP_NAME_ROOT"/>).</item>
        ///             <item><term>fileName</term> Matches the file name. This group will only fail if the source path is the root path
        ///                 (<see cref="FileUriConverter.MATCH_GROUP_NAME_FILE_NAME"/>).</item>
        ///         </list>
        ///     </item>
        /// </list></para></remarks>
        public static readonly Regex ABSOLUTE_FILESYSTEM_OR_LAX_FILE_URI_REGEX = new Regex(@"
^
(
    ((?<file>file)://|[\\/]{2})
    (?<host>
        (?i)
        (?<ipv4>((?<!\d)(0(?=\d))*(?!25[6-9]|([3-9]\d|1\d\d)\d)\d{1,3}\.?){4}(?<!\.))
    |
        (?=\[[a-f\d]*(:[a-f\d]*){2,7}\]|[a-f\d]*(:[a-f\d]*){2,7}(?=[/:?#]|$)|[a-f\d]*(-[a-f\d]*){2,7}\.ipv6-literal\.net$)
        \[?(?<ipv6>[a-f\d]{1,4}([:-][a-f\d]{1,4}){7}|(([a-f\d]{1,4}[:-])+|[:-])([:-][a-f\d]{1,4})+|::)(\]|(?<unc>\.ipv6-literal\.net))?
    |
        (?=[\w-.]{1,255}(?![\w-.]))
        (?<dns>[a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?)
    )
|
    ((?<file>file):///)?(?=[a-zA-Z]:)
)
(?<path>
    ([a-zA-Z]:)?
    (
        [\\/](?=$)
    |
        (
            [\\/]
            ([^\u0000-\u0019""<>|:;*?\\/%]+|%((?![A-F\d]{2})|2[013-9B-E]|3[\dD]|[57][\dABDEF]|[4689]))+
        )*
    )
)
[\\/]?$", RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

        /// <summary>
        /// Pattern which indicates the probable format of a string. This is not intended to be used for relative path types.
        /// </summary>
        /// <remarks>This expression is intended to determine the most probable string format (ie. URI string, UNC path, etc) of the input string.
        /// This will fail to match any character sequences that clearly do not represent an absolute path reference on a Windows system.
        /// <para>Named group definitions:
        /// <list type="bullet">
        ///     <item><term>file</term> Matches the <seealso cref="Uri.UriSchemeFile">file</seealso> URI scheme name
        ///         (<see cref="FileUriConverter.MATCH_GROUP_NAME_FILE"/>).</item>
        ///     <item><term>host</term> Matches the host name. This implies that the input text is an absolute UNC path
        ///             (<see cref="FileUriConverter.MATCH_GROUP_NAME_HOST"/>).</item>
        ///     <item><term>unc</term> Matches the UNC path string. This implies that the input text is an absolute UNC path
        ///             (<see cref="FileUriConverter.MATCH_GROUP_NAME_UNC"/>).</item>
        ///     <item><term>path</term> Matches the path string (<see cref="FileUriConverter.MATCH_GROUP_NAME_PATH"/>).</item>
        ///     <item><term>scheme</term> Matches non-file URI scheme (<see cref="FileUriConverter.MATCH_GROUP_NAME_SCHEME"/>).</item>
        /// </list></para></remarks>
        public static readonly Regex FORMAT_DETECTION_REGEX = new Regex(@"
^
(
    (?<file>
        (?i)
        (?<scheme>file):[\\/]{2}
        (
            (?<host>[^\\/]+)
            (?<path>[\\/].*)?
        |
            [\\/](?<path>[a-z]:([\\/].*)?)
        )
    )
|
    (?<unc>
        [\\/]{2}
        (?<host>[^\\/]+)
        (?<path>([\\/](?![\\/]).*)?)
    )
|
    (?<path>[a-z]:([\\/].*)?)
|
    (?!file:|((?i)FILE:/))
    (?<scheme>[a-zA-Z][\w-.]+):
    (//?)?
    (?<host>
        (
            [^?#/@:]*
            (:[^?#/@:]*)?
            @
        )?
        [^?#/@:]+
        (:\d+)?
    )?
    (?<path>([/:].*)?)
)$", RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

        /// <summary>
        /// Matches a well-formed URI that can be converted to a valid absolute or relative local path on a typical Windows filesystem.
        /// </summary>
        /// <remarks>Named group definitions:
        /// <list type="bullet">
        ///     <item><term>file</term> Matches the <seealso cref="Uri.UriSchemeFile">file</seealso> uri scheme and URI authority.
        ///         (<see cref="FileUriConverter.MATCH_GROUP_NAME_SCHEME"/>).</item>
        ///     <item><term>host</term> Matches the host name. This implies that the input text is an absolute UNC path
        ///             (<see cref="FileUriConverter.MATCH_GROUP_NAME_HOST"/>).
        ///         <list type="bullet">
        ///             <item><term>ipv4</term> Matches an <seealso cref="UriHostNameType.IPv4"/> host name. This implies that the input text is an absolute UNC path
        ///                  (<see cref="FileUriConverter.MATCH_GROUP_NAME_IPV4"/>).</item>
        ///             <item><term>ipv6</term> Matches an IPV6 host name. This implies that the input text is an absolute UNC path
        ///                     (<see cref="FileUriConverter.MATCH_GROUP_NAME_IPV6"/>).</item>
        ///             <item><term>dns</term> Matches a <seealso cref="UriHostNameType.Dns"/> or <seealso cref="UriHostNameType.Basic"/> host name.
        ///                 This implies that the input text is an absolute UNC path (<see cref="FileUriConverter.MATCH_GROUP_NAME_DNS"/>).</item>
        ///         </list>
        ///     </item>
        ///     <item><term>path</term> Matches the path string (<see cref="FileUriConverter.MATCH_GROUP_NAME_PATH"/>).
        ///         <list type="bullet">
        ///             <item><term>root</term> Matches drive letter. This implies that the input text is
        ///                 an absolute local path (<see cref="FileUriConverter.MATCH_GROUP_NAME_ROOT"/>).</item>
        ///         </list>
        ///     </item>
        /// </list></remarks>
        public static readonly Regex URI_HOST_AND_PATH_STRICT_REGEX = new Regex(@"
^
(
    (?![^:/]*:)
|
    (?<file>
        file://
        (
            (?i)
            (?<host>
                (?<ipv4>((?<!\d)(0(?=\d))*(?!25[6-9]|([3-9]\d|1\d\d)\d)\d{1,3}\.?){4}(?<!\.))
            |
                (?=\[[a-f\d]*(:[a-f\d]*){2,7}\]|[a-f\d]*(:[a-f\d]*){2,7}(?=[/:?#]|$))\[?(?<ipv6>[a-f\d]{1,4}(:[a-f\d]{1,4}){7}|(([a-f\d]{1,4}:)+|:)(:[a-f\d]{1,4})+|::)\]?
            |
                (?=[\w-.]{1,255}(?![\w-.]))
                (?<dns>[a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?)
            )
            (?=/|$)
            |
            /(?=[a-z]:)
        )
    )
)
(?<path>
    (
        (?<root>[a-zA-Z]):(/|(?=$))
    |
        /
    |
        (?![a-zA-Z]:)
    )
    (
        ([!$&-)+-.=@[\]\w]+|%(2[013-9B-E]|3[\dD]|[57][\dABDEF]|[4689][\dA-F]))+
        (
            /
            ([!$&-)+-.=@[\]\w]+|%(2[013-9B-E]|3[\dD]|[57][\dABDEF]|[4689][\dA-F]))+
        )*
    )?
)
(?=/?$)", RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

        /// <summary>
        /// Matches a well-formed URI that can be converted to a valid absolute or relative local path on a typical Windows filesystem.
        /// </summary>
        /// <remarks>Named group definitions:
        /// <list type="bullet">
        ///     <item><term>dir</term> Matches the parent directory path. This group will always succeed when the expression succeeds,
        ///         even if it is empty. The trailing slash will be omitted unless it is the root path
        ///         (<see cref="FileUriConverter.MATCH_GROUP_NAME_DIR"/>).</item>
        ///     <item><term>fileName</term> Matches the file name. This group will only fail if the source path is the root path
        ///         (<see cref="FileUriConverter.MATCH_GROUP_NAME_FILE_NAME"/>).</item>
        /// </list></remarks>
        public static readonly Regex URI_DIR_AND_FILE_STRICT_REGEX = new Regex(@"
^
(?<dir>
    (
        [a-zA-Z]:(?=/|$)
    |
        /?
        (?![a-zA-Z]:)
        (
            (?=[^/]+/[^/])
            ([!$&-)+-.=@[\]\w]+|%(2[013-9B-E]|3[\dD]|[57][\dABDEF]|[4689][\dA-F]))+
        )?
    )
    (
        /(?=([^/]+/?)?$)
        |
        (
            (?=/[^/]+/[^/])
            /
            ([!$&-)+-.=@[\]\w]+|%(2[013-9B-E]|3[\dD]|[57][\dABDEF]|[4689][\dA-F]))+
        )*
    )?
)
(
    /?
    (?<fileName>([!$&-)+-.=@[\]\w]+|%(2[013-9B-E]|3[\dD]|[57][\dABDEF]|[4689][\dA-F]))+)
)?
(?=/?$)", RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

        /// <summary>
        /// Matches a URI that can be converted to a valid absolute or relative local path on a typical Windows filesystem.
        /// </summary>
        /// <remarks>Named group match definition:
        /// <list type="bullet">
        ///     <item><term>root</term> Matches the host name or drive path that indicates it is an absolute path.</item>
        /// </list></remarks>
        public static readonly Regex URI_VALIDATION_REGEX = new Regex(@"
(?<=^\s*)
(
    file://
    (?<root>
        ((?<!\d)(0(?=\d))*(?!25[6-9]|([3-9]\d|1\d\d)\d)\d{1,3}\.?){4}(?<!\.)
    |
        (?=\[[a-f\d]*(:[a-f\d]*){2,7}\]|[a-f\d]*(:[a-f\d]*){2,7}$(?=[/:?#]|$)(?=[/:?#]|$))\[?([a-f\d]{1,4}(:[a-f\d]{1,4}){7}|(([a-f\d]{1,4}:)+|:)(:[a-f\d]{1,4})+|::)\]?
    |
        (?=[\w-.]{1,255}(?![\w-.]))
        [a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?
    |
        /*[a-z]:
    )
    (/+|(?=\s*$))
|
    (?!file:)
)
([^\u0000-\u0019""<>|:;*?\\%]+|%((?![A-F\d]{2})|2[013-9B-E]|3[\dD]|[57][\dABDEF]|[4689]))*
(?=/*\s*$)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

        #endregion

        public override char LocalDirectorySeparatorChar => DIRECTORY_SEPARATOR_CHAR;

        public override string FsDisplayName => "Windows";

        public override PlatformType FsPlatform => PlatformType.Windows;

        public override Regex UriHostAndPathStrictRegex => URI_HOST_AND_PATH_STRICT_REGEX;

        public override Regex UriValidationRegex => URI_VALIDATION_REGEX;

        public override Regex FsHostAndPathRegex => FS_HOST_AND_PATH_REGEX;

        public override Regex FsFullPathNormalizeRegex => FS_FULL_PATH_NORMALIZE_REGEX;

        private WindowsFileUriConverter() { }

        public override string SplitFsPathLeaf(string fsPath, out string leafSegment)
        {
            if (string.IsNullOrEmpty(fsPath))
            {
                leafSegment = "";
                return "";
            }
            int e = fsPath.Length - 1;
            int i = fsPath.LastIndexOfAny(FS_SEPARATORS);
            while (i == e)
            {
                if (i == 0)
                    break;
                fsPath = fsPath.Substring(0, i);
                e--;
                i = fsPath.LastIndexOfAny(FS_SEPARATORS);
            }
            if (i++ < 0)
            {
                leafSegment = fsPath;
                return "";
            }
            leafSegment = fsPath.Substring(i);
            return fsPath.Substring(0, i);
        }

        /// <summary>
        /// Converts a URI-compatible host name and URI-encoded path string to a filesystem path string.
        /// </summary>
        /// <param name="host">The URI-compatible host name or <see langword="null"/> or empty if <paramref name="uriEncodedPath"/> is to be converted
        /// to a filesystem-local path.</param>
        /// <param name="uriEncodedPath">The URI-encoded relative path string.</param>
        /// <returns>A filesystem path string.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="host"/> and/or <paramref name="uriEncodedPath"/> is invalid.</exception>
        public override string ToFileSystemPath(string hostName, string uriEncodedPath)
        {
            Match match;
            if (string.IsNullOrEmpty(uriEncodedPath) || (uriEncodedPath = NormalizeRelativeFileUri(uriEncodedPath)).Length == 0)
            {
                if (string.IsNullOrEmpty(hostName))
                    return "";
                if (IPV6_ADDRESS_REGEX.IsMatch(hostName))
                    return $"\\\\{hostName.Replace(":", "-")}{FQDN_IPV6_UNC}";
                if (BASIC_DNS_OR_IPV4_NAME_REGEX.IsMatch(hostName))
                    return $"\\\\{hostName}";
                throw new ArgumentOutOfRangeException(nameof(hostName));
            }
            if ((match = URI_DIR_AND_FILE_STRICT_REGEX.Match(uriEncodedPath)).Success)
            {
                string fsPath = Uri.UnescapeDataString(match.Value).Replace(URI_PATH_SEPARATOR_CHAR, DIRECTORY_SEPARATOR_CHAR);
                match = FS_LOCAL_PATH_REGEX.Match(fsPath);
                if (match.Success)
                {
                    if (string.IsNullOrEmpty(hostName))
                        return match.Value;
                    if (IPV6_ADDRESS_REGEX.IsMatch(hostName))
                        return match.Groups[MATCH_GROUP_NAME_ROOT].Success ? $"\\\\{hostName.Replace(":", "-")}{FQDN_IPV6_UNC}{fsPath}" :
                            $"\\\\{hostName.Replace(":", "-")}{FQDN_IPV6_UNC}\\{fsPath}";
                    if (BASIC_DNS_OR_IPV4_NAME_REGEX.IsMatch(hostName))
                        return match.Groups[MATCH_GROUP_NAME_ROOT].Success ? $"\\\\{hostName}{fsPath}" : $"\\\\{hostName}\\{fsPath}";
                    throw new ArgumentOutOfRangeException(nameof(hostName));
                }
            }
            throw new ArgumentOutOfRangeException(nameof(uriEncodedPath));
        }

        /// <summary>
        /// Converts a <seealso cref="Uri.UriSchemeFile">file</seealso> URL to a filesystem path string.
        /// </summary>
        /// <param name="fileUriString">An absolute <seealso cref="Uri.UriSchemeFile">file</seealso> URL string or a relative URI-encoded path string.</param>
        /// <returns>A filesystem path string.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="fileUriString"/> is invalid.</exception>
        public override string ToFileSystemPath(string fileUriString)
        {
            if (string.IsNullOrEmpty(fileUriString))
                return "";
            if (TrySplitFileUriString(fileUriString, out string hostName, out string path, out _))
                return ToFileSystemPath(hostName, path);
            return ToFileSystemPath("", fileUriString);
        }

        /// <summary>
        /// Converts a local filesystem path string to an URI encoded file name, relative directory path string, and host name.
        /// </summary>
        /// <param name="path">The local filesystem path string to convert.</param>
        /// <param name="hostName">Returns the host name that was referenced within the file system <paramref name="path"/>
        /// or <seealso cref="string.Empty"/> if it did not reference a host name.</param>
        /// <param name="directoryName">Returns the URI-encoded relative directory path string. This will never end with a path separator (<c>/</c>)
        /// unless it represents the root subdirectory.</param>
        /// <returns>The URI-encoded file name (leaf) portion of the path string or <seealso cref="string.Empty"/> if the file system <paramref name="path"/>
        /// referenced the root subdirectory.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="path"/> is invalid.</exception>
        public override string  FromFileSystemPath(string path, out string hostName, out string directoryName)
        {
            if (string.IsNullOrEmpty(path))
            {
                hostName = directoryName = "";
                return "";
            }

            Match match = FS_HOST_AND_PATH_REGEX.Match(path);
            if (match.Success)
            {
                hostName = (match.Groups[MATCH_GROUP_NAME_UNC].Success) ?
                    match.GetGroupValue(MATCH_GROUP_NAME_IPV6, "").Replace('-', URI_SCHEME_SEPARATOR_CHAR) :
                    match.GetGroupValue(MATCH_GROUP_NAME_HOST, "");
                //if ((directoryName = match.GetGroupValue(MATCH_GROUP_NAME_DIR, "")).Length > 0)
                //    directoryName = EscapeSpecialPathChars(Uri.EscapeUriString(directoryName.Contains(DIRECTORY_SEPARATOR_CHAR) ?
                //        directoryName.Replace(DIRECTORY_SEPARATOR_CHAR, URI_PATH_SEPARATOR_CHAR) : directoryName));
                //string fileName = match.GetGroupValue(MATCH_GROUP_NAME_FILE_NAME, "");
                path = match.GetGroupValue(MATCH_GROUP_NAME_PATH);
                directoryName = SplitFsPathLeaf(path, out string fileName).Replace(DIRECTORY_SEPARATOR_CHAR, URI_PATH_SEPARATOR_CHAR);
                directoryName = EscapeSpecialPathChars(Uri.EscapeUriString((directoryName.Length > 1 && directoryName.EndsWith(URI_PATH_SEPARATOR_CHAR)) ?
                    directoryName.Substring(0, directoryName.Length - 1) : directoryName));
                return (fileName.Length > 0) ? EscapeSpecialPathChars(Uri.EscapeUriString(fileName)) : fileName;
            }
            throw new ArgumentOutOfRangeException(nameof(path));
        }

        /// <summary>
        /// Converts a local filesystem path string to an URI encoded relative path string and host name.
        /// </summary>
        /// <param name="path">The local filesystem path string to convert.</param>
        /// <param name="hostName">Returns the URI-compatible host name that was referenced within the file system <paramref name="path"/>
        /// or <seealso cref="string.Empty"/> if it did not reference a host name.</param>
        /// <returns>The URI-encoded relative path string. This will never end with a path separator (<c>/</c>) unless it represents the root subdirectory.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="path"/> is invalid.</exception>
        public override string FromFileSystemPath(string path, out string hostName)
        {
            if (string.IsNullOrEmpty(path))
            {
                hostName = "";
                return "";
            }

            Match match = FS_HOST_AND_PATH_REGEX.Match(path);
            if (match.Success)
            {
                hostName = (match.Groups[MATCH_GROUP_NAME_UNC].Success) ?
                    match.GetGroupValue(MATCH_GROUP_NAME_IPV6, "").Replace('-', URI_SCHEME_SEPARATOR_CHAR) :
                    match.GetGroupValue(MATCH_GROUP_NAME_HOST, "");
                return ((path = match.GetGroupValue(MATCH_GROUP_NAME_PATH, "")).Length > 0) ?
                    EscapeSpecialPathChars(Uri.EscapeUriString(path.Contains(DIRECTORY_SEPARATOR_CHAR) ?
                        path.Replace(DIRECTORY_SEPARATOR_CHAR, URI_PATH_SEPARATOR_CHAR) : path)) : path;
            }
            throw new ArgumentOutOfRangeException(nameof(path));
        }

        /// <summary>
        /// Converts a local filesystem path string to an absolute <seealso cref="Uri.UriSchemeFile">file</seealso> URL or a URI encoded relative path string.
        /// </summary>
        /// <param name="path">The local filesystem path string to convert.</param>
        /// <returns>An absolute <seealso cref="Uri.UriSchemeFile">file</seealso> URL or a URI encoded relative path string.
        /// This will never end with a path separator (<c>/</c>) unless it represents the root subdirectory.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="path"/> is invalid.</exception>
        public override string FromFileSystemPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return "";

            Match match = FS_HOST_AND_PATH_REGEX.Match(path);
            if (match.Success)
            {
                if ((path = match.GetGroupValue(MATCH_GROUP_NAME_PATH, "")).Length > 0)
                    path = EscapeSpecialPathChars(Uri.EscapeUriString(path.Contains(DIRECTORY_SEPARATOR_CHAR) ?
                        path.Replace(DIRECTORY_SEPARATOR_CHAR, URI_PATH_SEPARATOR_CHAR) : path));
                string hostName;
                if (match.Groups[MATCH_GROUP_NAME_IPV6].Success)
                    hostName = (match.Groups[MATCH_GROUP_NAME_UNC].Success) ?
                        match.GetGroupValue(MATCH_GROUP_NAME_IPV6, "").Replace('-', URI_SCHEME_SEPARATOR_CHAR) :
                        match.GetGroupValue(MATCH_GROUP_NAME_IPV6, "");
                else if (match.Groups[MATCH_GROUP_NAME_HOST].Success)
                    hostName = match.GetGroupValue(MATCH_GROUP_NAME_HOST, "");
                else if (match.Groups[MATCH_GROUP_NAME_ROOT].Success)
                    hostName = "";
                else
                    return path;
                return (path.Length > 0 && path[0] == URI_PATH_SEPARATOR_CHAR) ? $"file://{hostName}{path}" : $"file://{hostName}/{path}";
            }
            throw new ArgumentOutOfRangeException(nameof(path));
        }

        /// <summary>
        /// Attempt to create a <seealso cref="FileUri"/> object from an absolute filesystem path string.
        /// </summary>
        /// <param name="fileSystemPath">The abolute filesystem path string.</param>
        /// <param name="fileUri">Returns the constructed <seealso cref="FileUri"/> object or <see langword="null"/> if the <paramref name="inputString"/>
        /// could not be parsed.</param>
        /// <returns><see langword="true"/> if the <paramref name="fileSystemPath"/> could be parsed as a filesystem path string;
        /// otherwise, <see langword="false"/>.</returns>
        public override bool TryCreateFileUriFromFsPath(string fileSystemPath, out FileUri fileUri)
        {
            Match match;
            if (!string.IsNullOrEmpty(fileSystemPath) && (match = FS_HOST_AND_PATH_REGEX.Match(fileSystemPath)).Success)
            {
                if (match.Groups[MATCH_GROUP_NAME_HOST].Success)
                {
                    string hostName = (match.Groups[MATCH_GROUP_NAME_UNC].Success) ?
                        match.Groups[MATCH_GROUP_NAME_IPV6].Value.Replace("-", URI_SCHEME_SEPARATOR_STRING) :
                        match.Groups[MATCH_GROUP_NAME_HOST].Value;
                    string path = EscapeSpecialPathChars(Uri.EscapeUriString(match.Groups[MATCH_GROUP_NAME_PATH].Value
                        .Replace(DIRECTORY_SEPARATOR_CHAR, URI_PATH_SEPARATOR_CHAR)));
                    fileUri = new FileUri((path.Length > 0 && path[0] != URI_PATH_SEPARATOR_CHAR) ? $"file://{hostName}/{path}" : $"file://{hostName}{path}");
                    return true;
                }
                if (match.Groups[MATCH_GROUP_NAME_ROOT].Success)
                {
                    string path = EscapeSpecialPathChars(Uri.EscapeUriString(match.Groups[MATCH_GROUP_NAME_PATH].Value
                        .Replace(DIRECTORY_SEPARATOR_CHAR, URI_PATH_SEPARATOR_CHAR)));
                    fileUri = new FileUri((path.Length > 0 && path[0] != URI_PATH_SEPARATOR_CHAR) ? $"file:///{path}" : $"file://{path}");
                    return true;
                }
            }
            fileUri = null;
            return false;
        }

        public override string EscapeSpecialPathChars(string uriString, out bool wasChanged)
        {
            if (uriString is null)
            {
                wasChanged = true;
                return "";
            }
            wasChanged = uriString.Length > 0 && uriString.Contains(URI_FRAGMENT_DELIMITER_CHAR);
            return wasChanged ? uriString.Replace(URI_FRAGMENT_DELIMITER_STRING, URI_FRAGMENT_DELIMITER_ESCAPED) : uriString;
        }

        /// <summary>
        /// Converts <seealso cref="FileUriConverter.URI_FRAGMENT_DELIMITER_CHAR">#</seealso> characters to its URI-encoded form so the target string value
        /// can be successfully parsed as a <seealso cref="Uri.UriSchemeFile">file</seealso> URI string.
        /// </summary>
        /// <param name="uriString">The URI-encoded path string. If a <see langword="null"/> value is provided, an empty string is returned.</param>
        /// <returns>The URI-encoded path string with special path characters encoded as well.</returns>
        /// <exception cref="UriFormatException"><paramref name="uriString"/> contains invalid unicode character sequence(s).</exception>
        public override string EscapeSpecialPathChars(string uriString) =>
            string.IsNullOrEmpty(uriString) ? "" : (uriString.Contains(URI_FRAGMENT_DELIMITER_CHAR) ?
            uriString.Replace(URI_FRAGMENT_DELIMITER_STRING, URI_FRAGMENT_DELIMITER_ESCAPED) : uriString);

        /// <summary>
        /// Indicates whether a URI-encoded path string contains characters which are acceptable for the current OS type but should be URI-encoded.
        /// </summary>
        /// <param name="uriString">The URI-encoded path string.</param>
        /// <returns><see langword="true"/> if the <paramref name="uriString"/> contains one or more characters which need to be URI encoded,
        /// but would not get encoded using <seealso cref="Uri.EscapeUriString(string)"/>; otherwise, <see langword="false"/>.</returns>
        public override bool ContainsSpecialPathChars(string uriString) => !string.IsNullOrEmpty(uriString) && uriString.Contains(URI_FRAGMENT_DELIMITER_CHAR);
    }
}

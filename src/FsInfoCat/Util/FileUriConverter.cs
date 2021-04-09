using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace FsInfoCat.Util
{
    /// <summary>
    /// Utility base class for filesystem-specific file URI conversion.
    /// </summary>
    /// <remarks>
    /// Templates for regular expressions used by this class:
    /// <list type="bullet">
    /// <item><term>Unsupported Whitespace Ranges</term>
    ///     <description>Matches control character and whitespace characters which are either not compatible with any filesystem or are not supported
    ///         for using with form fields.
    ///         <code>[\u0000-\u0019\u007f-\u00a0\u1680\u2028\u2029\ud800-\udfff]</code></description></item>
    /// <item><term>Basic and DNS Host Name</term>
    ///     <description>Matches a valid basic host name or DNS host name.
    ///         <code>(?i)(?=[\w-.]{1,255}(?![\w-.]))[a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?</code></description></item>
    /// <item><term>IPV4 Address</term>
    ///     <description>Matches a valid IPv4 internet address.
    ///         <code>((?<!\d)(0(?=\d))*(?!25[6-9]|([3-9]\d|1\d\d)\d)\d{1,3}\.?){4}(?<!\.)</code></description></item>
    /// <item><term>IPV6 Address</term>
    ///     <description>Matches a valid IPv6 internet address. The <c>ipv6</c> group matches the address string without surrounding brackets that might be present.
    ///         <code>(?=\[[a-f\d]*(:[a-f\d]*){2,7}\]|[a-f\d]*(:[a-f\d]*){2,7}(?=[/:?#]|$)(?=[/:?#]|$))\[?(?&lt;ipv6&gt;[a-f\d]{1,4}(:[a-f\d]{1,4}){7}|(([a-f\d]{1,4}:)+|:)(:[a-f\d]{1,4})+|::)\]?</code></description></item>
    /// <item><term>IPV6 Address for UNC Host Name</term>
    ///     <description>Matches a valid IPv6 internet address, including the format that is used in the UNC path format. The <c>ipv6</c> group matches the
    ///         address string without surrounding brackets that might be present. The <c>unc</c> group matches the <c>.ipv6-literal.net</c> domain which is
    ///         used for the UNC path format.
    ///         <code>(?=(\[[^]]+\]|[a-f\d:]+|[a-f\d-]+\.ipv6-literal\.net)$)\[?(?<ipv6>([a-f\d]{1,4}:){7}(:|[a-f\d]{1,4})|(?=(\w*:){2,7}\w*\]?$)(([a-f\d]{1,4}:)+|:):([a-f\d]{1,4}(:[a-f\d]{1,4})*)?)(\]|\.ipv6-literal\.net)?</code></description></item>
    /// </list></remarks>
    public abstract class FileUriConverter
    {
        // BUG: Need to conform to https://www.ietf.org/rfc/rfc2396.txt
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
        public const char UNC_HEX_SEPARATOR_CHAR = '-';
        public const string UNC_HEX_SEPARATOR_STRING = "-";
        public const string FQDN_IPV6_UNC = ".ipv6-literal.net";
        public const string MATCH_GROUP_NAME_FILE = "file";
        public const string MATCH_GROUP_NAME_HOST = "host";
        public const string MATCH_GROUP_NAME_PATH = "path";
        public const string MATCH_GROUP_NAME_FILE_NAME = "fileName";
        public const string MATCH_GROUP_NAME_IPV4 = "ipv4";
        public const string MATCH_GROUP_NAME_IPV6 = "ipv6";
        public const string MATCH_GROUP_NAME_UNC = "unc";
        public const string MATCH_GROUP_NAME_DNS = "dns";
        public const string MATCH_GROUP_NAME_DIR = "dir";
        public const string MATCH_GROUP_NAME_ROOT = "root";
        public const string MATCH_GROUP_NAME_SCHEME = "scheme";
        public const string MATCH_GROUP_NAME_ESC = "esc";
        public const string MATCH_GROUP_NAME_HEX = "hex";
        public const string MATCH_GROUP_NAME_CASE = "case";

        private static readonly char[] _INVALID_FILENAME_CHARS;

        /// <summary>
        /// The <see cref="FileUriConverter"/> for the current host operating system.
        /// </summary>
        public static readonly FileUriConverter CURRENT_FACTORY;

        /// <summary>
        /// The <see cref="FileUriConverter"/> for the operating system type that is alternate to the current host operating system.
        /// </summary>
        public static readonly FileUriConverter ALT_FACTORY;

        #region Regular Expressions

        public const string PATTERN_BASIC_OR_DNS_NAME = @"(?i)^\s*(?=[\w-.]{1,255}(?![\w-.]))[a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?\s*$";

        /// <summary>
        /// Matches a <seealso cref="UriHostNameType.Basic"/>, <seealso cref="UriHostNameType.Dns"/> or <seealso cref="UriHostNameType.IPv4"/> address.
        /// </summary>
        public static readonly Regex BASIC_DNS_OR_IPV4_NAME_REGEX = new Regex(@"
^
(?=[\w-.]{1,255}(?![\w-.]))
[a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?
$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

        /// <summary>
        /// Matches an IPV4 address.
        /// </summary>
        public static readonly Regex IPV4_ADDRESS_REGEX = new Regex(@"^
((?<!\d)(0(?=\d))*(?!25[6-9]|([3-9]\d|1\d\d)\d)\d{1,3}\.?){4}(?<!\.)
$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

        /// <summary>
        /// Matches an IPV6 address.
        /// </summary>
        /// <remarks>This does not match addresses with surrounding brackets or the format used in UNC path strings.</remarks>
        public static readonly Regex IPV6_ADDRESS_REGEX = new Regex(@"^
(([a-f\d]{1,4}:){7}(:|[a-f\d]{1,4})|(?=(\w*:){2,7}\w*$)(([a-f\d]{1,4}:)+|:):([a-f\d]{1,4}(:[a-f\d]{1,4})*)?)
$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

        public static readonly Regex TEMP_REGEX = new Regex(@"^
(([a-f\d]{1,4}:){7}(:|[a-f\d]{1,4})|(?=(\w*:){2,7}\w*$)(([a-f\d]{1,4}:)+|:):([a-f\d]{1,4}(:[a-f\d]{1,4})*)?)
$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

        public const string PATTERN_HOST_NAME_OR_ADDRESS = @"(?i)^\s*(((?<!\d)(0(?=\d))*(?!25[6-9]|([3-9]\d|1\d\d)\d)\d{1,3}\.?){4}(?<!\.)" +
            @"|(?=([a-f\d:]+|[a-f\d-]+\.ipv6-literal\.net|\[([a-f\d:]+|[a-f\d-]+\.ipv6-literal\.net)\])$)\[?(([a-f\d]{1,4}[:-]){7}([:-]|[a-f\d]{1,4})" +
            @"|(?=(\w*[:-]){2,7}\w*(\]|\.|$))(([a-f\d]{1,4}[:-])+|[:-])[:-]([a-f\d]{1,4}([:-][a-f\d]{1,4})*)?)(\.ipv6-literal\.net)?\]?" +
            @"|(?=[\w-.]{1,255}(?![\w-.]))[a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?)\s*$";

        /// <summary>
        /// Matches a host name or address that can be used as the host component of a UNC path string.
        /// </summary>
        /// <remarks>Named group definitions:
        /// <list type="bullet"><item><term>ipv4</term> Matches an <seealso cref="UriHostNameType.IPv4" /> host name. This implies that the input text is an absolute UNC path
        ///             (<see cref="MATCH_GROUP_NAME_IPV4" />).</item><item><term>ipv6</term> Matches an IPV6 host name. This implies that the input text is an absolute UNC path
        ///             (<see cref="MATCH_GROUP_NAME_IPV6" />).
        ///         <list type="bullet"><item><term>unc</term> Matches the domain of an IPV6 address.
        ///                 This implies that the input text is an absolute UNC path (<see cref="MATCH_GROUP_NAME_UNC" />).</item></list></item><item><term>dns</term> Matches a <seealso cref="UriHostNameType.Dns" /> or <seealso cref="UriHostNameType.Basic" /> host name.
        ///         This implies that the input text is an absolute UNC path (<see cref="MATCH_GROUP_NAME_DNS" />).</item></list></remarks>
        public static readonly Regex HOST_NAME_OR_ADDRESS_FOR_FS_REGEX = new Regex(@"
^
(
    (?<ipv4>((?<!\d)(0(?=\d))*(?!25[6-9]|([3-9]\d|1\d\d)\d)\d{1,3}\.?){4}(?<!\.))
|
    (?=(\[[a-f\d:]+\]|[a-f\d:]+|[a-f\d-]+\.ipv6-literal\.net)$)
    \[?
    (?<ipv6>([a-f\d]{1,4}[:-]){7}([:-]|[a-f\d]{1,4})|(?=(\w*[:-]){2,7}\w*\]?$)(([a-f\d]{1,4}[:-])+|[:-])[:-]([a-f\d]{1,4}([:-][a-f\d]{1,4})*)?)
    (\]|(?<unc>\.ipv6-literal\.net))?
|
    (?=[\w-.]{1,255}(?![\w-.]))
    (?<dns>[a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?)
)$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

        /// <summary>
        /// Matches a host name or address that can be used as the host component of a URI string.
        /// </summary>
        /// <remarks>Named group definitions:
        /// <list type="bullet"><item><term>ipv4</term> Matches an <seealso cref="UriHostNameType.IPv4" /> host name. This implies that the input text is an absolute UNC path
        ///             (<see cref="MATCH_GROUP_NAME_IPV4" />).</item><item><term>ipv6</term> Matches an IPV6 host name. This implies that the input text is an absolute UNC path
        ///             (<see cref="MATCH_GROUP_NAME_IPV6" />).</item><item><term>dns</term> Matches a <seealso cref="UriHostNameType.Dns" /> or <seealso cref="UriHostNameType.Basic" /> host name.
        ///         This implies that the input text is an absolute UNC path (<see cref="MATCH_GROUP_NAME_DNS" />).</item></list></remarks>
        public static readonly Regex HOST_NAME_OR_ADDRESS_FOR_URI_REGEX = new Regex(@"^
(
    (?<ipv4>((?<!\d)(0(?=\d))*(?!25[6-9]|([3-9]\d|1\d\d)\d)\d{1,3}\.?){4}(?<!\.))
|
    (?=(\[[a-f\d:]+\]|[a-f\d:]+)$)
    \[?(?<ipv6>([a-f\d]{1,4}:){7}(:|[a-f\d]{1,4})|(?=(\w*:){2,7}\w*\]?$)(([a-f\d]{1,4}:)+|:):([a-f\d]{1,4}(:[a-f\d]{1,4})*)?)\]?
|
    (?=[\w-.]{1,255}(?![\w-.]))
    (?<dns>[a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?)
)$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

        /// <summary>
        /// Matches a URI escape sequence that is not upper-case or a <seealso cref="Uri.UriSchemeFile">file</seealso> scheme name that is not lower case.
        /// </summary>
        public static readonly Regex URI_ENCODE_NORMALIZE_REGEX = new Regex(@"(?<=^\s*)(?<file>(?!file:)(?i)FILE(?=:))|(%(a-f[\dA-Fa-f]|[\dA-F][a-f]))+");

        /// <summary>
        /// Matches a path segment including optional trailing slash.
        /// </summary>
        public static readonly Regex URI_PATH_SEGMENT_REGEX = new Regex(@"(?:^|\G)(?:/|([^/]+)(?:/|$))", RegexOptions.Compiled);
        //public static readonly Regex URI_PATH_SEGMENT_REGEX_OLD = new Regex(@"(?:^|\G)(?:/|([^/]+)(?:/|$))", RegexOptions.Compiled);


        /// <summary>
        /// Matches consecutive URI path separators as well as leading and trailing whitespace and path separators.
        /// </summary>
        /// <remarks>This will also match relative self-reference sequences (<c>/./<c>). This does not match parent segment references (<c>/../</c>)
        /// unless they are at the beginning of the string.</remarks>
        public static readonly Regex URI_REL_PATH_SEPARATOR_NORMALIZE_REGEX = new Regex(@"^(\s*((\.\.?/+)+|/+)|\s+)|(/(?=/)|/\.(?=/|$))+|(/\s*|\s+)$", RegexOptions.Compiled);

        /// <summary>
        /// Matches consecutive and trailing URI path separators, allowing up to 3 consecutive path separator characters following the scheme separator.
        /// </summary>
        /// <remarks>This will also match surrounding whitespace and relative self-reference sequences (<c>/./<c>).. This does not match parent segment
        /// references (<c>/../</c>) unless they are at the beginning of the string.</remarks>
        public static readonly Regex URI_ABS_PATH_SEPARATOR_NORMALIZE_REGEX = new Regex(@"^((\s*(\.\.?/+)+)|\s+)|((?<!^\s*file:/?)/(?=/)|/\.(?=/|$))+|((?<=.)/\s*|\s+)$", RegexOptions.Compiled);

        /// <summary>
        /// Matches incorrectly-cased URI escape sequences as well as consecutive URI path separators as well as leading and trailing whitespace and path separators.
        /// </summary>
        /// <remarks>
        /// <remarks>Match group name definitions:
        /// <list type="bullet">
        /// <item><term>case</term> Escape sequence(s) not entirely upper-case (<seealso cref="MATCH_GROUP_NAME_CASE"/>).</item>
        /// <item><term>(no named group matching)</term> Extraneous path separator and/or whitespace.</item>
        /// </list>
        /// <para>This will also match relative self-reference sequences (<c>/./<c>). This does not match parent segment references (<c>/../</c>)
        /// unless they are at the beginning of the string.</para></remarks>
        public static readonly Regex REL_URI_STRING_NORMALIZE_REGEX = new Regex(@"^\s*((\.\.?/+)+|\s+)|(?<case>(%(a-f[\dA-Fa-f]|[\dA-F][a-f]))+)|(/(?=/)|/\.(?=/|$))+|(/\s*|\s+)$", RegexOptions.Compiled);

        /// <summary>
        /// Matches incorrectly-cased file scheme name and URI escape sequences as well as consecutive and trailing URI path separators, allowing up to 3 consecutive
        /// path separator characters following the scheme separator.
        /// </summary>
        /// <remarks>Match group name definitions:
        /// <list type="bullet">
        /// <item><term>scheme</term> <seealso cref="Uri.UriSchemeFile">file</seealso> scheme name is not entirely lower case.
        ///     This will also include any leading whitespace (<seealso cref="MATCH_GROUP_NAME_SCHEME"/>).</item>
        /// <item><term>case</term> Escape sequence(s) not entirely upper-case (<seealso cref="MATCH_GROUP_NAME_CASE"/>).</item>
        /// <item><term>(no named group matching)</term> Extraneous path separator and/or whitespace.</item>
        /// </list>
        /// <para>This will also match surrounding whitespace and relative self-reference sequences (<c>/./<c>).. This does not match parent segment
        /// references (<c>/../</c>) unless they are at the beginning of the string.</para></remarks>
        public static readonly Regex ABS_URI_STRING_NORMALIZE_REGEX = new Regex(@"
^
    \s*
    (
        (?<scheme>(?!file:)(?i)FILE(?=:))
    |
        (\.\.?/+)+
    |
        \s+
    )
|
    (?<esc>
        (
            %
            (
                a-f[\dA-Fa-f]
            |
                [\dA-F][a-f]
            )
        )+
    )
|
    (?<=file:///)
    /+
|
    (?<!file:/*)
    (/(?=/))+
|
    /\.(?=/|$)
|
    (/\s*|\s+)
$", RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

        public static readonly Regex NEEDS_ENCODING_REGEX = new Regex(@"([^!$=&-/:;=@[\]?#%\w]+|%(?![\dA-Z]{2}))*", RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
        /// <summary>
        /// Matches <seealso cref="Uri.UriSchemeFile">file</seealso> URI strings that is compatible with Windows and/or Linux platforms.
        /// </summary>
        public static readonly Regex ANY_FILE_URI_STRICT_REGEX = new Regex(@"
^
(
    file://
    ((?i)
        ((?<!\d)(0(?=\d))*(?!25[6-9]|([3-9]\d|1\d\d)\d)\d{1,3}\.?){4}(?<!\.)
    |
        (?=(\[[a-f\d:]+\]|[a-f\d:]+|[a-f\d-]+\.ipv6-literal\.net)$)
        \[?
        (([a-f\d]{1,4}:){7}(:|[a-f\d]{1,4})(?=[/?#]|$)|(?=(\w*:){2,7}\w*[/?#]|$)(([a-f\d]{1,4}:)+|:):([a-f\d]{1,4}(:[a-f\d]{1,4})*)?)
        \]?
    |
        (?=[\w-.]{1,255}(?![\w-.]))
        [a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?
    )?
    (?=/|$)
|
    (?!\w+:/)
    (
        [!$=&-.:;=@[\]\w]+
    |
        %(0[1-9A-F]|[1-9A-F][\dA-F])
    )*
)
(
    /
    (
        [!$=&-.:;=@[\]\w]+
    |
        %(0[1-9A-F]|[1-9A-F][\dA-F])
    )+
)*
/?
$", RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

        /// <summary>
        /// Pattern which matches a string which can be normalized as <seealso cref="Uri.UriSchemeFile">file</seealso> URI or local path that is compatible with
        /// Windows and/or Linux platforms. 
        /// </summary>
        public const string PATTERN_ANY_ABS_FILE_URI_OR_LOCAL_LAX = @"(?i)^\s*((file://|[\\/]{2})(((?<!\d)(0(?=\d))*(?!25[6-9]|([3-9]\d|1\d\d)\d)\d{1,3}\.?){4}(?<!\.)" +
            @"|(?=\[[a-f\d]*(:[a-f\d]*){2,7}\]|[a-f\d]*(:[a-f\d]*){2,7}(?=[/:?#]|$)(?=[/:?#]|$))\[?([a-f\d]{1,4}(:[a-f\d]{1,4}){7}|(([a-f\d]{1,4}:)+|:)(:[a-f\d]{1,4})+|::)\]?" +
            @"|(?=[\w-.]{1,255}(?![\w-.]))[a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?)|(file:///)?(?=[a-z]:|/))" +
            @"(([a-z]:)?([\\/](?=\s*$)|([\\/]([^\u0000-\u0019\u007f-\u00a0\u1680\u2028\u2029\ud800-\udfff\\/%]+|%((?![A-F\d]{2})|0[1-9A-F]|[1-9A-F]))+)*))[\\/]?\s*$";

        #endregion

        /// <summary>
        /// Character which separates path segments on the target filesystem.
        /// </summary>
        public abstract char LocalDirectorySeparatorChar { get; }

        /// <summary>
        /// Display name for the target filesystem type.
        /// </summary>
        public abstract string FsDisplayName { get; }

        /// <summary>
        /// The target filesystem platform type.
        /// </summary>
        public abstract PlatformType FsPlatform { get; }

        /// <summary>
        /// Matches a well-formed URI that can be converted to a valid absolute or relative local path on the typical target filesystem type.
        /// </summary>
        /// <remarks>Named group definitions:
        /// <list type="bullet">
        ///     <item><term>file</term> Matches the <seealso cref="Uri.UriSchemeFile">file</seealso> uri scheme and URI authority.
        ///         (<see cref="MATCH_GROUP_NAME_SCHEME"/>).</item>
        ///     <item><term>host</term> Matches the host name. This implies that the input text is an absolute UNC path
        ///             (<see cref="MATCH_GROUP_NAME_HOST"/>).
        ///         <list type="bullet">
        ///             <item><term>ipv4</term> Matches an <seealso cref="UriHostNameType.IPv4"/> host name. This implies that the input text is an absolute UNC path
        ///                  (<see cref="MATCH_GROUP_NAME_IPV4"/>).</item>
        ///             <item><term>ipv6</term> Matches an IPV6 host name. This implies that the input text is an absolute UNC path
        ///                     (<see cref="MATCH_GROUP_NAME_IPV6"/>).</item>
        ///             <item><term>dns</term> Matches a <seealso cref="UriHostNameType.Dns"/> or <seealso cref="UriHostNameType.Basic"/> host name.
        ///                 This implies that the input text is an absolute UNC path (<see cref="MATCH_GROUP_NAME_DNS"/>).</item>
        ///         </list>
        ///     </item>
        ///     <item><term>path</term> Matches the path string (<see cref="MATCH_GROUP_NAME_PATH"/>).
        ///         <list type="bullet">
        ///             <item><term>root</term> Matches path root character. This implies that the input text is
        ///                 an absolute local path (<see cref="MATCH_GROUP_NAME_ROOT"/>).</item>
        ///         </list>
        ///     </item>
        /// </list></remarks>
        public abstract Regex UriHostAndPathStrictRegex { get; }

        /// <summary>
        /// Matches a URI that can be converted to a valid absolute or relative local path on a typical target filesystem type.
        /// </summary>
        /// <remarks>Named group match definition:
        /// <list type="bullet">
        ///     <item><term>root</term> Matches the host name or path root character that indicates it is an absolute path.</item>
        /// </list></remarks>
        public abstract Regex UriValidationRegex { get; }

        /// <summary>
        /// Matches consecutive and trailing filesystem path separators, allowing up to 2 consecutive path separator characters at the beginning of the string.
        /// </summary>
        /// <remarks>This will also match surrounding whitespace and relative self-reference sequences (<c>\.\<c>). This does not match parent segment
        /// references (<c>\..\</c>) if they are not at the beginning of the string.</remarks>
        public abstract Regex FsFullPathNormalizeRegex { get; }

        /// <summary>
        /// Matches a well-formed relative or absolute local path on the typicaltypical target filesystem type.
        /// </summary>
        /// <remarks>Named group definitions:
        /// <list type="bullet">
        ///     <item><term>host</term> Matches the host name. This implies that the input text is an absolute UNC path
        ///             (<see cref=MATCH_GROUP_NAME_HOST"/>).
        ///         <list type="bullet">
        ///             <item><term>ipv4</term> Matches an <seealso cref="UriHostNameType.IPv4"/> host name. This implies that the input text is an absolute UNC path
        ///                  (<see cref=MATCH_GROUP_NAME_IPV4"/>).</item>
        ///             <item><term>ipv6</term> Matches an IPV6 host name. This implies that the input text is an absolute UNC path
        ///                     (<see cref=MATCH_GROUP_NAME_IPV6"/>).
        ///                 <list type="bullet">
        ///                     <item><term>unc</term> Matches the domain of an IPV6 address.
        ///                         This implies that the input text is an absolute UNC path (<see cref=MATCH_GROUP_NAME_UNC"/>).</item>
        ///                 </list>
        ///             </item>
        ///             <item><term>dns</term> Matches a <seealso cref="UriHostNameType.Dns"/> or <seealso cref="UriHostNameType.Basic"/> host name.
        ///                 This implies that the input text is an absolute UNC path (<see cref=MATCH_GROUP_NAME_DNS"/>).</item>
        ///         </list>
        ///     </item>
        ///     <item><term>path</term> Matches the path string (<see cref=MATCH_GROUP_NAME_PATH"/>).
        ///         <list type="bullet">
        ///             <item><term>root</term> Matches leading directory separator character. This implies that the input text is
        ///                 an absolute local path (<see cref=MATCH_GROUP_NAME_ROOT"/>).</item>
        ///         </list>
        ///     </item>
        /// </list></remarks>
        public abstract Regex FsHostAndPathRegex { get; }

        /// <summary>
        /// Determines whether a string is valid for use as a file name on the local file system.
        /// </summary>
        /// <param name="name">The file name to test.</param>
        /// <returns><see langword="true"/> if <paramref name="name"/> is not <see langword="null"/> or empty and contains only valid local file system
        /// characters.</returns>
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
            _INVALID_FILENAME_CHARS = invalidFileNameChars.Contains(Path.PathSeparator) ? invalidFileNameChars :
                invalidFileNameChars.Concat(new char[] { Path.PathSeparator }).ToArray();
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
        /// Converts a URI-compatible host name and URI-encoded absolute path string to a filesystem path string.
        /// </summary>
        /// <param name="fileUriString">An absolute <seealso cref="Uri.UriSchemeFile">file</seealso> URL string or a relative URI-encoded path string.</param>
        /// <param name="platform">The target platform type that determines the format of the filesystem path string.</param>
        /// <param name="allowAlt">If <see langword="true"/>, allows a valid filesystem path of the platform type which is alternate to the specified
        /// <seealso cref="PlatformType"/> to be used if <paramref name="fileUriString"/> does not not represent a valid path according to the <paramref name="platform"/>.</param>
        /// <returns>A filesystem path string.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="fileUriString"/> is invalid.</exception>
        public static string ToFileSystemPath(string fileUriString, PlatformType platform, bool allowAlt = false)
        {
            if (allowAlt)
            {
                FileUriConverter currentFactory = GetFactory(platform, out FileUriConverter altFactory);
                if (!currentFactory.IsWellFormedUriString(fileUriString, UriKind.Absolute) && altFactory.IsWellFormedUriString(fileUriString, UriKind.Absolute))
                    return altFactory.ToFileSystemPath(fileUriString);
                return currentFactory.ToFileSystemPath(fileUriString);
            }
            return GetFactory(platform).ToFileSystemPath(fileUriString);
        }

        /// <summary>
        /// Converts a <seealso cref="Uri.UriSchemeFile">file</seealso> URL to a filesystem path string.
        /// </summary>
        /// <param name="fileUriString">An absolute <seealso cref="Uri.UriSchemeFile">file</seealso> URL string or a relative URI-encoded path string.</param>
        /// <returns>A filesystem path string.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="fileUriString"/> is invalid.</exception>
        public abstract string ToFileSystemPath(string fileUriString);

        /// <summary>
        /// Converts a local filesystem path string to an URI encoded file name, relative directory path string, and host name.
        /// </summary>
        /// <param name="path">The local filesystem path string to convert.</param>
        /// <param name="hostName">Returns the URI-compatible host name that was referenced within the file system <paramref name="path"/>
        /// or <seealso cref="string.Empty"/> if it did not reference a host name.</param>
        /// <param name="directoryName">Returns the URI-encoded relative directory path string. This will never end with a path separator (<c>/</c>)
        /// unless it represents the root subdirectory.</param>
        /// <returns>The URI-encoded file name (leaf) portion of the path string or <seealso cref="string.Empty"/> if the file system <paramref name="path"/>
        /// referenced the root subdirectory.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="path"/> is invalid.</exception>
        public abstract string FromFileSystemPath(string path, out string hostName, out string directoryName);

        /// <summary>
        /// Converts a local filesystem path string to an URI encoded relative path string and host name.
        /// </summary>
        /// <param name="path">The local filesystem path string to convert.</param>
        /// <param name="hostName">Returns the URI-compatible host name that was referenced within the file system <paramref name="path"/>
        /// or <seealso cref="string.Empty"/> if it did not reference a host name.</param>
        /// <returns>The URI-encoded relative path string. This will never end with a path separator (<c>/</c>) unless it represents the root subdirectory.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="path"/> is invalid.</exception>
        public abstract string FromFileSystemPath(string path, out string hostName);

        /// <summary>
        /// Converts a local filesystem path string to an absolute <seealso cref="Uri.UriSchemeFile">file</seealso> URL or a URI encoded relative path string.
        /// </summary>
        /// <param name="path">The local filesystem path string to convert.</param>
        /// <returns>An absolute <seealso cref="Uri.UriSchemeFile">file</seealso> URL or a URI encoded relative path string.
        /// This will never end with a path separator (<c>/</c>) unless it represents the root subdirectory.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="path"/> is invalid.</exception>
        public abstract string FromFileSystemPath(string path);

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
                return currentFactory.TryCreateFileUriFromUriString(inputString, out fileUri) || altFactory.TryCreateFileUriFromUriString(inputString, out fileUri) ||
                    currentFactory.TryCreateFileUriFromFsPath(inputString, out fileUri) || altFactory.TryCreateFileUriFromFsPath(inputString, out fileUri);
            return currentFactory.TryCreateFileUriFromFsPath(inputString, out fileUri) || altFactory.TryCreateFileUriFromFsPath(inputString, out fileUri) ||
                currentFactory.TryCreateFileUriFromUriString(inputString, out fileUri) || altFactory.TryCreateFileUriFromUriString(inputString, out fileUri);
        }

        /// <summary>
        /// Attempt to create a <seealso cref="FileUri"/> object from an absolute filesystem path string.
        /// </summary>
        /// <param name="fileSystemPath">The abolute filesystem path string.</param>
        /// <param name="fileUri">Returns the constructed <seealso cref="FileUri"/> object or <see langword="null"/> if the <paramref name="inputString"/>
        /// could not be parsed.</param>
        /// <returns><see langword="true"/> if the <paramref name="fileSystemPath"/> could be parsed as a filesystem path string;
        /// otherwise, <see langword="false"/>.</returns>
        public abstract bool TryCreateFileUriFromFsPath(string fileSystemPath, out FileUri fileUri);

        /// <summary>
        /// Attempt to create a <seealso cref="FileUri"/> object from an absolute <seealso cref="Uri.UriSchemeFile">file</seealso> URL string.
        /// </summary>
        /// <param name="uriString">The absolute file URI string.</param>
        /// <param name="fileUri">Returns the constructed <seealso cref="FileUri"/> object or <see langword="null"/> if the <paramref name="inputString"/>
        /// could not be parsed.</param>
        /// <returns><see langword="true"/> if the <paramref name="uriString"/> could be parsed as an absolute <seealso cref="Uri.UriSchemeFile">file</seealso> URL;
        /// otherwise, <see langword="false"/>.</returns>
        public bool TryCreateFileUriFromUriString(string uriString, out FileUri fileUri)
        {
            if (string.IsNullOrEmpty(uriString))
            {
                fileUri = null;
                return false;
            }
            Match match = UriHostAndPathStrictRegex.Match(uriString);
            if (!match.Success)
            {
                bool wasChanged;
                if ((match = UriValidationRegex.Match(uriString)).Success)
                    uriString = match.Groups[MATCH_GROUP_NAME_FILE].Success ? NormalizeAbsoluteFileUrl(uriString, out wasChanged) :
                        NormalizeRelativeFileUri(uriString, out wasChanged);
                else
                {
                    uriString = EscapeSpecialPathChars(uriString, out wasChanged);
                    if (wasChanged)
                    {
                        if ((match = UriValidationRegex.Match(uriString)).Success)
                        {
                            if (match.Groups[MATCH_GROUP_NAME_FILE].Success)
                                uriString = NormalizeAbsoluteFileUrl(uriString);
                            else
                                uriString = NormalizeRelativeFileUri(uriString);
                        }
                        else
                            wasChanged = false;
                    }
                }
                if (!(wasChanged && (match = UriHostAndPathStrictRegex.Match(uriString)).Success))
                {
                    fileUri = null;
                    return false;
                }
            }
            if (match.Groups[MATCH_GROUP_NAME_HOST].Success || match.Groups[MATCH_GROUP_NAME_ROOT].Success)
            {
                fileUri = new FileUri(uriString);
                return true;
            }
            fileUri = null;
            return false;
        }

        /// <summary>
        /// Attempt to parse components from a URI-encoded <seealso cref="Uri.UriSchemeFile">file</seealso> URL or a relative path string.
        /// </summary>
        /// <param name="uriString">The URI-encoded string to parse.</param>
        /// <param name="platform">The target platform type.</param>
        /// <param name="includeAltPlatform">If <see langword="true"/> and the <paramref name="uriString"/> is not compatible with the target
        /// <paramref name="platform"/> type, this will attempt to parse the <paramref name="uriString"/> targeting the alternative platform type.</param>
        /// <param name="hostName">Returns the host name from the URI-encoded string or empty if it contained no host name.</param>
        /// <param name="directory">Returns the parent directory path without the host name.</param>
        /// <param name="fileName">Returns the file name (leaf) from the path string. This can be empty if the URI refers to the root directory.</param>
        /// <param name="isAbsolute">Returns <see langword="true"/> if <paramref name="directory"/> is absolute; otherwise, <see langword="false"/>.
        /// This will always return <see langword="true"/> when <paramref name="hostName"/> is not empty.</param>
        /// <returns><see langword="true"/> if the <paramref name="uriString"/> is is compatible with the target OS type and is a
        /// <seealso cref="Uri.UriSchemeFile">file</seealso> URL or a relative path string.</returns>
        public static bool TrySplitFileUriString(string uriString, PlatformType platform, bool includeAltPlatform, out string hostName, out string directory,
            out string fileName, out bool isAbsolute)
        {
            if (includeAltPlatform)
            {
                FileUriConverter currentFactory = GetFactory(platform, out FileUriConverter altFactory);
                return currentFactory.TrySplitFileUriString(uriString, out hostName, out directory, out fileName, out isAbsolute) ||
                        altFactory.TrySplitFileUriString(uriString, out hostName, out directory, out fileName, out isAbsolute);
            }
            return GetFactory(platform).TrySplitFileUriString(uriString, out hostName, out directory, out fileName, out isAbsolute);
        }

        public abstract string SplitFsPathLeaf(string fsPath, out string leafSegment);

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

        public static string SplitUriPathLeaf(string uriString, out string leafSegment)
        {
            if (string.IsNullOrEmpty(uriString))
            {
                leafSegment = "";
                return "";
            }
            int e = uriString.Length - 1;
            int i = uriString.LastIndexOf(URI_PATH_SEPARATOR_CHAR);
            while (i == e)
            {
                if (i == 0)
                    break;
                uriString = uriString.Substring(0, i);
                e--;
                i = uriString.LastIndexOf(URI_PATH_SEPARATOR_CHAR);
            }
            if (i++ < 0)
            {
                leafSegment = uriString;
                return "";
            }
            leafSegment = uriString.Substring(i);
            return uriString.Substring(0, (i > 1) ? i - 1 : i);
        }

        /// <summary>
        /// Attempt to parse components from a URI-encoded <seealso cref="Uri.UriSchemeFile">file</seealso> URL or a relative path string.
        /// </summary>
        /// <param name="uriString">The URI-encoded string to parse.</param>
        /// <param name="hostName">Returns the host name from the URI-encoded string or empty if it contained no host name.</param>
        /// <param name="directory">Returns the parent directory path without the host name.</param>
        /// <param name="fileName">Returns the file name (leaf) from the path string. This can be empty if the URI refers to the root directory.</param>
        /// <param name="isAbsolute">Returns <see langword="true"/> if <paramref name="directory"/> is absolute; otherwise, <see langword="false"/>.
        /// This will always return <see langword="true"/> when <paramref name="hostName"/> is not empty.</param>
        /// <returns><see langword="true"/> if the <paramref name="uriString"/> is is compatible with the target OS type and is a
        /// <seealso cref="Uri.UriSchemeFile">file</seealso> URL or a relative path string.</returns>
        public bool TrySplitFileUriString(string uriString, out string hostName, out string directory, out string fileName, out bool isAbsolute)
        {
            if (string.IsNullOrEmpty(uriString))
            {
                hostName = directory = fileName = "";
                isAbsolute = false;
                return false;
            }

            Match match = UriHostAndPathStrictRegex.Match(uriString);
            if (!match.Success)
            {
                bool wasChanged;
                if ((match = UriValidationRegex.Match(uriString)).Success)
                {
                    uriString = match.Groups[MATCH_GROUP_NAME_FILE].Success ? NormalizeAbsoluteFileUrl(uriString, out wasChanged) :
                        NormalizeRelativeFileUri(uriString, out wasChanged);
                    if (wasChanged)
                    {
                        uriString = EscapeSpecialPathChars(uriString);
                        uriString = FixEncoding(uriString);
                    }
                    else
                    {
                        uriString = EscapeSpecialPathChars(uriString, out wasChanged);
                        if (wasChanged)
                            uriString = FixEncoding(uriString);
                        else
                            uriString = FixEncoding(uriString, out wasChanged);
                    }
                }
                else
                {
                    uriString = EscapeSpecialPathChars(uriString, out wasChanged);
                    if (wasChanged)
                        uriString = FixEncoding(uriString);
                    else
                        uriString = FixEncoding(uriString, out wasChanged);
                    if (wasChanged)
                    {
                        if ((match = UriValidationRegex.Match(uriString)).Success)
                        {
                            if (match.Groups[MATCH_GROUP_NAME_FILE].Success)
                                uriString = NormalizeAbsoluteFileUrl(uriString);
                            else
                                uriString = NormalizeRelativeFileUri(uriString);
                        }
                        else
                            wasChanged = false;
                    }
                }
                if (!(wasChanged && (match = UriHostAndPathStrictRegex.Match(uriString)).Success))
                {
                    directory = uriString;
                    hostName = fileName = "";
                    isAbsolute = false;
                    return false;
                }
            }

            Group group = match.Groups[MATCH_GROUP_NAME_IPV6];
            isAbsolute = group.Success;
            if (isAbsolute)
                hostName = group.Value;
            else
            {
                hostName = match.GetGroupValue(MATCH_GROUP_NAME_HOST, "");
                isAbsolute = (match.Groups[MATCH_GROUP_NAME_HOST].Success || match.Groups[MATCH_GROUP_NAME_ROOT].Success);
            }
            string path = match.GetGroupValue(MATCH_GROUP_NAME_PATH, "");
            directory = SplitUriPathLeaf(path, out fileName);
            //directory = match.GetGroupValue(MATCH_GROUP_NAME_DIR, "");
            //fileName = match.GetGroupValue(MATCH_GROUP_NAME_FILE_NAME, "");
            return true;
        }

        public static string FixEncoding(string uriString)
        {
            if (string.IsNullOrEmpty(uriString))
                return "";

            if (NEEDS_ENCODING_REGEX.IsMatch(uriString))
                return NEEDS_ENCODING_REGEX.Replace(uriString, m => Uri.EscapeUriString(m.Value));
            return uriString;
        }

        public static string FixEncoding(string uriString, out bool wasChanged)
        {
            if (uriString is null)
            {
                wasChanged = true;
                return "";
            }

            wasChanged = uriString.Length > 0 && NEEDS_ENCODING_REGEX.IsMatch(uriString);
            if (wasChanged)
                return NEEDS_ENCODING_REGEX.Replace(uriString, m => Uri.EscapeUriString(m.Value));
            return uriString;
        }

        /// <summary>
        /// Attempt to parse components from a URI-encoded <seealso cref="Uri.UriSchemeFile">file</seealso> URL or a relative path string.
        /// </summary>
        /// <param name="uriString">The URI-encoded string to parse.</param>
        /// <param name="hostName">Returns the host name from the URI-encoded string or empty if it contained no host name.</param>
        /// <param name="path">Returns the path string.</param>
        /// <param name="isAbsolute">Returns <see langword="true"/> if <paramref name="path"/> is absolute; otherwise, <see langword="false"/>.
        /// This will always return <see langword="true"/> when <paramref name="hostName"/> is not empty.</param>
        /// <returns><see langword="true"/> if the <paramref name="uriString"/> is is compatible with the target OS type and is a
        /// <seealso cref="Uri.UriSchemeFile">file</seealso> URL or a relative path string.</returns>
        public bool TrySplitFileUriString(string uriString, out string hostName, out string path, out bool isAbsolute)
        {
            if (string.IsNullOrEmpty(uriString))
            {
                hostName = path = "";
                isAbsolute = false;
                return false;
            }

            Match match = UriHostAndPathStrictRegex.Match(uriString);
            if (!match.Success)
            {
                bool wasChanged;

                if ((match = UriValidationRegex.Match(uriString)).Success)
                {
                    uriString = match.Groups[MATCH_GROUP_NAME_FILE].Success ? NormalizeAbsoluteFileUrl(uriString, out wasChanged) :
                        NormalizeRelativeFileUri(uriString, out wasChanged);
                    if (wasChanged)
                    {
                        uriString = EscapeSpecialPathChars(uriString);
                        uriString = FixEncoding(uriString);
                    }
                    else
                    {
                        uriString = EscapeSpecialPathChars(uriString, out wasChanged);
                        if (wasChanged)
                            uriString = FixEncoding(uriString);
                        else
                            uriString = FixEncoding(uriString, out wasChanged);
                    }
                }
                else
                {
                    uriString = EscapeSpecialPathChars(uriString, out wasChanged);
                    if (wasChanged)
                        uriString = FixEncoding(uriString);
                    else
                        uriString = FixEncoding(uriString, out wasChanged);
                    if (wasChanged)
                    {
                        if ((match = UriValidationRegex.Match(uriString)).Success)
                        {
                            if (match.Groups[MATCH_GROUP_NAME_FILE].Success)
                                uriString = NormalizeAbsoluteFileUrl(uriString);
                            else
                                uriString = NormalizeRelativeFileUri(uriString);
                        }
                        else
                            wasChanged = false;
                    }
                }
                if (!(wasChanged && (match = UriHostAndPathStrictRegex.Match(uriString)).Success))
                {
                    hostName = path = "";
                    isAbsolute = false;
                    return false;
                }
            }

            Group group = match.Groups[MATCH_GROUP_NAME_IPV6];
            isAbsolute = group.Success;
            if (isAbsolute)
                hostName = group.Value;
            else
            {
                hostName = match.GetGroupValue(MATCH_GROUP_NAME_HOST, "");
                isAbsolute = (match.Groups[MATCH_GROUP_NAME_HOST].Success || match.Groups[MATCH_GROUP_NAME_ROOT].Success);
            }
            path = match.GetGroupValue(MATCH_GROUP_NAME_PATH, "");
            return true;
        }

        /// <summary>
        /// Removes extraneous path separators and whitespace, ensures the file scheme name is lower case and escape sequence characters are upper-case.
        /// </summary>
        /// <param name="uriString">Absolute URI string to normalize.</param>
        /// <param name="wasChanged">Returns <see langword="true"/> if the <paramref name="uriString"/> was normalized or <see langword="false"/>
        /// if <paramref name="uriString"/> did not need to be normalized.</param>
        /// <returns>The normalized URI string.</returns>
        public static string NormalizeAbsoluteFileUrl(string uriString, out bool wasChanged)
        {
            if (uriString is null)
            {
                wasChanged = true;
                return "";
            }

            wasChanged = uriString.Length > 0 && ABS_URI_STRING_NORMALIZE_REGEX.IsMatch(uriString);
            if (wasChanged)
                return ABS_URI_STRING_NORMALIZE_REGEX.Replace(uriString, m =>
                    (m.Groups[MATCH_GROUP_NAME_CASE].Success) ? m.Value.ToUpper() :
                    ((m.Groups[MATCH_GROUP_NAME_SCHEME].Success) ? m.Groups[MATCH_GROUP_NAME_SCHEME].Value.ToLower() : ""));
            return uriString;
        }

        /// <summary>
        /// Removes extraneous path separators and whitespace, ensures the file scheme name is lower case and escape sequence characters are upper-case.
        /// </summary>
        /// <param name="uriString">Absolute URI string to normalize.</param>
        /// <returns>The normalized URI string.</returns>
        public static string NormalizeAbsoluteFileUrl(string uriString)
        {
            if (string.IsNullOrEmpty(uriString))
                return "";
            if (ABS_URI_STRING_NORMALIZE_REGEX.IsMatch(uriString))
                return ABS_URI_STRING_NORMALIZE_REGEX.Replace(uriString, m =>
                    (m.Groups[MATCH_GROUP_NAME_CASE].Success) ? m.Value.ToUpper() :
                    ((m.Groups[MATCH_GROUP_NAME_SCHEME].Success) ? m.Groups[MATCH_GROUP_NAME_SCHEME].Value.ToLower() : ""));
            return uriString;
        }

        /// <summary>
        /// Removes extraneous path separators and whitespace and ensures escape sequence characters are upper-case.
        /// </summary>
        /// <param name="uriPathString">Encoded URI path string to normalize.</param>
        /// <param name="wasChanged">Returns <see langword="true"/> if the <paramref name="uriPathString"/> was normalized or <see langword="false"/>
        /// if <paramref name="uriPathString"/> did not need to be normalized.</param>
        /// <returns>The normalized URI string.</returns>
        public static string NormalizeRelativeFileUri(string uriPathString, out bool wasChanged)
        {
            if (uriPathString is null)
            {
                wasChanged = true;
                return "";
            }

            wasChanged = uriPathString.Length > 0 && REL_URI_STRING_NORMALIZE_REGEX.IsMatch(uriPathString);
            if (wasChanged)
                return REL_URI_STRING_NORMALIZE_REGEX.Replace(uriPathString, m =>
                    (m.Groups[MATCH_GROUP_NAME_CASE].Success) ? m.Value.ToUpper() :
                    ((m.Groups[MATCH_GROUP_NAME_SCHEME].Success) ? m.Groups[MATCH_GROUP_NAME_SCHEME].Value.ToLower() : ""));
            return uriPathString;
        }

        /// <summary>
        /// Removes extraneous path separators and whitespace and ensures escape sequence characters are upper-case.
        /// </summary>
        /// <param name="uriString">Encoded URI path string to normalize.</param>
        /// <returns>The normalized URI string.</returns>
        public static string NormalizeRelativeFileUri(string uriPathString)
        {
            if (string.IsNullOrEmpty(uriPathString))
                return "";
            if (URI_ABS_PATH_SEPARATOR_NORMALIZE_REGEX.IsMatch(uriPathString))
                return URI_ABS_PATH_SEPARATOR_NORMALIZE_REGEX.Replace(uriPathString, m =>
                    (m.Groups[MATCH_GROUP_NAME_CASE].Success) ? m.Value.ToUpper() :
                    ((m.Groups[MATCH_GROUP_NAME_SCHEME].Success) ? m.Groups[MATCH_GROUP_NAME_SCHEME].Value.ToLower() : ""));
            return uriPathString;
        }

        /// <summary>
        /// Converts path characters that are valid for the target OS type, but not for file URI path string, to their URI encoded form so the target string value
        /// can be successfully parsed as a <seealso cref="Uri.UriSchemeFile">file</seealso> URI string.
        /// </summary>
        /// <param name="uriString">The URI-encoded path string. If a <see langword="null"/> value is provided, an empty string is returned.</param>
        /// <param name="wasChanged">Returns <see langword="true"/> if any characters were encoded; otherwise <see langword="false"/> to indicate that the
        /// string value is unchanged.</param>
        /// <returns>The URI-encoded path string with special path characters encoded as well.</returns>
        public abstract string EscapeSpecialPathChars(string uriString, out bool wasChanged);

        /// <summary>
        /// Converts path characters that are valid for the target OS type, but not for file URI path string, to their URI encoded form so the target string value
        /// can be successfully parsed as a <seealso cref="Uri.UriSchemeFile">file</seealso> URI string.
        /// </summary>
        /// <param name="uriString">The URI-encoded path string. If a <see langword="null"/> value is provided, an empty string is returned.</param>
        /// <returns>The URI-encoded path string with special path characters encoded as well.</returns>
        public abstract string EscapeSpecialPathChars(string uriString);

        /// <summary>
        /// Indicates whether a URI-encoded path string contains characters which are acceptable for the current OS type but should be URI-encoded.
        /// </summary>
        /// <param name="uriString">The URI-encoded path string.</param>
        /// <returns><see langword="true"/> if the <paramref name="uriString"/> contains one or more characters which need to be URI encoded,
        /// but would not get encoded using <seealso cref="Uri.EscapeUriString(string)"/>; otherwise, <see langword="false"/>.</returns>
        public abstract bool ContainsSpecialPathChars(string uriString);

        /// <summary>
        /// Ensures that a string is a well-formed file URI that is compatible with the specified platform type.
        /// </summary>
        /// <param name="uriString">The input URI string.</param>
        /// <param name="kind">The expected URI type. If <seealso cref="UriKind.Absolute"/> is used, then the local filesystem equivalent should be a absolute reference as well.</param>
        /// <param name="platform">The target platform type.</param>
        /// <param name="includeAltPlatformType">If <see langword="true"/> and the <paramref name="uriString"/> is not compatible with the target
        /// <paramref name="platform"/> type, this will ensure the <paramref name="uriString"/> is compatible with the the alternative platform type.</param>
        /// <returns>A well-formed file URI that is compatible with the specified platform type.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="uriString"/> type does not match the specified <paramref name="kind"/> or
        /// is not compatible with the specified platform type.</exception>
        public static string EnsureWellFormedUri(string uriString, UriKind kind, PlatformType platform, bool includeAltPlatformType = false)
        {
            if (includeAltPlatformType)
            {
                FileUriConverter currentFactory = GetFactory(platform, out FileUriConverter altFactory);
                if (currentFactory.IsWellFormedUriString(uriString, kind) || altFactory.IsWellFormedUriString(uriString, kind))
                    return uriString;
                return currentFactory.EnsureWellFormedUriString(uriString, kind);
            }
            return GetFactory(platform).EnsureWellFormedUriString(uriString, kind);
        }

        /// <summary>
        /// Ensures that a string is a well-formed file URI that is compatible with the current platform type.
        /// </summary>
        /// <param name="uriString">The input URI string.</param>
        /// <param name="kind">The expected URI type.</param>
        /// <param name="wasChanged">Returns <see langword="true"/> if any characters were encoded or normalized; otherwise <see langword="false"/> to indicate that the
        /// string value is unchanged.</param>
        /// <returns>A well-formed file URI that is compatible with the specified platform type.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="uriString"/> type does not match the specified <paramref name="kind"/> or
        /// is not compatible with the current platform type.</exception>
        public string EnsureWellFormedUriString(string uriString, UriKind kind, out bool wasChanged)
        {
            if (uriString is null)
            {
                if (kind == UriKind.Absolute)
                    throw new ArgumentOutOfRangeException(nameof(uriString));
                wasChanged = true;
                return "";
            }
            if (uriString.Length == 0)
            {
                if (kind == UriKind.Absolute)
                    throw new ArgumentOutOfRangeException(nameof(uriString));
                wasChanged = false;
                return uriString;
            }

            Match match = UriHostAndPathStrictRegex.Match(uriString);
            if (match.Success)
                wasChanged = match.Length < uriString.Length;
            else
            {
                if ((match = UriValidationRegex.Match(uriString)).Success)
                {
                    if (match.Groups[MATCH_GROUP_NAME_FILE].Success)
                        uriString = NormalizeAbsoluteFileUrl(uriString, out wasChanged);
                    else
                        uriString = NormalizeRelativeFileUri(uriString, out wasChanged);
                }
                else
                {
                    uriString = EscapeSpecialPathChars(uriString, out wasChanged);
                    if (wasChanged && (match = UriValidationRegex.Match(uriString)).Success)
                    {
                        if (match.Groups[MATCH_GROUP_NAME_FILE].Success)
                            uriString = NormalizeAbsoluteFileUrl(uriString);
                        else
                            uriString = NormalizeRelativeFileUri(uriString);
                    }
                }
                if (!(wasChanged && (match = UriHostAndPathStrictRegex.Match(uriString)).Success))
                    throw new ArgumentOutOfRangeException(nameof(uriString));
            }

            switch (kind)
            {
                case UriKind.Absolute:
                    if (!(match.Groups[MATCH_GROUP_NAME_HOST].Success || match.Groups[MATCH_GROUP_NAME_ROOT].Success))
                        throw new ArgumentOutOfRangeException(nameof(uriString));
                    break;
                case UriKind.Relative:
                    if (match.Groups[MATCH_GROUP_NAME_HOST].Success || match.Groups[MATCH_GROUP_NAME_ROOT].Success)
                        throw new ArgumentOutOfRangeException(nameof(uriString));
                    break;
            }
            return match.Value;
        }

        /// <summary>
        /// Ensures that a string is a well-formed file URI that is compatible with the current platform type.
        /// </summary>
        /// <param name="uriString">The input URI string.</param>
        /// <param name="kind">The expected URI type.</param>
        /// <returns>A well-formed file URI that is compatible with the specified platform type.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="uriString"/> type does not match the specified <paramref name="kind"/> or
        /// is not compatible with the current platform type.</exception>
        public string EnsureWellFormedUriString(string uriString, UriKind kind)
        {
            if (string.IsNullOrEmpty(uriString))
            {
                if (kind == UriKind.Absolute)
                    throw new ArgumentOutOfRangeException(nameof(uriString));
                return "";
            }

            Match match = UriHostAndPathStrictRegex.Match(uriString);
            if (!match.Success)
            {
                bool wasChanged;
                if ((match = UriValidationRegex.Match(uriString)).Success)
                {
                    if (match.Groups[MATCH_GROUP_NAME_FILE].Success)
                        uriString = NormalizeAbsoluteFileUrl(uriString, out wasChanged);
                    else
                        uriString = NormalizeRelativeFileUri(uriString, out wasChanged);
                }
                else
                {
                    uriString = EscapeSpecialPathChars(uriString, out wasChanged);
                    if (wasChanged && (match = UriValidationRegex.Match(uriString)).Success)
                    {
                        if (match.Groups[MATCH_GROUP_NAME_FILE].Success)
                            uriString = NormalizeAbsoluteFileUrl(uriString);
                        else
                            uriString = NormalizeRelativeFileUri(uriString);
                    }
                }
                if (!(wasChanged && (match = UriHostAndPathStrictRegex.Match(uriString)).Success))
                    throw new ArgumentOutOfRangeException(nameof(uriString));
            }

            switch (kind)
            {
                case UriKind.Absolute:
                    if (!(match.Groups[MATCH_GROUP_NAME_HOST].Success || match.Groups[MATCH_GROUP_NAME_ROOT].Success))
                        throw new ArgumentOutOfRangeException(nameof(uriString));
                    break;
                case UriKind.Relative:
                    if (match.Groups[MATCH_GROUP_NAME_HOST].Success || match.Groups[MATCH_GROUP_NAME_ROOT].Success)
                        throw new ArgumentOutOfRangeException(nameof(uriString));
                    break;
            }
            return match.Value;
        }

        /// <summary>
        /// Determines if a <seealso cref="Uri.UriSchemeFile">file</seealso> URL is well-formed and compatible with the target filesystem type.
        /// </summary>
        /// <param name="uriString">The URI string to test.</param>
        /// <param name="kind">The URI type to test for. If <seealso cref="UriKind.Absolute"/> is used, then the local filesystem equivalent should be a absolute reference as well.</param>
        /// <returns><see langword="true"/> if the <paramref name="uriString"/> is well-formed and the decoded string is compatible with the target filesystem type;
        /// otherwise, <see langword="false"/>.</returns>
        public bool IsWellFormedUriString(string uriString, UriKind kind)
        {
            if (string.IsNullOrEmpty(uriString))
                return kind != UriKind.Absolute;

            Match match = UriHostAndPathStrictRegex.Match(uriString);
            if (!match.Success)
                return false;
            switch (kind)
            {
                case UriKind.Absolute:
                    return match.Groups[MATCH_GROUP_NAME_HOST].Success || match.Groups[MATCH_GROUP_NAME_ROOT].Success;
                case UriKind.Relative:
                    return !(match.Groups[MATCH_GROUP_NAME_HOST].Success || match.Groups[MATCH_GROUP_NAME_ROOT].Success);
            }
            return true;
        }

        /// <summary>
        /// Determines if a path string is valid for the target filesystem type.
        /// </summary>
        /// <param name="fileSystemPath">The filesystem path to test.</param>
        /// <param name="kind">The kind of path.</param>
        /// <returns><see langword="true"/> if the <paramref name="fileSystemPath"/> is valid for the target filesystem and matches the specified type;
        /// otherwise, <see langword="false"/>.</returns>
        public bool IsValidFileSystemPath(string fileSystemPath, FsPathKind kind)
        {
            if (string.IsNullOrEmpty(fileSystemPath))
                return false;
            Match match = FsHostAndPathRegex.Match(fileSystemPath);
            if (match.Success)
                switch (kind)
                {
                    case FsPathKind.Absolute:
                        return match.Groups[MATCH_GROUP_NAME_ROOT].Success || match.Groups[MATCH_GROUP_NAME_HOST].Success;
                    case FsPathKind.Local:
                        return match.Groups[MATCH_GROUP_NAME_ROOT].Success;
                    case FsPathKind.UNC:
                        return match.Groups[MATCH_GROUP_NAME_HOST].Success;
                    case FsPathKind.Relative:
                        return !(match.Groups[MATCH_GROUP_NAME_ROOT].Success || match.Groups[MATCH_GROUP_NAME_HOST].Success);
                    default:
                        return true;
                }
            return false;
        }

        /// <summary>
        /// Determines if a path string is valid for the target filesystem type.
        /// </summary>
        /// <param name="fileSystemPath">The filesystem path to test.</param>
        /// <param name="type">The path type to check for.</param>
        /// <returns><see langword="true"/> if the <paramref name="fileSystemPath"/> is valid for the target filesystem and matches the
        /// specified <paramref name="type"/>; otherwise, <see langword="false"/>.</returns>
        public bool IsValidFileSystemPath(string fileSystemPath, FsPathType type)
        {
            if (string.IsNullOrEmpty(fileSystemPath))
                return false;
            Match match = FsHostAndPathRegex.Match(fileSystemPath);
            if (match.Success)
                switch (type)
                {
                    case FsPathType.Local:
                        return match.Groups[MATCH_GROUP_NAME_ROOT].Success;
                    case FsPathType.UNC:
                        return match.Groups[MATCH_GROUP_NAME_HOST].Success;
                    default:
                        return !(match.Groups[MATCH_GROUP_NAME_ROOT].Success || match.Groups[MATCH_GROUP_NAME_HOST].Success);
                }
            return false;
        }

        /// <summary>
        /// Determines if a path string is an absolute path string that is valid for the target filesystem type.
        /// </summary>
        /// <param name="fileSystemPath">The filesystem path to test.</param>
        /// <returns><see langword="true"/> if the <paramref name="fileSystemPath"/> is valid for the target filesystem and is an absolute local or
        /// UNC path string; otherwise, <see langword="false"/>.</returns>
        public bool IsFileSystemPathAbsolute(string fileSystemPath)
        {
            if (string.IsNullOrEmpty(fileSystemPath))
                return false;
            Match match = FsHostAndPathRegex.Match(fileSystemPath);
            return match.Success && (match.Groups[MATCH_GROUP_NAME_ROOT].Success || match.Groups[MATCH_GROUP_NAME_HOST].Success);
        }

        /// <summary>
        /// Determines if a path string is an absolute local filesystem (not a UNC path) path string that is valid for the target filesystem type.
        /// </summary>
        /// <param name="fileSystemPath">The filesystem path to test.</param>
        /// <returns><see langword="true"/> if the <paramref name="fileSystemPath"/> is valid for the target filesystem and is an absolute local path string;
        /// otherwise, <see langword="false"/>.</returns>
        public bool IsFileSystemPathLocal(string fileSystemPath)
        {
            if (string.IsNullOrEmpty(fileSystemPath))
                return false;
            Match match = FsHostAndPathRegex.Match(fileSystemPath);
            return match.Success && match.Groups[MATCH_GROUP_NAME_ROOT].Success;
        }

        /// <summary>
        /// Gets the filesystem path type.
        /// </summary>
        /// <param name="fileSystemPath">The filesystem path to validate.</param>
        /// <param name="hostName">Returns the host name contained withing the path string or an empty string if no host name was found.</param>
        /// <param name="directoryName">The directory name portion of the path string without the UNC host name.</param>
        /// <param name="fileName">The file name portion of the path string.</param>
        /// <returns></returns>
        public FsPathType? CheckFileSytemPath(string fileSystemPath, out string hostName, out string directoryName, out string fileName)
        {
            Match m;
            if (string.IsNullOrEmpty(fileSystemPath) || !(m = FsHostAndPathRegex.Match(fileSystemPath)).Success)
            {
                hostName = directoryName = fileName = "";
                return null;
            }
            string path = m.Groups[MATCH_GROUP_NAME_PATH].Value;
            directoryName = SplitFsPathLeaf(path, out fileName);
            //directoryName = m.Groups[MATCH_GROUP_NAME_DIR].Value;
            //fileName = m.Groups[MATCH_GROUP_NAME_FILE_NAME].Value;
            if (m.Groups[MATCH_GROUP_NAME_UNC].Success)
            {
                hostName = m.Groups[MATCH_GROUP_NAME_IPV6].Value.Replace(UNC_HEX_SEPARATOR_STRING, URI_SCHEME_SEPARATOR_STRING);
                return FsPathType.UNC;
            }
            if (m.Groups[MATCH_GROUP_NAME_HOST].Success)
            {
                hostName = (m.Groups[MATCH_GROUP_NAME_IPV6].Success ? m.Groups[MATCH_GROUP_NAME_IPV6] : m.Groups[MATCH_GROUP_NAME_HOST]).Value;
                return FsPathType.UNC;
            }
            hostName = "";
            return (m.Groups[MATCH_GROUP_NAME_ROOT].Success) ? FsPathType.Local : FsPathType.Relative;
        }

        /// <summary>
        /// Gets the filesystem path type.
        /// </summary>
        /// <param name="fileSystemPath">The filesystem path to validate.</param>
        /// <param name="hostName">Returns the host name contained withing the path string or an empty string if no host name was found.</param>
        /// <param name="path">Returns the path string without the UNC host name.</param>
        /// <returns>The <seealso cref="FsPathType"/> indicating the path type or <see langword="null"/> if the <paramref name="fileSystemPath"/> is invalid.</returns>
        public FsPathType? CheckFileSytemPath(string fileSystemPath, out string hostName, out string path)
        {
            if (string.IsNullOrEmpty(fileSystemPath))
            {
                hostName = path = "";
                return null;
            }
            fileSystemPath = FsFullPathNormalizeRegex.Replace(fileSystemPath, "");
            Match m;
            if (string.IsNullOrEmpty(fileSystemPath) || !(m = FsHostAndPathRegex.Match(fileSystemPath)).Success)
            {
                hostName = path = "";
                return null;
            }
            path = m.Groups[MATCH_GROUP_NAME_PATH].Value;
            if (m.Groups[MATCH_GROUP_NAME_UNC].Success)
            {
                hostName = m.Groups[MATCH_GROUP_NAME_IPV6].Value.Replace(UNC_HEX_SEPARATOR_STRING, URI_SCHEME_SEPARATOR_STRING);
                return FsPathType.UNC;
            }
            if (m.Groups[MATCH_GROUP_NAME_HOST].Success)
            {
                hostName = (m.Groups[MATCH_GROUP_NAME_IPV6].Success ? m.Groups[MATCH_GROUP_NAME_IPV6] : m.Groups[MATCH_GROUP_NAME_HOST]).Value;
                return FsPathType.UNC;
            }
            hostName = "";
            return (m.Groups[MATCH_GROUP_NAME_ROOT].Success) ? FsPathType.Local : FsPathType.Relative;
        }

        /// <summary>
        /// Gets the filesystem path type.
        /// </summary>
        /// <param name="fileSystemPath">The filesystem path to validate.</param>
        /// <param name="hostName">Returns the host name contained withing the path string or an empty string if no host name was found.</param>
        /// <returns>The <seealso cref="FsPathType"/> indicating the path type or <see langword="null"/> if the <paramref name="fileSystemPath"/> is invalid.</returns>
        public FsPathType? CheckFileSytemPath(string fileSystemPath, out string hostName)
        {
            Match m;
            if (string.IsNullOrEmpty(fileSystemPath) || !(m = FsHostAndPathRegex.Match(fileSystemPath)).Success)
            {
                hostName = "";
                return null;
            }
            if (m.Groups[MATCH_GROUP_NAME_UNC].Success)
            {
                hostName = m.Groups[MATCH_GROUP_NAME_IPV6].Value.Replace(UNC_HEX_SEPARATOR_STRING, URI_SCHEME_SEPARATOR_STRING);
                return FsPathType.UNC;
            }
            if (m.Groups[MATCH_GROUP_NAME_HOST].Success)
            {
                hostName = (m.Groups[MATCH_GROUP_NAME_IPV6].Success ? m.Groups[MATCH_GROUP_NAME_IPV6] : m.Groups[MATCH_GROUP_NAME_HOST]).Value;
                return FsPathType.UNC;
            }
            hostName = "";
            return (m.Groups[MATCH_GROUP_NAME_ROOT].Success) ? FsPathType.Local : FsPathType.Relative;
        }

        /// <summary>
        /// Gets the filesystem path type.
        /// </summary>
        /// <param name="fileSystemPath">The filesystem path to validate.</param>
        /// <returns>The <seealso cref="FsPathType"/> indicating the path type or <see langword="null"/> if the <paramref name="fileSystemPath"/> is invalid.</returns>
        public FsPathType? CheckFileSytemPath(string fileSystemPath)
        {
            Match m;
            if (string.IsNullOrEmpty(fileSystemPath) || !(m = FsHostAndPathRegex.Match(fileSystemPath)).Success)
                return null;
            if (m.Groups[MATCH_GROUP_NAME_UNC].Success)
                return FsPathType.UNC;
            if (m.Groups[MATCH_GROUP_NAME_HOST].Success)
                return FsPathType.UNC;
            return (m.Groups[MATCH_GROUP_NAME_ROOT].Success) ? FsPathType.Local : FsPathType.Relative;
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

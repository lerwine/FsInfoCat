using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FsInfoCat.Util
{
    public class LinuxFileUriConverter : FileUriConverter
    {
        public static readonly LinuxFileUriConverter INSTANCE = new LinuxFileUriConverter();

        /*
         * Patterns
         * Any Name Validation: ([^\u0000-\u0019\u007f-\u00a0\u1680\u2028\u2029\ud800-\udfff/%]+|%((?![a-f\d]{2})|0[1-9a-f]|[1-9a-f]))+
         * Any Name Match: ([^\u0000-\u0019/%]+|%((?![a-f\d]{2})|0[1-9a-f]|[1-9a-f]))+
         * FS Name: [^\u0000/]+
         * Encoded Name: ([!$=&-.:;=@[\]\w]+|%(0[1-9a-f]|[1-9a-f][\da-f]))+
         */

        //public const string PATTERN_LOCAL_FS_NAME = @"^[^\u0000/]+$";

        ///// <summary>
        ///// Matches a well-formed, absolute local path on the typical Linux filesystem.
        ///// </summary>
        //public const string PATTERN_ABS_FS_PATH = @"^(?i)\s*(//((?=\d+(\.\d+){3})((2(5[0-5|[0-4]?\d?)?|[01]?\d\d?)(\.|(?![.\d]))){4}|(?=\[[a-f\d]*(:[a-f\d]*){2,7}\]|[a-f\d]*(:[a-f\d]*){2,7}|[a-f\d]*(-[a-f\d]*){2,7}\.ipv6-literal\.net)\[?([a-f\d]{1,4}([:-][a-f\d]{1,4}){7}|(([a-f\d]{1,4}[:-])+|[:-])([:-][a-f\d]{1,4})+)(\.ipv6-literal\.net)?\]?|(?=[^\s/]{1,255}\s*$)[a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?)(?=/|$)|(?=/))(/[^\u0000/]+)*/?\s*$";

        //        /// <summary>
        //        /// Matches a well-formed, absolute local path on the typical Linux filesystem.
        //        /// </summary>
        //        /// <remarks>Named group match meanings:
        //        /// <list type="bullet">
        //        /// <item><term>host</term> Matches the host name. This implies that the text is a UNC path.
        //        /// <list type="bullet">
        //        /// <item><term>ipv4</term> Matches an IPV4 host name. This implies that the text is a UNC path.</item>
        //        /// <item><term>ipv6</term> Matches an IPV6 host name. This implies that the text is a UNC path.
        //        /// <list type="bullet">
        //        /// <item><term>d</term> Matches the domain of an IPV6 address. This implies that the text is a UNC path.</item>
        //        /// </list></item>
        //        /// <item><term>dns</term> Matches a DNS host name. This implies that the text is a UNC path.</item>
        //        /// </list></item>
        //        /// <item><term>path</term> Matches the path string.
        //        /// <list type="bullet">
        //        /// <item><term>dir</term> Matches the parent directory path. This group will always succeed when the expression succeeds, even if it is empty.
        //        /// The trailing directory separator will be omitted unless it is the root path.</item>
        //        /// <item><term>file</term> Matches the file name. This group will only fail if the source path is the root path.</item>
        //        /// </list></item>
        //        /// </list></remarks>
        //        public static readonly Regex ABS_FS_PATH_REGEX = new Regex(@"^
        //(
        //    //
        //    (?<host>
        //        (?=\d+(\.\d+){3})
        //        (?<ipv4>((2(5[0-5|[0-4]?\d?)?|[01]?\d\d?)(\.|(?![.\d]))){4})
        //    |
        //        (?=
        //            \[[a-f\d]*(:[a-f\d]*){2,7}\]
        //        |
        //            [a-f\d]*
        //            (:[a-f\d]*){2,7}
        //        |
        //            [a-f\d]*
        //            (-[a-f\d]*){2,7}
        //            \.ipv6-literal\.net
        //        )
        //        \[?
        //        (?<ipv6>
        //            [a-f\d]{1,4}
        //            ([:-][a-f\d]{1,4}){7}
        //        |
        //            (
        //                ([a-f\d]{1,4}[:-])+
        //            |
        //                [:-]
        //            )
        //            ([:-][a-f\d]{1,4})+
        //        )
        //        (?<d>\.ipv6-literal\.net)?
        //        \]?
        //    |
        //        (?=[^\s/]{1,255}\s*$)
        //        (?<dns>
        //            [a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?
        //        )
        //    )
        //    (?=/|$)
        //|
        //    (?=/)
        //)
        //(?<path>
        //    (?<dir>
        //        (
        //            (?=(/[^/]+/?)?$)/
        //        |
        //            (
        //                (?=/[^/]+/[^/])
        //                /
        //                [^\u0000/]+
        //            )*
        //        )
        //    )
        //    (
        //        /?
        //        (?<file>[^\u0000/]+)
        //    )?
        //)
        ///?$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

        /// <summary>
        /// Matches a well-formed relative or absolute path on the typical Linux filesystem.
        /// </summary>
        /// <remarks>Named group match meanings:
        /// <list type="bullet">
        ///     <item><term>host</term> Matches the host name. This implies that the text is an absolute UNC path
        ///             (<see cref="FileUriConverter.MATCH_GROUP_NAME_HOST"/>).
        ///         <list type="bullet">
        ///             <item><term>ipv4</term> Matches an <seealso cref="UriHostNameType.IPv4"/> host name. This implies that the text is an absolute UNC path
        ///                  (<see cref="FileUriConverter.MATCH_GROUP_NAME_IPV4"/>).</item>
        ///             <item><term>ipv6</term> Matches an IPV6 host name. This implies that the text is an absolute UNC path
        ///                     (<see cref="FileUriConverter.MATCH_GROUP_NAME_IPV6"/>).
        ///                 <list type="bullet">
        ///                     <item><term>unc</term> Matches the domain of an IPV6 address.
        ///                         This implies that the text is an absolute UNC path (<see cref="FileUriConverter.MATCH_GROUP_NAME_UNC"/>).</item>
        ///                 </list>
        ///             </item>
        ///             <item><term>dns</term> Matches a <seealso cref="UriHostNameType.Dns"/> or <seealso cref="UriHostNameType.Basic"/> host name. This implies that the text is an absolute UNC path
        ///                 (<see cref="FileUriConverter.MATCH_GROUP_NAME_DNS"/>).</item>
        ///         </list>
        ///     </item>
        ///     <item><term>path</term> Matches the path string. (<see cref="FileUriConverter.MATCH_GROUP_NAME_PATH"/>)
        ///         <list type="bullet">
        ///             <item><term>dir</term> Matches the parent directory path. This group will always succeed when the expression succeeds,
        ///                 even if it is empty. The trailing slash will be omitted unless it is the root path
        ///                 (<see cref="FileUriConverter.MATCH_GROUP_NAME_DIR"/>).</item>
        ///             <item><term>root</term> Matches leading directory separator character. This implies that the text is
        ///                 an absolute local path (<see cref="FileUriConverter.MATCH_GROUP_NAME_ROOT"/>).</item>
        ///             <item><term>fileName</term> Matches the file name. This group will only fail if the source path is the root path
        ///                 (<see cref="FileUriConverter.MATCH_GROUP_NAME_FILE_NAME"/>).</item>
        ///         </list>
        ///     </item>
        /// </list></remarks>
        public static readonly Regex FS_PATH_REGEX = new Regex(@"^
(
    //
    (?<host>
        (?=(\d+\.){3}\d+)
        (?<ipv4>((2(5[0-5|[0-4]?\d?)?|[01]?\d\d?)(\.|(?![.\d]))){4})
    |
        (?=\[[^:]*(:[^:]*){2,7}\]|[^:]*(:[^:]*){2,7}|[^-]*(-[^-]*){2,7}\.ipv6-literal\.net)
        \[?
        (?<ipv6>[a-f\d]{1,4}([:-][a-f\d]{1,4}){7}|(([a-f\d]{1,4}[:-])+|[:-])([:-][a-f\d]{1,4})+)
        (?<unc>\.ipv6-literal\.net)?
        \]?
    |
        (?=[^\s/]{1,255}(?![\w-]))
        (?<dns>[a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?)
    )
    (?=/|$)
)?
(?<path>
    (?<dir>
        (?<root>/)?
        (
            (?=[^/]+/[^/])[^\u0000/]+
            (
                (?=/[^/]+/[^/])
                /
                [^\u0000/]+
            )*
        )?
    )
    (
        /?
        (?<fileName>[^\u0000/]+)
    )?
)
/?$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

        /// <summary>
        /// Regular expression pattern that matches a string that can be parsed as an absolute <seealso cref="Uri.UriSchemeFile">file</seealso> URI or an absolute
        /// filesystem path, and does not contain any characters which are not compatible with the Linux filesystem.
        /// </summary>
        /// <remarks>Only the host name needs to be well-formed to match this expression.</remarks>
        public const string PATTERN_ABSOLUTE_FILESYSTEM_OR_LAX_FILE_URI = @"(?i)^\s*((file:)?//((?=(\d+\.){3}\d+)((2(5[0-5|[0-4]?\d?)?|[01]?\d\d?)(\.|(?![.\d]))){4}|(?=\[[a-f\d]*(:[a-f\d]*){2,7}\]|[a-f\d]*(:[a-f\d]*){2,7}$|[a-f\d]*(-[a-f\d]*){2,7}\.ipv6-literal\.net$)\[?([a-f\d]{1,4}([:-][a-f\d]{1,4}){7}|(([a-f\d]{1,4}[:-])+|[:-])(([:-][a-f\d]{1,4})+|[:-]))(\]|\.ipv6-literal\.net)?|(?=[^\s/]{1,255}(?![\w-]))[a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?)(?=/|\s*$)|(file://)?(?=/))(/(?=\s*$)|(/([^\u0000-\u0019/%]+|%((?![a-f\d]{2})|0[1-9a-f]|[1-9a-f]))+)*)/?\s*$";

        public static readonly Regex TEMP_REGEX = new Regex(@"

", RegexOptions.Compiled);

        /// <summary>
        /// Matches a string that can be parsed as an absolute <seealso cref="Uri.UriSchemeFile">file</seealso> URI or an absolute filesystem path, and does not contain
        /// any characters which are not compatible with the Linux filesystem.
        /// </summary>
        /// <remarks>Only the host name needs to be well-formed to match this expression.
        /// <para>Named group match meanings:
        /// <list type="bullet">
        ///     <item><term>file</term> Matches the <seealso cref="Uri.UriSchemeFile">file</seealso> uri scheme.
        ///         (<see cref="FileUriConverter.MATCH_GROUP_NAME_SCHEME"/>).</item>
        ///     <item><term>host</term> Matches the host name. This implies that the text is an absolute UNC path
        ///             (<see cref="FileUriConverter.MATCH_GROUP_NAME_HOST"/>).
        ///         <list type="bullet">
        ///             <item><term>ipv4</term> Matches an IPV4 host name. This implies that the text is an absolute UNC path
        ///                  (<see cref="FileUriConverter.MATCH_GROUP_NAME_IPV4"/>).</item>
        ///             <item><term>ipv6</term> Matches an IPV6 host name. This implies that the text is an absolute UNC path
        ///                     (<see cref="FileUriConverter.MATCH_GROUP_NAME_IPV6"/>).
        ///                 <list type="bullet">
        ///                     <item><term>unc</term> Matches the domain of an IPV6 address.
        ///                         This implies that the text is an absolute UNC path (<see cref="FileUriConverter.MATCH_GROUP_NAME_UNC"/>).</item>
        ///                 </list>
        ///             </item>
        ///             <item><term>dns</term> Matches a DNS host name. This implies that the text is an absolute UNC path
        ///                 (<see cref="FileUriConverter.MATCH_GROUP_NAME_DNS"/>).</item>
        ///         </list>
        ///     </item>
        ///     <item><term>path</term> Matches the path string. (<see cref="FileUriConverter.MATCH_GROUP_NAME_PATH"/>)
        ///         <list type="bullet">
        ///             <item><term>dir</term> Matches the parent directory path. This group will always succeed when the expression succeeds,
        ///                 even if it is empty. The trailing slash will be omitted unless it is the root path
        ///                 (<see cref="FileUriConverter.MATCH_GROUP_NAME_DIR"/>).</item>
        ///             <item><term>root</term> Matches leading directory separator character. This implies that the text is
        ///                 an absolute local path (<see cref="FileUriConverter.MATCH_GROUP_NAME_ROOT"/>).</item>
        ///             <item><term>fileName</term> Matches the file name. This group will only fail if the source path is the root path
        ///                 (<see cref="FileUriConverter.MATCH_GROUP_NAME_FILE_NAME"/>).</item>
        ///         </list>
        ///     </item>
        /// </list></para></remarks>
        public static readonly Regex ABSOLUTE_FILESYSTEM_OR_LAX_FILE_URI_REGEX = new Regex(@"^
(
    ((?<file>file):)?//
    (?<host>
        (?=(\d+\.){3}\d+)
        (?<ipv4>((2(5[0-5|[0-4]?\d?)?|[01]?\d\d?)(\.|(?![.\d]))){4})
    |
        (?=\[[a-f\d]*(:[a-f\d]*){2,7}\]|[a-f\d]*(:[a-f\d]*){2,7}$|[a-f\d]*(-[a-f\d]*){2,7}\.ipv6-literal\.net$)\[?(?<ipv6>[a-f\d]{1,4}([:-][a-f\d]{1,4}){7}|(([a-f\d]{1,4}[:-])+|[:-])(([:-][a-f\d]{1,4})+|[:-]))(\]|(?<unc>\.ipv6-literal\.net))?
    |
        (?=[^\s/]{1,255}(?![\w-]))
        (?<dns>[a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?)
    )
    (?=/|$)
|
    ((?<file>file)://)?(?=/)
)
(?<path>
    (
        /(?=$)
    |
        (
            /
            ([^\u0000-\u0019/%]+|%((?![a-f\d]{2})|0[1-9a-f]|[1-9a-f]))+
        )*
    )
)
/?$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

        /// <summary>
        /// <seealso cref="Regex"/> that can be used to guess the format of a string from the perspective of a typical Linux file system.
        /// </summary>
        /// <remarks>Named group match meanings:
        /// <list type="bullet">
        /// <item><term>file</term> Text appears to be a file URL.</item>
        /// <item><term>unc</term> Text appears to be UNC notation.</item>
        /// <item><term>host</term> Host name.</item>
        /// <item><term>path</term> Absolute path.</item>
        /// <item><term>scheme</term> Matches the URI scheme.</item>
        /// <item>Any group match may succeeed independently of other groups.</item>
        /// <item>At least one group will match if the entire expression matches.</item>
        /// <item>Groups <c>file</c> and <c>unc</c> are exclusive of each other.</item>
        /// <item>If the expression matches, but groups <c>file</c>, <c>path</c> and <c>unc</c> do not, then <c>scheme</c> will match the non-file URI scheme.</item>
        /// <item>If the expression does not match at all, then the text is assumed to be a non-rooted relative path.</item>
        /// </list></remarks>
        public static readonly Regex FORMAT_DETECTION_REGEX = new Regex(@"^
(
    (?<file>
        (?<scheme>file):[\\/]{2}
        (/(?<host>[^/]+))?
        (?<path>/.*)?
    )
|
    (?<unc>
        //
        (?<host>[^/]+)
        (?<path>(/.*)?)
    )
|
    (?<path>/(?!/).*)
|
    (?!file:)
    (?<scheme>[a-z][\w-]+):
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
)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Matches a well-formed URI that can be converted to a valid absolute or relative local path on a typical Linux filesystem.
        /// </summary>
        /// <remarks>Named group match meanings:
        /// <list type="bullet">
        /// <item><term>host</term> Matches the host name. This implies that the text is an absolute UNC path.
        /// <list type="bullet">
        /// <item><term>ipv4</term> Matches an IPV4 host name. This implies that the text is an absolute UNC path.</item>
        /// <item><term>ipv6</term> Matches an IPV6 host name. This implies that the text is an absolute UNC path.
        /// <list type="bullet">
        /// <item><term>d</term> Matches the domain of an IPV6 address. This implies that the text is an absolute UNC path.</item>
        /// </list></item>
        /// <item><term>dns</term> Matches a DNS host name. This implies that the text is an absolute UNC path.</item>
        /// </list></item>
        /// <item><term>path</term> Matches the path string.
        /// <list type="bullet">
        /// <item><term>dir</term> Matches the parent directory path. This group will always succeed when the expression succeeds, even if it is empty.
        /// The trailing directory separator will be omitted unless it is the root path.</item>
        /// <item><term>fileName</term> Matches the file name. This group will only fail if the source path is the root path.</item>
        /// </list></item>
        /// </list></remarks>
        public static readonly Regex FILE_URI_STRICT_REGEX = new Regex(@"^
(
    (?!file://)
|
    (?<file>
 
        file://
        (
            (?i)
            (?<host>
                (?=(\d+\.){3}\d+)
                (?<ipv4>((2(5[0-5|[0-4]?\d?)?|[01]?\d\d?)(\.|(?![.\d]))){4})
            |
                (?=\[[^:]*(:[^:]*){2,7}\]|[^:]*(:[^:]*){2,7}|[^-]*(-[^-]*){2,7}\.ipv6-literal\.net)
                \[?
                (?<ipv6>[a-f\d]{1,4}([:-][a-f\d]{1,4}){7}|(([a-f\d]{1,4}[:-])+|[:-])([:-][a-f\d]{1,4})+)
                (?<d>\.ipv6-literal\.net)?
                \]?
            |
                (?=[^\s/]{1,255}(?![\w-]))
                (?<dns>[a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?)
            )
            (?=/|$)
            |
            (?=/)
        )
    )
)
(?i)
(?<path>
    (?<dir>
        (
            (?=[^/]+/[^/])
            ([!$=&-.:;=@[\]\w]+|%(0[1-9a-f]|[1-9a-f][\da-f]))+
        )?
        (
            /(?=([^/]+/?)?$)
            |
            (
                (?=/[^/]+/[^/])
                /
                ([!$=&-.:;=@[\]\w]+|%(0[1-9a-f]|[1-9a-f][\da-f]))+
            )*
        )
    )
    (
        /?
        (?<fileName>([!$=&-.:;=@[\]\w]+|%(0[1-9a-f]|[1-9a-f][\da-f]))+)
    )?
)
/?$", RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

        private LinuxFileUriConverter() { }

        //public override Regex FormatDetectionRegex => FORMAT_DETECTION_REGEX;

        ///// <summary>
        ///// Regular expression that matches a valid host name.
        ///// </summary>
        //public override Regex HostNameRegex => HOST_NAME_REGEX;

        ///// <summary>
        ///// Matches a well-formed URI that can be converted to a valid absolute or relative local path on a typical Linux filesystem.
        ///// </summary>
        ///// <remarks>Named group match meanings:
        ///// <list type="bullet">
        ///// <item><term><seealso cref="PATH_MATCH_GROUP_NAME_ABSOLUTE">a</seealso></term> Text is an absolute file URL. If this group is not matched, then the text is a relative URL that can be used as the path of a file URL.</item>
        ///// <item><term><seealso cref="PATH_MATCH_GROUP_NAME_HOST">h</seealso></term> Matches the host name. This will never match when group <c>a</c> is not matched.</item>1:2:3:4:5:6:7:8
        ///// <item><term><seealso cref="PATH_MATCH_GROUP_NAME_PATH">p</seealso></term> Matches the path. This match will always succeed when entire expression succeeds, even if the path is an empty string.</item>
        ///// </list></remarks>
        //public override Regex FileUriStrictRegex => FILE_URI_STRICT_REGEX;

        //public override Regex LocalFsPathRegex => FS_PATH_REGEX;

        public override char LocalDirectorySeparatorChar => '/';

        public override string FsDisplayName => "Linux";

        public override PlatformType FsPlatform => PlatformType.Linux;

        protected override string EscapeSpecialPathChars(string path) => path.Replace(UriHelper.URI_QUERY_DELIMITER_STRING, UriHelper.URI_QUERY_DELIMITER_ESCAPED)
            .Replace(UriHelper.URI_FRAGMENT_DELIMITER_STRING, UriHelper.URI_FRAGMENT_DELIMITER_ESCAPED);

        protected override bool ContainsSpecialPathChars(string path) => path.Contains(UriHelper.URI_QUERY_DELIMITER_CHAR) || path.Contains(UriHelper.URI_FRAGMENT_DELIMITER_CHAR);

        /// <summary>
        /// Converts a URI-compatible host name and URI-encoded path string to a filesystem path string.
        /// </summary>
        /// <param name="host">The URI-compatible host name.</param>
        /// <param name="path">The URI-encoded path string.</param>
        /// <returns>A filesystem path string.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="host"/> and/or <paramref name="path"/> is invalid.</exception>
        public override string ToFileSystemPath(string hostName, string uriPath)
        {
            if (string.IsNullOrEmpty(hostName))
                return (string.IsNullOrEmpty(uriPath)) ? uriPath : Uri.UnescapeDataString(uriPath);
            return (string.IsNullOrEmpty(uriPath)) ? $"//{hostName}" : Uri.UnescapeDataString(uriPath);
        }

        /// <summary>
        /// Converts a local filesystem path string to an URI encoded relative path string and host name.
        /// </summary>
        /// <param name="path">The local filesystem path string to convert.</param>
        /// <param name="hostName">Returns the URI-compatible host name that was referenced within the file system <paramref name="path"/> or <seealso cref="string.Empty"/> if it did not reference a host name.</param>
        /// <returns>The URI-encoded relative path string. This will never end with a path separator (<c>/</c>) unless it represents the root subdirectory.</returns>
        public override string FromFileSystemPath(string path, out string hostName)
        {
            if (string.IsNullOrEmpty(path))
            {
                hostName = "";
                return "";
            }
            Match match = FILE_URI_STRICT_REGEX.Match(path);
            if (match.Success)
            {
                hostName = match.GetGroupValue(MATCH_GROUP_NAME_HOST);
                path = match.GetGroupValue(MATCH_GROUP_NAME_PATH);
            }
            else
                hostName = "";
            return EscapeSpecialPathChars(Uri.EscapeUriString(path.Replace(LocalDirectorySeparatorChar, URI_PATH_SEPARATOR_CHAR)));
        }

        /// <summary>
        /// Converts a local filesystem path string to an URI encoded file name, relative directory path string, and host name.
        /// </summary>
        /// <param name="path">The local filesystem path string to convert.</param>
        /// <param name="hostName">Returns the URI-compatible host name that was referenced within the file system <paramref name="path"/> or <seealso cref="string.Empty"/> if it did not reference a host name.</param>
        /// <param name="directoryName">Returns the URI-encoded relative directory path string. This will never end with a path separator (<c>/</c>) unless it represents the root subdirectory.</param>
        /// <returns>The URI-encoded file name (leaf) portion of the path string or <seealso cref="string.Empty"/> if the file system <paramref name="path"/> referenced the root subdirectory.</returns>
        public override string FromFileSystemPath(string path, out string hostName, out string directoryName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Ensures that a string is a well-formed file URI.
        /// </summary>
        /// <param name="uriString">The input URI string.</param>
        /// <param name="kind">The expected URI type.</param>
        /// <returns>A well-formed file URI.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="uriString"/> type does not match the specified <paramref name="kind"/>.</exception>
        public override string EnsureWellFormedUriString(string value, UriKind kind)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determines if a <seealso cref="Uri.UriSchemeFile">file</seealso> URL is well-formed and compatible with the target filesystem type.
        /// </summary>
        /// <param name="uriString">The URI string to test.</param>
        /// <param name="kind">The URI type to test for.</param>
        /// <returns><see langword="true"/> if the <paramref name="uriString"/> is well-formed and the decoded string is compatible with the target filesystem type;
        /// otherwise, <see langword="false"/>.</returns>
        public override bool IsWellFormedUriString(string value, UriKind kind)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Attempt to create a <seealso cref="FileUri"/> object from an absolute filesystem path string.
        /// </summary>
        /// <param name="path">The abolute filesystem path string.</param>
        /// <param name="fileUri">Returns the constructed <seealso cref="FileUri"/> object or <see langword="null"/> if the <paramref name="inputString"/>
        /// could not be parsed.</param>
        /// <returns><see langword="true"/> if the <paramref name="path"/> could be parsed as a filesystem path string;
        /// otherwise, <see langword="false"/>.</returns>
        protected override bool TryParseFsPath(string path, out FileUri fileUri)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Attempt to create a <seealso cref="FileUri"/> object from an absolute <seealso cref="Uri.UriSchemeFile">file</seealso> URL string.
        /// </summary>
        /// <param name="uriString">The absolute file URI string.</param>
        /// <param name="fileUri">Returns the constructed <seealso cref="FileUri"/> object or <see langword="null"/> if the <paramref name="inputString"/>
        /// could not be parsed.</param>
        /// <returns><see langword="true"/> if the <paramref name="uriString"/> could be parsed as an absolute <seealso cref="Uri.UriSchemeFile">file</seealso> URL;
        /// otherwise, <see langword="false"/>.</returns>
        protected override bool TryParseUriString(string uriString, out FileUri fileUri)
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
        public override bool IsWellFormedFileSystemPath(string fileSystemPath, bool absoluteOnly)
        {
            throw new NotImplementedException();
        }

        public override bool TrySplitFileUriString(string uriString, out string hostName, out string directory, out string fileName, out bool isAbsolute)
        {
            throw new NotImplementedException();
        }
    }
}

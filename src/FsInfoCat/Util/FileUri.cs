using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace FsInfoCat.Util
{
    /// <summary>
    /// Represents a file URI that can be sorted and compared.
    /// </summary>
    public class FileUri : IEquatable<FileUri>, IEquatable<Uri>, IComparable<FileUri>
    {
        delegate string GetHostAndPath(string localPath, out string path);
        public const char UriPathSeparatorChar = '/';
        public const string FileSchemaWithSeparator = "file://";
        public static bool HasAltDirectorySeparatorChar { get; }
        public static bool DirectorySeparatorSameAsUriPathSeparator { get; }
        private static readonly Func<string, string> _toLocalPath;
        private static readonly GetHostAndPath _getPathAndHost;
        /// <summary>
        /// URI encode host name.
        /// </summary>
        public string Host { get; }

        /// <summary>
        /// URI path segments.
        /// </summary>
        public ReadOnlyCollection<string> Segments { get; }

        /// <summary>
        /// Indicates whether the path represents a subdirectory (ends with <c>/</c>)
        /// </summary>
        public bool IsDirectory { get; }

        /// <summary>
        /// Indicates whether the path represents a subdirectory (<see cref="Segments"/>[0] is <c>/</c>)
        /// </summary>
        public bool IsAbsolute { get; }

        /// <summary>
        /// Indicates whether this is an empty file URI (lengths of <see cref="Segments"/> and <see cref="Host"/> are zero).
        /// </summary>
        public bool IsEmpty { get; }

        /// <summary>
        /// Creates a <see cref="FileUri"/> instance representing the parent directory.
        /// </summary>
        /// <returns>A <see cref="FileUri"/> instance representing the parent directory or <c>null</c> if there is no parent.</returns>
        public FileUri GetParentUri()
        {
            if (Segments.Count < 2)
                return null;
            return new FileUri(Host, Segments.Take(Segments.Count - 1));
        }

        static FileUri()
        {
            HasAltDirectorySeparatorChar = Path.AltDirectorySeparatorChar != Path.DirectorySeparatorChar;
            DirectorySeparatorSameAsUriPathSeparator = Path.DirectorySeparatorChar == UriPathSeparatorChar;
            if (DirectorySeparatorSameAsUriPathSeparator)
            {
                _toLocalPath = p => Uri.UnescapeDataString(p);
                _getPathAndHost = (string u, out string p) =>
                {
                    u = Uri.EscapeUriString(u);
                    if (u.Length > 2 && u[0] == UriPathSeparatorChar && u[1] == UriPathSeparatorChar)
                    {
                        int i = u.IndexOf(UriPathSeparatorChar, 2);
                        if (i > 2)
                        {
                            p = u.Substring(i + 1);
                            return u.Substring(2, i - 2);
                        }
                    }
                    p = u;
                    return "";
                };
            }
            else
            {
                _toLocalPath = p => Uri.UnescapeDataString(p).Replace(UriPathSeparatorChar, Path.DirectorySeparatorChar);
                if (Path.AltDirectorySeparatorChar == UriPathSeparatorChar)
                    _getPathAndHost = (string u, out string p) =>
                    {
                        u = Uri.EscapeDataString(u.Replace(Path.DirectorySeparatorChar, UriPathSeparatorChar));
                        if (u.Length > 2 && u[0] == UriPathSeparatorChar && u[1] == UriPathSeparatorChar)
                        {
                            int i = u.IndexOf(UriPathSeparatorChar, 2);
                            if (i > 2)
                            {
                                p = u.Substring(i + 1);
                                return u.Substring(2, i - 2);
                            }
                        }
                        p = u;
                        return "";
                    };
                else
                    _getPathAndHost = (string u, out string p) =>
                    {
                        u = Uri.EscapeDataString(u.Replace(Path.DirectorySeparatorChar, UriPathSeparatorChar).Replace(Path.AltDirectorySeparatorChar, UriPathSeparatorChar));
                        if (u.Length > 2 && u[0] == UriPathSeparatorChar && u[1] == UriPathSeparatorChar)
                        {
                            int i = u.IndexOf(UriPathSeparatorChar, 2);
                            if (i > 2)
                            {
                                p = u.Substring(i + 1);
                                return u.Substring(2, i - 2);
                            }
                        }
                        p = u;
                        return "";
                    };
            }
        }

        /// <summary>
        /// Create new <c>FileUri</c> to represent a parent <c>FileUri</c>.
        /// </summary>
        /// <param name="host">The host name.</param>
        /// <param name="segments">The parent path segments</param>
        private FileUri(string host, IEnumerable<string> segments)
        {
            Host = host;
            Segments = new ReadOnlyCollection<string>(segments.ToArray());
            IsDirectory = true;
            int endIdx = Segments.Count - 1;
            IsEmpty = endIdx == 0 && Host.Length == 0 && Segments[endIdx].Length == 0;
            IsAbsolute = Host.Length > 0 || Segments[0].StartsWith(UriPathSeparatorChar);
        }

        /// <summary>
        /// Creates a new <c>FileUri</c> object.
        /// </summary>
        /// <param name="host">The host name, which can be <c>null</c> or empty if this is to represent a relative URI.</param>
        /// <param name="uriPath">The URI encoded path path</param>
        public FileUri(string host, string uriPath)
        {
            Host = host ?? "";
            if (string.IsNullOrEmpty(uriPath))
            {
                IsAbsolute = Host.Length > 0;
                if (IsAbsolute && Uri.CheckHostName(Host) == UriHostNameType.Unknown)
                    throw new ArgumentOutOfRangeException(nameof(host));
                IsEmpty = Host.Length == 0;
                IsDirectory = IsEmpty = true;
                Segments = new ReadOnlyCollection<string>(new string[] { (IsEmpty) ? "" : "/" });
            }
            else
            {
                string[] segments;
                if (!Uri.IsWellFormedUriString(uriPath, UriKind.Relative) || uriPath.Contains('?') || uriPath.Contains('#') ||
                        (segments = UriHelper.PathSegmentPattern.Matches(uriPath).Cast<Match>().Select(m => m.Value).ToArray()).Length == 0 || segments.Any(s => s.Length == 0))
                    throw new ArgumentOutOfRangeException(nameof(uriPath));
                if (segments.Length == 1 && Host.Length > 0 && segments[0].Length == 0)
                    segments[0] = "/";
                IsAbsolute = Host.Length > 0 || segments[0].StartsWith(UriPathSeparatorChar);
                Segments = new ReadOnlyCollection<string>(segments);
                if (IsAbsolute && Uri.CheckHostName(Host) == UriHostNameType.Unknown)
                    throw new ArgumentOutOfRangeException(nameof(host));
                int endIdx = segments.Length - 1;
                IsEmpty = endIdx == 0 && Host.Length == 0 && segments[endIdx].Length == 0;
                IsDirectory = Host.Length > 0 || segments[endIdx].EndsWith(UriPathSeparatorChar);
            }
        }

        /// <summary>
        /// Creates a new <c>FileUri</c> object.
        /// </summary>
        /// <param name="fileUriString">The well-formed relative or absolute file URI. A <see langword="null"/> or empty value results in an <see cref="IsEmpty">empty</see> file URI.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="fileUriString"/> is not a well-formed file URI string or it is not a well-formed relative URI string.
        /// <para>This exception will also be thrown if any path segments are empty (2 or more consecutive <c>/</c> characters).</para></exception>
        /// <remarks>The <seealso cref="Uri.IsWellFormedUriString(string, UriKind)"/> method is utilized to determine if <paramref name="fileUriString"/> is well-formed.
        /// <para><see cref="IsDirectory"/> will be set to <see langword="true"/> if <paramref name="fileUriString"/> ends with the <c>/</c> character, is a file URI with no path specified, or if <paramref name="fileUriString"/> is null or empty.</para>
        /// <para><seealso cref="UriHelper.AsNormalized(Uri)"/></para></remarks>
        public FileUri(string fileUriString = "")
        {
            IsEmpty = string.IsNullOrEmpty(fileUriString);
            if (IsEmpty)
            {
                Host = "";
                Segments = new ReadOnlyCollection<string>(new string[] { "" });
                IsDirectory = IsAbsolute = false;
            }
            else
            {
                IsAbsolute = Uri.IsWellFormedUriString(fileUriString, UriKind.Absolute);
                if (IsAbsolute)
                {
                    Uri uri = new Uri(fileUriString, UriKind.Absolute);
                    if (uri.Scheme != Uri.UriSchemeFile || !(string.IsNullOrEmpty(uri.Fragment) && string.IsNullOrEmpty(uri.Query)))
                        throw new ArgumentOutOfRangeException(nameof(fileUriString));
                    Host = (uri.Host ?? "").ToLower();
                    fileUriString = uri.AbsolutePath;
                }
                else if (Uri.IsWellFormedUriString(fileUriString, UriKind.Relative) && !(fileUriString.Contains('?') || fileUriString.Contains('#')))
                    Host = "";
                else
                    throw new ArgumentOutOfRangeException(nameof(fileUriString));

                string[] segments = UriHelper.PathSegmentPattern.Matches(fileUriString).Cast<Match>().Select(m => m.Value).ToArray();
                if (segments.Length == 0 || segments.Any(s => s.Length == 0))
                    throw new ArgumentOutOfRangeException(nameof(fileUriString));
                if (segments.Length == 1 && Host.Length > 0 && segments[0].Length == 0)
                    segments[0] = "/";
                Segments = new ReadOnlyCollection<string>(segments);
                IsDirectory = segments[segments.Length - 1].EndsWith(UriPathSeparatorChar);
            }
        }

        /// <summary>
        /// Creates a <see cref="FileUri"/> from a <see cref="FileSystemInfo"/> object representing a local file system path.
        /// </summary>
        /// <param name="fileSystemInfo">The<see cref="FileSystemInfo"/> object representing a local file system path.</param>
        /// <returns>A <see cref="FileUri"/> representing the specified local path.</returns>
        public static FileUri FromFileSystemInfo(FileSystemInfo fileSystemInfo)
        {
            new Uri(fileSystemInfo.FullName, UriKind.Absolute).TrySetTrailingEmptyPathSegment(fileSystemInfo is DirectoryInfo, out Uri uri);
            return new FileUri(uri.AbsoluteUri);
        }

        public static implicit operator FileUri(Uri uri)
        {
            if (uri is null)
                return null;
            if (uri.IsAbsoluteUri)
            {
                if (uri.Scheme != Uri.UriSchemeFile || string.IsNullOrEmpty(uri.Query) || string.IsNullOrEmpty(uri.Fragment))
                    return null;
                return new FileUri(uri.Host, uri.AbsolutePath);
            }
            if (Uri.IsWellFormedUriString(uri.OriginalString, UriKind.Relative))
                return new FileUri("", uri.OriginalString);
            return null;
        }

        public static implicit operator Uri(FileUri uri) => (uri is null) ? null : ((uri.IsAbsolute) ?
            new Uri($"file://{uri.Host}{string.Join("", uri.Segments)}", UriKind.Absolute) : new Uri(string.Join("", uri.Segments), UriKind.Absolute));

        public static implicit operator FileUri(FileSystemInfo fsi) => (fsi is null) ? null : FromFileSystemInfo(fsi);

        /// <summary>
        /// Creates a local path string from file URI.
        /// </summary>
        /// <returns>A local path string from file URI.</returns>
        public string ToLocalPath()
        {
            if (IsEmpty)
                return "";
            if (IsAbsolute)
                return $"{Path.DirectorySeparatorChar}{Path.DirectorySeparatorChar}{Host}{_toLocalPath(string.Join("", Segments))}";
            return _toLocalPath(string.Join("", Segments));
        }

        /// <summary>
        /// Gets a <seealso cref="DirectoryInfo"/> or <seealso cref="FileInfo"/> representing the current <c>FileUri</c>.
        /// </summary>
        /// <returns>A <seealso cref="DirectoryInfo"/> or <seealso cref="FileInfo"/> representing the current <c>FileUri</c> or <see langword="null"/> if the current
        /// <c>FileUri</c> <see cref="IsEmpty"/> is <see langword="true"/>, <see cref="IsAbsolute"/> is <see langword="false"/>, or if a <seealso cref="FileSystemInfo"/>
        /// object could not be created.</returns>
        public FileSystemInfo ToFileSystemInfo()
        {
            if (IsEmpty || !IsAbsolute)
                return null;
            try
            {
                if (IsDirectory)
                    return new DirectoryInfo(ToLocalPath());
                return new FileInfo(ToLocalPath());
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Compares <c>FileUri</c> objects with individual case sensitivity options.
        /// </summary>
        /// <param name="currentCaseSensitive">Indicates whether the current <c>FileUri</c> path is to be treated as case-sensitive.</param>
        /// <param name="other">The <c>FileUri</c> to compare to.</param>
        /// <param name="otherCaseSensitive">Indicates whether the <paramref name="other"/> <c>FileUri</c> path is to be treated as case-sensitive.</param>
        /// <returns><see langword="true"/> if the current <c>FileUri</c> is equal to the <paramref name="other"/>; otherwise, <see langword="false"/>.</returns>
        /// <remarks>Does a case-sensitive comparison if either <paramref name="currentCaseSensitive"/> or <paramref name="otherCaseSensitive"/> is <see langword="true"/>.</remarks>
        public bool Equals(bool currentCaseSensitive, FileUri other, bool otherCaseSensitive)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            if (IsEmpty)
                return other.IsEmpty;
            return !other.IsEmpty && IsDirectory == other.IsDirectory && IsAbsolute == other.IsAbsolute && Host.Equals(other.Host) && Segments.Count == other.Segments.Count &&
                Segments.SequenceEqual(other.Segments, (currentCaseSensitive || otherCaseSensitive) ? StringComparer.InvariantCulture : StringComparer.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Compares <c>FileUri</c> objects with case sensitivity option.
        /// </summary>
        /// <param name="other">The <c>FileUri</c> to compare to.</param>
        /// <param name="caseSensitive">Indicates whether the path comparisons are case-sensitive.</param>
        /// <returns><see langword="true"/> if the current <c>FileUri</c> is equal to the <paramref name="other"/>; otherwise, <see langword="false"/>.</returns>
        public bool Equals(FileUri other, bool caseSensitive)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            if (IsEmpty)
                return other.IsEmpty;
            return !other.IsEmpty && IsDirectory == other.IsDirectory && IsAbsolute == other.IsAbsolute && Host.Equals(other.Host) && Segments.Count == other.Segments.Count &&
                Segments.SequenceEqual(other.Segments, (caseSensitive) ? StringComparer.InvariantCulture : StringComparer.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Compares the current <c>FileUri</c> object with a <seealso cref="FileSystemInfo"/> object.
        /// </summary>
        /// <param name="other">The <seealso cref="FileSystemInfo"/> to compare to</param>
        /// <param name="caseSensitive">Indicates whether the path comparisons are case-sensitive.</param>
        /// <returns><see langword="true"/> if the current <c>FileUri</c> is equal to the <paramref name="other"/>; otherwise, <see langword="false"/>.</returns>
        /// <remarks>Host name comparison is always case-insensitive.</remarks>
        public bool Equals(FileSystemInfo other, bool caseSensitive)
        {
            if (other is null || IsEmpty || !IsAbsolute || (other is DirectoryInfo) != IsDirectory)
                return false;
            string a = ToString();
            Uri uri = new Uri(other.FullName);
            if (!uri.Host.ToLower().Equals(uri.Host))
                uri.TrySetHostComponent(uri.Host.ToLower(), null, out uri);
            string b = uri.AbsoluteUri;
            if (IsDirectory)
            {
                if (!b.EndsWith(UriPathSeparatorChar))
                    b = $"{b}{UriPathSeparatorChar}";
            }
            else if (b.EndsWith(UriPathSeparatorChar))
                b = b.Substring(b.Length - 1);
            return ((caseSensitive) ? StringComparer.InvariantCulture : StringComparer.InvariantCultureIgnoreCase).Equals(a, b);
        }

        public bool Equals(FileUri other) => !(other is null) && (ReferenceEquals(this, other) ||
            (Host.Equals(other.Host) && Segments.Count == other.Segments.Count && !Segments.Zip(other.Segments, (a, b) => !a.Equals(b)).Any()));

        public bool Equals(Uri other) => null != other && ((other.IsAbsoluteUri) ? IsAbsolute && other.Scheme == Uri.UriSchemeFile &&
            string.IsNullOrEmpty(other.Fragment) && string.IsNullOrEmpty(other.Fragment) && Host.Equals(other.Host, StringComparison.InvariantCultureIgnoreCase) &&
            string.Join("", Segments).Equals(other.AbsolutePath) :
            ((IsEmpty) ? other.OriginalString.Length == 0 : !IsAbsolute && string.Join("", Segments).Equals(other.OriginalString)));

        public override bool Equals(object obj) => (obj is FileUri uri) ? Equals(uri) : Equals(obj as Uri);

        public override int GetHashCode() => (IsEmpty ? "" : $"{Host}{string.Join("", Segments)}").GetHashCode();

        public override string ToString() => (IsEmpty) ? "" : ((IsAbsolute) ? ((Segments.Count > 0) ? $"file://{Host}{string.Join("", Segments)}" : $"file://{Host}/") : string.Join("", Segments));

        public int CompareTo(FileUri other)
        {
            if (other is null)
                return 1;
            if (ReferenceEquals(this, other))
                return 0;
            if (IsEmpty)
                return (other.IsEmpty) ? 0 : -1;
            if (other.IsEmpty)
                return 1;
            int result;
            if (IsAbsolute)
            {
                if (other.IsAbsolute)
                {
                    result = Host.CompareTo(other.Host);
                    if (result != 0)
                        return result;
                }
                else
                    return 1;
            }
            else if (other.IsAbsolute)
                return -1;
            if (Segments.Count < other.Segments.Count)
                return Segments.Zip(other.Segments.Take(Segments.Count), (a, b) => a.CompareTo(b)).Where(r => r != 0).DefaultIfEmpty(-1).First();
            if (Segments.Count > other.Segments.Count)
                return Segments.Take(other.Segments.Count).Zip(other.Segments, (a, b) => a.CompareTo(b)).Where(r => r != 0).DefaultIfEmpty(1).First();
            return Segments.Zip(other.Segments, (a, b) => a.CompareTo(b)).Where(r => r != 0).DefaultIfEmpty(0).First();
        }
    }
}

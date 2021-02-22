using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
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
        private static readonly char[] _queryOrFragmentChar = new char[] { '?', '#' };

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
        /// Indicates whether the path represents a subdirectory (ends with <c>/</c>)
        /// </summary>
        public bool IsAbsolute { get; }

        /// <summary>
        /// Indicates whether this is an empty file URI (<see cref="Path"/> and <see cref="Host"/> are empty strings).
        /// </summary>
        public bool IsEmpty { get;  }

        static FileUri()
        {
            HasAltDirectorySeparatorChar = System.IO.Path.AltDirectorySeparatorChar != System.IO.Path.DirectorySeparatorChar;
            DirectorySeparatorSameAsUriPathSeparator = System.IO.Path.DirectorySeparatorChar == UriPathSeparatorChar;
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
                _toLocalPath = p => Uri.UnescapeDataString(p).Replace(UriPathSeparatorChar, System.IO.Path.DirectorySeparatorChar);
                if (System.IO.Path.AltDirectorySeparatorChar == UriPathSeparatorChar)
                    _getPathAndHost = (string u, out string p) =>
                    {
                        u = Uri.EscapeDataString(u.Replace(System.IO.Path.DirectorySeparatorChar, UriPathSeparatorChar));
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
                        u = Uri.EscapeDataString(u.Replace(System.IO.Path.DirectorySeparatorChar, UriPathSeparatorChar).Replace(System.IO.Path.AltDirectorySeparatorChar, UriPathSeparatorChar));
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

        public static bool TryCreate(object obj, bool assumeLocalPath, out FileUri fileUri)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a <see cref="FileUri"/> instance representing the parent /
        /// </summary>
        /// <returns>A <see cref="FileUri"/> instance representing the parent directory or <c>null</c> if there is no parent.</returns>
        public FileUri GetParentUri()
        {
            if (Segments.Count < 2)
                return null;
            return new FileUri(Host, Segments.Take(Segments.Count - 1));
        }

        private FileUri(string host, IEnumerable<string> segments)
        {
            Host = host;
            IsAbsolute = host.Length > 0;
            IsEmpty = false;
            IsDirectory = true;
            Segments = new ReadOnlyCollection<string>(segments.ToArray());
        }

        /// <summary>
        /// Creates a new <c>FileUri</c> object.
        /// </summary>
        /// <param name="host">The host name, which can be <c>null</c> or empty if this is to represent a relative URI.</param>
        /// <param name="uriPath">The URI encoded path path</param>
        public FileUri(string host, string uriPath)
        {
            IsAbsolute = !string.IsNullOrEmpty(host);
            if (IsAbsolute)
            {
                if (Uri.CheckHostName(host = host.ToLower()) == UriHostNameType.Unknown)
                    throw new ArgumentOutOfRangeException(nameof(host));
                Host = host.ToLower();
            }
            else
                Host = "";
            if (string.IsNullOrEmpty(uriPath))
            {
                IsEmpty = !IsAbsolute;
                Segments = new ReadOnlyCollection<string>((IsEmpty) ? new string[0] : new string[] { "/" });
                IsDirectory = IsAbsolute;
                return;
            }
            IsEmpty = false;
            if (!Uri.IsWellFormedUriString(uriPath, UriKind.Relative))
                throw new ArgumentOutOfRangeException(nameof(uriPath));

            string[] segments = UriHelper.PathSegmentPattern.Matches(uriPath).Cast<Match>().Select(m => m.Value).ToArray();
            if ((segments[0][0] != UriPathSeparatorChar && Host.Length > 0) || segments.Skip(1).Any(s => s.IsEqualTo(UriPathSeparatorChar)))
                throw new ArgumentOutOfRangeException(nameof(uriPath));
            Segments = new ReadOnlyCollection<string>(segments);
            IsDirectory = segments[segments.Length - 1].EndsWith('/');
        }

        /// <summary>
        /// Creates a new <c>FileUri</c> object.
        /// </summary>
        /// <param name="fileUriString">The relative or absolute file URI.</param>
        public FileUri(string fileUriString)
        {
            IsEmpty = string.IsNullOrEmpty(fileUriString);
            if (IsEmpty)
            {
                Host = "";
                Segments = new ReadOnlyCollection<string>(new string[0]);
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
                else if (Uri.IsWellFormedUriString(fileUriString, UriKind.Relative))
                    Host = "";
                else
                    throw new ArgumentOutOfRangeException(nameof(fileUriString));

                string[] segments = UriHelper.PathSegmentPattern.Matches(fileUriString).Cast<Match>().Select(m => m.Value).ToArray();
                if ((segments[0][0] != UriPathSeparatorChar && Host.Length > 0) || segments.Skip(1).Any(s => s.IsEqualTo(UriPathSeparatorChar)))
                    throw new ArgumentOutOfRangeException(nameof(fileUriString));
                Segments = new ReadOnlyCollection<string>(segments);
                IsDirectory = segments[segments.Length - 1].EndsWith('/');
            }
        }

        /// <summary>
        /// Creates a <see cref="FileUri"/> from a local file path.
        /// </summary>
        /// <param name="path">The local filesystem path.</param>
        /// <returns>A <see cref="FileUri"/> representing the specified local path.</returns>
        public static FileUri FromLocalPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return new FileUri("");
            return new FileUri(_getPathAndHost(path, out path), path);
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

        public static implicit operator FileUri(FileSystemInfo fsi) =>
            (fsi is null) ? null :
            FromLocalPath((fsi is FileInfo || fsi.FullName.EndsWith(System.IO.Path.DirectorySeparatorChar)) ? fsi.FullName : $"{fsi.FullName}/");

        /// <summary>
        /// Creates a local path string from file URI.
        /// </summary>
        /// <returns>A local path string from file URI.</returns>
        public string ToLocalPath()
        {
            if (IsEmpty)
                return "";
            if (IsAbsolute)
                return $"{System.IO.Path.DirectorySeparatorChar}{System.IO.Path.DirectorySeparatorChar}{Host}{_toLocalPath(string.Join("", Segments))}";
            return _toLocalPath(string.Join("", Segments));
        }

        public bool Equals(FileUri other) => !(other is null) && (ReferenceEquals(this, other) ||
            (Host.Equals(other.Host) && Segments.Count == other.Segments.Count && !Segments.Zip(other.Segments, (a, b) => !a.Equals(b)).Any()));

        public bool Equals(Uri other) => null != other && ((other.IsAbsoluteUri) ? IsAbsolute && other.Scheme == Uri.UriSchemeFile &&
            string.IsNullOrEmpty(other.Fragment) && string.IsNullOrEmpty(other.Fragment) && Host.Equals(other.Host, StringComparison.InvariantCultureIgnoreCase) &&
            string.Join("", Segments).Equals(other.AbsolutePath) :
            ((IsEmpty) ? other.OriginalString.Length == 0 : !IsAbsolute && string.Join("", Segments).Equals(other.OriginalString)));

        public override bool Equals(object obj) => (obj is FileUri) ? Equals((FileUri)obj) : Equals(obj as Uri);

        public override int GetHashCode() => ((IsEmpty) ? "" : Host + string.Join("", Segments)).GetHashCode();

        public override string ToString() => (IsEmpty) ? "" : ((IsAbsolute) ? $"file://{Host}{string.Join("", Segments)}" : string.Join("", Segments));

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

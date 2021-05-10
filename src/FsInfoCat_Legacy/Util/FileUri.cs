using FsInfoCat.Models.Volumes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FsInfoCat.Util
{
    /// <summary>
    /// Represents a file URI that can be sorted and compared.
    /// </summary>
    public class FileUri
    {
        /// <summary>
        /// URI encode host name.
        /// </summary>
        public string Host { get; }

        public string Name { get; }

        public FileUri Parent { get; }

        public int PathSegmentCount { get; }

        /// <summary>
        /// Creates a new <c>FileUri</c> object.
        /// </summary>
        /// <param name="fileUriString">The well-formed relative or absolute file URI. A <see langword="null"/> or empty value results in an <see cref="IsEmpty">empty</see> file URI.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="fileUriString"/> is not a well-formed file URI string or it is not a well-formed relative URI string.
        /// <para>This exception will also be thrown if any path segments are empty (2 or more consecutive <c>/</c> characters).</para></exception>
        /// <remarks>The <seealso cref="Uri.IsWellFormedUriString(string, UriKind)"/> method is utilized to determine if <paramref name="fileUriString"/> is well-formed.
        /// <para><see cref="IsDirectory"/> will be set to <see langword="true"/> if <paramref name="fileUriString"/> ends with the <c>/</c> character, is a file URI with no path specified, or if <paramref name="fileUriString"/> is null or empty.</para>
        /// <para><seealso cref="UriHelper.AsNormalized(Uri)"/></para></remarks>
        public FileUri(string fileUriString = "", PlatformType platform = PlatformType.Unknown)
        {
            if (string.IsNullOrEmpty(fileUriString))
            {
                Host = Name = "";
                Parent = null;
                PathSegmentCount = 0;
            }
            else if (Uri.IsWellFormedUriString(fileUriString, UriKind.Absolute) && FileUriConverter.GetFactory(platform).TrySplitFileUriString(fileUriString,
                out string hostName, out string directory, out string fileName, out bool isAbsolute) && isAbsolute)
            {
                Host = hostName;
                Name = fileName;
                if (directory.Length > 0)
                    PathSegmentCount = (Parent = new FileUri(Host, directory)).PathSegmentCount + 1;
                else
                {
                    PathSegmentCount = 1;
                    Parent = null;
                }
            }
            else
                throw new ArgumentOutOfRangeException(nameof(fileUriString));
        }

        public FileUri(FileUri parent, string name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));
            if (name.Length == 0 || (name = FileUriConverter.CURRENT_FACTORY.EnsureWellFormedUriString(name, UriKind.Relative)).Contains(UriHelper.URI_PATH_SEPARATOR_CHAR))
                throw new ArgumentOutOfRangeException(nameof(name));
            Name = name;
            PathSegmentCount = ((Parent = parent) is null) ? 1 : parent.PathSegmentCount + 1;
        }

        /// <summary>
        /// Creates a <see cref="FileUri"/> from a <see cref="FileSystemInfo"/> object representing a local file system path.
        /// </summary>
        /// <param name="fileSystemInfo">The<see cref="FileSystemInfo"/> object representing a local file system path.</param>
        public FileUri(FileSystemInfo fileSystemInfo)
        {
            if (fileSystemInfo is null)
            {
                Host = Name = "";
                PathSegmentCount = 0;
            }
            else
            {
                string fileName = FileUriConverter.CURRENT_FACTORY.FromFileSystemPath(fileSystemInfo.FullName, out string hostName, out string directoryName);
                Host = hostName;
                if (fileName.Length > 0)
                {
                    Name = fileName;
                    PathSegmentCount = (Parent = new FileUri(Host, directoryName)).PathSegmentCount + 1;
                    return;
                }
                PathSegmentCount = 1;
                Name = directoryName;
            }
            Parent = null;
        }

        private FileUri(string host, string absolutePath)
        {
            Host = host;
            int leafIndex;
            if (absolutePath.Length < 2 || (leafIndex = absolutePath.LastIndexOf(UriHelper.URI_PATH_SEPARATOR_CHAR)) < 0)
            {
                Name = absolutePath;
                Parent = null;
                PathSegmentCount = 1;
            }
            else
            {
                Name = absolutePath.Substring(leafIndex + 1);
                PathSegmentCount = (Parent = new FileUri(host, absolutePath.Substring(0, leafIndex - 1))).PathSegmentCount;
            }
        }

        public static implicit operator FileUri(FileSystemInfo fsi) => (fsi is null) ? null : new FileUri(fsi);

        public IEnumerable<string> GetPathSegments()
        {
            if (PathSegmentCount == 0)
                return Array.Empty<string>();
            Stack<string> result = new Stack<string>();
            result.Push((Name == UriHelper.URI_PATH_SEPARATOR_STRING) ? "" : Name);
            for (FileUri fileUri = Parent; !(fileUri is null); fileUri = fileUri.Parent)
                result.Push(fileUri.Name);
            return result.AsEnumerable();
        }

        public IEnumerable<FileUri> GetAncestors()
        {
            for (FileUri parent = Parent; !(parent is null); parent = parent.Parent)
                yield return parent;
        }

        public IEnumerable<FileUri> GetAncestorsAndSelf()
        {
            if (!IsEmpty())
            {
                yield return this;
                for (FileUri parent = Parent; !(parent is null); parent = parent.Parent)
                    yield return parent;
            }
        }

        public bool IsEmpty() => PathSegmentCount == 0;

        private StringBuilder ToUriPath(StringBuilder stringBuilder)
        {
            if (Parent is null)
                return stringBuilder.Append(Name);
            if (Parent.Name.EndsWith(UriHelper.URI_PATH_SEPARATOR_CHAR))
                return Parent.ToUriPath(stringBuilder).Append(Name);
            return Parent.ToUriPath(stringBuilder).Append(UriHelper.URI_PATH_SEPARATOR_CHAR).Append(Name);
        }

        /// <summary>
        /// Creates a local path string from file URI.
        /// </summary>
        /// <returns>A local path string from file URI.</returns>
        public string ToLocalPath(PlatformType platform = PlatformType.Unknown)
        {
            if (IsEmpty())
                return "";
            return FileUriConverter.ToFileSystemPath(ToString(), platform);
        }

        private StringBuilder ToString(StringBuilder stringBuilder)
        {
            if (Parent is null)
                return (Name.StartsWith(UriHelper.URI_PATH_SEPARATOR_STRING)) ? stringBuilder.Append(Name) : stringBuilder.Append(UriHelper.URI_PATH_SEPARATOR_CHAR).Append(Name);
            return Parent.ToString(stringBuilder).Append(UriHelper.URI_PATH_SEPARATOR_CHAR).Append(Name);
        }

        public override string ToString()
        {
            if (Parent is null)
                return (IsEmpty()) ? "" : ((Name.StartsWith(UriHelper.URI_PATH_SEPARATOR_STRING)) ? $"file://{Host}{Name}" : $"file://{Host}/{Name}");
            return Parent.ToString((Host.Length == 0) ? new StringBuilder("file://") : new StringBuilder("file://").Append(Host)).Append(Name).ToString();
        }

        public bool IsUriOf(IVolumeSetItem volume)
        {
            if (volume is null || IsEmpty())
                return false;
            FileUri rootUri = volume.RootUri;
            if (rootUri is null || rootUri.IsEmpty())
                return false;
            if (ReferenceEquals(this, rootUri))
                return true;
            if (!DynamicStringComparer.IgnoreCaseEquals(Host, rootUri.Host))
                return false;
            int rc = rootUri.PathSegmentCount;
            int sc = PathSegmentCount;
            if (rc > sc)
                return false;
            FileUri target = this;
            while (sc > rc--)
                target = target.Parent;
            if (ReferenceEquals(target, rootUri))
                return true;
            IFileUriKey mountPointParentUri = volume.MountPointParentUri;
            IVolumeSetItem parentVol;
            if (mountPointParentUri is null || (parentVol = mountPointParentUri.Volume) is null)
            {
                IEqualityComparer<string> nameComparer = volume.GetNameComparer();
                while (nameComparer.Equals(rootUri.Name, target.Name))
                {
                    if ((rootUri = rootUri.Parent) is null)
                        return true;
                    target = target.Parent;
                }
                return false;
            }
            return parentVol.GetNameComparer().Equals(target.Name, rootUri.Name) && target.IsUriOf(mountPointParentUri.Volume);
        }

        public static bool IsNullOrEmpty(FileUri fileUri) => fileUri is null || fileUri.IsEmpty();

        public bool IsSameHost(FileUri other) => !(other is null) && (ReferenceEquals(this, other) ||
            (DynamicStringComparer.IgnoreCaseEquals(Host, other.Host) && IsEmpty() == other.IsEmpty()));

        public static bool AreSegmentsEqual(FileUri x, FileUri y, int startIndex, IEqualityComparer<string> comparer)
        {
            if (comparer is null)
                throw new ArgumentNullException(nameof(comparer));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (x is null || x.IsEmpty())
                return y is null || y.IsEmpty();
            if (y is null || y.IsEmpty())
                return false;
            if (ReferenceEquals(x, y))
                return true;
            int endIndex = x.PathSegmentCount;
            if (startIndex >= endIndex)
                return startIndex >= y.PathSegmentCount;
            if (startIndex >= y.PathSegmentCount || endIndex != y.PathSegmentCount)
                return false;
            do
            {
                if (!comparer.Equals(x.Name, y.Name))
                    return false;
                x = x.Parent;
                y = y.Parent;
            } while (endIndex > ++startIndex);
            return true;
        }

        public static bool AreSegmentsEqual(FileUri x, FileUri y, int startIndex, int endIndex, IEqualityComparer<string> comparer)
        {
            if (comparer is null)
                throw new ArgumentNullException(nameof(comparer));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (startIndex > endIndex)
                throw new ArgumentOutOfRangeException(nameof(endIndex));
            if (x is null || x.IsEmpty())
                return y is null || y.IsEmpty();
            if (y is null || y.IsEmpty())
                return false;
            if (ReferenceEquals(x, y))
                return true;
            if (startIndex >= x.PathSegmentCount)
                return startIndex >= y.PathSegmentCount;
            if (startIndex >= y.PathSegmentCount)
                return false;
            if (startIndex == endIndex)
                return true;
            if (x.PathSegmentCount < endIndex)
            {
                if (y.PathSegmentCount != x.PathSegmentCount)
                    return false;
                endIndex = x.PathSegmentCount;
            }
            else
            {
                if (y.PathSegmentCount < endIndex)
                    return false;
                while (x.PathSegmentCount > endIndex)
                    x = x.Parent;
                while (y.PathSegmentCount > endIndex)
                    y = y.Parent;
            }
            do
            {
                if (!comparer.Equals(x.Name, y.Name))
                    return false;
                x = x.Parent;
                y = y.Parent;
            } while (--endIndex > startIndex);
            return true;
        }

        public bool TryGetAtSegmentCount(int segmentCount, out FileUri fileUri)
        {
            if (segmentCount == PathSegmentCount)
                fileUri = this;
            else if (segmentCount > 0 && PathSegmentCount > 0 && segmentCount < PathSegmentCount)
            {
                fileUri = Parent;
                while (segmentCount < fileUri.PathSegmentCount)
                    fileUri = fileUri.Parent;
            }
            else
            {
                fileUri = null;
                return false;
            }
            return true;
        }
    }
}

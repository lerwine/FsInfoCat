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
    public class FileUri : IEquatable<FileUri>
    {
        /// <summary>
        /// URI encode host name.
        /// </summary>
        public string Host { get; }

        public string Name { get; }

        public FileUri Parent { get; }

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
            if (string.IsNullOrEmpty(fileUriString))
            {
                Host = Name = "";
                Parent = null;
            }
            else if (UriHelper.TryParseFileUriString(fileUriString, out string hostName, out string absolutePath, out int leafIndex))
            {
                Host = hostName.ToLower();
                if (leafIndex > 0 && absolutePath.Length > 1)
                {
                    Name = absolutePath.Substring(leafIndex);
                    Parent = new FileUri(Host, absolutePath.Substring(0, (leafIndex > 1) ? leafIndex - 1 : leafIndex));
                }
                else
                {
                    Parent = null;
                    Name = absolutePath;
                }
            }
            else
                throw new ArgumentOutOfRangeException(nameof(fileUriString));
        }

        public FileUri(FileUri parent, string name)
        {
            if (parent is null)
                throw new ArgumentNullException(nameof(parent));
            if (name is null)
                throw new ArgumentNullException(nameof(name));
            if (name.Length == 0 || (name = UriHelper.EnsureWellFormedUriPath(name)).Contains(UriHelper.URI_PATH_SEPARATOR_CHAR))
                throw new ArgumentOutOfRangeException(nameof(name));
            Name = name;
            Parent = parent;
        }

        /// <summary>
        /// Creates a <see cref="FileUri"/> from a <see cref="FileSystemInfo"/> object representing a local file system path.
        /// </summary>
        /// <param name="fileSystemInfo">The<see cref="FileSystemInfo"/> object representing a local file system path.</param>
        public FileUri(FileSystemInfo fileSystemInfo)
        {
            if (fileSystemInfo is null)
                Host = Name = "";
            else
            {
                Uri uri = fileSystemInfo.ToUri();
                Host = uri.Host.ToLower();
                string absolutePath = uri.AbsolutePath;
                if (absolutePath.Length > 1)
                {
                    int leafIndex = absolutePath.LastIndexOf(UriHelper.URI_PATH_SEPARATOR_CHAR);
                    if (((leafIndex == absolutePath.Length - 1) ? (leafIndex = (absolutePath = absolutePath.Substring(0, leafIndex)).LastIndexOf(UriHelper.URI_PATH_SEPARATOR_CHAR)) : leafIndex) > -1)
                    {
                        Name = absolutePath.Substring(leafIndex - 1);
                        Parent = new FileUri(Host, absolutePath.Substring(0, (leafIndex == 0) ? 1 : leafIndex));
                        return;
                    }
                }
                Name = absolutePath;
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
            }
            else
            {
                Name = absolutePath.Substring(leafIndex + 1);
                Parent = new FileUri(host, absolutePath.Substring(0, leafIndex - 1));
            }
        }

        public static implicit operator FileUri(FileSystemInfo fsi) => (fsi is null) ? null : new FileUri(fsi);

        public IEnumerable<string> GetPathComponents()
        {
            FileUri fileUri = this;
            if (!(Parent is null))
                do
                {
                    yield return fileUri.Name;
                } while (!((fileUri = fileUri.Parent) is null));
            yield return (Name == UriHelper.URI_PATH_SEPARATOR_STRING) ? "" : Name;
        }

        public bool IsEmpty() => Name.Length == 0 && Host.Length == 0 && Parent is null;

        [Obsolete("This not logical due to posible case sensitivity differences with parent paths.")]
        public bool Equals(FileUri other, IEqualityComparer<string> comparer)
        {
            if (comparer is null)
                throw new ArgumentNullException(nameof(comparer));
            return !(other is null) && Host.Equals(other.Host) && comparer.Equals(Name, other.Name) && ((Parent is null) ? other.Parent is null : Parent.Equals(other.Parent, comparer));
        }

        private StringBuilder ToLocalPath(char separator, StringBuilder stringBuilder)
        {
            if (Parent is null)
                return (Name.Equals(UriHelper.URI_PATH_SEPARATOR_STRING)) ? stringBuilder : stringBuilder.Append(separator).Append(Name);
            return Parent.ToLocalPath(separator, stringBuilder).Append(separator).Append(Name);
        }

        /// <summary>
        /// Creates a local path string from file URI.
        /// </summary>
        /// <returns>A local path string from file URI.</returns>
        public string ToLocalPath(PlatformType platform = PlatformType.Unknown)
        {
            if (IsEmpty())
                return "";
            switch ((platform == PlatformType.Unknown) ? UriHelper.PLATFORM_TYPE : platform)
            {
                case PlatformType.Linux:
                case PlatformType.OSX:
                    if (Parent is null)
                    {
                        if (Host.Length == 0)
                            return $"/{Uri.UnescapeDataString(Name)}";
                        return $"//{Host}/{Uri.UnescapeDataString(Name)}";
                    }
                    return ToLocalPath('/', (Host.Length == 0) ? new StringBuilder() : new StringBuilder("//").Append(Host)).ToString();
                default:
                    if (Parent is null)
                    {
                        if (Host.Length == 0)
                            return $"\\{Uri.UnescapeDataString(Name)}";
                        return $"\\\\{Host}\\{Uri.UnescapeDataString(Name)}";
                    }
                    return ToLocalPath('\\', (Host.Length == 0) ? new StringBuilder() : new StringBuilder("\\\\").Append(Host)).ToString();
            }
        }

        [Obsolete("This not logical due to posible case sensitivity differences with parent paths.")]
        public bool Equals(FileUri other) => !(other is null) && (ReferenceEquals(this, other) || (Host.Equals(other.Host) && Name.Equals(other.Name) && ((Parent is null) ? other.Parent is null : Parent.Equals(other.Parent))));

        public override bool Equals(object obj) => obj is FileUri fileUri && Equals(fileUri);

        public override int GetHashCode() => ToString().GetHashCode();

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
    }
}

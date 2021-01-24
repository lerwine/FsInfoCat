using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace FsInfoCat.Models.Crawl
{
    public sealed class FsRoot : IFsDirectory, IEquatable<FsRoot>, IEqualityComparer<IFsChildNode>
    {
        /// <summary>
        /// Gets the full path name of the volume root directory.
        /// </summary>
        public string RootPathName { get; set; }

        /// <summary>
        /// Gets the name of the file system.
        /// </summary>
        public string FileSystemName { get; set; }

        /// <summary>
        /// Gets the name of the volume.
        /// </summary>
        public string VolumeName { get; set; }

        /// <summary>
        /// Gets the volume serial number.
        /// </summary>
        public uint SerialNumber { get; set; }

        /// <summary>
        /// Gets the maximum length for file/directory names.
        /// </summary>
        public uint MaxNameLength { get; set; }

        private FileSystemFeature _flags;

        /// <summary>
        /// Gets a value that indicates the volume capabilities and attributes.
        /// </summary>
        public FileSystemFeature Flags
        {
            get => _flags;
            set
            {
                if (_flags == value)
                    return;
                bool wasCaseSensitive = _flags.HasFlag(FileSystemFeature.CaseSensitiveSearch);
                if ((_flags = value).HasFlag(FileSystemFeature.CaseSensitiveSearch) != wasCaseSensitive)
                    _nameComparer = null;
            }
        }

        private Collection<IFsChildNode> _childNodes = null;
        public Collection<IFsChildNode> ChildNodes
        {
            get
            {
                Collection<IFsChildNode> childNodes = _childNodes;
                if (null == childNodes)
                    _childNodes = childNodes = new Collection<IFsChildNode>();
                return childNodes;
            }
            set { _childNodes = value; }
        }

        /// <summary>
        /// Looks for the first nested partial crawl.
        /// </summary>
        /// <param name="message">The <seealso cref="PartialCrawlWarning" /> representing the partial crawl.</param>
        /// <param name="segments">The <seealso cref="IFsDirectory" /> representing the path segments, relative to the root path, for the parent directory of the partial crawl.
        /// The last segment will contain the items that were already crawled.</param>
        /// <returns>True if a <seealso cref="PartialCrawlWarning" /> was found; otherwise, false.</returns>
        public bool TryFindPartialCrawl(out PartialCrawlWarning message, out IEnumerable<IFsDirectory> segments)
        {
            message = Messages.OfType<PartialCrawlWarning>().Where(m => m.NotCrawled.Any(s => !string.IsNullOrWhiteSpace(s))).FirstOrDefault();
            if (null != message)
            {
                segments = new IFsDirectory[0];
                return true;
            }
            foreach (IFsDirectory root in ChildNodes.OfType<IFsDirectory>())
            {
                if (root.TryFindPartialCrawl(out message, out segments))
                {
                    segments = (new IFsDirectory[] { root }).Concat(segments);
                    return true;
                }
            }
            segments = new IFsDirectory[0];
            return false;
        }

        private Collection<ICrawlMessage> _messages = null;
        public Collection<ICrawlMessage> Messages
        {
            get
            {
                Collection<ICrawlMessage> messages = _messages;
                if (null == messages)
                    _messages = messages = new Collection<ICrawlMessage>();
                return messages;
            }
            set { _messages = value; }
        }


        private IEqualityComparer<string> _nameComparer = null;

        public IEqualityComparer<string> GetNameComparer()
        {
            IEqualityComparer<string> nameComparer = _nameComparer;
            if (null == nameComparer)
                _nameComparer = nameComparer = (_flags.HasFlag(FileSystemFeature.CaseSensitiveSearch)) ?
                    StringComparer.InvariantCultureIgnoreCase : StringComparer.InvariantCulture;
            return nameComparer;
        }

        public T FindByName<T>(IEnumerable<IFsChildNode> source, string name)
            where T : class, IFsChildNode
        {
            if (string.IsNullOrEmpty(name))
                return null;
            IEqualityComparer<string> nameComparer = GetNameComparer();
            return source.OfType<T>().FirstOrDefault(c => nameComparer.Equals(c.Name, name));
        }

        public bool Equals(FsRoot other)
        {
            return null != other && (ReferenceEquals(this, other) || (SerialNumber == other.SerialNumber && String.Equals(VolumeName, other.VolumeName, StringComparison.InvariantCultureIgnoreCase)));
        }

        public override bool Equals(object obj)
        {
            return null != obj && obj is FsRoot && Equals((FsRoot)obj);
        }

        public override int GetHashCode()
        {
            return BitConverter.ToInt32(BitConverter.GetBytes(SerialNumber), 0);
        }

        public int GetHashCode(IFsChildNode obj)
        {
            string n;
            return GetNameComparer().GetHashCode((null == obj || null == (n = obj.Name)) ? "" : n);
        }

        public bool Equals(IFsChildNode x, IFsChildNode y)
        {
            if (null == x)
                return null == y;
            if (null == y)
                return false;
            if (ReferenceEquals(x, y))
                return true;
            string a = x.Name;
            string b = y.Name;
            if (null == a)
                return null == b;
            return null != b && GetNameComparer().Equals(a, b);
        }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(RootPathName))
            {
                if (String.IsNullOrWhiteSpace(VolumeName))
                    return Path.PathSeparator + " " + (SerialNumber >> 16).ToString("X4") + "-" + (SerialNumber & 0xFFFF).ToString("X4");
                return Path.PathSeparator + " " + VolumeName.Trim() + " " + (SerialNumber >> 16).ToString("X4") + "-" + (SerialNumber & 0xFFFF).ToString("X4");
            }
            if (String.IsNullOrWhiteSpace(VolumeName))
                return RootPathName.Trim() + Path.PathSeparator + " " + (SerialNumber >> 16).ToString("X4") + "-" + (SerialNumber & 0xFFFF).ToString("X4");
            return RootPathName.Trim() + Path.PathSeparator + " " + VolumeName.Trim() + " " + (SerialNumber >> 16).ToString("X4") + "-" + (SerialNumber & 0xFFFF).ToString("X4");
        }
    }
}

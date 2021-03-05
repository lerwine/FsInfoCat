using FsInfoCat.Models;
using FsInfoCat.Models.Volumes;
using FsInfoCat.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace FsInfoCat.Models.Crawl
{
    public sealed class FsRoot : ComponentBase, IVolumeInfo, IFsDirectory, IEquatable<FsRoot>, IEqualityComparer<IFsChildNode>
    {
        private FileUri _rootUri = new FileUri("");
        private string _driveFormat = "";
        private string _volumeName = "";

        private readonly ComponentList.AttachableContainer _container;
        private ComponentList<CrawlMessage> _messagesList;
        private ComponentList<IFsChildNode> _childNodes;
        private bool _caseSensitive;
        private StringComparer _segmentNameComparer;

        public FileUri RootUri
        {
            get => _rootUri;
            set
            {
                if (value is null)
                    _rootUri = new FileUri();
                else
                    _rootUri = value;
            }
        }

        /// <summary>
        /// Gets the full path name of the volume root directory.
        /// </summary>
        public string RootPathName => _rootUri.ToLocalPath();

        /// <summary>
        /// Gets the name of the file system.
        /// </summary>
        public string DriveFormat
        {
            get => _driveFormat;
            set => _driveFormat = value ?? "";
        }

        public DriveType DriveType { get; set; }

        /// <summary>
        /// Gets the name of the volume.
        /// </summary>
        public string VolumeName
        {
            get => _volumeName;
            set => _volumeName = value ?? "";
        }

        public VolumeIdentifier Identifier { get; set; }

        string INamedComponent.Name => (RootUri.IsEmpty()) ? ((string.IsNullOrWhiteSpace(VolumeName)) ? Identifier.ToString() : VolumeName) : RootUri.ToLocalPath();

        public ComponentList<CrawlMessage> Messages
        {
            get => _messagesList;
            set
            {
                ComponentList<CrawlMessage> list = (value is null) ? new ComponentList<CrawlMessage>() : value;
                if (ReferenceEquals(list, _messagesList))
                    return;
                _container.Detach(_messagesList);
                _container.Attach(list);
                _messagesList = list;
            }
        }

        IList<CrawlMessage> IFsNode.Messages { get => Messages; set => Messages = (ComponentList<CrawlMessage>)value; }

        public ComponentList<IFsChildNode> ChildNodes
        {
            get => _childNodes;
            set
            {
                ComponentList<IFsChildNode> list = (value is null) ? new ComponentList<IFsChildNode>() : value;
                if (ReferenceEquals(list, _childNodes))
                    return;
                _container.Detach(_childNodes);
                _container.Attach(list);
                _childNodes = list;
            }
        }
        IList<IFsChildNode> IFsDirectory.ChildNodes { get => _childNodes; set => ChildNodes = (ComponentList<IFsChildNode>)value; }

        public bool CaseSensitive
        {
            get => _caseSensitive;
            set
            {
                if (_caseSensitive == value)
                    return;
                _caseSensitive = value;
                _segmentNameComparer = null;
            }
        }

        public IEqualityComparer<string> SegmentNameComparer
        {
            get
            {
                StringComparer comparer = _segmentNameComparer;
                if (comparer is null)
                    _segmentNameComparer = comparer = (_caseSensitive) ? StringComparer.InvariantCulture : StringComparer.InvariantCultureIgnoreCase;
                return comparer;
            }
        }

        public FsRoot(IVolumeInfo driveInfo) : this()
        {
            if (driveInfo is null)
                throw new ArgumentNullException(nameof(driveInfo));
            RootUri = driveInfo.RootUri;
            DriveFormat = driveInfo.DriveFormat;
            VolumeName = driveInfo.VolumeName;
            Identifier = driveInfo.Identifier;
            CaseSensitive = driveInfo.CaseSensitive;
        }

        public FsRoot()
        {
            _container = new ComponentList.AttachableContainer(this);
            _messagesList = new ComponentList<CrawlMessage>(_container);
            _childNodes = new ComponentList<IFsChildNode>(_container);
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

        public bool Equals(FsRoot other)
        {
            return null != other && (ReferenceEquals(this, other) || (DriveType == other.DriveType && String.Equals(DriveFormat, other.DriveFormat, StringComparison.InvariantCultureIgnoreCase) &&
                string.Equals(VolumeName, other.VolumeName, StringComparison.InvariantCultureIgnoreCase) &&
                RootUri.Equals(other.RootUri, (CaseSensitive || !other.CaseSensitive) ? SegmentNameComparer : other.SegmentNameComparer)));
        }

        public override bool Equals(object obj)
        {
            return null != obj && obj is FsRoot root && Equals(root);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public int GetHashCode(IFsChildNode obj)
        {
            string n;
            return StringComparer.InvariantCultureIgnoreCase.GetHashCode((obj is null || null == (n = obj.Name)) ? "" : n);
        }

        public bool Equals(IFsChildNode x, IFsChildNode y)
        {
            if (x is null)
                return y is null;
            if (y is null)
                return false;
            if (ReferenceEquals(x, y))
                return true;
            string a = x.Name;
            string b = y.Name;
            if (a is null)
                return b is null;
            return null != b && StringComparer.InvariantCultureIgnoreCase.Equals(a, b);
        }

        public override string ToString()
        {
            if (RootUri.IsEmpty())
                return (VolumeName is null) ? "" : " " + VolumeName.Trim();
            if (string.IsNullOrWhiteSpace(VolumeName))
                return RootUri.ToLocalPath();
            return RootUri.ToLocalPath() + " " + VolumeName.Trim();
        }
    }
}

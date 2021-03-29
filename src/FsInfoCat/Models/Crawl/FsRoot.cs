using FsInfoCat.Models.Volumes;
using FsInfoCat.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;

namespace FsInfoCat.Models.Crawl
{
    // TODO: IEqualityComparer<IFsChildNode> may need to be replaced by using IVolumeSet provider due to nested case-sensitivity difference issues
    public sealed class FsRoot : ComponentBase, IVolumeInfo, IFsDirectory, IEqualityComparer<IFsChildNode>, IEquatable<FsRoot>
    {
        private FileUri _rootUri = new FileUri("");
        private string _driveFormat = "";
        private string _volumeName = "";

        private readonly ComponentList.AttachableContainer _container;
        private ComponentList<CrawlMessage> _messagesList;
        private ComponentList<IFsChildNode> _childNodes;
        private readonly DynamicStringComparer _segmentNameComparer;
        private DriveType _driveType;
        private VolumeIdentifier _identifier;

        public event PropertyValueChangeEventHandler PropertyValueChanging;
        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyValueChangeEventHandler PropertyValueChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        public FileUri RootUri
        {
            get => _rootUri;
            set
            {
                Monitor.Enter(this);
                try
                {
                    FileUri newValue = value;
                    if (newValue is null)
                    {
                        if (_rootUri.IsEmpty())
                            return;
                        newValue = new FileUri();
                    }
                    else if (ReferenceEquals(_rootUri, newValue))
                        return;
                    RaisePropertyChanging(nameof(RootUri), _rootUri, newValue);
                    string oldPathName = RootPathName;
                    string newPathName = newValue.ToLocalPath();
                    if (!oldPathName.Equals(newPathName))
                        RaisePropertyChanging(nameof(RootPathName), oldPathName, newPathName);
                    string oldName = CalculateName(RootUri, VolumeName, Identifier);
                    string newName = CalculateName(value, VolumeName, Identifier);
                    if (!oldName.Equals(newName))
                        RaisePropertyChanging(nameof(INamedComponent.Name), oldName, newName);
                    FileUri oldValue = _rootUri;
                    _rootUri = newValue;
                    RaisePropertyChanged(nameof(RootUri), oldValue, _rootUri);
                    if (!oldPathName.Equals(newPathName))
                        RaisePropertyChanged(nameof(RootPathName), oldPathName, RootPathName);
                    if (!oldName.Equals(newName))
                        RaisePropertyChanged(nameof(INamedComponent.Name), oldName, CalculateName(RootUri, VolumeName, Identifier));
                }
                finally { Monitor.Exit(this); }
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
            set
            {
                Monitor.Enter(this);
                try
                {
                    string newValue = value ?? "";
                    if (newValue.Equals(_driveFormat))
                        return;
                    RaisePropertyChanging(nameof(DriveFormat), _driveFormat, newValue);
                    string oldValue = _driveFormat;
                    _driveFormat = newValue;
                    RaisePropertyChanged(nameof(DriveFormat), oldValue, _driveFormat);
                }
                finally { Monitor.Exit(this); }
            }
        }

        public DriveType DriveType
        {
            get => _driveType;
            set
            {
                Monitor.Enter(this);
                try
                {
                    if (_driveType == value)
                        return;
                    RaisePropertyChanging(nameof(DriveFormat), _driveType, value);
                    DriveType oldValue = _driveType;
                    _driveType = value;
                    RaisePropertyChanged(nameof(DriveFormat), oldValue, _driveType);
                }
                finally { Monitor.Exit(this); }
            }
        }

        /// <summary>
        /// Gets the name of the volume.
        /// </summary>
        public string VolumeName
        {
            get => _volumeName;
            set
            {
                Monitor.Enter(this);
                try
                {
                    string newValue = value ?? "";
                    if (newValue.Equals(_volumeName))
                        return;
                    RaisePropertyChanging(nameof(VolumeName), _volumeName, newValue);
                    string oldName = CalculateName(RootUri, VolumeName, Identifier);
                    string newName = CalculateName(RootUri, value, Identifier);
                    if (!oldName.Equals(newName))
                        RaisePropertyChanging(nameof(INamedComponent.Name), oldName, newName);
                    string oldValue = _driveFormat;
                    _volumeName = newValue;
                    RaisePropertyChanged(nameof(VolumeName), oldValue, _volumeName);
                    if (!oldName.Equals(newName))
                        RaisePropertyChanged(nameof(INamedComponent.Name), oldName, CalculateName(RootUri, VolumeName, Identifier));
                }
                finally { Monitor.Exit(this); }
            }
        }

        public VolumeIdentifier Identifier
        {
            get => _identifier;
            set
            {
                Monitor.Enter(this);
                try
                {
                    if (_identifier.Equals(value))
                        return;
                    RaisePropertyChanging(nameof(DriveFormat), _identifier, value);
                    string oldName = CalculateName(RootUri, VolumeName, Identifier);
                    string newName = CalculateName(RootUri, VolumeName, value);
                    if (!oldName.Equals(newName))
                        RaisePropertyChanging(nameof(INamedComponent.Name), oldName, newName);
                    VolumeIdentifier oldValue = _identifier;
                    _identifier = value;
                    RaisePropertyChanged(nameof(DriveFormat), oldValue, _identifier);
                    if (!oldName.Equals(newName))
                        RaisePropertyChanged(nameof(INamedComponent.Name), oldName, CalculateName(RootUri, VolumeName, Identifier));
                }
                finally { Monitor.Exit(this); }
            }
        }

        string INamedComponent.Name => CalculateName(RootUri, VolumeName, Identifier);

        public ComponentList<CrawlMessage> Messages
        {
            get => _messagesList;
            set
            {
                Monitor.Enter(this);
                try
                {
                    ComponentList<CrawlMessage> newValue = value;
                    if (value is null)
                    {
                        if (_messagesList.Count == 0)
                            return;
                        newValue = new ComponentList<CrawlMessage>();
                    }
                    else if (ReferenceEquals(newValue, _messagesList))
                        return;
                    RaisePropertyChanging(nameof(Messages), _messagesList, newValue);
                    ComponentList<CrawlMessage> oldValue = _messagesList;
                    _container.Detach(_messagesList);
                    _container.Attach(newValue);
                    _messagesList = newValue;
                    RaisePropertyChanged(nameof(Messages), oldValue, _messagesList);
                }
                finally { Monitor.Exit(this); }
            }
        }

        IList<CrawlMessage> IFsNode.Messages { get => Messages; set => Messages = (ComponentList<CrawlMessage>)value; }

        public ComponentList<IFsChildNode> ChildNodes
        {
            get => _childNodes;
            set
            {
                Monitor.Enter(this);
                try
                {
                    ComponentList<IFsChildNode> newValue = value;
                    if (value is null)
                    {
                        if (_childNodes.Count == 0)
                            return;
                        newValue = new ComponentList<IFsChildNode>();
                    }
                    else if (ReferenceEquals(newValue, _childNodes))
                        return;
                    RaisePropertyChanging(nameof(ChildNodes), _childNodes, newValue);
                    ComponentList<IFsChildNode> oldValue = value;
                    _container.Detach(_childNodes);
                    _container.Attach(newValue);
                    _childNodes = newValue;
                    RaisePropertyChanged(nameof(ChildNodes), oldValue, _childNodes);
                }
                finally { Monitor.Exit(this); }
            }
        }
        IList<IFsChildNode> IFsDirectory.ChildNodes { get => _childNodes; set => ChildNodes = (ComponentList<IFsChildNode>)value; }

        public bool CaseSensitive
        {
            get => _segmentNameComparer.CaseSensitive;
            set
            {
                Monitor.Enter(this);
                try
                {
                    _segmentNameComparer.CaseSensitive = value;
                }
                finally { Monitor.Exit(this); }
            }
        }

        public IEqualityComparer<string> GetNameComparer() => _segmentNameComparer;

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

        public FsRoot() : this(false) { }

        public FsRoot(bool caseSensitive)
        {
            _segmentNameComparer = new DynamicStringComparer(caseSensitive);
            _segmentNameComparer.PropertyValueChanging += SegmentNameComparer_PropertyValueChanging;
            _segmentNameComparer.PropertyValueChanged += SegmentNameComparer_PropertyValueChanged;
            _container = new ComponentList.AttachableContainer(this);
            _messagesList = new ComponentList<CrawlMessage>(_container);
            _childNodes = new ComponentList<IFsChildNode>(_container);
        }

        private void SegmentNameComparer_PropertyValueChanging(object sender, IPropertyValueChangeEventArgs<bool> e)
        {
            RaisePropertyChanging(nameof(CaseSensitive), e.OldValue, e.NewValue);
        }

        private void SegmentNameComparer_PropertyValueChanged(object sender, IPropertyValueChangeEventArgs<bool> e)
        {
            RaisePropertyChanged(nameof(CaseSensitive), e.OldValue, e.NewValue);
        }

        private static string CalculateName(FileUri rootUri, string volumeName, VolumeIdentifier identifier) => (rootUri.IsEmpty()) ? ((string.IsNullOrWhiteSpace(volumeName)) ? identifier.ToString() : volumeName) : rootUri.ToLocalPath();

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
            if (!(message is null))
            {
                segments = Array.Empty<IFsDirectory>();
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
            segments = Array.Empty<IFsDirectory>();
            return false;
        }

        public bool Equals(IFsChildNode x, IFsChildNode y)
        {
            if (x is null)
                return y is null;
            if (y is null)
                return false;
            return ReferenceEquals(x, y) || _segmentNameComparer.Equals(x.Name, y.Name);
        }

        public int GetHashCode(IFsChildNode obj) => _segmentNameComparer.GetHashCode(obj?.Name);

        public override string ToString()
        {
            if (RootUri.IsEmpty())
                return (VolumeName is null) ? "" : " " + VolumeName.Trim();
            if (string.IsNullOrWhiteSpace(VolumeName))
                return RootUri.ToLocalPath();
            return RootUri.ToLocalPath() + " " + VolumeName.Trim();
        }

        private void RaisePropertyChanging<T>(string propertyName, T oldValue, T newValue)
        {
            PropertyValueChangingEventArgs<T> args = new PropertyValueChangingEventArgs<T>(propertyName, oldValue, newValue);
            PropertyValueChanging?.Invoke(this, args);
            PropertyChanging?.Invoke(this, args);
        }

        private void RaisePropertyChanged<T>(string propertyName, T oldValue, T newValue)
        {
            PropertyValueChangedEventArgs<T> args = new PropertyValueChangedEventArgs<T>(propertyName, oldValue, newValue);
            try { PropertyValueChanged?.Invoke(this, args); }
            finally { PropertyChanged?.Invoke(this, args); }
        }

        public bool Equals(FsRoot other) => !(other is null) && (ReferenceEquals(this, other) || _identifier.Equals(other._identifier) ||
            DynamicStringComparer.IgnoreCaseEquals(_volumeName, other._volumeName) || (DynamicStringComparer.IgnoreCaseEquals(_rootUri.Host, other._rootUri.Host) &&
            _segmentNameComparer.Equals(_rootUri.GetPathComponents(), other._rootUri.GetPathComponents())));

        public override bool Equals(object obj) => Equals(obj as FsRoot);

        public override int GetHashCode() => _identifier.GetHashCode();
    }
}

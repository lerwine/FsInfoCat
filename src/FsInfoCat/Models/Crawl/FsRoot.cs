using FsInfoCat.Models.Volumes;
using FsInfoCat.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;

namespace FsInfoCat.Models.Crawl
{
    // TODO: IEqualityComparer<IFsChildNode> may need to be replaced by using IVolumeSet provider due to nested case-sensitivity difference issues
    public sealed class FsRoot : ComponentBase, IVolumeInfo, IFsDirectory, IEqualityComparer<IFsChildNode>
    {
        private FileUri _rootUri = new FileUri("");
        private string _driveFormat = "";
        private string _volumeName = "";

        private readonly ComponentList.AttachableContainer _container;
        private ComponentList<CrawlMessage> _messagesList;
        private ComponentList<IFsChildNode> _childNodes;
        private bool _caseSensitive;
        private StringComparer _segmentNameComparer;
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
            get => _caseSensitive;
            set
            {
                Monitor.Enter(this);
                try
                {
                    // TODO: Invoke change events
                    if (_caseSensitive == value)
                        return;
                    RaisePropertyChanging(nameof(ChildNodes), _caseSensitive, value);
                    _caseSensitive = value;
                    _segmentNameComparer = null;
                    RaisePropertyChanged(nameof(ChildNodes), !_caseSensitive, _caseSensitive);
                }
                finally { Monitor.Exit(this); }
            }
        }

#warning This should be a method.
        public IEqualityComparer<string> PathComparer
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

        [Obsolete("No good way to try determine equality when parent directory case sensitivity can be different. User IVolumeSetProvider, instead")]
        public bool Equals(FsRoot other)
        {
            return null != other && (ReferenceEquals(this, other) || (DriveType == other.DriveType
                && string.Equals(DriveFormat, other.DriveFormat, StringComparison.InvariantCultureIgnoreCase)
                && string.Equals(VolumeName, other.VolumeName, StringComparison.InvariantCultureIgnoreCase)
                && RootUri.Equals(other.RootUri, (CaseSensitive || !other.CaseSensitive) ? PathComparer : other.PathComparer)));
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
    }
}

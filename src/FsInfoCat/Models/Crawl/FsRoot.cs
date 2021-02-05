using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using FsInfoCat.Models.Volumes;
using FsInfoCat.Util;

namespace FsInfoCat.Models.Crawl
{
    public sealed class FsRoot : ComponentBase, IVolumeInfo, IFsDirectory, IEquatable<FsRoot>, IEqualityComparer<IFsChildNode>
    {
        private readonly ComponentList.AttachableContainer _container;
        private ComponentList<CrawlMessage> _messagesList;
        private ComponentList<IFsChildNode> _childNodes;

        /// <summary>
        /// Gets the full path name of the volume root directory.
        /// </summary>
        public string RootPathName { get; set; }

        /// <summary>
        /// Gets the name of the file system.
        /// </summary>
        public string DriveFormat { get; set; }

        public DriveType DriveType { get; set; }

        /// <summary>
        /// Gets the name of the volume.
        /// </summary>
        public string VolumeName { get; set; }

#warning Data type does not support file shares or linux
        [Obsolete("Use UniqueIdentifier instead")]
        public uint SerialNumber { get; set; }

        /*
            Get-WmiObject -Class 'Win32_LogicalDisk' can be used to get 32-bit serial number in windows
            lsblk -a -b -f -J -o NAME,LABEL,MOUNTPOINT,SIZE,FSTYPE,UUID
        */
        public UniqueIdentifier UniqueIdentifier { get; set; }

        string INamedComponent.Name => (string.IsNullOrWhiteSpace(RootPathName)) ? ((string.IsNullOrWhiteSpace(VolumeName)) ? SerialNumber.ToString() : VolumeName) : RootPathName;

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

        public bool CaseSensitive { get; set; }

        public FsRoot(IVolumeInfo driveInfo) : this()
        {
            if (driveInfo is null)
                throw new ArgumentNullException(nameof(driveInfo));
            DriveFormat = driveInfo.DriveFormat;
            VolumeName = driveInfo.VolumeName;
            SerialNumber = driveInfo.SerialNumber;
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
                String.Equals(VolumeName, other.VolumeName, StringComparison.InvariantCultureIgnoreCase) && RootPathName.Equals(other.RootPathName)));
        }

        public override bool Equals(object obj)
        {
            return null != obj && obj is FsRoot && Equals((FsRoot)obj);
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
            if (string.IsNullOrWhiteSpace(RootPathName))
                return (VolumeName is null) ? "" : " " + VolumeName.Trim();
            if (String.IsNullOrWhiteSpace(VolumeName))
                return RootPathName.Trim();
            return RootPathName.Trim() + Path.PathSeparator + " " + VolumeName.Trim();
        }
        public static string NormalizePath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return "";
            path = Path.GetFullPath(path);
            while (string.IsNullOrEmpty(Path.GetFileName(path)))
            {
                string d = Path.GetDirectoryName(path);
                if (string.IsNullOrEmpty(d))
                    break;
                path = d;
            }
            return path;
        }

        private static IFsDirectory ImportDirectory(Collection<FsRoot> fsRoots, string path, Dictionary<string, IVolumeInfo> drives, out string realPath)
        {
            string key = path;
            FsRoot root;
            if (drives.ContainsKey(key) || null != (key = drives.Keys.FirstOrDefault(d => StringComparer.InvariantCultureIgnoreCase.Equals(d, path))))
            {
                if (null == (root = fsRoots.FirstOrDefault(r => StringComparer.InvariantCulture.Equals(key))))
                {
                    root = new FsRoot(drives[key]);
                    fsRoots.Add(root);
                }
                realPath = key;
                return root;
            }
            string name = Path.GetFileName(path);
            string directoryName = Path.GetDirectoryName(path);
            if (string.IsNullOrEmpty(directoryName))
                root = null;
            else
            {
                IFsDirectory parent = ImportDirectory(fsRoots, directoryName, drives, out realPath);
                if (null != parent)
                {
                    string[] names = Directory.GetDirectories(directoryName).Select(p => Path.GetFileName(p)).ToArray();
                    if (names.Any(n => StringComparer.InvariantCulture.Equals(n, name)) || null != (name = names.FirstOrDefault(n => StringComparer.InvariantCultureIgnoreCase.Equals(n, name))))
                    {
                        FsDirectory result = parent.ChildNodes.OfType<FsDirectory>().FirstOrDefault(d => StringComparer.InvariantCulture.Equals(d.Name, name));
                        if (result is null)
                        {
                            result = new FsDirectory() { Name = name };
                            parent.ChildNodes.Add(result);
                        }
                        realPath = Path.Combine(realPath, name);
                        return result;
                    }
                }
            }
            realPath = null;
            return null;
        }


        public static IFsDirectory ImportDirectory(Collection<FsRoot> fsRoots, string path, Func<IEnumerable<IVolumeInfo>> getVolumes, out string realPath)
        {
            if ((path = NormalizePath(path)).Length > 0 && Directory.Exists(path))
                return ImportDirectory(fsRoots, path, getVolumes().ToDictionary(k => k.RootPathName, v => v), out realPath);
            realPath = null;
            return null;
        }
    }
}

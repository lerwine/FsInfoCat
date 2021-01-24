using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using FsInfoCat.Util;

namespace FsInfoCat.Models.Crawl
{
    public sealed class FsDirectory : IFsDirectory, IFsChildNode
    {
        public string Name { get; set; }

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

        public DateTime CreationTime { get; set; }
        public DateTime LastWriteTime { get; set; }
        public int Attributes { get; set; }

        public static FsRoot GetRoot(FsHost host, DirectoryInfo directory, out IFsDirectory branch)
        {
            FsRoot root;
            if (null == directory.Parent)
            {
                VolumeInformation volumeInformation = new VolumeInformation(directory);
                root = host.Roots.FirstOrDefault(h => h.SerialNumber == volumeInformation.SerialNumber && string.Equals(h.VolumeName, volumeInformation.VolumeName, StringComparison.InvariantCultureIgnoreCase));
                if (null == root)
                {
                    root = new FsRoot
                    {
                        FileSystemName = volumeInformation.FileSystemName,
                        MaxNameLength = volumeInformation.MaxNameLength,
                        Flags = volumeInformation.Flags,
                        RootPathName = volumeInformation.RootPathName,
                        SerialNumber = volumeInformation.SerialNumber,
                        VolumeName = volumeInformation.VolumeName,
                    };
                    host.Roots.Add(root);
                }
                branch = root;
            }
            else
            {
                root = GetRoot(host, directory.Parent, out IFsDirectory parent);
                branch = root.FindByName<FsDirectory>(parent.ChildNodes, directory.Name);
                if (null == branch)
                {
                    FsDirectory childDir = new FsDirectory
                    {
                        Name = directory.Name,
                        CreationTime = directory.CreationTimeUtc,
                        LastWriteTime = directory.LastAccessTimeUtc,
                        Attributes = (int)directory.Attributes
                    };
                    branch = childDir;
                    parent.ChildNodes.Add(childDir);
                }
            }
            return root;
        }

        /// <summary>
        /// Looks for the first nested partial crawl.
        /// </summary>
        /// <param name="message">The <seealso cref="PartialCrawlWarning" /> representing the partial crawl.</param>
        /// <param name="segments">The <seealso cref="IFsDirectory" /> representing the path segments, relative to the current directory, for the parent directory of the partial crawl.
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
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Xml.Serialization;

namespace FsInfoCat.PS.Export
{
    public class Subdirectory : Volume.SubdirectoryBase
    {
        private string _notes;
        private readonly FileBase.Collection _files;
        private readonly Collection _subdirectories;
        private readonly AccessError.Collection _accessErrors;

        [XmlAttribute(nameof(Options))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_Options { get => Options.ToDirectoryCrawlOptionsXml(); set => Options = value.FromXmlDirectoryCrawlOptions(Options); }
#pragma warning restore IDE1006 // Naming Styles
        [XmlIgnore]
        public DirectoryCrawlOptions Options { get; set; }

        internal void SetAllProcessedFlags(bool value)
        {
            IsProcessed = value;
            foreach (File file in Files)
                file.IsProcessed = false;
            foreach (Subdirectory dir in SubDirectories)
                dir.SetAllProcessedFlags(value);
        }

        [XmlAttribute(nameof(Status))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_Status { get => Status.ToDirectoryStatusXml(); set => Status = value.FromXmlDirectoryStatus(Status); }
#pragma warning restore IDE1006 // Naming Styles
        [XmlIgnore]
        public DirectoryStatus Status { get; set; }

        [XmlAttribute(nameof(LastAccessed))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_LastAccessed { get => LastAccessed.ToDateTimeXml(); set => LastAccessed = value.FromXmlDateTime(LastAccessed); }
#pragma warning restore IDE1006 // Naming Styles
        [XmlIgnore]
        public DateTime LastAccessed { get; set; }

        [XmlAttribute(nameof(CreationTime))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_CreationTime { get => CreationTime.ToDateTimeXml(); set => CreationTime = value.FromXmlDateTime(CreationTime); }
#pragma warning restore IDE1006 // Naming Styles
        [XmlIgnore]
        public DateTime CreationTime { get; set; }

        [XmlAttribute(nameof(LastWriteTime))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_LastWriteTime { get => LastWriteTime.ToDateTimeXml(); set => LastWriteTime = value.FromXmlDateTime(LastWriteTime); }
#pragma warning restore IDE1006 // Naming Styles
        [XmlIgnore]
        public DateTime LastWriteTime { get; set; }

        [XmlElement]
        public string Notes { get => _notes; set => _notes = value.NullIfWhitespace(); }

        internal bool IsProcessed { get; set; }

        [XmlElement(nameof(CrawlConfiguration))]
        public CrawlConfiguration CrawlConfiguration { get; set; }

        [XmlElement(nameof(File))]
        public Collection<File> Files
        {
            get => _files;
            set
            {
                if (ReferenceEquals(_files, value))
                    return;
                if (value is FileBase.Collection)
                    throw new InvalidOperationException();
                Monitor.Enter(SyncRoot);
                try
                {
                    _files.Clear();
                    _files.AddRange(value);
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        [XmlElement(nameof(Subdirectory))]
        public Collection<Subdirectory> SubDirectories
        {
            get => _subdirectories;
            set
            {
                if (ReferenceEquals(_subdirectories, value))
                    return;
                if (value is Collection)
                    throw new InvalidOperationException();
                Monitor.Enter(SyncRoot);
                try
                {
                    _subdirectories.Clear();
                    _subdirectories.AddRange(value);
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        [XmlElement(nameof(AccessError))]
        public Collection<AccessError> AccessErrors
        {
            get => _accessErrors;
            set
            {
                if (ReferenceEquals(_accessErrors, value))
                    return;
                if (value is AccessError.Collection)
                    throw new InvalidOperationException();
                Monitor.Enter(SyncRoot);
                try
                {
                    _accessErrors.Clear();
                    _accessErrors.AddRange(value);
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        public Subdirectory()
        {
            _files = new FileBase.Collection(this);
            _subdirectories = new Collection(this);
            _accessErrors = new AccessError.Collection(this);
        }

        public abstract class FileBase : EntityExportElement, IOwnedElement<Subdirectory>
        {
            private string _name = "";

            [XmlAttribute]
            public string Name { get => _name; set => _name = value.EmptyIfNullOrWhiteSpace(); }

            [XmlIgnore]
            public Subdirectory Parent { get; private set; }

            Subdirectory IOwnedElement<Subdirectory>.Owner => Parent;

            internal class Collection : OwnedCollection<Subdirectory, File>
            {
                internal Collection(Subdirectory owner) : base(owner) { }

                internal Collection(Subdirectory owner, IEnumerable<File> items) : base(owner, items) { }

                protected override void OnItemAdding(File item) => item.Parent = Owner;

                protected override void OnItemRemoved(File item) => item.Parent = null;
            }
        }

        public abstract class CrawlConfigurationBase : EntityExportElement
        {
            private string _displayName = "";

            [XmlAttribute]
            public string DisplayName { get => _displayName; set => _displayName = value.TrimmedNotNull(); }

            [XmlIgnore]
            public Subdirectory Root { get; private set; }
        }

        public class AccessError : AccessErrorElement<Subdirectory>, IOwnedElement<Subdirectory>
        {
            private Subdirectory _target;

            public override Subdirectory Target => _target;

            Subdirectory IOwnedElement<Subdirectory>.Owner => _target;

            internal class Collection : OwnedCollection<Subdirectory, AccessError>
            {
                internal Collection(Subdirectory owner) : base(owner) { }

                internal Collection(Subdirectory owner, IEnumerable<AccessError> items) : base(owner, items) { }

                protected override void OnItemAdding(AccessError item) => item._target = Owner;

                protected override void OnItemRemoved(AccessError item) => item._target = null;
            }
        }

        public IEnumerable<File> GetAllFiles() => Files.Concat(SubDirectories.SelectMany(d => d.GetAllFiles()));
    }
}

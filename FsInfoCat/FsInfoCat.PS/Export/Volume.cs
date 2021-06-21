using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Xml.Serialization;

namespace FsInfoCat.PS.Export
{
    public class Volume : FileSystem.VolumeBase
    {
        private string _notes;
        private Subdirectory _rootDirectory;
        private readonly AccessError.Collection _accessErrors;

        [XmlAttribute(nameof(CaseSensitiveSearch))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_CaseSensitiveSearch { get => CaseSensitiveSearch.ToBooleanXml(); set => CaseSensitiveSearch = value.FromXmlBoolean(); }

        internal void SetAllProcessedFlags(bool value)
        {
            IsProcessed = value;
            RootDirectory?.SetAllProcessedFlags(value);
        }
#pragma warning restore IDE1006 // Naming Styles
        [XmlIgnore]
        public bool? CaseSensitiveSearch { get; set; }

        [XmlAttribute(nameof(ReadOnly))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_ReadOnly { get => ReadOnly.ToBooleanXml(); set => ReadOnly = value.FromXmlBoolean(); }
#pragma warning restore IDE1006 // Naming Styles
        [XmlIgnore]
        public bool? ReadOnly { get; set; }

        [XmlAttribute(nameof(MaxNameLength))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_Priority { get => MaxNameLength.ToInt32Xml(); set => MaxNameLength = value.FromXmlInt32(); }
#pragma warning restore IDE1006 // Naming Styles
        [XmlIgnore]
        public int? MaxNameLength { get; set; }

        [XmlAttribute(nameof(Type))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_Type { get => Type.ToDriveTypeXml(); set => Type = value.FromXmlDriveType(Type); }
#pragma warning restore IDE1006 // Naming Styles
        [XmlIgnore]
        public DriveType Type { get; set; }

        [XmlAttribute(nameof(Status))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_Status { get => Status.ToVolumeStatusXml(); set => Status = value.FromXmlVolumeStatus(Status); }
#pragma warning restore IDE1006 // Naming Styles
        [XmlIgnore]
        public VolumeStatus Status { get; set; }

        [XmlElement]
        public string Notes { get => _notes; set => _notes = value.NullIfWhitespace(); }

        internal bool IsProcessed { get; set; }

        [XmlElement]
        public Subdirectory RootDirectory
        {
            get => _rootDirectory;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    Subdirectory oldValue = _rootDirectory;
                    if (value is null)
                    {
                        _rootDirectory = null;
                        if (oldValue is not null)
                            oldValue.Volume = null;
                    }
                    else if (!ReferenceEquals(oldValue, value))
                    {
                        Monitor.Enter(value.SyncRoot);
                        try
                        {
                            if (value.Parent is not null || value.Volume is not null)
                                throw new InvalidOperationException();
                            (_rootDirectory = value).Volume = this;
                        }
                        finally { Monitor.Exit(value.SyncRoot); }
                    }
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

        public Volume()
        {
            _accessErrors = new AccessError.Collection(this);
        }

        public abstract class SubdirectoryBase : EntityExportElement, IOwnedElement<Subdirectory>
        {
            private string _name = "";
            private Volume _volume;

            [XmlAttribute]
            public string Name { get => _name; set => _name = value.EmptyIfNullOrWhiteSpace(); }

            [XmlIgnore]
            public Subdirectory Parent { get; private set; }

            [XmlIgnore]
            public Volume Volume
            {
                get
                {
                    Subdirectory parent = Parent;
                    return (parent is null) ? _volume : parent.Volume;
                }
                internal set
                {
                    Monitor.Enter(SyncRoot);
                    try
                    {
                        if (Parent is not null)
                            throw new InvalidOperationException();
                        if (value is null)
                        {
                            if (_volume is not null && ReferenceEquals(_volume.RootDirectory, this))
                                throw new InvalidOperationException();
                        }
                        else if (_volume is not null)
                        {
                            if (ReferenceEquals(_volume, value))
                                return;
                            if (!ReferenceEquals(value.RootDirectory, this))
                                throw new InvalidOperationException();
                        }
                        _volume = value;
                    }
                    finally { Monitor.Exit(SyncRoot); }
                }
            }

            Subdirectory IOwnedElement<Subdirectory>.Owner => Parent;

            internal class Collection : OwnedCollection<Subdirectory, Subdirectory>
            {
                internal Collection(Subdirectory owner) : base(owner) { }

                internal Collection(Subdirectory owner, IEnumerable<Subdirectory> items) : base(owner, items) { }

                protected override void OnItemAdding(Subdirectory item)
                {
                    if (item._volume is not null)
                        throw new InvalidOperationException();
                    item.Volume = null;
                    item.Parent = Owner;
                }

                protected override void OnItemRemoved(Subdirectory item)
                {
                    item.Parent = null;
                    item.Volume = null;
                }
            }
        }

        public class AccessError : AccessErrorElement<Volume>, IOwnedElement<Volume>
        {
            private Volume _target;

            public override Volume Target => _target;

            Volume IOwnedElement<Volume>.Owner => _target;

            internal class Collection : OwnedCollection<Volume, AccessError>
            {
                internal Collection(Volume owner) : base(owner) { }

                internal Collection(Volume owner, IEnumerable<AccessError> items) : base(owner, items) { }

                protected override void OnItemAdding(AccessError item) => item._target = Owner;

                protected override void OnItemRemoved(AccessError item) => item._target = null;
            }
        }
    }
}

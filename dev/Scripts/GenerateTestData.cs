using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;

namespace GenerateTestData
{
    public interface IChild<TParent>
    {
        object SyncRoot { get; }
        TParent Parent { get; set; }
    }
    public class OneToOneRelationship<TParent, TChild>
        where TChild : class, IChild<TParent>
    {
        private TChild _child;
        internal object SyncRoot { get; } = new object();
        public TParent Parent { get; }
        public TChild Child
        {
            get { return _child; }
            set
            {
                TChild oldValue;
                Monitor.Enter(SyncRoot);
                try
                {
                    oldValue = _child;
                    if (value is null)
                    {
                        if (oldValue is null)
                            return;
                        _child = null;
                    }
                    else
                    {
                        if (ReferenceEquals(oldValue, value))
                            return;
                        Monitor.Enter(value.SyncRoot);
                        try
                        {
                            if (!(value.Parent is null))
                                throw new InvalidOperationException();
                            (_child = value).Parent = Parent;
                        }
                        finally { Monitor.Exit(value.SyncRoot); }
                    }
                    if (!(oldValue is null))
                        oldValue.Parent = null;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }
        internal OneToOneRelationship(TParent parent)
        {
            Parent = parent ?? throw new ArgumentNullException(nameof(parent));
        }
    }
    public class OneToManyRelationship<TParent, TChild> : Collection<TChild>
        where TChild : class, IChild<TParent>
    {
        internal object SyncRoot { get; } = new object();
        public TParent Parent { get; }
        internal OneToManyRelationship(TParent parent)
        {
            Parent = parent ?? throw new ArgumentNullException(nameof(parent));
        }
        protected override void ClearItems()
        {
            TChild[] removed;
            Monitor.Enter(SyncRoot);
            try
            {
                removed = Items.ToArray();
                base.ClearItems();
                IEnumerator enumerator = removed.GetEnumerator();
                OnItemsRemoved(enumerator);
            }
            finally { Monitor.Exit(SyncRoot); }
        }
        private void OnItemsRemoved(IEnumerator enumerator)
        {
            if (enumerator.MoveNext())
                try { ((TChild)enumerator.Current).Parent = null; }
                finally { OnItemsRemoved(enumerator); }
        }
        protected override void InsertItem(int index, TChild item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            Monitor.Enter(SyncRoot);
            try
            {
                Monitor.Enter(item.SyncRoot);
                try
                {
                    if (item.Parent != null)
                        throw new InvalidOperationException();
                    base.InsertItem(index, item);
                    item.Parent = this;
                }
                finally { Monitor.Exit(item.SyncRoot); }
            }
            finally { Monitor.Exit(SyncRoot); }
        }
        protected override void RemoveItem(int index)
        {
            Monitor.Enter(SyncRoot);
            try
            {
                TChild item = Items[index];
                base.RemoveItem(index);
                item.Parent = null;
            }
            finally { Monitor.Exit(SyncRoot); }
        }
        protected override void SetItem(int index, TChild item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            Monitor.Enter(SyncRoot);
            try
            {
                Monitor.Enter(item.SyncRoot);
                try
                {
                    if (item.Parent != null)
                        throw new InvalidOperationException();
                    TChild oldItem = Items[index];
                    base.SetItem(index, item);
                    try { item.Parent = this; }
                    finally { oldItem.Parent = null; }
                }
                finally { Monitor.Exit(item.SyncRoot); }
            }
            finally { Monitor.Exit(SyncRoot); }
        }
    }
    public interface IDbEntity
    {
        DateTime CreatedOn { get; set; }
        DateTime ModifiedOn { get; set; }
    }
    public abstract class DbEntityCollection<T> : Collection<T>
        where T : class, IDbEntity
    {
        internal object SyncRoot { get; } = new object();
        protected DbEntityCollection(IEnumerable<T> list) : base(list.Where(e => e != null).Distinct().ToList())
        {
            Monitor.Enter(SyncRoot);
            try
            {
                foreach (T item in Items)
                    OnItemAdding(item);
                foreach (T item in Items)
                    OnItemAdded(item);
            }
            catch (Exception exception)
            {
                throw new ArgumentException(exception.Message, "list", exception);
            }
            finally { Monitor.Exit(SyncRoot); }
        }
        protected DbEntityCollection() { }
        protected abstract void OnItemRemoved(T item);
        protected abstract void OnItemAdding(T item);
        protected abstract void OnItemAdded(T item);
        protected override void ClearItems()
        {
            Monitor.Enter(SyncRoot);
            try
            {
            }
            finally { Monitor.Exit(SyncRoot); }
            T[] items = Items.ToArray();
            base.ClearItems();
            IEnumerator enumerator = items.GetEnumerator();
            OnItemsRemoved(enumerator);
        }
        private void OnItemsRemoved(IEnumerator enumerator)
        {
            if (enumerator.MoveNext())
                try { OnItemRemoved((T)enumerator.Current); }
                finally { OnItemsRemoved(enumerator); }
        }
        protected override void InsertItem(int index, T item)
        {
            if (item == null)
                 throw new ArgumentNullException("item");
            Monitor.Enter(SyncRoot);
            try
            {
            }
            finally { Monitor.Exit(SyncRoot); }
            OnItemAdding(item);
            base.InsertItem(index, item);
            OnItemAdded(item);
        }
        protected override void RemoveItem(int index)
        {
            Monitor.Enter(SyncRoot);
            try
            {
            }
            finally { Monitor.Exit(SyncRoot); }
            T item = Items[index];
            base.RemoveItem(index);
            OnItemRemoved(item);
        }
        protected override void SetItem(int index, T item)
        {
            if (item == null)
                 throw new ArgumentNullException("item");
            Monitor.Enter(SyncRoot);
            try
            {
            }
            finally { Monitor.Exit(SyncRoot); }
            T oldItem = Items[index];
            if (ReferenceEquals(item, oldItem))
                return;
            OnItemAdding(item);
            base.SetItem(index, item);
            try { OnItemRemoved(oldItem); }
            finally { OnItemAdded(item); }
        }
    }
    public abstract class DbEntity : IDbEntity
    {
        public const int Default_MaxNameLength = 255;
        public const uint Default_MaxRecursionDepth = 256;
        private DateTime _createdOn;
        private DateTime _modifiedOn;
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlAttribute("CreatedOn")]
        public string __CreatedOn_Text { get { return FromDateTime(_createdOn); } set { _createdOn = ToDateTime(value, _createdOn).Value; } }
        [XmlIgnore]
        public DateTime CreatedOn { get { return _createdOn; } set { _createdOn = value; } }
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlAttribute("ModifiedOn")]
        public string __ModifiedOn_Text { get { return FromDateTime(_modifiedOn); } set { _modifiedOn = ToDateTime(value, _modifiedOn).Value; } }
        [XmlIgnore]
        public DateTime ModifiedOn { get { return _modifiedOn; } set { _modifiedOn = value; } }
        protected DbEntity() { _createdOn = _modifiedOn = DateTime.Now; }
        protected string FromDateTime(DateTime? value) { return value.HasValue ? value.Value.ToString("yyyy-MM-dd HH:mm:ss") : null; }
        protected DateTime? ToDateTime(string value, DateTime? defaultValue = null)
        {
            DateTime dateTime;
            if (value == null || (value = value.Trim()).Length == 0 || !DateTime.TryParse(value, out dateTime))
                return defaultValue;
            return dateTime;
        }
        protected string FromGuid(Guid? value) { return value.HasValue ? XmlConvert.ToString(value.Value) : null; }
        protected Guid? ToGuid(string value, Guid? defaultValue = null)
        {
            if (value != null && (value = value.Trim()).Length > 0)
                try { return XmlConvert.ToGuid(value); } catch { }
            return defaultValue;
        }
        protected string FromDriveType(DriveType? value) { return value.HasValue ? XmlConvert.ToString((int)value.Value) : null; }
        protected DriveType? ToDriveType(string value, DriveType? defaultValue = null) { return (DriveType?)(defaultValue.HasValue ? ToInt32(value, (int)defaultValue.Value) : ToInt32(value)); }
        protected string FromBoolean(bool? value, bool? defaultValue = null)
        {
            if (value.HasValue)
                return (defaultValue.HasValue && value.Value == defaultValue.Value) ? null : XmlConvert.ToString(value.Value);
            return defaultValue.HasValue ? "" : null;
        }
        protected bool? ToBoolean(string value, bool? defaultValue = null)
        {
            if (value != null && (value = value.Trim()).Length > 0)
                try { return XmlConvert.ToBoolean(value); } catch { }
            return defaultValue;
        }
        protected string FromByte(byte? value, byte? defaultValue = null)
        {
            if (value.HasValue)
                return (defaultValue.HasValue && value.Value == defaultValue.Value) ? null : XmlConvert.ToString(value.Value);
            return defaultValue.HasValue ? "" : null;
        }
        protected byte? ToByte(string value, byte? defaultValue = null)
        {
            if (value != null && (value = value.Trim()).Length > 0)
                try { return XmlConvert.ToByte(value); } catch { }
            return defaultValue;
        }
        protected string FromUInt16(ushort? value, ushort? defaultValue = null)
        {
            if (value.HasValue)
                return (defaultValue.HasValue && value.Value == defaultValue.Value) ? null : XmlConvert.ToString(value.Value);
            return defaultValue.HasValue ? "" : null;
        }
        protected ushort? ToUInt16(string value, ushort? defaultValue = null)
        {
            if (value != null && (value = value.Trim()).Length > 0)
                try { return XmlConvert.ToUInt16(value); } catch { }
            return defaultValue;
        }
        protected string FromInt32(int? value, int? defaultValue = null)
        {
            if (value.HasValue)
                return (defaultValue.HasValue && value.Value == defaultValue.Value) ? null : XmlConvert.ToString(value.Value);
            return defaultValue.HasValue ? "" : null;
        }
        protected int? ToInt32(string value, int? defaultValue = null)
        {
            if (value != null && (value = value.Trim()).Length > 0)
                try { return XmlConvert.ToInt32(value); } catch { }
            return defaultValue;
        }
        protected string FromUInt32(uint? value, uint? defaultValue = null)
        {
            if (value.HasValue)
                return (defaultValue.HasValue && value.Value == defaultValue.Value) ? null : XmlConvert.ToString(value.Value);
            return defaultValue.HasValue ? "" : null;
        }
        protected uint? ToUInt32(string value, uint? defaultValue = null)
        {
            if (value != null && (value = value.Trim()).Length > 0)
                try { return XmlConvert.ToUInt32(value); } catch { }
            return defaultValue;
        }
        protected string FromUInt64(ulong? value, ulong? defaultValue = null)
        {
            if (value.HasValue)
                return (defaultValue.HasValue && value.Value == defaultValue.Value) ? null : XmlConvert.ToString(value.Value);
            return defaultValue.HasValue ? "" : null;
        }
        protected ulong? ToUInt64(string value, ulong? defaultValue = null)
        {
            if (value != null && (value = value.Trim()).Length > 0)
                try { return XmlConvert.ToUInt64(value); } catch { }
            return defaultValue;
        }
        protected string EmptyIfNull(string value) { return (value == null) ? "" : value; }
        protected Collection<T> EmptyIfNull<T>(Collection<T> value) { return (value == null) ? new Collection<T>() : value; }
        protected string NullIfWhitespace(string value) { return string.IsNullOrWhiteSpace(value) ? null : value; }
        protected string TrimmedNotNull(string value) { return (value == null) ? "" : value.Trim(); }
        protected string FromAbsoluteUri(Uri uri) { return (uri is null || !uri.IsAbsoluteUri) ? null : uri.AbsoluteUri; }
        protected Uri ToAbsoluteUri(string value)
        {
            Uri uri;
            return (string.IsNullOrWhiteSpace(value) || !Uri.TryCreate(value, UriKind.Absolute, out uri)) ? null : uri;
        }
    }
    public interface ISynchronizedDbEntity : IDbEntity
    {
        Guid? UpstreamId { get; set; }
        DateTime? LastSynchronizedOn { get; set; }
    }
    public abstract class SynchronizedDbEntity : DbEntity, ISynchronizedDbEntity
    {
        private Guid? _upstreamId;
        private DateTime? _lastSynchronizedOn;

        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlAttribute("UpstreamId")]
        public string __UpstreamId_Text { get { return FromGuid(_upstreamId); } set { _upstreamId = ToGuid(value); } }
        [XmlIgnore]
        public Guid? UpstreamId { get { return _upstreamId; } set { _upstreamId = value; } }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlAttribute("LastSynchronizedOn")]
        public string __LastSynchronizedOn_Text { get { return FromDateTime(_lastSynchronizedOn); } set { _lastSynchronizedOn = ToDateTime(value); } }
        [XmlIgnore]
        public DateTime? LastSynchronizedOn { get { return _lastSynchronizedOn; } set { _lastSynchronizedOn = value; } }
    }
    public abstract class DefaultPkEntity : SynchronizedDbEntity
    {
        private Guid? _id;
        internal object SyncRoot { get; } = new object();
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlAttribute("Id")]
        public string __Id_Text { get { return FromGuid(Id); } set { _id = ToGuid(value); } }
        [XmlIgnore]
        public Guid Id
        {
            get
            {
                Monitor.Enter(SyncRoot);
                try
                {
                }
                finally { Monitor.Exit(SyncRoot); }
                lock (SyncRoot)
                {
                    if (!_id.HasValue)
                        _id = Guid.NewGuid();
                }
                return _id.Value;
            }
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                }
                finally { Monitor.Exit(SyncRoot); }
                _id = value;
            }
        }
    }
    public abstract class AccessError : DbEntity
    {
        private Guid? _id;
        private string _message = "";
        private string _details;
        private int _errorCode = 0;
        internal object SyncRoot { get; } = new object();
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlAttribute("Id")]
        public string __Id_Text { get { return FromGuid(_id); } set { _id = ToGuid(value); } }
        [XmlIgnore]
        public Guid Id
        {
            get
            {
                Monitor.Enter(SyncRoot);
                try
                {
                }
                finally { Monitor.Exit(SyncRoot); }
                lock (SyncRoot)
                {
                    if (!_id.HasValue)
                        _id = Guid.NewGuid();
                }
                return _id.Value;
            }
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                }
                finally { Monitor.Exit(SyncRoot); }
                _id = value;
            }
        }
        [XmlAttribute]
        public string Message { get { return _message; } set { _message = TrimmedNotNull(value); } }
        [XmlAttribute]
        public int ErrorCode { get { return _errorCode; } set { _errorCode = value; } }
        [XmlText]
        public string Details { get { return _details; } set { _details = NullIfWhitespace(value); } }
        [XmlIgnore]
        public abstract Guid? TargetId { get; }
    }
    public sealed class FileAccessError : AccessError, IChild<DbFile>
    {
        internal object SyncRoot { get; } = new object();
        private DbFile _target;
        DbFile IChild<DbFile>.Parent
        {
            get { return _target; }
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (_target != null && _target.AccessErrors.Contains(this))
                    {
                        if (ReferenceEquals(_target, value))
                            return;
                        throw new InvalidOperationException();
                    }

                    if (value != null && !value.AccessErrors.Contains(this))
                        throw new InvalidOperationException();
                    _target = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }
        public override Guid? TargetId
        {
            get
            {
                DbFile target = _target;
                return (target is null) ? null : (Guid?)target.Id;
            }
        }
        [XmlIgnore]
        public DbFile Target
        {
            get { return _target; }
            internal set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                }
                finally { Monitor.Exit(SyncRoot); }
                lock (SyncRoot)
                {
                    if (_target != null && _target.AccessErrors.Contains(this))
                    {
                        if (ReferenceEquals(_target, value))
                            return;
                        throw new InvalidOperationException();
                    }

                    if (value != null && !value.AccessErrors.Contains(this))
                        throw new InvalidOperationException();
                    _target = value;
                }
            }
        }
    }
    public sealed class FileAccessErrorCollection : DbEntityCollection<FileAccessError>
    {
        private DbFile _owner;
        internal FileAccessErrorCollection(DbFile owner) { _owner = owner; }
        internal FileAccessErrorCollection(DbFile owner, IEnumerable<FileAccessError> list) : base(list) { _owner = owner; }
        protected override void OnItemAdding(FileAccessError item)
        {
            if (item.Target != null)
                throw new InvalidOperationException();
        }
        protected override void OnItemAdded(FileAccessError item) { item.Target = _owner; }
        protected override void OnItemRemoved(FileAccessError item) { item.Target = null; }
    }
    public class Comparison : SynchronizedDbEntity
    {
        public Guid TargetFileId { get; set; }
        public bool AreEqual { get; set; }
    }
    public sealed class DbFile : DefaultPkEntity
    {
        private string _name = "";
        private byte _options = 0;
        private DateTime _lastAccessed;
        private DateTime? _lastHashCalculation;
        private string _notes;
        private bool _deleted = false;
        private DateTime _creationTime;
        private DateTime _lastWriteTime;
        private Guid? _extendedPropertyId;
        private ExtendedProperties _extendedProperties;
        private Guid _contentId;
        private ContentInfo _contentInfo;
        private FileAccessErrorCollection _accessErrors;
        private Subdirectory _parent;
        internal object SyncRoot { get; } = new object();
        [XmlAttribute]
        public string Name { get { return _name; } set { _name = TrimmedNotNull(value); } }
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlAttribute("Options")]
        public string __Options_Text { get { return FromByte(_options, 0); } set { _options = ToByte(value, 0).Value; } }
        [XmlIgnore]
        public byte Options { get { return _options; } set { _options = value; } }
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlAttribute("LastAccessed")]
        public string __LastAccessed_Text { get { return FromDateTime(_lastAccessed); } set { _lastAccessed = ToDateTime(value, _lastAccessed).Value; } }
        [XmlIgnore]
        public DateTime LastAccessed { get { return _lastAccessed; } set { _lastAccessed = value; } }
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlAttribute("LastHashCalculation")]
        public string __LastHashCalculation_Text { get { return FromDateTime(_lastHashCalculation); } set { _lastHashCalculation = ToDateTime(value); } }
        [XmlIgnore]
        public DateTime? LastHashCalculation { get { return _lastHashCalculation; } set { _lastHashCalculation = value; } }
        public string Notes { get { return _notes; } set { _notes = NullIfWhitespace(value); } }
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlAttribute("Deleted")]
        public string __Deleted_Text { get { return FromBoolean(_deleted, false); } set { _deleted = ToBoolean(value, false).Value; } }
        [XmlIgnore]
        public bool Deleted { get { return _deleted; } set { _deleted = value; } }
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlAttribute("CreationTime")]
        public string __CreationTime_Text { get { return FromDateTime(_creationTime); } set { _creationTime = ToDateTime(value, _creationTime).Value; } }
        [XmlIgnore]
        public DateTime CreationTime { get { return _creationTime; } set { _creationTime = value; } }
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlAttribute("LastWriteTime")]
        public string __LastWriteTime_Text { get { return FromDateTime(_lastWriteTime); } set { _lastWriteTime = ToDateTime(value, _lastWriteTime).Value; } }
        [XmlIgnore]
        public DateTime LastWriteTime { get { return _lastWriteTime; } set { _lastWriteTime = value; } }
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlAttribute("ExtendedPropertyId")]
        public string __ExtendedPropertyId_Text { get { return FromGuid(ExtendedPropertyId); } set { ExtendedPropertyId = ToGuid(value); } }
        private class ExtendedPropertiesReference : KeyReference<ExtendedProperties>
        {
            private DbFile _target;
            internal ExtendedPropertiesReference(DbFile target) { _target = target; }
            protected override T Find(Guid id)
            {
                Monitor.Enter(_target.SyncRoot);
                try
                {
                    Subdirectory parent = _target.Parent;
                    if (parent != null)
                    {
                        Volume volume = parent.Volume;
                        if (volume != null)
                        {
                            FileSystem fileSystem = volume.FileSystem;
                            if (fileSystem != null)
                            {

                            }
                        }
                    }
                }
                finally { Monitor.Exit(_target.SyncRoot); }
            }
        }
        [XmlIgnore]
        public Guid? ExtendedPropertyId
        {
            get
            {
                lock (SyncRoot)
            }
            set
            {
                _extendedPropertyId = value;
            }
        }
        [XmlIgnore]
        public ExtendedProperties ExtendedProperties
        {
            get { return _extendedProperties; }
            set
            {

            }
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlAttribute("ContentId")]
        public string __ContentId_Text { get { return FromGuid(_contentId); } set { _contentId = ToGuid(value, _contentId).Value; } }
        [XmlIgnore]
        public Guid ContentId { get { return _contentId; } set { _contentId = value; } }
        [XmlElement("AccessError")]
        public Collection<FileAccessError> AccessErrors
        {
            get
            {
                Monitor.Enter(SyncRoot);
                try
                {
                }
                finally { Monitor.Exit(SyncRoot); }
                lock (SyncRoot)
                {
                    if (_accessErrors == null)
                        _accessErrors = new FileAccessErrorCollection(this);
                    return _accessErrors;
                }
            }
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                }
                finally { Monitor.Exit(SyncRoot); }
                lock (SyncRoot)
                {
                    if (value == null)
                    {
                        if (_accessErrors != null)
                            _accessErrors.Clear();
                    }
                    else if (_accessErrors == null)
                        _accessErrors = new FileAccessErrorCollection(this, value);
                    else
                    {
                        _accessErrors.Clear();
                        foreach (FileAccessError item in value.Where(e => e != null).Distinct())
                            _accessErrors.Add(item);
                    }
                }
            }
        }
        [XmlIgnore]
        public Subdirectory Parent
        {
            get { return _parent; }
            set
            {
                lock (SyncRoot)
                {
                    if ((_parent != null && _parent.Files.Contains(this)) ||
                            (value != null && !value.Files.Contains(this)))
                        throw new InvalidOperationException();
                    _parent = value;
                }
            }
        }
    }
    public sealed class SubdirectoryAccessError : AccessError
    {
        private Subdirectory _target;
        internal object SyncRoot { get; } = new object();
        public override Guid? TargetId
        {
            get
            {
                Subdirectory target = _target;
                return (target is null) ? null : (Guid?)target.Id;
            }
        }
        [XmlIgnore]
        public Subdirectory Target
        {
            get { return _target; }
            internal set
            {
                lock (SyncRoot)
                {
                    if (_target != null && _target.AccessErrors.Contains(this))
                    {
                        if (ReferenceEquals(_target, value))
                            return;
                        throw new InvalidOperationException();
                    }

                    if (value != null && !value.AccessErrors.Contains(this))
                        throw new InvalidOperationException();
                    _target = value;
                }
            }
        }
    }
    public sealed class ChildSubdirectoryCollection : DbEntityCollection<Subdirectory>
    {
        private Subdirectory _owner;
        internal ChildSubdirectoryCollection(Subdirectory owner) { _owner = owner; }
        internal ChildSubdirectoryCollection(Subdirectory owner, IEnumerable<Subdirectory> list) : base(list) { _owner = owner; }
        protected override void OnItemAdding(Subdirectory item)
        {
            if (item.Volume != null)
                throw new InvalidOperationException();
            for (Subdirectory parent = _owner.Parent; parent != null; parent = parent.Parent)
            {
                if (ReferenceEquals(parent, item))
                    throw new InvalidOperationException();
            }
        }
        protected override void OnItemAdded(Subdirectory item) { item.Parent = _owner; }
        protected override void OnItemRemoved(Subdirectory item) { item.Parent = null; }
    }
    public sealed class SubdirectoryFileCollection : DbEntityCollection<DbFile>
    {
        private Subdirectory _owner;
        internal SubdirectoryFileCollection(Subdirectory owner) { _owner = owner; }
        internal SubdirectoryFileCollection(Subdirectory owner, IEnumerable<DbFile> list) : base(list) { _owner = owner; }
        protected override void OnItemAdding(DbFile item)
        {
            if (item.Parent != null)
                throw new InvalidOperationException();
        }
        protected override void OnItemAdded(DbFile item) { item.Parent = _owner; }
        protected override void OnItemRemoved(DbFile item) { item.Parent = null; }
    }
    public sealed class SubdirectoryAccessErrorCollection : DbEntityCollection<SubdirectoryAccessError>
    {
        private Subdirectory _owner;
        internal SubdirectoryAccessErrorCollection(Subdirectory owner) { _owner = owner; }
        internal SubdirectoryAccessErrorCollection(Subdirectory owner, IEnumerable<SubdirectoryAccessError> list) : base(list) { _owner = owner; }
        protected override void OnItemAdding(SubdirectoryAccessError item)
        {
            if (item.Target != null)
                throw new InvalidOperationException();
        }
        protected override void OnItemAdded(SubdirectoryAccessError item) { item.Target = _owner; }
        protected override void OnItemRemoved(SubdirectoryAccessError item) { item.Target = null; }
    }
    public sealed class CrawlConfiguration : DefaultPkEntity
    {
        private string _displayName = "";
        private uint _maxRecursionDepth = Default_MaxRecursionDepth;
        private ulong _maxTotalItems = ulong.MaxValue;
        private string _notes;
        private bool _isInactive = false;
        private Subdirectory _target;
        internal object SyncRoot { get; } = new object();
        [XmlAttribute]
        public string DisplayName { get { return _displayName; } set { _displayName = TrimmedNotNull(value); } }
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlAttribute("MaxRecursionDepth")]
        public string __MaxRecursionDepth_Text { get { return FromUInt32(_maxRecursionDepth, Default_MaxRecursionDepth); } set { _maxRecursionDepth = ToUInt32(value, _maxRecursionDepth).Value; } }
        [XmlIgnore]
        public uint MaxRecursionDepth { get { return _maxRecursionDepth; } set { _maxRecursionDepth = value; } }
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlAttribute("MaxTotalItems")]
        public string __MaxTotalItems_Text { get { return FromUInt64(_maxTotalItems, ulong.MaxValue); } set { _maxTotalItems = ToUInt64(value, _maxTotalItems).Value; } }
        [XmlIgnore]
        public ulong MaxTotalItems { get { return _maxTotalItems; } set { _maxTotalItems = value; } }
        [XmlText]
        public string Notes { get { return _notes; } set { _notes = NullIfWhitespace(value); } }
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlAttribute("IsInactive")]
        public string __IsInactive_Text { get { return FromBoolean(_isInactive, false); } set { _isInactive = ToBoolean(value, false).Value; } }
        [XmlIgnore]
        public bool IsInactive { get { return _isInactive; } set { _isInactive = value; } }
        [XmlIgnore]
        public Subdirectory Target
        {
            get { return _target; }
            set
            {
                lock (SyncRoot)
                {
                    if ((_target != null &&  ReferenceEquals(_target.CrawlConfiguration, this)) ||
                            (value != null && !ReferenceEquals(value.CrawlConfiguration, this)))
                        throw new InvalidOperationException();
                    _target = value;
                }
            }
        }
    }
    public sealed class Subdirectory : DefaultPkEntity
    {
        private string _name = "";
        private byte _options = 0;
        private string _notes;
        private byte _status = 0;
        private DateTime _lastAccessed;
        private DateTime _creationTime;
        private DateTime _lastWriteTime;
        private CrawlConfiguration _crawlConfiguration;
        private SubdirectoryFileCollection _files;
        private ChildSubdirectoryCollection _subdirectories;
        private SubdirectoryAccessErrorCollection _accessErrors;
        private Volume _volume;
        private Subdirectory _parent;
        internal object SyncRoot { get; } = new object();
        [XmlAttribute]
        public string Name { get { return _name; } set { _name = TrimmedNotNull(value); } }
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlAttribute("Options")]
        public string __Options_Text { get { return FromByte(_options, 0); } set { _options = ToByte(value, 0).Value; } }
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlAttribute("LastAccessed")]
        public string __LastAccessed_Text { get { return FromDateTime(_lastAccessed); } set { _lastAccessed = ToDateTime(value, _lastAccessed).Value; } }
        [XmlIgnore]
        public DateTime LastAccessed { get { return _lastAccessed; } set { _lastAccessed = value; } }
        [XmlIgnore]
        public byte Options { get { return _options; } set { _options = value; } }
        public string Notes { get { return _notes; } set { _notes = NullIfWhitespace(value); } }
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlAttribute("Status")]
        public string __Status_Text { get { return FromByte(_status, 0); } set { _status = ToByte(value, 0).Value; } }
        [XmlIgnore]
        public byte Status { get { return _status; } set { _status = value; } }
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlAttribute("CreationTime")]
        public string __CreationTime_Text { get { return FromDateTime(_creationTime); } set { _creationTime = ToDateTime(value, _creationTime).Value; } }
        [XmlIgnore]
        public DateTime CreationTime { get { return _creationTime; } set { _creationTime = value; } }
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlAttribute("LastWriteTime")]
        public string __LastWriteTime_Text { get { return FromDateTime(_lastWriteTime); } set { _lastWriteTime = ToDateTime(value, _lastWriteTime).Value; } }
        [XmlIgnore]
        public DateTime LastWriteTime { get { return _lastWriteTime; } set { _lastWriteTime = value; } }
        [XmlElement(IsNullable = false)]
        public CrawlConfiguration CrawlConfiguration
        {
            get { return _crawlConfiguration; }
            set
            {
                lock (SyncRoot)
                {
                    CrawlConfiguration oldValue = _crawlConfiguration;
                    if (value != null)
                    {
                        if (value.Target != null)
                        {
                            if (ReferenceEquals(value.Target, this))
                                return;
                            throw new InvalidOperationException();
                        }
                        try { (_crawlConfiguration = value).Target = this; }
                        finally
                        {
                            if (oldValue != null)
                                oldValue.Target = null;
                        }
                    }
                    else
                    {
                        _crawlConfiguration = null;
                        if (oldValue != null)
                            oldValue.Target = null;
                    }
                }
            }
        }
        [XmlElement("File")]
        public Collection<DbFile> Files
        {
            get
            {
                lock (SyncRoot)
                {
                    if (_files == null)
                        _files = new SubdirectoryFileCollection(this);
                    return _files;
                }
            }
            set
            {
                lock (SyncRoot)
                {
                    if (value == null)
                    {
                        if (_files != null)
                            _files.Clear();
                    }
                    else if (_files == null)
                        _files = new SubdirectoryFileCollection(this, value);
                    else
                    {
                        _files.Clear();
                        foreach (DbFile item in value.Where(e => e != null).Distinct())
                            _files.Add(item);
                    }
                }
            }
        }
        [XmlElement("Subdirectory")]
        public Collection<Subdirectory> Subdirectories
        {
            get
            {
                lock (SyncRoot)
                {
                    if (_subdirectories == null)
                        _subdirectories = new ChildSubdirectoryCollection(this);
                    return _subdirectories;
                }
            }
            set
            {
                lock (SyncRoot)
                {
                    if (value == null)
                    {
                        if (_subdirectories != null)
                            _subdirectories.Clear();
                    }
                    else if (_subdirectories == null)
                        _subdirectories = new ChildSubdirectoryCollection(this, value);
                    else
                    {
                        _subdirectories.Clear();
                        foreach (Subdirectory item in value.Where(e => e != null).Distinct())
                            _subdirectories.Add(item);
                    }
                }
            }
        }
        [XmlElement("AccessError")]
        public Collection<SubdirectoryAccessError> AccessErrors
        {
            get
            {
                lock (SyncRoot)
                {
                    if (_accessErrors == null)
                        _accessErrors = new SubdirectoryAccessErrorCollection(this);
                    return _accessErrors;
                }
            }
            set
            {
                lock (SyncRoot)
                {
                    if (value == null)
                    {
                        if (_accessErrors != null)
                            _accessErrors.Clear();
                    }
                    else if (_accessErrors == null)
                        _accessErrors = new SubdirectoryAccessErrorCollection(this, value);
                    else
                    {
                        _accessErrors.Clear();
                        foreach (SubdirectoryAccessError item in value.Where(e => e != null).Distinct())
                            _accessErrors.Add(item);
                    }
                }
            }
        }
        [XmlIgnore]
        public Volume Volume
        {
            get
            {
                Volume volume = _volume;
                if (volume != null)
                    return volume;
                Subdirectory parent = _parent;
                return (parent == null) ? null : parent.Volume;
            }
            set
            {
                lock (SyncRoot)
                {
                    if (((_volume != null) ? ReferenceEquals(_volume.RootDirectory, this) :
                            (_parent != null && _parent.Subdirectories.Contains(this))) ||
                            (value != null && !ReferenceEquals(value.RootDirectory, this)))
                        throw new InvalidOperationException();
                    _parent = null;
                    _volume = value;
                }
            }
        }
        [XmlIgnore]
        public Subdirectory Parent
        {
            get { return _parent; }
            set
            {
                lock (SyncRoot)
                {
                    if (((_parent != null) ? _parent.Subdirectories.Contains(this) :
                            (_volume != null && ReferenceEquals(_volume.RootDirectory, this))) ||
                            (value != null && !value.Subdirectories.Contains(this)))
                        throw new InvalidOperationException();
                    _volume = null;
                    _parent = value;
                }
            }
        }
        public Subdirectory() { _lastAccessed = _creationTime = _lastWriteTime = CreatedOn; }
    }
    public sealed class VolumeAccessError : AccessError
    {
        private Volume _target;
        internal object SyncRoot { get; } = new object();
        public override Guid? TargetId
        {
            get
            {
                Volume target = _target;
                return (target is null) ? null : (Guid?)target.Id;
            }
        }
        [XmlIgnore]
        public Volume Target
        {
            get { return _target; }
            internal set
            {
                lock (SyncRoot)
                {
                    if (_target != null && _target.AccessErrors.Contains(this))
                    {
                        if (ReferenceEquals(_target, value))
                            return;
                        throw new InvalidOperationException();
                    }

                    if (value != null && !value.AccessErrors.Contains(this))
                        throw new InvalidOperationException();
                    _target = value;
                }
            }
        }
    }
    public sealed class VolumeAccessErrorCollection : DbEntityCollection<VolumeAccessError>
    {
        private Volume _owner;
        internal VolumeAccessErrorCollection(Volume owner) { _owner = owner; }
        internal VolumeAccessErrorCollection(Volume owner, IEnumerable<VolumeAccessError> list) : base(list) { _owner = owner; }
        protected override void OnItemAdding(VolumeAccessError item)
        {
            if (item.Target != null)
                throw new InvalidOperationException();
        }
        protected override void OnItemAdded(VolumeAccessError item) { item.Target = _owner; }
        protected override void OnItemRemoved(VolumeAccessError item) { item.Target = null; }
    }
    public sealed class Volume : DefaultPkEntity
    {
        private string _volumeName = "";
        private string _displayName = "";
        private Uri _identifier;
        private bool? _caseSensitiveSearch;
        private bool? _readOnly;
        private int? _maxNameLength;
        private DriveType _type = DriveType.Unknown;
        private string _notes;
        private byte _status = 0;
        private Subdirectory _rootDirectory;
        private VolumeAccessErrorCollection _accessErrors;
        private FileSystem _fileSystem;
        internal object SyncRoot { get; } = new object();
        [XmlAttribute]
        public string DisplayName { get { return _displayName; } set { _displayName = TrimmedNotNull(value); } }
        [XmlAttribute]
        public string VolumeName { get { return _volumeName; } set { _volumeName = TrimmedNotNull(value); } }
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlAttribute("Identifier")]
        public string __Identifier_Text { get { return FromAbsoluteUri(_identifier); } set { _identifier = ToAbsoluteUri(value); } }
        [XmlIgnore]
        public Uri Identifier { get { return _identifier; } set { _identifier = (value != null && value.IsAbsoluteUri) ? value : null; } }
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlAttribute("CaseSensitiveSearch")]
        public string __CaseSensitiveSearch_Text { get { return FromBoolean(_caseSensitiveSearch); } set { _caseSensitiveSearch = ToBoolean(value); } }
        [XmlIgnore]
        public bool? CaseSensitiveSearch { get { return _caseSensitiveSearch; } set { _caseSensitiveSearch = value; } }
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlAttribute("ReadOnly")]
        public string __ReadOnly_Text { get { return FromBoolean(_readOnly); } set { _readOnly = ToBoolean(value); } }
        [XmlIgnore]
        public bool? ReadOnly { get { return _readOnly; } set { _readOnly = value; } }
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlAttribute("MaxNameLength")]
        public string __MaxNameLength_Text { get { return FromInt32(_maxNameLength); } set { _maxNameLength = ToInt32(value); } }
        [XmlIgnore]
        public int? MaxNameLength { get { return _maxNameLength; } set { _maxNameLength = value; } }
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlAttribute("Type")]
        public string __Type_Text { get { return FromDriveType(_type); } set { _type = ToDriveType(value, _type).Value; } }
        [XmlIgnore]
        public DriveType Type { get { return _type; } set { _type = value; } }
        [XmlElement(IsNullable = false)]
        public string Notes { get { return _notes; } set { _notes = NullIfWhitespace(value); } }
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlAttribute("Status")]
        public string __Status_Text { get { return FromByte(_status, 0); } set { _status = ToByte(value, 0).Value; } }
        [XmlIgnore]
        public byte Status { get { return _status; } set { _status = value; } }
        [XmlElement(IsNullable = false)]
        public Subdirectory RootDirectory
        {
            get { return _rootDirectory; }
            set
            {
                lock (SyncRoot)
                {
                    Subdirectory oldValue = _rootDirectory;
                    if (value != null)
                    {
                        if (value.Parent != null)
                            throw new InvalidOperationException();
                        if (value.Volume != null)
                        {
                            if (ReferenceEquals(value.Volume, this))
                                return;
                            throw new InvalidOperationException();
                        }
                        try { (_rootDirectory = value).Volume = this; }
                        finally
                        {
                            if (oldValue != null)
                                oldValue.Volume = null;
                        }
                    }
                    else
                    {
                        _rootDirectory = null;
                        if (oldValue != null)
                            oldValue.Volume = null;
                    }
                }
            }
        }
        [XmlElement("AccessError")]
        public Collection<VolumeAccessError> AccessErrors
        {
            get
            {
                lock (SyncRoot)
                {
                    if (_accessErrors == null)
                        _accessErrors = new VolumeAccessErrorCollection(this);
                    return _accessErrors;
                }
            }
            set
            {
                lock (SyncRoot)
                {
                    if (value == null)
                    {
                        if (_accessErrors != null)
                            _accessErrors.Clear();
                    }
                    else if (_accessErrors == null)
                        _accessErrors = new VolumeAccessErrorCollection(this, value);
                    else
                    {
                        _accessErrors.Clear();
                        foreach (VolumeAccessError item in value.Where(e => e != null).Distinct())
                            _accessErrors.Add(item);
                    }
                }
            }
        }
        [XmlIgnore]
        public FileSystem FileSystem
        {
            get { return _fileSystem; }
            internal set
            {
                lock (SyncRoot)
                {
                    if (_fileSystem != null && _fileSystem.Volumes.Contains(this))
                    {
                        if (ReferenceEquals(_fileSystem, value))
                            return;
                        throw new InvalidOperationException();
                    }

                    if (value != null && !value.Volumes.Contains(this))
                        throw new InvalidOperationException();
                    _fileSystem = value;
                }
            }
        }
    }
    public sealed class SymbolicName : DefaultPkEntity
    {
        private string _name = "";
        private int _priority = 0;
        private string _notes;
        private bool _isInactive = false;
        private FileSystem _fileSystem;
        internal object SyncRoot { get; } = new object();
        [XmlAttribute]
        public string Name { get { return _name; } set { _name = TrimmedNotNull(value); } }
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlAttribute("Priority")]
        public string __Priority_Text { get { return FromInt32(_priority, 0); } set { _priority = ToInt32(value, 0).Value; } }
        [XmlIgnore]
        public int Priority { get { return _priority; } set { _priority = value; } }
        [XmlText]
        public string Notes { get { return _notes; } set { _notes = NullIfWhitespace(value); } }
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlAttribute("IsInactive")]
        public string __IsInactive_Text { get { return FromBoolean(_isInactive, false); } set { _isInactive = ToBoolean(value, false).Value; } }
        [XmlIgnore]
        public bool IsInactive { get { return _isInactive; } set { _isInactive = value; } }
        [XmlIgnore]
        public FileSystem FileSystem
        {
            get { return _fileSystem; }
            internal set
            {
                lock (SyncRoot)
                {
                    if (_fileSystem != null && _fileSystem.SymbolicNames.Contains(this))
                    {
                        if (ReferenceEquals(_fileSystem, value))
                            return;
                        throw new InvalidOperationException();
                    }

                    if (value != null && !value.SymbolicNames.Contains(this))
                        throw new InvalidOperationException();
                    _fileSystem = value;
                }
            }
        }
    }
    public sealed class FileSystemSymbolicNameCollection : DbEntityCollection<SymbolicName>
    {
        private FileSystem _owner;
        internal FileSystemSymbolicNameCollection(FileSystem owner) { _owner = owner; }
        internal FileSystemSymbolicNameCollection(FileSystem owner, IEnumerable<SymbolicName> list) : base(list) { _owner = owner; }
        protected override void OnItemAdding(SymbolicName item)
        {
            if (item.FileSystem != null)
                throw new InvalidOperationException();
        }
        protected override void OnItemAdded(SymbolicName item) { item.FileSystem = _owner; }
        protected override void OnItemRemoved(SymbolicName item) { item.FileSystem = null; }
    }
    public sealed class FileSystemVolumeCollection : DbEntityCollection<Volume>
    {
        private FileSystem _owner;
        internal FileSystemVolumeCollection(FileSystem owner) { _owner = owner; }
        internal FileSystemVolumeCollection(FileSystem owner, IEnumerable<Volume> list) : base(list) { _owner = owner; }
        protected override void OnItemAdding(Volume item)
        {
            if (item.FileSystem != null)
                throw new InvalidOperationException();
        }
        protected override void OnItemAdded(Volume item) { item.FileSystem = _owner; }
        protected override void OnItemRemoved(Volume item) { item.FileSystem = null; }
    }
    public sealed class FileSystem : DefaultPkEntity
    {
        private string _displayName = "";
        private bool _caseSensitiveSearch = false;
        private bool _readOnly = false;
        private int _maxNameLength = Default_MaxNameLength;
        private DriveType? _defaultDriveType;
        private string _notes;
        private bool _isInactive = false;
        private SampleData _sampleData;
        private FileSystemVolumeCollection _volumes;
        private FileSystemSymbolicNameCollection _symbolicNames;
        internal object SyncRoot { get; } = new object();
        [XmlAttribute]
        public string DisplayName { get { return _displayName; } set { _displayName = TrimmedNotNull(value); } }
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlAttribute("CaseSensitiveSearch")]
        public string __CaseSensitiveSearch_Text { get { return FromBoolean(_caseSensitiveSearch, false); } set { _caseSensitiveSearch = ToBoolean(value, false).Value; } }
        [XmlIgnore]
        public bool CaseSensitiveSearch { get { return _caseSensitiveSearch; } set { _caseSensitiveSearch = value; } }
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlAttribute("ReadOnly")]
        public string __ReadOnly_Text { get { return FromBoolean(_readOnly, false); } set { _readOnly = ToBoolean(value, false).Value; } }
        [XmlIgnore]
        public bool ReadOnly { get { return _readOnly; } set { _readOnly = value; } }
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlAttribute("MaxNameLength")]
        public string __MaxNameLength_Text { get { return FromInt32(_maxNameLength, Default_MaxNameLength); } set { _maxNameLength = ToInt32(value, Default_MaxNameLength).Value; } }
        [XmlIgnore]
        public int MaxNameLength { get { return _maxNameLength; } set { _maxNameLength = value; } }
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlAttribute("DefaultDriveType")]
        public string __DefaultDriveType_Text { get { return FromDriveType(_defaultDriveType); } set { _defaultDriveType = ToDriveType(value); } }
        [XmlIgnore]
        public DriveType? DefaultDriveType { get { return _defaultDriveType; } set { _defaultDriveType = value; } }
        [XmlElement(IsNullable = false)]
        public string Notes { get { return _notes; } set { _notes = NullIfWhitespace(value); } }
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlAttribute("IsInactive")]
        public string __IsInactive_Text { get { return FromBoolean(_isInactive, false); } set { _isInactive = ToBoolean(value, false).Value; } }
        [XmlIgnore]
        public bool IsInactive { get { return _isInactive; } set { _isInactive = value; } }
        [XmlIgnore]
        public SampleData SampleData
        {
            get { return _sampleData; }
            internal set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (_sampleData != null && _sampleData.FileSystems.Contains(this))
                    {
                        if (ReferenceEquals(_sampleData, value))
                            return;
                        throw new InvalidOperationException();
                    }
                    if (value != null && !value.FileSystems.Contains(this))
                        throw new InvalidOperationException();
                    _sampleData = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }
        [XmlElement("SymbolicName")]
        public Collection<SymbolicName> SymbolicNames
        {
            get
            {
                lock (SyncRoot)
                {
                    if (_symbolicNames == null)
                        _symbolicNames = new FileSystemSymbolicNameCollection(this);
                    return _symbolicNames;
                }
            }
            set
            {
                lock (SyncRoot)
                {
                    if (value == null)
                    {
                        if (_symbolicNames != null)
                            _symbolicNames.Clear();
                    }
                    else if (_symbolicNames == null)
                        _symbolicNames = new FileSystemSymbolicNameCollection(this, value);
                    else
                    {
                        _symbolicNames.Clear();
                        foreach (SymbolicName item in value.Where(e => e != null).Distinct())
                            _symbolicNames.Add(item);
                    }
                }
            }
        }
        [XmlElement("Volume")]
        public Collection<Volume> Volumes
        {
            get
            {
                lock (SyncRoot)
                {
                    if (_volumes == null)
                        _volumes = new FileSystemVolumeCollection(this);
                    return _volumes;
                }
            }
            set
            {
                lock (SyncRoot)
                {
                    if (value == null)
                    {
                        if (_volumes != null)
                            _volumes.Clear();
                    }
                    else if (_volumes == null)
                        _volumes = new FileSystemVolumeCollection(this, value);
                    else
                    {
                        _volumes.Clear();
                        foreach (Volume item in value.Where(e => e != null).Distinct())
                            _volumes.Add(item);
                    }
                }
            }
        }
    }
    public class ExtendedProperties : DefaultPkEntity
    {
        public string Kind { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
    public class ContentInfo : DefaultPkEntity
    {
        public long Length { get; set; }
        public byte[] Hash { get; set; }
    }
    public class Redundancy : SynchronizedDbEntity
    {

    }
    public class RedundantSet : DefaultPkEntity
    {

    }
    public sealed class SampleDataFileSystemCollection : DbEntityCollection<FileSystem>
    {
        private SampleData _owner;
        internal FileSystemVolumeCollection(SampleData owner) { _owner = owner; }
        internal FileSystemVolumeCollection(SampleData owner, IEnumerable<Volume> list) : base(list) { _owner = owner; }
        protected override void OnItemAdding(FileSystem item)
        {
            if (item.SampleData != null)
                throw new InvalidOperationException();
        }
        protected override void OnItemAdded(FileSystem item) { item.SampleData = _owner; }
        protected override void OnItemRemoved(FileSystem item) { item.SampleData = null; }
    }
    public class SampleData
    {

    }
}

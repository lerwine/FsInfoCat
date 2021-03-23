using System;
using System.Xml.Serialization;

namespace FsInfoCat.Test.FileUriConverterTestHelpers
{
    public abstract class FsPathDetail<TOwner> : IOwnable<TOwner>, IFsPathDetail
        where TOwner : ISynchronized
    {
        internal object SyncRoot => _syncRoot;
        private readonly object _syncRoot = new object();
        object ISynchronized.SyncRoot => _syncRoot;
        private TOwner _owner;
        internal TOwner Owner
        {
            get => _owner;
            set
            {
                lock (_syncRoot)
                    _owner = value;
            }
        }
        TOwner IOwnable<TOwner>.Owner => _owner;
        ISynchronized IOwnable.Owner => _owner;

        [XmlAttribute]
        public bool IsAbsolute { get; set; }

        private UncHostInfo _host;
        [XmlElement(IsNullable = true, Order = 0)]
        public UncHostInfo Host
        {
            get => _host;
            set
            {
                if (value is null)
                {
                    lock (_syncRoot)
                    {
                        if (_host is null)
                            return;
                        lock (_host.SyncRoot)
                        {
                            if (ReferenceEquals(this, _host.Owner))
                                _host.Owner = null;
                            _host = null;
                        }
                    }
                    return;
                }
                lock (value.SyncRoot)
                {
                    lock (_syncRoot)
                    {
                        if (value.Owner is null)
                        {
                            if (_host is null)
                                value.Owner = this;
                            else
                                lock (_host.SyncRoot)
                                {
                                    if (ReferenceEquals(this, _host.Owner))
                                        _host.Owner = null;
                                    _host = null;
                                    value.Owner = this;
                                }
                            _host = value;
                        }
                        else if (!ReferenceEquals(value.Owner, this))
                            throw new InvalidOperationException();
                    }
                }
            }
        }

        private PathSegmentInfo _path;
        [XmlElement(IsNullable = false, Order = 1)]
        public PathSegmentInfo Path
        {
            get => _path;
            set
            {
                if (value is null)
                    throw new ArgumentNullException(nameof(value));
                lock (value.SyncRoot)
                {
                    lock (_syncRoot)
                    {
                        if (value.Owner is null)
                        {
                            if (_path is null)
                                value.Owner = this;
                            else
                                lock (_path.SyncRoot)
                                {
                                    if (ReferenceEquals(this, _path.Owner))
                                        _path.Owner = null;
                                    _path = null;
                                    value.Owner = this;
                                }
                            _path = value;
                        }
                        else if (!ReferenceEquals(value.Owner, this))
                            throw new InvalidOperationException();
                    }
                }
            }
        }

        protected FsPathDetail()
        {
            _path = new PathSegmentInfo() { Owner = this };
        }

        protected bool Equals(FsPathDetail<TOwner> other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            if (IsAbsolute == other.IsAbsolute && _path.Equals(other._path))
            {
                UncHostInfo host = _host;
                return (host is null) ? other._host is null : host.Equals(other._host);
            }
            return false;
        }
    }
}

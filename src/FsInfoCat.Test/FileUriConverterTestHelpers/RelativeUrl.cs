using System;
using System.Xml.Serialization;

namespace FsInfoCat.Test.FileUriConverterTestHelpers
{
    public abstract class RelativeUrl<TOwner> : IOwnable<TOwner>, IRelativeUrl
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

        private PathSegmentInfo _path;
        [XmlElement(IsNullable = false, Order = 0)]
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

        [XmlElement(IsNullable = true, Order = 1)]
        public string Query { get; set; }

        [XmlElement(IsNullable = true, Order = 2)]
        public string Fragment { get; set; }

        private FsPathInfo _localPath;
        [XmlElement(IsNullable = true, Order = 3)]
        public FsPathInfo LocalPath
        {
            get => _localPath;
            set
            {
                if (value is null)
                {
                    lock (_syncRoot)
                    {
                        if (_localPath is null)
                            return;
                        lock (_localPath.SyncRoot)
                        {
                            if (ReferenceEquals(this, _localPath.Owner))
                                _localPath.Owner = null;
                            _localPath = null;
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
                            if (_localPath is null)
                                value.Owner = this;
                            else
                                lock (_localPath.SyncRoot)
                                {
                                    if (ReferenceEquals(this, _localPath.Owner))
                                        _localPath.Owner = null;
                                    _path = null;
                                    value.Owner = this;
                                }
                            _localPath = value;
                        }
                        else if (!ReferenceEquals(value.Owner, this))
                            throw new InvalidOperationException();
                    }
                }
            }
        }

        protected RelativeUrl() => _path = new PathSegmentInfo { Owner = this };

        protected bool BaseEquals(RelativeUrl<TOwner> other)
        {
            if (Query == other.Query && Fragment == other.Fragment && _path.Equals(other._path))
            {
                FsPathInfo lp = LocalPath;
                return (lp is null) ? other.LocalPath is null : lp.Equals(other.LocalPath);
            }
            return false;
        }

        public abstract string GetXPath();
    }
}

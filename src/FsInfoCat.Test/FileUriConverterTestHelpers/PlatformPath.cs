using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace FsInfoCat.Test.FileUriConverterTestHelpers
{
    public sealed class PlatformPath : IOwnable<FilePathTestDataItem>, IEquatable<PlatformPath>
    {
        internal object SyncRoot => _syncRoot;
        private readonly object _syncRoot = new object();
        object ISynchronized.SyncRoot => _syncRoot;
        private FilePathTestDataItem _owner;
        internal FilePathTestDataItem Owner
        {
            get => _owner;
            set
            {
                lock (_syncRoot)
                    _owner = value;
            }
        }
        FilePathTestDataItem IOwnable<FilePathTestDataItem>.Owner => _owner;
        ISynchronized IOwnable.Owner => _owner;

        private AbsoluteMatchedUrl _absoluteUrl;
        [XmlElement(IsNullable = true, Order = 0)]
        public AbsoluteMatchedUrl AbsoluteUrl
        {
            get => _absoluteUrl;
            set
            {
                if (value is null)
                {
                    lock (_syncRoot)
                    {
                        if (_absoluteUrl is null)
                            return;
                        lock (_absoluteUrl.SyncRoot)
                        {
                            if (ReferenceEquals(this, _absoluteUrl.Owner))
                                _absoluteUrl.Owner = null;
                            _absoluteUrl = null;
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
                            if (_absoluteUrl is null)
                                value.Owner = this;
                            else
                                lock (_absoluteUrl.SyncRoot)
                                {
                                    if (ReferenceEquals(this, _absoluteUrl.Owner))
                                        _absoluteUrl.Owner = null;
                                    _absoluteUrl = null;
                                    value.Owner = this;
                                }
                            _absoluteUrl = value;
                        }
                        else if (!ReferenceEquals(value.Owner, this))
                            throw new InvalidOperationException();
                    }
                }
            }
        }

        private RelativeMatchedUrl _relativeUrl;
        [XmlElement(IsNullable = true, Order = 1)]
        public RelativeMatchedUrl RelativeUrl
        {
            get => _relativeUrl;
            set
            {
                if (value is null)
                {
                    lock (_syncRoot)
                    {
                        if (_relativeUrl is null)
                            return;
                        lock (_relativeUrl.SyncRoot)
                        {
                            if (ReferenceEquals(this, _relativeUrl.Owner))
                                _relativeUrl.Owner = null;
                            _relativeUrl = null;
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
                            if (_relativeUrl is null)
                                value.Owner = this;
                            else
                                lock (_relativeUrl.SyncRoot)
                                {
                                    if (ReferenceEquals(this, _relativeUrl.Owner))
                                        _relativeUrl.Owner = null;
                                    _relativeUrl = null;
                                    value.Owner = this;
                                }
                            _relativeUrl = value;
                        }
                        else if (!ReferenceEquals(value.Owner, this))
                            throw new InvalidOperationException();
                    }
                }
            }
        }

        private MatchedFilesystemPath _fileSystem;
        [XmlElement(IsNullable = true, Order = 2)]
        public MatchedFilesystemPath FileSystem
        {
            get => _fileSystem;
            set
            {
                if (value is null)
                {
                    lock (_syncRoot)
                    {
                        if (_fileSystem is null)
                            return;
                        lock (_fileSystem.SyncRoot)
                        {
                            if (ReferenceEquals(this, _fileSystem.Owner))
                                _fileSystem.Owner = null;
                            _fileSystem = null;
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
                            if (_fileSystem is null)
                                value.Owner = this;
                            else
                                lock (_fileSystem.SyncRoot)
                                {
                                    if (ReferenceEquals(this, _fileSystem.Owner))
                                        _fileSystem.Owner = null;
                                    _fileSystem = null;
                                    value.Owner = this;
                                }
                            _fileSystem = value;
                        }
                        else if (!ReferenceEquals(value.Owner, this))
                            throw new InvalidOperationException();
                    }
                }
            }
        }

        public bool Equals([AllowNull] PlatformPath other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            AbsoluteMatchedUrl a = _absoluteUrl;
            if ((a is null) ? other._absoluteUrl is null : a.Equals(other._absoluteUrl))
            {
                RelativeMatchedUrl r = _relativeUrl;
                if ((r is null) ? other._relativeUrl is null : r.Equals(other._relativeUrl))
                {
                    MatchedFilesystemPath f = _fileSystem;
                    return (f is null) ? other._fileSystem is null : f.Equals(other._fileSystem);
                }
            }
            return false;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as PlatformPath);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_absoluteUrl, _relativeUrl, _fileSystem);
        }

        public string GetXPath()
        {
            FilePathTestDataItem owner = Owner;
            if (owner is null)
                return nameof(PlatformPath);
            if (ReferenceEquals(owner.Windows, this))
                return $"{owner.GetXPath()}/{nameof(FilePathTestDataItem.Windows)}";
            if (ReferenceEquals(owner.Linux, this))
                return $"{owner.GetXPath()}/{nameof(FilePathTestDataItem.Linux)}";
            return nameof(PlatformPath);
        }
    }
}

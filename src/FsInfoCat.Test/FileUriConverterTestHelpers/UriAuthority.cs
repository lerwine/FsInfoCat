using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace FsInfoCat.Test.FileUriConverterTestHelpers
{
    public class UriAuthority : IOwnable<IAbsoluteUrl>, IEquatable<UriAuthority>
    {
        internal object SyncRoot => _syncRoot;
        private readonly object _syncRoot = new object();
        object ISynchronized.SyncRoot => _syncRoot;
        private IAbsoluteUrl _owner;
        internal IAbsoluteUrl Owner
        {
            get => _owner;
            set
            {
                lock (_syncRoot)
                    _owner = value;
            }
        }
        IAbsoluteUrl IOwnable<IAbsoluteUrl>.Owner => _owner;
        ISynchronized IOwnable.Owner => _owner;

        private UriScheme _scheme;
        [XmlElement(IsNullable = false, Order = 0)]
        public UriScheme Scheme
        {
            get => _scheme;
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
                            if (_scheme is null)
                                value.Owner = this;
                            else
                                lock (_scheme.SyncRoot)
                                {
                                    if (ReferenceEquals(this, _scheme.Owner))
                                        _scheme.Owner = null;
                                    _scheme = null;
                                    value.Owner = this;
                                }
                            _scheme = value;
                        }
                        else if (!ReferenceEquals(value.Owner, this))
                            throw new InvalidOperationException();
                    }
                }
            }
        }

        private UriUserInfo _userInfo;
        [XmlElement(IsNullable = true, Order = 1)]
        public UriUserInfo UserInfo
        {
            get => _userInfo;
            set
            {
                if (value is null)
                {
                    lock (_syncRoot)
                    {
                        if (_userInfo is null)
                            return;
                        lock (_userInfo.SyncRoot)
                        {
                            if (ReferenceEquals(this, _userInfo.Owner))
                                _userInfo.Owner = null;
                            _userInfo = null;
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
                            if (_userInfo is null)
                                value.Owner = this;
                            else
                                lock (_userInfo.SyncRoot)
                                {
                                    if (ReferenceEquals(this, _userInfo.Owner))
                                        _userInfo.Owner = null;
                                    _userInfo = null;
                                    value.Owner = this;
                                }
                            _userInfo = value;
                        }
                        else if (!ReferenceEquals(value.Owner, this))
                            throw new InvalidOperationException();
                    }
                }
            }
        }

        private UriHostInfo _host;
        [XmlElement(IsNullable = true, Order = 2)]
        public UriHostInfo Host
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

        public UriAuthority()
        {
            _scheme = new UriScheme { Owner = this };
        }

        public string GetHostAndPort()
        {
            UriHostInfo host = _host;
            if (host is null)
                return "";
            string p = host.PortString;
            return (string.IsNullOrEmpty(p)) ? host.Value : $"{host.Value}:{p}";
        }

        public string GetHostName()
        {
            UriHostInfo host = _host;
            return (host is null) ? "" : host.Value;
        }

        public bool Equals([AllowNull] UriAuthority other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            if (_scheme.Equals(other._scheme))
            {
                UriUserInfo userInfo = _userInfo;
                if ((userInfo is null) ? other._userInfo is null : userInfo.Equals(other._userInfo))
                {
                    UriHostInfo host = _host;
                    return (host is null) ? other._host is null : host.Equals(other._host);
                }
            }
            return false;
        }

        public override bool Equals(object obj) => Equals(obj as UriAuthority);

        public override int GetHashCode() => HashCode.Combine(_scheme, _userInfo, _host);
    }
}

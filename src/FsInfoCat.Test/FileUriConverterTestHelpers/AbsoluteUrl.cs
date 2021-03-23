using System;
using System.Xml.Serialization;

namespace FsInfoCat.Test.FileUriConverterTestHelpers
{
    public abstract class AbsoluteUrl<TOwner> : RelativeUrl<TOwner>, IAbsoluteUrl where TOwner : ISynchronized
    {
        public bool IsFileScheme()
        {
            UriAuthority authority = _authority;
            if (authority is null)
                return false;
            UriScheme scheme = authority.Scheme;
            return !(scheme is null) && scheme.Name == "file";
        }
        [XmlAttribute]
        public bool IsWellFormed { get; set; }

        private UriAuthority _authority;
        [XmlElement(IsNullable = false, Order = 4)]
        public UriAuthority Authority
        {
            get => _authority;
            set
            {
                if (value is null)
                    throw new ArgumentNullException(nameof(value));
                lock (value.SyncRoot)
                {
                    lock (SyncRoot)
                    {
                        if (value.Owner is null)
                        {
                            if (_authority is null)
                                value.Owner = this;
                            else
                                lock (_authority.SyncRoot)
                                {
                                    if (ReferenceEquals(this, _authority.Owner))
                                        _authority.Owner = null;
                                    _authority = null;
                                    value.Owner = this;
                                }
                            _authority = value;
                        }
                        else if (!ReferenceEquals(value.Owner, this))
                            throw new InvalidOperationException();
                    }
                }
            }
        }

        protected AbsoluteUrl()
        {
            _authority = new UriAuthority
            {
                Owner = this
            };
        }

        protected bool Equals(AbsoluteUrl<TOwner> other) => IsWellFormed == other.IsWellFormed && _authority.Equals(other.Authority) && base.BaseEquals(other);
    }
}

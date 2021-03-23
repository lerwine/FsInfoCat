using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace FsInfoCat.Test.FileUriConverterTestHelpers
{
    public class UriUserInfo : IOwnable<UriAuthority>, IEquatable<UriUserInfo>
    {
        internal object SyncRoot => _syncRoot;
        private readonly object _syncRoot = new object();
        object ISynchronized.SyncRoot => _syncRoot;
        private UriAuthority _owner;
        internal UriAuthority Owner
        {
            get => _owner;
            set
            {
                lock (_syncRoot)
                    _owner = value;
            }
        }
        UriAuthority IOwnable<UriAuthority>.Owner => _owner;
        ISynchronized IOwnable.Owner => _owner;

        private string _userName = "";
        [XmlAttribute]
        public string UserName
        {
            get => _userName;
            set => _userName = value ?? "";
        }

        [XmlAttribute]
        public string Password { get; set; }

        public bool Equals([AllowNull] UriUserInfo other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            if (_userName.Equals(other._userName))
            {
                string p = Password;
                return (p is null) ? other.Password is null : p.Equals(other.Password);
            }
            return false;
        }

        public override bool Equals(object obj) => Equals(obj as UriUserInfo);

        public override int GetHashCode() => HashCode.Combine(_userName, Password);
    }
}

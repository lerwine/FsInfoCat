using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace FsInfoCat.Test.FileUriConverterTestHelpers
{
    public sealed class UriScheme : ISynchronized, IOwnable<UriAuthority>, IEquatable<UriScheme>
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
        UriAuthority IOwnable<UriAuthority>.Owner => Owner;
        ISynchronized IOwnable.Owner => Owner;

        private string _name = "file";
        [XmlAttribute]
        public string Name
        {
            get => _name;
            set => _name = value ?? "";
        }

        private string _delimiter = "://";
        [XmlAttribute]
        public string Delimiter
        {
            get => _delimiter;
            set => _delimiter = value ?? "";
        }

        public bool Equals([AllowNull] UriScheme other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as UriScheme);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_name, _delimiter);
        }

        public string GetXPath()
        {
            UriAuthority owner = Owner;
            return (owner is null) ? nameof(UriAuthority.Scheme) : $"{Owner.GetXPath()}/{nameof(UriAuthority.Scheme)}";
        }
    }
}

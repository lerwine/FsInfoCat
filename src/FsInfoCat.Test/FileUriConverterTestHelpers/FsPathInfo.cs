using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace FsInfoCat.Test.FileUriConverterTestHelpers
{
    public sealed class FsPathInfo : IOwnable<IRelativeUrl>, IEquatable<FsPathInfo>
    {
        internal object SyncRoot => _syncRoot;
        private readonly object _syncRoot = new object();
        object ISynchronized.SyncRoot => _syncRoot;
        private IRelativeUrl _owner;
        internal IRelativeUrl Owner
        {
            get => _owner;
            set
            {
                lock (_syncRoot)
                    _owner = value;
            }
        }
        IRelativeUrl IOwnable<IRelativeUrl>.Owner => _owner;
        ISynchronized IOwnable.Owner => _owner;

        [XmlAttribute]
        public bool IsAbsolute { get; set; }

        private string _path = "";
        [XmlText]
        public string Path
        {
            get => _path;
            set => _path = value ?? "";
        }

        public bool Equals([AllowNull] FsPathInfo other) => !(other is null) && (ReferenceEquals(this, other) || IsAbsolute == other.IsAbsolute && _path.Equals(other._path));

        public override bool Equals(object obj) => Equals(obj as FsPathInfo);

        public override int GetHashCode() => HashCode.Combine(IsAbsolute, Path);

        public string GetXPath()
        {
            IRelativeUrl owner = Owner;
            return (owner is null) ? nameof(IRelativeUrl.LocalPath) : $"{Owner.GetXPath()}/{nameof(IRelativeUrl.LocalPath)}";
        }
    }
}

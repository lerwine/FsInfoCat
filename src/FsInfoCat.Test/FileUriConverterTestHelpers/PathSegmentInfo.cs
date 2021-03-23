using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace FsInfoCat.Test.FileUriConverterTestHelpers
{
    public class PathSegmentInfo : IOwnable<IHasPathSegmentInfo>, IRegexMatch, IEquatable<PathSegmentInfo>
    {
        internal object SyncRoot => _syncRoot;
        private readonly object _syncRoot = new object();
        object ISynchronized.SyncRoot => _syncRoot;
        private IHasPathSegmentInfo _owner;
        internal IHasPathSegmentInfo Owner
        {
            get => _owner;
            set
            {
                lock (_syncRoot)
                    _owner = value;
            }
        }
        IHasPathSegmentInfo IOwnable<IHasPathSegmentInfo>.Owner => _owner;
        ISynchronized IOwnable.Owner => _owner;

        private string _match = "";
        [XmlAttribute]
        public string Match
        {
            get => _match;
            set => _match = value ?? "";
        }

        private string _directory = "";
        [XmlElement(IsNullable = false, Order = 0)]
        public string Directory
        {
            get => _directory;
            set => _directory = value ?? "";
        }

        [XmlElement(IsNullable = true, Order = 1)]
        public string FileName { get; set; }

        public bool Equals([AllowNull] PathSegmentInfo other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            if (_match.Equals(other._match) && _directory.Equals(other._directory))
            {
                string f = FileName;
                return (f is null) ? other.FileName is null : f.Equals(other.FileName);
            }
            return false;
        }

        public override bool Equals(object obj) => Equals(obj as PathSegmentInfo);

        public override int GetHashCode() => HashCode.Combine(_match, _directory, FileName);
    }
}

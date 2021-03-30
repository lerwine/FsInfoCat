using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace FsInfoCat.Test.FileUriConverterTestHelpers
{
    public sealed class UncHostInfo : HostInfo<IFsPathDetail>, IEquatable<UncHostInfo>
    {
        [XmlAttribute]
        public MappingHostType Type { get; set; }

        public bool Equals([AllowNull] UncHostInfo other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return Type == other.Type && Match == other.Match;
        }

        public override bool Equals(object obj) => Equals(obj as UncHostInfo);

        public override int GetHashCode() => HashCode.Combine(Match, Value, Type);

        public override string GetXPath()
        {
            IFsPathDetail owner = Owner;
            return (owner is null) ? nameof(IFsPathDetail.Host) : $"{Owner.GetXPath()}/{nameof(IFsPathDetail.Host)}";
        }
    }
}

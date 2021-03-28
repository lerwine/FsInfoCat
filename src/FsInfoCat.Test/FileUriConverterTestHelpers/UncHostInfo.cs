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
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as UncHostInfo);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Match, Value, Type);
        }

        public override string GetXPath()
        {
            IFsPathDetail owner = Owner;
            return (owner is null) ? nameof(IFsPathDetail.Host) : $"{Owner.GetXPath()}/{nameof(IFsPathDetail.Host)}";
        }
    }
}

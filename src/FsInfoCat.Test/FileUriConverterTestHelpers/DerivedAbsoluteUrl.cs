using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace FsInfoCat.Test.FileUriConverterTestHelpers
{
    public sealed class DerivedAbsoluteUrl : AbsoluteUrl<AbsoluteMatchedUrl>, IDerivedPathInfo, IEquatable<DerivedAbsoluteUrl>
    {
        private string _value = "";
        [XmlAttribute]
        public string Value
        {
            get => _value;
            set => _value = value ?? "";
        }

        public bool Equals([AllowNull] DerivedAbsoluteUrl other) => !(other is null) && (ReferenceEquals(this, other) || _value.Equals(other._value) && base.Equals(other));

        public override bool Equals(object obj)
        {
            return Equals(obj as DerivedAbsoluteUrl);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Path, Query, Fragment, LocalPath, IsWellFormed, Authority, _value);
        }

        public override string GetXPath()
        {
            AbsoluteMatchedUrl owner = Owner;
            return (owner is null) ? nameof(AbsoluteMatchedUrl.WellFormed) : $"{Owner.GetXPath()}/{nameof(AbsoluteMatchedUrl.WellFormed)}";
        }
    }
}

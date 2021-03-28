using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace FsInfoCat.Test.FileUriConverterTestHelpers
{
    public sealed class DerivedRelativeUrl : DerivedRelativeUrl<RelativeMatchedUrl>, IEquatable<DerivedRelativeUrl>
    {
        public bool Equals([AllowNull] DerivedRelativeUrl other) => !(other is null) && (ReferenceEquals(this, other) || BaseEquals(other));

        public override bool Equals(object obj) => Equals(obj as DerivedRelativeUrl);

        public override int GetHashCode() => HashCode.Combine(Path, Query, Fragment, LocalPath, Value);

        public override string GetXPath()
        {
            RelativeMatchedUrl owner = Owner;
            return (owner is null) ? nameof(RelativeMatchedUrl.WellFormed) : $"{Owner.GetXPath()}/{nameof(RelativeMatchedUrl.WellFormed)}";
        }
    }

    public abstract class DerivedRelativeUrl<TOwner> : RelativeUrl<TOwner>, IDerivedPathInfo
        where TOwner : ISynchronized
    {
        private string _value = "";
        [XmlAttribute]
        public string Value
        {
            get => _value;
            set => _value = value ?? "";
        }

        protected bool BaseEquals(DerivedRelativeUrl<TOwner> other) => _value.Equals(other._value) && base.BaseEquals(other);
    }
}

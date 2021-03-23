using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace FsInfoCat.Test.FileUriConverterTestHelpers
{
    public class TranslatedRelativeUrl : DerivedRelativeUrl<RelativeMatchedUrl>, IEquatable<TranslatedRelativeUrl>
    {
        [XmlAttribute]
        public bool IsWellFormed { get; set; }

        public bool Equals([AllowNull] TranslatedRelativeUrl other) => !(other is null) && (ReferenceEquals(this, other) || (IsWellFormed == other.IsWellFormed && BaseEquals(other)));

        public override bool Equals(object obj) => Equals(obj as TranslatedRelativeUrl);

        public override int GetHashCode() => HashCode.Combine(Path, Query, Fragment, LocalPath, Value, IsWellFormed);
    }
}

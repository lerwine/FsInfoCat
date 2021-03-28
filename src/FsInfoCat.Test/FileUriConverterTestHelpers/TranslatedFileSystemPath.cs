using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace FsInfoCat.Test.FileUriConverterTestHelpers
{
    public sealed class TranslatedFileSystemPath : FsPathDetail<MatchedFilesystemPath>, IDerivedPathInfo, IEquatable<TranslatedFileSystemPath>
    {
        private string _value = "";
        [XmlAttribute]
        public string Value
        {
            get => _value;
            set => _value = value ?? "";
        }

        public bool Equals([AllowNull] TranslatedFileSystemPath other) => !(other is null) && (ReferenceEquals(this, other) || !_value.Equals(other._value) && base.Equals(other));

        public override bool Equals(object obj)
        {
            return Equals(obj as TranslatedFileSystemPath);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IsAbsolute, Host, Path, _value);
        }

        public override string GetXPath()
        {
            MatchedFilesystemPath owner = Owner;
            return (owner is null) ? nameof(MatchedFilesystemPath.Translated) : $"{Owner.GetXPath()}/{nameof(MatchedFilesystemPath.Translated)}";
        }
    }
}

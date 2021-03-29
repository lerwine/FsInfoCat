using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace FsInfoCat.Test.FileUriConverterTestHelpers
{
    public sealed class RelativeMatchedUrl : RelativeUrl<PlatformPath>, IRegexMatch, IEquatable<RelativeMatchedUrl>
    {
        private string _match = "";
        [XmlAttribute]
        public string Match
        {
            get => _match;
            set => _match = value ?? "";
        }

        [XmlAttribute]
        public bool IsWellFormed { get; set; }

        private TranslatedRelativeUrl _translated;
        [XmlElement(IsNullable = true, Order = 4)]
        public TranslatedRelativeUrl Translated
        {
            get => _translated;
            set
            {
                if (value is null)
                {
                    lock (SyncRoot)
                    {
                        if (_translated is null)
                            return;
                        lock (_translated.SyncRoot)
                        {
                            if (ReferenceEquals(this, _translated.Owner))
                                _translated.Owner = null;
                            _translated = null;
                        }
                    }
                    return;
                }
                lock (value.SyncRoot)
                {
                    lock (SyncRoot)
                    {
                        if (value.Owner is null)
                        {
                            if (_translated is null)
                                value.Owner = this;
                            else
                                lock (_translated.SyncRoot)
                                {
                                    if (ReferenceEquals(this, _translated.Owner))
                                        _translated.Owner = null;
                                    _translated = null;
                                    value.Owner = this;
                                }
                            _translated = value;
                        }
                        else if (!ReferenceEquals(value.Owner, this))
                            throw new InvalidOperationException();
                    }
                }
            }
        }

        private DerivedRelativeUrl _wellFormed;
        [XmlElement(IsNullable = true, Order = 5)]
        public DerivedRelativeUrl WellFormed
        {
            get => _wellFormed;
            set
            {
                if (value is null)
                {
                    lock (SyncRoot)
                    {
                        if (_wellFormed is null)
                            return;
                        lock (_wellFormed.SyncRoot)
                        {
                            if (ReferenceEquals(this, _wellFormed.Owner))
                                _wellFormed.Owner = null;
                            _wellFormed = null;
                        }
                    }
                    return;
                }
                lock (value.SyncRoot)
                {
                    lock (SyncRoot)
                    {
                        if (value.Owner is null)
                        {
                            if (_wellFormed is null)
                                value.Owner = this;
                            else
                                lock (_wellFormed.SyncRoot)
                                {
                                    if (ReferenceEquals(this, _wellFormed.Owner))
                                        _wellFormed.Owner = null;
                                    _wellFormed = null;
                                    value.Owner = this;
                                }
                            _wellFormed = value;
                        }
                        else if (!ReferenceEquals(value.Owner, this))
                            throw new InvalidOperationException();
                    }
                }
            }
        }

        internal AbsoluteMatchedUrl GetAbsoluteUrl(bool wellformedOnly = false)
        {
            PlatformPath platformPath = Owner;
            if (platformPath is null)
                return null;
            AbsoluteMatchedUrl url = platformPath.AbsoluteUrl;
            return (url is null || url.IsWellFormed || !wellformedOnly) ? url : null;
        }

        internal AbsoluteMatchedUrl GetAltAbsoluteUrl(bool wellformedOnly = false)
        {
            PlatformPath platformPath = Owner;
            if (platformPath is null || (platformPath = platformPath.GetAltPath()) is null)
                return null;
            AbsoluteMatchedUrl altAbsolute = platformPath.AbsoluteUrl;
            return (altAbsolute is null || altAbsolute.IsWellFormed || !wellformedOnly) ? altAbsolute : null;
        }

        internal RelativeMatchedUrl GetAltRelativeUrl(bool wellformedOnly = false)
        {
            PlatformPath platformPath = Owner;
            if (platformPath is null || (platformPath = platformPath.GetAltPath()) is null)
                return null;
            RelativeMatchedUrl alt = platformPath.RelativeUrl;
            return (alt is null || alt.IsWellFormed || !wellformedOnly) ? alt : null;
        }

        internal RelativeUrl<PlatformPath> GetAltUrl(bool wellformedOnly = false)
        {
            PlatformPath platformPath = Owner;
            if (platformPath is null || (platformPath = platformPath.GetAltPath()) is null)
                return null;
            AbsoluteMatchedUrl altAbsolute = platformPath.AbsoluteUrl;
            RelativeMatchedUrl altRelative;
            if (altAbsolute is null)
            {
                if (wellformedOnly)
                    return ((altRelative = platformPath.RelativeUrl) is null || !altRelative.IsWellFormed) ? null : altRelative;
                return platformPath.RelativeUrl;
            }
            if (altAbsolute.IsWellFormed)
                return altAbsolute;
            altRelative = platformPath.RelativeUrl;
            if (wellformedOnly)
                return (altRelative is null || altRelative.IsWellFormed) ? altRelative : null;
            return (altRelative is null || !altRelative.IsWellFormed) ? (RelativeUrl<PlatformPath>)altAbsolute : altRelative;
        }

        internal FsPathInfo GetAltLocalPath()
        {
            RelativeUrl<PlatformPath> altUrl = GetAltUrl();
            return altUrl?.LocalPath;
        }

        public bool Equals([AllowNull] RelativeMatchedUrl other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            if (IsWellFormed == other.IsWellFormed && _match.Equals(other._match))
            {
                TranslatedRelativeUrl translated = _translated;
                if ((translated is null) ? other.Translated is null : translated.Equals(other._translated))
                {
                    DerivedRelativeUrl wellFormed = _wellFormed;
                    return ((wellFormed is null) ? other._wellFormed is null : _wellFormed.Equals(other._wellFormed)) && base.BaseEquals(other);
                }
            }
            return false;
        }

        public override bool Equals(object obj) => Equals(obj as RelativeMatchedUrl);

        public override int GetHashCode() => HashCode.Combine(Path, Query, Fragment, LocalPath, _match, _translated, _wellFormed);

        public override string GetXPath()
        {
            PlatformPath owner = Owner;
            return (owner is null) ? nameof(PlatformPath.RelativeUrl) : $"{Owner.GetXPath()}/{nameof(PlatformPath.RelativeUrl)}";
        }
    }
}

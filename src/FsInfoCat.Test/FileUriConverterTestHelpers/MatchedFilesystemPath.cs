using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace FsInfoCat.Test.FileUriConverterTestHelpers
{
    public class MatchedFilesystemPath : FsPathDetail<PlatformPath>, IRegexMatch, IEquatable<MatchedFilesystemPath>
    {
        private string _match = "";
        private Uri _uri = new Uri("", UriKind.Relative);

        [XmlAttribute]
        public string Match
        {
            get => _match;
            set => _match = value ?? "";
        }


        [XmlElement(nameof(URI), IsNullable = false, Order = 2)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string URIString
        {
            get
            {
                Uri uri = URI;
                return (uri is null) ? null : ((uri.IsAbsoluteUri) ? uri.AbsoluteUri : uri.OriginalString);
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    _uri = new Uri("", UriKind.Relative);
                else if (Uri.TryCreate(value, UriKind.Absolute, out Uri uri) || Uri.TryCreate(value, UriKind.Relative, out uri))
                    _uri = uri;
                else
                {
                    string e = Uri.EscapeUriString(value);
                    if (Uri.TryCreate(e, UriKind.Absolute, out uri) || Uri.TryCreate(e, UriKind.Relative, out uri))
                        _uri = uri;
                    else
                        _uri = new Uri(Uri.EscapeDataString(e), UriKind.Relative);
                }   
            }
        }

        [XmlIgnore]
        public Uri URI
        {
            get => _uri;
            set => _uri = value ?? throw new ArgumentNullException(nameof(value));
        }

        private TranslatedFileSystemPath _translated;
        [XmlElement(IsNullable = true, Order = 3)]
        public TranslatedFileSystemPath Translated
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

        public bool Equals([AllowNull] MatchedFilesystemPath other) => !(other is null) && (ReferenceEquals(this, other) || _match.Equals(other._match) && _uri.Equals(other._uri) && base.Equals(other));

        public override bool Equals(object obj) => Equals(obj as MatchedFilesystemPath);

        public override int GetHashCode() => HashCode.Combine(IsAbsolute, Host, Path, _match, _uri, _translated);
    }
}

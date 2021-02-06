using System;
using System.Globalization;
using FsInfoCat.Util;

namespace FsInfoCat.Models.Volumes
{
    [Serializable]
    public struct VolumeIdentifier : IEquatable<VolumeIdentifier>
    {
        public static readonly VolumeIdentifier Empty = new VolumeIdentifier("");

        public enum ValidationCode
        {
            EmptyReferenceUrl,
            ReferenceUrlNotAbsolute,
            InvalidUUID,
            InvalidVolumeID,
            InvalidUrnNamespace,
            InvalidUriScheme,
            ValidationSucceeded
        }

        private Guid? _uuid;
        private uint? _serialNumber;
        private byte? _ordinal;
        private Uri _location;
        private string _query;
        private string _fragment;
        private ValidationCode _validation;

        public Guid? UUID => _uuid;

        public uint? SerialNumber => _serialNumber;

        public byte? Ordinal => _ordinal;

        public Uri Location => _location ?? Empty._location;

        public VolumeIdentifier(string url) : this()
        {
            if (string.IsNullOrEmpty(url) || !Uri.TryCreate(url, UriKind.Absolute, out Uri uri))
            {
                if (string.IsNullOrEmpty(url))
                {
                    _validation = ValidationCode.EmptyReferenceUrl;
                    _location = new Uri("", UriKind.Relative);
                }
                else
                {
                    _validation = ValidationCode.ReferenceUrlNotAbsolute;
                    _location = (Uri.TryCreate(url, UriKind.Relative, out uri)) ? uri : new Uri(Uri.EscapeUriString(url));
                }
                _uuid = null;
                _serialNumber = null;
                _ordinal = null;
                _query = null;
                return;
            }
            if (uri.Host.ToLower() != uri.Host || uri.Scheme.ToLower() != uri.Scheme)
            {
                UriBuilder uriBuilder = new UriBuilder(uri);
                uriBuilder.Scheme = uri.Scheme.ToLower();
                uriBuilder.Host = uri.Host.ToLower();
                uri = uriBuilder.Uri;
            }
            _fragment = uri.GetFragmentComponent();
            if (null != _fragment)
                uri.TrySetFragmentComponent(null, out uri);
            _query = uri.GetQueryComponent();
            if (null != _query)
                uri.TrySetQueryComponent(null, out uri);

            if (uri.Scheme.Equals(UrlHelper.URI_SCHEME_URN))
            {
                string[] segments = uri.AbsolutePath.Split(':', 3);
                switch (segments[0].ToLower())
                {
                    case UrlHelper.URN_NAMESPACE_UUID:
                        if (segments.Length == 2 && Guid.TryParse(segments[1], out Guid uuid))
                        {
                            _uuid = uuid;
                            _location = new Uri($"{UrlHelper.URI_SCHEME_URN}:{UrlHelper.URN_NAMESPACE_UUID}:{uuid.ToString("d").ToLower()}");
                            _serialNumber = null;
                            _ordinal = null;
                            _validation = ValidationCode.ValidationSucceeded;
                            return;
                        }
                        _validation = ValidationCode.InvalidUUID;
                        break;
                    case UrlHelper.URN_NAMESPACE_VOLUME:
                        if (segments.Length > 2 && segments[1] == UrlHelper.URN_NAMESPACE_ID &&
                                uint.TryParse(segments[2], NumberStyles.HexNumber, null, out uint vsn))
                        {
                            if (segments.Length == 3)
                            {
                                _serialNumber = vsn;
                                _location = new Uri($"{UrlHelper.URI_SCHEME_URN}:{UrlHelper.URN_NAMESPACE_VOLUME}:{UrlHelper.URN_NAMESPACE_ID}:{(vsn << 16).ToString("x4").ToLower()}-{(vsn & 0xffff).ToString("x4").ToLower()}");
                                _ordinal = null;
                                _uuid = null;
                                _validation = ValidationCode.ValidationSucceeded;
                            }
                            else if (segments.Length == 4 && uint.TryParse(segments[3], NumberStyles.HexNumber, null, out uint ord))
                            {
                                if (segments[3].Length > 2)
                                {
                                    if (vsn < 0x10000 && ord < 0x10000)
                                    {
                                        _serialNumber = ord | (vsn << 16);
                                        _ordinal = null;
                                        _location = new Uri($"{UrlHelper.URI_SCHEME_URN}:{UrlHelper.URN_NAMESPACE_VOLUME}:{UrlHelper.URN_NAMESPACE_ID}:{vsn.ToString("x4").ToLower()}-{ord.ToString("x4").ToLower()}");
                                        _uuid = null;
                                        _validation = ValidationCode.ValidationSucceeded;
                                        return;
                                    }
                                    if (ord < 0x100)
                                    {
                                        _serialNumber = vsn;
                                        _ordinal = (byte)ord;
                                        _location = new Uri($"{UrlHelper.URI_SCHEME_URN}:{UrlHelper.URN_NAMESPACE_VOLUME}:{UrlHelper.URN_NAMESPACE_ID}:{vsn.ToString("x8").ToLower()}-{ord.ToString("x2").ToLower()}");
                                        _uuid = null;
                                        _validation = ValidationCode.ValidationSucceeded;
                                        return;
                                    }
                                }
                                else
                                {
                                    if (ord < 0x100 && vsn < 0x1000000)
                                    {
                                        _serialNumber = vsn;
                                        _ordinal = (byte)ord;
                                        _location = new Uri($"{UrlHelper.URI_SCHEME_URN}:{UrlHelper.URN_NAMESPACE_VOLUME}:{UrlHelper.URN_NAMESPACE_ID}:{vsn.ToString("x8").ToLower()}-{ord.ToString("x2").ToLower()}");
                                        _uuid = null;
                                        _location = null;
                                        _validation = ValidationCode.ValidationSucceeded;
                                        return;
                                    }
                                    if (vsn < 0x10000)
                                    {
                                        _serialNumber = ord | (vsn << 16);
                                        _ordinal = null;
                                        _location = new Uri($"{UrlHelper.URI_SCHEME_URN}:{UrlHelper.URN_NAMESPACE_VOLUME}:{UrlHelper.URN_NAMESPACE_ID}:{vsn.ToString("x4").ToLower()}-{ord.ToString("x4").ToLower()}");
                                        _uuid = null;
                                        _validation = ValidationCode.ValidationSucceeded;
                                        return;
                                    }
                                }
                            }
                        }
                        _validation = ValidationCode.InvalidVolumeID;
                        break;
                    default:
                        _validation = ValidationCode.InvalidUrnNamespace;
                        break;
                }
            }
            else if (uri.Scheme.Equals(Uri.UriSchemeFile))
            {
                if (uri.IsDefaultPort)
                {
                    if (uri.OriginalString != uri.AbsoluteUri)
                        uri = new Uri(uri.AbsoluteUri);
                    if (uri.Host.ToLower() != uri.Host)
                        uri.TrySetHostComponent(uri.Host.ToLower(), null, out uri);
                    if (!uri.AbsolutePath.EndsWith('/'))
                        uri.TrySetPathComponent(uri.AbsolutePath + "/", out uri);
                    _location = uri;
                    _serialNumber = null;
                    _ordinal = null;
                    _uuid = null;
                    _validation = ValidationCode.ValidationSucceeded;
                }
            }
            else
                _validation = ValidationCode.InvalidUriScheme;

            _location = uri;
            _ordinal = null;
            _serialNumber = null;
            _uuid = null;
        }

        public bool Equals(VolumeIdentifier other)
        {
            if (_validation == ValidationCode.ValidationSucceeded)
            {
                if (other._validation != ValidationCode.ValidationSucceeded)
                    return false;
                if (_serialNumber.HasValue)
                {
                    if (!other._serialNumber.HasValue || _serialNumber.Value != other._serialNumber.Value)
                        return false;
                    if (_ordinal.HasValue)
                    {
                        if (!other._ordinal.HasValue || other._ordinal.Value != _ordinal.Value)
                            return false;
                    }
                    else if (other._ordinal.HasValue)
                        return false;
                }
                else
                {
                    if (_uuid.HasValue)
                        return other._uuid.HasValue && _uuid.Value.Equals(other._uuid.Value);
                    if (other._serialNumber.HasValue || other._uuid.HasValue)
                        return false;
                }
            }
            else if (other._validation == ValidationCode.ValidationSucceeded)
                return false;
            return _location.AbsoluteUri.Equals(other._location.IsAbsoluteUri);
        }
    }
}

using System;
using System.Globalization;
using System.Text.RegularExpressions;
using FsInfoCat.Util;

namespace FsInfoCat.Models.Volumes
{
    [Serializable]
    public struct VolumeIdentifier : IEquatable<VolumeIdentifier>
    {
        public const string GROUP_NAME_UUID_NS = "u";
        public const string GROUP_NAME_VOLUME_NS = "n";
        public const string GROUP_NAME_ID_NS = "i";
        public const string GROUP_NAME_VALUE = "v";
        public static Regex NsPathRegex = new Regex($@"^((?<{GROUP_NAME_UUID_NS}>uuid(:|$))|(?<{GROUP_NAME_VOLUME_NS}>volume(:(?<{GROUP_NAME_ID_NS}>id(:|$))?|$))?)(?<{GROUP_NAME_VALUE}>.+)?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex SerialNumberPathRegex = new Regex(@"^(?:(?=[a-f\d]{1,4}-[a-f\d])([a-f\d]{1,4})-([a-f\d]{1,4})|([a-f\d]{5,8})(?:-([a-f\d]{1,2}))?)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

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

        public ValidationCode GetValidationCode() => _validation;

        public bool IsValid() => _validation == ValidationCode.ValidationSucceeded;

        public VolumeIdentifier(uint serialNumber)
        {
            _serialNumber = serialNumber;
            _location = new Uri($"{UrlHelper.URI_SCHEME_URN}:{UrlHelper.URN_NAMESPACE_VOLUME}:{UrlHelper.URN_NAMESPACE_ID}:{(serialNumber >> 16).ToString("x4").ToLower()}-{(serialNumber & 0xffff).ToString("x4").ToLower()}", UriKind.Absolute);
            _ordinal = null;
            _uuid = null;
            _validation = ValidationCode.ValidationSucceeded;
            _query = _fragment = null;
        }

        public VolumeIdentifier(uint serialNumber, byte ordinal)
        {
            _serialNumber = serialNumber;
            _ordinal = ordinal;
            _location = new Uri($"{UrlHelper.URI_SCHEME_URN}:{UrlHelper.URN_NAMESPACE_VOLUME}:{UrlHelper.URN_NAMESPACE_ID}:{serialNumber.ToString("x8").ToLower()}-{ordinal.ToString("x2").ToLower()}", UriKind.Absolute);
            _uuid = null;
            _validation = ValidationCode.ValidationSucceeded;
            _query = _fragment = null;
        }

        public VolumeIdentifier(Guid uuid)
        {
            _uuid = uuid;
            _location = new Uri($"{UrlHelper.URI_SCHEME_URN}:{UrlHelper.URN_NAMESPACE_UUID}:{uuid.ToString("d").ToLower()}", UriKind.Absolute);
            _serialNumber = null;
            _ordinal = null;
            _validation = ValidationCode.ValidationSucceeded;
            _query = _fragment = null;
        }

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
                    _location = (Uri.TryCreate(url, UriKind.Relative, out uri)) ? uri : new Uri(Uri.EscapeUriString(url), UriKind.Relative);
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
                string[] segments = Uri.UnescapeDataString(uri.AbsolutePath).Split(':', 3);
                switch (segments[0].ToLower())
                {
                    case UrlHelper.URN_NAMESPACE_UUID:
                        if (segments.Length == 2 && Guid.TryParse(segments[1], out Guid uuid))
                        {
                            _uuid = uuid;
                            _location = new Uri($"{UrlHelper.URI_SCHEME_URN}:{UrlHelper.URN_NAMESPACE_UUID}:{uuid.ToString("d").ToLower()}", UriKind.Absolute);
                            _serialNumber = null;
                            _ordinal = null;
                            _validation = ValidationCode.ValidationSucceeded;
                            return;
                        }
                        _validation = ValidationCode.InvalidUUID;
                        break;
                    case UrlHelper.URN_NAMESPACE_VOLUME:
                        if (segments.Length == 3 && string.Equals(segments[1], UrlHelper.URN_NAMESPACE_ID, StringComparison.InvariantCultureIgnoreCase))
                        {
                            segments = segments[2].Split('-');
                            if (uint.TryParse(segments[0], NumberStyles.HexNumber, null, out uint vsn))
                            {
                                switch (segments.Length)
                                {
                                    case 1:
                                        _serialNumber = vsn;
                                        _location = new Uri($"{UrlHelper.URI_SCHEME_URN}:{UrlHelper.URN_NAMESPACE_VOLUME}:{UrlHelper.URN_NAMESPACE_ID}:{(vsn << 16).ToString("x4").ToLower()}-{(vsn & 0xffff).ToString("x4").ToLower()}", UriKind.Absolute);
                                        _ordinal = null;
                                        _uuid = null;
                                        _validation = ValidationCode.ValidationSucceeded;
                                        return;
                                    case 2:
                                        if (uint.TryParse(segments[1], NumberStyles.HexNumber, null, out uint ord))
                                        {
                                            if (segments[0].Length < 5 && segments[1].Length > 2)
                                            {
                                                if (vsn < 0x10000 && ord < 0x10000)
                                                {
                                                    _serialNumber = ord | (vsn << 16);
                                                    _ordinal = null;
                                                    _location = new Uri($"{UrlHelper.URI_SCHEME_URN}:{UrlHelper.URN_NAMESPACE_VOLUME}:{UrlHelper.URN_NAMESPACE_ID}:{vsn.ToString("x4").ToLower()}-{ord.ToString("x4").ToLower()}", UriKind.Absolute);
                                                    _uuid = null;
                                                    _validation = ValidationCode.ValidationSucceeded;
                                                    return;
                                                }
                                                if (ord < 0x100)
                                                {
                                                    _serialNumber = vsn;
                                                    _ordinal = (byte)ord;
                                                    _location = new Uri($"{UrlHelper.URI_SCHEME_URN}:{UrlHelper.URN_NAMESPACE_VOLUME}:{UrlHelper.URN_NAMESPACE_ID}:{vsn.ToString("x8").ToLower()}-{ord.ToString("x2").ToLower()}", UriKind.Absolute);
                                                    _uuid = null;
                                                    _validation = ValidationCode.ValidationSucceeded;
                                                    return;
                                                }
                                            }
                                            else
                                            {
                                                if (ord < 0x100)
                                                {
                                                    _serialNumber = vsn;
                                                    _ordinal = (byte)ord;
                                                    _location = new Uri($"{UrlHelper.URI_SCHEME_URN}:{UrlHelper.URN_NAMESPACE_VOLUME}:{UrlHelper.URN_NAMESPACE_ID}:{vsn.ToString("x8").ToLower()}-{ord.ToString("x2").ToLower()}", UriKind.Absolute);
                                                    _uuid = null;
                                                    _validation = ValidationCode.ValidationSucceeded;
                                                    return;
                                                }
                                                if (vsn < 0x10000 && ord < 0x10000)
                                                {
                                                    _serialNumber = ord | (vsn << 16);
                                                    _ordinal = null;
                                                    _location = new Uri($"{UrlHelper.URI_SCHEME_URN}:{UrlHelper.URN_NAMESPACE_VOLUME}:{UrlHelper.URN_NAMESPACE_ID}:{vsn.ToString("x4").ToLower()}-{ord.ToString("x4").ToLower()}", UriKind.Absolute);
                                                    _uuid = null;
                                                    _validation = ValidationCode.ValidationSucceeded;
                                                    return;
                                                }
                                            }
                                        }
                                        break;
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
                        uri = new Uri(uri.AbsoluteUri, UriKind.Absolute);
                    if (uri.Host.ToLower() != uri.Host)
                        uri.TrySetHostComponent(uri.Host.ToLower(), null, out uri);
                    if (!uri.AbsolutePath.EndsWith('/'))
                        uri.TrySetPathComponent(uri.AbsolutePath + "/", out uri);
                    _location = uri;
                    _serialNumber = null;
                    _ordinal = null;
                    _uuid = null;
                    _validation = ValidationCode.ValidationSucceeded;
                    return;
                }
                _validation = ValidationCode.InvalidUriScheme;
            }
            else
                _validation = ValidationCode.InvalidUriScheme;

            _location = uri;
            _ordinal = null;
            _serialNumber = null;
            _uuid = null;
        }

        public static bool TryCreate(object obj, out VolumeIdentifier volumeIdentifer)
        {
            if (null == obj)
            {
                volumeIdentifer = VolumeIdentifier.Empty;
                return false;
            }
            if (obj is VolumeIdentifier)
                volumeIdentifer = (VolumeIdentifier)obj;
            else if (obj is string s)
            {
                if (string.IsNullOrWhiteSpace(s) || !(volumeIdentifer = new VolumeIdentifier(s)).IsValid())
                {
                    volumeIdentifer = VolumeIdentifier.Empty;
                    return false;
                }
            }
            else if (obj is uint)
                volumeIdentifer = new VolumeIdentifier((uint)obj);
            else if (obj is Guid)
                volumeIdentifer = new VolumeIdentifier((Guid)obj);
            else if (obj is int i)
            {
                if (i < 0)
                {
                    volumeIdentifer = VolumeIdentifier.Empty;
                    return false;
                }
                volumeIdentifer = new VolumeIdentifier((uint)i);
            }
            else if (obj is double d)
            {
                if (d < 0.0 || Math.Floor(d) != d)
                {
                    volumeIdentifer = VolumeIdentifier.Empty;
                    return false;
                }
                volumeIdentifer = new VolumeIdentifier((uint)d);
            }
            else if (obj is long l)
            {
                if (l < 0 || l > (long)(uint.MaxValue))
                {
                    volumeIdentifer = VolumeIdentifier.Empty;
                    return false;
                }
                volumeIdentifer = new VolumeIdentifier((uint)l);
            }
            else if (obj is ulong ul)
            {
                if (ul > (ulong)(uint.MaxValue))
                {
                    volumeIdentifer = VolumeIdentifier.Empty;
                    return false;
                }
                volumeIdentifer = new VolumeIdentifier((uint)ul);
            }
            else if (obj is float f)
            {
                if (f < 0.0f || Math.Floor(f) != f)
                {
                    volumeIdentifer = VolumeIdentifier.Empty;
                    return false;
                }
                volumeIdentifer = new VolumeIdentifier((uint)f);
            }
            else if (obj is byte b)
                volumeIdentifer = new VolumeIdentifier((uint)b);
            else if (obj is sbyte sb)
            {
                if (sb < 0)
                {
                    volumeIdentifer = VolumeIdentifier.Empty;
                    return false;
                }
                volumeIdentifer = new VolumeIdentifier((uint)sb);
            }
            else if (obj is short ss)
            {
                if (ss < 0)
                {
                    volumeIdentifer = VolumeIdentifier.Empty;
                    return false;
                }
                volumeIdentifer = new VolumeIdentifier((uint)ss);
            }
            else if (obj is ushort us)
                volumeIdentifer = new VolumeIdentifier((uint)us);
            else if (!(obj is Uri uri && uri.IsAbsoluteUri && (uri.Scheme == Uri.UriSchemeFile || uri.Scheme == UrlHelper.URI_SCHEME_URN) &&
                (volumeIdentifer = new VolumeIdentifier(uri.AbsoluteUri)).IsValid()))
            {
                volumeIdentifer = VolumeIdentifier.Empty;
                return false;
            }
            return true;
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
            else
            {
                if (other._validation == ValidationCode.ValidationSucceeded)
                    return false;
                if (_location is null)
                    return other._location is null || other._location.OriginalString.Length == 0;
                if (other._location is null)
                    return _location.OriginalString.Length == 0; ;
            }
            return ((_location.IsAbsoluteUri) ? _location.AbsolutePath : _location.OriginalString).Equals(((other._location.IsAbsoluteUri) ? other._location.AbsolutePath : other._location.OriginalString));
        }

        public static string ToIdentifierString(Guid guid) => guid.ToString("d").ToLower();

        public static string ToIdentifierString(uint serialNumber) => $"{(serialNumber >> 16).ToString("x4")}-{(serialNumber & 0xffff).ToString("x4")}";

        public static string ToIdentifierString(uint serialNumber, byte ordinal) => $"{serialNumber.ToString("x8")}-{ordinal.ToString("x2")}";

        public static string ToUrn(Guid guid) => $"urn:uuid:{ToIdentifierString(guid)}";

        public static string ToUrn(uint serialNumber, byte ordinal) => $"urn:volume:id:{ToIdentifierString(serialNumber, ordinal)}";

        public static string ToUrn(uint serialNumber) => $"urn:volume:id:{ToIdentifierString(serialNumber)}";
    }
}

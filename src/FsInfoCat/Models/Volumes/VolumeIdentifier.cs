using FsInfoCat.Util;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace FsInfoCat.Models.Volumes
{
    [Serializable]
    public struct VolumeIdentifier : IEquatable<VolumeIdentifier>
    {
        public const string GROUP_NAME_UUID_NS = "u";
        public const string GROUP_NAME_VOLUME_NS = "n";
        public const string GROUP_NAME_ID_NS = "i";
        public const string GROUP_NAME_VALUE = "v";

        /// <summary>
        /// Matches consecutive and trailing URI path separators, allowing up to 3 consecutive path separator characters following the scheme separator.
        /// </summary>
        /// <remarks>This will also match surrounding whitespace and relative self-reference sequences (<c>/./<c>).. This does not match parent segment
        /// references (<c>/../</c>) unless they are at the beginning of the string.</remarks>
        public static readonly Regex PathSeparatorNormalize = new Regex(@"^\s*(\.\.?/+|\s+)+|(?<!^\s*file:/?)/(?=/)|/\.(?=/|$)", RegexOptions.Compiled);
        public static readonly Regex NsPathRegex = new Regex($@"^((?<{GROUP_NAME_UUID_NS}>uuid(:|$))|(?<{GROUP_NAME_VOLUME_NS}>volume(:(?<{GROUP_NAME_ID_NS}>id(:|$))?|$))?)(?<{GROUP_NAME_VALUE}>.+)?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static readonly Regex SerialNumberPathRegex = new Regex(@"^(?:(?=[a-f\d]{1,4}-[a-f\d])([a-f\d]{1,4})-([a-f\d]{1,4})|([a-f\d]{5,8})(?:-([a-f\d]{1,2}))?)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

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

        private readonly Guid? _uuid;
        private readonly uint? _serialNumber;
        private readonly byte? _ordinal;
        private readonly Uri _location;
        private readonly string _query;
        private readonly string _fragment;
        private readonly ValidationCode _validation;

        public Guid? UUID => _uuid;

        public uint? SerialNumber => _serialNumber;

        public byte? Ordinal => _ordinal;

        public Uri Location => _location ?? Empty._location;

        public ValidationCode GetValidationCode() => _validation;

        public bool IsValid() => _validation == ValidationCode.ValidationSucceeded;

        public VolumeIdentifier(uint serialNumber)
        {
            _serialNumber = serialNumber;
            _location = new Uri($"{UriHelper.URI_SCHEME_URN}:{UriHelper.URN_NAMESPACE_VOLUME}:{UriHelper.URN_NAMESPACE_ID}:{(serialNumber >> 16).ToString("x4").ToLower()}-{(serialNumber & 0xffff).ToString("x4").ToLower()}", UriKind.Absolute);
            _ordinal = null;
            _uuid = null;
            _validation = ValidationCode.ValidationSucceeded;
            _query = _fragment = null;
        }

        public VolumeIdentifier(uint serialNumber, byte ordinal)
        {
            _serialNumber = serialNumber;
            _ordinal = ordinal;
            _location = new Uri($"{UriHelper.URI_SCHEME_URN}:{UriHelper.URN_NAMESPACE_VOLUME}:{UriHelper.URN_NAMESPACE_ID}:{serialNumber.ToString("x8").ToLower()}-{ordinal.ToString("x2").ToLower()}", UriKind.Absolute);
            _uuid = null;
            _validation = ValidationCode.ValidationSucceeded;
            _query = _fragment = null;
        }

        public VolumeIdentifier(Guid uuid)
        {
            _uuid = uuid;
            _location = new Uri($"{UriHelper.URI_SCHEME_URN}:{UriHelper.URN_NAMESPACE_UUID}:{uuid.ToString("d").ToLower()}", UriKind.Absolute);
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
                UriBuilder uriBuilder = new UriBuilder(uri)
                {
                    Scheme = uri.Scheme.ToLower(),
                    Host = uri.Host.ToLower()
                };
                uri = uriBuilder.Uri;
            }
            _fragment = uri.GetFragmentComponent();
            if (null != _fragment)
                uri.TrySetFragmentComponent(null, out uri);
            _query = uri.GetQueryComponent();
            if (null != _query)
                uri.TrySetQueryComponent(null, out uri);

            if (uri.Scheme.Equals(UriHelper.URI_SCHEME_URN))
            {
                string[] segments = Uri.UnescapeDataString(uri.AbsolutePath).Split(UriHelper.URI_SCHEME_SEPARATOR_CHAR, 3);
                switch (segments[0].ToLower())
                {
                    case UriHelper.URN_NAMESPACE_UUID:
                        if (segments.Length == 2 && Guid.TryParse(segments[1], out Guid uuid))
                        {
                            _uuid = uuid;
                            _location = new Uri($"{UriHelper.URI_SCHEME_URN}:{UriHelper.URN_NAMESPACE_UUID}:{uuid.ToString("d").ToLower()}", UriKind.Absolute);
                            _serialNumber = null;
                            _ordinal = null;
                            _validation = ValidationCode.ValidationSucceeded;
                            return;
                        }
                        _validation = ValidationCode.InvalidUUID;
                        break;
                    case UriHelper.URN_NAMESPACE_VOLUME:
                        if (segments.Length == 3 && string.Equals(segments[1], UriHelper.URN_NAMESPACE_ID, StringComparison.InvariantCultureIgnoreCase))
                        {
                            segments = segments[2].Split('-');
                            if (uint.TryParse(segments[0], NumberStyles.HexNumber, null, out uint vsn))
                            {
                                switch (segments.Length)
                                {
                                    case 1:
                                        _serialNumber = vsn;
                                        _location = new Uri($"{UriHelper.URI_SCHEME_URN}:{UriHelper.URN_NAMESPACE_VOLUME}:{UriHelper.URN_NAMESPACE_ID}:{(vsn << 16).ToString("x4").ToLower()}-{(vsn & 0xffff).ToString("x4").ToLower()}", UriKind.Absolute);
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
                                                    _location = new Uri($"{UriHelper.URI_SCHEME_URN}:{UriHelper.URN_NAMESPACE_VOLUME}:{UriHelper.URN_NAMESPACE_ID}:{vsn.ToString("x4").ToLower()}-{ord.ToString("x4").ToLower()}", UriKind.Absolute);
                                                    _uuid = null;
                                                    _validation = ValidationCode.ValidationSucceeded;
                                                    return;
                                                }
                                                if (ord < 0x100)
                                                {
                                                    _serialNumber = vsn;
                                                    _ordinal = (byte)ord;
                                                    _location = new Uri($"{UriHelper.URI_SCHEME_URN}:{UriHelper.URN_NAMESPACE_VOLUME}:{UriHelper.URN_NAMESPACE_ID}:{vsn.ToString("x8").ToLower()}-{ord.ToString("x2").ToLower()}", UriKind.Absolute);
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
                                                    _location = new Uri($"{UriHelper.URI_SCHEME_URN}:{UriHelper.URN_NAMESPACE_VOLUME}:{UriHelper.URN_NAMESPACE_ID}:{vsn.ToString("x8").ToLower()}-{ord.ToString("x2").ToLower()}", UriKind.Absolute);
                                                    _uuid = null;
                                                    _validation = ValidationCode.ValidationSucceeded;
                                                    return;
                                                }
                                                if (vsn < 0x10000 && ord < 0x10000)
                                                {
                                                    _serialNumber = ord | (vsn << 16);
                                                    _ordinal = null;
                                                    _location = new Uri($"{UriHelper.URI_SCHEME_URN}:{UriHelper.URN_NAMESPACE_VOLUME}:{UriHelper.URN_NAMESPACE_ID}:{vsn.ToString("x4").ToLower()}-{ord.ToString("x4").ToLower()}", UriKind.Absolute);
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
                    string path = PathSeparatorNormalize.Replace(uri.AbsolutePath, "");
                    if (path != uri.AbsolutePath)
                        uri.TrySetPathComponent(path.EndsWith(UriHelper.URI_PATH_SEPARATOR_CHAR) ? path : $"{path}/", out uri);
                    else if (!path.EndsWith(UriHelper.URI_PATH_SEPARATOR_CHAR))
                        uri.TrySetPathComponent($"{path}/", out uri);
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
                volumeIdentifer = Empty;
                return false;
            }
            if (obj is VolumeIdentifier identifier)
                volumeIdentifer = identifier;
            else if (obj is string s)
            {
                if (string.IsNullOrWhiteSpace(s) || !(volumeIdentifer = new VolumeIdentifier(s)).IsValid())
                {
                    volumeIdentifer = Empty;
                    return false;
                }
            }
            else if (obj is uint u)
                volumeIdentifer = new VolumeIdentifier(u);
            else if (obj is Guid g)
                volumeIdentifer = new VolumeIdentifier(g);
            else if (obj is int i)
            {
                if (i < 0)
                {
                    volumeIdentifer = Empty;
                    return false;
                }
                volumeIdentifer = new VolumeIdentifier((uint)i);
            }
            else if (obj is double d)
            {
                if (d < 0.0 || Math.Floor(d) != d)
                {
                    volumeIdentifer = Empty;
                    return false;
                }
                volumeIdentifer = new VolumeIdentifier((uint)d);
            }
            else if (obj is long l)
            {
                if (l < 0 || l > uint.MaxValue)
                {
                    volumeIdentifer = Empty;
                    return false;
                }
                volumeIdentifer = new VolumeIdentifier((uint)l);
            }
            else if (obj is ulong ul)
            {
                if (ul > (ulong)(uint.MaxValue))
                {
                    volumeIdentifer = Empty;
                    return false;
                }
                volumeIdentifer = new VolumeIdentifier((uint)ul);
            }
            else if (obj is float f)
            {
                if (f < 0.0f || Math.Floor(f) != f)
                {
                    volumeIdentifer = Empty;
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
                    volumeIdentifer = Empty;
                    return false;
                }
                volumeIdentifer = new VolumeIdentifier((uint)sb);
            }
            else if (obj is short ss)
            {
                if (ss < 0)
                {
                    volumeIdentifer = Empty;
                    return false;
                }
                volumeIdentifer = new VolumeIdentifier((uint)ss);
            }
            else if (obj is ushort us)
                volumeIdentifer = new VolumeIdentifier((uint)us);
            else if (!(obj is Uri uri && uri.IsAbsoluteUri && (uri.Scheme == Uri.UriSchemeFile || uri.Scheme == UriHelper.URI_SCHEME_URN) &&
                (volumeIdentifer = new VolumeIdentifier(uri.AbsoluteUri)).IsValid()))
            {
                volumeIdentifer = Empty;
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

        public override bool Equals(object obj)
        {
            VolumeIdentifier? v = obj as VolumeIdentifier?;
            return v.HasValue && Equals(v.Value);
        }

        public override int GetHashCode()
        {
            if (_uuid.HasValue)
                return _uuid.Value.GetHashCode();
            if (_serialNumber.HasValue)
                return _serialNumber.Value.GetHashCode();
            Uri uri = Location;
            return ((uri.IsAbsoluteUri) ? uri.AbsoluteUri : uri.OriginalString).GetHashCode();
        }

        public override string ToString()
        {
            Uri uri = Location;
            return (uri.IsAbsoluteUri) ? uri.AbsoluteUri : ((string.IsNullOrWhiteSpace(uri.OriginalString)) ? "(empty)" : uri.OriginalString);
        }

        public static string ToIdentifierString(Guid guid) => guid.ToString("d").ToLower();

        public static string ToIdentifierString(uint serialNumber) => $"{serialNumber >> 16:x4}-{serialNumber & 0xffff:x4}";

        public static string ToIdentifierString(uint serialNumber, byte ordinal) => $"{serialNumber:x8}-{ordinal:x2}";

        public static string ToUrn(Guid guid) => $"urn:uuid:{ToIdentifierString(guid)}";

        public static string ToUrn(uint serialNumber, byte ordinal) => $"urn:volume:id:{ToIdentifierString(serialNumber, ordinal)}";

        public static string ToUrn(uint serialNumber) => $"urn:volume:id:{ToIdentifierString(serialNumber)}";
    }
}

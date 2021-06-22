using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace FsInfoCat
{
    [Serializable]
    public struct VolumeIdentifier : IEquatable<VolumeIdentifier>, IConvertible
    {
        //public const string GROUP_NAME_UUID_NS = "u";
        //public const string GROUP_NAME_VOLUME_NS = "n";
        //public const string GROUP_NAME_ID_NS = "i";
        //public const string GROUP_NAME_VALUE = "v";

        public static readonly ValueConverter<VolumeIdentifier, string> Converter = new(
            v => v.ToString(),
            s => Parse(s)
        );

        /// <summary>
        /// Matches consecutive and trailing URI path separators, allowing up to 3 consecutive path separator characters following the scheme separator.
        /// </summary>
        /// <remarks>This will also match surrounding whitespace and relative self-reference sequences (<c>/./<c>).. This does not match parent segment
        /// references (<c>/../</c>) unless they are at the beginning of the string.</remarks>
        public static readonly Regex PathSeparatorNormalize = new(@"^\s*(\.\.?/+|\s+)+|(?<!^\s*file:/?)/(?=/)|/\.(?=/|$)", RegexOptions.Compiled);
        //public static readonly Regex NsPathRegex = new($@"^((?<{GROUP_NAME_UUID_NS}>uuid(:|$))|(?<{GROUP_NAME_VOLUME_NS}>volume(:(?<{GROUP_NAME_ID_NS}>id(:|$))?|$))?)(?<{GROUP_NAME_VALUE}>.+)?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static readonly Regex VsnRegex = new(@"^([a-f\d]{4})-([a-f\d]{4})$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static readonly Regex UuidRegex = new(@"^[a-f\d]{8}(-[a-f\d]{4}){4}[a-f\d]{8}$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static readonly VolumeIdentifier Empty = new(null);

        private readonly Guid? _uuid;
        private readonly uint? _serialNumber;
        private readonly Uri _location;

        public Guid? UUID => _uuid;

        public uint? SerialNumber => _serialNumber;

        public Uri Location => _location ?? Empty._location;

        public VolumeIdentifier(uint vsn)
        {
            _serialNumber = vsn;
            _location = new($"{UriHelper.URI_SCHEME_URN}:{UriHelper.URN_NAMESPACE_VOLUME}:{UriHelper.URN_NAMESPACE_ID}:{vsn >> 16:x4}-{vsn & 0xffff:x4}", UriKind.Absolute);
            _uuid = null;
        }

        public VolumeIdentifier(Guid uuid)
        {
            _uuid = uuid;
            _location = new($"{UriHelper.URI_SCHEME_URN}:{UriHelper.URN_NAMESPACE_UUID}:{uuid:d}", UriKind.Absolute);
            _serialNumber = null;
        }

        public VolumeIdentifier(Uri uri)
        {
            if (uri is null)
            {
                _uuid = null;
                _location = new("", UriKind.Relative);
                _serialNumber = null;
                return;
            }
            if (!(string.IsNullOrEmpty(uri.Query) && string.IsNullOrEmpty(uri.Fragment)))
                throw new ArgumentOutOfRangeException(nameof(uri));
            if (!uri.IsAbsoluteUri)
            {
                if (uri.ToString().Length > 0)
                    throw new ArgumentOutOfRangeException(nameof(uri));
                _location = uri;
                _serialNumber = null;
                _uuid = null;
                return;
            }
            string path = PathSeparatorNormalize.Replace(uri.AbsolutePath, "");
            if (path.EndsWith(UriHelper.URI_PATH_SEPARATOR_STRING))
                path = path[0..^1];
            if (uri.Scheme == UriHelper.URI_SCHEME_URN)
            {
                string prefix = $"{UriHelper.URN_NAMESPACE_VOLUME}:{UriHelper.URN_NAMESPACE_ID}:";
                if (path.StartsWith(prefix))
                {
                    if (path.Length > prefix.Length)
                    {
                        Match match = VsnRegex.Match(path[prefix.Length..]);
                        if (match.Success && uint.TryParse(match.Groups[1].Value, NumberStyles.HexNumber, null, out uint vsnH) &&
                            uint.TryParse(match.Groups[2].Value, NumberStyles.HexNumber, null, out uint vsnL))
                        {
                            _serialNumber = vsnL | (vsnH << 16);
                            _location = new($"{UriHelper.URI_SCHEME_URN}:{UriHelper.URN_NAMESPACE_VOLUME}:{UriHelper.URN_NAMESPACE_ID}:{vsnH:x4}-{vsnL:x4}", UriKind.Absolute);
                            _uuid = null;
                            return;
                        }
                    }
                }
                else
                {
                    prefix = $"{UriHelper.URN_NAMESPACE_UUID}:";
                    if (path.StartsWith(prefix) && path.Length > prefix.Length)
                    {
                        Match match = UuidRegex.Match(path[prefix.Length..]);
                        if (match.Success)
                        {
                            Guid id = Guid.Parse(match.Value);
                            _uuid = id;
                            _location = new($"{UriHelper.URI_SCHEME_URN}:{UriHelper.URN_NAMESPACE_UUID}:{id:d}", UriKind.Absolute);
                            _serialNumber = null;
                            return;
                        }
                    }
                }
                throw new ArgumentOutOfRangeException(nameof(uri));
            }
            if (uri.Scheme == Uri.UriSchemeFile)
            {
                if (string.IsNullOrWhiteSpace(uri.Host))
                    throw new ArgumentOutOfRangeException(nameof(uri));
                if (path.Split('/').Length != 2)
                    throw new ArgumentOutOfRangeException(nameof(uri));
            }
            else
                throw new ArgumentOutOfRangeException(nameof(uri));
            _uuid = null;
            _location = new Uri($"file://{uri.Host}{path}", UriKind.Absolute);
            _serialNumber = null;
        }

        public static VolumeIdentifier Parse(string text)
        {
            if (text is null || (text = text.TrimStart()).Length == 0)
                return new VolumeIdentifier();

            Match match = VsnRegex.Match(text);
            if (match.Success)
            {
                if (uint.TryParse(match.Groups[1].Value, NumberStyles.HexNumber, null, out uint vsnH) &&
                    uint.TryParse(match.Groups[2].Value, NumberStyles.HexNumber, null, out uint vsnL))
                    return new VolumeIdentifier(vsnL | (vsnH << 16));
            }
            else if (UuidRegex.IsMatch(text))
                return new VolumeIdentifier(Guid.Parse(text));
            else
                try { return new VolumeIdentifier(new Uri(text, UriKind.Absolute)); }
                catch (FormatException e) { throw new ArgumentOutOfRangeException(nameof(text), e.Message); }
                catch { /* okay to ignore */ }
            throw new ArgumentOutOfRangeException(nameof(text));
        }

        public static bool TryParse(string text, out VolumeIdentifier result)
        {
            if (text is null || (text = text.TrimStart()).Length == 0)
            {
                result = new VolumeIdentifier();
                return true;
            }
            Match match = VsnRegex.Match(text);
            if (match.Success)
            {
                if (uint.TryParse(match.Groups[1].Value, NumberStyles.HexNumber, null, out uint vsnH) &&
                    uint.TryParse(match.Groups[1].Value, NumberStyles.HexNumber, null, out uint vsnL))
                {
                    result = new VolumeIdentifier(vsnL | (vsnH << 16));
                    return true;
                }
            }
            else if (UuidRegex.IsMatch(text))
            {
                result = new VolumeIdentifier(Guid.Parse(text));
                return true;
            }
            else if (Uri.TryCreate(text, UriKind.Absolute, out Uri uri))
                try
                {
                    result = new VolumeIdentifier(uri);
                    return true;
                }
                catch { /* okay to ignore */ }
            result = default;
            return false;
        }

        public bool Equals(VolumeIdentifier other)
        {
            if (_location is null || !_location.IsAbsoluteUri)
                return (other._location is null || !other._location.IsAbsoluteUri);
            if (other._location is null || !other._location.IsAbsoluteUri)
                return false;
            if (_uuid.HasValue)
                return other._uuid.HasValue && _uuid.Value.Equals(other._uuid.Value);
            if (_serialNumber.HasValue)
                return other._serialNumber.HasValue && _serialNumber.Value.Equals(other._serialNumber.Value);
            return !(other._uuid.HasValue || other._serialNumber.HasValue) && _location.AbsolutePath.Equals(other._location.AbsolutePath, StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool Equals(object obj) => obj is VolumeIdentifier identifier && Equals(identifier);

        public override int GetHashCode() => (_location is null || !_location.IsAbsoluteUri) ? 0 : _location.AbsolutePath.GetHashCode();

        public bool IsEmpty() => _location is null;

        public override string ToString() => (_location is null) ? "" : _location.ToString();

        TypeCode IConvertible.GetTypeCode() => TypeCode.String;
        bool IConvertible.ToBoolean(IFormatProvider provider) => _serialNumber.HasValue ? Convert.ToBoolean(_serialNumber.Value, provider) :
            Convert.ToBoolean(ToString(), provider);
        byte IConvertible.ToByte(IFormatProvider provider) => _serialNumber.HasValue ? Convert.ToByte(_serialNumber.Value, provider) :
            Convert.ToByte(ToString(), provider);
        char IConvertible.ToChar(IFormatProvider provider) => Convert.ToChar(ToString(), provider);
        DateTime IConvertible.ToDateTime(IFormatProvider provider) => _serialNumber.HasValue ? Convert.ToDateTime(_serialNumber.Value, provider) :
            Convert.ToDateTime(ToString(), provider);
        decimal IConvertible.ToDecimal(IFormatProvider provider) => _serialNumber.HasValue ? Convert.ToDecimal(_serialNumber.Value, provider) :
            Convert.ToDecimal(ToString(), provider);
        double IConvertible.ToDouble(IFormatProvider provider) => _serialNumber.HasValue ? Convert.ToDouble(_serialNumber.Value, provider) :
            Convert.ToDouble(ToString(), provider);
        short IConvertible.ToInt16(IFormatProvider provider) => _serialNumber.HasValue ? Convert.ToInt16(_serialNumber.Value, provider) :
            Convert.ToInt16(ToString(), provider);
        int IConvertible.ToInt32(IFormatProvider provider) => _serialNumber.HasValue ? Convert.ToInt32(_serialNumber.Value, provider) :
            Convert.ToInt32(ToString(), provider);
        long IConvertible.ToInt64(IFormatProvider provider) => _serialNumber.HasValue ? Convert.ToInt64(_serialNumber.Value, provider) :
            Convert.ToInt64(ToString(), provider);
        sbyte IConvertible.ToSByte(IFormatProvider provider) => _serialNumber.HasValue ? Convert.ToSByte(_serialNumber.Value, provider) :
            Convert.ToSByte(ToString(), provider);
        float IConvertible.ToSingle(IFormatProvider provider) => _serialNumber.HasValue ? Convert.ToSingle(_serialNumber.Value, provider) :
            Convert.ToSingle(ToString(), provider);
        string IConvertible.ToString(IFormatProvider provider) => ToString();
        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            if (!(conversionType is null))
            {
                if (conversionType.Equals(typeof(VolumeIdentifier)))
                    return this;
                if (conversionType.Equals(typeof(Uri)))
                    return _location ?? Empty._location;
                if (conversionType.Equals(typeof(Guid)) && _uuid.HasValue)
                    return _uuid.Value;
            }
            return Convert.ChangeType(ToString(), conversionType, provider);
        }
        ushort IConvertible.ToUInt16(IFormatProvider provider) => _serialNumber.HasValue ? Convert.ToUInt16(_serialNumber.Value, provider) :
            Convert.ToUInt16(ToString(), provider);
        uint IConvertible.ToUInt32(IFormatProvider provider) => _serialNumber ?? Convert.ToUInt32(ToString(), provider);
        ulong IConvertible.ToUInt64(IFormatProvider provider) => _serialNumber.HasValue ? Convert.ToUInt64(_serialNumber.Value, provider) :
            Convert.ToUInt64(ToString(), provider);

        public static bool operator ==(VolumeIdentifier left, VolumeIdentifier right) => left.Equals(right);

        public static bool operator !=(VolumeIdentifier left, VolumeIdentifier right) => !(left == right);

        public static implicit operator VolumeIdentifier(string text) => TryParse(text, out VolumeIdentifier result) ? result : new VolumeIdentifier();

        public static implicit operator string(VolumeIdentifier volumeIdentifier) => volumeIdentifier.ToString();

        public static implicit operator VolumeIdentifier(Uri uri) => new(uri);

        public static implicit operator Uri(VolumeIdentifier volumeIdentifier) => volumeIdentifier._location ?? Empty._location;
    }
}

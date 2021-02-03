using System;
using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace FsInfoCat.Util
{
    public class UniqueIdentifier : MarshalByRefObject, IEquatable<UniqueIdentifier>
    {
        public const string GROUP_NAME_UUID_NS = "u";
        public const string GROUP_NAME_VOLUME_NS = "n";
        public const string GROUP_NAME_ID_NS = "i";
        public const string GROUP_NAME_VALUE = "v";
        public static Regex NsPathRegex = new Regex($@"^((?<{GROUP_NAME_UUID_NS}>uuid(:|$))|(?<{GROUP_NAME_VOLUME_NS}>volume(:(?<{GROUP_NAME_ID_NS}>id(:|$))?|$))?)(?<{GROUP_NAME_VALUE}>.+)?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static Regex SerialNumberPathRegex = new Regex(@"^(?:(?=[a-f\d]{1,4}-[a-f\d])([a-f\d]{1,4})-([a-f\d]{1,4})|([a-f\d]{5,8})(?:-([a-f\d]{1,2}))?)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private const string URI_SCHEME_URN = "urn";
        private const string URI_SCHEME_FILE = "file";
        private const string URN_NAMESPACE_VOLUME = "volume";
        private const string URN_NAMESPACE_ID = "id";
        private const string URN_NAMESPACE_UUID = "uuid";
        private const string ERROR_MESSAGE_INVALID_UUID_URI = "Invalid UUID URI format";
        private const string ERROR_MESSAGE_INVALID_VOLUME_ID_URI = "Invalid volume identifier URI format";
        private const string ERROR_MESSAGE_INVALID_URN_NAMESPACE = "Unsupported URN namespace";
        private const string ERROR_MESSAGE_INVALID_HOST_NAME = "Invalid host name or path type";
        private const string ERROR_MESSAGE_INVALID_URI_SCHEME = "Unsupported URI scheme";

        public Uri URL { get; }

        public Guid? UUID { get; }

        public uint? SerialNumber { get; }

        public byte? Ordinal { get; }

        public string Value { get; }

        public UniqueIdentifier(Guid guid)
        {
            URL = new Uri(ToUrn(guid), UriKind.Absolute);
            UUID = guid;
            SerialNumber = null;
            Ordinal = null;
            Value = ToIdentifierString(guid);
        }

        public UniqueIdentifier(uint serialNumber, byte ordinal)
        {
            URL = new Uri(ToUrn(serialNumber, ordinal), UriKind.Absolute);
            UUID = null;
            SerialNumber = serialNumber;
            Ordinal = ordinal;
            Value = ToIdentifierString(serialNumber);
        }

        public UniqueIdentifier(uint serialNumber)
        {
            URL = new Uri(ToUrn(serialNumber), UriKind.Absolute);
            UUID = null;
            SerialNumber = serialNumber;
            Ordinal = null;
            Value = ToIdentifierString(serialNumber);
        }

        public UniqueIdentifier(string url)
        {
            Uri uri;
            try { uri = new Uri(url, UriKind.Absolute); }
            catch (Exception exc)
            {
                if (Uri.TryCreate(url, UriKind.Relative, out uri))
                    throw new ArgumentOutOfRangeException(nameof(url), url, "URI must be absolute");
                throw new ArgumentOutOfRangeException(nameof(url), url, (string.IsNullOrWhiteSpace(exc.Message)) ? "Invalid URI string" : exc.Message);
            }
            switch (uri.Scheme)
            {
                case URI_SCHEME_URN:
                    Debug.WriteLine($"Parsing URN {uri.AbsolutePath}");
                    uri = ToNormalizedUri(uri, true);
                    ParseUrn(uri.AbsolutePath, out Guid? uuid, out uint? serialNumber, out byte? ordinal);
                    break;
                case URI_SCHEME_FILE:
                    Debug.WriteLine($"Parsing file {uri.AbsolutePath}");
                    switch (uri.HostNameType)
                    {
                        case UriHostNameType.Basic:
                        case UriHostNameType.Unknown:
                            throw new ArgumentOutOfRangeException(nameof(url), ERROR_MESSAGE_INVALID_HOST_NAME);
                    }
                    uri = ToNormalizedUri(uri, false);
                    UUID = null;
                    SerialNumber = null;
                    Ordinal = null;
                    Value = uri.AbsolutePath;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(url), ERROR_MESSAGE_INVALID_URI_SCHEME);
            }
            URL = uri;
        }

        public bool Equals(UniqueIdentifier other) => null != other && (ReferenceEquals(this, other) || URL.AbsoluteUri.Equals(other.URL.AbsoluteUri, StringComparison.InvariantCultureIgnoreCase));

        public override bool Equals(object obj) => Equals(obj as UniqueIdentifier);

        public override int GetHashCode() => URL.AbsoluteUri.GetHashCode();

        public static void ParseUrn(string uriString, out Guid? uuid, out uint? serialNumber, out byte? ordinal)
        {
            Match match = NsPathRegex.Match(uriString);
            if (match.Groups[GROUP_NAME_VALUE].Success)
            {
                Debug.WriteLine("Value match");
                if (match.Groups[GROUP_NAME_UUID_NS].Success)
                {
                    Debug.WriteLine("UUID NS match");
                    if (Guid.TryParse(match.Groups[GROUP_NAME_VALUE].Value, out Guid id))
                    {
                        Debug.WriteLine("GUID parsed");
                        uuid = id;
                        serialNumber = null;
                        ordinal = null;
                        return;
                    }
                    throw new ArgumentOutOfRangeException(nameof(uriString), ERROR_MESSAGE_INVALID_UUID_URI);
                }
                uuid = null;
                if (match.Groups[GROUP_NAME_ID_NS].Success)
                {
                    Debug.WriteLine("ID matched");
                    if ((match = SerialNumberPathRegex.Match(match.Groups[GROUP_NAME_VALUE].Value)).Success)
                    {
                        Debug.WriteLine("Serial number pattern matched");
                        if (match.Groups[3].Success)
                        {
                            Debug.WriteLine("Group 3 matched");
                            if (uint.TryParse(match.Groups[3].Value, NumberStyles.HexNumber, null, out uint sn))
                            {
                                serialNumber = sn;
                                if (match.Groups[4].Success)
                                {
                                    Debug.WriteLine("Group 4 matched");
                                    if (byte.TryParse(match.Groups[4].Value, NumberStyles.HexNumber, null, out byte ov))
                                    {
                                        Debug.WriteLine("Group 4 parsed");
                                        ordinal = ov;
                                        return;
                                    }
                                }
                                else
                                {
                                    Debug.WriteLine("Group 4 not matched");
                                    ordinal = null;
                                    return;
                                }
                            }
                            throw new ArgumentOutOfRangeException(nameof(uriString), ERROR_MESSAGE_INVALID_VOLUME_ID_URI);
                        }
                        ordinal = null;
                        if (uint.TryParse(match.Groups[1].Value, out uint h) && uint.TryParse(match.Groups[2].Value, out uint l))
                        {
                            Debug.WriteLine("Group matched and parsed");
                            serialNumber = (h << 16) | l;
                            return;
                        }
                    }
                    throw new ArgumentOutOfRangeException(nameof(uriString), ERROR_MESSAGE_INVALID_VOLUME_ID_URI);
                }
            }

            if (match.Groups[GROUP_NAME_UUID_NS].Success)
                throw new ArgumentOutOfRangeException(nameof(uriString), ERROR_MESSAGE_INVALID_UUID_URI);

            throw new ArgumentOutOfRangeException(nameof(uriString), (match.Groups[GROUP_NAME_VOLUME_NS].Success) ?
                ERROR_MESSAGE_INVALID_VOLUME_ID_URI : ERROR_MESSAGE_INVALID_URN_NAMESPACE);
        }

        public static Uri ToNormalizedUri(Uri url, bool noTrailingSlash)
        {
            UriBuilder uriBuilder;
            if (url.Fragment == "#")
            {
                uriBuilder = new UriBuilder(url);
                uriBuilder.Fragment = "";
                if (uriBuilder.Query == "?")
                    uriBuilder.Query = "";
                if (noTrailingSlash != (uriBuilder.Path.EndsWith("/") && uriBuilder.Path.Length > 1))
                    return uriBuilder.Uri;
            }
            else if (url.PathAndQuery.EndsWith("?"))
            {
                uriBuilder = new UriBuilder(url);
                uriBuilder.Query = "";
                if (noTrailingSlash != (uriBuilder.Path.EndsWith("/") && uriBuilder.Path.Length > 1))
                    return uriBuilder.Uri;
            }
            else
            {
                if (noTrailingSlash != (url.AbsolutePath.EndsWith("/") && url.AbsolutePath.Length > 1))
                    return url;
                uriBuilder = new UriBuilder(url);
            }
            if (noTrailingSlash)
            {
                int i = uriBuilder.Path.Length - 1;
                while (i > 1 && uriBuilder.Path[i - 1] == '/')
                    i--;
                uriBuilder.Path = uriBuilder.Path.Substring(0, i);
            }
            else
                uriBuilder.Path += "/";
            return uriBuilder.Uri;
        }

        public static string ToIdentifierString(Guid guid) => guid.ToString("d").ToLower();

        public static string ToIdentifierString(uint serialNumber) => $"{(serialNumber >> 16).ToString("x4")}-{(serialNumber & 0xffff).ToString("x4")}";

        public static string ToIdentifierString(uint serialNumber, byte ordinal) => $"{serialNumber.ToString("x8")}-{ordinal.ToString("x2")}";

        public static string ToUrn(Guid guid) => $"urn:uuid:{ToIdentifierString(guid)}";

        public static string ToUrn(uint serialNumber, byte ordinal) => $"rn:volume:id:{ToIdentifierString(serialNumber, ordinal)}";

        public static string ToUrn(uint serialNumber) => $"urn:volume:id:{ToIdentifierString(serialNumber)}";
    }
}

using System;
using System.Globalization;
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
            if (url is null)
                throw new ArgumentNullException(nameof(url));
            Uri uri;
            try { uri = new Uri(url, UriKind.Absolute); }
            catch (Exception exc)
            {
                if (Uri.TryCreate(url, UriKind.Relative, out uri))
                    throw new ArgumentOutOfRangeException(nameof(url), url, "URI must be absolute");
                int index = (exc.Message is null) ? -1 : exc.Message.IndexOf("(Parameter '");
                throw new ArgumentOutOfRangeException(nameof(uri), (index < 1) ? exc.Message : exc.Message.Substring(0, index).Trim());
           }
            switch (uri.Scheme)
            {
                case URI_SCHEME_URN:
                    try { uri = NormalizeUri(uri, false, false, NormalizePathOption.NoTrailingSeparator); }
                    catch (ArgumentOutOfRangeException exc)
                    {
                        int index = (exc.Message is null) ? -1 : exc.Message.IndexOf("(Parameter '");
                        throw new ArgumentOutOfRangeException(nameof(uri), (index < 1) ? exc.Message : exc.Message.Substring(0, index).Trim());
                    }
                    catch (ArgumentException exc)
                    {
                        int index = (exc.Message is null) ? -1 : exc.Message.IndexOf("(Parameter '");
                        throw new ArgumentException(nameof(uri), (index < 1) ? exc.Message : exc.Message.Substring(0, index).Trim());
                    }
                    Guid? uuid;
                    uint? serialNumber;
                    byte? ordinal;
                    try { ParseUrnPath(uri.AbsolutePath, out uuid, out serialNumber, out ordinal); }
                    catch (ArgumentOutOfRangeException exc)
                    {
                        int index = (exc.Message is null) ? -1 : exc.Message.IndexOf("(Parameter '");
                        throw new ArgumentOutOfRangeException(nameof(uri), (index < 1) ? exc.Message : exc.Message.Substring(0, index).Trim());
                    }
                    catch (ArgumentException exc)
                    {
                        int index = (exc.Message is null) ? -1 : exc.Message.IndexOf("(Parameter '");
                        throw new ArgumentException(nameof(uri), (index < 1) ? exc.Message : exc.Message.Substring(0, index).Trim());
                    }
                    if (uuid.HasValue)
                        Value = uuid.Value.ToString("d").ToLower();
                    else if (ordinal.HasValue)
                        Value = $"{serialNumber.Value.ToString("x8")}-{ordinal.Value.ToString("x2")}";
                    else
                        Value = $"{(serialNumber.Value >> 16).ToString("x4")}-{(serialNumber.Value & 0xffff).ToString("x4")}";
                    UUID = uuid;
                    SerialNumber = serialNumber;
                    Ordinal = ordinal;
                    break;
                case URI_SCHEME_FILE:
                    uri = NormalizeUri(uri, false, false, NormalizePathOption.TrailingSlash);
                    if (uri.Query.Length > 0 || uri.Fragment.Length > 0)
                        throw new ArgumentOutOfRangeException(nameof(url), ERROR_MESSAGE_INVALID_HOST_NAME);
                    switch (uri.HostNameType)
                    {
                        case UriHostNameType.Basic:
                        case UriHostNameType.Unknown:
                            throw new ArgumentOutOfRangeException(nameof(url), ERROR_MESSAGE_INVALID_HOST_NAME);
                    }
                    UUID = null;
                    SerialNumber = null;
                    Ordinal = null;
                    Value = uri.LocalPath;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(url), ERROR_MESSAGE_INVALID_URI_SCHEME);
            }
            URL = uri;
        }

        public bool Equals(UniqueIdentifier other) => null != other &&
            (ReferenceEquals(this, other) || URL.AbsoluteUri.Equals(other.URL.AbsoluteUri, StringComparison.InvariantCultureIgnoreCase));

        public override bool Equals(object obj) => Equals(obj as UniqueIdentifier);

        public override int GetHashCode() => URL.AbsoluteUri.GetHashCode();

        public static void ParseUrnPath(string uriPath, out Guid? uuid, out uint? serialNumber, out byte? ordinal)
        {
            Match match = NsPathRegex.Match(uriPath);
            if (match.Groups[GROUP_NAME_VALUE].Success)
            {
                if (match.Groups[GROUP_NAME_UUID_NS].Success)
                {
                    if (Guid.TryParse(Uri.UnescapeDataString(match.Groups[GROUP_NAME_VALUE].Value), out Guid id))
                    {
                        uuid = id;
                        serialNumber = null;
                        ordinal = null;
                        return;
                    }
                    throw new ArgumentOutOfRangeException(nameof(uriPath), ERROR_MESSAGE_INVALID_UUID_URI);
                }
                uuid = null;
                if (match.Groups[GROUP_NAME_ID_NS].Success)
                {
                    if ((match = SerialNumberPathRegex.Match(Uri.UnescapeDataString(match.Groups[GROUP_NAME_VALUE].Value))).Success)
                    {
                        if (match.Groups[3].Success)
                        {
                            if (uint.TryParse(Uri.UnescapeDataString(match.Groups[3].Value), NumberStyles.HexNumber, null, out uint sn))
                            {
                                serialNumber = sn;
                                if (match.Groups[4].Success)
                                {
                                    if (byte.TryParse(Uri.UnescapeDataString(match.Groups[4].Value), NumberStyles.HexNumber, null, out byte ov))
                                    {
                                        ordinal = ov;
                                        return;
                                    }
                                }
                                else
                                {
                                    ordinal = null;
                                    return;
                                }
                            }
                            throw new ArgumentOutOfRangeException(nameof(uriPath), ERROR_MESSAGE_INVALID_VOLUME_ID_URI);
                        }
                        ordinal = null;
                        if (uint.TryParse(Uri.UnescapeDataString(match.Groups[1].Value), NumberStyles.HexNumber, null, out uint h) &&
                            uint.TryParse(Uri.UnescapeDataString(match.Groups[2].Value), NumberStyles.HexNumber, null, out uint l))
                        {
                            serialNumber = (h << 16) | l;
                            return;
                        }
                    }
                    throw new ArgumentOutOfRangeException(nameof(uriPath), ERROR_MESSAGE_INVALID_VOLUME_ID_URI);
                }
            }

            if (match.Groups[GROUP_NAME_UUID_NS].Success)
                throw new ArgumentOutOfRangeException(nameof(uriPath), ERROR_MESSAGE_INVALID_UUID_URI);

            throw new ArgumentOutOfRangeException(nameof(uriPath), ((match.Groups[GROUP_NAME_VOLUME_NS].Success) ?
                ERROR_MESSAGE_INVALID_VOLUME_ID_URI : ERROR_MESSAGE_INVALID_URN_NAMESPACE));
        }

        public enum NormalizePathOption
        {
            None,
            TrailingSlash,
            NoTrailingSeparator,
            NoTrailingSlash,
            Remove
        }

        public static Uri NormalizeUri(Uri uri, bool stripQuery = false, bool stripFragment = false, NormalizePathOption pathOption = NormalizePathOption.None)
        {
            if (uri is null)
                throw new ArgumentNullException(nameof(uri));

            System.Diagnostics.Debug.WriteLine($"Normalizing {uri.OriginalString}");
            bool isRelative;

            #region Convert uri to normalized absolute

            Uri returnUri = uri;
            if (uri.IsAbsoluteUri)
            {
                isRelative = false;
                if (!uri.AbsoluteUri.Equals(uri.OriginalString))
                    returnUri = uri = new Uri(uri.AbsoluteUri, UriKind.Absolute);
            }
            else
            {
                isRelative = true;
                string uriString = uri.OriginalString;
                if (uriString.Length == 0)
                    return (pathOption == NormalizePathOption.TrailingSlash) ? new Uri("/", UriKind.Relative) : uri;
                if (uriString.Length == 1)
                    switch (uriString[0])
                    {
                        case '/':
                            switch (pathOption)
                            {
                                case NormalizePathOption.NoTrailingSeparator:
                                case NormalizePathOption.NoTrailingSlash:
                                case NormalizePathOption.Remove:
                                    return new Uri("", UriKind.Relative);
                            }
                            return uri;
                        case ':':
                            switch (pathOption)
                            {
                                case NormalizePathOption.None:
                                case NormalizePathOption.TrailingSlash:
                                    return new Uri("/", UriKind.Relative);
                            }
                            return new Uri("", UriKind.Relative);
                        case '?':
                        case '#':
                            return new Uri((pathOption == NormalizePathOption.TrailingSlash) ? "/" : "", UriKind.Relative);
                        case '\\':
                            switch (pathOption)
                            {
                                case NormalizePathOption.NoTrailingSlash:
                                case NormalizePathOption.Remove:
                                    return new Uri("", UriKind.Relative);
                            }
                            return new Uri("/", UriKind.Relative);
                        default:
                            if (!Uri.IsWellFormedUriString(uriString, UriKind.Relative))
                                return new Uri(Uri.EscapeUriString(uriString), UriKind.Relative);
                            return uri;
                    }
                int index = uriString.IndexOfAny(new char[] { '?', '#' });
                if (index > 0)
                {
                    string p = uriString.Substring(0, index);
                    if (p.Contains("\\"))
                        uriString = p.Replace('\\', '/') + uriString.Substring(index);
                    if (Uri.IsWellFormedUriString(uriString, UriKind.Relative))
                        returnUri = new Uri(uriString, UriKind.Relative);
                    else
                    {
                        uriString = Uri.EscapeUriString(uriString);
                        if (uriString[0] == ':' && !Uri.IsWellFormedUriString(uriString, UriKind.Absolute))
                            uriString = $"/{uriString.Substring(1)}";
                        returnUri = new Uri(uriString, UriKind.Relative);
                    }
                }
                else if (!Uri.IsWellFormedUriString(uriString, UriKind.Relative))
                {
                    uriString = Uri.EscapeUriString(uriString);
                    if (uriString[0] == ':' && !Uri.IsWellFormedUriString(uriString, UriKind.Absolute))
                        uriString = $"/{uriString.Substring(1)}";
                    returnUri = new Uri(uriString, UriKind.Relative);
                }
                else
                    returnUri = uri;
                uri = new Uri($"urn:{returnUri.OriginalString}", UriKind.Absolute);
            }

            #endregion

            string fragment = uri.Fragment;
            string query = uri.Query;
            string path = uri.AbsolutePath;
            switch (pathOption)
            {
                case NormalizePathOption.NoTrailingSlash:
                    if (path == "/")
                        path = "";
                    else if (path.Length > 0 && path[path.Length - 1] == '/')
                        path = path.Substring(0, path.Length - 1);
                    break;
                case NormalizePathOption.NoTrailingSeparator:
                    if (path.Length > 0)
                    {
                        switch (path[path.Length - 1])
                        {
                            case '/':
                            case ':':
                                path = (path.Length == 1) ? "" : path.Substring(0, path.Length - 1);
                                break;
                        }
                    }
                    break;
                case NormalizePathOption.TrailingSlash:
                    if (path.Length == 0)
                        path = "/";
                    else
                        switch (path[path.Length - 1])
                        {
                            case '/':
                                break;
                            case ':':
                                path = (path.Length == 1) ? "/" : $"{path.Substring(0, path.Length - 1)}/";
                                break;
                            default:
                                path += "/";
                                break;
                        }
                    break;
                case NormalizePathOption.Remove:
                    path = "";
                    break;
            }
            if (fragment == "#" || stripFragment)
                fragment = "";
            if (query == "?" || stripQuery)
                query = "";
            if (fragment == uri.Fragment && query == uri.Query && path == uri.AbsolutePath)
                return returnUri;
            if (isRelative)
                return new Uri(path + query + fragment, UriKind.Relative);
            UriComponents components = UriComponents.Scheme;
            if (uri.Host.Length > 0)
            {
                if (uri.UserInfo.Length > 0)
                    components |= UriComponents.UserInfo;
                components |= UriComponents.Host;
                if (!uri.IsDefaultPort)
                    components |= UriComponents.Port;
            }
            if (path.Length > 0)
                components |= UriComponents.Path;
            if (query.Length > 0)
                components |= UriComponents.Query;
            if (fragment.Length > 0)
                components |= UriComponents.Fragment;
            Uri result = new Uri(returnUri.GetComponents(components, UriFormat.UriEscaped), UriKind.Absolute);
            if (result.AbsolutePath.EndsWith("/") == path.EndsWith("/"))
                return result;
            UriBuilder uriBuilder = new UriBuilder(result);
            if (path.EndsWith("/"))
                uriBuilder.Path += "/";
            else
                uriBuilder.Path = uriBuilder.Path.Substring(0, uriBuilder.Path.Length - 1);
            return uriBuilder.Uri;
        }

        public static string ToIdentifierString(Guid guid) => guid.ToString("d").ToLower();

        public static string ToIdentifierString(uint serialNumber) => $"{(serialNumber >> 16).ToString("x4")}-{(serialNumber & 0xffff).ToString("x4")}";

        public static string ToIdentifierString(uint serialNumber, byte ordinal) => $"{serialNumber.ToString("x8")}-{ordinal.ToString("x2")}";

        public static string ToUrn(Guid guid) => $"urn:uuid:{ToIdentifierString(guid)}";

        public static string ToUrn(uint serialNumber, byte ordinal) => $"urn:volume:id:{ToIdentifierString(serialNumber, ordinal)}";

        public static string ToUrn(uint serialNumber) => $"urn:volume:id:{ToIdentifierString(serialNumber)}";
    }
}

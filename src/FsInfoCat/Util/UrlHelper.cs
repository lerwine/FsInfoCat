using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace FsInfoCat.Util
{
    public static class UrlHelper
    {
        public const string URI_SCHEME_URN = "urn";
        public const string URN_NAMESPACE_UUID = "uuid";
        public const string URN_NAMESPACE_VOLUME = "volume";
        public const string URN_NAMESPACE_ID = "id";

        public const UriComponents BEFORE_USERINFO_COMPONENTS = UriComponents.Scheme | UriComponents.KeepDelimiter;

        public const UriComponents AFTER_USERINFO_COMPONENTS = UriComponents.HostAndPort | UriComponents.PathAndQuery |
            UriComponents.Fragment | UriComponents.KeepDelimiter;

        public const UriComponents BEFORE_HOST_COMPONENTS = UriComponents.Scheme | UriComponents.UserInfo | UriComponents.KeepDelimiter;

        public const UriComponents AFTER_PORT_COMPONENTS = UriComponents.PathAndQuery | UriComponents.Fragment | UriComponents.KeepDelimiter;

        public const UriComponents BEFORE_PATH_COMPONENTS = UriComponents.Scheme | UriComponents.UserInfo | UriComponents.HostAndPort |
            UriComponents.KeepDelimiter;

        public const UriComponents AFTER_PATH_COMPONENTS = UriComponents.Query | UriComponents.Fragment | UriComponents.KeepDelimiter;

        public const UriComponents BEFORE_QUERY_COMPONENTS = UriComponents.Scheme | UriComponents.UserInfo | UriComponents.HostAndPort |
            UriComponents.Path | UriComponents.KeepDelimiter;

        public const UriComponents AFTER_QUERY_COMPONENTS = UriComponents.Fragment | UriComponents.KeepDelimiter;

        public const UriComponents BEFORE_FRAGMENT_COMPONENTS = UriComponents.Scheme | UriComponents.UserInfo | UriComponents.HostAndPort |
            UriComponents.PathAndQuery | UriComponents.KeepDelimiter;


        private static readonly char[] QueryOrFragmentChar = new char[] { '?', '#' };

        public static Uri AsNormalized(this Uri uri)
        {
            if (null == uri)
                return uri;
            string originalString;
            if (uri.IsAbsoluteUri)
            {
                if (!(uri.Scheme.ToLower().Equals(uri.Scheme) && uri.Host.ToLower().Equals(uri.Host)))
                {
                    UriBuilder uriBuilder = new UriBuilder(uri);
                    uriBuilder.Scheme = uri.Scheme.ToLower();
                    uriBuilder.Host = uri.Host.ToLower();
                    if (uri.Query == "?")
                        uriBuilder.Query = "";
                    if (uri.Fragment == "#")
                        uriBuilder.Fragment = "";
                    uri = uriBuilder.Uri;
                }
                else if (!uri.Host.ToLower().Equals(uri.Host))
                {
                    if (uri.IsDefaultPort)
                        uri.TrySetHostComponent(uri.Host.ToLower(),  null, out uri);
                    uri.TrySetHostComponent(uri.Host.ToLower(),  uri.Port, out uri);
                }
                if (uri.Query == "?")
                    uri.TrySetQueryComponent(null, out uri);
                if (uri.Fragment == "#")
                    uri.TrySetFragmentComponent(null, out uri);
                originalString = uri.OriginalString;
                if (Uri.IsWellFormedUriString(originalString, UriKind.Absolute))
                    return uri;
                if (Uri.IsWellFormedUriString(uri.AbsoluteUri, UriKind.Absolute))
                    return new Uri(uri.AbsoluteUri, UriKind.Absolute);
                return new Uri(Uri.EscapeUriString(uri.OriginalString), UriKind.Absolute);
            }

            string s = uri.GetFragmentComponent();
            if (null != s && s.Length == 0)
                uri.TrySetFragmentComponent(null, out uri);
            s = uri.GetQueryComponent();
            if (null != s && s.Length == 0)
                uri.TrySetQueryComponent(null, out uri);
            if ((originalString = uri.OriginalString).Length == 0 || Uri.IsWellFormedUriString(originalString, UriKind.Relative))
                return uri;
            originalString = Uri.EscapeUriString(originalString);
            if (Uri.IsWellFormedUriString(originalString, UriKind.Relative))
                return new Uri(originalString, UriKind.Relative);
            return new Uri(Uri.EscapeDataString(uri.OriginalString), UriKind.Relative);
        }

        public static Uri SplitQueryAndFragment(this Uri uri, out string query, out string fragment)
        {
            if (uri is null)
            {
                query = fragment = null;
                return null;
            }
            if (uri.IsAbsoluteUri)
            {
                fragment = (uri.Fragment.Length > 0) ? uri.Fragment.Substring(1) : null;
                query = (uri.Fragment.Length > 0) ? uri.Fragment.Substring(1) : null;
                if (null == query && null == fragment)
                    return uri;
                UriBuilder uriBuilder = new UriBuilder(uri);
                uriBuilder.Query = uriBuilder.Fragment = "";
                return uriBuilder.Uri;
            }

            string originalString = uri.OriginalString;
            int index = originalString.IndexOfAny(QueryOrFragmentChar);
            if (index < 0)
            {
                query = fragment = null;
                return uri;
            }
            if (originalString[index] == '#')
            {
                query = null;
                fragment = originalString.Substring(index + 1);
                originalString = originalString.Substring(0, index);
            }
            else
            {
                query = originalString.Substring(index + 1);
                originalString = originalString.Substring(0, index);
                index = query.IndexOf('#');
                if (index < 0)
                    fragment = null;
                else
                {
                    fragment = query.Substring(index + 1);
                    query = query.Substring(0, index);
                }
            }
            return new Uri(originalString, UriKind.Relative);
        }

        public static bool AuthorityCaseInsensitiveEquals(this Uri uri, Uri other)
        {
            if (null == uri)
                return null == other;
            if (null == other)
                return false;
            if (ReferenceEquals(uri, other))
                return true;
            if (uri.IsAbsoluteUri)
            {
                if (!other.IsAbsoluteUri)
                    return false;
                UriComponents c = UriComponents.Scheme | UriComponents.UserInfo | UriComponents.HostAndPort | UriComponents.KeepDelimiter;
                return uri.GetComponents(c, UriFormat.UriEscaped).Equals(other.GetComponents(c, UriFormat.UriEscaped),
                    StringComparison.InvariantCultureIgnoreCase) &&
                    uri.PathAndQuery.Equals(other.PathAndQuery) && uri.Fragment.Equals(other.Fragment);
            }
            return !other.IsAbsoluteUri && uri.OriginalString.Equals(other.OriginalString);
        }

        public static string AsRelativeUriString(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;
            if (!Uri.IsWellFormedUriString(value, UriKind.RelativeOrAbsolute))
                value = Uri.EscapeUriString(value);
            if (Uri.IsWellFormedUriString(value, UriKind.Absolute))
                return (new Uri(value, UriKind.Absolute).GetComponents(UriComponents.PathAndQuery | UriComponents.Fragment | UriComponents.KeepDelimiter, UriFormat.UriEscaped));
            return (Uri.IsWellFormedUriString(value, UriKind.Relative)) ? value : Uri.EscapeUriString(value);
        }

        public static string AsUserNameComponentEncoded(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return (Uri.IsWellFormedUriString(value, UriKind.Relative) ? value : Uri.EscapeUriString(value))
                .Replace("/", "%2F").Replace(":", "%3A").Replace("?", "%3F").Replace("#", "%23").Replace("@", "%40");
        }

        public static string AsPasswordComponentEncoded(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return (Uri.IsWellFormedUriString(value, UriKind.Relative) ? value : Uri.EscapeUriString(value))
                .Replace("/", "%2F").Replace("?", "%3F").Replace("#", "%23").Replace("@", "%40");
        }

        public static string GetUserNameAndPassword(this Uri uri, out string password)
        {
            if (null != uri && uri.IsAbsoluteUri)
            {
                string userInfo = uri.GetComponents(UriComponents.UserInfo | UriComponents.KeepDelimiter, UriFormat.UriEscaped);
                if (userInfo.Length > 0)
                {
                    string[] upw = uri.GetComponents(UriComponents.UserInfo, UriFormat.UriEscaped).Split(':', 2);
                    password = (upw.Length > 1) ? upw[1] : null;
                    return upw[0];
                }
            }
            password = null;
            return null;
        }

        public static bool TrySetUserInfoComponent(this Uri uri, string userName, string password, out Uri result)
        {
            if (!uri.IsAbsoluteUri || uri.Host.Length == 0)
            {
                result = uri;
                return false;
            }

            string oldUserInfo = uri.GetComponents(UriComponents.UserInfo, UriFormat.UriEscaped);
            if (null != userName)
            {
                string newUserInfo = userName.AsUserNameComponentEncoded();
                if (null != password && (newUserInfo.Length > 0 || password.Length > 0))
                    newUserInfo = $"{newUserInfo}:{password.AsPasswordComponentEncoded()}";
                if (newUserInfo.Length > 0)
                {
                    if (oldUserInfo != newUserInfo)
                        try
                        {
                            string preceding = uri.GetComponents(BEFORE_USERINFO_COMPONENTS, UriFormat.UriEscaped);
                            string following = uri.GetComponents(AFTER_USERINFO_COMPONENTS, UriFormat.UriEscaped);
                            result = new Uri($"{preceding}{newUserInfo}@{following}", UriKind.Absolute);
                            if (preceding != uri.GetComponents(BEFORE_USERINFO_COMPONENTS, UriFormat.UriEscaped) ||
                                following != uri.GetComponents(AFTER_USERINFO_COMPONENTS, UriFormat.UriEscaped))
                            {
                                result = uri;
                                return false;
                            }
                        }
                        catch
                        {
                            result = uri;
                            return false;
                        }
                    else
                        result = uri;
                    return true;
                }
            }

            if (string.IsNullOrEmpty(oldUserInfo))
                result = uri;
            else
                try
                {
                    result = new Uri(uri.GetComponents(UriComponents.Scheme | AFTER_USERINFO_COMPONENTS, UriFormat.UriEscaped), UriKind.Absolute);
                }
                catch
                {
                    result = uri;
                    return false;
                }
            return true;
        }

        public static bool TrySetHostComponent(this Uri uri, string hostName, int? port, out Uri result)
        {
            if (!uri.IsAbsoluteUri)
            {
                result = uri;
                return false;
            }


            string oldHostAndPort = uri.GetComponents(UriComponents.HostAndPort, UriFormat.UriEscaped);
            if (!string.IsNullOrEmpty(hostName))
            {
                hostName = hostName.ToLower();
                if (Uri.CheckHostName(hostName) == UriHostNameType.Unknown)
                {
                    result = uri;
                    return false;
                }
                string hostAndPort = hostName;
                if (port.HasValue && port.Value > -1)
                {
                    if (port.Value > 65535)
                    {
                        result = uri;
                        return false;
                    }
                    hostAndPort = $"{hostAndPort}:{port.Value}";
                }
                if (oldHostAndPort != hostAndPort)
                    try
                    {
                        string preceding = uri.GetComponents(BEFORE_HOST_COMPONENTS, UriFormat.UriEscaped);
                        string following = uri.GetComponents(AFTER_PORT_COMPONENTS, UriFormat.UriEscaped);
                        result = new Uri($"{preceding}{hostAndPort}{following}", UriKind.Absolute);
                        if (preceding != uri.GetComponents(BEFORE_HOST_COMPONENTS, UriFormat.UriEscaped) ||
                            following != uri.GetComponents(AFTER_PORT_COMPONENTS, UriFormat.UriEscaped))
                        {
                            result = uri;
                            return false;
                        }
                    }
                    catch
                    {
                        result = uri;
                        return false;
                    }
                else
                    result = uri;
                return true;
            }

            if (string.IsNullOrEmpty(oldHostAndPort))
                result = uri;
            else
                try
                {
                    result = new Uri(uri.GetComponents(BEFORE_HOST_COMPONENTS | AFTER_PORT_COMPONENTS, UriFormat.UriEscaped), UriKind.Absolute);
                }
                catch
                {
                    result = uri;
                    return false;
                }
            return true;
        }

        public static string GetPathComponent(this Uri uri)
        {
            if (uri is null)
                return null;
            if (uri.IsAbsoluteUri)
                return uri.GetComponents(UriComponents.Path, UriFormat.UriEscaped);
            string originalString = uri.OriginalString.AsRelativeUriString();
            int index = originalString.IndexOfAny(QueryOrFragmentChar);
            return (index < 0) ? originalString : originalString.Substring(0, index);
        }

        public static bool TrySetPathComponent(this Uri uri, string path, out Uri result)
        {
            if (uri is null)
            {
                result = uri;
                return false;
            }

            string oldPath = uri.GetPathComponent();
            Uri target;
            int index;
            if (uri.IsAbsoluteUri)
            {
                if (string.IsNullOrEmpty(path))
                {
                    if (oldPath.Length == 0)
                        result = uri;
                    else
                        try
                        {
                            result = new Uri(uri.GetComponents(BEFORE_PATH_COMPONENTS | AFTER_PATH_COMPONENTS, UriFormat.UriEscaped), UriKind.Absolute);
                        }
                        catch
                        {
                            result = uri;
                            return false;
                        }
                    return true;
                }

                path = path.AsRelativeUriString();
                target = uri;
                if ((index = path.IndexOfAny(QueryOrFragmentChar)) > -1 && !((path[index] == '#') ?
                        uri.TrySetFragmentComponent(path.Substring(index + 1), out target) :
                        uri.TrySetQueryComponent(path.Substring(index + 1), out target)))
                {
                    result = uri;
                    return false;
                }

                if (oldPath != path)
                    try
                    {
                        string preceding = uri.GetComponents(BEFORE_PATH_COMPONENTS, UriFormat.UriEscaped);
                        string following = uri.GetComponents(AFTER_PATH_COMPONENTS, UriFormat.UriEscaped);
                        if (path[0] != '/')
                        {
                            if (uri.Scheme == URI_SCHEME_URN && !preceding.Contains("/"))
                            {
                                if (uri.Host.Length > 0)
                                    path = $":{path}";
                                else
                                    path = $"::{path}";
                            }
                            else if (uri.Host.Length > 0)
                                path = $"/{path}";
                            else
                                path = $"//{path}";
                        }
                        result = new Uri($"{preceding}{path}{following}", UriKind.Relative);
                        if (preceding != uri.GetComponents(BEFORE_PATH_COMPONENTS, UriFormat.UriEscaped) ||
                            following != uri.GetComponents(AFTER_PATH_COMPONENTS, UriFormat.UriEscaped))
                        {
                            result = uri;
                            return false;
                        }
                    }
                    catch
                    {
                        result = uri;
                        return false;
                    }
                else
                    result = uri;
                return true;
            }

            string originalString = uri.OriginalString;
            int fragmentIndex;
            index = originalString.IndexOfAny(QueryOrFragmentChar);
            if (index < 0)
                index = fragmentIndex = originalString.Length;
            if (originalString[index] == '#')
                fragmentIndex = index;
            else if (index == originalString.Length - 1 || (fragmentIndex = originalString.IndexOf('#', index + 1)) < 0)
                fragmentIndex = originalString.Length;

            if (path is null)
            {
                if (index == fragmentIndex)
                    result = uri;
                else
                    try
                    {
                        if (index == 0)
                            result = new Uri((fragmentIndex < originalString.Length) ? originalString.Substring(fragmentIndex) : "", UriKind.Relative);
                        else if (fragmentIndex < originalString.Length)
                            result = new Uri(originalString.Substring(0, index) + originalString.Substring(fragmentIndex), UriKind.Relative);
                        else
                            result = new Uri(originalString.Substring(0, index), UriKind.Relative);
                    }
                    catch
                    {
                        result = uri;
                        return false;
                    }
                return true;
            }

            path = $"?{path.AsRelativeUriString()}";
            if (path.Contains("#") || fragmentIndex == originalString.Length)
            {
                if (index == 0)
                {
                    if (path.Equals(originalString))
                        result = uri;
                    else
                        try
                        {
                            result = new Uri(path, UriKind.Relative);
                        }
                        catch
                        {
                            result = uri;
                            return false;
                        }
                }
                else
                {
                    if (path.Length == originalString.Length - index && originalString.Substring(index).Equals(path))
                        result = uri;
                    else
                        try
                        {
                            result = new Uri($"{originalString.Substring(0, index)}{path}", UriKind.Relative);
                        }
                        catch
                        {
                            result = uri;
                            return false;
                        }
                }
            }
            else if (index == 0)
            {
                if (index == path.Length && path.Equals(originalString.Substring(0, index)))
                    result = uri;
                else
                    try
                    {
                        result = new Uri($"{path}{originalString.Substring(fragmentIndex)}", UriKind.Relative);
                    }
                    catch
                    {
                        result = uri;
                        return false;
                    }
            }
            else
            {
                int replaceLen = fragmentIndex - index;
                if (replaceLen == path.Length && path.Equals(originalString.Substring(index, replaceLen)))
                    result = uri;
                else
                    try
                    {
                        result = new Uri($"{originalString.Substring(0, index)}{path}{originalString.Substring(fragmentIndex)}", UriKind.Relative);
                    }
                    catch
                    {
                        result = uri;
                        return false;
                    }
            }
            return true;
        }

        public static string GetQueryComponent(this Uri uri)
        {
            if (uri is null)
                return null;
            if (uri.IsAbsoluteUri)
                return (uri.Query.Length == 0) ? null : uri.Query.Substring(1);
            string originalString = uri.OriginalString.AsRelativeUriString();
            int index = originalString.IndexOfAny(QueryOrFragmentChar);
            if (index < 0 || originalString[index] == '#')
                return null;
            int fragmentIndex = index + 1;
            if (fragmentIndex < originalString.Length && (fragmentIndex = originalString.IndexOf('#', fragmentIndex)) > index)
                return originalString.Substring(index + 1, fragmentIndex - index);
            return originalString.Substring(index + 1);
        }

        public static bool TrySetQueryComponent(this Uri uri, string query, out Uri result)
        {
            if (uri is null)
            {
                result = uri;
                return false;
            }

            Uri target;
            int index;
            if (uri.IsAbsoluteUri)
            {
                if (query is null)
                {
                    if (uri.Query.Length == 0)
                        result = uri;
                    else
                        try
                        {
                            result = new Uri(uri.GetComponents(BEFORE_QUERY_COMPONENTS | AFTER_QUERY_COMPONENTS, UriFormat.UriEscaped), UriKind.Absolute);
                        }
                        catch
                        {
                            result = uri;
                            return false;
                        }
                    return true;
                }

                query = query.AsRelativeUriString();
                target = uri;
                if ((index = query.IndexOf('#')) > -1 && !uri.TrySetFragmentComponent(query.Substring(index + 1), out target))
                {
                    result = uri;
                    return false;
                }
                query = "?" + query.Replace("?", Uri.HexEscape('?'));
                if (uri.Query != query)
                    try
                    {
                        string preceding = uri.GetComponents(BEFORE_QUERY_COMPONENTS, UriFormat.UriEscaped);
                        string following = uri.GetComponents(AFTER_QUERY_COMPONENTS, UriFormat.UriEscaped);
                        result = new Uri($"{preceding}{query}{following}", UriKind.Relative);
                        if (preceding != uri.GetComponents(BEFORE_QUERY_COMPONENTS, UriFormat.UriEscaped) ||
                            following != uri.GetComponents(AFTER_QUERY_COMPONENTS, UriFormat.UriEscaped))
                        {
                            result = uri;
                            return false;
                        }
                    }
                    catch
                    {
                        result = uri;
                        return false;
                    }
                else
                    result = uri;
                return true;
            }

            string originalString = uri.OriginalString;
            int fragmentIndex;
            index = originalString.IndexOfAny(QueryOrFragmentChar);
            if (index < 0)
                index = fragmentIndex = originalString.Length;
            if (originalString[index] == '#')
                fragmentIndex = index;
            else if (index == originalString.Length - 1 || (fragmentIndex = originalString.IndexOf('#', index + 1)) < 0)
                fragmentIndex = originalString.Length;

            if (query is null)
            {
                if (index == fragmentIndex)
                    result = uri;
                else
                    try
                    {
                        if (index == 0)
                            result = new Uri((fragmentIndex < originalString.Length) ? originalString.Substring(fragmentIndex) : "", UriKind.Relative);
                        else if (fragmentIndex < originalString.Length)
                            result = new Uri(originalString.Substring(0, index) + originalString.Substring(fragmentIndex), UriKind.Relative);
                        else
                            result = new Uri(originalString.Substring(0, index), UriKind.Relative);
                    }
                    catch
                    {
                        result = uri;
                        return false;
                    }
                return true;
            }

            query = $"?{query.AsRelativeUriString()}";
            if (query.Contains("#") || fragmentIndex == originalString.Length)
            {
                if (index == 0)
                {
                    if (query.Equals(originalString))
                        result = uri;
                    else
                        try
                        {
                            result = new Uri(query, UriKind.Relative);
                        }
                        catch
                        {
                            result = uri;
                            return false;
                        }
                }
                else
                {
                    if (query.Length == originalString.Length - index && originalString.Substring(index).Equals(query))
                        result = uri;
                    else
                        try
                        {
                            result = new Uri($"{originalString.Substring(0, index)}{query}", UriKind.Relative);
                        }
                        catch
                        {
                            result = uri;
                            return false;
                        }
                }
            }
            else if (index == 0)
            {
                if (index == query.Length && query.Equals(originalString.Substring(0, index)))
                    result = uri;
                else
                    try
                    {
                        result = new Uri($"{query}{originalString.Substring(fragmentIndex)}", UriKind.Relative);
                    }
                    catch
                    {
                        result = uri;
                        return false;
                    }
            }
            else
            {
                int replaceLen = fragmentIndex - index;
                if (replaceLen == query.Length && query.Equals(originalString.Substring(index, replaceLen)))
                    result = uri;
                else
                    try
                    {
                        result = new Uri($"{originalString.Substring(0, index)}{query}{originalString.Substring(fragmentIndex)}", UriKind.Relative);
                    }
                    catch
                    {
                        result = uri;
                        return false;
                    }
            }
            return true;
        }

        public static string GetFragmentComponent(this Uri uri)
        {
            if (uri is null)
                return null;
            if (uri.IsAbsoluteUri)
                return (uri.Fragment.Length == 0) ? null : uri.Fragment.Substring(1);
            string originalString = uri.OriginalString.AsRelativeUriString();
            int index = originalString.IndexOf('#');
            return (index < 0) ? null : originalString.Substring(index + 1);
        }

        public static bool TrySetFragmentComponent(this Uri uri, string fragment, out Uri result)
        {
            if (uri is null)
            {
                result = uri;
                return false;
            }

            if (uri.IsAbsoluteUri)
            {
                if (fragment is null)
                {
                    if (uri.Fragment.Length == 0)
                        result = uri;
                    else
                        try
                        {
                            result = new Uri(uri.GetComponents(BEFORE_FRAGMENT_COMPONENTS, UriFormat.UriEscaped), UriKind.Absolute);
                        }
                        catch
                        {
                            result = uri;
                            return false;
                        }
                    return true;
                }

                fragment = fragment.AsRelativeUriString();
                fragment = $"#{fragment}";
                if (uri.Fragment != fragment)
                    try
                    {
                        string preceding = uri.GetComponents(BEFORE_FRAGMENT_COMPONENTS, UriFormat.UriEscaped);
                        result = new Uri($"{preceding}{fragment}", UriKind.Relative);
                        if (preceding != uri.GetComponents(BEFORE_QUERY_COMPONENTS, UriFormat.UriEscaped))
                        {
                            result = uri;
                            return false;
                        }
                    }
                    catch
                    {
                        result = uri;
                        return false;
                    }
                else
                    result = uri;
                return true;
            }


            string originalString = uri.OriginalString;
            int index = originalString.IndexOf('#');
            if (fragment is null)
            {
                if (index < 0)
                    result = uri;
                else
                    try
                    {
                        result = new Uri(originalString.Substring(index + 1), UriKind.Relative);
                    }
                    catch
                    {
                        result = uri;
                        return false;
                    }
                return true;
            }
            if (index < 0)
                try
                {
                    result = new Uri($"{originalString}#{fragment}", UriKind.Relative);
                }
                catch
                {
                    result = uri;
                    return false;
                }
            else if (index == 0)
            {
                if (originalString.Equals(fragment))
                    result = uri;
                else
                    try
                    {
                        result = new Uri($"#{fragment}", UriKind.Relative);
                    }
                    catch
                    {
                        result = uri;
                        return false;
                    }
            }
            else
            {
                if (fragment.Length == originalString.Length - index && fragment.Equals(originalString.Substring(index + 1)))
                    result = uri;
                else
                    try
                    {
                        result = new Uri($"{originalString.Substring(0, index)}#{fragment}", UriKind.Relative);
                    }
                    catch
                    {
                        result = uri;
                        return false;
                    }
            }
            return true;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace FsInfoCat.Util
{
    public static class UriHelper
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

        public static readonly Regex PathSegmentPattern = new Regex(@"(?:^|\G)(?:/|([^/]+)(?:/|$))", RegexOptions.Compiled);

        private static readonly char[] QueryOrFragmentChar = new char[] { '?', '#' };
        private static readonly char[] InvalidPathChars;

        static UriHelper()
        {
            char[] invalidPathChars = System.IO.Path.GetInvalidPathChars();
            char[] notIncluded = QueryOrFragmentChar.Where(c => !invalidPathChars.Contains(c)).ToArray();
            InvalidPathChars = (notIncluded.Length == 0) ? invalidPathChars : notIncluded.Concat(invalidPathChars).ToArray();
        }

        /// <summary>
        /// Normalizes a URL so it is compatible for searches and comparison.
        /// </summary>
        /// <remarks><seealso cref="Uri.Host"/> name and <seealso cref="Uri.Scheme"/> is converted to lower case. Default <seealso cref="Uri.Port"/> numbers
        /// and empty <seealso cref="Uri.Query"/> strings and <seealso cref="Uri.Fragment"/>s are removed. If the <seealso cref="Uri.OriginalString"/> is not a well-formed URI,
        /// a new <seealso cref="Uri"/> is returned where the <seealso cref="Uri.OriginalString"/> property is well-formed.
        /// Both absolute and relative URIs are supported by this extension method.</remarks>
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
                        uri.TrySetHostComponent(uri.Host.ToLower(), null, out uri);
                    uri.TrySetHostComponent(uri.Host.ToLower(), uri.Port, out uri);
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
                return new Uri(Uri.EscapeUriString(uri.OriginalString.Replace('\\', '/')), UriKind.Absolute);
            }

            string s = uri.GetFragmentComponent();
            if (null != s && s.Length == 0)
                uri.TrySetFragmentComponent(null, out uri);
            s = uri.GetQueryComponent();
            if (null != s && s.Length == 0)
                uri.TrySetQueryComponent(null, out uri);
            if ((originalString = uri.OriginalString).Length == 0 || Uri.IsWellFormedUriString(originalString, UriKind.Relative))
                return uri;
            if (originalString.Contains('\\') && Uri.IsWellFormedUriString(originalString = originalString.Replace('\\', '/'), UriKind.Relative))
                return new Uri(originalString);
            originalString = Uri.EscapeUriString(originalString);
            if (Uri.IsWellFormedUriString(originalString, UriKind.Relative))
                return new Uri(originalString, UriKind.Relative);
            return new Uri(Uri.EscapeDataString(uri.OriginalString), UriKind.Relative);
        }

        /// <summary>
        /// Splits the <seealso cref="Uri.Query"/> strings and <seealso cref="Uri.Fragment"/>, returning a new <seealso cref="Uri"/> without these components.
        /// </summary>
        /// <param name="uri">The <seealso cref="Uri"/> to split</param>
        /// <param name="query">The query string from the original <paramref name="uri"/> or <c>null</c> if it contained no query.
        /// This will not include the leading separator character (<c>?</c>) of the query string.</param>
        /// <param name="fragment">The fragment string from the original <paramref name="uri"/> or <c>null</c> if it contained no fragment.
        /// This will not include the leading separator character (<c>#</c>) of the fragment.</param>
        /// <returns>A <seealso cref="Uri"/> without the <seealso cref="Uri.Query"/> or <seealso cref="Uri.Fragment"/> components.</returns>
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

        /// <summary>
        /// Test whether a <seealso cref="Uri"/> is equal to another <seealso cref="Uri"/> with authority component comparison being case-insensitive.
        /// </summary>
        /// <param name="uri">The target <seealso cref="Uri"/> of the comparison.</param>
        /// <param name="other">The <seealso cref="Uri"/> to compare to.</param>
        /// <returns><c>true</c> if the authority components both <seealso cref="Uri"/>s are equivalent; otherwise, <c>false</c>.</returns>
        /// <remarks>The <seealso cref="UriComponents.Scheme"/>, <seealso cref="UriComponents.Scheme"/> and <seealso cref="UriComponents.Scheme"/> components
        /// are tested using the <seealso cref="StringComparison.InvariantCultureIgnoreCase"/> comparison option. All other components will use exact comparison.</remarks>
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

        /// <summary>
        /// Returns a well-formed relative URI string from the current string value.
        /// </summary>
        /// <param name="value">The string value to be converted to a well-formed relative URI string.</param>
        /// <returns>A string value containing a well-formed relative URI string. Empty and <c>null</c> strings will be returned as-is.</returns>
        /// <remarks>This will do &quot;smart&quot; URI escaping, whereby <seealso cref="Uri.EscapeUriString"/> will not be applied to the string if it already
        /// represents a well-formed URI string.</remarks>
        public static string AsRelativeUriString(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;
            if (!Uri.IsWellFormedUriString(value, UriKind.RelativeOrAbsolute))
            {
                int n = value.IndexOf('\\');
                if (n < 0)
                    value = Uri.EscapeUriString(value);
                else
                {
                    int i = value.IndexOfAny(QueryOrFragmentChar);
                    if (i < 0)
                    {
                        if (!Uri.IsWellFormedUriString(value = value.Replace('\\', '/'), UriKind.RelativeOrAbsolute))
                            value = Uri.EscapeUriString(value);
                    }
                    else if (n > i || !Uri.IsWellFormedUriString(value = value.Substring(0, i).Replace('\\', '/') + value.Substring(i), UriKind.RelativeOrAbsolute))
                        value = Uri.EscapeUriString(value);
                }
            }
            if (Uri.IsWellFormedUriString(value, UriKind.Absolute))
                return (new Uri(value, UriKind.Absolute).GetComponents(UriComponents.PathAndQuery | UriComponents.Fragment | UriComponents.KeepDelimiter, UriFormat.UriEscaped));
            return value;
        }

        /// <summary>
        /// Ensures a string value is escaped for inclusion in a URI string as the username portion of the <seealso cref="UriComponents.UserInfo"/> component.
        /// </summary>
        /// <param name="value">The user name to escape.</param>
        /// <returns>A string value that is propertly escaped for inclusion in a URI string as the username portion of the <seealso cref="UriComponents.UserInfo"/> component.</returns>
        public static string AsUserNameComponentEncoded(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return (Uri.IsWellFormedUriString(value, UriKind.Relative) ? value : Uri.EscapeUriString(value))
                .Replace("/", "%2F").Replace(":", "%3A").Replace("?", "%3F").Replace("#", "%23").Replace("@", "%40");
        }

        /// <summary>
        /// Ensures a string value is escaped for inclusion in a URI string as the password portion of the <seealso cref="UriComponents.UserInfo"/> component.
        /// </summary>
        /// <param name="value">The password to escape.</param>
        /// <returns>A string value that is propertly escaped for inclusion in a URI string as the password portion of the <seealso cref="UriComponents.UserInfo"/> component.</returns>
        public static string AsPasswordComponentEncoded(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return (Uri.IsWellFormedUriString(value, UriKind.Relative) ? value : Uri.EscapeUriString(value))
                .Replace("/", "%2F").Replace("?", "%3F").Replace("#", "%23").Replace("@", "%40");
        }

        /// <summary>
        /// Gets the username and password from the <seealso cref="UriComponents.UserInfo"/> component of a <seealso cref="Uri"/>.
        /// </summary>
        /// <param name="uri">The source <seealso cref="Uri"/>.</param>
        /// <param name="password">The password portion from the <seealso cref="UriComponents.UserInfo"/> component of the source <paramref name="uri"/> or <c>null</c> if it contained no password.</param>
        /// <returns>The user name portion from the <seealso cref="UriComponents.UserInfo"/> component of the source <paramref name="uri"/> or <c>null</c> if it contained no <seealso cref="UriComponents.UserInfo"/> component.</returns>
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

        /// <summary>
        /// Attempts to create a variant of a <seealso cref="Uri"/> with the specified <seealso cref="UriComponents.UserInfo"/> components applied.
        /// </summary>
        /// <param name="uri">The original <seealso cref="Uri"/>.</param>
        /// <param name="userName">The username portion of the <seealso cref="UriComponents.UserInfo"/> component for the new <seealso cref="Uri"/>.
        /// This can be <c>null</c> if you wish to create a <seealso cref="Uri"/> with the <seealso cref="UriComponents.UserInfo"/> omitted.</param>
        /// <param name="password">The password portion of the <seealso cref="UriComponents.UserInfo"/> component for the new <seealso cref="Uri"/>.
        /// This can be <c>null</c> if you wish to create a <seealso cref="Uri"/> with only the username portion of the <seealso cref="UriComponents.UserInfo"/>.</param>
        /// <param name="result">The new <seealso cref="Uri"/> with the specified <seealso cref="UriComponents.UserInfo"/> components applied.</param>
        /// <returns><c>true</c> if the <paramref name="result"/> <seealso cref="Uri"/> was successfully created; otherwise, <c>false</c>.</returns>
        /// <remarks>This can return <c>false</c> if the source <paramref name="uri"/> is <c>null</c>, if it not an absolute URI,
        /// and in cases where the <seealso cref="Uri.Scheme"/> does not support the <seealso cref="UriComponents.UserInfo"/> component.</remarks>
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

        /// <summary>
        /// Attempts to create a variant of a <seealso cref="Uri"/> with the specified <seealso cref="UriComponents.HostAndPort"/> components applied.
        /// </summary>
        /// <param name="uri">The original <seealso cref="Uri"/>.</param>
        /// <param name="hostName">The new <seealso cref="UriComponents.Host"/> component or <c>null</c> to create a <seealso cref="Uri"/> without this component.</param>
        /// <param name="port">The new <seealso cref="UriComponents.Port"/> component or <c>null</c> to create a <seealso cref="Uri"/> without this component.</param>
        /// <param name="result">The new <seealso cref="Uri"/> with the specified <seealso cref="UriComponents.HostAndPort"/> components applied.</param>
        /// <remarks>This can return <c>false</c> if the source <paramref name="uri"/> is <c>null</c>, if it not an absolute URI,
        /// and in cases where the <seealso cref="Uri.Scheme"/> does not support the inclusion or omission of the <seealso cref="UriComponents.HostAndPort"/> components.</remarks>
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

        /// <summary>
        /// Gets the <seealso cref="UriComponents.Path"/> component of a <seealso cref="Uri"/>.
        /// </summary>
        /// <param name="uri">The source <seealso cref="Uri"/>.</param>
        /// <returns>The <seealso cref="UriComponents.Path"/> component of the source <paramref name="uri"/>.
        /// This will return <c>null</c> if the source <paramref name="uri"/> was <c>null</c>.</returns>
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

        /// <summary>
        /// Attempts to create a variant of a <seealso cref="Uri"/> with the specified <seealso cref="UriComponents.Path"/> component applied.
        /// </summary>
        /// <param name="uri">The original <seealso cref="Uri"/>.</param>
        /// <param name="path">The new <seealso cref="UriComponents.Path"/> component. This can be <c>null</c> to create a <seealso cref="Uri"/> without this component.</param>
        /// <param name="result">The new <seealso cref="Uri"/> with the specified <seealso cref="UriComponents.Path"/> component applied.</param>
        /// <returns><c>true</c> if the new <paramref name="result"/> <seealso cref="Uri"/> was successfully created; otherwise, <c>false</c>.</returns>
        /// <remarks>This can return <c>false</c> in circumstances where the change would create an invalid URI string as it pertains to the current <seealso cref="Uri.Scheme"/>.
        /// If the specified <paramref name="path"/> contains a query (<c>?</c>) and/or fragment (<c>#</c>) separator, those components will be applied
        /// to the <paramref name="result"/> <seealso cref="Uri"/> as well.</remarks>
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
                        result = new Uri($"{preceding}{path}{following}", UriKind.Absolute);
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

        /// <summary>
        /// Gets the <seealso cref="UriComponents.Query"/> component of a <seealso cref="Uri"/>.
        /// </summary>
        /// <param name="uri">The source <seealso cref="Uri"/>.</param>
        /// <returns>The <seealso cref="UriComponents.Query"/> component of the source <paramref name="uri"/> or <c>null</c> if it contained no query component.
        /// This will also return <c>null</c> if the source <paramref name="uri"/> was <c>null</c>.</returns>
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

        /// <summary>
        /// Attempts to create a variant of a <seealso cref="Uri"/> with the specified <seealso cref="UriComponents.Query"/> component applied.
        /// </summary>
        /// <param name="uri">The original <seealso cref="Uri"/>.</param>
        /// <param name="query">The new <seealso cref="UriComponents.Query"/> component. This can be <c>null</c> to create a <seealso cref="Uri"/> without this component.</param>
        /// <param name="result">The new <seealso cref="Uri"/> with the specified <seealso cref="UriComponents.Query"/> component applied.</param>
        /// <returns><c>true</c> if the new <paramref name="result"/> <seealso cref="Uri"/> was successfully created; otherwise, <c>false</c>.</returns>
        /// <remarks>This can return <c>false</c> in circumstances where the change would create an invalid URI string as it pertains to the current <seealso cref="Uri.Scheme"/>.
        /// If the specified <paramref name="query"/> contains a fragment (<c>#</c>) separator, that component will be applied to the <paramref name="result"/>
        /// <seealso cref="Uri"/> as well.</remarks>
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

        /// <summary>
        /// Gets the <seealso cref="UriComponents.Fragment"/> component of a <seealso cref="Uri"/>.
        /// </summary>
        /// <param name="uri">The source <seealso cref="Uri"/>.</param>
        /// <returns>The <seealso cref="UriComponents.Fragment"/> component of the source <paramref name="uri"/> or <c>null</c> if it contained no fragment component.
        /// This will also return <c>null</c> if the source <paramref name="uri"/> was <c>null</c>.</returns>
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

        /// <summary>
        /// Attempts to create a variant of a <seealso cref="Uri"/> with the specified <seealso cref="UriComponents.Fragment"/> component applied.
        /// </summary>
        /// <param name="uri">The original <seealso cref="Uri"/>.</param>
        /// <param name="fragment">The new <seealso cref="UriComponents.Fragment"/> component. This can be <c>null</c> to create a <seealso cref="Uri"/> without this component.</param>
        /// <param name="result">The new <seealso cref="Uri"/> with the specified <seealso cref="UriComponents.Fragment"/> component applied.</param>
        /// <returns><c>true</c> if the new <paramref name="result"/> <seealso cref="Uri"/> was successfully created; otherwise, <c>false</c>.</returns>
        /// <remarks>This can return <c>false</c> in circumstances where the change would create an invalid URI string as it pertains to the current <seealso cref="Uri.Scheme"/>.</remarks>
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

        /// <summary>
        /// Gets the path segments of a relative or absolute <seealso cref="Uri"/>;
        /// </summary>
        /// <param name="uri">The target <seealso cref="Uri"/>.</param>
        /// <returns>A System.String array that contains the path segments that make up the specified <seealso cref="Uri"/>.</returns>
        public static string[] GetPathSegments(this Uri uri)
        {
            if (uri is null)
                return new string[0];
            if (uri.IsAbsoluteUri)
                return uri.Segments;
            string u = uri.OriginalString;
            if (u.Length == 0)
                return new string[] { u };
            if (!(Uri.IsWellFormedUriString(u, UriKind.Relative) || (u.Contains('\\') && Uri.IsWellFormedUriString(u = u.Replace('\\', '/'), UriKind.Relative))))
            {
                string s = Uri.EscapeUriString(u);
                u = (Uri.IsWellFormedUriString(s, UriKind.Relative)) ? s : Uri.EscapeDataString(u);
            }
            return PathSegmentPattern.Matches(u).Cast<Match>().Select(m => m.Value).ToArray();
        }

        /// <summary>
        /// Adds or removes a trailing slash (empty segment) to the <seealso cref="UriComponents.Path"/> component of a <seealso cref="Uri"/>.
        /// </summary>
        /// <param name="uri">The target <seealso cref="Uri"/>.</param>
        /// <param name="shouldHaveTrailingSlash"><c>true</c> if the <seealso cref="UriComponents.Path"/> component of the <paramref name="result"/>
        /// <seealso cref="Uri"/> should have a trailing slash; otherwise <c>false</c> if the trailing slash should be omitted.</param>
        /// <param name="result">A <seealso cref="Uri"/> with the trailing slash included or omitted.</param>
        /// <returns><c>true</c> if the trailing slash was successfully included or omitted; otherwise, <c>false</c>.</returns>
        /// <remarks>This can return <c>false</c> in cases where the change would create an invalid URI as per the current <seealso cref="Uri.Scheme"/>
        /// or if the <seealso cref="Uri.Scheme"/> automatically appends a trailing slash with at it's root path.</remarks>
        public static bool TrySetTrailingEmptyPathSegment(this Uri uri, bool shouldHaveTrailingSlash, out Uri result)
        {
            string path = uri.GetPathComponent();

            if (string.IsNullOrEmpty(path))
            {
                if (shouldHaveTrailingSlash)
                    return uri.TrySetPathComponent("/", out result) && uri.GetPathComponent().EndsWith('/');
            }
            else if (path.EndsWith('/') != shouldHaveTrailingSlash)
            {
                if (shouldHaveTrailingSlash)
                    return uri.TrySetPathComponent($"{path}/", out result) && uri.GetPathComponent().EndsWith('/');
                int i = path.Length - 1;
                while (i > 0 && path[i - 1] == '/')
                    i--;
                return uri.TrySetPathComponent(path.Substring(0, i), out result) && !uri.GetPathComponent().EndsWith('/');
            }
            result = uri;
            return true;
        }
    }
}

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using static FsInfoCat.Util.FileUriConverter;

namespace FsInfoCat.Util
{
    // TODO: Rename to UriFactory and make instance class after extension methods moved to ExtensionMethods
    public static class UriHelper
    {
        public const string URI_SCHEME_URN = "urn";
        public const string URN_NAMESPACE_UUID = "uuid";
        public const string URN_NAMESPACE_VOLUME = "volume";
        public const string URN_NAMESPACE_ID = "id";
        public const char URI_QUERY_DELIMITER_CHAR = '?';
        public const string URI_QUERY_DELIMITER_STRING = "?";
        public const string URI_QUERY_DELIMITER_ESCAPED = "%3F";
        public const char URI_FRAGMENT_DELIMITER_CHAR = '#';
        public const string URI_FRAGMENT_DELIMITER_STRING = "#";
        public const string URI_FRAGMENT_DELIMITER_ESCAPED = "%23";
        public const char URI_SCHEME_SEPARATOR_CHAR = ':';
        public const string URI_SCHEME_SEPARATOR_STRING = ":";
        public const string URI_SCHEME_DELIMITER_ESCAPED = "%3A";
        public const char URI_USERINFO_SEPARATOR_CHAR = '@';
        public const string URI_USERINFO_SEPARATOR_STRING = "@";
        public const string URI_USERINFO_DELIMITER_ESCAPED = "%40";
        public const char URI_PATH_SEPARATOR_CHAR = '/';
        public const string URI_PATH_SEPARATOR_STRING = "/";
        public const string URI_PATH_SEPARATOR_ESCAPED = "%2F";
        private static readonly char[] _QUERY_OR_FRAGMENT_DELIMITER = new char[] { URI_QUERY_DELIMITER_CHAR, URI_FRAGMENT_DELIMITER_CHAR };

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

        // TODO: Move  to ExtensionMethods
        /// <summary>
        /// Normalizes a URL so it is compatible for searches and comparison.
        /// </summary>
        /// <remarks><seealso cref="Uri.Host"/> name and <seealso cref="Uri.Scheme"/> is converted to lower case. Default <seealso cref="Uri.Port"/> numbers
        /// and empty <seealso cref="Uri.Query"/> strings and <seealso cref="Uri.Fragment"/>s are removed. If the <seealso cref="Uri.OriginalString"/>
        /// is not a well-formed URI, a new <seealso cref="Uri"/> is returned where the <seealso cref="Uri.OriginalString"/> property is well-formed.
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
                    UriBuilder uriBuilder = new UriBuilder(uri)
                    {
                        Scheme = uri.Scheme.ToLower(),
                        Host = uri.Host.ToLower()
                    };
                    if (uri.Query == URI_QUERY_DELIMITER_STRING)
                        uriBuilder.Query = "";
                    if (uri.Fragment == URI_FRAGMENT_DELIMITER_STRING)
                        uriBuilder.Fragment = "";
                    uri = uriBuilder.Uri;
                }
                else if (!uri.Host.ToLower().Equals(uri.Host))
                {
                    if (uri.IsDefaultPort)
                        uri.TrySetHostComponent(uri.Host.ToLower(), null, out uri);
                    uri.TrySetHostComponent(uri.Host.ToLower(), uri.Port, out uri);
                }
                if (uri.Query == URI_QUERY_DELIMITER_STRING)
                    uri.TrySetQueryComponent(null, out uri);
                if (uri.Fragment == URI_FRAGMENT_DELIMITER_STRING)
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
            try { return new Uri(Uri.EscapeUriString(originalString), UriKind.Relative); }
            catch { return new Uri(Uri.EscapeDataString(originalString), UriKind.Relative); }
        }

        public static string EnsureWellFormedUriString(string uriString, UriKind kind)
        {
            if (string.IsNullOrEmpty(uriString))
            {
                if (kind == UriKind.Absolute)
                {
                    if (uriString is null)
                        throw new ArgumentNullException(nameof(uriString));
                    throw new ArgumentOutOfRangeException(nameof(uriString));
                }
                return "";
            }
            if (Uri.IsWellFormedUriString(uriString, kind))
                return uriString;
            string e = Uri.EscapeUriString(uriString);
            if (Uri.IsWellFormedUriString(e, kind))
                return e;
            if (kind == UriKind.Absolute)
                throw new ArgumentOutOfRangeException(nameof(uriString));
            return Uri.EscapeDataString(uriString);
        }

        /// <summary>
        /// Ensures a string value is escaped for inclusion in a URI string as the username portion of the <seealso cref="UriComponents.UserInfo"/> component.
        /// </summary>
        /// <param name="value">The user name to escape.</param>
        /// <returns>A string value that is propertly escaped for inclusion in a URI string as the username portion of the <seealso cref="UriComponents.UserInfo"/>
        /// component.</returns>
        public static string AsUserNameComponentEncoded(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return (Uri.IsWellFormedUriString(value, UriKind.Relative) ? value : Uri.EscapeUriString(value))
                .Replace(URI_PATH_SEPARATOR_STRING, URI_PATH_SEPARATOR_ESCAPED)
                .Replace(URI_SCHEME_SEPARATOR_STRING, URI_SCHEME_DELIMITER_ESCAPED)
                .Replace(URI_QUERY_DELIMITER_STRING, URI_QUERY_DELIMITER_ESCAPED)
                .Replace(URI_FRAGMENT_DELIMITER_STRING, URI_FRAGMENT_DELIMITER_ESCAPED)
                .Replace(URI_USERINFO_SEPARATOR_STRING, URI_USERINFO_DELIMITER_ESCAPED);
        }

        /// <summary>
        /// Ensures a string value is escaped for inclusion in a URI string as the password portion of the <seealso cref="UriComponents.UserInfo"/> component.
        /// </summary>
        /// <param name="value">The password to escape.</param>
        /// <returns>A string value that is propertly escaped for inclusion in a URI string as the password portion of the <seealso cref="UriComponents.UserInfo"/>
        /// component.</returns>
        public static string AsPasswordComponentEncoded(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return (Uri.IsWellFormedUriString(value, UriKind.Relative) ? value : Uri.EscapeUriString(value))
                .Replace(URI_PATH_SEPARATOR_STRING, URI_PATH_SEPARATOR_ESCAPED)
                .Replace(URI_QUERY_DELIMITER_STRING, URI_QUERY_DELIMITER_ESCAPED)
                .Replace(URI_FRAGMENT_DELIMITER_STRING, URI_FRAGMENT_DELIMITER_ESCAPED)
                .Replace(URI_USERINFO_SEPARATOR_STRING, URI_USERINFO_DELIMITER_ESCAPED);
        }

        public static string SplitQueryAndFragment(string uriString, bool keepDelimiters, out string query, out string fragment)
        {
            int index;
            if (string.IsNullOrEmpty(uriString) || (index = uriString.IndexOfAny(_QUERY_OR_FRAGMENT_DELIMITER)) < 0)
            {
                query = fragment = (keepDelimiters) ? "" : null;
                return uriString;
            }

            if (uriString[index] == URI_FRAGMENT_DELIMITER_CHAR)
            {
                if (keepDelimiters)
                {
                    query = "";
                    fragment = uriString.Substring(index);
                }
                else
                {
                    query = null;
                    fragment = uriString.Substring(index + 1);
                }
            }
            else if (keepDelimiters)
            {
                query = uriString.Substring(index);
                index = query.IndexOf(URI_FRAGMENT_DELIMITER_CHAR);
                if (index < 0)
                    fragment = "";
                else
                {
                    fragment = query.Substring(index);
                    query = query.Substring(0, index);
                }
            }
            else
            {
                query = uriString.Substring(index + 1);
                index = query.IndexOf(URI_FRAGMENT_DELIMITER_CHAR);
                if (index < 0)
                    fragment = null;
                else
                {
                    fragment = query.Substring(index + 1);
                    query = query.Substring(0, index);
                }
            }
            return uriString.Substring(0, index);
        }

        public static string SplitQueryAndFragment(string uriString, out string query, out string fragment) =>
            SplitQueryAndFragment(uriString, false, out query, out fragment);

        public static string SplitQueryAndFragment(string uriString, bool keepDelimiters, out string queryAndFragment)
        {
            int index;
            if (string.IsNullOrEmpty(uriString) || (index = uriString.IndexOfAny(_QUERY_OR_FRAGMENT_DELIMITER)) < 0)
            {
                queryAndFragment = (keepDelimiters) ? "" : null;
                return uriString;
            }

            queryAndFragment = uriString.Substring((keepDelimiters) ? index : index + 1);
            return uriString.Substring(0, index);
        }

        public static string SplitQueryAndFragment(string uriString, out string queryAndFragment) =>
            SplitQueryAndFragment(uriString, false, out queryAndFragment);

        // TODO: Move  to ExtensionMethods
        /// <summary>
        /// Splits the <seealso cref="Uri.Query"/> strings and <seealso cref="Uri.Fragment"/>, returning a new <seealso cref="Uri"/> without these components.
        /// </summary>
        /// <param name="uri">The <seealso cref="Uri"/> to split</param>
        /// <param name="query">The query string from the original <paramref name="uri"/> or <c>null</c> if it contained no query.
        /// This will not include the leading separator character (<c>?</c>) of the query string.</param>
        /// <param name="fragment">The fragment string from the original <paramref name="uri"/> or <c>null</c> if it contained no fragment.
        /// This will not include the leading separator character (<c>#</c>) of the fragment.</param>
        /// <returns>A <seealso cref="Uri"/> without the <seealso cref="Uri.Query"/> or <seealso cref="Uri.Fragment"/> components.</returns>
        public static Uri SplitQueryAndFragment(this Uri uri, bool keepDelimiters, out string query, out string fragment)
        {
            if (uri is null)
            {
                query = fragment = null;
                return null;
            }
            if (uri.IsAbsoluteUri)
            {
                if (keepDelimiters)
                {
                    fragment = uri.Fragment ?? "";
                    query = uri.Query ?? "";
                    if (query.Length == 0 && fragment.Length == 0)
                        return uri;
                }
                else
                {
                    fragment = (uri.Fragment.Length > 0) ? uri.Fragment.Substring(1) : null;
                    query = (uri.Fragment.Length > 0) ? uri.Fragment.Substring(1) : null;
                    if (null == query && null == fragment)
                        return uri;
                }
                UriBuilder uriBuilder = new UriBuilder(uri);
                uriBuilder.Query = uriBuilder.Fragment = "";
                return uriBuilder.Uri;
            }

            return new Uri(SplitQueryAndFragment(uri.OriginalString, false, out query, out fragment), UriKind.Relative);
        }

        // TODO: Move  to ExtensionMethods
        public static Uri SplitQueryAndFragment(this Uri uri, out string query, out string fragment) =>
            SplitQueryAndFragment(uri, false, out query, out fragment);

        public static string SplitFragment(string uriString, bool keepDelimiter, out string fragment)
        {
            int index;
            if (string.IsNullOrEmpty(uriString) || (index = uriString.IndexOf(URI_FRAGMENT_DELIMITER_CHAR)) < 0)
            {
                fragment = (keepDelimiter) ? "" : null;
                return uriString;
            }

            fragment = uriString.Substring((keepDelimiter) ? index : index + 1);
            return uriString.Substring(0, index);
        }

        public static string SplitFragment(string uriString, out string fragment) =>
            SplitFragment(uriString, false, out fragment);
        
        // TODO: Move  to ExtensionMethods
        public static string SplitUriComponents(this Uri uri, bool keepQueryAndFragmentDelimiters, UriFormat format, out string schemeAndAuthority,
            out string queryAndFragment)
        {
            if (uri is null)
            {
                schemeAndAuthority = queryAndFragment = null;
                return null;
            }

            if (uri.IsAbsoluteUri)
            {
                schemeAndAuthority = uri.GetComponents(UriComponents.Scheme | UriComponents.UserInfo | UriComponents.Host | UriComponents.Port | UriComponents.KeepDelimiter, format);
                queryAndFragment = uri.GetComponents(UriComponents.Fragment | UriComponents.Query | UriComponents.KeepDelimiter, format);
                if (keepQueryAndFragmentDelimiters)
                {
                    if (queryAndFragment is null)
                        queryAndFragment = "";
                }
                else if (string.IsNullOrEmpty(queryAndFragment))
                    queryAndFragment = null;
                else
                    if (queryAndFragment[0] == URI_QUERY_DELIMITER_CHAR || queryAndFragment[0] == URI_FRAGMENT_DELIMITER_CHAR)
                    queryAndFragment = queryAndFragment.Substring(1);
                return uri.GetComponents(UriComponents.Path | UriComponents.KeepDelimiter, format);
            }
            schemeAndAuthority = "";
            string originalString = uri.OriginalString;
            if (originalString.Length == 0)
            {
                queryAndFragment = (keepQueryAndFragmentDelimiters) ? "" : null;
                return "";
            }

            bool wellFormatted = Uri.IsWellFormedUriString(originalString, UriKind.Relative);
            if (format == UriFormat.UriEscaped && !wellFormatted)
            {
                wellFormatted = true;
                originalString = EnsureWellFormedUriString(originalString, UriKind.Relative);
            }

            originalString = SplitQueryAndFragment(originalString, keepQueryAndFragmentDelimiters, out queryAndFragment);
            if (string.IsNullOrEmpty(queryAndFragment))
                return (wellFormatted && format != UriFormat.UriEscaped) ? Uri.UnescapeDataString(originalString) : originalString;
            if (wellFormatted && format != UriFormat.UriEscaped)
            {
                queryAndFragment = Uri.UnescapeDataString(queryAndFragment);
                return Uri.UnescapeDataString(originalString);
            }
            return originalString;
        }

        // TODO: Move  to ExtensionMethods
        public static string SplitUriComponents(this Uri uri, UriFormat format, out string schemeAndAuthority, out string queryAndFragment) =>
            SplitUriComponents(uri, false, format, out schemeAndAuthority, out queryAndFragment);

        // TODO: Move  to ExtensionMethods
        public static string SplitUriComponents(this Uri uri, bool keepQueryAndFragmentDelimiters, out string schemeAndAuthority, out string queryAndFragment) =>
            SplitUriComponents(uri, keepQueryAndFragmentDelimiters, UriFormat.UriEscaped, out schemeAndAuthority, out queryAndFragment);

        // TODO: Move  to ExtensionMethods
        public static string SplitUriComponents(this Uri uri, out string schemeAndAuthority, out string queryAndFragment) =>
            SplitUriComponents(uri, false, out schemeAndAuthority, out queryAndFragment);

        // TODO: Move  to ExtensionMethods
        public static string SplitUriComponents(this Uri uri, bool keepQueryAndFragmentDelimiters, UriFormat format, out string schemeAndAuthority, out string query,
            out string fragment)
        {
            if (uri is null)
            {
                schemeAndAuthority = query = fragment = null;
                return null;
            }

            if (uri.IsAbsoluteUri)
            {
                schemeAndAuthority = uri.GetComponents(UriComponents.Scheme | UriComponents.UserInfo | UriComponents.Host | UriComponents.Port |
                    UriComponents.KeepDelimiter, format);
                query = uri.GetComponents(UriComponents.Query | UriComponents.KeepDelimiter, format);
                fragment = uri.GetComponents(UriComponents.Fragment | UriComponents.KeepDelimiter, format);
                if (!keepQueryAndFragmentDelimiters)
                {
                    query = (string.IsNullOrEmpty(query)) ? null : query.Substring(1);
                    fragment = (string.IsNullOrEmpty(fragment)) ? null : fragment.Substring(1);
                }
                return uri.GetComponents(UriComponents.Path | UriComponents.KeepDelimiter, format);
            }
            schemeAndAuthority = "";
            string originalString = uri.OriginalString;
            if (originalString.Length == 0)
            {
                query = fragment = (keepQueryAndFragmentDelimiters) ? "" : null;
                return "";
            }
            bool wellFormatted = Uri.IsWellFormedUriString(originalString, UriKind.Relative);
            if (format == UriFormat.UriEscaped && !wellFormatted)
            {
                wellFormatted = true;
                originalString = EnsureWellFormedUriString(originalString, UriKind.Relative);
            }

            originalString = SplitQueryAndFragment(originalString, keepQueryAndFragmentDelimiters, out query, out fragment);

            if (string.IsNullOrEmpty(query))
            {
                if (string.IsNullOrEmpty(fragment))
                    return (wellFormatted && format != UriFormat.UriEscaped) ? Uri.UnescapeDataString(originalString) : originalString;
                if (wellFormatted && format != UriFormat.UriEscaped)
                {
                    fragment = Uri.UnescapeDataString(fragment);
                    return Uri.UnescapeDataString(originalString);
                }
            }
            else if (wellFormatted && format != UriFormat.UriEscaped)
            {
                query = Uri.UnescapeDataString(query);
                if (string.IsNullOrEmpty(fragment))
                    fragment = Uri.UnescapeDataString(fragment);
                return Uri.UnescapeDataString(originalString);
            }
            return originalString;
        }

        // TODO: Move  to ExtensionMethods
        public static string SplitUriComponents(this Uri uri, UriFormat format, out string schemeAndAuthority, out string query, out string fragment) =>
            SplitUriComponents(uri, false, format, out schemeAndAuthority, out query, out fragment);

        // TODO: Move  to ExtensionMethods
        public static string SplitUriComponents(this Uri uri, bool keepQueryAndFragmentDelimiters, out string schemeAndAuthority, out string query,
            out string fragment) => SplitUriComponents(uri, keepQueryAndFragmentDelimiters, UriFormat.UriEscaped, out schemeAndAuthority, out query, out fragment);

        // TODO: Move  to ExtensionMethods
        public static string SplitUriComponents(this Uri uri, out string schemeAndAuthority, out string query, out string fragment) =>
            SplitUriComponents(uri, false, out schemeAndAuthority, out query, out fragment);

        // TODO: Move  to ExtensionMethods
        public static string SplitUriComponents(this Uri uri, bool keepQueryAndFragmentDelimiters, UriFormat format, out string scheme, out string authority,
            out string query, out string fragment)
        {
            if (uri is null)
            {
                scheme = authority = query = fragment = null;
                return null;
            }

            if (uri.IsAbsoluteUri)
            {
                scheme = uri.GetComponents(UriComponents.Scheme | UriComponents.KeepDelimiter, format);
                authority = uri.GetComponents(UriComponents.UserInfo | UriComponents.Host | UriComponents.Port | UriComponents.KeepDelimiter, format);
                query = uri.GetComponents(UriComponents.Query | UriComponents.KeepDelimiter, format);
                fragment = uri.GetComponents(UriComponents.Fragment | UriComponents.KeepDelimiter, format);
                if (!keepQueryAndFragmentDelimiters)
                {
                    query = (string.IsNullOrEmpty(query)) ? null : query.Substring(1);
                    fragment = (string.IsNullOrEmpty(fragment)) ? null : fragment.Substring(1);
                }
                return uri.GetComponents(UriComponents.Path | UriComponents.KeepDelimiter, format);
            }
            scheme = authority = "";
            string originalString = uri.OriginalString;
            if (originalString.Length == 0)
            {
                query = fragment = "";
                return "";
            }
            bool wellFormatted = Uri.IsWellFormedUriString(originalString, UriKind.Relative);
            if (format == UriFormat.UriEscaped && !wellFormatted)
            {
                wellFormatted = true;
                originalString = EnsureWellFormedUriString(originalString, UriKind.Relative);
            }

            originalString = SplitQueryAndFragment(originalString, keepQueryAndFragmentDelimiters, out query, out fragment);

            if (string.IsNullOrEmpty(query))
            {
                if (string.IsNullOrEmpty(fragment))
                    return (wellFormatted && format != UriFormat.UriEscaped) ? Uri.UnescapeDataString(originalString) : originalString;
                if (wellFormatted && format != UriFormat.UriEscaped)
                {
                    fragment = Uri.UnescapeDataString(fragment);
                    return Uri.UnescapeDataString(originalString);
                }
            }
            else if (wellFormatted && format != UriFormat.UriEscaped)
            {
                query = Uri.UnescapeDataString(query);
                if (string.IsNullOrEmpty(fragment))
                    fragment = Uri.UnescapeDataString(fragment);
                return Uri.UnescapeDataString(originalString);
            }
            return originalString;
        }

        // TODO: Move  to ExtensionMethods
        public static string SplitUriComponents(this Uri uri, UriFormat format, out string scheme, out string authority, out string query, out string fragment) =>
            SplitUriComponents(uri, false, format, out scheme, out authority, out query, out fragment);

        // TODO: Move  to ExtensionMethods
        public static string SplitUriComponents(this Uri uri, bool keepQueryAndFragmentDelimiters, out string scheme, out string authority, out string query,
                out string fragment) =>
            SplitUriComponents(uri, keepQueryAndFragmentDelimiters, UriFormat.UriEscaped, out scheme, out authority, out query, out fragment);

        // TODO: Move  to ExtensionMethods
        public static string SplitUriComponents(this Uri uri, out string scheme, out string authority, out string query, out string fragment) =>
            SplitUriComponents(uri, false, out scheme, out authority, out query, out fragment);

        // TODO: Move  to ExtensionMethods
        public static string SplitUriComponents(this Uri uri, bool keepQueryAndFragmentDelimiters, UriFormat format, out string scheme, out string userInfo,
            out string hostAndPort, out string query, out string fragment)
        {
            if (uri is null)
            {
                scheme = userInfo = hostAndPort = query = fragment = null;
                return null;
            }

            if (uri.IsAbsoluteUri)
            {
                scheme = uri.GetComponents(UriComponents.Scheme | UriComponents.KeepDelimiter, format);
                userInfo = uri.GetComponents(UriComponents.UserInfo | UriComponents.KeepDelimiter, format);
                hostAndPort = uri.GetComponents(UriComponents.Host | UriComponents.Port | UriComponents.KeepDelimiter, format);
                query = uri.GetComponents(UriComponents.Query | UriComponents.KeepDelimiter, format);
                fragment = uri.GetComponents(UriComponents.Fragment | UriComponents.KeepDelimiter, format);
                if (!keepQueryAndFragmentDelimiters)
                {
                    query = (string.IsNullOrEmpty(query)) ? null : query.Substring(1);
                    fragment = (string.IsNullOrEmpty(fragment)) ? null : fragment.Substring(1);
                }
                return uri.GetComponents(UriComponents.Path | UriComponents.KeepDelimiter, format);
            }
            scheme = userInfo = hostAndPort = "";
            string originalString = uri.OriginalString;
            if (originalString.Length == 0)
            {
                query = fragment = "";
                return "";
            }
            bool wellFormatted = Uri.IsWellFormedUriString(originalString, UriKind.Relative);
            if (format == UriFormat.UriEscaped && !wellFormatted)
            {
                wellFormatted = true;
                originalString = EnsureWellFormedUriString(originalString, UriKind.Relative);
            }

            originalString = SplitQueryAndFragment(originalString, keepQueryAndFragmentDelimiters, out query, out fragment);

            if (string.IsNullOrEmpty(query))
            {
                if (string.IsNullOrEmpty(fragment))
                    return (wellFormatted && format != UriFormat.UriEscaped) ? Uri.UnescapeDataString(originalString) : originalString;
                if (wellFormatted && format != UriFormat.UriEscaped)
                {
                    fragment = Uri.UnescapeDataString(fragment);
                    return Uri.UnescapeDataString(originalString);
                }
            }
            else if (wellFormatted && format != UriFormat.UriEscaped)
            {
                query = Uri.UnescapeDataString(query);
                if (string.IsNullOrEmpty(fragment))
                    fragment = Uri.UnescapeDataString(fragment);
                return Uri.UnescapeDataString(originalString);
            }
            return originalString;
        }

        // TODO: Move  to ExtensionMethods
        public static string SplitUriComponents(this Uri uri, bool keepQueryAndFragmentDelimiters, out string scheme, out string userInfo, out string hostAndPort,
                out string query, out string fragment) =>
            SplitUriComponents(uri, keepQueryAndFragmentDelimiters, UriFormat.UriEscaped, out scheme, out userInfo, out hostAndPort, out query, out fragment);

        // TODO: Move  to ExtensionMethods
        public static string SplitUriComponents(this Uri uri, out string scheme, out string userInfo, out string hostAndPort, out string query, out string fragment) =>
            SplitUriComponents(uri, false, out scheme, out userInfo, out hostAndPort, out query, out fragment);

        // TODO: Move  to ExtensionMethods
        public static string GetGroupValue(this Match match, string groupName, string defaultValue = null)
        {
            if (match.Success)
            {
                Group g = match.Groups[groupName];
                return (g.Success) ? g.Value : defaultValue;
            }
            return defaultValue;
        }

        public static string AsRelativeUriString(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;
            if (Uri.TryCreate(value, UriKind.Absolute, out Uri uri))
                return uri.GetComponents(UriComponents.PathAndQuery | UriComponents.Fragment | UriComponents.KeepDelimiter, UriFormat.UriEscaped);
            return EnsureWellFormedUriString(value, UriKind.Relative);
        }

        // TODO: Move  to ExtensionMethods
        /// <summary>
        /// Test whether a <seealso cref="Uri"/> is equal to another <seealso cref="Uri"/> with authority component comparison being case-insensitive.
        /// </summary>
        /// <param name="uri">The target <seealso cref="Uri"/> of the comparison.</param>
        /// <param name="other">The <seealso cref="Uri"/> to compare to.</param>
        /// <returns><c>true</c> if the authority components both <seealso cref="Uri"/>s are equivalent; otherwise, <c>false</c>.</returns>
        /// <remarks>The <seealso cref="UriComponents.Scheme"/>, <seealso cref="UriComponents.Scheme"/> and <seealso cref="UriComponents.Scheme"/> components
        /// are tested using the <seealso cref="StringComparison.InvariantCultureIgnoreCase"/> comparison option. All other components will use exact
        /// comparison.</remarks>
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

        // TODO: Move  to ExtensionMethods
        /// <summary>
        /// Gets the username and password from the <seealso cref="UriComponents.UserInfo"/> component of a <seealso cref="Uri"/>.
        /// </summary>
        /// <param name="uri">The source <seealso cref="Uri"/>.</param>
        /// <param name="password">The password portion from the <seealso cref="UriComponents.UserInfo"/> component of the source <paramref name="uri"/> or
        /// <c>null</c> if it contained no password.</param>
        /// <returns>The user name portion from the <seealso cref="UriComponents.UserInfo"/> component of the source <paramref name="uri"/> or <c>null</c>
        /// if it contained no <seealso cref="UriComponents.UserInfo"/> component.</returns>
        public static string GetUserNameAndPassword(this Uri uri, out string password)
        {
            if (null != uri && uri.IsAbsoluteUri)
            {
                string userInfo = uri.GetComponents(UriComponents.UserInfo | UriComponents.KeepDelimiter, UriFormat.UriEscaped);
                if (userInfo.Length > 0)
                {
                    string[] upw = uri.GetComponents(UriComponents.UserInfo, UriFormat.UriEscaped).Split(URI_SCHEME_SEPARATOR_CHAR, 2);
                    password = (upw.Length > 1) ? upw[1] : null;
                    return upw[0];
                }
            }
            password = null;
            return null;
        }

        // TODO: Move  to ExtensionMethods
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
                string newUserInfo = AsUserNameComponentEncoded(userName);
                if (null != password && (newUserInfo.Length > 0 || password.Length > 0))
                    newUserInfo = $"{newUserInfo}:{AsPasswordComponentEncoded(password)}";
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

        // TODO: Move  to ExtensionMethods
        /// <summary>
        /// Attempts to create a variant of a <seealso cref="Uri"/> with the specified <seealso cref="UriComponents.HostAndPort"/> components applied.
        /// </summary>
        /// <param name="uri">The original <seealso cref="Uri"/>.</param>
        /// <param name="hostName">The new <seealso cref="UriComponents.Host"/> component or <c>null</c> to create a <seealso cref="Uri"/> without this
        /// component.</param>
        /// <param name="port">The new <seealso cref="UriComponents.Port"/> component or <c>null</c> to create a <seealso cref="Uri"/> without this component.</param>
        /// <param name="result">The new <seealso cref="Uri"/> with the specified <seealso cref="UriComponents.HostAndPort"/> components applied.</param>
        /// <remarks>This can return <c>false</c> if the source <paramref name="uri"/> is <c>null</c>, if it not an absolute URI,
        /// and in cases where the <seealso cref="Uri.Scheme"/> does not support the inclusion or omission of the <seealso cref="UriComponents.HostAndPort"/>
        /// components.</remarks>
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

        // TODO: Move  to ExtensionMethods
        /// <summary>
        /// Gets the <seealso cref="UriComponents.Path"/> component of a <seealso cref="Uri"/>.
        /// </summary>
        /// <param name="uri">The source <seealso cref="Uri"/>.</param>
        /// <returns>The <seealso cref="UriComponents.Path"/> component of the source <paramref name="uri"/>.
        /// This will return <c>null</c> if the source <paramref name="uri"/> was <c>null</c>.</returns>
        public static string GetPathComponent(this Uri uri, UriFormat format = UriFormat.UriEscaped)
        {
            if (uri is null)
                return null;
            if (uri.IsAbsoluteUri)
                return uri.GetComponents(UriComponents.Path | UriComponents.KeepDelimiter, format);
            string originalString = uri.OriginalString;
            if (originalString.Length == 0)
                return "";
            originalString = SplitQueryAndFragment(originalString, false, out _);
            if (format == UriFormat.UriEscaped)
                return EnsureWellFormedUriString(originalString, UriKind.Relative);
            return (Uri.IsWellFormedUriString(originalString, UriKind.Relative)) ? Uri.UnescapeDataString(originalString) : originalString;
        }

        // TODO: Move  to ExtensionMethods
        /// <summary>
        /// Attempts to create a variant of a <seealso cref="Uri"/> with the specified <seealso cref="UriComponents.Path"/> component applied.
        /// </summary>
        /// <param name="uri">The original <seealso cref="Uri"/>.</param>
        /// <param name="newPath">The new <seealso cref="UriComponents.Path"/> component. This can be <c>null</c> to create a <seealso cref="Uri"/>
        /// without this component.</param>
        /// <param name="result">The new <seealso cref="Uri"/> with the specified <seealso cref="UriComponents.Path"/> component applied.</param>
        /// <returns><c>true</c> if the new <paramref name="result"/> <seealso cref="Uri"/> was successfully created; otherwise, <c>false</c>.</returns>
        /// <remarks>This can return <c>false</c> in circumstances where the change would create an invalid URI string as it pertains to the current
        /// <seealso cref="Uri.Scheme"/>. If the specified <paramref name="newPath"/> contains a query (<c>?</c>) and/or fragment (<c>#</c>) separator,
        /// those components will be aplied to the <paramref name="result"/> <seealso cref="Uri"/> as well.</remarks>
        public static bool TrySetPathComponent(this Uri uri, string newPath, out Uri result)
        {
            if (uri is null)
            {
                result = uri;
                return false;
            }

            string newQuery, newFragment;
            if (string.IsNullOrEmpty(newPath))
                newPath = newQuery = newFragment = "";
            else
                newPath = SplitQueryAndFragment(EnsureWellFormedUriString(newPath, UriKind.Relative), true, out newQuery, out newFragment);
            string oldPath = uri.SplitUriComponents(true, out string scheme, out string authority, out string oldQuery, out string oldFragment);
            if (uri.IsAbsoluteUri && newPath.Length > 0 && newPath[0] != URI_PATH_SEPARATOR_CHAR)
            {
                if (scheme.StartsWith($"{URI_SCHEME_URN}:") && !(scheme.Contains(URI_PATH_SEPARATOR_STRING) || oldPath.Contains(URI_PATH_SEPARATOR_STRING)))
                {
                    if (authority.Length > 0)
                        newPath = $":{newPath}";
                    else
                        newPath = $"::{newPath}";
                }
                else if (authority.Length > 0)
                    newPath = $"/{newPath}";
                else
                    newPath = $"//{newPath}";
            }
            if (oldPath.Equals(newPath) && (newQuery.Length == 0 || oldQuery.Equals(newQuery)) && (newFragment.Length == 0 || oldFragment.Equals(newFragment)))
            {
                result = uri;
                return true;
            }
            try
            {
                result = (string.IsNullOrEmpty(scheme)) ?
                    new Uri($"{newPath}{((newQuery.Length == 0) ? oldQuery : newQuery)}{((newFragment.Length == 0) ? oldFragment : newFragment)}", UriKind.Relative)
                    : new Uri($"{scheme}{authority}{newPath}{((newQuery.Length == 0) ? oldQuery : newQuery)}{((newFragment.Length == 0) ? oldFragment : newFragment)}",
                        UriKind.Absolute);
            }
            catch
            {
                result = uri;
                return false;
            }
            return true;
        }

        // TODO: Move  to ExtensionMethods
        /// <summary>
        /// Gets the <seealso cref="UriComponents.Query"/> component of a <seealso cref="Uri"/>.
        /// </summary>
        /// <param name="uri">The source <seealso cref="Uri"/>.</param>
        /// <returns>The <seealso cref="UriComponents.Query"/> component of the source <paramref name="uri"/> or <c>null</c> if it contained no query component.
        /// This will also return <c>null</c> if the source <paramref name="uri"/> was <c>null</c>.</returns>
        public static string GetQueryComponent(this Uri uri, bool keepDelimiters = false)
        {
            if (uri is null)
                return null;
            if (uri.IsAbsoluteUri)
                return (uri.Query.Length == 0) ? null : uri.Query.Substring(1);
            SplitQueryAndFragment(uri.OriginalString, keepDelimiters, out string query, out _);
            return query;
        }

        // TODO: Move  to ExtensionMethods
        /// <summary>
        /// Attempts to create a variant of a <seealso cref="Uri"/> with the specified <seealso cref="UriComponents.Query"/> component applied.
        /// </summary>
        /// <param name="uri">The original <seealso cref="Uri"/>.</param>
        /// <param name="query">The new <seealso cref="UriComponents.Query"/> component. This can be <c>null</c> to create a <seealso cref="Uri"/>
        /// without this component.</param>
        /// <param name="result">The new <seealso cref="Uri"/> with the specified <seealso cref="UriComponents.Query"/> component applied.</param>
        /// <returns><c>true</c> if the new <paramref name="result"/> <seealso cref="Uri"/> was successfully created; otherwise, <c>false</c>.</returns>
        /// <remarks>This can return <c>false</c> in circumstances where the change would create an invalid URI string as it pertains to the current
        /// <seealso cref="Uri.Scheme"/>. If the specified <paramref name="query"/> contains a fragment (<c>#</c>) separator, that component will be applied to
        /// the <paramref name="result"/> <seealso cref="Uri"/> as well.</remarks>
        public static bool TrySetQueryComponent(this Uri uri, string query, out Uri result)
        {
            if (uri is null)
            {
                result = uri;
                return false;
            }

            string newFragment;
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

                query = $"{URI_QUERY_DELIMITER_STRING}{SplitFragment(query, true, out newFragment).Replace(URI_QUERY_DELIMITER_STRING, URI_QUERY_DELIMITER_ESCAPED)}";
                try
                {
                    if (query.Equals(uri.Query))
                    {
                        if (newFragment.Length == 0 || newFragment.Equals(uri.Fragment))
                            result = uri;
                        else
                        {
                            string preceding = uri.GetComponents(BEFORE_FRAGMENT_COMPONENTS, UriFormat.UriEscaped);
                            result = new Uri($"{preceding}{newFragment}", UriKind.Relative);
                        }
                    }
                    else
                    {
                        string preceding = uri.GetComponents(BEFORE_QUERY_COMPONENTS, UriFormat.UriEscaped);
                        if (newFragment.Length > 0)
                            result = new Uri($"{preceding}{query}{newFragment}", UriKind.Relative);
                        else
                            result = new Uri($"{preceding}{query}{uri.Fragment}", UriKind.Relative);
                    }
                }
                catch
                {
                    result = uri;
                    return false;
                }
                return true;
            }

            string originalString = SplitQueryAndFragment(uri.OriginalString, true, out string oldQuery, out string oldFragment);

            if (query is null)
            {
                if (oldQuery.Length == 0)
                {
                    result = uri;
                    return true;
                }
                try
                {
                    result = new Uri($"{originalString}{oldFragment}", UriKind.Relative);
                }
                catch
                {
                    result = uri;
                    return false;
                }
            }

            query = $"{URI_QUERY_DELIMITER_STRING}{SplitFragment(query, true, out newFragment).Replace(URI_QUERY_DELIMITER_STRING, URI_QUERY_DELIMITER_ESCAPED)}";
            if (query.Equals(oldQuery) && (newFragment.Length == 0 || oldFragment.Equals(newFragment)))
                result = uri;
            else
                try
                {
                    result = new Uri($"{originalString}{query}{((newFragment.Length > 0) ? newFragment : oldFragment)}", UriKind.Relative);
                }
                catch
                {
                    result = uri;
                    return false;
                }
            return true;
        }

        // TODO: Move  to ExtensionMethods
        /// <summary>
        /// Gets the <seealso cref="UriComponents.Fragment"/> component of a <seealso cref="Uri"/>.
        /// </summary>
        /// <param name="uri">The source <seealso cref="Uri"/>.</param>
        /// <returns>The <seealso cref="UriComponents.Fragment"/> component of the source <paramref name="uri"/> or <c>null</c> if it contained no fragment component.
        /// This will also return <c>null</c> if the source <paramref name="uri"/> was <c>null</c>.</returns>
        public static string GetFragmentComponent(this Uri uri, bool keepDelimiter = false)
        {
            if (uri is null)
                return null;
            if (uri.IsAbsoluteUri)
                return (uri.Fragment.Length == 0) ? null : uri.Fragment.Substring(1);
            string originalString = AsRelativeUriString(uri.OriginalString);
            int index = originalString.IndexOf(URI_FRAGMENT_DELIMITER_CHAR);
            return (index < 0) ? ((keepDelimiter) ? "" : null) : originalString.Substring((keepDelimiter) ? index : index + 1);
        }

        // TODO: Move  to ExtensionMethods
        /// <summary>
        /// Attempts to create a variant of a <seealso cref="Uri"/> with the specified <seealso cref="UriComponents.Fragment"/> component applied.
        /// </summary>
        /// <param name="uri">The original <seealso cref="Uri"/>.</param>
        /// <param name="fragment">The new <seealso cref="UriComponents.Fragment"/> component. This can be <c>null</c> to create a <seealso cref="Uri"/>
        /// without this component.</param>
        /// <param name="result">The new <seealso cref="Uri"/> with the specified <seealso cref="UriComponents.Fragment"/> component applied.</param>
        /// <returns><c>true</c> if the new <paramref name="result"/> <seealso cref="Uri"/> was successfully created; otherwise, <c>false</c>.</returns>
        /// <remarks>This can return <c>false</c> in circumstances where the change would create an invalid URI string as it pertains to the current
        /// <seealso cref="Uri.Scheme"/>.</remarks>
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

                fragment = $"{URI_FRAGMENT_DELIMITER_STRING}{fragment}";
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


            string originalString = SplitFragment(uri.OriginalString, true, out string oldFragment);
            if (fragment is null)
            {
                if (oldFragment.Length > 0)
                    try
                    {
                        result = new Uri(originalString, UriKind.Relative);
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
            if (oldFragment.Length > 0 && oldFragment.Substring(1).Equals(fragment))
                result = uri;
            else
                try
                {
                    result = new Uri($"{originalString}{URI_FRAGMENT_DELIMITER_STRING}{fragment}", UriKind.Relative);
                }
                catch
                {
                    result = uri;
                    return false;
                }
            return true;
        }

        // TODO: Move  to ExtensionMethods
        /// <summary>
        /// Gets the path segments of a relative or absolute <seealso cref="Uri"/>;
        /// </summary>
        /// <param name="uri">The target <seealso cref="Uri"/>.</param>
        /// <returns>A System.String array that contains the path segments that make up the specified <seealso cref="Uri"/>.</returns>
        public static string[] GetPathSegments(this Uri uri)
        {
            if (uri is null)
                return Array.Empty<string>();
            if (uri.IsAbsoluteUri)
                return uri.Segments;
            string u = uri.OriginalString;
            if (u.Length == 0)
                return new string[] { u };
            u = EnsureWellFormedUriString(u, UriKind.Relative);
            return URI_PATH_SEGMENT_REGEX.Matches(u).Cast<Match>().Select(m => m.Value).ToArray();
        }

        // TODO: Move  to ExtensionMethods
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
                    return uri.TrySetPathComponent(URI_PATH_SEPARATOR_STRING, out result) && result.GetPathComponent().EndsWith(URI_PATH_SEPARATOR_CHAR);
            }
            else if (path.EndsWith(URI_PATH_SEPARATOR_CHAR) != shouldHaveTrailingSlash)
            {
                if (shouldHaveTrailingSlash)
                    return uri.TrySetPathComponent($"{path}/", out result) && result.GetPathComponent().EndsWith(URI_PATH_SEPARATOR_CHAR);
                int i = path.Length - 1;
                while (i > 0 && path[i - 1] == URI_PATH_SEPARATOR_CHAR)
                    i--;
                return uri.TrySetPathComponent(path.Substring(0, i), out result) && !result.GetPathComponent().EndsWith(URI_PATH_SEPARATOR_CHAR);
            }
            if (!(uri.IsAbsoluteUri || Uri.IsWellFormedUriString(uri.OriginalString, UriKind.Relative)))
                uri = new Uri(EnsureWellFormedUriString(uri.OriginalString, UriKind.Relative), UriKind.Relative);
            result = uri;
            return true;
        }
    }
}

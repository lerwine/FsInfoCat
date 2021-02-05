using System;

namespace FsInfoCat.Util
{
    public static class UrlHelper
    {
        public static Uri WithTrailingSlash(this Uri uri)
        {
            if (uri.IsAbsoluteUri)
            {
                if (uri.AbsolutePath.EndsWith("/"))
                    return uri;
                UriBuilder ub = new UriBuilder(uri);
                ub.Path += "/";
                return ub.Uri;
            }
            int index = uri.OriginalString.IndexOfAny(new char[] { '?', '#' });
            string p = uri.OriginalString;
            string qf;
            if (index < 0)
                qf = "";
            else
            {
                qf = p.Substring(index);
                p = p.Substring(0, index);
            }
            if (p.EndsWith("/"))
                return uri;
            return new Uri($"{p}/{qf}", UriKind.Relative);
        }

        public static Uri WithoutTrailingSlash(this Uri uri)
        {
            if (uri.IsAbsoluteUri)
            {
                if (!uri.AbsolutePath.EndsWith("/"))
                    return uri;
                UriBuilder ub = new UriBuilder(uri);
                ub.Path = (ub.Path.Length < 2) ? "" : ub.Path.Substring(0, ub.Path.Length - 1);
                return ub.Uri;
            }
            int index = uri.OriginalString.IndexOfAny(new char[] { '?', '#' });
            string p = uri.OriginalString;
            string qf;
            if (index < 0)
                qf = "";
            else
            {
                qf = p.Substring(index);
                p = p.Substring(0, index);
            }
            if (!p.EndsWith("/"))
                return uri;
            if (p.Length < 2)
                p = "";
            else
                p = p.Substring(0, p.Length - 1);
            return new Uri($"{p}/{qf}", UriKind.Relative);
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
                return uri.Authority.Equals(other.Authority, StringComparison.InvariantCultureIgnoreCase) &&
                    uri.PathAndQuery.Equals(other.PathAndQuery) && uri.Fragment.Equals(other.Fragment);
            }
            return !other.IsAbsoluteUri && uri.OriginalString.Equals(other.OriginalString);
        }
    }
}

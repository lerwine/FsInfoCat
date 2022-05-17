namespace FsInfoCat
{
    // TODO: Document UriHelper class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
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
        //private static readonly char[] _QUERY_OR_FRAGMENT_DELIMITER = new char[] { URI_QUERY_DELIMITER_CHAR, URI_FRAGMENT_DELIMITER_CHAR };
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}

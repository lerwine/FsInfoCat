namespace FsInfoCat.UriParsing
{
    /// <summary>
    /// Represents a URI query or path segment parameter.
    /// </summary>
    /// <remarks>For <c>mailto:</c> URI schemes, this represents a header field, such as 'to', and 'subject'.</remarks>
    public interface IUriParameterElement : IUriComponent
    {
        /// <summary>
        /// The URI-encoded value which precedes the key/value delimiter (<c>'='</c>) in the URI string.
        /// </summary>
        /// <remarks>This will be <see langword="null"/> if the URI query element does not contain a key/value delimiter (<c>'='</c>).</remarks>
        string Key { get; }

        /// <summary>
        /// The URI-encoded value which follows the key/value delimiter (<c>'='</c>) character in the URI string.
        /// </summary>
        /// <remarks>If the URI query element does not contain a key/value delimiter (<c>'='</c>), then this will contain the entire value of the URI query element.</remarks>
        string Value { get; }
    }
}

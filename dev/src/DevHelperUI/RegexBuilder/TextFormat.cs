namespace DevHelperUI
{
    public enum TextFormat
    {
        /// <summary>
        /// Text is displayed with no encoding.
        /// </summary>
        Normal,

        /// <summary>
        /// Text interprets C#-compatible escape sequences.
        /// </summary>
        BackslashEscaped,

        /// <summary>
        /// Text interprets PowerShell-compatible escape sequences.
        /// </summary>
        BacktickEscaped,

        /// <summary>
        /// Text interprets XML character entities.
        /// </summary>
        XmlEncoded,

        /// <summary>
        /// Text interprets URI escape sequences.
        /// </summary>
        UriEncoded
    }
}

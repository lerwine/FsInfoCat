namespace FsInfoCat.UriParsing
{
    /// <summary>
    /// Represents any parsed absolute or relative URI.
    /// </summary>
    /// <remarks>References
    /// <list type="bullet">
    ///     <item>Berners-Lee, T., Fielding, R. T., &amp; Masinter, L. (2005, January). Uniform resource identifier (uri): Generic syntax.
    ///         <para>Retrieved April 07, 2021, from https://tools.ietf.org/html/rfc3986</para></item>
    ///     <item>Kerwin, M. (2017, February). The "file" URI Scheme.
    ///         <para>Retrieved April 07, 2021, from https://tools.ietf.org/html/rfc8089</para></item>
    ///     <item>Carpenter, B., Cheshire, S., Hinden, R. M. (2017, February). Representing IPv6 Zone Identifiers in Address Literals and Uniform Resource Identifiers.
    ///         <para>Retrieved April 07, 2021, from https://tools.ietf.org/html/rfc6874</para></item>
    ///     <item>Duerst, M., Masinter, L., Zawinski, J. (2010, October). The 'mailto' URI Scheme.
    ///         <para>Retrieved April 07, 2021, from https://tools.ietf.org/html/rfc6068</para></item>
    ///     <item>HTML Living Standard. (2021, April).
    ///         <para>Retrieved April 07, 2021, from https://html.spec.whatwg.org/</para></item>
    /// </list>
    /// </remarks>
    public interface IAnyURI : IUriComponent
    {
        /// <summary>
        /// The absolute URI base components or <see langword="null"/> for relative URIs.
        /// </summary>
        IAbsoluteURIBase AbsoluteBase { get; }

        /// <summary>
        /// URI path segments.
        /// </summary>
        /// <remarks>If this is an empty list, then the path is assumed to be an empty string.</remarks>
        IPathSegmentList<IUriPathSegment> Segments { get; }

        /// <summary>
        /// URI query component elements or <see langword="null"/> if there is no query component.
        /// </summary>
        IUriComponentList<IUriParameterElement> Query { get; }

        /// <summary>
        /// URI fragment component (without leading delimiter) or <see langword="null"/> if there is no fragment component.
        /// </summary>
        string Fragment { get; }
    }
}

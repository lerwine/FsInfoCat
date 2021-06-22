using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace FsInfoCat.Util
{
    public class StringEscapingHelper
    {
        public static readonly Regex CsEscapablePattern = new Regex(@"(?<l>[""\\])|(\p{C}|(?! )(\s|\p{Z}))(?<x>[\da-fA-F])?", RegexOptions.Compiled);

        public static readonly Regex JsEscapableSqPattern = new Regex(@"(?<l>['\\])|(\p{C}|(?! )(\s|\p{Z}))(?<x>[\da-fA-F])?", RegexOptions.Compiled);

        public static readonly Regex JsEscapableDqPattern = new Regex(@"(?<l>[""\\])|(\p{C}|(?! )(\s|\p{Z}))(?<x>[\da-fA-F])?", RegexOptions.Compiled);

        public static readonly Regex CsEscapedPattern = new Regex(@"\\
(
    (?<l>[""\\])
|
    [0abfnrtv]
|
    
)", RegexOptions.Compiled);

        public static readonly Regex InvalidEscapedCsPattern = new Regex(@"(?<l>[""\\])|[\0\a\b\f\n\r\t\v]|(\p{C}|(?! )(\s|\p{Z}))(?<x>[\da-fA-F])?", RegexOptions.Compiled);

        public static readonly Regex BackslashEscapablePattern = new Regex(@"(?<l>[""\\])|[\0\a\b\f\n\r\t\v]|(\p{C}|(?! )(\s|\p{Z}))(?<x>[\da-fA-F])?", RegexOptions.Compiled);

        public static readonly Regex BackslashEscapableLBPattern = new Regex(@"(?<l>[""\\])|(?<n>\r\n?|\n)|[\0\a\b\f\t\v]|(\p{C}|(?! )(\s|\p{Z}))(?<x>[\da-fA-F])?", RegexOptions.Compiled);

        public static readonly Regex StrictBackslashEscapeSequencePattern = new Regex(@"\\((?<l>['""\\])|u(?<u16>[\dA-Fa-f]{4})|U(?<u32>[\dA-Fa-f]{8})|x(?<x>[\dA-Fa-f]{1,4})|(?<a>.)|$)", RegexOptions.Compiled);

        public static readonly Regex StrictBackslashEscapeSequenceLBPattern = new Regex(@"\\((?<l>['""\\])|r\n(?<l>\r\n)|r(?<l>\r)\n?|n(?<l>\n)|(?<a>[rn])(\r\n?|\n)|u(?<u16>[\dA-Fa-f]{4})|U(?<u32>[\dA-Fa-f]{8})|x(?<x>[\dA-Fa-f]{1,4})|(?<a>.)|$)", RegexOptions.Compiled);

        public static readonly Regex NormalBackslashEscapeSequencePattern = new Regex(@"\\((?<a>[0abfnrtv])|u(?<u16>[\dA-Fa-f]{4})|U(?<u32>[\dA-Fa-f]{8})|x(?<x>[\dA-Fa-f]{1,4})|[Uux](.|$)|(?<l>.))", RegexOptions.Compiled);

        public static readonly Regex NormalBackslashEscapeSequenceLBPattern = new Regex(@"\\(r\n(?<l>\r\n)|r(?<l>\r)\n?|n(?<l>\n)|(?<a>[rn])(\r\n?|\n)|(?<a>[0abfnrtv])|u(?<u16>[\dA-Fa-f]{4})|U(?<u32>[\dA-Fa-f]{8})|x(?<x>[\dA-Fa-f]{1,4})|[Uux](.|$)|(?<l>.))", RegexOptions.Compiled);

        public static readonly Regex LooseBackslashEscapeSequencePattern = new Regex(@"\\((?<a>[0abfnrtv])|u(?<u16>[\dA-Fa-f]{4})|U(?<u32>[\dA-Fa-f]{8})|x(?<x>[\dA-Fa-f]{1,4})|(?<l>.))", RegexOptions.Compiled);

        public static readonly Regex LooseBackslashEscapeSequenceLBPattern = new Regex(@"\\(r\n(?<l>\r\n)|r(?<l>\r)\n?|n(?<l>\n)|(?<a>[rn])(\r\n?|\n)|(?<a>[0abfnrtv])|u(?<u16>[\dA-Fa-f]{4})|U(?<u32>[\dA-Fa-f]{8})|x(?<x>[\dA-Fa-f]{1,4})|(?<l>.))", RegexOptions.Compiled);

        public static readonly Regex BacktickEscapablePattern = new Regex(@"(?<l>[""`$])|[\0\x1b\a\b\f\n\r\t\v]|\p{C}|(?! )(\s|\p{Z})", RegexOptions.Compiled);

        public static readonly Regex BacktickEscapableLBPattern = new Regex(@"(?<l>[""\\])|(?<n>\r\n?|\n)|[\0\x1b\a\b\f\t\v]|(\p{C}|(?! )(\s|\p{Z}))(?<x>[\da-fA-F])?", RegexOptions.Compiled);

        public static readonly Regex BacktickUnescapableSingleQuotePattern = new Regex(@"(?!\t)\p{C}|(?![ \t])(\s|\p{Z})", RegexOptions.Compiled);

        public static readonly Regex StrictBacktickEscapeSequencePattern = new Regex(@"`((?<a>[0abefnrtv])|u(\{(?<u>[\dA-Fa-f]{4})\}|.|$)|(?<l>.)|$)", RegexOptions.Compiled);

        public static readonly Regex StrictBacktickEscapeSequenceLBPattern = new Regex(@"`(r`n(?<l>\r\n)|r(?<l>\r)\n?|n(?<l>\n)|(?<a>[rn])(\r\n?|\n)|(?<a>[0abefnrtv])|u(\{(?<u>[\dA-Fa-f]{4})\}|.|$)|(?<l>.)|$)", RegexOptions.Compiled);

        public static readonly Regex NormalBacktickEscapeSequencePattern = new Regex(@"`((?<a>[0abefnrtv])|u\{((?<u>[\dA-Fa-f]{4})|.*)\}|(?<l>.)|$)", RegexOptions.Compiled);

        public static readonly Regex NormalBacktickEscapeSequenceLBPattern = new Regex(@"`(r`n(?<l>\r\n)|r(?<l>\r)\n?|n(?<l>\n)|(?<a>[rn])(\r\n?|\n)|(?<a>[0abefnrtv])|u\{((?<u>[\dA-Fa-f]{4})|.*)\}|(?<l>.)|$)", RegexOptions.Compiled);

        public static readonly Regex LooseBacktickEscapeSequencePattern = new Regex(@"`((?<a>[0abefnrtv])|u\{(?<u>[\dA-Fa-f]{4})\}|(?<l>.))", RegexOptions.Compiled);

        public static readonly Regex LooseBacktickEscapeSequenceLBPattern = new Regex(@"`(r`n(?<l>\r\n)|r(?<l>\r)\n?|n(?<l>\n)|(?<a>[rn])(\r\n?|\n)|(?<a>[0abefnrtv])|u\{(?<u>[\dA-Fa-f]{4})\}|(?<l>.))", RegexOptions.Compiled);

        public static readonly Regex XmlTextEncodablePattern = new Regex(@"[<>&]|(?![\t\r\n])(\p{C}|\s|\p{Z})", RegexOptions.Compiled);

        public static readonly Regex XmlAttributeEncodablePattern = new Regex(@"[<>&""]|\p{C}|(?! )(\s|\p{Z})", RegexOptions.Compiled);

        public static readonly Regex XmlAttributeEncodableLBPattern = new Regex(@"[<>&""]|(?<n>\r\n?|\n)|\p{C}|(?! )(\s|\p{Z})", RegexOptions.Compiled);

        public static readonly Regex StrictXmlTextEntityPattern = new Regex(@"&(#((?<d>\d+)|x(?<h>[\da-fA-F]{2}([\da-fA-F]{2})?))|(?<n>[^;]+));|[<>&]", RegexOptions.Compiled);

        public static readonly Regex StrictXmlAttributeEntityPattern = new Regex(@"&(#((?<d>\d+)|x(?<h>[\da-fA-F]{2}([\da-fA-F]{2})?))|(?<n>[^;]+));|[<>&""]", RegexOptions.Compiled);

        public static readonly Regex StrictXmlTextEntityLBPattern = new Regex(@"&((((?<d>13)|x(?<x>(00)?0[Dd]));((?<l>\r)\n?|&#(10|x(00)?0[Aa]);(?<l>\r\n))?|((?<d>10)|x(?<x>(00)?0[Aa]));(?<l>\n)?)|(#((?<d>\d+)|x(?<h>[\da-fA-F]{2}([\da-fA-F]{2})?))|(?<n>[^:]+));)|[<>&]", RegexOptions.Compiled);

        public static readonly Regex StrictXmlAttributeEntityLBPattern = new Regex(@"&((((?<d>13)|x(?<x>(00)?0[Dd]));((?<l>\r)\n?|&#(10|x(00)?0[Aa]);(?<l>\r\n))?|((?<d>10)|x(?<x>(00)?0[Aa]));(?<l>\n)?)|(#((?<d>\d+)|x(?<h>[\da-fA-F]{2}([\da-fA-F]{2})?))|(?<n>[^:]+));)|[<>&""]", RegexOptions.Compiled);

        public static readonly Regex LooseXmlEntityPattern = new Regex(@"&(#((?<d>\d+)|x(?<h>[\da-fA-F]{2}([\da-fA-F]{2})?))|(?<n>quot|amp|apos|lt|gt));", RegexOptions.Compiled);

        public static readonly Regex LooseXmlEntityLBPattern = new Regex(@"&((((?<d>13)|x(?<x>(00)?0[Dd]));((?<l>\r)\n?|&#(10|x(00)?0[Aa]);(?<l>\r\n))?|((?<d>10)|x(?<x>(00)?0[Aa]));(?<l>\n)?)|(#((?<d>\d+)|x(?<h>[\da-fA-F]{2}([\da-fA-F]{2})?))|(?<n>[^:]+));)", RegexOptions.Compiled);

        /// <summary>
        /// Unescapes the js string strict.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="ignoreFollowingLineBreaks">if set to <c>true</c> [ignore following line breaks].</param>
        /// <returns></returns>
        /// <remarks>APA References
        /// <list type="bullet">
        ///     <item><term>MDN contributors (2021, April 08)</term>
        ///         <description>String - JavaScript (Mozilla)
        ///         <para>Retrieved 2021, April 08, from MDN Web Docs: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/String#string_primitives_and_string_objects</para>
        ///         <para>doi: </para></description>
        ///     </item>
        ///     <item><term>Guo, S., Michael, F.,  &amp; Gibbons, K. (2021, April 06)</term>
        ///         <description>ECMAScript&#xae; 2022 Language Specification (ECMA International)
        ///         <para>Retrieved 2021, April 10, from TC39: https://tc39.es/ecma262/#sec-ecmascript-language-lexical-grammar-literals</para>
        ///         <para>doi: </para></description>
        ///     </item>
        ///     <item><term>Duerst, M., Masinter, L.,  &amp; Zawinski, J. (2010, October)</term>
        ///         <description>The 'mailto' URI Scheme (Internet Engineering Task Force (IETF))
        ///         <para>Retrieved 2021, April 07, from IETF Tools: https://tools.ietf.org/html/rfc6068</para>
        ///         <para>doi: 10.17487/RFC6068</para></description>
        ///     </item>
        ///     <item><term>URLs and Fetching Resources (2020, April)</term>
        ///         <description>WHATWG (Apple, Google, Mozilla, Microsoft)
        ///         <para>Retrieved 2021, April 07, from HTML Living Standard: https://html.spec.whatwg.org/multipage/urls-and-fetching.html#urls</para>
        ///         <para>doi: </para></description>
        ///     </item>
        ///     <item><term>URLs (2021, March 23)</term>
        ///         <description>WHATWG (Apple, Google, Mozilla, Microsoft)
        ///         <para>Retrieved 2021, April 10, from URL - Living Standard: https://url.spec.whatwg.org/</para>
        ///         <para>doi: </para></description>
        ///     </item>
        ///     <item><term>Carpenter, B., Hinden, R.,  &amp; Cheshire, S. (2013, February)</term>
        ///         <description>Representing IPv6 Zone Identifiers in Address Literals and Uniform Resource Identifiers (Internet Engineering Task Force (IETF))
        ///         <para>Retrieved 2021, April 07, from IETF Tools: https://tools.ietf.org/html/rfc6874</para>
        ///         <para>doi: 10.17487/RFC6874</para></description>
        ///     </item>
        ///     <item><term>Berners-Lee, T., Masinter, L.,  &amp; Fielding, R. T. (2005, January)</term>
        ///         <description>Uniform Resource Identifier (URI): Generic Syntax (Internet Engineering Task Force (IETF))
        ///         <para>Retrieved 2021, April 10, from IETF Tools: https://tools.ietf.org/html/rfc3986</para>
        ///         <para>doi: 10.17487/RFC3986</para></description>
        ///     </item>
        ///     <item><term>Kerwin, M. (2017, February)</term>
        ///         <description>The "file" URI Scheme (Internet Engineering Task Force (IETF))
        ///         <para>Retrieved 2021, April 07, from IETF Tools: https://tools.ietf.org/html/rfc8089</para>
        ///         <para>doi: 10.17487/RFC8089</para></description>
        ///     </item>
        /// </list>
        /// https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/String#escape_notation</remarks>
        public static string UnescapeJsStringStrict(string source, bool ignoreFollowingLineBreaks = false)
        {
            Regex regex;
            if (string.IsNullOrEmpty(source) || !(regex = ignoreFollowingLineBreaks ? StrictBackslashEscapeSequenceLBPattern : StrictBackslashEscapeSequencePattern).IsMatch(source))
                return source;
            return regex.Replace(source, m =>
            {
                Group g = m.Groups["l"];
                if (g.Success)
                    return g.Value;
                if ((g = m.Groups["a"]).Success)
                    switch (g.Value[0])
                    {
                        case '0':
                            return "\0";
                        case 'a':
                            return "\a";
                        case 'b':
                            return "\b";
                        case 'f':
                            return "\f";
                        case 'n':
                            return "\n";
                        case 'r':
                            return "\r";
                        case 't':
                            return "\t";
                        case 'v':
                            return "\v";
                    }
                else if ((g = m.Groups["u16"]).Success || (g = m.Groups["u32"]).Success || (g = m.Groups["x"]).Success)
                    return new string(new char[] { (char)uint.Parse(g.Value, System.Globalization.NumberStyles.HexNumber) });
                throw new ArgumentOutOfRangeException(nameof(source), $"Invalid escape sequence at index {m.Index}");
            });
        }

        public static string EscapeCsString(string source, bool keepLineBreaks = false)
        {
            if (string.IsNullOrEmpty(source) || !BackslashEscapablePattern.IsMatch(source))
                return source;
            if (keepLineBreaks)
                return BackslashEscapableLBPattern.Replace(source, m =>
                {
                    if (m.Groups["l"].Success)
                        return $"\\{m.Value}";
                    Group g = m.Groups["n"];
                    if (g.Success)
                        switch (g.Value)
                        {
                            case "\r":
                                return "\\r\r";
                            case "\n":
                                return "\\n\n";
                            default:
                                return "\\r\\n\r\n";
                        }
                    char c = m.Value[0];
                    switch (c)
                    {
                        case '\0':
                            return "\\0";
                        case '\a':
                            return "\\a";
                        case '\b':
                            return "\\b";
                        case '\f':
                            return "\\f";
                        case '\t':
                            return "\\t";
                        case '\v':
                            return "\\v";
                        default:
                            g = m.Groups["x"];
                            uint i = (uint)c;
                            if (g.Success)
                                return $"\\x{i:x4}{g.Value}";
                            return (i > 0xff) ? $"\\x{i:x4}" : $"\\x{i:x2}";
                    }
                });
            return BackslashEscapablePattern.Replace(source, m =>
            {
                if (m.Groups["l"].Success)
                    return $"\\{m.Value}";
                char c = m.Value[0];
                switch (c)
                {
                    case '\0':
                        return "\\0";
                    case '\a':
                        return "\\a";
                    case '\b':
                        return "\\b";
                    case '\f':
                        return "\\f";
                    case '\n':
                        return "\\n";
                    case '\r':
                        return "\\r";
                    case '\t':
                        return "\\t";
                    case '\v':
                        return "\\v";
                    default:
                        Group g = m.Groups["x"];
                        uint i = (uint)c;
                        if (g.Success)
                            return $"\\x{i:x4}{g.Value}";
                        return (i > 0xff) ? $"\\x{i:x4}" : $"\\x{i:x2}";
                }
            });
        }

        /// <summary>
        /// Decodes a back-slash escaped C# string.
        /// </summary>
        /// <param name="source">The string to decode.</param>
        /// <param name="ignoreFollowingLineBreaks"><see langword="true"/> to ignore actual line breaks following an escape sequence that represents line breaks.</param>
        /// <returns>The decoded string.</returns>
        /// <exception cref="FormatException">The <paramref name="source"/> string has an invalid escape sequence.</exception>
        /// <remarks>https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/#string-escape-sequences</remarks>
        public static string UnescapeCsStringStrict(string source, bool ignoreFollowingLineBreaks = false)
        {
            Regex regex;
            if (string.IsNullOrEmpty(source) || !(regex = ignoreFollowingLineBreaks ? StrictBackslashEscapeSequenceLBPattern : StrictBackslashEscapeSequencePattern).IsMatch(source))
                return source;
            return regex.Replace(source, m =>
            {
                Group g = m.Groups["l"];
                if (g.Success)
                    return g.Value;
                if ((g = m.Groups["a"]).Success)
                    switch (g.Value[0])
                    {
                        case '0':
                            return "\0";
                        case 'a':
                            return "\a";
                        case 'b':
                            return "\b";
                        case 'f':
                            return "\f";
                        case 'n':
                            return "\n";
                        case 'r':
                            return "\r";
                        case 't':
                            return "\t";
                        case 'v':
                            return "\v";
                    }
                else if ((g = m.Groups["u16"]).Success || (g = m.Groups["u32"]).Success || (g = m.Groups["x"]).Success)
                    return new string(new char[] { (char)uint.Parse(g.Value, System.Globalization.NumberStyles.HexNumber) });
                throw new ArgumentOutOfRangeException(nameof(source), $"Invalid escape sequence at index {m.Index}");
            });
        }

        /// <summary>
        /// Decodes a back-slash escaped C# string, ignoring unknown escape sequence characters.
        /// </summary>
        /// <param name="source">The string to decode.</param>
        /// <param name="ignoreFollowingLineBreaks"><see langword="true"/> to ignore actual line breaks following an escape sequence that represents line breaks.</param>
        /// <returns>The decoded string.</returns>
        /// <exception cref="FormatException">The <paramref name="source"/> string has an invalid hexidecimal escape sequence.</exception>
        public static string UnescapeCsString(string source, bool ignoreFollowingLineBreaks = false)
        {
            Regex regex;
            if (string.IsNullOrEmpty(source) || !(regex = ignoreFollowingLineBreaks ? NormalBackslashEscapeSequenceLBPattern : NormalBackslashEscapeSequencePattern).IsMatch(source))
                return source;
            return regex.Replace(source, m =>
            {
                Group g = m.Groups["l"];
                if (g.Success)
                    return g.Value;
                if ((g = m.Groups["a"]).Success)
                    switch (g.Value[0])
                    {
                        case '0':
                            return "\0";
                        case 'a':
                            return "\a";
                        case 'b':
                            return "\b";
                        case 'f':
                            return "\f";
                        case 'r':
                            return "\r";
                        case 't':
                            return "\t";
                        case 'v':
                            return "\v";
                        default:
                            return "\n";
                    }
                if ((g = m.Groups["u16"]).Success || (g = m.Groups["u32"]).Success || (g = m.Groups["x"]).Success)
                    return new string(new char[] { (char)uint.Parse(g.Value, System.Globalization.NumberStyles.HexNumber) });
                throw new ArgumentOutOfRangeException(nameof(source), $"Invalid escape sequence at index {m.Index}");
            });
        }

        public static string ForceUnescapeCsString(string source, bool ignoreFollowingLineBreaks = false)
        {
            Regex regex;
            if (string.IsNullOrEmpty(source) || !(regex = ignoreFollowingLineBreaks ? LooseBackslashEscapeSequenceLBPattern : LooseBackslashEscapeSequencePattern).IsMatch(source))
                return source;
            return regex.Replace(source, m =>
            {
                Group g = m.Groups["a"];
                if (g.Success)
                    switch (g.Value[0])
                    {
                        case '0':
                            return "\0";
                        case 'a':
                            return "\a";
                        case 'b':
                            return "\b";
                        case 'f':
                            return "\f";
                        case 'r':
                            return "\r";
                        case 't':
                            return "\t";
                        case 'v':
                            return "\v";
                        default:
                            return "\n";
                    }
                if ((g = m.Groups["u16"]).Success || (g = m.Groups["u32"]).Success || (g = m.Groups["x"]).Success)
                    return new string(new char[] { (char)uint.Parse(g.Value, System.Globalization.NumberStyles.HexNumber) });
                return m.Groups["l"].Value;
            });
        }

        public static string EscapePsStringDoubleQuote(string source, bool keepLineBreaks = false)
        {
            if (string.IsNullOrEmpty(source) || !BacktickEscapablePattern.IsMatch(source))
                return source;
            if (keepLineBreaks)
                return BacktickEscapableLBPattern.Replace(source, m =>
                {
                    if (m.Groups["l"].Success)
                        return $"`{m.Value}";
                    Group g = m.Groups["n"];
                    if (g.Success)
                        switch (g.Value)
                        {
                            case "\r":
                                return "`r\r";
                            case "\n":
                                return "`n\n";
                            default:
                                return "`r`n\r\n";
                        }
                    char c = m.Value[0];
                    switch (c)
                    {
                        case '\0':
                            return "`0";
                        case '\a':
                            return "`a";
                        case '\b':
                            return "`b";
                        case '\x1b':
                            return "`e";
                        case '\f':
                            return "`f";
                        case '\t':
                            return "`t";
                        case '\v':
                            return "`v";
                        default:
                            return $"`u{{{((uint)c):x4}}}";
                    }
                });
            return BacktickEscapablePattern.Replace(source, m =>
            {
                if (m.Groups["l"].Success)
                    return $"`{m.Value}";
                char c = m.Value[0];
                switch (c)
                {
                    case '\0':
                        return "`0";
                    case '\a':
                        return "`a";
                    case '\b':
                        return "`b";
                    case '\x1b':
                        return "`e";
                    case '\f':
                        return "`f";
                    case '\n':
                        return "`n";
                    case '\r':
                        return "`r";
                    case '\t':
                        return "`t";
                    case '\v':
                        return "`v";
                    default:
                        return $"`u{{{((uint)c):x4}}}";
                }
            });
        }

        public static string EscapePsString(string source, out bool escapedForDoubleQuote) => EscapePsString(source, false, out escapedForDoubleQuote);

        public static string EscapePsString(string source, bool keepLineBreaks, out bool escapedForDoubleQuote)
        {
            if (string.IsNullOrEmpty(source) || !BackslashEscapablePattern.IsMatch(source))
            {
                escapedForDoubleQuote = false;
                return source;
            }

            if (BacktickUnescapableSingleQuotePattern.IsMatch(source))
            {
                // Must do double-quote
                escapedForDoubleQuote = true;
                return EscapePsStringDoubleQuote(source, keepLineBreaks);
            }

            if (source.Contains('\''))
            {
                string d = EscapePsStringDoubleQuote(source, keepLineBreaks);
                string s = source.Replace("'", "''");
                escapedForDoubleQuote = d.Length < s.Length;
                return escapedForDoubleQuote ? d : s;
            }
            escapedForDoubleQuote = false;
            return source;
        }

        /// <summary>
        /// Decodes a back-tick escaped PowerShell string.
        /// </summary>
        /// <param name="source">The string to decode.</param>
        /// <param name="ignoreFollowingLineBreaks"><see langword="true"/> to ignore actual line breaks following an escape sequence that represents line breaks.</param>
        /// <returns>The decoded string.</returns>
        /// <exception cref="FormatException">The <paramref name="source"/> string has an invalid escape sequence.</exception>
        /// <remarks>This supports the unicode escape sequences (<c>`u{xxxx}</c>) as well
        /// <para>https://docs.microsoft.com/en-us/powershell/module/microsoft.powershell.core/about/about_special_characters?view=powershell-7.1&viewFallbackFrom=powershell-6#unicode-character-ux</para></remarks>
        public static string UnescapePsStringStrict(string source, bool ignoreFollowingLineBreaks = false)
        {
            Regex regex;
            if (string.IsNullOrEmpty(source) || !(regex = ignoreFollowingLineBreaks ? StrictBacktickEscapeSequenceLBPattern : StrictBacktickEscapeSequencePattern).IsMatch(source))
                return source;
            return regex.Replace(source, m =>
            {
                Group g = m.Groups["l"];
                if (g.Success)
                    return g.Value;
                if ((g = m.Groups["a"]).Success)
                    switch (g.Value[0])
                    {
                        case '0':
                            return "\0";
                        case 'a':
                            return "\a";
                        case 'b':
                            return "\b";
                        case 'e':
                            return "\x1b";
                        case 'f':
                            return "\f";
                        case 'r':
                            return "\r";
                        case 't':
                            return "\t";
                        case 'v':
                            return "\v";
                        default:
                            return "\n";
                    }
                if ((g = m.Groups["u"]).Success)
                    return new string(new char[] { (char)uint.Parse(g.Value, System.Globalization.NumberStyles.HexNumber) });
                throw new ArgumentOutOfRangeException(nameof(source), $"Invalid escape sequence at index {m.Index}");
            });
        }

        /// <summary>
        /// Decodes a back-tick escaped PowerShell string, ignoring unknown escape sequence characters.
        /// </summary>
        /// <param name="source">The string to decode.</param>
        /// <param name="ignoreFollowingLineBreaks"><see langword="true"/> to ignore actual line breaks following an escape sequence that represents line breaks.</param>
        /// <returns>The decoded string.</returns>
        /// <exception cref="FormatException">The <paramref name="source"/> string has an invalid hexidecimal escape sequence.</exception>
        public static string UnescapePsString(string source, bool ignoreFollowingLineBreaks = false)
        {
            Regex regex;
            if (string.IsNullOrEmpty(source) || !(regex = ignoreFollowingLineBreaks ? NormalBacktickEscapeSequenceLBPattern : NormalBacktickEscapeSequencePattern).IsMatch(source))
                return source;
            return regex.Replace(source, m =>
            {
                Group g = m.Groups["l"];
                if (g.Success)
                    return g.Value;
                if ((g = m.Groups["a"]).Success)
                    switch (g.Value[0])
                    {
                        case '0':
                            return "\0";
                        case 'a':
                            return "\a";
                        case 'b':
                            return "\b";
                        case 'f':
                            return "\f";
                        case 'r':
                            return "\r";
                        case 't':
                            return "\t";
                        case 'v':
                            return "\v";
                        default:
                            return "\n";
                    }
                if ((g = m.Groups["u"]).Success)
                    return new string(new char[] { (char)uint.Parse(g.Value, System.Globalization.NumberStyles.HexNumber) });
                throw new ArgumentOutOfRangeException(nameof(source), $"Invalid escape sequence at index {m.Index}");
            });
        }

        public static string ForceUnescapePsString(string source, bool ignoreFollowingLineBreaks = false)
        {
            Regex regex;
            if (string.IsNullOrEmpty(source) || !(regex = ignoreFollowingLineBreaks ? LooseBacktickEscapeSequenceLBPattern : LooseBacktickEscapeSequencePattern).IsMatch(source))
                return source;
            return regex.Replace(source, m =>
            {
                Group g = m.Groups["a"];
                if (g.Success)
                    switch (g.Value[0])
                    {
                        case '0':
                            return "\0";
                        case 'a':
                            return "\a";
                        case 'b':
                            return "\b";
                        case 'f':
                            return "\f";
                        case 'r':
                            return "\r";
                        case 't':
                            return "\t";
                        case 'v':
                            return "\v";
                        default:
                            return "\n";
                    }
                if ((g = m.Groups["u"]).Success)
                    return new string(new char[] { (char)uint.Parse(g.Value, System.Globalization.NumberStyles.HexNumber) });
                return m.Groups["l"].Value;
            });
        }

        public static string EscapeXmlTextString(string source)
        {
            if (string.IsNullOrEmpty(source) || !XmlTextEncodablePattern.IsMatch(source))
                return source;
            return XmlTextEncodablePattern.Replace(source, m =>
            {
                char c = m.Value[0];
                switch (c)
                {
                    case '<':
                        return "&lt;";
                    case '>':
                        return "&gt;";
                    case '&':
                        return "&amp;";
                    default:
                        uint i = (uint)c;
                        return (i > 0xff) ? $"&#x{i:x4};" : $"&#x{i:x2};";
                }
            });
        }

        public static string EscapeAttributeString(string source, bool keepLineBreaks = false)
        {
            if (string.IsNullOrEmpty(source) || !XmlAttributeEncodablePattern.IsMatch(source))
                return source;
            if (keepLineBreaks)
                return XmlAttributeEncodableLBPattern.Replace(source, m =>
                {
                    Group g = m.Groups["n"];
                    if (g.Success)
                        switch (g.Value)
                        {
                            case "\r":
                                return "&#x0D;\r";
                            case "\n":
                                return "&#x0A;\n";
                            default:
                                return "&#x0D;&#x0A;\r\n";
                        }
                    char c = m.Value[0];
                    switch (c)
                    {
                        case '<':
                            return "&lt;";
                        case '>':
                            return "&gt;";
                        case '&':
                            return "&amp;";
                        case '"':
                            return "&quot;";
                        default:
                            uint i = (uint)c;
                            return (i > 0xff) ? $"&#x{i:x4};" : $"&#x{i:x2};";
                    }
                });
            return XmlAttributeEncodablePattern.Replace(source, m =>
            {
                char c = m.Value[0];
                switch (c)
                {
                    case '<':
                        return "&lt;";
                    case '>':
                        return "&gt;";
                    case '&':
                        return "&amp;";
                    case '"':
                        return "&quot;";
                    default:
                        uint i = (uint)c;
                        return (i > 0xff) ? $"&#x{i:x4};" : $"&#x{i:x2};";
                }
            });
        }

        public static string UnescapeXmlTextStringStrict(string source, bool ignoreFollowingLineBreaks = false)
        {
            if (string.IsNullOrEmpty(source))
                return source;

            if (ignoreFollowingLineBreaks)
            {
                if (!StrictXmlTextEntityLBPattern.IsMatch(source))
                    return source;
                return StrictXmlTextEntityLBPattern.Replace(source, m =>
                {
                    Group g = m.Groups["l"];
                    if (g.Success)
                        return g.Value;
                    if ((g = m.Groups["d"]).Success)
                    {
                        if (int.TryParse(g.Value, out int i) && i < 65536)
                            return new string(new char[] { (char)i });
                    }
                    else if ((g = m.Groups["h"]).Success)
                    {
                        if (int.TryParse(g.Value, System.Globalization.NumberStyles.HexNumber, null, out int i) && i < 65536)
                            return new string(new char[] { (char)i });
                    }
                    else if ((g = m.Groups["n"]).Success)
                        switch (g.Value)
                        {
                            case "quot":
                                return "\"";
                            case "amp":
                                return "&";
                            case "apos":
                                return "'";
                            case "lt":
                                return "<";
                            case "gt":
                                return ">";
                        }
                    if (m.Value == "&")
                        throw new ArgumentOutOfRangeException(nameof(source), $"Invalid character entity at index {m.Index}");
                    throw new ArgumentOutOfRangeException(nameof(source), $"Invalid character at index {m.Index}");
                });
            }

            if (!StrictXmlTextEntityPattern.IsMatch(source))
                return source;
            return StrictXmlTextEntityPattern.Replace(source, m =>
            {
                Group g = m.Groups["d"];
                if (g.Success)
                {
                    if (int.TryParse(g.Value, out int i) && i < 65536)
                        return new string(new char[] { (char)i });
                }
                else if ((g = m.Groups["h"]).Success)
                {
                    if (int.TryParse(g.Value, System.Globalization.NumberStyles.HexNumber, null, out int i) && i < 65536)
                        return new string(new char[] { (char)i });
                }
                else if ((g = m.Groups["n"]).Success)
                    switch (g.Value)
                    {
                        case "quot":
                            return "\"";
                        case "amp":
                            return "&";
                        case "apos":
                            return "'";
                        case "lt":
                            return "<";
                        case "gt":
                            return ">";
                    }
                if (m.Value == "&")
                    throw new ArgumentOutOfRangeException(nameof(source), $"Invalid character entity at index {m.Index}");
                throw new ArgumentOutOfRangeException(nameof(source), $"Invalid character at index {m.Index}");
            });
        }

        public static string UnescapeXmlAttributeStringStrict(string source, bool ignoreFollowingLineBreaks = false)
        {
            if (string.IsNullOrEmpty(source))
                return source;

            if (ignoreFollowingLineBreaks)
            {
                if (!StrictXmlAttributeEntityLBPattern.IsMatch(source))
                    return source;
                return StrictXmlAttributeEntityLBPattern.Replace(source, m =>
                {
                    Group g = m.Groups["l"];
                    if (g.Success)
                        return g.Value;
                    if ((g = m.Groups["d"]).Success)
                    {
                        if (int.TryParse(g.Value, out int i) && i < 65536)
                            return new string(new char[] { (char)i });
                    }
                    else if ((g = m.Groups["h"]).Success)
                    {
                        if (int.TryParse(g.Value, System.Globalization.NumberStyles.HexNumber, null, out int i) && i < 65536)
                            return new string(new char[] { (char)i });
                    }
                    else if ((g = m.Groups["n"]).Success)
                        switch (g.Value)
                        {
                            case "quot":
                                return "\"";
                            case "amp":
                                return "&";
                            case "apos":
                                return "'";
                            case "lt":
                                return "<";
                            case "gt":
                                return ">";
                        }
                    if (m.Value == "&")
                        throw new ArgumentOutOfRangeException(nameof(source), $"Invalid character entity at index {m.Index}");
                    throw new ArgumentOutOfRangeException(nameof(source), $"Invalid character at index {m.Index}");
                });
            }

            if (!StrictXmlAttributeEntityPattern.IsMatch(source))
                return source;
            return StrictXmlAttributeEntityPattern.Replace(source, m =>
            {
                Group g = m.Groups["d"];
                if (g.Success)
                {
                    if (int.TryParse(g.Value, out int i) && i < 65536)
                        return new string(new char[] { (char)i });
                }
                else if ((g = m.Groups["h"]).Success)
                {
                    if (int.TryParse(g.Value, System.Globalization.NumberStyles.HexNumber, null, out int i) && i < 65536)
                        return new string(new char[] { (char)i });
                }
                else if ((g = m.Groups["n"]).Success)
                    switch (g.Value)
                    {
                        case "quot":
                            return "\"";
                        case "amp":
                            return "&";
                        case "apos":
                            return "'";
                        case "lt":
                            return "<";
                        case "gt":
                            return ">";
                    }
                if (m.Value == "&")
                    throw new ArgumentOutOfRangeException(nameof(source), $"Invalid character entity at index {m.Index}");
                throw new ArgumentOutOfRangeException(nameof(source), $"Invalid character at index {m.Index}");
            });
        }

        public static string ForceUnescapeXmlString(string source, bool ignoreFollowingLineBreaks = false)
        {
            if (string.IsNullOrEmpty(source))
                return source;

            if (ignoreFollowingLineBreaks)
            {
                if (!LooseXmlEntityLBPattern.IsMatch(source))
                    return source;
                return LooseXmlEntityLBPattern.Replace(source, m =>
                {
                    Group g = m.Groups["l"];
                    if (g.Success)
                        return g.Value;
                    if ((g = m.Groups["d"]).Success)
                    {
                        if (int.TryParse(g.Value, out int i) && i < 65536)
                            return new string(new char[] { (char)i });
                    }
                    else if ((g = m.Groups["h"]).Success)
                    {
                        if (int.TryParse(g.Value, System.Globalization.NumberStyles.HexNumber, null, out int i) && i < 65536)
                            return new string(new char[] { (char)i });
                    }
                    else if ((g = m.Groups["n"]).Success)
                        switch (g.Value)
                        {
                            case "quot":
                                return "\"";
                            case "amp":
                                return "&";
                            case "apos":
                                return "'";
                            case "lt":
                                return "<";
                            case "gt":
                                return ">";
                        }
                    if (m.Value == "&")
                        throw new ArgumentOutOfRangeException(nameof(source), $"Invalid character entity at index {m.Index}");
                    throw new ArgumentOutOfRangeException(nameof(source), $"Invalid character at index {m.Index}");
                });
            }

            if (!LooseXmlEntityPattern.IsMatch(source))
                return source;
            return LooseXmlEntityPattern.Replace(source, m =>
            {
                Group g = m.Groups["d"];
                if (g.Success)
                {
                    if (int.TryParse(g.Value, out int i) && i < 65536)
                        return new string(new char[] { (char)i });
                }
                else if ((g = m.Groups["h"]).Success)
                {
                    if (int.TryParse(g.Value, System.Globalization.NumberStyles.HexNumber, null, out int i) && i < 65536)
                        return new string(new char[] { (char)i });
                }
                else if ((g = m.Groups["n"]).Success)
                    switch (g.Value)
                    {
                        case "quot":
                            return "\"";
                        case "amp":
                            return "&";
                        case "apos":
                            return "'";
                        case "lt":
                            return "<";
                        case "gt":
                            return ">";
                    }
                if (m.Value == "&")
                    throw new ArgumentOutOfRangeException(nameof(source), $"Invalid character entity at index {m.Index}");
                throw new ArgumentOutOfRangeException(nameof(source), $"Invalid character at index {m.Index}");
            });
        }

        public static string ForceUnescapeXmlAttributeString(string source, bool ignoreFollowingLineBreaks = false)
        {
            if (string.IsNullOrEmpty(source))
                return source;

            if (ignoreFollowingLineBreaks)
            {
                if (!StrictXmlAttributeEntityLBPattern.IsMatch(source))
                    return source;
                return StrictXmlAttributeEntityLBPattern.Replace(source, m =>
                {
                    Group g = m.Groups["l"];
                    if (g.Success)
                        return g.Value;
                    if ((g = m.Groups["d"]).Success)
                    {
                        if (int.TryParse(g.Value, out int i) && i < 65536)
                            return new string(new char[] { (char)i });
                    }
                    else if ((g = m.Groups["h"]).Success)
                    {
                        if (int.TryParse(g.Value, System.Globalization.NumberStyles.HexNumber, null, out int i) && i < 65536)
                            return new string(new char[] { (char)i });
                    }
                    else if ((g = m.Groups["n"]).Success)
                        switch (g.Value)
                        {
                            case "quot":
                                return "\"";
                            case "amp":
                                return "&";
                            case "apos":
                                return "'";
                            case "lt":
                                return "<";
                            case "gt":
                                return ">";
                        }
                    if (m.Value == "&")
                        throw new ArgumentOutOfRangeException(nameof(source), $"Invalid character entity at index {m.Index}");
                    throw new ArgumentOutOfRangeException(nameof(source), $"Invalid character at index {m.Index}");
                });
            }

            if (StrictXmlAttributeEntityPattern.IsMatch(source))
                return source;
            return StrictXmlAttributeEntityPattern.Replace(source, m =>
            {
                Group g = m.Groups["d"];
                if (g.Success)
                {
                    if (int.TryParse(g.Value, out int i) && i < 65536)
                        return new string(new char[] { (char)i });
                }
                else if ((g = m.Groups["h"]).Success)
                {
                    if (int.TryParse(g.Value, System.Globalization.NumberStyles.HexNumber, null, out int i) && i < 65536)
                        return new string(new char[] { (char)i });
                }
                else if ((g = m.Groups["n"]).Success)
                    switch (g.Value)
                    {
                        case "quot":
                            return "\"";
                        case "amp":
                            return "&";
                        case "apos":
                            return "'";
                        case "lt":
                            return "<";
                        case "gt":
                            return ">";
                    }
                if (m.Value == "&")
                    throw new ArgumentOutOfRangeException(nameof(source), $"Invalid character entity at index {m.Index}");
                throw new ArgumentOutOfRangeException(nameof(source), $"Invalid character at index {m.Index}");
            });
        }
    }
}

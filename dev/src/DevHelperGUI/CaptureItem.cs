using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace DevHelperGUI
{
    public abstract class CaptureItem
    {
        private static readonly Regex NewLineRegex = new Regex(@"\r\n?|\n", RegexOptions.Compiled);
        private static readonly Regex UnescapeCharRegex = new Regex(@"\\((?<s>[afnrtv\\])|x(?<x>[\dA-Fa-f]{2})|u(?<u>[\dA-Fa-f]{4}))", RegexOptions.Compiled);
        private static readonly Regex UnescapeUriRegex = new Regex(@"%(?<e>[\da-fA-F]{2})", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex UnescapeXmlRegex = new Regex(@"&((?<e>quot|amp|apos|lt|gt)|#(x(?<x>[\da-fA-F]{2,4})|(?<d>\d+)));", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex XmlEncodeRegex = new Regex(@"(?<e>[""&'<>])|[\p{Zl}\p{Zp}\p{C}]|(?![ \t\r\n])\s", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex XmlEncodeRegexT = new Regex(@"(?<e>[""&'<>])|(?<n>[\p{Zl}\p{Zp}]|\r?\n|\n)|(?![\r\n])[\p{C}]|(?! )\s", RegexOptions.Compiled);
        private static readonly Regex UriEncodeRegex = new Regex(@"[^\r\n\t!$=&-.:;=@[\]/\w%]|%(?![\da-fA-F]{2})", RegexOptions.Compiled);
        private static readonly Regex UriEncodeRegexT = new Regex(@"[^\r\n\t!$=&-.:;=@[\]/\w%]|%(?![\da-fA-F]{2})|(?<n>[\p{Zl}\p{Zp}]|\r?\n|\n)", RegexOptions.Compiled);
        private static readonly Regex ExtendedCharRegex = new Regex(@"[\u0100-\uffff]", RegexOptions.Compiled);
        private static readonly Regex EscapeCharRegex = new Regex(@"(?![ \t\r\n\p{Zl}\p{Zp}])[\p{C}\p{Z}\s\\]|(?<n>\r\n?|[\n\p{Zl}\p{Zp}])", RegexOptions.Compiled);
        private static readonly Regex EscapeCharRegexT = new Regex(@"(?![ \r\n\p{Zl}\p{Zp}])[\p{C}\p{Z}\s\\]|(?<n>\r\n?|[\n\p{Zl}\p{Zp}])", RegexOptions.Compiled);
        private static readonly Regex QuotedCharRegex = new Regex(@"(?![ \t\r\n\p{Zl}\p{Zp}])[\p{C}\p{Z}\s""\\]|(?<n>\r\n?|[\n\p{Zl}\p{Zp}])", RegexOptions.Compiled);
        private static readonly Regex QuotedCharRegexT = new Regex(@"(?![ \r\n\p{Zl}\p{Zp}])[\p{C}\p{Z}\s""\\]|(?<n>\r\n?|[\n\p{Zl}\p{Zp}])", RegexOptions.Compiled);
        private static readonly Regex LineMatchRegex = new Regex(@"\G(?<t>[^\r\n\p{Zl}\p{Zp}]*)(\r\n?|[\n\p{Zl}\p{Zp}])", RegexOptions.Compiled);

        private string _escapedValue;
        private string _escapedValueLE;
        private string _quotedLines;
        private string _quotedLinesLE;
        private string _uriEncoded;
        private string _uriEncodedLE;
        private string _xmlEncoded;
        private string _xmlEncodedLE;
        private string _hexEncoded;

        public int Index { get; }

        public int Length { get; }

        public bool Success { get; }

        public string RawValue { get; }

        public string EscapedValue
        {
            get
            {
                if (_escapedValue is null)
                    _escapedValue = ToEscapedLines(RawValue, false);
                return _escapedValue;
            }
        }
        public string EscapedValueLE
        {
            get
            {
                if (_escapedValueLE is null)
                    _escapedValueLE = ToEscapedLines(RawValue, true);
                return _escapedValueLE;
            }
        }
        public string QuotedLines
        {
            get
            {
                if (_quotedLines is null)
                    _quotedLines = ToQuotedLines(RawValue, false);
                return _quotedLines;
            }
        }
        public string QuotedLinesLE
        {
            get
            {
                if (_quotedLinesLE is null)
                    _quotedLinesLE = ToQuotedLines(RawValue, true);
                return _quotedLinesLE;
            }
        }

        public string UriEncodedValue
        {
            get
            {
                if (_uriEncoded is null)
                    _uriEncoded = ToUriEncodedLines(RawValue, false);
                return _uriEncoded;
            }
        }

        public string UriEncodedValueLE
        {
            get
            {
                if (_uriEncodedLE is null)
                    _uriEncodedLE = ToUriEncodedLines(RawValue, true);
                return _uriEncodedLE;
            }
        }

        public string XmlEncodedValue
        {
            get
            {
                if (_xmlEncoded is null)
                    _xmlEncoded = ToXmlEncodedLines(RawValue, false);
                return _xmlEncoded;
            }
        }

        public string XmlEncodedValueLE
        {
            get
            {
                if (_xmlEncodedLE is null)
                    _xmlEncodedLE = ToXmlEncodedLines(RawValue, true);
                return _xmlEncodedLE;
            }
        }

        public string HexEncoded
        {
            get
            {
                if (_hexEncoded is null)
                    _hexEncoded = ToHexEncoded(RawValue);
                return _hexEncoded;
            }
        }

        protected CaptureItem(Capture capture)
        {
            if (capture is null)
                throw new ArgumentNullException(nameof(capture));
            if (capture is Group group)
            {
                Success = group.Success;
                if (!Success)
                {
                    RawValue = _escapedValue = _escapedValueLE = _quotedLines = _quotedLinesLE = _uriEncoded = _uriEncodedLE = _xmlEncoded = _xmlEncodedLE = _hexEncoded = "";
                    Index = -1;
                    Length = 0;
                    return;
                }
            }
            else
                Success = true;
            RawValue = capture.Value;
            Index = capture.Index;
            Length = capture.Length;
        }

        protected abstract XElement CreateElement();

        protected abstract void SetElementContent(XElement element);

        public XElement ToXElement()
        {
            XElement result = CreateElement();
            SetElementContent(result);
            return result;
        }

        public static string[] FromRawTextLines(string source)
        {
            return string.IsNullOrEmpty(source) ? new string[] { "" } : NewLineRegex.Split(source);
        }

        public static string FromEscapedText(string source)
        {
            if (string.IsNullOrEmpty(source))
                return "";
            if (UnescapeCharRegex.IsMatch(source))
                return UnescapeCharRegex.Replace(source, m =>
                {
                    if (m.Groups["x"].Success)
                        return $"{(char)int.Parse(m.Groups["x"].Value, NumberStyles.HexNumber)}";
                    if (m.Groups["u"].Success)
                        return $"{(char)int.Parse(m.Groups["u"].Value, NumberStyles.HexNumber)}";
                    return m.Groups["s"].Value[0] switch
                    {
                        'a' => "\a",
                        'f' => "\f",
                        'n' => "\n",
                        'r' => "\r",
                        't' => "\t",
                        'v' => "\v",
                        _ => "\\",
                    };
                });
            return source;
        }

        public static string[] FromEscapedTextLines(string source)
        {
            if (string.IsNullOrEmpty(source))
                return new string[] { "" };
            return NewLineRegex.Split(source).Select(FromEscapedText).ToArray();
        }

        public static string FromUriEncoded(string source)
        {
            if (string.IsNullOrEmpty(source))
                return "";
            if (UnescapeUriRegex.IsMatch(source))
                return UnescapeUriRegex.Replace(source, m =>
                {
                    try
                    {
                        return $"{(char)int.Parse(m.Groups["e"].Value, NumberStyles.HexNumber)}";
                    }
                    catch { /* ignored intentionally */ }
                    return m.Value;
                });
            return source;
        }

        public static string[] FromUriEncodedLines(string source)
        {
            if (string.IsNullOrEmpty(source))
                return new string[] { "" };
            return NewLineRegex.Split(source).Select(FromUriEncoded).ToArray();
        }

        public static string FromXmlEncoded(string source)
        {
            if (string.IsNullOrEmpty(source))
                return "";
            if (UnescapeUriRegex.IsMatch(source))
                return UnescapeXmlRegex.Replace(source, m =>
                {
                    if (m.Groups["e"].Success)
                        return m.Groups["e"].Value switch
                        {
                            "quot" => "\"",
                            "apos" => "'",
                            "lt" => "<",
                            "gt" => ">",
                            _ => "&",
                        };
                    try
                    {
                        if (m.Groups["d"].Success)
                        {
                            if (int.TryParse(m.Groups["d"].Value, out int i) && i < 0x10000)
                                return $"{(char)i}";
                        }
                        else
                            return $"{(char)int.Parse(m.Groups["x"].Value, NumberStyles.HexNumber)}";
                    }
                    catch { /* ignored intentionally */ }
                    return m.Value;
                });
            return source;
        }

        public static string[] FromXmlEncodedLines(string source)
        {
            if (string.IsNullOrEmpty(source))
                return new string[] { "" };
            return NewLineRegex.Split(source).Select(FromXmlEncoded).ToArray();
        }

        public static string ToEscapedLines(string rawValue, bool showLineEndingsAndTabs)
        {
            if (string.IsNullOrEmpty(rawValue))
                return "";
            if (showLineEndingsAndTabs)
            {
                if (EscapeCharRegexT.IsMatch(rawValue))
                    return EscapeCharRegexT.Replace(rawValue, match =>
                    {
                        Group group = match.Groups["n"];
                        if (group.Success)
                            return group.Value switch
                            {
                                "\n" => @"\n
",
                                "\r" => @"\r
",
                                "\r\n" => @"\r\n
",
                                _ => $@"\\u{(int)group.Value[0]:x4}
",
                            };
                        char c = match.Value[0];
                        switch (c)
                        {
                            case '\a':
                                return @"\a";
                            case '\f':
                                return @"\f";
                            case '\t':
                                return @"\t";
                            case '\v':
                                return @"\v";
                            case '\\':
                                return @"\\";
                            default:
                                int n = (int)c;
                                if (n < 0x0100)
                                    return $@"\x{n:x2}";
                                return $@"\u{n:x4}";
                        }
                    });
                return rawValue;
            }
            if (EscapeCharRegex.IsMatch(rawValue))
                return EscapeCharRegex.Replace(rawValue, match =>
                {
                    Group group = match.Groups["n"];
                    if (group.Success)
                        switch (group.Value)
                        {
                            case "\n":
                            case "\r":
                            case "\r\n":
                                return @"
";
                            default:
                                return $@"\\u{(int)group.Value[0]:x4}
";
                        }
                    char c = match.Value[0];
                    switch (c)
                    {
                        case '\a':
                            return @"\a";
                        case '\f':
                            return @"\f";
                        case '\v':
                            return @"\v";
                        case '\\':
                            return @"\\";
                        default:
                            int n = (int)c;
                            if (n < 0x0100)
                                return $@"\x{n:x2}";
                            return $@"\u{n:x4}";
                    }
                });
            return rawValue;
        }

        public static string ToQuotedLines(string rawValue, bool showLineEndingsAndTabs)
        {
            if (string.IsNullOrEmpty(rawValue))
                return "";
            if (showLineEndingsAndTabs)
            {
                if (QuotedCharRegexT.IsMatch(rawValue))
                    rawValue = QuotedCharRegexT.Replace(rawValue, match =>
                    {
                        Group group = match.Groups["n"];
                        if (group.Success)
                            return group.Value switch
                            {
                                "\n" => @"\n""
""",
                                "\r" => @"\r""
""",
                                "\r\n" => @"\r\n""
""",
                                _ => $@"\\u{(int)group.Value[0]:x4}""
""",
                            };
                        char c = match.Value[0];
                        switch (c)
                        {
                            case '\a':
                                return @"\a";
                            case '\f':
                                return @"\f";
                            case '\t':
                                return @"\t";
                            case '\v':
                                return @"\v";
                            case '\\':
                                return @"\\";
                            case '"':
                                return "\\\"";
                            default:
                                int n = (int)c;
                                if (n < 0x0100)
                                    return $@"\x{n:x2}";
                                return $@"\u{n:x4}";
                        }
                    });
            }
            else if (QuotedCharRegex.IsMatch(rawValue))
                rawValue = QuotedCharRegex.Replace(rawValue, match =>
                {
                    Group group = match.Groups["n"];
                    if (group.Success)
                        switch (group.Value)
                        {
                            case "\n":
                            case "\r":
                            case "\r\n":
                                return @"""
""";
                            default:
                                return $@"\\u{(int)group.Value[0]:x4}""
""";
                        }
                    char c = match.Value[0];
                    switch (c)
                    {
                        case '\a':
                            return @"\a";
                        case '\f':
                            return @"\f";
                        case '\v':
                            return @"\v";
                        case '\\':
                            return @"\\";
                        case '"':
                            return "\\\"";
                        default:
                            int n = (int)c;
                            if (n < 0x0100)
                                return $@"\x{n:x2}";
                            return $@"\u{n:x4}";
                    }
                });
            return $"\"{rawValue}\"";
        }

        public static string ToXmlEncodedLines(string rawValue, bool showLineEndingsAndTabs)
        {
            if (string.IsNullOrEmpty(rawValue))
                return "";
            if (showLineEndingsAndTabs)
            {
                if (XmlEncodeRegexT.IsMatch(rawValue))
                    return XmlEncodeRegexT.Replace(rawValue, match =>
                    {
                        if (match.Groups["n"].Success)
                            switch (match.Value)
                            {
                                case "\n":
                                    return @"&#x0A;
";
                                case "\r":
                                    return @"&#x0D;
";
                                case "\r\n":
                                    return @"&#x0D;&#x0A;
";
                                default:
                                    int n = match.Value[0];
                                    if (n > 0xff)
                                        return $@"&#x{n:x4};
";
                                    return $@"&#x{n:x2};
";
                            }
                        char c = match.Value[0];
                        switch (c)
                        {
                            case '<':
                                return @"&lt;";
                            case '>':
                                return @"&gt;";
                            case '&':
                                return @"&amp;";
                            default:
                                int n = (int)c;
                                if (n > 0xff)
                                    return $@"&#x{n:x4};";
                                return $@"&#x{n:x2};";
                        }
                    });
                return rawValue;
            }
            if (XmlEncodeRegex.IsMatch(rawValue))
                return XmlEncodeRegex.Replace(rawValue, match =>
                {
                    if (match.Groups["n"].Success)
                        switch (match.Value)
                        {
                            case "\n":
                            case "\r":
                            case "\r\n":
                                return @"
";
                            default:
                                int n = match.Value[0];
                                if (n > 0xff)
                                    return $@"&#x{n:x4};
";
                                return $@"&#x{n:x2};
";
                        }
                    char c = match.Value[0];
                    switch (c)
                    {
                        case '<':
                            return @"&lt;";
                        case '>':
                            return @"&gt;";
                        case '&':
                            return @"&amp;";
                        default:
                            int n = (int)c;
                            if (n > 0xff)
                                return $@"&#x{n:x4};";
                            return $@"&#x{n:x2};";
                    }
                });
            return rawValue;
        }

        public static string ToUriEncodedLines(string rawValue, bool showLineEndingsAndTabs)
        {
            if (string.IsNullOrEmpty(rawValue))
                return "";
            string e;
            if (showLineEndingsAndTabs)
            {
                if (UriEncodeRegexT.IsMatch(rawValue))
                    return UriEncodeRegexT.Replace(rawValue, match =>
                    {
                        if (match.Groups["n"].Success)
                            switch (match.Value)
                            {
                                case "\n":
                                    return @"%0A
";
                                case "\r":
                                    return @"%0D
";
                                case "\r\n":
                                    return @"%0D%0A
";
                                default:
                                    try { e = Uri.HexEscape(match.Value[0]); }
                                    catch
                                    {
                                        int i = match.Value[0];
                                        if (i > 0xff)
                                            e = $"%{(i >> 8):x2}%{(i & 0xff):x2}";
                                        else
                                            e = $"%{i:x2}";
                                    }
                                    return $@"{e}
";
                            }
                        try { return Uri.HexEscape(match.Value[0]); }
                        catch
                        {
                            int i = match.Value[0];
                            if (i > 0xff)
                                return $"%{(i >> 8):x2}%{(i & 0xff):x2}";
                            return $"%{i:x2}";
                        }
                    });
                return rawValue;
            }
            if (UriEncodeRegex.IsMatch(rawValue))
                return UriEncodeRegex.Replace(rawValue, match =>
                {
                    if (match.Groups["n"].Success)
                        switch (match.Value)
                        {
                            case "\n":
                            case "\r":
                            case "\r\n":
                                return @"
";
                            default:
                                try { e = Uri.HexEscape(match.Value[0]); }
                                catch
                                {
                                    int i = match.Value[0];
                                    if (i > 0xff)
                                        e = $"%{(i >> 8):x2}%{(i & 0xff):x2}";
                                    else
                                        e = $"%{i:x2}";
                                }
                                return $@"{e}
";
                        }
                    try { return Uri.HexEscape(match.Value[0]); }
                    catch
                    {
                        int i = match.Value[0];
                        if (i > 0xff)
                            return $"%{(i >> 8):x2}%{(i & 0xff):x2}";
                        return $"%{i:x2}";
                    }
                });
            return rawValue;
        }

        public static string ToHexEncoded(string rawValue)
        {
            if (rawValue == null)
                return "null";
            if (rawValue.Length == 0)
                return "";
            StringBuilder sb = new StringBuilder();
            using (CharEnumerator enumerator = rawValue.GetEnumerator())
            {
                int index = 0;
                enumerator.MoveNext();
                if (ExtendedCharRegex.IsMatch(rawValue))
                {
                    sb.Append(((int)enumerator.Current).ToString("x4"));
                    while (enumerator.MoveNext())
                    {
                        switch (++index % 32)
                        {
                            case 0:
                                sb.AppendLine().Append(((int)enumerator.Current).ToString("x4"));
                                break;
                            case 4:
                            case 12:
                            case 20:
                            case 28:
                                sb.Append("\u2011").Append(((int)enumerator.Current).ToString("x4"));
                                break;
                            case 8:
                            case 16:
                            case 24:
                                sb.Append("\u202f").Append(((int)enumerator.Current).ToString("x4"));
                                break;
                            default:
                                sb.Append(":").Append(((int)enumerator.Current).ToString("x4"));
                                break;
                        }
                    }
                }
                else
                {
                    sb.Append(((int)enumerator.Current).ToString("x2"));
                    while (enumerator.MoveNext())
                    {
                        switch (++index % 64)
                        {
                            case 0:
                                sb.AppendLine().Append(((int)enumerator.Current).ToString("x2"));
                                break;
                            case 4:
                            case 8:
                            case 12:
                            case 20:
                            case 24:
                            case 28:
                            case 36:
                            case 40:
                            case 44:
                            case 52:
                            case 56:
                            case 60:
                                sb.Append("\u2011").Append(((int)enumerator.Current).ToString("x2"));
                                break;
                            case 16:
                            case 32:
                            case 48:
                                sb.Append("\u202f").Append(((int)enumerator.Current).ToString("x2"));
                                break;
                            default:
                                sb.Append(":").Append(((int)enumerator.Current).ToString("x2"));
                                break;
                        }
                    }
                }
            }
            return sb.ToString();
        }
    }
}

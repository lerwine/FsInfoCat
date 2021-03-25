using System;
using System.Xml.Linq;

namespace DevHelperGUI
{
    public class LineResult
    {
        public string RawValue { get; }
        public string UriEncodedValue { get; }
        public string XmlEncodedValue { get; }
        public int LineNumber { get; }

        public LineResult(string rawValue, int lineNumber)
        {
            RawValue = rawValue;
            UriEncodedValue = Uri.EscapeUriString(rawValue);
            XmlEncodedValue = new XAttribute("A", rawValue).ToString().Substring(2);
            LineNumber = lineNumber;
        }
    }
}

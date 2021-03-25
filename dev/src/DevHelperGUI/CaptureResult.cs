using System;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace DevHelperGUI
{
    public class CaptureResult
    {
        public bool Success { get; }
        public string RawValue { get; }
        public string UriEncodedValue { get; }
        public string XmlEncodedValue { get; }
        public int Index { get; }
        public int Length { get; }

        public CaptureResult(Capture capture)
        {
            if (capture is Group group)
            {
                Success = group.Success;
                if (!Success)
                {
                    RawValue = UriEncodedValue = XmlEncodedValue = "";
                    Index = -1;
                    Length = 0;
                    return;
                }
            }
            else
                Success = true;
            RawValue = capture.Value;
            UriEncodedValue = Uri.EscapeUriString(RawValue);
            XmlEncodedValue = new XAttribute("A", RawValue).ToString().Substring(2);
            Index = capture.Index;
            Length = capture.Length;
        }

    }
}

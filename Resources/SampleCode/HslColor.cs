using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Management.Automation;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Media;

namespace Erwine.Leonard.T.PSWebSrv
{
    [StructLayout(LayoutKind.Explicit)]
    public struct HslColor : IEquatable<HslColor>, IComparable<HslColor>, IComparable
    {
        public static readonly Regex HtmlHexRegex = new Regex(@"^\#?((?<a>[a-f\d]{2})(?<r>[a-f\d]{2})(?<g>[a-f\d]{2})(?<b>[a-f\d]{2})|(?<r>[a-f\d]{2})(?<g>[a-f\d]{2})(?<b>[a-f\d]{2})|(?<r>[a-f\d])(?<g>[a-f\d])(?<b>[a-f\d]))$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static readonly Regex CssRgbRegex = new Regex(@"^(rgb\((?<r>0*(2(5[1-5]|0-4\d)|1\d\d|\d\d?)|0+),\s*(?<g>0*(2(5[1-5]|0-4\d)|1\d\d|\d\d?)|0+),\s*(?<b>0*(2(5[1-5]|0-4\d)|1\d\d|\d\d?)|0+)\)|rgba\((?<r>0*(2(5[1-5]|0-4\d)|1\d\d|\d\d?)|0+),\s*(?<g>0*(2(5[1-5]|0-4\d)|1\d\d|\d\d?)|0+),\s*(?<b>0*(2(5[1-5]|0-4\d)|1\d\d|\d\d?)|0+),\s*(?<a>(0*1|0+)?\.\d+|0*1|0+)\))$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static readonly Regex CssHslRegex = new Regex(@"^(hsl\((?<h>0*(3(60|5\d)|[12]\d\d|\d\d?)|0+),\s*((?<s>(0*(100|\d\d?)|0+))\s*%),\s*((?<l>(0*(100|\d\d?)|0+))\s*%)\)|hsla\((?<h>0*(3(60|5\d)|[12]\d\d|\d\d?)|0+),\s*((?<s>(0*(100|\d\d?)|0+))\s*%),\s*((?<l>(0*(100|\d\d?)|0+))\s*%),\s*(?<a>(0*1|0+)?\.\d+|0*1|0+)\))$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static readonly Regex ByteValueRegex = new Regex(@"0*(2(5[1-5]|0-4\d)|1\d\d|\d\d?)|0+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static readonly Regex HueValueRegex = new Regex(@"0*(3(60|5\d)|[12]\d\d|\d\d?)|0+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static readonly Regex PercentvalueRegex = new Regex(@"(0*(100|\d\d?)|0+)\s*%", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static readonly Regex DecimalValueRegex = new Regex(@"(0*1|0+)?\.\d+|0*1|0+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        
        #region Fields

        [FieldOffset(0)]
        private int _hashCode;
        [FieldOffset(0)]
        private byte _h;
        [FieldOffset(1)]
        private byte _s;
        [FieldOffset(2)]
        private byte _l;
        [FieldOffset(3)]
        private byte _a;
        
        #endregion

        #region Properties

        public byte A { get { return _a; } }
        
        public byte H { get { return _h; } }
        
        public byte S { get { return _s; } }
        
        public byte L { get { return _l; } }
        
        #endregion

        #region Constructors
        
        public HslColor(byte a, byte h, byte s, byte l)
        {
            _hashCode = 0;
            _a = a;
            _h = h;
            _s = s;
            _l = l;
        }
        
        public HslColor(byte h, byte s, byte l)
        {
            _hashCode = 0;
            _a = 255;
            _h = h;
            _s = s;
            _l = l;
        }

        public HslColor(HslColorF color)
        {
            _hashCode = 0;
            _a = Convert.ToByte(color.A * 255.0);
            _h = Convert.ToByte(color.H * 255.0);
            _s = Convert.ToByte(color.S * 255.0);
            _l = Convert.ToByte(color.L * 255.0);
        }

        public HslColor(RgbColorF color)
        {
            _hashCode = 0;
            double h, s, l;
            RgbColorF.RgbToHsl(color.R, color.G, color.B, out h, out s, out l);
            _h = Convert.ToByte(h * 255.0);
            _s = Convert.ToByte(s * 255.0);
            _l = Convert.ToByte(l * 255.0);
            _a = Convert.ToByte(color.A * 255.0);
        }
        
        public HslColor(Color color)
        {
            _hashCode = 0;
            double h, s, l;
            RgbColorF.RgbToHsl(Convert.ToDouble(color.R) / 255.0, Convert.ToDouble(color.G) / 255.0, Convert.ToDouble(color.B) / 255.0, out h, out s, out l);
            _h = Convert.ToByte(h * 255.0);
            _s = Convert.ToByte(s * 255.0);
            _l = Convert.ToByte(l * 255.0);
            _a = color.A;
        }

        #endregion

        public static bool TryParse(string text, out HslColor color)
        {
            if (String.IsNullOrEmpty(text))
            {
                color = default(HslColor);
                return false;
            }

            double h, s, l;
            byte? a;
            Match match = CssHslRegex.Match(text);
            if (match.Success)
            {
                h = (Convert.ToDouble(int.Parse(match.Groups["h"].Value)) / 360.0) * 255.0;
                s = Convert.ToDouble(int.Parse(match.Groups["s"].Value)) * 2.55;
                l = Convert.ToDouble(int.Parse(match.Groups["l"].Value)) * 2.55;
                a = (match.Groups["a"].Success) ? Convert.ToByte(double.Parse(match.Groups["a"].Value) * 255.0) as byte? : null;
            }
            else if ((match = HtmlHexRegex.Match(text)).Success)
            {
                if (match.Groups["r"].Length == 1)
                {
                    RgbColorF.RgbToHsl(Convert.ToDouble(int.Parse(match.Groups["r"].Value, NumberStyles.HexNumber) * 0x11),
                        Convert.ToDouble(int.Parse(match.Groups["g"].Value, NumberStyles.HexNumber) * 0x11),
                        Convert.ToDouble(int.Parse(match.Groups["b"].Value, NumberStyles.HexNumber) * 0x11), out h, out s, out l);
                }
                else
                {
                    RgbColorF.RgbToHsl(Convert.ToDouble(int.Parse(match.Groups["r"].Value, NumberStyles.HexNumber)),
                        Convert.ToDouble(int.Parse(match.Groups["g"].Value, NumberStyles.HexNumber)),
                        Convert.ToDouble(int.Parse(match.Groups["b"].Value, NumberStyles.HexNumber)), out h, out s, out l);
                }
                
                a = (match.Groups["a"].Success) ? byte.Parse(match.Groups["a"].Value, NumberStyles.HexNumber) as byte? : null;
            }
            else if ((match = CssRgbRegex.Match(text)).Success)
            {
                RgbColorF.RgbToHsl(Convert.ToDouble(int.Parse(match.Groups["r"].Value)),
                    Convert.ToDouble(int.Parse(match.Groups["g"].Value)),
                    Convert.ToDouble(int.Parse(match.Groups["b"].Value)), out h, out s, out l);
                a = (match.Groups["a"].Success) ? Convert.ToByte(double.Parse(match.Groups["a"].Value) * 255.0) as byte? : null;
            }
            else
            {
                color = default(HslColor);
                return false;
            }

            if (a.HasValue)
                color = new HslColor(a.Value, Convert.ToByte(h), Convert.ToByte(s), Convert.ToByte(l));
            else
                color = new HslColor(Convert.ToByte(h), Convert.ToByte(s), Convert.ToByte(l));
            return true;
        }

        public bool Equals(HslColor other) { return _hashCode == other._hashCode; }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(HslColor other)
        {
            if (_hashCode == other._hashCode)
                return 0;

            if (_hashCode == 0)
                return -1;

            if (other._hashCode == 0)
                return 1;

            int i = _a.CompareTo(other._a);
            if (i == 0 && (i = _l.CompareTo(other._l)) == 0 && (i = _s.CompareTo(other._s)) == 0)
                return _h.CompareTo(other._h);
            return i;
        }

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode() { return _hashCode; }

        public override string ToString()
        {
            return "#" + A.ToString("x2") + H.ToString("x2") + S.ToString("x2") + L.ToString("x2");
        }
    }
}
Add-Type -TypeDefinition @'
namespace ColorHelper
{
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
    public struct HslColorF : IEquatable<HslColorF>, IComparable<HslColorF>, IComparable
    {
        private double _a;
        private double _h;
        private double _s;
        private double _l;
        
        public double A { get { return _a; } }
        
        public double H { get { return _h; } }
        
        public double S { get { return _s; } }
        
        public double L { get { return _l; } }
        
        public HslColorF(double a, double h, double s, double l)
        {
            if (a < 0 || a > 1.0)
                throw new ArgumentOutOfRangeException("a", "Alpha must be a value from 0.0 to 1.0");
            AssertValidHsl(h, s, l);
            _a = a;
            _h = h;
            _s = s;
            _l = l;
        }
        
        public HslColorF(double h, double s, double l)
        {
            AssertValidHsl(h, s, l);
            _a = 1.0;
            _h = h;
            _s = s;
            _l = l;
        }

        public HslColorF(HslColor color)
        {
            _a = Convert.ToDouble(color.A);
            _h = Convert.ToDouble(color.H);
            _s = Convert.ToDouble(color.S);
            _l = Convert.ToDouble(color.L);
        }

        public HslColorF(RgbColorF color)
        {
            _a = color.A;
            double h, s, l;
            RgbColorF.RgbToHsl(color.R, color.G, color.B, out h, out s, out l);
            _h = h;
            _s = s;
            _l = l;
        }
        
        public HslColorF(Color color)
        {
            double h, s, l;
            RgbColorF.RgbToHsl(Convert.ToDouble(color.R) / 255.0, Convert.ToDouble(color.G) / 255.0, Convert.ToDouble(color.B) / 255.0,
                out h, out s, out l);
            _h = h;
            _s = s;
            _l = l;
            _a = Convert.ToDouble(color.A) / 255.0;
        }
        
        public static void HslToRgb(double h, double s, double l, out double r, out double g, out double b)
        {
            AssertValidHsl(h, s, l);
            if (s == 0.0)
            {
                r = g = b = l;
                return;
            }

            var q = l < 0.5 ? l * (1.0 + s) : l + s - l * s;
            var p = 2.0 * l - q;
            Func<double, double> getValue = v =>
            {
                double a = (v < 0.0) ? v + 1.0 : v;
                if (a > 1.0)
                    a -= 1.0;
                if (a < 1.0 / 6.0)
                    return p + (q - p) * 6.0 * a;
                if (a < 1.0 / 2.0)
                    return q;
                if (a < 2.0 / 3.0)
                    return p + (q - p) * (2.0 / 3.0 - a) * 6.0;
                return p;
            };
            r = getValue(h + 1.0 / 3.0);
            g = getValue(h);
            b = getValue(h - 1.0 / 3.0);
        }

        private static void AssertValidHsl(double h, double s, double l)
        {
            if (h < 0 || h > 1.0)
                throw new ArgumentOutOfRangeException("h", "Hue must be a value from 0.0 to 1.0");
            if (s < 0 || s > 1.0)
                throw new ArgumentOutOfRangeException("s", "Saturation must be a value from 0.0 to 1.0");
            if (l < 0 || l > 1.0)
                throw new ArgumentOutOfRangeException("l", "Lightness must be a value from 0.0 to 1.0");
        }

        public const string AhslDesignatorString = "AHSL(";
        
        public bool Equals(HslColorF other)
        {
            return _a == other._a && _h == other._h && _s == other._s && _l == other._l;
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(HslColorF other)
        {
            int i = _a.CompareTo(other._a);
            if (i == 0 && (i = _l.CompareTo(other._l)) == 0 && (i = _s.CompareTo(other._s)) == 0)
                return _h.CompareTo(other._h);
            return i;
        }

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return AhslDesignatorString + (A * 100).ToString() + "%," + (H * 100).ToString() + "%," + (S * 100).ToString() + "%," + 
                (L * 100).ToString() + "%)";
        }
    }
    public struct RgbColorF : IEquatable<RgbColorF>, IComparable<RgbColorF>, IComparable
    {
        private double _a;
        private double _r;
        private double _g;
        private double _b;
        
        public double A { get { return _a; } }
        
        public double R { get { return _r; } }
        
        public double G { get { return _g; } }
        
        public double B { get { return _b; } }

        public RgbColorF(double a, double r, double g, double b)
        {
            if (a < 0 || a > 1.0)
                throw new ArgumentOutOfRangeException("a", "Alpha must be a value from 0.0 to 1.0");
            AssertValidRgb(r, g, b);
            _a = a;
            _r = r;
            _g = g;
            _b = b;
        }
        
        public RgbColorF(double r, double g, double b)
        {
            AssertValidRgb(r, g, b);
            _a = 1.0;
            _r = r;
            _g = g;
            _b = b;
        }

        public RgbColorF(HslColorF color)
        {
            double r, g, b;
            HslColorF.HslToRgb(color.H, color.S, color.L, out r, out g, out b);
            _r = r;
            _g = g;
            _b = b;
            _a = color.A;
        }

        public RgbColorF(HslColor color)
        {
            double r, g, b;
            
            HslColorF.HslToRgb(Convert.ToDouble(color.H) * 255.0, Convert.ToDouble(color.S) * 255.0, Convert.ToDouble(color.L) * 255.0, out r, out g, out b);
            _r = r;
            _g = g;
            _b = b;
            _a = Convert.ToDouble(color.A) * 255.0;
        }
        
        public RgbColorF(Color color)
        {
            _a = Convert.ToDouble(color.A) / 255.0;
            _r = Convert.ToDouble(color.R) / 255.0;
            _g = Convert.ToDouble(color.G) / 255.0;
            _b = Convert.ToDouble(color.B) / 255.0;
        }
        
        public RgbColorF(double value)
        {
            throw new NotImplementedException();
        }

        public Color ToColor()
        {
            return Color.FromArgb(Convert.ToByte(_a * 255.0), Convert.ToByte(_r * 255.0), Convert.ToByte(_g * 255.0), Convert.ToByte(_b * 255.0));
        }

        public static void RgbToHsl(double r, double g, double b, out double h, out double s, out double l)
        {
            double max = Math.Max(r, Math.Max(g, b));
            double min = Math.Min(r, Math.Min(g, b));

            h = s = l = (max + min) / 2.0;

            if (max == min)
            {
                h = s = 0.0;
                return;
            }

            double d = max - min;
            s = (l > 0.5) ? d / (2.0 - max - min) : d / (max + min);

            if (r > g && r > b)
                h = (g - b) / d + (g < b ? 6.0 : 0.0);

            else if (g > b)
                h = (b - r) / d + 2.0;

            else
                h = (r - g) / d + 4.0;

            h /= 6.0;
        }

        private static void AssertValidRgb(double r, double g, double b)
        {
            if (r < 0 || r > 1.0)
                throw new ArgumentOutOfRangeException("r", "Red must be a value from 0.0 to 1.0");
            if (g < 0 || g > 1.0)
                throw new ArgumentOutOfRangeException("g", "Green must be a value from 0.0 to 1.0");
            if (b < 0 || b > 1.0)
                throw new ArgumentOutOfRangeException("b", "Blue must be a value from 0.0 to 1.0");
        }

        public bool Equals(RgbColorF other)
        {
            return _a == other._a && _r == other._r && _g == other._g && _b == other._b;
        }

        private const string ArgbDesignatorString = "ARGB(";

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is PSObject)
                obj = (obj as PSObject).BaseObject;
            if (obj is RgbColorF)
                return Equals((RgbColorF)obj);
            if (obj is Color)
            {
                Color color = (Color)obj;
                return Convert.ToByte(_a * 255.0) == color.A && Convert.ToByte(_r * 255.0) == color.R &&
                    Convert.ToByte(_g * 255.0) == color.G && Convert.ToByte(_b * 255.0) == color.B;
            }

            if (obj is HslColorF)
                return Equals(new RgbColorF((HslColorF)obj));
            
            if (obj is HslColor)
                return Equals(new RgbColorF((HslColor)obj));
            
            if (obj is double)
                return ToDouble().Equals((double)obj);

            if (obj is string)
            {
                string s = obj as string;
                if (s.Length == 0)
                    return false;
                int l = ArgbDesignatorString.Length;
                if (!s.StartsWith(ArgbDesignatorString))
                    s = ArgbDesignatorString + s + ")";
                    
                return String.Equals(ToString(), s, StringComparison.InvariantCultureIgnoreCase);
            }
            
            return false;
        }

        public int CompareTo(RgbColorF other)
        {
            int i = _a.CompareTo(other._a);
            if (i != 0 ||
                    (i = ((_r * _g * _b) / 3.0).CompareTo((other._r * other._g * other._b) / 3.0)) != 0 ||
                    (Math.Max(_r, Math.Max(_g, _b)).CompareTo(Math.Max(other._r, Math.Max(other._g, other._b)))) != 0 ||
                    (Math.Min(_r, Math.Min(_g, _b)).CompareTo(Math.Min(other._r, Math.Min(other._g, other._b)))) != 0 ||
                    (i = _r.CompareTo(other._r)) != 0 || (i = _g.CompareTo(other._g)) != 0)
                return i;
            
            return _b.CompareTo(other._b);
        }

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode() { return ToDouble().GetHashCode(); }

        public override string ToString()
        {
            throw new NotImplementedException();
        }

        public double ToDouble() { return (A * 8.0) + (R * 4.0) + (G * 2.0) + B; }
    }
}
'@ -ReferencedAssemblies 'PresentationCore' -ErrorAction Stop;

$Xml = [System.Xml.XmlDocument]::new();
$Xml.PreserveWhitespace = $true;
[Xml]$Xml = [System.Windows.Clipboard]::GetText();
@($Xml.SelectNodes('//GeometryDrawing/@Brush')) | ForEach-Object {
    $HslColorF = [ColorHelper.HslColorF]::new([System.Windows.Media.Color]::FromArgb([byte]::Parse($_.Value.Substring(1, 2), [System.Globalization.NumberStyles]::HexNumber), [byte]::Parse($_.Value.Substring(3, 2), [System.Globalization.NumberStyles]::HexNumber), [byte]::Parse($_.Value.Substring(5, 2), [System.Globalization.NumberStyles]::HexNumber), [byte]::Parse($_.Value.Substring(7), [System.Globalization.NumberStyles]::HexNumber)));
    $HslColorF = [ColorHelper.HslColorF]::new($HslColorF.A, $HslColorF.H, ($HslColorF.S / 2.0), ($HslColorF.L / 2.0));
    $RgbColorF = [ColorHelper.RgbColorF]::new($HslColorF);
    $_.Value = $RgbColorF.ToColor().ToString().ToUpper();
}
[System.Windows.Clipboard]::SetText($Xml.OuterXml)
#[System.Windows.Clipboard]::SetText($RgbColorF.ToColor().ToString().Substring(1).ToUpper());
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Net;
using System.Windows.Media;

namespace Erwine.Leonard.T.PSWebSrv
{
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
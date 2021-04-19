using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Net;
using System.Windows.Media;

namespace Erwine.Leonard.T.PSWebSrv
{
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
}
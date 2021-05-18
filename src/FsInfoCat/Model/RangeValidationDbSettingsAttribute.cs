using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FsInfoCat.Model
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class RangeValidationDbSettingsAttribute : DbSettingsAttribute
    {
        /// <summary>
        /// Gets name of the application setting within <see cref="DBSettings"/> to reference as the minimum validation range value.
        /// </summary>
        /// <value>
        /// The name of the application setting from <see cref="DBSettings"/> to use for the minimum range value or <see langword="null"/> if there is
        /// no minimum range value.
        /// </value>
        public string MinValueSettingsName { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the minimum range value is exclusive.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the minimum range value is exclusive; otherwise, <see langword="false"/> to indicate that it is inclusive.
        /// <para>The default value is <see langword="false"/> (minimum range is inclusive).</para>
        /// </value>
        public bool MinimumIsExclusive { get; set; }

        /// <summary>
        /// Gets name of the application setting within <see cref="DBSettings"/> to reference as the maximum validation range value.
        /// </summary>
        /// <value>
        /// The name of the application setting from <see cref="DBSettings"/> to use for the maximum range value or <see langword="null"/> if there is
        /// no maximum range value.
        /// </value>
        public string MaxValueSettingsName { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the maximum range value is exclusive.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the maximum range value is exclusive; otherwise, <see langword="false"/> to indicate that it is inclusive.
        /// <para>The default value is <see langword="false"/> (maximum range is inclusive).</para>
        /// </value>
        public bool MaximumIsExclusive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a <see langword="null"/> value will cause the validation to fail.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if <see langword="null"/> values cause validations to fail; otherwise, <see langword="false"/> if null values are ignored by this
        /// <see cref="RangeValidationDbSettingsAttribute"/>.
        /// </value>
        public bool NullIsInvalid { get; set; }

        public RangeValidationDbSettingsAttribute(string settingsName) : this(settingsName, false) { }

        public RangeValidationDbSettingsAttribute(string settingsName, bool isMinLength)
        {
            if (isMinLength)
                MinValueSettingsName = string.IsNullOrWhiteSpace(settingsName) ? "" : settingsName;
            else
                MaxValueSettingsName = string.IsNullOrWhiteSpace(settingsName) ? "" : settingsName;
        }

        public RangeValidationDbSettingsAttribute(string minValueSettingsName, string maxValueSettingsName)
        {
            MinValueSettingsName = string.IsNullOrWhiteSpace(minValueSettingsName) ? "" : minValueSettingsName;
            MaxValueSettingsName = string.IsNullOrWhiteSpace(maxValueSettingsName) ? "" : maxValueSettingsName;
        }

        private static IEnumerable<string> GetMemberNames(ValidationContext validationContext)
        {
            if (!(validationContext is null || string.IsNullOrWhiteSpace(validationContext.MemberName)))
                yield return validationContext.MemberName;
        }

        public override bool IsValid(object value)
        {
            if (value is null)
                return !NullIsInvalid;

            object minValue, maxValue;
            try { minValue = string.IsNullOrEmpty(MinValueSettingsName) ? null : GetRawSettingsValue(MinValueSettingsName); }
            catch (Exception exc)
            {
                throw new ValidationException($"Error getting minimum length value from {nameof(DBSettings)}: {(string.IsNullOrWhiteSpace(exc.Message) ? exc.ToString() : exc.Message)}", exc);
            }
            try { maxValue = string.IsNullOrEmpty(MinValueSettingsName) ? null : GetRawSettingsValue(MaxValueSettingsName); }
            catch (Exception exc)
            {
                throw new ValidationException($"Error getting maximum length value from {nameof(DBSettings)}: {(string.IsNullOrWhiteSpace(exc.Message) ? exc.ToString() : exc.Message)}", exc);
            }
            if (minValue is null && maxValue is null)
                return true;
            if (TryConvertToSimpleType(value, out TypeCode valueTypeCode, out object simplifiedValue))
            {
                switch (valueTypeCode)
                {
                    case TypeCode.Boolean:
                        throw new ValidationException("Cannot evaluate boolean values.");
                    case TypeCode.Empty:
                    case TypeCode.DBNull:
                        return !NullIsInvalid;
                    case TypeCode.Byte:
                        byte? bx, by;
                        try
                        {
                            bx = ToByteRangeValue(minValue);
                        }
                        catch (Exception exc)
                        {
                            throw new ValidationException($"Cannot compare {nameof(MinValueSettingsName)} {minValue.GetType().FullName} with byte values.", exc);
                        }
                        try
                        {
                            by = ToByteRangeValue(maxValue);
                        }
                        catch (Exception exc)
                        {
                            throw new ValidationException($"Cannot compare {nameof(MaxValueSettingsName)} {maxValue.GetType().FullName} with byte values.", exc);
                        }
                        byte bv = Convert.ToByte(value);
                        return (!bx.HasValue || (MinimumIsExclusive ? bv > bx.Value : bv >= bx.Value)) &&
                            (!by.HasValue || (MinimumIsExclusive ? bv < by.Value : bv <= by.Value));
                    case TypeCode.Char:
                        char? cx, cy;
                        try
                        {
                            cx = ToCharRangeValue(minValue);
                        }
                        catch (Exception exc)
                        {
                            throw new ValidationException($"Cannot compare {nameof(MinValueSettingsName)} {minValue.GetType().FullName} with char values.", exc);
                        }
                        try
                        {
                            cy = ToCharRangeValue(maxValue);
                        }
                        catch (Exception exc)
                        {
                            throw new ValidationException($"Cannot compare {nameof(MaxValueSettingsName)} {maxValue.GetType().FullName} with char values.", exc);
                        }
                        char c = Convert.ToChar(value);
                        return (!cx.HasValue || (MinimumIsExclusive ? c > cx.Value : c >= cx.Value)) &&
                            (!cy.HasValue || (MinimumIsExclusive ? c < cy.Value : c <= cy.Value));
                    case TypeCode.DateTime:
                        DateTime? dtx, dty;
                        try
                        {
                            TryConvertToSimpleType(minValue, out TypeCode _, out object o);
                            dtx = o is DateTime dtv ? dtv : Convert.ToDateTime(o);
                        }
                        catch (Exception exc)
                        {
                            throw new ValidationException($"Cannot compare {nameof(MinValueSettingsName)} {minValue.GetType().FullName} with DateTime values.", exc);
                        }
                        try
                        {
                            TryConvertToSimpleType(maxValue, out TypeCode _, out object o);
                            dty = o is DateTime dtv ? dtv : Convert.ToDateTime(o);
                        }
                        catch (Exception exc)
                        {
                            throw new ValidationException($"Cannot compare {nameof(MaxValueSettingsName)} {maxValue.GetType().FullName} with DateTime values.", exc);
                        }
                        DateTime dt = Convert.ToDateTime(value);
                        return (!dtx.HasValue || (MinimumIsExclusive ? dt > dtx.Value : dt >= dtx.Value)) &&
                            (!dty.HasValue || (MinimumIsExclusive ? dt < dty.Value : dt <= dty.Value));
                    case TypeCode.Decimal:
                        decimal? mx, my;
                        try
                        {
                            mx = ToDecimalRangeValue(minValue);
                        }
                        catch (Exception exc)
                        {
                            throw new ValidationException($"Cannot compare {nameof(MinValueSettingsName)} {minValue.GetType().FullName} with decimal values.", exc);
                        }
                        try
                        {
                            my = ToDecimalRangeValue(maxValue);
                        }
                        catch (Exception exc)
                        {
                            throw new ValidationException($"Cannot compare {nameof(MaxValueSettingsName)} {maxValue.GetType().FullName} with decimal values.", exc);
                        }
                        decimal m = Convert.ToDecimal(value);
                        return (!mx.HasValue || (MinimumIsExclusive ? m > mx.Value : m >= mx.Value)) &&
                            (!my.HasValue || (MinimumIsExclusive ? m < my.Value : m <= my.Value));
                    case TypeCode.Double:
                        double? dx, dy;
                        try
                        {
                            dx = ToDoubleRangeValue(minValue);
                        }
                        catch (Exception exc)
                        {
                            throw new ValidationException($"Cannot compare {nameof(MinValueSettingsName)} {minValue.GetType().FullName} with decimal values.", exc);
                        }
                        try
                        {
                            dy = ToDoubleRangeValue(maxValue);
                        }
                        catch (Exception exc)
                        {
                            throw new ValidationException($"Cannot compare {nameof(MaxValueSettingsName)} {maxValue.GetType().FullName} with decimal values.", exc);
                        }
                        double d = Convert.ToDouble(value);
                        return (!dx.HasValue || (MinimumIsExclusive ? d > dx.Value : d >= dx.Value)) &&
                            (!dy.HasValue || (MinimumIsExclusive ? d < dy.Value : d <= dy.Value));
                    case TypeCode.Int16:
                        short? svx, svy;
                        try
                        {
                            svx = ToInt16RangeValue(minValue);
                        }
                        catch (Exception exc)
                        {
                            throw new ValidationException($"Cannot compare {nameof(MinValueSettingsName)} {minValue.GetType().FullName} with decimal values.", exc);
                        }
                        try
                        {
                            svy = ToInt16RangeValue(maxValue);
                        }
                        catch (Exception exc)
                        {
                            throw new ValidationException($"Cannot compare {nameof(MaxValueSettingsName)} {maxValue.GetType().FullName} with decimal values.", exc);
                        }
                        short sv = Convert.ToInt16(value);
                        return (!svx.HasValue || (MinimumIsExclusive ? sv > svx.Value : sv >= svx.Value)) &&
                            (!svy.HasValue || (MinimumIsExclusive ? sv < svy.Value : sv <= svy.Value));
                    case TypeCode.Int32:
                        int? ix, iy;
                        try
                        {
                            ix = ToInt32RangeValue(minValue);
                        }
                        catch (Exception exc)
                        {
                            throw new ValidationException($"Cannot compare {nameof(MinValueSettingsName)} {minValue.GetType().FullName} with decimal values.", exc);
                        }
                        try
                        {
                            iy = ToInt32RangeValue(maxValue);
                        }
                        catch (Exception exc)
                        {
                            throw new ValidationException($"Cannot compare {nameof(MaxValueSettingsName)} {maxValue.GetType().FullName} with decimal values.", exc);
                        }
                        int i = Convert.ToInt32(value);
                        return (!ix.HasValue || (MinimumIsExclusive ? i > ix.Value : i >= ix.Value)) &&
                            (!iy.HasValue || (MinimumIsExclusive ? i < iy.Value : i <= iy.Value));
                    case TypeCode.Int64:
                        long? lvx, lvy;
                        try
                        {
                            lvx = ToInt64RangeValue(minValue);
                        }
                        catch (Exception exc)
                        {
                            throw new ValidationException($"Cannot compare {nameof(MinValueSettingsName)} {minValue.GetType().FullName} with decimal values.", exc);
                        }
                        try
                        {
                            lvy = ToInt64RangeValue(maxValue);
                        }
                        catch (Exception exc)
                        {
                            throw new ValidationException($"Cannot compare {nameof(MaxValueSettingsName)} {maxValue.GetType().FullName} with decimal values.", exc);
                        }
                        long lv = Convert.ToInt64(value);
                        return (!lvx.HasValue || (MinimumIsExclusive ? lv > lvx.Value : lv >= lvx.Value)) &&
                            (!lvy.HasValue || (MinimumIsExclusive ? lv < lvy.Value : lv <= lvy.Value));
                    case TypeCode.SByte:
                        sbyte? sbx, sby;
                        try
                        {
                            sbx = ToSByteRangeValue(minValue);
                        }
                        catch (Exception exc)
                        {
                            throw new ValidationException($"Cannot compare {nameof(MinValueSettingsName)} {minValue.GetType().FullName} with decimal values.", exc);
                        }
                        try
                        {
                            sby = ToSByteRangeValue(maxValue);
                        }
                        catch (Exception exc)
                        {
                            throw new ValidationException($"Cannot compare {nameof(MaxValueSettingsName)} {maxValue.GetType().FullName} with decimal values.", exc);
                        }
                        sbyte sb = Convert.ToSByte(value);
                        return (!sbx.HasValue || (MinimumIsExclusive ? sb > sbx.Value : sb >= sbx.Value)) &&
                            (!sby.HasValue || (MinimumIsExclusive ? sb < sby.Value : sb <= sby.Value));
                    case TypeCode.Single:
                        float? fx, fy;
                        try
                        {
                            fx = ToSingleRangeValue(minValue);
                        }
                        catch (Exception exc)
                        {
                            throw new ValidationException($"Cannot compare {nameof(MinValueSettingsName)} {minValue.GetType().FullName} with decimal values.", exc);
                        }
                        try
                        {
                            fy = ToSingleRangeValue(maxValue);
                        }
                        catch (Exception exc)
                        {
                            throw new ValidationException($"Cannot compare {nameof(MaxValueSettingsName)} {maxValue.GetType().FullName} with decimal values.", exc);
                        }
                        float f = Convert.ToSingle(value);
                        return (!fx.HasValue || (MinimumIsExclusive ? f > fx.Value : f >= fx.Value)) &&
                            (!fy.HasValue || (MinimumIsExclusive ? f < fy.Value : f <= fy.Value));
                    case TypeCode.String:
                        string tx, ty;
                        try
                        {
                            TryConvertToSimpleType(minValue, out TypeCode _, out object o);
                            tx = o is string tv ? tv : Convert.ToString(o);
                        }
                        catch (Exception exc)
                        {
                            throw new ValidationException($"Cannot compare {nameof(MinValueSettingsName)} {minValue.GetType().FullName} with DateTime values.", exc);
                        }
                        try
                        {
                            TryConvertToSimpleType(maxValue, out TypeCode _, out object o);
                            ty = o is string tv ? tv : Convert.ToString(o);
                        }
                        catch (Exception exc)
                        {
                            throw new ValidationException($"Cannot compare {nameof(MaxValueSettingsName)} {maxValue.GetType().FullName} with DateTime values.", exc);
                        }
                        string t = Convert.ToString(value);
                        return (tx is null || (MinimumIsExclusive ? t.CompareTo(tx) > 0 : t.CompareTo(tx) >= 0)) &&
                            (ty is null || (MinimumIsExclusive ? t.CompareTo(ty) < 0 : t.CompareTo(ty) <= 0));
                    case TypeCode.UInt16:
                        ushort? usx, usy;
                        try
                        {
                            usx = ToUInt16RangeValue(minValue);
                        }
                        catch (Exception exc)
                        {
                            throw new ValidationException($"Cannot compare {nameof(MinValueSettingsName)} {minValue.GetType().FullName} with decimal values.", exc);
                        }
                        try
                        {
                            usy = ToUInt16RangeValue(maxValue);
                        }
                        catch (Exception exc)
                        {
                            throw new ValidationException($"Cannot compare {nameof(MaxValueSettingsName)} {maxValue.GetType().FullName} with decimal values.", exc);
                        }
                        ushort us = Convert.ToUInt16(value);
                        return (!usx.HasValue || (MinimumIsExclusive ? us > usx.Value : us >= usx.Value)) &&
                            (!usy.HasValue || (MinimumIsExclusive ? us < usy.Value : us <= usy.Value));
                    case TypeCode.UInt32:
                        uint? ux, uy;
                        try
                        {
                            ux = ToUInt32RangeValue(minValue);
                        }
                        catch (Exception exc)
                        {
                            throw new ValidationException($"Cannot compare {nameof(MinValueSettingsName)} {minValue.GetType().FullName} with decimal values.", exc);
                        }
                        try
                        {
                            uy = ToUInt32RangeValue(maxValue);
                        }
                        catch (Exception exc)
                        {
                            throw new ValidationException($"Cannot compare {nameof(MaxValueSettingsName)} {maxValue.GetType().FullName} with decimal values.", exc);
                        }
                        uint u = Convert.ToUInt32(value);
                        return (!ux.HasValue || (MinimumIsExclusive ? u > ux.Value : u >= ux.Value)) &&
                            (!uy.HasValue || (MinimumIsExclusive ? u < uy.Value : u <= uy.Value));
                    case TypeCode.UInt64:
                        ulong? ulx, uly;
                        try
                        {
                            ulx = ToUInt64RangeValue(minValue);
                        }
                        catch (Exception exc)
                        {
                            throw new ValidationException($"Cannot compare {nameof(MinValueSettingsName)} {minValue.GetType().FullName} with decimal values.", exc);
                        }
                        try
                        {
                            uly = ToUInt64RangeValue(maxValue);
                        }
                        catch (Exception exc)
                        {
                            throw new ValidationException($"Cannot compare {nameof(MaxValueSettingsName)} {maxValue.GetType().FullName} with decimal values.", exc);
                        }
                        ulong ul = Convert.ToUInt64(value);
                        return (!ulx.HasValue || (MinimumIsExclusive ? ul > ulx.Value : ul >= ulx.Value)) &&
                            (!uly.HasValue || (MinimumIsExclusive ? ul < uly.Value : ul <= uly.Value));
                }
            }

            Type a = value.GetType();
            int? result;
            Type x, ct;
            if (!(maxValue is null))
            {
                x = maxValue.GetType();
                ct = a.GetComparableInterfaces().FirstOrDefault(ci => ci.IsAssignableFrom(a) && ci.IsAssignableFrom(x));
                if (ct is null)
                {
                    ct = x.GetComparableInterfaces().FirstOrDefault(ci => ci.IsAssignableFrom(a) && ci.IsAssignableFrom(x));
                    if (ct is null)
                        result = null;
                    else
                        try { result = 0 - (int)ct.GetMethod(nameof(IComparable<object>.CompareTo), ct.GetGenericArguments()).Invoke(x, new object[] { a }); }
                        catch { result = null; }

                }
                else
                    try { result = (int)ct.GetMethod(nameof(IComparable<object>.CompareTo), ct.GetGenericArguments()).Invoke(a, new object[] { x }); }
                    catch { result = null; }
                if (!result.HasValue)
                {
                    if (value is IComparable vc)
                        try { result = vc.CompareTo(maxValue); } catch { result = null; }
                    if (maxValue is IComparable mxc)
                        try { result = 0 - mxc.CompareTo(value); } catch { result = null; }
                    if (!result.HasValue)
                        throw new ValidationException($"Cannot compare {nameof(MaxValueSettingsName)} {x.FullName} with {a.FullName} values.");
                }
                if (MaximumIsExclusive ? result.Value >= 0 : result.Value > 0)
                    return false;
            }

            if (minValue is null)
                return true;

            x = minValue.GetType();
            ct = a.GetComparableInterfaces().FirstOrDefault(ci => ci.IsAssignableFrom(a) && ci.IsAssignableFrom(x));
            if (ct is null)
            {
                ct = x.GetComparableInterfaces().FirstOrDefault(ci => ci.IsAssignableFrom(a) && ci.IsAssignableFrom(x));
                if (ct is null)
                    result = null;
                else
                    try { result = 0 - (int)ct.GetMethod(nameof(IComparable<object>.CompareTo), ct.GetGenericArguments()).Invoke(x, new object[] { a }); }
                    catch { result = null; }

            }
            else
                try { result = (int)ct.GetMethod(nameof(IComparable<object>.CompareTo), ct.GetGenericArguments()).Invoke(a, new object[] { x }); }
                catch { result = null; }
            if (!result.HasValue)
            {
                if (value is IComparable vc)
                    try { result = vc.CompareTo(minValue); } catch { result = null; }
                if (minValue is IComparable mxc)
                    try { result = 0 - mxc.CompareTo(value); } catch { result = null; }
                if (!result.HasValue)
                    throw new ValidationException($"Cannot compare {nameof(MinValueSettingsName)} {x.FullName} with {a.FullName} values.");
            }
            return (MinimumIsExclusive ? result.Value > 0 : result.Value >= 0);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            bool isValid;
            try { isValid = IsValid(value); }
            catch (ValidationException exc)
            {
                return new ValidationResult(string.IsNullOrWhiteSpace(exc.Message) ? exc.ToString() : exc.Message, GetMemberNames(validationContext));
            }
            catch (Exception exc)
            {
                return new ValidationResult($"Unexpected error while validating range: {(string.IsNullOrWhiteSpace(exc.Message) ? exc.ToString() : exc.Message)}", GetMemberNames(validationContext));
            }
            return isValid ? ValidationResult.Success : new ValidationResult(ErrorMessageString, GetMemberNames(validationContext));
        }

        private static bool TryConvertToSimpleType(object value, out TypeCode typeCode, out object result)
        {
            typeCode = Convert.GetTypeCode(value);
            try
            {
                switch (typeCode)
                {
                    case TypeCode.Boolean:
                        result = (value is bool tf) ? tf : Convert.ToBoolean(value);
                        return true;
                    case TypeCode.Byte:
                        result = (value is byte b) ? b : Convert.ToByte(value);
                        return true;
                    case TypeCode.Char:
                        result = (value is char c) ? c : Convert.ToChar(value);
                        return true;
                    case TypeCode.DateTime:
                        result = (value is DateTime dt) ? dt : Convert.ToDateTime(value);
                        return true;
                    case TypeCode.Decimal:
                        result = (value is decimal m) ? m : Convert.ToDecimal(value);
                        return true;
                    case TypeCode.Double:
                        result = (value is double d) ? d : Convert.ToDouble(value);
                        return true;
                    case TypeCode.Int16:
                        result = (value is short sv) ? sv : Convert.ToInt16(value);
                        return true;
                    case TypeCode.Int32:
                        result = (value is int i) ? i : Convert.ToInt32(value);
                        return true;
                    case TypeCode.Int64:
                        result = (value is long lv) ? lv : Convert.ToInt64(value);
                        return true;
                    case TypeCode.SByte:
                        result = (value is sbyte sb) ? sb : Convert.ToSByte(value);
                        return true;
                    case TypeCode.Single:
                        result = (value is float f) ? f : Convert.ToSingle(value);
                        return true;
                    case TypeCode.String:
                        result = (value is string s) ? s : Convert.ToString(value);
                        return true;
                    case TypeCode.UInt16:
                        result = (value is ushort us) ? us : Convert.ToUInt16(value);
                        return true;
                    case TypeCode.UInt32:
                        result = (value is uint u) ? u : Convert.ToUInt32(value);
                        return true;
                    case TypeCode.UInt64:
                        result = (value is ulong ul) ? ul : Convert.ToUInt64(value);
                        return true;
                    case TypeCode.DBNull:
                        result = value;
                        return value is DBNull;
                }
            }
            catch { /* Ignore unexpected conversions and fail */ }
            result = value;
            return false;
        }

        private static object ToRangeValue(TypeCode targetTypeCode, object value)
        {
            if (value is null)
                return null;
            switch (targetTypeCode)
            {
                case TypeCode.Byte:
                    return ToByteRangeValue(value);
                case TypeCode.Char:
                    return ToCharRangeValue(value);
                case TypeCode.Decimal:
                    return ToDecimalRangeValue(value);
                case TypeCode.Double:
                    return ToDoubleRangeValue(value);
                case TypeCode.Int16:
                    return ToInt16RangeValue(value);
                case TypeCode.Int32:
                    return ToInt32RangeValue(value);
                case TypeCode.Int64:
                    return ToInt64RangeValue(value);
                case TypeCode.SByte:
                    return ToSByteRangeValue(value);
                case TypeCode.Single:
                    return ToSingleRangeValue(value);
                case TypeCode.UInt16:
                    return ToUInt16RangeValue(value);
                case TypeCode.UInt32:
                    return ToUInt32RangeValue(value);
                case TypeCode.UInt64:
                    return ToUInt64RangeValue(value);
                case TypeCode.Boolean:
                    return (value is bool bv) ? bv : Convert.ToBoolean(value);
                case TypeCode.DateTime:
                    return (value is DateTime dt) ? dt : Convert.ToDateTime(value);
                case TypeCode.String:
                    return (value is string t) ? t : Convert.ToString(value);
            }
            return value;
        }

        private static byte? ToByteRangeValue(object value)
        {
            if (value is null)
                return null;
            if (TryConvertToSimpleType(value, out TypeCode _, out value))
            {
                if (value is byte bv0)
                    return bv0;
                if (value is char c0)
                {
                    if (c0 > byte.MaxValue)
                        return null;
                    return (byte)c0;
                }
                if (value is decimal m0)
                {
                    if (m0 < 0m || m0 > byte.MaxValue)
                        return null;
                    return (byte)m0;
                }
                if (value is double d0)
                {
                    if (d0 < 0.0 || d0 > byte.MaxValue)
                        return null;
                    return (byte)d0;
                }
                if (value is short sv0)
                {
                    if (sv0 < 0 || sv0 > byte.MaxValue)
                        return null;
                    return (byte)sv0;
                }
                if (value is int i0)
                {
                    if (i0 < 0 || i0 > byte.MaxValue)
                        return null;
                    return (byte)i0;
                }
                if (value is long lv0)
                {
                    if (lv0 < 0 || lv0 > byte.MaxValue)
                        return null;
                    return (byte)lv0;
                }
                if (value is sbyte sb0)
                {
                    if (sb0 < 0)
                        return null;
                    return (byte)sb0;
                }
                if (value is float f0)
                {
                    if (f0 < 0f || f0 > byte.MaxValue)
                        return null;
                    return (byte)f0;
                }
                if (value is ushort us0)
                {
                    if (us0 > byte.MaxValue)
                        return null;
                    return (byte)us0;
                }
                if (value is uint u0)
                {
                    if (u0 > byte.MaxValue)
                        return null;
                    return (byte)u0;
                }
                if (value is ulong ul0)
                {
                    if (ul0 > byte.MaxValue)
                        return null;
                    return (byte)ul0;
                }
            }
            return Convert.ToByte(value);
        }

        private static char? ToCharRangeValue(object value)
        {
            if (value is null)
                return null;
            if (TryConvertToSimpleType(value, out TypeCode _, out value))
            {
                if (value is char c1)
                    return c1;
                if (value is decimal m1)
                {
                    if (m1 < 0m || m1 > char.MaxValue)
                        return null;
                    return (char)m1;
                }
                if (value is double d1)
                {
                    if (d1 < 0.0 || d1 > char.MaxValue)
                        return null;
                    return (char)d1;
                }
                if (value is ushort sv1)
                {
                    if (sv1 < 0)
                        return null;
                    return (char)sv1;
                }
                if (value is int i1)
                {
                    if (i1 < 0 || i1 > char.MaxValue)
                        return null;
                    return (char)i1;
                }
                if (value is long lv1)
                {
                    if (lv1 < 0 || lv1 > char.MaxValue)
                        return null;
                    return (char)lv1;
                }
                if (value is sbyte sb1)
                {
                    if (sb1 < 0)
                        return null;
                    return (char)sb1;
                }
                if (value is float f1)
                {
                    if (f1 < 0f || f1 > char.MaxValue)
                        return null;
                    return (char)f1;
                }
                if (value is uint u1)
                {
                    if (u1 > char.MaxValue)
                        return null;
                    return (char)u1;
                }
                if (value is ulong ul1)
                {
                    if (ul1 > char.MaxValue)
                        return null;
                    return (char)ul1;
                }
            }
            return Convert.ToChar(value);
        }

        private static short? ToInt16RangeValue(object value)
        {
            if (value is null)
                return null;
            if (TryConvertToSimpleType(value, out TypeCode _, out value))
            {
                if (value is short sv3)
                    return sv3;
                if (value is char c3)
                {
                    if (c3 > short.MaxValue)
                        return null;
                    return (short)c3;
                }
                if (value is decimal m3)
                {
                    if (m3 < short.MinValue || m3 > short.MaxValue)
                        return null;
                    return (short)m3;
                }
                if (value is double d3)
                {
                    if (d3 < short.MinValue || d3 > short.MaxValue)
                        return null;
                    return (short)d3;
                }
                if (value is int i3)
                {
                    if (i3 < short.MinValue || i3 > short.MaxValue)
                        return null;
                    return (short)i3;
                }
                if (value is long lv3)
                {
                    if (lv3 < short.MinValue || lv3 > short.MaxValue)
                        return null;
                    return (short)lv3;
                }
                if (value is float f3)
                {
                    if (f3 < short.MinValue || f3 > short.MaxValue)
                        return null;
                    return (short)f3;
                }
                if (value is ushort us3)
                {
                    if (us3 > short.MaxValue)
                        return null;
                    return (short)us3;
                }
                if (value is uint u3)
                {
                    if (u3 > short.MaxValue)
                        return null;
                    return (short)u3;
                }
                if (value is ulong ul3)
                {
                    if (ul3 > (ulong)short.MaxValue)
                        return null;
                    return (short)ul3;
                }
            }
            return Convert.ToInt16(value);
        }

        private static int? ToInt32RangeValue(object value)
        {
            if (value is null)
                return null;
            if (TryConvertToSimpleType(value, out TypeCode _, out value))
            {
                if (value is int i4)
                    return i4;
                if (value is decimal m4)
                {
                    if (m4 < int.MinValue || m4 > int.MaxValue)
                        return null;
                    return (int)m4;
                }
                if (value is double d4)
                {
                    if (d4 < int.MinValue || d4 > int.MaxValue)
                        return null;
                    return (int)d4;
                }
                if (value is long lv4)
                {
                    if (lv4 < int.MinValue || lv4 > int.MaxValue)
                        return null;
                    return (int)lv4;
                }
                if (value is float f4)
                {
                    if (f4 < int.MinValue || f4 > int.MaxValue)
                        return null;
                    return (int)f4;
                }
                if (value is uint u4)
                {
                    if (u4 > int.MaxValue)
                        return null;
                    return (int)u4;
                }
                if (value is ulong ul4)
                {
                    if (ul4 > int.MaxValue)
                        return null;
                    return (int)ul4;
                }
            }
            return Convert.ToInt32(value);
        }

        private static long? ToInt64RangeValue(object value)
        {
            if (value is null)
                return null;
            if (TryConvertToSimpleType(value, out TypeCode _, out value))
            {
                if (value is long lv5)
                    return lv5;
                if (value is decimal m5)
                {
                    if (m5 < long.MinValue || m5 > long.MaxValue)
                        return null;
                    return (long)m5;
                }
                if (value is double d5)
                {
                    if (d5 < long.MinValue || d5 > long.MaxValue)
                        return null;
                    return (long)d5;
                }
                if (value is float f5)
                {
                    if (f5 < long.MinValue || f5 > long.MaxValue)
                        return null;
                    return (long)f5;
                }
                if (value is ulong ul5)
                {
                    if (ul5 > long.MaxValue)
                        return null;
                    return (long)ul5;
                }
            }
            return Convert.ToInt64(value);
        }

        private static sbyte? ToSByteRangeValue(object value)
        {
            if (value is null)
                return null;
            if (TryConvertToSimpleType(value, out TypeCode _, out value))
            {
                if (value is sbyte sb6)
                    return sb6;
                if (value is byte bv6)
                {
                    if (bv6 > sbyte.MaxValue)
                        return null;
                    return (sbyte)bv6;
                }
                if (value is char c6)
                {
                    if (c6 > sbyte.MaxValue)
                        return null;
                    return (sbyte)c6;
                }
                if (value is decimal m6)
                {
                    if (m6 < sbyte.MinValue || m6 > sbyte.MaxValue)
                        return null;
                    return (sbyte)m6;
                }
                if (value is double d6)
                {
                    if (d6 < sbyte.MinValue || d6 > sbyte.MaxValue)
                        return null;
                    return (sbyte)d6;
                }
                if (value is short sv6)
                {
                    if (sv6 < sbyte.MinValue || sv6 > sbyte.MaxValue)
                        return null;
                    return (sbyte)sv6;
                }
                if (value is int i6)
                {
                    if (i6 < sbyte.MinValue || i6 > sbyte.MaxValue)
                        return null;
                    return (sbyte)i6;
                }
                if (value is long lv6)
                {
                    if (lv6 < sbyte.MinValue || lv6 > sbyte.MaxValue)
                        return null;
                    return (sbyte)lv6;
                }
                if (value is float f6)
                {
                    if (f6 < sbyte.MinValue || f6 > sbyte.MaxValue)
                        return null;
                    return (sbyte)f6;
                }
                if (value is ushort us6)
                {
                    if (us6 > sbyte.MaxValue)
                        return null;
                    return (sbyte)us6;
                }
                if (value is uint u6)
                {
                    if (u6 > sbyte.MaxValue)
                        return null;
                    return (sbyte)u6;
                }
                if (value is ulong ul6)
                {
                    if (ul6 > (ulong)sbyte.MaxValue)
                        return null;
                    return (sbyte)ul6;
                }
            }
            return Convert.ToSByte(value);
        }

        private static float? ToSingleRangeValue(object value)
        {
            if (value is null)
                return null;
            if (TryConvertToSimpleType(value, out TypeCode _, out value))
            {
                if (value is float f7)
                    return f7;
                if (value is double d7)
                {
                    if (d7 < float.MinValue || d7 > float.MaxValue)
                        return null;
                    return (float)d7;
                }
            }
            return Convert.ToSingle(value);
        }

        private static ushort? ToUInt16RangeValue(object value)
        {
            if (value is null)
                return null;
            if (TryConvertToSimpleType(value, out TypeCode _, out value))
            {
                if (value is ushort us8)
                    return us8;
                if (value is decimal m8)
                {
                    if (m8 < 0m || m8 > ushort.MaxValue)
                        return null;
                    return (ushort)m8;
                }
                if (value is double d8)
                {
                    if (d8 < 0.0 || d8 > ushort.MaxValue)
                        return null;
                    return (ushort)d8;
                }
                if (value is sbyte sb8)
                {
                    if (sb8 < 0)
                        return null;
                    return (ushort)sb8;
                }
                if (value is short sv8)
                {
                    if (sv8 < 0)
                        return null;
                    return (ushort)sv8;
                }
                if (value is int i8)
                {
                    if (i8 < 0 || i8 > ushort.MaxValue)
                        return null;
                    return (ushort)i8;
                }
                if (value is long lv8)
                {
                    if (lv8 < 0L || lv8 > ushort.MaxValue)
                        return null;
                    return (ushort)lv8;
                }
                if (value is float f8)
                {
                    if (f8 < 0f || f8 > ushort.MaxValue)
                        return null;
                    return (ushort)f8;
                }
                if (value is uint u8)
                {
                    if (u8 > ushort.MaxValue)
                        return null;
                    return (ushort)u8;
                }
                if (value is ulong ul8)
                {
                    if (ul8 > ushort.MaxValue)
                        return null;
                    return (ushort)ul8;
                }
            }
            return Convert.ToUInt16(value);
        }

        private static uint? ToUInt32RangeValue(object value)
        {
            if (value is null)
                return null;
            if (TryConvertToSimpleType(value, out TypeCode _, out value))
            {
                if (value is uint u9)
                    return u9;
                if (value is decimal m9)
                {
                    if (m9 < 0m || m9 > uint.MaxValue)
                        return null;
                    return (uint)m9;
                }
                if (value is double d9)
                {
                    if (d9 < 0.0 || d9 > uint.MaxValue)
                        return null;
                    return (uint)d9;
                }
                if (value is sbyte sb9)
                {
                    if (sb9 < 0)
                        return null;
                    return (uint)sb9;
                }
                if (value is short sv9)
                {
                    if (sv9 < 0)
                        return null;
                    return (uint)sv9;
                }
                if (value is int i9)
                {
                    if (i9 < 0)
                        return null;
                    return (uint)i9;
                }
                if (value is long lv9)
                {
                    if (lv9 < 0L || lv9 > uint.MaxValue)
                        return null;
                    return (uint)lv9;
                }
                if (value is float f9)
                {
                    if (f9 < 0f || f9 > uint.MaxValue)
                        return null;
                    return (uint)f9;
                }
                if (value is ulong ul9)
                {
                    if (ul9 > uint.MaxValue)
                        return null;
                    return (uint)ul9;
                }
            }
            return Convert.ToUInt32(value);
        }

        private static ulong? ToUInt64RangeValue(object value)
        {
            if (value is null)
                return null;
            if (TryConvertToSimpleType(value, out TypeCode _, out value))
            {
                if (value is ulong ulA)
                    return ulA;
                if (value is decimal mA)
                {
                    if (mA < 0m || mA > uint.MaxValue)
                        return null;
                    return (ulong)mA;
                }
                if (value is double dA)
                {
                    if (dA < 0.0 || dA > uint.MaxValue)
                        return null;
                    return (ulong)dA;
                }
                if (value is sbyte sbA)
                {
                    if (sbA < 0)
                        return null;
                    return (ulong)sbA;
                }
                if (value is short svA)
                {
                    if (svA < 0)
                        return null;
                    return (ulong)svA;
                }
                if (value is int iA)
                {
                    if (iA < 0)
                        return null;
                    return (ulong)iA;
                }
                if (value is long lvA)
                {
                    if (lvA < 0L || lvA > uint.MaxValue)
                        return null;
                    return (ulong)lvA;
                }
                if (value is float fA)
                {
                    if (fA < 0f || fA > uint.MaxValue)
                        return null;
                    return (ulong)fA;
                }
            }
            return Convert.ToUInt64(value);
        }

        private static decimal? ToDecimalRangeValue(object value)
        {
            if (value is null)
                return null;
            if (TryConvertToSimpleType(value, out TypeCode _, out value))
            {
                if (value is decimal m2)
                    return m2;
                if (value is double d2)
                {
                    if (d2 < Convert.ToDouble(decimal.MinValue) || d2 > Convert.ToDouble(decimal.MaxValue))
                        return null;
                    return (decimal)d2;
                }
                if (value is float f2)
                {
                    if (f2 < Convert.ToDouble(decimal.MinValue) || f2 > Convert.ToDouble(decimal.MaxValue))
                        return null;
                    return (decimal)f2;
                }
            }
            return Convert.ToDecimal(value);
        }

        private static double? ToDoubleRangeValue(object value)
        {
            if (value is null)
                return null;
            TryConvertToSimpleType(value, out TypeCode _, out object o);
            return (o is double d) ? d : Convert.ToDouble(value);
        }
    }
}

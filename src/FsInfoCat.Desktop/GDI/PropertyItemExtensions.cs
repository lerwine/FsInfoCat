using FsInfoCat.Numerics;
using System;
using System.Drawing.Imaging;

namespace FsInfoCat.Desktop.GDI
{
    public static class PropertyItemExtensions
    {
        public static PropertyType GetPropertyType(this PropertyItem property) => (PropertyType)property.Type;

        public static bool TryGetRational(this PropertyItem property, out FractionU32 result)
        {
            uint denominator;
            if (property is null || property.Value is null || property.Value.Length < 8 || property.Len < 8 || property.GetPropertyType() != PropertyType.Rational ||
                (denominator = BitConverter.ToUInt32(property.Value, 4)) == 0)
            {
                result = default;
                return false;
            }
            result = new FractionU32(BitConverter.ToUInt32(property.Value, 0), denominator);
            return true;
        }
    }
}

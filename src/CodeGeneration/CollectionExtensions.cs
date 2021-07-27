using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CodeGeneration
{
    public static class CollectionExtensions
    {
        public static Type ToUnderlyingType(this Type type)
        {
            if (type is null)
                return null;
            Type e;
            if (type.HasElementType)
            {
                e = type.GetElementType();
                if (e.IsArray || e.IsPointer)
                    return type;
                if (type.IsArray || type.IsPointer)
                    return ToUnderlyingType(e).MakeArrayType();
                return ToUnderlyingType(e);
            }

            if (type.IsValueType)
            {
                if (type.IsGenericType && typeof(Nullable<>).IsAssignableFrom(type.GetGenericTypeDefinition()))
                    return ToUnderlyingType(Nullable.GetUnderlyingType(type));
                if (type.IsEnum)
                    type = Enum.GetUnderlyingType(type);
                if (type.IsPrimitive || type == typeof(decimal) || type == typeof(DateTime) || type == typeof(DateTimeOffset) || type == typeof(TimeSpan) || type == typeof(Guid))
                    return type;
            }
            else
            {
                if (type == typeof(string))
                    return type;
                if (type == typeof(Uri))
                    return typeof(string);
            }
            e = type.GetInterfaces().Where(i => i.IsGenericType && typeof(IEnumerable<>) == i.GetGenericTypeDefinition()).Select(i => i.GetGenericArguments()[0]).FirstOrDefault();
            if (e is not null)
            {
                Type u = ToUnderlyingType(e);
                if (u.IsPrimitive || u == typeof(string) || u == typeof(decimal) || u == typeof(DateTime) || u == typeof(DateTimeOffset) || u == typeof(TimeSpan) || u == typeof(Guid))
                    return u.MakeArrayType();
                return e.MakeArrayType();
            }
            return type;
        }

        public static int FindPrimeNumber(this int startValue)
        {
            try
            {
                if ((Math.Abs(startValue) & 1) == 0)
                    startValue++;
                while (!IsPrimeNumber(startValue))
                    startValue += 2;
            }
            catch (OverflowException) { return 1; }
            return startValue;
        }

        public static bool IsPrimeNumber(this int value)
        {
            if (((value = Math.Abs(value)) & 1) == 0)
                return false;
            for (int i = value >> 1; i > 1; i--)
            {
                if (value % i == 0)
                    return false;
            }
            return true;
        }
    }
}

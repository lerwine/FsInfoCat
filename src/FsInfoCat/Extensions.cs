using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace FsInfoCat
{
    public static class Extensions
    {
        public static readonly IServiceProvider ServiceProvider = new ServiceCollection()
            .AddSingleton<Services.IThreadLockService, Internal.ThreadLockService>()
            .AddSingleton<Services.IComparisonService, Internal.ComparisonService>()
            .AddSingleton<Services.ICollectionsService, Internal.CollectionsService>()
            .AddTransient<Services.ISuspendable, Internal.Suspendable>()
            .AddSingleton<Services.ISuspendableService, Internal.SuspendableService>()
            .BuildServiceProvider();

        public static Services.IThreadLockService GetThreadLockService() => ServiceProvider.GetService<Services.IThreadLockService>();

        public static Services.IComparisonService GetComparisonService() => ServiceProvider.GetService<Services.IComparisonService>();

        public static Services.ISuspendable NewSuspendable() => ServiceProvider.GetService<Services.ISuspendable>();

        public static Services.ISuspendableService GetSuspendableService() => ServiceProvider.GetService<Services.ISuspendableService>();

        public static Services.ICollectionsService GetCollectionsService() => ServiceProvider.GetService<Services.ICollectionsService>();

        public static ISuspensionProvider NewSuspensionProvider() => new Internal.SuspensionProvider();

        public static readonly Regex BackslashEscapablePattern = new Regex(@"(?<l>[""\\])|[\0\a\b\f\n\r\t\v]|(\p{C}|(?! )(\s|\p{Z}))(?<x>[\da-fA-F])?", RegexOptions.Compiled);

        public static readonly Regex BackslashEscapableLBPattern = new Regex(@"(?<l>[""\\])|(?<n>\r\n?|\n)|[\0\a\b\f\t\v]|(\p{C}|(?! )(\s|\p{Z}))(?<x>[\da-fA-F])?", RegexOptions.Compiled);

        public static string ToCsTypeName(this Type type)
        {
            if (type is null)
                return "null";
            if (type.IsGenericParameter)
                return type.Name;
            if (type.IsPointer)
                return $"{ToCsTypeName(type.GetElementType())}*";
            if (type.IsByRef)
                return $"{ToCsTypeName(type.GetElementType())}&";
            if (type.IsArray)
            {
                int rank = type.GetArrayRank();
                if (rank < 2)
                    return $"{ToCsTypeName(type.GetElementType())}[]";
                if (rank == 2)
                    return $"{ToCsTypeName(type.GetElementType())}[,]";
                return $"{ToCsTypeName(type.GetElementType())}[{new string(',', rank - 1)}]";
            }
            if (type.IsNullableType())
                return $"{ToCsTypeName(Nullable.GetUnderlyingType(type))}?";

            if (type.IsValueType)
            {
                if (type.Equals(typeof(char)))
                    return "char";
                if (type.Equals(typeof(bool)))
                    return "bool";
                if (type.Equals(typeof(byte)))
                    return "byte";
                if (type.Equals(typeof(sbyte)))
                    return "sbyte";
                if (type.Equals(typeof(short)))
                    return "short";
                if (type.Equals(typeof(ushort)))
                    return "ushort";
                if (type.Equals(typeof(int)))
                    return "int";
                if (type.Equals(typeof(uint)))
                    return "uint";
                if (type.Equals(typeof(long)))
                    return "long";
                if (type.Equals(typeof(ulong)))
                    return "ulong";
                if (type.Equals(typeof(float)))
                    return "float";
                if (type.Equals(typeof(double)))
                    return "double";
                if (type.Equals(typeof(decimal)))
                    return "decimal";
            }
            else
            {
                if (type.Equals(typeof(string)))
                    return "string";
                if (type.Equals(typeof(object)))
                    return "object";
            }
            string n = type.Name;
            string ns;
            if (type.IsNested)
                ns = ToCsTypeName(type.DeclaringType);
            else if ((ns = type.Namespace) is null || ns == "System")
                ns = "";

            if (type.IsGenericType)
            {
                int i = n.IndexOf("`");
                if (i > 0)
                    n = n.Substring(0, i);
                if (ns.Length > 0)
                    return $"{ns}.{n}<{string.Join(",", type.GetGenericArguments().Select(a => a.ToCsTypeName()))}>";
                return $"{n}<{string.Join(",", type.GetGenericArguments().Select(a => a.ToCsTypeName()))}>";
            }
            return (ns.Length > 0) ? $"{ns}.{n}" : n;
        }

        public static string ToPseudoCsText(object obj)
        {
            if (obj is null)
                return "null";
            if (obj is string s)
                return $"\"{EscapeCsString(s)}\"";
            if (obj is char c)
            {
                switch (c)
                {
                    case '\'':
                        return "'\\''";
                    case '"':
                        return "'\"'";
                    default:
                        return $"'{EscapeCsString(new string(new char[] { c }))}'";
                }
            }
            if (obj is bool bv)
                return bv ? "true" : "false";
            if (obj is byte bn)
                return bn.ToString("X2");
            if (obj is sbyte sb)
                return $"(sbyte){sb:X2}";
            if (obj is short sv)
                return sv.ToString("X4");
            if (obj is ushort us)
                return $"(ushort){us:X4}";
            if (obj is int i)
                return i.ToString("X8");
            if (obj is uint ui)
                return $"{ui:X8}U";
            if (obj is long l)
                return l.ToString("X16");
            if (obj is ulong ul)
                return $"{ul:16}UL";
            if (obj is float fv)
                return $"{fv}f";
            if (obj is double d)
                return d.ToString();
            if (obj is decimal m)
                return $"{m}m";
            if (obj is DateTime dt)
                return dt.ToString();
            if (obj is DBNull)
                return "DBNull";
            if (obj is Type t)
                return t.ToCsTypeName();
            if (obj is IFormattable fm)
                fm.ToString();
            if (obj is IConvertible cv)
            {
                switch (cv.GetTypeCode())
                {
                    case TypeCode.Boolean:
                        return ToPseudoCsText(cv.ToBoolean(null));
                    case TypeCode.Byte:
                        return ToPseudoCsText(cv.ToByte(null));
                    case TypeCode.Char:
                        return ToPseudoCsText(cv.ToChar(null));
                    case TypeCode.DateTime:
                        return ToPseudoCsText(cv.ToDateTime(null));
                    case TypeCode.DBNull:
                        return "DBNull";
                    case TypeCode.Decimal:
                        return ToPseudoCsText(cv.ToDecimal(null));
                    case TypeCode.Double:
                        return ToPseudoCsText(cv.ToDouble(null));
                    case TypeCode.Int16:
                        return ToPseudoCsText(cv.ToInt16(null));
                    case TypeCode.Int32:
                        return ToPseudoCsText(cv.ToInt32(null));
                    case TypeCode.Int64:
                        return ToPseudoCsText(cv.ToInt64(null));
                    case TypeCode.SByte:
                        return ToPseudoCsText(cv.ToSByte(null));
                    case TypeCode.Single:
                        return ToPseudoCsText(cv.ToSingle(null));
                    case TypeCode.String:
                        return ToPseudoCsText(cv.ToString(null));
                    case TypeCode.UInt16:
                        return ToPseudoCsText(cv.ToUInt16(null));
                    case TypeCode.UInt32:
                        return ToPseudoCsText(cv.ToUInt32(null));
                    case TypeCode.UInt64:
                        return ToPseudoCsText(cv.ToUInt64(null));
                }
            }
            return obj.ToString();
        }

        public static string EscapeCsString(string source, bool keepLineBreaks = false)
        {
            if (string.IsNullOrEmpty(source) || !BackslashEscapablePattern.IsMatch(source))
                return source;
            if (keepLineBreaks)
                return BackslashEscapableLBPattern.Replace(source, m =>
                {
                    if (m.Groups["l"].Success)
                        return $"\\{m.Value}";
                    Group g = m.Groups["n"];
                    if (g.Success)
                        switch (g.Value)
                        {
                            case "\r":
                                return "\\r\r";
                            case "\n":
                                return "\\n\n";
                            default:
                                return "\\r\\n\r\n";
                        }
                    char c = m.Value[0];
                    switch (c)
                    {
                        case '\0':
                            return "\\0";
                        case '\a':
                            return "\\a";
                        case '\b':
                            return "\\b";
                        case '\f':
                            return "\\f";
                        case '\t':
                            return "\\t";
                        case '\v':
                            return "\\v";
                        default:
                            g = m.Groups["x"];
                            uint i = (uint)c;
                            if (g.Success)
                                return $"\\x{i:x4}{g.Value}";
                            return (i > 0xff) ? $"\\x{i:x4}" : $"\\x{i:x2}";
                    }
                });
            return BackslashEscapablePattern.Replace(source, m =>
            {
                if (m.Groups["l"].Success)
                    return $"\\{m.Value}";
                char c = m.Value[0];
                switch (c)
                {
                    case '\0':
                        return "\\0";
                    case '\a':
                        return "\\a";
                    case '\b':
                        return "\\b";
                    case '\f':
                        return "\\f";
                    case '\n':
                        return "\\n";
                    case '\r':
                        return "\\r";
                    case '\t':
                        return "\\t";
                    case '\v':
                        return "\\v";
                    default:
                        Group g = m.Groups["x"];
                        uint i = (uint)c;
                        if (g.Success)
                            return $"\\x{i:x4}{g.Value}";
                        return (i > 0xff) ? $"\\x{i:x4}" : $"\\x{i:x2}";
                }
            });
        }

        public static bool IsNullableType(this Type type) => (type ?? throw new ArgumentNullException(nameof(type))).IsValueType && type.IsGenericType &&
            typeof(Nullable<>).Equals(type.GetGenericTypeDefinition());

        public static bool IsNullAssignable(this Type type) => !(type ?? throw new ArgumentNullException(nameof(type))).IsValueType || type.IsNullableType();

        /// <summary>
        /// Determines whether the specified type has the same generic type definition as another.
        /// </summary>
        /// <param name="type">The generic <see cref="Type"/>.</param>
        /// <param name="other">The <see cref="Type"/> to compare to.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="type"/> and <paramref name="other"/> are both generic types and have the same generic type definition;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type"/> or <paramref name="other"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <c><paramref name="type"/>.<see cref="Type.IsGenericType">IsGenericType</see></c> is <see langword="false"/>.
        /// </exception>
        public static bool IsSameTypeDefinition(this Type type, Type other)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            if (other is null)
                throw new ArgumentNullException(nameof(other));
            if (!type.IsGenericType)
                throw new ArgumentOutOfRangeException(nameof(type));
            return other.IsGenericType && type.GetGenericTypeDefinition().Equals(other.GetGenericTypeDefinition());
        }

        //public static Type GetCommonBaseType(this Type type, Type other)
        //{
        //    if (type is null)
        //        throw new ArgumentNullException(nameof(type));
        //    if (other is null)
        //        throw new ArgumentNullException(nameof(other));
        //    if (type.IsAssignableFrom(other))
        //        return type;
        //    if (other.IsAssignableFrom(type))
        //        return other;
        //    Type r;
        //    if (type.IsValueType)
        //    {
        //        if (!other.IsValueType)
        //            return null;
        //        if (type.IsEnum)
        //        {
        //            if (!other.IsEnum)
        //                return null;
        //            r = typeof(Enum);
        //        }
        //        r =  typeof(ValueTuple);
        //    }
        //    else
        //    {
        //        if (other.IsValueType)
        //            return null;
        //        r = typeof(object);
        //    }
        //    Type t = type.BaseType;
        //    Type a = null;
        //    while (!(t is null || t.Equals(r)))
        //    {
        //        if (t.IsAssignableFrom(other))
        //        {
        //            a = t;
        //            break;
        //        }
        //    }

        //    t = other.BaseType;
        //    Type b = null;
        //    while (!(t is null || t.Equals(r)))
        //    {
        //        if (t.IsAssignableFrom(other))
        //        {
        //            b = t;
        //            break;
        //        }
        //    }

        //    if (a is null)
        //        return b;
        //    return (b is null || b.IsAssignableFrom(a)) ? a : b;
        //}

        //public static Type[] GetCommonAssignables(this Type type, Type other)
        //{
        //    if (type is null)
        //        throw new ArgumentNullException(nameof(type));
        //    if (other is null)
        //        throw new ArgumentNullException(nameof(other));
        //    if (type.IsAssignableFrom(other))
        //        return new Type[] { type };
        //    if (other.IsAssignableFrom(type))
        //        return new Type[] { other };
        //    Type[] commonAssignables = type.GetInterfaces().Where(i => i.IsAssignableFrom(other))
        //        .Concat(other.GetInterfaces().Where(i => i.IsAssignableFrom(type))).Distinct().ToArray();
        //    Type b = type.GetCommonBaseType(other);
        //    if (!(b is null))
        //        commonAssignables = new Type[] { b }.Concat(commonAssignables).ToArray();
        //    while (commonAssignables.Length > 1)
        //    {
        //        Type[] narrowed = commonAssignables.Where((t, i) => commonAssignables.Where((c, n) => i != n && c.IsAssignableFrom(t)).Any()).ToArray();
        //        if (narrowed.Length == 0 || narrowed.Length == commonAssignables.Length)
        //            break;
        //        commonAssignables = narrowed;
        //    }
        //    return commonAssignables;
        //}

        /// <summary>
        /// Creates a constructed generic type from another generic type.
        /// </summary>
        /// <param name="genericType">The generic type.</param>
        /// <param name="genericArguments">The type parameters to use in creating the constructed generic type.</param>
        /// <returns>The constructed generic type.</returns>
        /// <remarks>If <paramref name="genericType"/>.<see cref="Type.IsConstructedGenericType">IsConstructedGenericType</see> is <see langword="true"/>,
        /// then another generic type will be constructed using <c><paramref name="genericType"/>.<see cref="Type.GetGenericTypeDefinition()">GetGenericTypeDefinition()</see></c></remarks>
        /// <exception cref="ArgumentNullException"><paramref name="genericType"/> or <paramref name="genericArguments"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="genericType"/> is not a generic type or the number of <paramref name="genericArguments"/>
        /// provided doesn't equal the arity of the <paramref name="genericType"/> definition.</exception>
        public static Type AsConstructedGeneric(this Type genericType, params Type[] genericArguments)
        {
            if (genericType is null)
                throw new ArgumentNullException(nameof(genericType));
            if (!genericType.IsGenericType)
                throw new ArgumentOutOfRangeException(nameof(genericType), $"{(string.IsNullOrWhiteSpace(genericType.FullName) ? genericType.Name : genericType.FullName)} is not a generic type.");
            try { return (genericType.IsConstructedGenericType ? genericType.GetGenericTypeDefinition() : genericType).MakeGenericType(genericArguments); }
            catch (ArgumentNullException) { throw new ArgumentNullException(nameof(genericArguments)); }
            catch (Exception exc)
            {
                throw new ArgumentOutOfRangeException(nameof(genericArguments), string.IsNullOrWhiteSpace(exc.Message) ? "Invalid generic arguments" : exc.Message);
            }
        }

        public static IEnumerable<Type> GetEquatableInterfaces(this Type type)
        {
            if (type is null)
                yield break;
            Type genericDefinition = typeof(IEquatable<>);
            if (type.IsInterface && (type.IsGenericTypeDefinition || (type.IsConstructedGenericType && type.GetGenericTypeDefinition().Equals(genericDefinition))))
                yield return type;
            foreach (Type i in type.GetInterfaces())
            {
                if (i.IsGenericTypeDefinition || (i.IsConstructedGenericType && i.GetGenericTypeDefinition().Equals(genericDefinition)))
                    yield return i;
            }
        }

        public static IEnumerable<Type> GetComparableInterfaces(this Type type)
        {
            if (type is null)
                yield break;
            Type genericDefinition = typeof(IComparable<>);
            if (type.IsInterface && (type.IsGenericTypeDefinition || (type.IsConstructedGenericType && type.GetGenericTypeDefinition().Equals(genericDefinition))))
                yield return type;
            foreach (Type i in type.GetInterfaces())
            {
                if (i.IsGenericTypeDefinition || (i.IsConstructedGenericType && i.GetGenericTypeDefinition().Equals(genericDefinition)))
                    yield return i;
            }
        }

        public static IEnumerable<Type> GetAllComparableInterfaces(this Type type)
        {
            if (type is null)
                yield break;
            Type c = typeof(IComparable);
            if (c.Equals(type))
            {
                yield return type;
                yield break;
            }
            Type genericDefinition = typeof(IComparable<>);
            if (type.IsInterface && (type.IsGenericTypeDefinition || (type.IsConstructedGenericType && type.GetGenericTypeDefinition().Equals(genericDefinition))))
                yield return type;
            foreach (Type i in type.GetInterfaces())
            {
                if (c.Equals(i) || i.IsGenericTypeDefinition || (i.IsConstructedGenericType && i.GetGenericTypeDefinition().Equals(genericDefinition)))
                    yield return i;
            }
        }

        public static bool HasGenericComparableInterface(this Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            Type c = typeof(IComparable);
            return c.Equals(type) || type.GetInterfaces().Any(i => i.Equals(c));
        }

        public static bool IsSelfEquatable(this Type type, bool strict = false)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            if (strict)
                return typeof(IEquatable<>).MakeGenericType(type).IsAssignableFrom(type);
            return type.GetEquatableInterfaces().Any(i => i.GetGenericArguments()[0].IsAssignableFrom(type));
        }

        public static bool IsSelfComparable(this Type type, bool strict = false)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            if (strict)
                return typeof(IComparable<>).MakeGenericType(type).IsAssignableFrom(type);
            return type.GetComparableInterfaces().Any(i => i.GetGenericArguments()[0].IsAssignableFrom(type));
        }

        public static bool HasTypeConverter<T>() => HasTypeConverter(typeof(T));

        public static bool HasTypeConverter(this Type type) => (type ?? throw new ArgumentNullException(nameof(type))).GetCustomAttributes<TypeConverterAttribute>(true)
            .Any(a => !string.IsNullOrWhiteSpace(a.ConverterTypeName));

        public static PropertyDescriptor GetDefaultPropertyDescriptor(this Type type) => TypeDescriptor.GetDefaultProperty((type ?? throw new ArgumentNullException(nameof(type))));

        public static IEnumerable<PropertyDescriptor> GetPropertyDescriptors<T>() => GetPropertyDescriptors(typeof(T));

        public static IEnumerable<PropertyDescriptor> GetPropertyDescriptors(this Type type) => TypeDescriptor.GetProperties((type ?? throw new ArgumentNullException(nameof(type))))
            .Cast<PropertyDescriptor>().Where(p => !p.DesignTimeOnly);

        public static Predicate<T> ToPredicate<T>(this Func<T, bool> function) => new Predicate<T>(function ?? throw new ArgumentNullException(nameof(function)));

        public static Func<T, bool> ToFunc<T>(this Predicate<T> predicate) => new Func<T, bool>(predicate ?? throw new ArgumentNullException(nameof(predicate)));
    }
}

using Microsoft.EntityFrameworkCore;
using FsInfoCat.Local;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations;
using System.CodeDom;
using System.IO;
using System.Xml;

namespace DevUtil
{
    public static class ReflectionExtensions
    {
        private const string CS_STATEMENT_TERMINATOR = ";";
        private const string CS_EQUALS = " = ";
        private const string INDENT_4X3 = "            ";
        private const string CS_METHOD_BLOCK_OPEN = "        {";
        private const string CS_METHOD_BLOCK_CLOSE = "        }";
        private const string CS_GETHASHCODE_OVERRIDE = "        public override int GetHashCode()";
        public static readonly Assembly BaseAssembly = typeof(FsInfoCat.DbEntity).Assembly;

        public static readonly Assembly LocalAssembly = typeof(LocalDbEntity).Assembly;

        public static bool IsConstructedNullableType(this Type type) => (type?.IsValueType ?? false) && type.IsConstructedGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));

        public static bool IsIEnumerableInterface(this Type type) => (type?.IsGenericType ?? false) && type.GetGenericTypeDefinition().Equals(typeof(IEnumerable<>));

        public static bool IsConstructedIEnumerableInterface(this Type type) => (type?.IsConstructedGenericType ?? false) && type.GetGenericTypeDefinition().Equals(typeof(IEnumerable<>));

        public static bool IsIEquatableInterface(this Type type) => (type?.IsGenericType ?? false) && type.GetGenericTypeDefinition().Equals(typeof(IEquatable<>));

        public static bool IsConstructedIEquatableInterface(this Type type) => (type?.IsConstructedGenericType ?? false) && type.GetGenericTypeDefinition().Equals(typeof(IEquatable<>));

        public static bool IsFsInfoCatAssemblyType(this Type type) => type is not null && (type.Assembly.Equals(BaseAssembly) || type.Assembly.Equals(LocalAssembly));

        public static bool IsBaseAssemblyType(this Type type) => type?.Assembly.Equals(BaseAssembly) ?? false;

        public static bool IsLocalAssemblyType(this Type type) => type?.Assembly.Equals(LocalAssembly) ?? false;

        public static bool ImplementsIDbEntity(this Type type) => type?.IsAssignableTo(typeof(FsInfoCat.IDbEntity)) ?? false;

        public static bool IsDbEntityType(this Type type) => type?.IsAssignableTo(typeof(FsInfoCat.DbEntity)) ?? false;

        public static bool ImplementsILocalDbEntity(this Type type) => type?.IsAssignableTo(typeof(ILocalDbEntity)) ?? false;

        public static bool IsLocalDbEntityType(this Type type) => type?.IsAssignableTo(typeof(LocalDbEntity)) ?? false;

        public static IEnumerable<Type> GetBaseTypes([DisallowNull] this Type type)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));
            if (!type.IsClass) throw new ArgumentException($"{type.FullName} is not a class type");
            while (!type.Equals(typeof(object)))
            {
                yield return type;
                if ((type = type.BaseType) is null)
                    break;
            }
        }

        public static Collection<Type> GetPolymorphTypes([DisallowNull] this Type type)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));
            Collection<Type> collection = new();
            if (type.IsClass)
                foreach (Type t in GetBaseTypes(type))
                    collection.Add(t);
            Type[] interfaces = type.GetInterfaces();
            interfaces = interfaces.Where(i =>
            {
                if (interfaces.Any(t => i.IsAssignableFrom(t) && !t.Equals(i))) return true;
                collection.Add(i);
                return false;
            }).ToArray();
            while (interfaces.Length > 0)
            {
                int count = collection.Count;
                interfaces = interfaces.Where(i =>
                {
                    if (interfaces.Any(t => i.IsAssignableFrom(t) && !t.Equals(i))) return true;
                    collection.Add(i);
                    return false;
                }).ToArray();
                if (collection.Count == count)
                {
                    foreach (Type i in interfaces)
                        collection.Add(i);
                    break;
                }
            }
            return collection;
        }

        public static Collection<T> Pull<T>([DisallowNull] this Collection<T> source, [DisallowNull] Func<T, bool> predicate)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            Collection<T> pulled = new();
            int index = 0;
            while (index < source.Count)
            {
                T item = source[index];
                if (predicate(item))
                {
                    pulled.Add(item);
                    source.RemoveAt(index);
                }
                else
                    index++;
            }
            return pulled;
        }

        public static (Collection<T>, Collection<T>) Split<T>([DisallowNull] this IEnumerable<T> source, [DisallowNull] Func<T, int, bool> predicate)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            Collection<T> trueItems = new();
            Collection<T> falseItems = new();
            if (source is not null)
            {
                int index = -1;
                foreach (T item in source)
                    if (predicate(item, ++index))
                        trueItems.Add(item);
                    else
                        falseItems.Add(item);
            }
            return (trueItems, falseItems);
        }

        public static (Collection<T>, Collection<T>) Split<T>([DisallowNull] this IEnumerable<T> source, [DisallowNull] Func<T, bool> predicate)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            Collection<T> trueItems = new();
            Collection<T> falseItems = new();
            if (source is not null)
                foreach (T item in source)
                    if (predicate(item))
                        trueItems.Add(item);
                    else
                        falseItems.Add(item);
            return (trueItems, falseItems);
        }

        private static void WriteTypeName([DisallowNull] EnhancedTypeDescriptor descriptor, [DisallowNull] TextWriter writer)
        {
            switch (descriptor.Category)
            {
                case TypeCategory.Array:
                    WriteTypeName(descriptor.UnderlyingType, writer);
                    int rank = descriptor.Type.GetArrayRank();
                    if (rank < 2)
                        writer.Write("[]");
                    else
                    {
                        writer.Write("[");
                        writer.Write(new string(',', rank - 1));
                        writer.Write("]");
                    }
                    break;
                case TypeCategory.Pointer:
                    WriteTypeName(descriptor.UnderlyingType, writer);
                    writer.Write("*");
                    break;
                case TypeCategory.ByRef:
                    WriteTypeName(descriptor.UnderlyingType, writer);
                    writer.Write("&");
                    break;
                case TypeCategory.Primitive:
                case TypeCategory.Enum:
                    writer.Write(descriptor.BaseName);
                    break;
                case TypeCategory.Nullable:
                    writer.Write(descriptor.BaseName);
                    writer.Write("?");
                    break;
                default:
                    writer.Write(descriptor.BaseName);
                    if (descriptor is EnhancedConstructedTypeDescriptor constructedTypeDescriptor)
                    {
                        if (constructedTypeDescriptor.GenericArguments.Count > 0)
                        {
                            writer.Write("<");
                            writer.WriteTypeName(constructedTypeDescriptor.GenericArguments[0]);
                            foreach (EnhancedTypeDescriptor td in constructedTypeDescriptor.GenericArguments.Skip(1))
                            {
                                writer.Write(",");
                                writer.WriteTypeName(td);
                            }
                            writer.Write(">");
                        }
                    }
                    else if (descriptor.Type.IsGenericType)
                    {
                        int count = descriptor.Type.GetGenericArguments().Length;
                        if (count < 2)
                            writer.Write("<>");
                        else
                        {
                            writer.Write("<");
                            writer.Write(new string(',', count - 1));
                            writer.Write(">");
                        }
                    }
                    break;
            }
        }

        private static void WriteFullName([DisallowNull] EnhancedTypeDescriptor descriptor, [DisallowNull] TextWriter writer)
        {
            switch (descriptor.Category)
            {
                case TypeCategory.Array:
                    WriteFullName(descriptor.UnderlyingType, writer);
                    int rank = descriptor.Type.GetArrayRank();
                    if (rank < 2)
                        writer.Write("[]");
                    else
                    {
                        writer.Write("[");
                        writer.Write(new string(',', rank - 1));
                        writer.Write("]");
                    }
                    break;
                case TypeCategory.Pointer:
                    WriteFullName(descriptor.UnderlyingType, writer);
                    writer.Write("*");
                    break;
                case TypeCategory.ByRef:
                    WriteFullName(descriptor.UnderlyingType, writer);
                    writer.Write("&");
                    break;
                case TypeCategory.Primitive:
                    writer.Write(descriptor.BaseName);
                    break;
                case TypeCategory.Enum:
                    if (descriptor.Namespace.Length > 0)
                    {
                        writer.Write(descriptor.Namespace);
                        writer.Write(".");
                    }
                    writer.Write(descriptor.BaseName);
                    break;
                case TypeCategory.Nullable:
                    WriteFullName(descriptor.UnderlyingType, writer);
                    writer.Write("?");
                    break;
                default:
                    if (descriptor.Namespace.Length > 0)
                    {
                        writer.Write(descriptor.Namespace);
                        writer.Write(".");
                    }
                    writer.Write(descriptor.BaseName);
                    if (descriptor is EnhancedConstructedTypeDescriptor constructedTypeDescriptor)
                    {
                        if (constructedTypeDescriptor.GenericArguments.Count > 0)
                        {
                            writer.Write("<");
                            writer.WriteFullName(constructedTypeDescriptor.GenericArguments[0]);
                            foreach (EnhancedTypeDescriptor td in constructedTypeDescriptor.GenericArguments.Skip(1))
                            {
                                writer.Write(",");
                                writer.WriteFullName(td);
                            }
                            writer.Write(">");
                        }
                    }
                    else if (descriptor.Type.IsGenericType)
                    {
                        int count = descriptor.Type.GetGenericArguments().Length;
                        if (count < 2)
                            writer.Write("<>");
                        else
                        {
                            writer.Write("<");
                            writer.Write(new string(',', count - 1));
                            writer.Write(">");
                        }
                    }
                    break;
            }
        }

        private static StringBuilder AppendTypeName([DisallowNull] EnhancedTypeDescriptor descriptor, [DisallowNull] StringBuilder stringBuilder)
        {
            switch (descriptor.Category)
            {
                case TypeCategory.Array:
                    int rank = descriptor.Type.GetArrayRank();
                    if (rank < 2) return stringBuilder.AppendTypeName(descriptor.UnderlyingType).Append("[]");
                    return stringBuilder.AppendTypeName(descriptor.UnderlyingType).Append('[').Append(new string(',', rank - 1)).Append(']');
                case TypeCategory.Pointer:
                    return stringBuilder.AppendTypeName(descriptor.UnderlyingType).Append('*');
                case TypeCategory.ByRef:
                    return stringBuilder.AppendTypeName(descriptor.UnderlyingType).Append('&');
                case TypeCategory.Primitive:
                case TypeCategory.Enum:
                    return stringBuilder.Append(descriptor.BaseName);
                case TypeCategory.Nullable:
                    return stringBuilder.Append(descriptor.BaseName).Append('?');
                default:
                    stringBuilder.Append(descriptor.BaseName);
                    if (descriptor is EnhancedConstructedTypeDescriptor constructedTypeDescriptor)
                    {
                        if (constructedTypeDescriptor.GenericArguments.Count > 0)
                        {
                            stringBuilder.Append('<').AppendTypeName(constructedTypeDescriptor.GenericArguments[0]);
                            foreach (EnhancedTypeDescriptor td in constructedTypeDescriptor.GenericArguments.Skip(1))
                                stringBuilder.Append(',').AppendTypeName(td);
                            return stringBuilder.Append('>');
                        }
                    }
                    else if (descriptor.Type.IsGenericType)
                    {
                        int count = descriptor.Type.GetGenericArguments().Length;
                        if (count < 2) return stringBuilder.Append("<>");
                        return stringBuilder.Append('<').Append(new string(',', count - 1)).Append('>');
                    }
                    return stringBuilder;
            }
        }

        private static StringBuilder AppendFullName([DisallowNull] EnhancedTypeDescriptor descriptor, [DisallowNull] StringBuilder stringBuilder)
        {
            switch (descriptor.Category)
            {
                case TypeCategory.Array:
                    int rank = descriptor.Type.GetArrayRank();
                    if (rank < 2) return stringBuilder.AppendFullName(descriptor.UnderlyingType).Append("[]");
                    return stringBuilder.AppendFullName(descriptor.UnderlyingType).Append('[').Append(new string(',', rank - 1)).Append(']');
                case TypeCategory.Pointer:
                    return stringBuilder.AppendFullName(descriptor.UnderlyingType).Append('*');
                case TypeCategory.ByRef:
                    return stringBuilder.AppendFullName(descriptor.UnderlyingType).Append('&');
                case TypeCategory.Primitive:
                    return stringBuilder.Append(descriptor.BaseName);
                case TypeCategory.Enum:
                    return (descriptor.Namespace.Length > 0) ? stringBuilder.Append(descriptor.Namespace).Append('.').Append(descriptor.BaseName) : stringBuilder.Append(descriptor.BaseName);
                case TypeCategory.Nullable:
                    return stringBuilder.AppendFullName(descriptor.UnderlyingType).Append('?');
                default:
                    if (descriptor.Namespace.Length > 0) stringBuilder.Append(descriptor.Namespace).Append('.');
                    stringBuilder.Append(descriptor.BaseName);
                    if (descriptor is EnhancedConstructedTypeDescriptor constructedTypeDescriptor)
                    {
                        if (constructedTypeDescriptor.GenericArguments.Count > 0)
                        {
                            stringBuilder.Append('<').AppendFullName(constructedTypeDescriptor.GenericArguments[0]);
                            foreach (EnhancedTypeDescriptor td in constructedTypeDescriptor.GenericArguments.Skip(1))
                                stringBuilder.Append(',').AppendFullName(td);
                            return stringBuilder.Append('>');
                        }
                    }
                    else if (descriptor.Type.IsGenericType)
                    {
                        int count = descriptor.Type.GetGenericArguments().Length;
                        if (count < 2) return stringBuilder.Append("<>");
                        return stringBuilder.Append('<').Append(new string(',', count - 1)).Append('>');
                    }
                    return stringBuilder;
            }
        }

        public static void WriteTypeName([DisallowNull] this TextWriter writer, [DisallowNull] EnhancedTypeDescriptor descriptor)
        {
            if (writer is null) throw new ArgumentNullException(nameof(writer));
            WriteTypeName(descriptor, writer);
        }

        public static void WriteFullName([DisallowNull] this TextWriter writer, [DisallowNull] EnhancedTypeDescriptor descriptor)
        {
            if (writer is null) throw new ArgumentNullException(nameof(writer));
            WriteFullName(descriptor, writer);
        }

        public static StringBuilder AppendTypeName([DisallowNull] this StringBuilder stringBuilder, [DisallowNull] EnhancedTypeDescriptor descriptor)
        {
            if (stringBuilder is null) throw new ArgumentNullException(nameof(stringBuilder));
            if (descriptor is null) throw new ArgumentNullException(nameof(descriptor));
            return AppendTypeName(descriptor, stringBuilder);
        }

        public static StringBuilder AppendFullName([DisallowNull] this StringBuilder stringBuilder, [DisallowNull] EnhancedTypeDescriptor descriptor)
        {
            if (stringBuilder is null) throw new ArgumentNullException(nameof(stringBuilder));
            return AppendFullName(descriptor, stringBuilder);
        }

        public static StringBuilder WriteCodeTemplate([DisallowNull] this StringBuilder stringBuilder, [DisallowNull] EnhancedTypeDescriptor baseType)
        {
            if (stringBuilder is null) throw new ArgumentNullException(nameof(stringBuilder));
            if (baseType is null) throw new ArgumentNullException(nameof(baseType));
            stringBuilder.Append("    public class ").AppendTypeName(baseType);
            stringBuilder.AppendLine().AppendLine("    {");
            foreach (EnhancedPropertyDescriptor pd in baseType.Properties)
            {
                stringBuilder.Append("        public ").AppendTypeName(pd.Type).Append(' ').Append(pd.Name).AppendLine(pd.BackingDescriptor.IsReadOnly ? " { get; }" : " { get; set; }");
            }
            return stringBuilder.AppendLine("    }");
        }

        private static void WriteGetHashCodeCombineCombine(StringBuilder stringBuilder, IList<IFieldOrPropertyDescriptor> nullableKeys, IList<IFieldOrPropertyDescriptor> guidKeys, IList<IFieldOrPropertyDescriptor> otherKeys, IList<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            stringBuilder.AppendLine(CS_GETHASHCODE_OVERRIDE).AppendLine(CS_METHOD_BLOCK_OPEN);
            int index = 0;
            foreach (IFieldOrPropertyDescriptor pd in nullableKeys)
                stringBuilder.Append(INDENT_4X3).AppendTypeName(pd.Type).Append(" key").Append(++index).Append(CS_EQUALS).Append(pd.Name).AppendLine(CS_STATEMENT_TERMINATOR);
            foreach (IFieldOrPropertyDescriptor pd in guidKeys)
                stringBuilder.Append("            Guid key").Append(++index).Append(CS_EQUALS).Append(pd.Name).AppendLine(CS_STATEMENT_TERMINATOR);
            IFieldOrPropertyDescriptor instanceProperty = otherInstanceProperties[0];
            stringBuilder.Append("            return ");
            if (nullableKeys.Count < guidKeys.Count)
            {
                if (guidKeys.Count == 1)
                    stringBuilder.Append("key1.Equals(Guid.Empty) ? ");
                else
                {
                    stringBuilder.Append("(key").Append(nullableKeys.Count + 1).Append(".Equals(Guid.Empty)");
                    for (int i = 2; i <= guidKeys.Count; i++)
                        stringBuilder.Append(" || key").Append(nullableKeys.Count + i).Append(".Equals(Guid.Empty)");
                    if (nullableKeys.Count == 1)
                        stringBuilder.Append(" || !key1.HasValue) ? ");
                    else if (nullableKeys.Count > 1)
                    {
                        stringBuilder.Append(" || !(key1.HasValue");
                        for (int i = 2; i <= nullableKeys.Count; i++)
                            stringBuilder.Append(" && key").Append(i).Append(".HasValue");
                        stringBuilder.Append(")) ? ");
                    }
                    else
                        stringBuilder.Append(") ? ");
                }
                if (otherInstanceProperties.Count == 1)
                {
                    if (instanceProperty.NotNull)
                        stringBuilder.Append(instanceProperty.Name).Append(".GetHashCode() : ");
                    else
                        stringBuilder.Append('(').Append(instanceProperty.Name).Append("?.GetHashCode() ?? 0) : ");
                }
                else
                {
                    stringBuilder.Append("HashCode.Combine(").Append(instanceProperty.Name);
                    foreach (IFieldOrPropertyDescriptor pd in otherInstanceProperties.Skip(1))
                        stringBuilder.Append(", ").Append(pd.Name);
                    stringBuilder.Append(") : ");
                }
                if (nullableKeys.Count == 0 && otherKeys.Count == 0 && guidKeys.Count == 1)
                    stringBuilder.AppendLine("key1.GetHashCode();");
                else if (guidKeys.Count == 0 && otherKeys.Count == 0 && nullableKeys.Count == 1)
                    stringBuilder.AppendLine("key1.Value.GetHashCode();");
                else
                {
                    stringBuilder.Append("HashCode.Combine(key1");
                    for (int i = 1; i <= nullableKeys.Count; i++)
                        stringBuilder.Append(", key").Append(i).Append(".Value");
                    for (int i = 1; i <= guidKeys.Count; i++)
                        stringBuilder.Append(", key").Append(nullableKeys.Count + i);
                    foreach (IFieldOrPropertyDescriptor pd in otherKeys)
                        stringBuilder.Append(", ").Append(pd.Name);
                    stringBuilder.AppendLine(");");
                }
            }
            else
            {
                if (guidKeys.Count == 0)
                {
                    if (nullableKeys.Count == 1)
                        stringBuilder.Append("key1.HasValue ? ");
                    else
                    {
                        stringBuilder.Append("(key1.HasValue");
                        for (int i = 2; i <= nullableKeys.Count; i++)
                            stringBuilder.Append(" && key").Append(i).Append(".HasValue");
                        stringBuilder.Append(") ? ");
                    }
                }
                else
                {
                    stringBuilder.Append("(key1.HasValue");
                    for (int i = 2; i <= nullableKeys.Count; i++)
                        stringBuilder.Append(" && key").Append(i).Append(".HasValue");
                    if (guidKeys.Count == 1)
                        stringBuilder.Append(" && !key").Append(nullableKeys.Count + 1).Append(".Equals(Guid.Empty)) ? ");
                    else
                    {
                        stringBuilder.Append(" && !(key").Append(nullableKeys.Count + 1).Append(".Equals(Guid.Empty)");
                        for (int i = 2; i <= guidKeys.Count; i++)
                            stringBuilder.Append(" || key").Append(nullableKeys.Count + i).Append(".Equals(Guid.Empty)");
                        stringBuilder.Append(")) ? ");
                    }
                }
                if (nullableKeys.Count == 0 && otherKeys.Count == 0 && guidKeys.Count == 1)
                    stringBuilder.AppendLine("key1.GetHashCode() : ");
                else if (guidKeys.Count == 0 && otherKeys.Count == 0 && nullableKeys.Count == 1)
                    stringBuilder.AppendLine("key1.Value.GetHashCode() : ");
                else
                {
                    stringBuilder.Append("HashCode.Combine(key1");
                    for (int i = 1; i <= nullableKeys.Count; i++)
                        stringBuilder.Append(", key").Append(i).Append(".Value");
                    for (int i = 1; i <= guidKeys.Count; i++)
                        stringBuilder.Append(", key").Append(nullableKeys.Count + i);
                    foreach (IFieldOrPropertyDescriptor pd in otherKeys)
                        stringBuilder.Append(", ").Append(pd.Name);
                    stringBuilder.Append(") : ");
                }
                if (otherInstanceProperties.Count == 1)
                {
                    if (instanceProperty.NotNull)
                        stringBuilder.Append(instanceProperty.Name).AppendLine(".GetHashCode();");
                    else
                        stringBuilder.Append(instanceProperty.Name).AppendLine("?.GetHashCode() ?? 0;");
                }
                else
                {
                    stringBuilder.Append("HashCode.Combine(").Append(instanceProperty.Name);
                    foreach (IFieldOrPropertyDescriptor pd in otherInstanceProperties.Skip(1))
                        stringBuilder.Append(", ").Append(pd.Name);
                    stringBuilder.AppendLine(");");
                }
            }
            stringBuilder.AppendLine(CS_METHOD_BLOCK_CLOSE);
        }

        private static void WriteGetHashCodeCombineAdd(StringBuilder stringBuilder, IList<IFieldOrPropertyDescriptor> nullableKeys, IList<IFieldOrPropertyDescriptor> guidKeys, IList<IFieldOrPropertyDescriptor> otherKeys, IList<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            stringBuilder.AppendLine(CS_GETHASHCODE_OVERRIDE).AppendLine(CS_METHOD_BLOCK_OPEN);
            int index = 0;
            foreach (IFieldOrPropertyDescriptor pd in nullableKeys)
                stringBuilder.Append(INDENT_4X3).AppendTypeName(pd.Type).Append(" key").Append(++index).Append(CS_EQUALS).Append(pd.Name).AppendLine(CS_STATEMENT_TERMINATOR);
            foreach (IFieldOrPropertyDescriptor pd in guidKeys)
                stringBuilder.Append("            Guid key").Append(++index).Append(CS_EQUALS).Append(pd.Name).AppendLine(CS_STATEMENT_TERMINATOR);

            if (nullableKeys.Count < guidKeys.Count)
            {
                stringBuilder.Append("            if (key").Append(nullableKeys.Count + 1).Append(".Equals(Guid.Empty))");
                for (int i = 2; i <= guidKeys.Count; i++)
                    stringBuilder.Append(" || key").Append(nullableKeys.Count + i).Append(".Equals(Guid.Empty))");
                if (nullableKeys.Count > 0)
                {
                    stringBuilder.Append(" || !(key1.HasValue");
                    for (int i = 2; i <= nullableKeys.Count; i++)
                        stringBuilder.Append(" && key").Append(i).Append(".HasValue");
                    stringBuilder.AppendLine("))");
                }
                else
                    stringBuilder.AppendLine(")");
                stringBuilder.AppendLine("            {").AppendLine("                HashCode hashCode = new();");
                foreach (IFieldOrPropertyDescriptor pd in otherInstanceProperties)
                    stringBuilder.Append("                hashCode.Add(").Append(pd.Name).AppendLine(");");
                stringBuilder.AppendLine("                return hashCode.ToHashCode();").AppendLine("            }");
                if (nullableKeys.Count == 0 && otherKeys.Count == 0 && guidKeys.Count == 1)
                    stringBuilder.AppendLine("            return key1.GetHashCode();");
                else
                {
                    if (nullableKeys.Count > 0)
                    {
                        stringBuilder.AppendLine("            return HashCode.Combine(key1.Value");
                        for (int i = 2; i <= nullableKeys.Count; i++)
                            stringBuilder.Append(", key").Append(i).Append(".Value");
                        for (int i = 1; i <= guidKeys.Count; i++)
                            stringBuilder.Append(", key").Append(nullableKeys.Count + i);
                    }
                    else
                    {
                        stringBuilder.AppendLine("            return HashCode.Combine(key1");
                        for (int i = 2; i <= guidKeys.Count; i++)
                            stringBuilder.Append(", key").Append(i);
                    }
                    foreach (IFieldOrPropertyDescriptor pd in otherKeys)
                        stringBuilder.Append(", ").Append(pd.Name);
                    stringBuilder.AppendLine(");");
                }
            }
            else
            {
                // TODO: Implement WriteGetHashCodeCombineAdd
                if (guidKeys.Count == 0 && otherKeys.Count == 0 && nullableKeys.Count == 1)
                {
                    // "            if (key3.HasValue) return key3.Value.GetHashCode();"
                }
                else
                {
                    // "            if (key3.HasValue && key4.HasValue && !(key1.Equals(Guid.Empty) || key2.Equals(Guid.Empty))) return HashCode.Combine(Key1, Key2, Key3.Value, Key4.Value, Key5, Key6);"
                }
                // "            HashCode hashCode = new();"
                // "            hashCode.Add(Value1);"
                // "            hashCode.Add(Value2);"
                // "            return hashCode.ToHashCode();"
            }

            stringBuilder.AppendLine(CS_METHOD_BLOCK_CLOSE);
        }

        private static void WriteGetHashCodeAddCombine(StringBuilder stringBuilder, IList<IFieldOrPropertyDescriptor> nullableKeys, IList<IFieldOrPropertyDescriptor> guidKeys, IList<IFieldOrPropertyDescriptor> otherKeys, IList<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            stringBuilder.AppendLine(CS_GETHASHCODE_OVERRIDE).AppendLine(CS_METHOD_BLOCK_OPEN);
            int index = 0;
            foreach (IFieldOrPropertyDescriptor pd in nullableKeys)
                stringBuilder.Append(INDENT_4X3).AppendTypeName(pd.Type).Append(" key").Append(++index).Append(CS_EQUALS).Append(pd.Name).AppendLine(CS_STATEMENT_TERMINATOR);
            foreach (IFieldOrPropertyDescriptor pd in guidKeys)
                stringBuilder.Append("            Guid key").Append(++index).Append(CS_EQUALS).Append(pd.Name).AppendLine(CS_STATEMENT_TERMINATOR);

            // TODO: Implement WriteGetHashCodeAddCombine
            //if (nullableKeys.Count < guidKeys.Count)
            //{
            //    // "            if (key3.HasValue && key4.HasValue && !(key1.Equals(Guid.Empty) || key2.Equals(Guid.Empty)))"
            //    // "            {"
            //    // "                HashCode hashCode = new();"
            //    // "                hashCode.Add(Key1);"
            //    // "                hashCode.Add(Key2);"
            //    // "                hashCode.Add(Key3.Value);"
            //    // "                hashCode.Add(Key4.Value);"
            //    // "                hashCode.Add(Key5);"
            //    // "                hashCode.Add(Key6);"
            //    // "                return hashCode.ToHashCode();"
            //    // "            }"
            //    if (otherInstanceProperties.Count == 1)
            //    {
            //        // "            return Value1?.GetHashCode() ?? 0;
            //    }
            //    else
            //    {
            //        // "            return HashCode.Combine(Value1, Value2);"
            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperties.Count == 1)
            //    {
            //        // "            if (key1.Equals(Guid.Empty) || key2.Equals(Guid.Empty) && !(key3.HasValue && key4.HasValue)) return Value1?.GetHashCode() ?? 0;
            //    }
            //    else
            //    {
            //        // "            if (key1.Equals(Guid.Empty) || key2.Equals(Guid.Empty) && !(key3.HasValue && key4.HasValue)) return HashCode.Combine(Value1, Value2);"
            //    }
            //    // "            HashCode hashCode = new();"
            //    // "            hashCode.Add(Key1);"
            //    // "            hashCode.Add(Key2);"
            //    // "            hashCode.Add(Key3.Value);"
            //    // "            hashCode.Add(Key4.Value);"
            //    // "            hashCode.Add(Key5);"
            //    // "            hashCode.Add(Key6);"
            //    // "            return hashCode.ToHashCode();"
            //}

            stringBuilder.AppendLine(CS_METHOD_BLOCK_CLOSE);
        }

        private static void WriteGetHashCodeAddAdd(StringBuilder stringBuilder, IList<IFieldOrPropertyDescriptor> nullableKeys, IList<IFieldOrPropertyDescriptor> guidKeys, IList<IFieldOrPropertyDescriptor> otherKeys, IList<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            stringBuilder.AppendLine(CS_GETHASHCODE_OVERRIDE).AppendLine(CS_METHOD_BLOCK_OPEN);
            int index = 0;
            foreach (IFieldOrPropertyDescriptor pd in nullableKeys)
                stringBuilder.Append(INDENT_4X3).AppendTypeName(pd.Type).Append(" key").Append(++index).Append(CS_EQUALS).Append(pd.Name).AppendLine(CS_STATEMENT_TERMINATOR);
            foreach (IFieldOrPropertyDescriptor pd in guidKeys)
                stringBuilder.Append("            Guid key").Append(++index).Append(CS_EQUALS).Append(pd.Name).AppendLine(CS_STATEMENT_TERMINATOR);
            stringBuilder.AppendLine("            HashCode hashCode = new();");

            // TODO: Implement WriteGetHashCodeAddAdd
            if (nullableKeys.Count < guidKeys.Count)
            {
                // "            if (key1.Equals(Guid.Empty) || key2.Equals(Guid.Empty) && !(key3.HasValue && key4.HasValue))

                stringBuilder.AppendLine("            {");

                // "                hashCode.Add(Value1);
                // "                hashCode.Add(Value2);

                stringBuilder.AppendLine("            }").AppendLine("            else").AppendLine("            {");

                // "                hashCode.Add(Key1);
                // "                hashCode.Add(Key3.Value);
                // "                hashCode.Add(Key6);
            }
            else
            {
                // "            if (key3.HasValue && key4.HasValue && !(key1.Equals(Guid.Empty) || key2.Equals(Guid.Empty)))

                stringBuilder.AppendLine("            {");

                // "                hashCode.Add(Key1);
                // "                hashCode.Add(Key3.Value);
                // "                hashCode.Add(Key6);

                stringBuilder.AppendLine("            }").AppendLine("            else").AppendLine("            {");

                // "                hashCode.Add(Value1);
                // "                hashCode.Add(Value2);
            }

            stringBuilder.AppendLine("            }").AppendLine("            return hashCode.ToHashCode();").AppendLine(CS_METHOD_BLOCK_CLOSE);
        }

        private static void WriteGetHashCodeCode(StringBuilder stringBuilder, IList<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            if (otherInstanceProperties.Count == 1)
            {
                IFieldOrPropertyDescriptor pd = otherInstanceProperties[0];
                stringBuilder.Append("        public override int GetHashCode() => ").Append(pd.Name);
                if (pd.NotNull)
                    stringBuilder.AppendLine(".GetHashCode();");
                else
                    stringBuilder.AppendLine("?.GetHashCode() ?? 0;");
            }
            else if (otherInstanceProperties.Count < 9)
            {
                stringBuilder.Append("        public override int GetHashCode() => HashCode.Combine(").Append(otherInstanceProperties[0].Name);
                foreach (IFieldOrPropertyDescriptor pd in otherInstanceProperties.Skip(1))
                    stringBuilder.Append(", ").Append(pd.Name);
                stringBuilder.AppendLine(");");
            }
            else
            {
                stringBuilder.AppendLine(CS_GETHASHCODE_OVERRIDE).AppendLine(CS_METHOD_BLOCK_OPEN).AppendLine("            HashCode hashCode = new();");
                foreach (IFieldOrPropertyDescriptor pd in otherInstanceProperties.Skip(1))
                    stringBuilder.Append("            hashCode.Add(").Append(pd.Name).AppendLine(");");
                stringBuilder.AppendLine("            return hashCode.ToHashCode();").AppendLine(CS_METHOD_BLOCK_CLOSE);
            }
        }

        public static StringBuilder WriteGetHashCodeCode(this EnhancedTypeDescriptor type, StringBuilder stringBuilder = null, Func<IFieldOrPropertyDescriptor, bool> predicate = null)
        {
            if (stringBuilder is null) stringBuilder = new();
            if (type is null) return stringBuilder;
            (IList<IFieldOrPropertyDescriptor> otherKeys, IList<IFieldOrPropertyDescriptor> otherInstanceProperties) = ((predicate is null) ? type.Properties.Select(p => p.EffectiveDescriptor) : type.Properties.Select(p => p.EffectiveDescriptor).Where(predicate)).Split(p => p.IsKey);
            if (otherKeys.Count == 0)
            {
                if (otherInstanceProperties.Count > 0)
                    WriteGetHashCodeCode(stringBuilder, otherInstanceProperties);
            }
            else if (otherInstanceProperties.Count == 0)
                WriteGetHashCodeCode(stringBuilder, otherKeys);
            else
            {
                IList<IFieldOrPropertyDescriptor> nullableKeys;
                (otherKeys, nullableKeys) = otherKeys.Split(p => p.NotNull);
                if (otherKeys.Count > 0)
                {
                    IList<IFieldOrPropertyDescriptor> guidKeys;
                    (guidKeys, otherKeys) = otherKeys.Split(p => p.Type.Type.Equals(typeof(Guid)));
                    if (nullableKeys.Count == 0 && guidKeys.Count == 0)
                        WriteGetHashCodeCode(stringBuilder, otherKeys);
                    else if (nullableKeys.Count + guidKeys.Count + otherKeys.Count < 9)
                    {
                        if (otherInstanceProperties.Count < 9)
                            WriteGetHashCodeCombineCombine(stringBuilder, nullableKeys, guidKeys, otherKeys, otherInstanceProperties);
                        else
                            WriteGetHashCodeCombineAdd(stringBuilder, nullableKeys, guidKeys, otherKeys, otherInstanceProperties);
                    }
                    else if (otherInstanceProperties.Count < 9)
                        WriteGetHashCodeAddCombine(stringBuilder, nullableKeys, guidKeys, otherKeys, otherInstanceProperties);
                    else
                        WriteGetHashCodeAddAdd(stringBuilder, nullableKeys, guidKeys, otherKeys, otherInstanceProperties);
                }
                else if (nullableKeys.Count < 9)
                {
                    if (otherInstanceProperties.Count < 9)
                        WriteGetHashCodeCombineCombine(stringBuilder, nullableKeys, otherKeys, otherKeys, otherInstanceProperties);
                    else
                        WriteGetHashCodeCombineAdd(stringBuilder, nullableKeys, otherKeys, otherKeys, otherInstanceProperties);
                }
                else if (otherInstanceProperties.Count < 9)
                    WriteGetHashCodeAddCombine(stringBuilder, nullableKeys, otherKeys, otherKeys, otherInstanceProperties);
                else
                    WriteGetHashCodeAddAdd(stringBuilder, nullableKeys, otherKeys, otherKeys, otherInstanceProperties);
            }
            return stringBuilder;
        }
    }
}

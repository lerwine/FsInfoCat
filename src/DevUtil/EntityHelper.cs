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

namespace DevUtil
{
    public static class EntityHelper
    {
        private const string Indent2 = "        ";
        private const string Indent3 = "            ";
        private const string Indent4 = "                ";

        public static readonly Assembly BaseAssembly = typeof(FsInfoCat.DbEntity).Assembly;

        public static readonly Assembly LocalAssembly = typeof(LocalDbEntity).Assembly;

        public static readonly Type ObjectType = typeof(object);

        public static readonly Type DbEntityClassType = typeof(FsInfoCat.DbEntity);

        public static readonly Type DbEntityInterfaceType = typeof(FsInfoCat.IDbEntity);

        public static readonly Type LocalDbEntityClassType = typeof(LocalDbEntity);

        public static readonly Type LocalDbEntityInterfaceType = typeof(ILocalDbEntity);

        public static readonly Type GuidType = typeof(Guid);

        public static bool IsFsInfoCatAssemblyType(this Type type) => type is not null && (type.Assembly.Equals(BaseAssembly) || type.Assembly.Equals(LocalAssembly));

        public static bool IsBaseAssemblyType(this Type type) => type is not null && type.Assembly.Equals(BaseAssembly);

        public static bool IsLocalAssemblyType(this Type type) => type is not null && type.Assembly.Equals(LocalAssembly);

        public static IEnumerable<Type> GetBaseTypes(this Type type)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));
            if (!type.IsClass) throw new ArgumentException($"{type.FullName} is not a class type");
            while (!type.Equals(ObjectType))
            {
                yield return type;
                if ((type = type.BaseType) is null)
                    break;
            }
        }

        public static Collection<Type> GetPolymorphTypes(this Type type)
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

        public static IEnumerable<Type> GetLocalConcreteTypes() => LocalAssembly.GetTypes().Where(t => DbEntityClassType.IsAssignableFrom(t) && !t.Equals(LocalDbEntityClassType));

        public static IEnumerable<Type> GetLocalInterfaceTypes() => LocalAssembly.GetTypes().Where(t => t.IsInterface && LocalDbEntityInterfaceType.IsAssignableFrom(t));

        public static IEnumerable<Type> GetBaseInterfaceTypes() => BaseAssembly.GetTypes().Where(t => t.IsInterface && LocalDbEntityInterfaceType.IsAssignableFrom(t));

        public static Collection<T> Pull<T>(this Collection<T> source, Func<T, bool> predicate)
        {
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

        public static (Collection<T>, Collection<T>) Split<T>(this IEnumerable<T> source, Func<T, int, bool> predicate)
        {
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

        public static (Collection<T>, Collection<T>) Split<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
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


        public static StringBuilder WriteGetHashCodeCode(this EnhancedTypeDescriptor type, StringBuilder stringBuilder = null, Func<IFieldOrPropertyDescriptor, bool> predicate = null)
        {
            if (stringBuilder is null) stringBuilder = new();
            if (type is null || type.Properties.Count == 0) return stringBuilder;
            if (type.Properties.Count == 1)
                WriteGetHashCodeCode(stringBuilder, new IFieldOrPropertyDescriptor[] { type.Properties[0].EffectiveDescriptor });
            else
            {
                (IList<IFieldOrPropertyDescriptor> otherKeys, IList<IFieldOrPropertyDescriptor> otherInstanceProperties) = type.Properties.Select(p => p.EffectiveDescriptor).Split(p => p.IsKey);
                if (otherInstanceProperties.Count == 0)
                    WriteGetHashCodeCode(stringBuilder, otherKeys);
                else if (otherKeys.Count > 0)
                {
                    IList<IFieldOrPropertyDescriptor> nullableKeys;
                    (otherKeys, nullableKeys) = otherKeys.Split(p => p.NotNull);
                    if (otherKeys.Count > 0)
                    {
                        IList<IFieldOrPropertyDescriptor> guidKeys;
                        (guidKeys, otherKeys) = otherKeys.Split(p => p.Type.Type.Equals(GuidType));
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
                else
                    WriteGetHashCodeCode(stringBuilder, otherInstanceProperties);
            }
            return stringBuilder;
        }

        private static void WriteGetHashCodeCombineCombine(StringBuilder stringBuilder, IList<IFieldOrPropertyDescriptor> nullableKeys, IList<IFieldOrPropertyDescriptor> guidKeys, IList<IFieldOrPropertyDescriptor> otherKeys, IList<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            stringBuilder.AppendLine("        public override int GetHashCode()").AppendLine("        {");
            int index = 0;
            foreach (IFieldOrPropertyDescriptor pd in nullableKeys)
                stringBuilder.Append("            ").Append(pd.Type.TypeName).Append(" key").Append(++index).Append(" = ").Append(pd.Name).AppendLine(";");
            foreach (IFieldOrPropertyDescriptor pd in guidKeys)
                stringBuilder.Append("            Guid key").Append(++index).Append(" = ").Append(pd.Name).AppendLine(";");
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
            stringBuilder.AppendLine("        }");
        }

        private static void WriteGetHashCodeCombineAdd(StringBuilder stringBuilder, IList<IFieldOrPropertyDescriptor> nullableKeys, IList<IFieldOrPropertyDescriptor> guidKeys, IList<IFieldOrPropertyDescriptor> otherKeys, IList<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            stringBuilder.AppendLine("        public override int GetHashCode()").AppendLine("        {");
            int index = 0;
            foreach (IFieldOrPropertyDescriptor pd in nullableKeys)
                stringBuilder.Append("            ").Append(pd.Type.TypeName).Append(" key").Append(++index).Append(" = ").Append(pd.Name).AppendLine(";");
            foreach (IFieldOrPropertyDescriptor pd in guidKeys)
                stringBuilder.Append("            Guid key").Append(++index).Append(" = ").Append(pd.Name).AppendLine(";");

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

            stringBuilder.AppendLine("        }");
        }

        private static void WriteGetHashCodeAddCombine(StringBuilder stringBuilder, IList<IFieldOrPropertyDescriptor> nullableKeys, IList<IFieldOrPropertyDescriptor> guidKeys, IList<IFieldOrPropertyDescriptor> otherKeys, IList<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            stringBuilder.AppendLine("        public override int GetHashCode()").AppendLine("        {");
            int index = 0;
            foreach (IFieldOrPropertyDescriptor pd in nullableKeys)
                stringBuilder.Append("            ").Append(pd.Type.TypeName).Append(" key").Append(++index).Append(" = ").Append(pd.Name).AppendLine(";");
            foreach (IFieldOrPropertyDescriptor pd in guidKeys)
                stringBuilder.Append("            Guid key").Append(++index).Append(" = ").Append(pd.Name).AppendLine(";");

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

            stringBuilder.AppendLine("        }");
        }

        private static void WriteGetHashCodeAddAdd(StringBuilder stringBuilder, IList<IFieldOrPropertyDescriptor> nullableKeys, IList<IFieldOrPropertyDescriptor> guidKeys, IList<IFieldOrPropertyDescriptor> otherKeys, IList<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            stringBuilder.AppendLine("        public override int GetHashCode()").AppendLine("        {");
            int index = 0;
            foreach (IFieldOrPropertyDescriptor pd in nullableKeys)
                stringBuilder.Append("            ").Append(pd.Type.TypeName).Append(" key").Append(++index).Append(" = ").Append(pd.Name).AppendLine(";");
            foreach (IFieldOrPropertyDescriptor pd in guidKeys)
                stringBuilder.Append("            Guid key").Append(++index).Append(" = ").Append(pd.Name).AppendLine(";");
            stringBuilder.AppendLine("            HashCode hashCode = new();");

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

            stringBuilder.AppendLine("            }").AppendLine("            return hashCode.ToHashCode();").AppendLine("        }");
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
                stringBuilder.AppendLine("        public override int GetHashCode()").AppendLine("        {").AppendLine("            HashCode hashCode = new();");
                foreach (IFieldOrPropertyDescriptor pd in otherInstanceProperties.Skip(1))
                    stringBuilder.Append("            hashCode.Add(").Append(pd.Name).AppendLine(");");
                stringBuilder.AppendLine("            return hashCode.ToHashCode();").AppendLine("        }");
            }
        }
    }
}

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

        /// <summary>
        /// Writes GetHashCode code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKeys">Two or more unique identifier keys.</param>
        /// <param name="nullableKeys">Two or more nullable keys.</param>
        /// <param name="otherKeys">Two or more other keys.</param>
        /// <param name="otherInstanceProperties">Two or more other instance properties.</param>
        /// <seealso cref="MyClassA1"/>
        private static void WriteGetHashCode(TextWriter writer, Collection<IFieldOrPropertyDescriptor> guidKeys, Collection<IFieldOrPropertyDescriptor> nullableKeys, Collection<IFieldOrPropertyDescriptor> otherKeys, Collection<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            //if (guidKeys.Count + nullableKeys.Count + otherKeys.Count < 9)
            //{
            //    if (guidKeys.Count > nullableKeys.Count)
            //    {
            //        if (otherInstanceProperties.Count < 9)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //    else
            //    {
            //        if (otherInstanceProperties.Count < 9)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //}
            //else
            //{
            //    if (guidKeys.Count > nullableKeys.Count)
            //    {
            //        if (otherInstanceProperties.Count < 9)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //    else
            //    {
            //        if (otherInstanceProperties.Count < 9)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes GetHashCode code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKeys">Two or more unique identifier keys.</param>
        /// <param name="nullableKeys">Two or more nullable keys.</param>
        /// <param name="otherKeys">Two or more other keys.</param>
        /// <param name="otherInstanceProperty">The other instance property.</param>
        /// <seealso cref="MyClassA2"/>
        private static void WriteGetHashCode(TextWriter writer, Collection<IFieldOrPropertyDescriptor> guidKeys, Collection<IFieldOrPropertyDescriptor> nullableKeys, Collection<IFieldOrPropertyDescriptor> otherKeys, IFieldOrPropertyDescriptor otherInstanceProperty)
        {
            //if (guidKeys.Count + nullableKeys.Count + otherKeys.Count < 9)
            //{
            //    if (guidKeys.Count > nullableKeys.Count)
            //    {
            //        if (otherInstanceProperty.NotNull)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //    else
            //    {
            //        if (otherInstanceProperty.NotNull)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //}
            //else
            //{
            //    if (guidKeys.Count > nullableKeys.Count)
            //    {
            //        if (otherInstanceProperty.NotNull)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //    else
            //    {
            //        if (otherInstanceProperty.NotNull)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes GetHashCode code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKey">The unique identifier key.</param>
        /// <param name="nullableKeys">Two or more nullable keys.</param>
        /// <param name="otherKeys">Two or more other keys.</param>
        /// <param name="otherInstanceProperties">Two or more other instance properties.</param>
        /// <seealso cref="MyClassB1"/>
        private static void WriteGetHashCode(TextWriter writer, IFieldOrPropertyDescriptor guidKey, Collection<IFieldOrPropertyDescriptor> nullableKeys, Collection<IFieldOrPropertyDescriptor> otherKeys, Collection<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            //if (nullableKeys.Count + otherKeys.Count < 8)
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes GetHashCode code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKey">The unique identifier key.</param>
        /// <param name="nullableKeys">Two or more nullable keys.</param>
        /// <param name="otherKeys">Two or more other keys.</param>
        /// <param name="otherInstanceProperty">The other instance property.</param>
        /// <seealso cref="MyClassB2"/>
        private static void WriteGetHashCode(TextWriter writer, IFieldOrPropertyDescriptor guidKey, Collection<IFieldOrPropertyDescriptor> nullableKeys, Collection<IFieldOrPropertyDescriptor> otherKeys, IFieldOrPropertyDescriptor otherInstanceProperty)
        {
            //if (nullableKeys.Count + otherKeys.Count < 8)
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes GetHashCode code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKeys">Two or more unique identifier keys.</param>
        /// <param name="nullableKey">The nullable key.</param>
        /// <param name="otherKeys">Two or more other keys.</param>
        /// <param name="otherInstanceProperties">Two or more other instance properties.</param>
        /// <seealso cref="MyClassC1"/>
        private static void WriteGetHashCode(TextWriter writer, Collection<IFieldOrPropertyDescriptor> guidKeys, IFieldOrPropertyDescriptor nullableKey, Collection<IFieldOrPropertyDescriptor> otherKeys, Collection<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            //if (guidKeys.Count + otherKeys.Count < 8)
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes GetHashCode code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKeys">Two or more unique identifier keys.</param>
        /// <param name="nullableKey">The nullable key.</param>
        /// <param name="otherKeys">Two or more other keys.</param>
        /// <param name="otherInstanceProperty">The other instance property.</param>
        /// <seealso cref="MyClassC2"/>
        private static void WriteGetHashCode(TextWriter writer, Collection<IFieldOrPropertyDescriptor> guidKeys, IFieldOrPropertyDescriptor nullableKey, Collection<IFieldOrPropertyDescriptor> otherKeys, IFieldOrPropertyDescriptor otherInstanceProperty)
        {
            //if (guidKeys.Count + otherKeys.Count < 8)
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes GetHashCode code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKey">The unique identifier key.</param>
        /// <param name="nullableKey">The nullable key.</param>
        /// <param name="otherKeys">Two or more other keys.</param>
        /// <param name="otherInstanceProperties">Two or more other instance properties.</param>
        /// <seealso cref="MyClassD1"/>
        private static void WriteGetHashCode(TextWriter writer, IFieldOrPropertyDescriptor guidKey, IFieldOrPropertyDescriptor nullableKey, Collection<IFieldOrPropertyDescriptor> otherKeys, Collection<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            //if (otherKeys.Count < 7)
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes GetHashCode code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKey">The unique identifier key.</param>
        /// <param name="nullableKey">The nullable key.</param>
        /// <param name="otherKeys">Two or more other keys.</param>
        /// <param name="otherInstanceProperty">The other instance property.</param>
        /// <seealso cref="MyClassD2"/>
        private static void WriteGetHashCode(TextWriter writer, IFieldOrPropertyDescriptor guidKey, IFieldOrPropertyDescriptor nullableKey, Collection<IFieldOrPropertyDescriptor> otherKeys, IFieldOrPropertyDescriptor otherInstanceProperty)
        {
            //if (otherKeys.Count < 7)
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes GetHashCode code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKeys">Two or more unique identifier keys.</param>
        /// <param name="nullableKeys">Two or more nullable keys.</param>
        /// <param name="otherKey">The other key.</param>
        /// <param name="otherInstanceProperties">Two or more other instance properties.</param>
        /// <seealso cref="MyClassE1"/>
        private static void WriteGetHashCode(TextWriter writer, Collection<IFieldOrPropertyDescriptor> guidKeys, Collection<IFieldOrPropertyDescriptor> nullableKeys, IFieldOrPropertyDescriptor otherKey, Collection<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            //if (guidKeys.Count + nullableKeys.Count < 8)
            //{
            //    if (guidKeys.Count > nullableKeys.Count)
            //    {
            //        if (otherInstanceProperties.Count < 9)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //    else
            //    {
            //        if (otherInstanceProperties.Count < 9)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //}
            //else
            //{
            //    if (guidKeys.Count > nullableKeys.Count)
            //    {
            //        if (otherInstanceProperties.Count < 9)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //    else
            //    {
            //        if (otherInstanceProperties.Count < 9)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes GetHashCode code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKeys">Two or more unique identifier keys.</param>
        /// <param name="nullableKeys">Two or more nullable keys.</param>
        /// <param name="otherKey">The other key.</param>
        /// <param name="otherInstanceProperty">The other instance property.</param>
        /// <seealso cref="MyClassE2"/>
        private static void WriteGetHashCode(TextWriter writer, Collection<IFieldOrPropertyDescriptor> guidKeys, Collection<IFieldOrPropertyDescriptor> nullableKeys, IFieldOrPropertyDescriptor otherKey, IFieldOrPropertyDescriptor otherInstanceProperty)
        {
            //if (guidKeys.Count + nullableKeys.Count < 8)
            //{
            //    if (guidKeys.Count > nullableKeys.Count)
            //    {
            //        if (otherInstanceProperty.NotNull)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //    else
            //    {
            //        if (otherInstanceProperty.NotNull)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //}
            //else
            //{
            //    if (guidKeys.Count > nullableKeys.Count)
            //    {
            //        if (otherInstanceProperty.NotNull)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //    else
            //    {
            //        if (otherInstanceProperty.NotNull)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes GetHashCode code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKey">The unique identifier key.</param>
        /// <param name="nullableKeys">Two or more nullable keys.</param>
        /// <param name="otherKey">The other key.</param>
        /// <param name="otherInstanceProperties">Two or more other instance properties.</param>
        /// <seealso cref="MyClassF1"/>
        private static void WriteGetHashCode(TextWriter writer, IFieldOrPropertyDescriptor guidKey, Collection<IFieldOrPropertyDescriptor> nullableKeys, IFieldOrPropertyDescriptor otherKey, Collection<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            //if (nullableKeys.Count < 7)
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes GetHashCode code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKey">The unique identifier key.</param>
        /// <param name="nullableKeys">Two or more nullable keys.</param>
        /// <param name="otherKey">The other key.</param>
        /// <param name="otherInstanceProperty">The other instance property.</param>
        /// <seealso cref="MyClassF2"/>
        private static void WriteGetHashCode(TextWriter writer, IFieldOrPropertyDescriptor guidKey, Collection<IFieldOrPropertyDescriptor> nullableKeys, IFieldOrPropertyDescriptor otherKey, IFieldOrPropertyDescriptor otherInstanceProperty)
        {
            //if (nullableKeys.Count < 7)
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes GetHashCode code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKeys">Two or more unique identifier keys.</param>
        /// <param name="nullableKey">The nullable key.</param>
        /// <param name="otherKey">The other key.</param>
        /// <param name="otherInstanceProperties">Two or more other instance properties.</param>
        /// <seealso cref="MyClassG1"/>
        private static void WriteGetHashCode(TextWriter writer, Collection<IFieldOrPropertyDescriptor> guidKeys, IFieldOrPropertyDescriptor nullableKey, IFieldOrPropertyDescriptor otherKey, Collection<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            //if (guidKeys.Count < 7)
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes GetHashCode code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKeys">Two or more unique identifier keys.</param>
        /// <param name="nullableKey">The nullable key.</param>
        /// <param name="otherKey">The other key.</param>
        /// <param name="otherInstanceProperty">The other instance property.</param>
        /// <seealso cref="MyClassG2"/>
        private static void WriteGetHashCode(TextWriter writer, Collection<IFieldOrPropertyDescriptor> guidKeys, IFieldOrPropertyDescriptor nullableKey, IFieldOrPropertyDescriptor otherKey, IFieldOrPropertyDescriptor otherInstanceProperty)
        {
            //if (guidKeys.Count < 7)
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes GetHashCode code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKey">The unique identifier key.</param>
        /// <param name="nullableKey">The nullable key.</param>
        /// <param name="otherKey">The other key.</param>
        /// <param name="otherInstanceProperty">The other instance property.</param>
        /// <seealso cref="MyClassH2"/>
        private static void WriteGetHashCode(TextWriter writer, IFieldOrPropertyDescriptor guidKey, IFieldOrPropertyDescriptor nullableKey, IFieldOrPropertyDescriptor otherKey, IFieldOrPropertyDescriptor otherInstanceProperty)
        {
            //if (otherInstanceProperty.NotNull)
            //{

            //}
            //else
            //{

            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes GetHashCode code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKey">The unique identifier key.</param>
        /// <param name="nullableKey">The nullable key.</param>
        /// <param name="otherKey">The other key.</param>
        /// <param name="otherInstanceProperties">Two or more other instance properties.</param>
        /// <seealso cref="MyClassH1"/>
        private static void WriteGetHashCode(TextWriter writer, IFieldOrPropertyDescriptor guidKey, IFieldOrPropertyDescriptor nullableKey, IFieldOrPropertyDescriptor otherKey, Collection<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            //if (otherInstanceProperties.Count < 9)
            //{

            //}
            //else
            //{

            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes GetHashCode code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKeys">Two or more unique identifier keys.</param>
        /// <param name="nullableKeys">Two or more nullable keys.</param>
        /// <param name="otherInstanceProperty">The other instance property.</param>
        /// <seealso cref="MyClassI2"/>
        private static void WriteGetHashCode(TextWriter writer, Collection<IFieldOrPropertyDescriptor> guidKeys, Collection<IFieldOrPropertyDescriptor> nullableKeys, IFieldOrPropertyDescriptor otherInstanceProperty)
        {
            //if (guidKeys.Count + nullableKeys.Count < 9)
            //{
            //    if (guidKeys.Count > nullableKeys.Count)
            //    {
            //        if (otherInstanceProperty.NotNull)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //    else
            //    {
            //        if (otherInstanceProperty.NotNull)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //}
            //else
            //{
            //    if (guidKeys.Count > nullableKeys.Count)
            //    {
            //        if (otherInstanceProperty.NotNull)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //    else
            //    {
            //        if (otherInstanceProperty.NotNull)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes GetHashCode code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKeys">Two or more unique identifier keys.</param>
        /// <param name="nullableKeys">Two or more nullable keys.</param>
        /// <param name="otherInstanceProperties">Two or more other instance properties.</param>
        /// <seealso cref="MyClassI1"/>
        private static void WriteGetHashCode(TextWriter writer, Collection<IFieldOrPropertyDescriptor> guidKeys, Collection<IFieldOrPropertyDescriptor> nullableKeys, Collection<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            //if (guidKeys.Count + nullableKeys.Count < 9)
            //{
            //    if (guidKeys.Count > nullableKeys.Count)
            //    {
            //        if (otherInstanceProperties.Count < 9)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //    else
            //    {
            //        if (otherInstanceProperties.Count < 9)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //}
            //else
            //{
            //    if (guidKeys.Count > nullableKeys.Count)
            //    {
            //        if (otherInstanceProperties.Count < 9)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //    else
            //    {
            //        if (otherInstanceProperties.Count < 9)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes GetHashCode code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKey">The unique identifier key.</param>
        /// <param name="nullableKeys">Two or more nullable keys.</param>
        /// <param name="otherInstanceProperty">The other instance property.</param>
        /// <seealso cref="MyClassJ2"/>
        private static void WriteGetHashCode(TextWriter writer, IFieldOrPropertyDescriptor guidKey, Collection<IFieldOrPropertyDescriptor> nullableKeys, IFieldOrPropertyDescriptor otherInstanceProperty)
        {
            //if (nullableKeys.Count < 8)
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes GetHashCode code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKey">The unique identifier key.</param>
        /// <param name="nullableKeys">Two or more nullable keys.</param>
        /// <param name="otherInstanceProperties">Two or more other instance properties.</param>
        /// <seealso cref="MyClassJ1"/>
        private static void WriteGetHashCode(TextWriter writer, IFieldOrPropertyDescriptor guidKey, Collection<IFieldOrPropertyDescriptor> nullableKeys, Collection<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            //if (nullableKeys.Count < 8)
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes GetHashCode code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKeys">Two or more unique identifier keys.</param>
        /// <param name="nullableKey">The nullable key.</param>
        /// <param name="otherInstanceProperty">The other instance property.</param>
        /// <seealso cref="MyClassK2"/>
        private static void WriteGetHashCode(TextWriter writer, Collection<IFieldOrPropertyDescriptor> guidKeys, IFieldOrPropertyDescriptor nullableKey, IFieldOrPropertyDescriptor otherInstanceProperty)
        {
            //if (guidKeys.Count < 8)
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes GetHashCode code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKeys">Two or more unique identifier keys.</param>
        /// <param name="nullableKey">The nullable key.</param>
        /// <param name="otherInstanceProperties">Two or more other instance properties.</param>
        /// <seealso cref="MyClassK1"/>
        private static void WriteGetHashCode(TextWriter writer, Collection<IFieldOrPropertyDescriptor> guidKeys, IFieldOrPropertyDescriptor nullableKey, Collection<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            //if (guidKeys.Count < 8)
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes GetHashCode code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKey">The unique identifier key.</param>
        /// <param name="nullableKey">The nullable key.</param>
        /// <param name="otherInstanceProperty">The other instance property.</param>
        /// <seealso cref="MyClassL2"/>
        private static void WriteGetHashCode(TextWriter writer, IFieldOrPropertyDescriptor guidKey, IFieldOrPropertyDescriptor nullableKey, Collection<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            //if (otherInstanceProperties.Count < 9)
            //{

            //}
            //else
            //{

            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes GetHashCode code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKey">The unique identifier key.</param>
        /// <param name="nullableKey">The nullable key.</param>
        /// <param name="otherInstanceProperties">Two or more other instance properties.</param>
        /// <seealso cref="MyClassL1"/>
        private static void WriteGetHashCode(TextWriter writer, IFieldOrPropertyDescriptor guidKey, IFieldOrPropertyDescriptor nullableKey, IFieldOrPropertyDescriptor otherInstanceProperty)
        {
            //if (otherInstanceProperty.NotNull)
            //{

            //}
            //else
            //{

            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes GetHashCode code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="properties">Two or more properties.</param>
        /// <seealso cref="MyClassM1"/>
        private static void WriteGetHashCode(TextWriter writer, Collection<IFieldOrPropertyDescriptor> properties)
        {
            //if (properties.Count < 9)
            //{

            //}
            //else
            //{

            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes GetHashCode code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="property">The single property.</param>
        /// <seealso cref="MyClassM2"/>
        private static void WriteGetHashCode(TextWriter writer, IFieldOrPropertyDescriptor property)
        {
            //if (property.NotNull)
            //{

            //}
            //else
            //{

            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the unique identifier keys get hash code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKeys">Two or more unique identifier keys.</param>
        /// <param name="otherKeys">Two or more other keys.</param>
        /// <param name="otherInstanceProperties">Two or more other instance properties.</param>
        /// <seealso cref="MyClassN1"/>
        private static void WriteGuidKeysGetHashCode(TextWriter writer, Collection<IFieldOrPropertyDescriptor> guidKeys, Collection<IFieldOrPropertyDescriptor> otherKeys, Collection<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            //if (guidKeys.Count + otherKeys.Count < 9)
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the unique identifier keys get hash code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKeys">Two or more unique identifier keys.</param>
        /// <param name="otherKeys">Two or more other keys.</param>
        /// <param name="otherInstanceProperty">The other instance property.</param>
        /// <seealso cref="MyClassN2"/>
        private static void WriteGuidKeysGetHashCode(TextWriter writer, Collection<IFieldOrPropertyDescriptor> guidKeys, Collection<IFieldOrPropertyDescriptor> otherKeys, IFieldOrPropertyDescriptor otherInstanceProperty)
        {
            //if (guidKeys.Count + otherKeys.Count < 9)
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the unique identifier keys get hash code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKeys">Two or more unique identifier keys.</param>
        /// <param name="otherKey">The other key.</param>
        /// <param name="otherInstanceProperties">Two or more other instance properties.</param>
        /// <seealso cref="MyClassO1"/>
        private static void WriteGuidKeysGetHashCode(TextWriter writer, Collection<IFieldOrPropertyDescriptor> guidKeys, IFieldOrPropertyDescriptor otherKey, Collection<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            //if (guidKeys.Count < 8)
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the unique identifier keys get hash code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKeys">Two or more unique identifier keys.</param>
        /// <param name="otherKey">The other key.</param>
        /// <param name="otherInstanceProperty">The other instance property.</param>
        /// <seealso cref="MyClassO2"/>
        private static void WriteGuidKeysGetHashCode(TextWriter writer, Collection<IFieldOrPropertyDescriptor> guidKeys, IFieldOrPropertyDescriptor otherKey, IFieldOrPropertyDescriptor otherInstanceProperty)
        {
            //if (guidKeys.Count < 8)
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the unique identifier keys get hash code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKeys">Two or more unique identifier keys.</param>
        /// <param name="otherInstanceProperties">Two or more other instance properties.</param>
        /// <seealso cref="MyClassP1"/>
        private static void WriteGuidKeysGetHashCode(TextWriter writer, Collection<IFieldOrPropertyDescriptor> guidKeys, Collection<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            //if (guidKeys.Count < 9)
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the unique identifier keys get hash code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKeys">Two or more unique identifier keys.</param>
        /// <param name="otherInstanceProperty">The other instance property.</param>
        /// <seealso cref="MyClassP2"/>
        private static void WriteGuidKeysGetHashCode(TextWriter writer, Collection<IFieldOrPropertyDescriptor> guidKeys, IFieldOrPropertyDescriptor otherInstanceProperty)
        {
            //if (guidKeys.Count < 9)
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the nullable keys get hash code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="nullableKeys">Two or more nullable keys.</param>
        /// <param name="otherKeys">Two or more other keys.</param>
        /// <param name="otherInstanceProperties">Two or more other instance properties.</param>
        /// <seealso cref="MyClassQ1"/>
        private static void WriteNullableKeysGetHashCode(TextWriter writer, Collection<IFieldOrPropertyDescriptor> nullableKeys, Collection<IFieldOrPropertyDescriptor> otherKeys, Collection<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            //if (nullableKeys.Count + otherKeys.Count < 9)
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the nullable keys get hash code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="nullableKeys">Two or more nullable keys.</param>
        /// <param name="otherKeys">Two or more other keys.</param>
        /// <param name="otherInstanceProperty">The other instance property.</param>
        /// <seealso cref="MyClassQ2"/>
        private static void WriteNullableKeysGetHashCode(TextWriter writer, Collection<IFieldOrPropertyDescriptor> nullableKeys, Collection<IFieldOrPropertyDescriptor> otherKeys, IFieldOrPropertyDescriptor otherInstanceProperty)
        {
            //if (nullableKeys.Count + otherKeys.Count < 9)
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the nullable keys get hash code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="nullableKeys">Two or more nullable keys.</param>
        /// <param name="otherKey">The other key.</param>
        /// <param name="otherInstanceProperties">Two or more other instance properties.</param>
        /// <seealso cref="MyClassR1"/>
        private static void WriteNullableKeysGetHashCode(TextWriter writer, Collection<IFieldOrPropertyDescriptor> nullableKeys, IFieldOrPropertyDescriptor otherKey, Collection<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            //if (nullableKeys.Count < 8)
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the nullable keys get hash code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="nullableKeys">Two or more nullable keys.</param>
        /// <param name="otherKey">The other key.</param>
        /// <param name="otherInstanceProperty">The other instance property.</param>
        /// <seealso cref="MyClassR2"/>
        private static void WriteNullableKeysGetHashCode(TextWriter writer, Collection<IFieldOrPropertyDescriptor> nullableKeys, IFieldOrPropertyDescriptor otherKey, IFieldOrPropertyDescriptor otherInstanceProperty)
        {
            //if (nullableKeys.Count < 8)
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the nullable keys get hash code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="nullableKeys">Two or more nullable keys.</param>
        /// <param name="otherInstanceProperties">Two or more other instance properties.</param>
        /// <seealso cref="MyClassS1"/>
        private static void WriteNullableKeysGetHashCode(TextWriter writer, Collection<IFieldOrPropertyDescriptor> nullableKeys, Collection<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            //if (nullableKeys.Count < 9)
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the nullable keys get hash code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="nullableKeys">Two or more nullable keys.</param>
        /// <param name="otherInstanceProperty">The other instance property.</param>
        /// <seealso cref="MyClassS2"/>
        private static void WriteNullableKeysGetHashCode(TextWriter writer, Collection<IFieldOrPropertyDescriptor> nullableKeys, IFieldOrPropertyDescriptor otherInstanceProperty)
        {
            //if (nullableKeys.Count < 9)
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the unique identifier or nullable key get hash code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidOrNullableKey">The unique identifier or nullable key.</param>
        /// <param name="otherKeys">Two or more other keys.</param>
        /// <param name="otherInstanceProperties">Two or more other instance properties.</param>
        /// <seealso cref="MyClassT1"/>
        private static void WriteGuidOrNullableKeyGetHashCode(TextWriter writer, IFieldOrPropertyDescriptor guidOrNullableKey, Collection<IFieldOrPropertyDescriptor> otherKeys, Collection<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            //if (otherKeys.Count < 8)
            //{
            //    if (guidOrNullableKey.NotNull)
            //    {
            //        if (otherInstanceProperties.Count < 9)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //    else
            //    {
            //        if (otherInstanceProperties.Count < 9)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //}
            //else
            //{
            //    if (guidOrNullableKey.NotNull)
            //    {
            //        if (otherInstanceProperties.Count < 9)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //    else
            //    {
            //        if (otherInstanceProperties.Count < 9)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the unique identifier or nullable key get hash code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidOrNullableKey">The unique identifier or nullable key.</param>
        /// <param name="otherKeys">Two or more other keys.</param>
        /// <param name="otherInstanceProperty">The other instance property.</param>
        /// <seealso cref="MyClassT2"/>
        private static void WriteGuidOrNullableKeyGetHashCode(TextWriter writer, IFieldOrPropertyDescriptor guidOrNullableKey, Collection<IFieldOrPropertyDescriptor> otherKeys, IFieldOrPropertyDescriptor otherInstanceProperty)
        {
            //if (otherKeys.Count < 8)
            //{
            //    if (guidOrNullableKey.NotNull)
            //    {
            //        if (otherInstanceProperty.NotNull)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //    else
            //    {
            //        if (otherInstanceProperty.NotNull)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //}
            //else
            //{
            //    if (guidOrNullableKey.NotNull)
            //    {
            //        if (otherInstanceProperty.NotNull)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //    else
            //    {
            //        if (otherInstanceProperty.NotNull)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the unique identifier or nullable key get hash code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidOrNullableKey">The unique identifier or nullable key.</param>
        /// <param name="otherKey">The other key.</param>
        /// <param name="otherInstanceProperties">Two or more other instance properties.</param>
        /// <seealso cref="MyClassU1"/>
        private static void WriteGuidOrNullableKeyGetHashCode(TextWriter writer, IFieldOrPropertyDescriptor guidOrNullableKey, IFieldOrPropertyDescriptor otherKey, Collection<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            //if (guidOrNullableKey.NotNull)
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the unique identifier or nullable key get hash code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidOrNullableKey">The unique identifier or nullable key.</param>
        /// <param name="otherKey">The other key.</param>
        /// <param name="otherInstanceProperty">The other instance property.</param>
        /// <seealso cref="MyClassU2"/>
        private static void WriteGuidOrNullableKeyGetHashCode(TextWriter writer, IFieldOrPropertyDescriptor guidOrNullableKey, IFieldOrPropertyDescriptor otherKey, IFieldOrPropertyDescriptor otherInstanceProperty)
        {
            //if (guidOrNullableKey.NotNull)
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the unique identifier or nullable key get hash code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidOrNullableKey">The unique identifier or nullable key.</param>
        /// <param name="otherInstanceProperties">Two or more other instance properties.</param>
        /// <seealso cref="MyClassV1"/>
        private static void WriteGuidOrNullableKeyGetHashCode(TextWriter writer, IFieldOrPropertyDescriptor guidOrNullableKey, Collection<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            //if (guidOrNullableKey.NotNull)
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the unique identifier or nullable key get hash code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidOrNullableKey">The unique identifier or nullable key.</param>
        /// <param name="otherInstanceProperty">The other instance property.</param>
        /// <seealso cref="MyClassV2"/>
        private static void WriteGuidOrNullableKeyGetHashCode(TextWriter writer, IFieldOrPropertyDescriptor guidOrNullableKey, IFieldOrPropertyDescriptor otherInstanceProperty)
        {
            //if (guidOrNullableKey.NotNull)
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        public static bool TryWriteGetHashCode(this EnhancedTypeDescriptor type, TextWriter writer, Func<IFieldOrPropertyDescriptor, bool> predicate = null)
        {
            if (writer is null) throw new ArgumentNullException(nameof(writer));
            if (type is null) return false;
            (
                Collection<IFieldOrPropertyDescriptor> keyProperties,
                Collection<IFieldOrPropertyDescriptor> otherInstanceProperties
            ) = ((predicate is null) ? type.Properties.Select(p => p.EffectiveDescriptor) : type.Properties.Select(p => p.EffectiveDescriptor).Where(predicate)).Split(p => p.IsKey);

            if (keyProperties.Count == 0)
            {
                if (otherInstanceProperties.Count == 0) return false;
                if (otherInstanceProperties.Count > 1)
                    WriteGetHashCode(writer, otherInstanceProperties);
                else
                    WriteGetHashCode(writer, otherInstanceProperties[0]);
            }
            else if (otherInstanceProperties.Count > 0)
            {
                (
                    Collection<IFieldOrPropertyDescriptor> notNullKeys,
                    Collection<IFieldOrPropertyDescriptor> otherKeys
                ) = keyProperties.Split(p => p.NotNull && !p.Type.Type.Equals(GuidType));
                if (notNullKeys.Count == 0)
                {
                    if (otherKeys.Count > 1)
                        WriteGetHashCode(writer, otherKeys);
                    else
                        WriteGetHashCode(writer, otherKeys[0]);
                }
                else
                {
                    #region notNullKeys.Count > 0

                    if (notNullKeys.Count == 1)
                        switch (otherKeys.Count)
                        {
                            case 0:
                                if (otherInstanceProperties.Count == 1)
                                    WriteGuidOrNullableKeyGetHashCode(writer, notNullKeys[0], otherInstanceProperties[0]);
                                else
                                    WriteGuidOrNullableKeyGetHashCode(writer, notNullKeys[0], otherInstanceProperties);
                                break;
                            case 1:
                                if (otherInstanceProperties.Count == 1)
                                    WriteGuidOrNullableKeyGetHashCode(writer, notNullKeys[0], otherKeys[0], otherInstanceProperties[0]);
                                else
                                    WriteGuidOrNullableKeyGetHashCode(writer, notNullKeys[0], otherKeys[0], otherInstanceProperties);
                                break;
                            default:
                                if (otherInstanceProperties.Count == 1)
                                    WriteGuidOrNullableKeyGetHashCode(writer, notNullKeys[0], otherKeys, otherInstanceProperties[0]);
                                else
                                    WriteGuidOrNullableKeyGetHashCode(writer, notNullKeys[0], otherKeys, otherInstanceProperties);
                                break;
                        }
                    else
                    {
                        #region notNullKeys.Count > 1

                        (
                            Collection<IFieldOrPropertyDescriptor> guidKeys,
                            Collection<IFieldOrPropertyDescriptor> nullableKeys
                        ) = keyProperties.Split(p => p.NotNull);
                        switch (nullableKeys.Count)
                        {
                            case 0:
                                #region notNullKeys.Count > 1  && nullableKeys.Count == 0

                                switch (otherKeys.Count)
                                {
                                    case 0:
                                        if (otherInstanceProperties.Count == 1)
                                            WriteGuidKeysGetHashCode(writer, guidKeys, otherInstanceProperties[0]);
                                        else
                                            WriteGuidKeysGetHashCode(writer, guidKeys, otherInstanceProperties);
                                        break;
                                    case 1:
                                        if (otherInstanceProperties.Count == 1)
                                            WriteGuidKeysGetHashCode(writer, guidKeys, otherKeys[0], otherInstanceProperties[0]);
                                        else
                                            WriteGuidKeysGetHashCode(writer, guidKeys, otherKeys[0], otherInstanceProperties);
                                        break;
                                    default:
                                        if (otherInstanceProperties.Count == 1)
                                            WriteGuidKeysGetHashCode(writer, guidKeys, otherKeys, otherInstanceProperties[0]);
                                        else
                                            WriteGuidKeysGetHashCode(writer, guidKeys, otherKeys, otherInstanceProperties);
                                        break;
                                }

                                #endregion

                                break;
                            case 1:
                                #region notNullKeys.Count > 1  && nullableKeys.Count == 1

                                switch (otherKeys.Count)
                                {
                                    case 0:
                                        if (guidKeys.Count == 1)
                                        {
                                            if (otherInstanceProperties.Count == 1)
                                                WriteGetHashCode(writer, guidKeys[0], nullableKeys[0], otherInstanceProperties[0]);
                                            else
                                                WriteGetHashCode(writer, guidKeys[0], nullableKeys[0], otherInstanceProperties);
                                        }
                                        else if (otherInstanceProperties.Count == 1)
                                            WriteGetHashCode(writer, guidKeys, nullableKeys[0], otherInstanceProperties[0]);
                                        else
                                            WriteGetHashCode(writer, guidKeys, nullableKeys[0], otherInstanceProperties);
                                        break;
                                    case 1:
                                        if (guidKeys.Count == 1)
                                        {
                                            if (otherInstanceProperties.Count == 1)
                                                WriteGetHashCode(writer, guidKeys[0], nullableKeys[0], otherKeys[0], otherInstanceProperties[0]);
                                            else
                                                WriteGetHashCode(writer, guidKeys[0], nullableKeys[0], otherKeys[0], otherInstanceProperties);
                                        }
                                        else if (otherInstanceProperties.Count == 1)
                                            WriteGetHashCode(writer, guidKeys, nullableKeys[0], otherKeys[0], otherInstanceProperties[0]);
                                        else
                                            WriteGetHashCode(writer, guidKeys, nullableKeys[0], otherKeys[0], otherInstanceProperties);
                                        break;
                                    default:
                                        if (guidKeys.Count == 1)
                                        {
                                            if (otherInstanceProperties.Count == 1)
                                                WriteGetHashCode(writer, guidKeys[0], nullableKeys[0], otherKeys, otherInstanceProperties[0]);
                                            else
                                                WriteGetHashCode(writer, guidKeys[0], nullableKeys[0], otherKeys, otherInstanceProperties);
                                        }
                                        else if (otherInstanceProperties.Count == 1)
                                            WriteGetHashCode(writer, guidKeys, nullableKeys[0], otherKeys, otherInstanceProperties[0]);
                                        else
                                            WriteGetHashCode(writer, guidKeys, nullableKeys[0], otherKeys, otherInstanceProperties);
                                        break;
                                }

                                #endregion

                                break;
                            default:
                                #region notNullKeys.Count > 1  && nullableKeys.Count > 0

                                switch (otherKeys.Count)
                                {
                                    case 0:
                                        switch (guidKeys.Count)
                                        {
                                            case 0:
                                                if (otherInstanceProperties.Count == 1)
                                                    WriteNullableKeysGetHashCode(writer, nullableKeys, otherInstanceProperties[0]);
                                                else
                                                    WriteNullableKeysGetHashCode(writer, nullableKeys, otherInstanceProperties);
                                                break;
                                            case 1:
                                                if (otherInstanceProperties.Count == 1)
                                                    WriteGetHashCode(writer, guidKeys[0], nullableKeys, otherInstanceProperties[0]);
                                                else
                                                    WriteGetHashCode(writer, guidKeys[0], nullableKeys, otherInstanceProperties);
                                                break;
                                            default:
                                                if (otherInstanceProperties.Count == 1)
                                                    WriteGetHashCode(writer, guidKeys, nullableKeys, otherInstanceProperties[0]);
                                                else
                                                    WriteGetHashCode(writer, guidKeys, nullableKeys, otherInstanceProperties);
                                                break;
                                        }
                                        break;
                                    case 1:
                                        switch (guidKeys.Count)
                                        {
                                            case 0:
                                                if (otherInstanceProperties.Count == 1)
                                                    WriteNullableKeysGetHashCode(writer, nullableKeys, otherKeys[0], otherInstanceProperties[0]);
                                                else
                                                    WriteNullableKeysGetHashCode(writer, nullableKeys, otherKeys[0], otherInstanceProperties);
                                                break;
                                            case 1:
                                                if (otherInstanceProperties.Count == 1)
                                                    WriteGetHashCode(writer, guidKeys[0], nullableKeys, otherKeys[0], otherInstanceProperties[0]);
                                                else
                                                    WriteGetHashCode(writer, guidKeys[0], nullableKeys, otherKeys[0], otherInstanceProperties);
                                                break;
                                            default:
                                                if (otherInstanceProperties.Count == 1)
                                                    WriteGetHashCode(writer, guidKeys, nullableKeys, otherKeys[0], otherInstanceProperties[0]);
                                                else
                                                    WriteGetHashCode(writer, guidKeys, nullableKeys, otherKeys[0], otherInstanceProperties);
                                                break;
                                        }
                                        break;
                                    default:
                                        switch (guidKeys.Count)
                                        {
                                            case 0:
                                                if (otherInstanceProperties.Count == 1)
                                                    WriteNullableKeysGetHashCode(writer, nullableKeys, otherKeys, otherInstanceProperties[0]);
                                                else
                                                    WriteNullableKeysGetHashCode(writer, nullableKeys, otherKeys, otherInstanceProperties);
                                                break;
                                            case 1:
                                                if (otherInstanceProperties.Count == 1)
                                                    WriteGetHashCode(writer, guidKeys[0], nullableKeys, otherKeys, otherInstanceProperties[0]);
                                                else
                                                    WriteGetHashCode(writer, guidKeys[0], nullableKeys, otherKeys, otherInstanceProperties);
                                                break;
                                            default:
                                                if (otherInstanceProperties.Count == 1)
                                                    WriteGetHashCode(writer, guidKeys, nullableKeys, otherKeys, otherInstanceProperties[0]);
                                                else
                                                    WriteGetHashCode(writer, guidKeys, nullableKeys, otherKeys, otherInstanceProperties);
                                                break;
                                        }
                                        break;
                                }

                                #endregion

                                break;
                        }

                        #endregion
                    }

                    #endregion
                }
            }
            else if (keyProperties.Count > 1)
                WriteGetHashCode(writer, keyProperties);
            else
                WriteGetHashCode(writer, keyProperties[0]);
            return true;
        }

        /// <summary>
        /// Writes Equals override code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKeys">Two or more unique identifier keys.</param>
        /// <param name="nullableKeys">Two or more nullable keys.</param>
        /// <param name="otherKeys">Two or more other keys.</param>
        /// <param name="otherInstanceProperties">Two or more other instance properties.</param>
        /// <seealso cref="MyClassA1"/>
        private static void WriteEquals(TextWriter writer, Collection<IFieldOrPropertyDescriptor> guidKeys, Collection<IFieldOrPropertyDescriptor> nullableKeys, Collection<IFieldOrPropertyDescriptor> otherKeys, Collection<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            //if (guidKeys.Count + nullableKeys.Count + otherKeys.Count < 9)
            //{
            //    if (guidKeys.Count > nullableKeys.Count)
            //    {
            //        if (otherInstanceProperties.Count < 9)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //    else
            //    {
            //        if (otherInstanceProperties.Count < 9)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //}
            //else
            //{
            //    if (guidKeys.Count > nullableKeys.Count)
            //    {
            //        if (otherInstanceProperties.Count < 9)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //    else
            //    {
            //        if (otherInstanceProperties.Count < 9)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes Equals override code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKeys">Two or more unique identifier keys.</param>
        /// <param name="nullableKeys">Two or more nullable keys.</param>
        /// <param name="otherKeys">Two or more other keys.</param>
        /// <param name="otherInstanceProperty">The other instance property.</param>
        /// <seealso cref="MyClassA2"/>
        private static void WriteEquals(TextWriter writer, Collection<IFieldOrPropertyDescriptor> guidKeys, Collection<IFieldOrPropertyDescriptor> nullableKeys, Collection<IFieldOrPropertyDescriptor> otherKeys, IFieldOrPropertyDescriptor otherInstanceProperty)
        {
            //if (guidKeys.Count + nullableKeys.Count + otherKeys.Count < 9)
            //{
            //    if (guidKeys.Count > nullableKeys.Count)
            //    {
            //        if (otherInstanceProperty.NotNull)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //    else
            //    {
            //        if (otherInstanceProperty.NotNull)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //}
            //else
            //{
            //    if (guidKeys.Count > nullableKeys.Count)
            //    {
            //        if (otherInstanceProperty.NotNull)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //    else
            //    {
            //        if (otherInstanceProperty.NotNull)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes Equals override code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKey">The unique identifier key.</param>
        /// <param name="nullableKeys">Two or more nullable keys.</param>
        /// <param name="otherKeys">Two or more other keys.</param>
        /// <param name="otherInstanceProperties">Two or more other instance properties.</param>
        /// <seealso cref="MyClassB1"/>
        private static void WriteEquals(TextWriter writer, IFieldOrPropertyDescriptor guidKey, Collection<IFieldOrPropertyDescriptor> nullableKeys, Collection<IFieldOrPropertyDescriptor> otherKeys, Collection<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            //if (nullableKeys.Count + otherKeys.Count < 8)
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes Equals override code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKey">The unique identifier key.</param>
        /// <param name="nullableKeys">Two or more nullable keys.</param>
        /// <param name="otherKeys">Two or more other keys.</param>
        /// <param name="otherInstanceProperty">The other instance property.</param>
        /// <seealso cref="MyClassB2"/>
        private static void WriteEquals(TextWriter writer, IFieldOrPropertyDescriptor guidKey, Collection<IFieldOrPropertyDescriptor> nullableKeys, Collection<IFieldOrPropertyDescriptor> otherKeys, IFieldOrPropertyDescriptor otherInstanceProperty)
        {
            //if (nullableKeys.Count + otherKeys.Count < 8)
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes Equals override code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKeys">Two or more unique identifier keys.</param>
        /// <param name="nullableKey">The nullable key.</param>
        /// <param name="otherKeys">Two or more other keys.</param>
        /// <param name="otherInstanceProperties">Two or more other instance properties.</param>
        /// <seealso cref="MyClassC1"/>
        private static void WriteEquals(TextWriter writer, Collection<IFieldOrPropertyDescriptor> guidKeys, IFieldOrPropertyDescriptor nullableKey, Collection<IFieldOrPropertyDescriptor> otherKeys, Collection<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            //if (guidKeys.Count + otherKeys.Count < 8)
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes Equals override code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKeys">Two or more unique identifier keys.</param>
        /// <param name="nullableKey">The nullable key.</param>
        /// <param name="otherKeys">Two or more other keys.</param>
        /// <param name="otherInstanceProperty">The other instance property.</param>
        /// <seealso cref="MyClassC2"/>
        private static void WriteEquals(TextWriter writer, Collection<IFieldOrPropertyDescriptor> guidKeys, IFieldOrPropertyDescriptor nullableKey, Collection<IFieldOrPropertyDescriptor> otherKeys, IFieldOrPropertyDescriptor otherInstanceProperty)
        {
            //if (guidKeys.Count + otherKeys.Count < 8)
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes Equals override code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKey">The unique identifier key.</param>
        /// <param name="nullableKey">The nullable key.</param>
        /// <param name="otherKeys">Two or more other keys.</param>
        /// <param name="otherInstanceProperties">Two or more other instance properties.</param>
        /// <seealso cref="MyClassD1"/>
        private static void WriteEquals(TextWriter writer, IFieldOrPropertyDescriptor guidKey, IFieldOrPropertyDescriptor nullableKey, Collection<IFieldOrPropertyDescriptor> otherKeys, Collection<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            //if (otherKeys.Count < 7)
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes Equals override code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKey">The unique identifier key.</param>
        /// <param name="nullableKey">The nullable key.</param>
        /// <param name="otherKeys">Two or more other keys.</param>
        /// <param name="otherInstanceProperty">The other instance property.</param>
        /// <seealso cref="MyClassD2"/>
        private static void WriteEquals(TextWriter writer, IFieldOrPropertyDescriptor guidKey, IFieldOrPropertyDescriptor nullableKey, Collection<IFieldOrPropertyDescriptor> otherKeys, IFieldOrPropertyDescriptor otherInstanceProperty)
        {
            //if (otherKeys.Count < 7)
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes Equals override code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKeys">Two or more unique identifier keys.</param>
        /// <param name="nullableKeys">Two or more nullable keys.</param>
        /// <param name="otherKey">The other key.</param>
        /// <param name="otherInstanceProperties">Two or more other instance properties.</param>
        /// <seealso cref="MyClassE1"/>
        private static void WriteEquals(TextWriter writer, Collection<IFieldOrPropertyDescriptor> guidKeys, Collection<IFieldOrPropertyDescriptor> nullableKeys, IFieldOrPropertyDescriptor otherKey, Collection<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            //if (guidKeys.Count + nullableKeys.Count < 8)
            //{
            //    if (guidKeys.Count > nullableKeys.Count)
            //    {
            //        if (otherInstanceProperties.Count < 9)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //    else
            //    {
            //        if (otherInstanceProperties.Count < 9)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //}
            //else
            //{
            //    if (guidKeys.Count > nullableKeys.Count)
            //    {
            //        if (otherInstanceProperties.Count < 9)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //    else
            //    {
            //        if (otherInstanceProperties.Count < 9)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes Equals override code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKeys">Two or more unique identifier keys.</param>
        /// <param name="nullableKeys">Two or more nullable keys.</param>
        /// <param name="otherKey">The other key.</param>
        /// <param name="otherInstanceProperty">The other instance property.</param>
        /// <seealso cref="MyClassE2"/>
        private static void WriteEquals(TextWriter writer, Collection<IFieldOrPropertyDescriptor> guidKeys, Collection<IFieldOrPropertyDescriptor> nullableKeys, IFieldOrPropertyDescriptor otherKey, IFieldOrPropertyDescriptor otherInstanceProperty)
        {
            //if (guidKeys.Count + nullableKeys.Count < 8)
            //{
            //    if (guidKeys.Count > nullableKeys.Count)
            //    {
            //        if (otherInstanceProperty.NotNull)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //    else
            //    {
            //        if (otherInstanceProperty.NotNull)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //}
            //else
            //{
            //    if (guidKeys.Count > nullableKeys.Count)
            //    {
            //        if (otherInstanceProperty.NotNull)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //    else
            //    {
            //        if (otherInstanceProperty.NotNull)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes Equals override code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKey">The unique identifier key.</param>
        /// <param name="nullableKeys">Two or more nullable keys.</param>
        /// <param name="otherKey">The other key.</param>
        /// <param name="otherInstanceProperties">Two or more other instance properties.</param>
        /// <seealso cref="MyClassF1"/>
        private static void WriteEquals(TextWriter writer, IFieldOrPropertyDescriptor guidKey, Collection<IFieldOrPropertyDescriptor> nullableKeys, IFieldOrPropertyDescriptor otherKey, Collection<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            //if (nullableKeys.Count < 7)
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes Equals override code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKey">The unique identifier key.</param>
        /// <param name="nullableKeys">Two or more nullable keys.</param>
        /// <param name="otherKey">The other key.</param>
        /// <param name="otherInstanceProperty">The other instance property.</param>
        /// <seealso cref="MyClassF2"/>
        private static void WriteEquals(TextWriter writer, IFieldOrPropertyDescriptor guidKey, Collection<IFieldOrPropertyDescriptor> nullableKeys, IFieldOrPropertyDescriptor otherKey, IFieldOrPropertyDescriptor otherInstanceProperty)
        {
            //if (nullableKeys.Count < 7)
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes Equals override code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKeys">Two or more unique identifier keys.</param>
        /// <param name="nullableKey">The nullable key.</param>
        /// <param name="otherKey">The other key.</param>
        /// <param name="otherInstanceProperties">Two or more other instance properties.</param>
        /// <seealso cref="MyClassG1"/>
        private static void WriteEquals(TextWriter writer, Collection<IFieldOrPropertyDescriptor> guidKeys, IFieldOrPropertyDescriptor nullableKey, IFieldOrPropertyDescriptor otherKey, Collection<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            //if (guidKeys.Count < 7)
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes Equals override code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKeys">Two or more unique identifier keys.</param>
        /// <param name="nullableKey">The nullable key.</param>
        /// <param name="otherKey">The other key.</param>
        /// <param name="otherInstanceProperty">The other instance property.</param>
        /// <seealso cref="MyClassG2"/>
        private static void WriteEquals(TextWriter writer, Collection<IFieldOrPropertyDescriptor> guidKeys, IFieldOrPropertyDescriptor nullableKey, IFieldOrPropertyDescriptor otherKey, IFieldOrPropertyDescriptor otherInstanceProperty)
        {
            //if (guidKeys.Count < 7)
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes Equals override code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKey">The unique identifier key.</param>
        /// <param name="nullableKey">The nullable key.</param>
        /// <param name="otherKey">The other key.</param>
        /// <param name="otherInstanceProperty">The other instance property.</param>
        /// <seealso cref="MyClassH2"/>
        private static void WriteEquals(TextWriter writer, IFieldOrPropertyDescriptor guidKey, IFieldOrPropertyDescriptor nullableKey, IFieldOrPropertyDescriptor otherKey, IFieldOrPropertyDescriptor otherInstanceProperty)
        {
            //if (otherInstanceProperty.NotNull)
            //{

            //}
            //else
            //{

            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes Equals override code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKey">The unique identifier key.</param>
        /// <param name="nullableKey">The nullable key.</param>
        /// <param name="otherKey">The other key.</param>
        /// <param name="otherInstanceProperties">Two or more other instance properties.</param>
        /// <seealso cref="MyClassH1"/>
        private static void WriteEquals(TextWriter writer, IFieldOrPropertyDescriptor guidKey, IFieldOrPropertyDescriptor nullableKey, IFieldOrPropertyDescriptor otherKey, Collection<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            //if (otherInstanceProperties.Count < 9)
            //{

            //}
            //else
            //{

            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes Equals override code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKeys">Two or more unique identifier keys.</param>
        /// <param name="nullableKeys">Two or more nullable keys.</param>
        /// <param name="otherInstanceProperty">The other instance property.</param>
        /// <seealso cref="MyClassI2"/>
        private static void WriteEquals(TextWriter writer, Collection<IFieldOrPropertyDescriptor> guidKeys, Collection<IFieldOrPropertyDescriptor> nullableKeys, IFieldOrPropertyDescriptor otherInstanceProperty)
        {
            //if (guidKeys.Count + nullableKeys.Count < 9)
            //{
            //    if (guidKeys.Count > nullableKeys.Count)
            //    {
            //        if (otherInstanceProperty.NotNull)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //    else
            //    {
            //        if (otherInstanceProperty.NotNull)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //}
            //else
            //{
            //    if (guidKeys.Count > nullableKeys.Count)
            //    {
            //        if (otherInstanceProperty.NotNull)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //    else
            //    {
            //        if (otherInstanceProperty.NotNull)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes Equals override code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKeys">Two or more unique identifier keys.</param>
        /// <param name="nullableKeys">Two or more nullable keys.</param>
        /// <param name="otherInstanceProperties">Two or more other instance properties.</param>
        /// <seealso cref="MyClassI1"/>
        private static void WriteEquals(TextWriter writer, Collection<IFieldOrPropertyDescriptor> guidKeys, Collection<IFieldOrPropertyDescriptor> nullableKeys, Collection<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            //if (guidKeys.Count + nullableKeys.Count < 9)
            //{
            //    if (guidKeys.Count > nullableKeys.Count)
            //    {
            //        if (otherInstanceProperties.Count < 9)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //    else
            //    {
            //        if (otherInstanceProperties.Count < 9)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //}
            //else
            //{
            //    if (guidKeys.Count > nullableKeys.Count)
            //    {
            //        if (otherInstanceProperties.Count < 9)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //    else
            //    {
            //        if (otherInstanceProperties.Count < 9)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes Equals override code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKey">The unique identifier key.</param>
        /// <param name="nullableKeys">Two or more nullable keys.</param>
        /// <param name="otherInstanceProperty">The other instance property.</param>
        /// <seealso cref="MyClassJ2"/>
        private static void WriteEquals(TextWriter writer, IFieldOrPropertyDescriptor guidKey, Collection<IFieldOrPropertyDescriptor> nullableKeys, IFieldOrPropertyDescriptor otherInstanceProperty)
        {
            //if (nullableKeys.Count < 8)
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes Equals override code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKey">The unique identifier key.</param>
        /// <param name="nullableKeys">Two or more nullable keys.</param>
        /// <param name="otherInstanceProperties">Two or more other instance properties.</param>
        /// <seealso cref="MyClassJ1"/>
        private static void WriteEquals(TextWriter writer, IFieldOrPropertyDescriptor guidKey, Collection<IFieldOrPropertyDescriptor> nullableKeys, Collection<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            //if (nullableKeys.Count < 8)
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes Equals override code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKeys">Two or more unique identifier keys.</param>
        /// <param name="nullableKey">The nullable key.</param>
        /// <param name="otherInstanceProperty">The other instance property.</param>
        /// <seealso cref="MyClassK2"/>
        private static void WriteEquals(TextWriter writer, Collection<IFieldOrPropertyDescriptor> guidKeys, IFieldOrPropertyDescriptor nullableKey, IFieldOrPropertyDescriptor otherInstanceProperty)
        {
            //if (guidKeys.Count < 8)
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes Equals override code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKeys">Two or more unique identifier keys.</param>
        /// <param name="nullableKey">The nullable key.</param>
        /// <param name="otherInstanceProperties">Two or more other instance properties.</param>
        /// <seealso cref="MyClassK1"/>
        private static void WriteEquals(TextWriter writer, Collection<IFieldOrPropertyDescriptor> guidKeys, IFieldOrPropertyDescriptor nullableKey, Collection<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            //if (guidKeys.Count < 8)
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes Equals override code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKey">The unique identifier key.</param>
        /// <param name="nullableKey">The nullable key.</param>
        /// <param name="otherInstanceProperty">The other instance property.</param>
        /// <seealso cref="MyClassL2"/>
        private static void WriteEquals(TextWriter writer, IFieldOrPropertyDescriptor guidKey, IFieldOrPropertyDescriptor nullableKey, Collection<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            //if (otherInstanceProperties.Count < 9)
            //{

            //}
            //else
            //{

            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes Equals override code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKey">The unique identifier key.</param>
        /// <param name="nullableKey">The nullable key.</param>
        /// <param name="otherInstanceProperties">Two or more other instance properties.</param>
        /// <seealso cref="MyClassL1"/>
        private static void WriteEquals(TextWriter writer, IFieldOrPropertyDescriptor guidKey, IFieldOrPropertyDescriptor nullableKey, IFieldOrPropertyDescriptor otherInstanceProperty)
        {
            //if (otherInstanceProperty.NotNull)
            //{

            //}
            //else
            //{

            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes Equals override code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="properties">Two or more properties.</param>
        /// <seealso cref="MyClassM1"/>
        private static void WriteEquals(TextWriter writer, Collection<IFieldOrPropertyDescriptor> properties)
        {
            //if (properties.Count < 9)
            //{

            //}
            //else
            //{

            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes Equals override code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="property">The single property.</param>
        /// <seealso cref="MyClassM2"/>
        private static void WriteEquals(TextWriter writer, IFieldOrPropertyDescriptor property)
        {
            //if (property.NotNull)
            //{

            //}
            //else
            //{

            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the unique identifier keys get hash code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKeys">Two or more unique identifier keys.</param>
        /// <param name="otherKeys">Two or more other keys.</param>
        /// <param name="otherInstanceProperties">Two or more other instance properties.</param>
        /// <seealso cref="MyClassN1"/>
        private static void WriteGuidKeysEquals(TextWriter writer, Collection<IFieldOrPropertyDescriptor> guidKeys, Collection<IFieldOrPropertyDescriptor> otherKeys, Collection<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            //if (guidKeys.Count + otherKeys.Count < 9)
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the unique identifier keys get hash code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKeys">Two or more unique identifier keys.</param>
        /// <param name="otherKeys">Two or more other keys.</param>
        /// <param name="otherInstanceProperty">The other instance property.</param>
        /// <seealso cref="MyClassN2"/>
        private static void WriteGuidKeysEquals(TextWriter writer, Collection<IFieldOrPropertyDescriptor> guidKeys, Collection<IFieldOrPropertyDescriptor> otherKeys, IFieldOrPropertyDescriptor otherInstanceProperty)
        {
            //if (guidKeys.Count + otherKeys.Count < 9)
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the unique identifier keys get hash code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKeys">Two or more unique identifier keys.</param>
        /// <param name="otherKey">The other key.</param>
        /// <param name="otherInstanceProperties">Two or more other instance properties.</param>
        /// <seealso cref="MyClassO1"/>
        private static void WriteGuidKeysEquals(TextWriter writer, Collection<IFieldOrPropertyDescriptor> guidKeys, IFieldOrPropertyDescriptor otherKey, Collection<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            //if (guidKeys.Count < 8)
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the unique identifier keys get hash code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKeys">Two or more unique identifier keys.</param>
        /// <param name="otherKey">The other key.</param>
        /// <param name="otherInstanceProperty">The other instance property.</param>
        /// <seealso cref="MyClassO2"/>
        private static void WriteGuidKeysEquals(TextWriter writer, Collection<IFieldOrPropertyDescriptor> guidKeys, IFieldOrPropertyDescriptor otherKey, IFieldOrPropertyDescriptor otherInstanceProperty)
        {
            //if (guidKeys.Count < 8)
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the unique identifier keys get hash code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKeys">Two or more unique identifier keys.</param>
        /// <param name="otherInstanceProperties">Two or more other instance properties.</param>
        /// <seealso cref="MyClassP1"/>
        private static void WriteGuidKeysEquals(TextWriter writer, Collection<IFieldOrPropertyDescriptor> guidKeys, Collection<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            //if (guidKeys.Count < 9)
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the unique identifier keys get hash code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidKeys">Two or more unique identifier keys.</param>
        /// <param name="otherInstanceProperty">The other instance property.</param>
        /// <seealso cref="MyClassP2"/>
        private static void WriteGuidKeysEquals(TextWriter writer, Collection<IFieldOrPropertyDescriptor> guidKeys, IFieldOrPropertyDescriptor otherInstanceProperty)
        {
            //if (guidKeys.Count < 9)
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the nullable keys get hash code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="nullableKeys">Two or more nullable keys.</param>
        /// <param name="otherKeys">Two or more other keys.</param>
        /// <param name="otherInstanceProperties">Two or more other instance properties.</param>
        /// <seealso cref="MyClassQ1"/>
        private static void WriteNullableKeysEquals(TextWriter writer, Collection<IFieldOrPropertyDescriptor> nullableKeys, Collection<IFieldOrPropertyDescriptor> otherKeys, Collection<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            //if (nullableKeys.Count + otherKeys.Count < 9)
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the nullable keys get hash code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="nullableKeys">Two or more nullable keys.</param>
        /// <param name="otherKeys">Two or more other keys.</param>
        /// <param name="otherInstanceProperty">The other instance property.</param>
        /// <seealso cref="MyClassQ2"/>
        private static void WriteNullableKeysEquals(TextWriter writer, Collection<IFieldOrPropertyDescriptor> nullableKeys, Collection<IFieldOrPropertyDescriptor> otherKeys, IFieldOrPropertyDescriptor otherInstanceProperty)
        {
            //if (nullableKeys.Count + otherKeys.Count < 9)
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the nullable keys get hash code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="nullableKeys">Two or more nullable keys.</param>
        /// <param name="otherKey">The other key.</param>
        /// <param name="otherInstanceProperties">Two or more other instance properties.</param>
        /// <seealso cref="MyClassR1"/>
        private static void WriteNullableKeysEquals(TextWriter writer, Collection<IFieldOrPropertyDescriptor> nullableKeys, IFieldOrPropertyDescriptor otherKey, Collection<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            //if (nullableKeys.Count < 8)
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the nullable keys get hash code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="nullableKeys">Two or more nullable keys.</param>
        /// <param name="otherKey">The other key.</param>
        /// <param name="otherInstanceProperty">The other instance property.</param>
        /// <seealso cref="MyClassR2"/>
        private static void WriteNullableKeysEquals(TextWriter writer, Collection<IFieldOrPropertyDescriptor> nullableKeys, IFieldOrPropertyDescriptor otherKey, IFieldOrPropertyDescriptor otherInstanceProperty)
        {
            //if (nullableKeys.Count < 8)
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the nullable keys get hash code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="nullableKeys">Two or more nullable keys.</param>
        /// <param name="otherInstanceProperties">Two or more other instance properties.</param>
        /// <seealso cref="MyClassS1"/>
        private static void WriteNullableKeysEquals(TextWriter writer, Collection<IFieldOrPropertyDescriptor> nullableKeys, Collection<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            //if (nullableKeys.Count < 9)
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the nullable keys get hash code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="nullableKeys">Two or more nullable keys.</param>
        /// <param name="otherInstanceProperty">The other instance property.</param>
        /// <seealso cref="MyClassS2"/>
        private static void WriteNullableKeysEquals(TextWriter writer, Collection<IFieldOrPropertyDescriptor> nullableKeys, IFieldOrPropertyDescriptor otherInstanceProperty)
        {
            //if (nullableKeys.Count < 9)
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the unique identifier or nullable key get hash code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidOrNullableKey">The unique identifier or nullable key.</param>
        /// <param name="otherKeys">Two or more other keys.</param>
        /// <param name="otherInstanceProperties">Two or more other instance properties.</param>
        /// <seealso cref="MyClassT1"/>
        private static void WriteGuidOrNullableKeyEquals(TextWriter writer, IFieldOrPropertyDescriptor guidOrNullableKey, Collection<IFieldOrPropertyDescriptor> otherKeys, Collection<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            //if (otherKeys.Count < 8)
            //{
            //    if (guidOrNullableKey.NotNull)
            //    {
            //        if (otherInstanceProperties.Count < 9)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //    else
            //    {
            //        if (otherInstanceProperties.Count < 9)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //}
            //else
            //{
            //    if (guidOrNullableKey.NotNull)
            //    {
            //        if (otherInstanceProperties.Count < 9)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //    else
            //    {
            //        if (otherInstanceProperties.Count < 9)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the unique identifier or nullable key get hash code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidOrNullableKey">The unique identifier or nullable key.</param>
        /// <param name="otherKeys">Two or more other keys.</param>
        /// <param name="otherInstanceProperty">The other instance property.</param>
        /// <seealso cref="MyClassT2"/>
        private static void WriteGuidOrNullableKeyEquals(TextWriter writer, IFieldOrPropertyDescriptor guidOrNullableKey, Collection<IFieldOrPropertyDescriptor> otherKeys, IFieldOrPropertyDescriptor otherInstanceProperty)
        {
            //if (otherKeys.Count < 8)
            //{
            //    if (guidOrNullableKey.NotNull)
            //    {
            //        if (otherInstanceProperty.NotNull)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //    else
            //    {
            //        if (otherInstanceProperty.NotNull)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //}
            //else
            //{
            //    if (guidOrNullableKey.NotNull)
            //    {
            //        if (otherInstanceProperty.NotNull)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //    else
            //    {
            //        if (otherInstanceProperty.NotNull)
            //        {

            //        }
            //        else
            //        {

            //        }
            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the unique identifier or nullable key get hash code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidOrNullableKey">The unique identifier or nullable key.</param>
        /// <param name="otherKey">The other key.</param>
        /// <param name="otherInstanceProperties">Two or more other instance properties.</param>
        /// <seealso cref="MyClassU1"/>
        private static void WriteGuidOrNullableKeyEquals(TextWriter writer, IFieldOrPropertyDescriptor guidOrNullableKey, IFieldOrPropertyDescriptor otherKey, Collection<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            //if (guidOrNullableKey.NotNull)
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the unique identifier or nullable key get hash code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidOrNullableKey">The unique identifier or nullable key.</param>
        /// <param name="otherKey">The other key.</param>
        /// <param name="otherInstanceProperty">The other instance property.</param>
        /// <seealso cref="MyClassU2"/>
        private static void WriteGuidOrNullableKeyEquals(TextWriter writer, IFieldOrPropertyDescriptor guidOrNullableKey, IFieldOrPropertyDescriptor otherKey, IFieldOrPropertyDescriptor otherInstanceProperty)
        {
            //if (guidOrNullableKey.NotNull)
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the unique identifier or nullable key get hash code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidOrNullableKey">The unique identifier or nullable key.</param>
        /// <param name="otherInstanceProperties">Two or more other instance properties.</param>
        /// <seealso cref="MyClassV1"/>
        private static void WriteGuidOrNullableKeyEquals(TextWriter writer, IFieldOrPropertyDescriptor guidOrNullableKey, Collection<IFieldOrPropertyDescriptor> otherInstanceProperties)
        {
            //if (guidOrNullableKey.NotNull)
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperties.Count < 9)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the unique identifier or nullable key get hash code.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="guidOrNullableKey">The unique identifier or nullable key.</param>
        /// <param name="otherInstanceProperty">The other instance property.</param>
        /// <seealso cref="MyClassV2"/>
        private static void WriteGuidOrNullableKeyEquals(TextWriter writer, IFieldOrPropertyDescriptor guidOrNullableKey, IFieldOrPropertyDescriptor otherInstanceProperty)
        {
            //if (guidOrNullableKey.NotNull)
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{
            //    if (otherInstanceProperty.NotNull)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            throw new NotImplementedException();
        }

        public static bool TryWriteEquals(this EnhancedTypeDescriptor type, TextWriter writer, Func<IFieldOrPropertyDescriptor, bool> predicate = null)
        {
            if (writer is null) throw new ArgumentNullException(nameof(writer));
            if (type is null) return false;
            (
                Collection<IFieldOrPropertyDescriptor> keyProperties,
                Collection<IFieldOrPropertyDescriptor> otherInstanceProperties
            ) = ((predicate is null) ? type.Properties.Select(p => p.EffectiveDescriptor) : type.Properties.Select(p => p.EffectiveDescriptor).Where(predicate)).Split(p => p.IsKey);

            if (keyProperties.Count == 0)
            {
                if (otherInstanceProperties.Count == 0) return false;
                if (otherInstanceProperties.Count > 1)
                    WriteEquals(writer, otherInstanceProperties);
                else
                    WriteEquals(writer, otherInstanceProperties[0]);
            }
            else if (otherInstanceProperties.Count > 0)
            {
                (
                    Collection<IFieldOrPropertyDescriptor> notNullKeys,
                    Collection<IFieldOrPropertyDescriptor> otherKeys
                ) = keyProperties.Split(p => p.NotNull && !p.Type.Type.Equals(GuidType));
                if (notNullKeys.Count == 0)
                {
                    if (otherKeys.Count > 1)
                        WriteEquals(writer, otherKeys);
                    else
                        WriteEquals(writer, otherKeys[0]);
                }
                else
                {
                    #region notNullKeys.Count > 0

                    if (notNullKeys.Count == 1)
                        switch (otherKeys.Count)
                        {
                            case 0:
                                if (otherInstanceProperties.Count == 1)
                                    WriteGuidOrNullableKeyEquals(writer, notNullKeys[0], otherInstanceProperties[0]);
                                else
                                    WriteGuidOrNullableKeyEquals(writer, notNullKeys[0], otherInstanceProperties);
                                break;
                            case 1:
                                if (otherInstanceProperties.Count == 1)
                                    WriteGuidOrNullableKeyEquals(writer, notNullKeys[0], otherKeys[0], otherInstanceProperties[0]);
                                else
                                    WriteGuidOrNullableKeyEquals(writer, notNullKeys[0], otherKeys[0], otherInstanceProperties);
                                break;
                            default:
                                if (otherInstanceProperties.Count == 1)
                                    WriteGuidOrNullableKeyEquals(writer, notNullKeys[0], otherKeys, otherInstanceProperties[0]);
                                else
                                    WriteGuidOrNullableKeyEquals(writer, notNullKeys[0], otherKeys, otherInstanceProperties);
                                break;
                        }
                    else
                    {
                        #region notNullKeys.Count > 1

                        (
                            Collection<IFieldOrPropertyDescriptor> guidKeys,
                            Collection<IFieldOrPropertyDescriptor> nullableKeys
                        ) = keyProperties.Split(p => p.NotNull);
                        switch (nullableKeys.Count)
                        {
                            case 0:
                                #region notNullKeys.Count > 1  && nullableKeys.Count == 0

                                switch (otherKeys.Count)
                                {
                                    case 0:
                                        if (otherInstanceProperties.Count == 1)
                                            WriteGuidKeysEquals(writer, guidKeys, otherInstanceProperties[0]);
                                        else
                                            WriteGuidKeysEquals(writer, guidKeys, otherInstanceProperties);
                                        break;
                                    case 1:
                                        if (otherInstanceProperties.Count == 1)
                                            WriteGuidKeysEquals(writer, guidKeys, otherKeys[0], otherInstanceProperties[0]);
                                        else
                                            WriteGuidKeysEquals(writer, guidKeys, otherKeys[0], otherInstanceProperties);
                                        break;
                                    default:
                                        if (otherInstanceProperties.Count == 1)
                                            WriteGuidKeysEquals(writer, guidKeys, otherKeys, otherInstanceProperties[0]);
                                        else
                                            WriteGuidKeysEquals(writer, guidKeys, otherKeys, otherInstanceProperties);
                                        break;
                                }

                                #endregion

                                break;
                            case 1:
                                #region notNullKeys.Count > 1  && nullableKeys.Count == 1

                                switch (otherKeys.Count)
                                {
                                    case 0:
                                        if (guidKeys.Count == 1)
                                        {
                                            if (otherInstanceProperties.Count == 1)
                                                WriteEquals(writer, guidKeys[0], nullableKeys[0], otherInstanceProperties[0]);
                                            else
                                                WriteEquals(writer, guidKeys[0], nullableKeys[0], otherInstanceProperties);
                                        }
                                        else if (otherInstanceProperties.Count == 1)
                                            WriteEquals(writer, guidKeys, nullableKeys[0], otherInstanceProperties[0]);
                                        else
                                            WriteEquals(writer, guidKeys, nullableKeys[0], otherInstanceProperties);
                                        break;
                                    case 1:
                                        if (guidKeys.Count == 1)
                                        {
                                            if (otherInstanceProperties.Count == 1)
                                                WriteEquals(writer, guidKeys[0], nullableKeys[0], otherKeys[0], otherInstanceProperties[0]);
                                            else
                                                WriteEquals(writer, guidKeys[0], nullableKeys[0], otherKeys[0], otherInstanceProperties);
                                        }
                                        else if (otherInstanceProperties.Count == 1)
                                            WriteEquals(writer, guidKeys, nullableKeys[0], otherKeys[0], otherInstanceProperties[0]);
                                        else
                                            WriteEquals(writer, guidKeys, nullableKeys[0], otherKeys[0], otherInstanceProperties);
                                        break;
                                    default:
                                        if (guidKeys.Count == 1)
                                        {
                                            if (otherInstanceProperties.Count == 1)
                                                WriteEquals(writer, guidKeys[0], nullableKeys[0], otherKeys, otherInstanceProperties[0]);
                                            else
                                                WriteEquals(writer, guidKeys[0], nullableKeys[0], otherKeys, otherInstanceProperties);
                                        }
                                        else if (otherInstanceProperties.Count == 1)
                                            WriteEquals(writer, guidKeys, nullableKeys[0], otherKeys, otherInstanceProperties[0]);
                                        else
                                            WriteEquals(writer, guidKeys, nullableKeys[0], otherKeys, otherInstanceProperties);
                                        break;
                                }

                                #endregion

                                break;
                            default:
                                #region notNullKeys.Count > 1  && nullableKeys.Count > 0

                                switch (otherKeys.Count)
                                {
                                    case 0:
                                        switch (guidKeys.Count)
                                        {
                                            case 0:
                                                if (otherInstanceProperties.Count == 1)
                                                    WriteNullableKeysEquals(writer, nullableKeys, otherInstanceProperties[0]);
                                                else
                                                    WriteNullableKeysEquals(writer, nullableKeys, otherInstanceProperties);
                                                break;
                                            case 1:
                                                if (otherInstanceProperties.Count == 1)
                                                    WriteEquals(writer, guidKeys[0], nullableKeys, otherInstanceProperties[0]);
                                                else
                                                    WriteEquals(writer, guidKeys[0], nullableKeys, otherInstanceProperties);
                                                break;
                                            default:
                                                if (otherInstanceProperties.Count == 1)
                                                    WriteEquals(writer, guidKeys, nullableKeys, otherInstanceProperties[0]);
                                                else
                                                    WriteEquals(writer, guidKeys, nullableKeys, otherInstanceProperties);
                                                break;
                                        }
                                        break;
                                    case 1:
                                        switch (guidKeys.Count)
                                        {
                                            case 0:
                                                if (otherInstanceProperties.Count == 1)
                                                    WriteNullableKeysEquals(writer, nullableKeys, otherKeys[0], otherInstanceProperties[0]);
                                                else
                                                    WriteNullableKeysEquals(writer, nullableKeys, otherKeys[0], otherInstanceProperties);
                                                break;
                                            case 1:
                                                if (otherInstanceProperties.Count == 1)
                                                    WriteEquals(writer, guidKeys[0], nullableKeys, otherKeys[0], otherInstanceProperties[0]);
                                                else
                                                    WriteEquals(writer, guidKeys[0], nullableKeys, otherKeys[0], otherInstanceProperties);
                                                break;
                                            default:
                                                if (otherInstanceProperties.Count == 1)
                                                    WriteEquals(writer, guidKeys, nullableKeys, otherKeys[0], otherInstanceProperties[0]);
                                                else
                                                    WriteEquals(writer, guidKeys, nullableKeys, otherKeys[0], otherInstanceProperties);
                                                break;
                                        }
                                        break;
                                    default:
                                        switch (guidKeys.Count)
                                        {
                                            case 0:
                                                if (otherInstanceProperties.Count == 1)
                                                    WriteNullableKeysEquals(writer, nullableKeys, otherKeys, otherInstanceProperties[0]);
                                                else
                                                    WriteNullableKeysEquals(writer, nullableKeys, otherKeys, otherInstanceProperties);
                                                break;
                                            case 1:
                                                if (otherInstanceProperties.Count == 1)
                                                    WriteEquals(writer, guidKeys[0], nullableKeys, otherKeys, otherInstanceProperties[0]);
                                                else
                                                    WriteEquals(writer, guidKeys[0], nullableKeys, otherKeys, otherInstanceProperties);
                                                break;
                                            default:
                                                if (otherInstanceProperties.Count == 1)
                                                    WriteEquals(writer, guidKeys, nullableKeys, otherKeys, otherInstanceProperties[0]);
                                                else
                                                    WriteEquals(writer, guidKeys, nullableKeys, otherKeys, otherInstanceProperties);
                                                break;
                                        }
                                        break;
                                }

                                #endregion

                                break;
                        }

                        #endregion
                    }

                    #endregion
                }
            }
            else if (keyProperties.Count > 1)
                WriteEquals(writer, keyProperties);
            else
                WriteEquals(writer, keyProperties[0]);
            return true;
        }
    }
}

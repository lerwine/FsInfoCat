using FsInfoCat.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FsInfoCat.Internal
{
    class ComparisonService : IComparisonService
    {
        private static readonly MethodInfo GetComparerMethodDefinition;
        private static readonly MethodInfo GetEqualityComparerMethodDefinition0;
        private static readonly MethodInfo GetEqualityComparerMethodDefinition1;
        private static readonly MethodInfo GetComponentComparerMethodDefinition;
        private static readonly MethodInfo GetComponentEqualityComparerMethodDefinition;

        static ComparisonService()
        {
            GetComparerMethodDefinition = typeof(IComparisonService).GetMethod(nameof(GetComparer), Array.Empty<Type>());
            GetEqualityComparerMethodDefinition0 = typeof(IComparisonService).GetMethod(nameof(GetEqualityComparer), Array.Empty<Type>());
            GetEqualityComparerMethodDefinition1 = typeof(IComparisonService).GetMethod(nameof(GetEqualityComparer), new Type[] { typeof(bool) });
            GetComponentComparerMethodDefinition = typeof(IComparisonService).GetMethod(nameof(GetComponentComparer), Array.Empty<Type>());
            GetComponentEqualityComparerMethodDefinition = typeof(IComparisonService).GetMethod(nameof(GetComponentEqualityComparer), Array.Empty<Type>());
        }

        public ICoersion GetDefaultCoersion(Type type) => (ICoersion)typeof(Coersion<>).MakeGenericType(type ?? throw new ArgumentNullException(nameof(type)))
            .GetField(nameof(Coersion<string>.Default)).GetValue(null);

        public ICoersion<T> GetDefaultCoersion<T>() => Coersion<T>.Default;

        public IComparer<T> GetComparer<T>() => (IComparer<T>)GetComparer(typeof(T), false);

        public IComparer<T> GetComparer<T>(bool noPropertyComparer) => (IComparer<T>)GetComparer(typeof(T), noPropertyComparer);

        public IComparer GetComparer(Type type) => GetComparer(type, false);

        public IComparer GetComparer(Type type, bool noPropertyComparer)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            if (noPropertyComparer || type.IsPrimitive || type.IsEnum || type.IsValueType || type.Equals(typeof(string)) || type.IsSelfComparable())
                return GetDefaultComparer(type);
            if (type.HasTypeConverter())
                return GetConvertingComparer(type);
            if (type.GetDefaultPropertyDescriptor() is null)
                return GetDefaultComparer(type);
            return GetComponentComparer(type);
        }

        public IEqualityComparer<T> GetConvertingEqualityComparer<T>() where T : class => TypeConvertingEqualityComparer<T>.Default;

        public IEqualityComparer GetConvertingEqualityComparer(Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            return (IEqualityComparer)typeof(TypeConvertingEqualityComparer<>).MakeGenericType(type).GetField(nameof(TypeConvertingEqualityComparer<string>.Default)).GetValue(null);
        }

        public IComparer<T> GetConvertingComparer<T>() where T : class => TypeConvertingEqualityComparer<T>.Default;

        public IComparer GetConvertingComparer(Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            return (IComparer)typeof(TypeConvertingEqualityComparer<>).MakeGenericType(type).GetField(nameof(TypeConvertingEqualityComparer<string>.Default)).GetValue(null);
        }

        public IEqualityComparer<T> GetEqualityComparer<T>() => (IEqualityComparer<T>)GetEqualityComparer(typeof(T), false);

        public IEqualityComparer<T> GetEqualityComparer<T>(bool noPropertyComparer) => (IEqualityComparer<T>)GetEqualityComparer(typeof(T), noPropertyComparer);

        public IEqualityComparer GetEqualityComparer(Type type) => GetEqualityComparer(type, false);

        public IEqualityComparer GetEqualityComparer(Type type, bool noPropertyComparer)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            if (type.IsPrimitive || type.IsValueType || type.Equals(typeof(string)) || type.IsSelfComparable())
                return GetDefaultEqualityComparer(type);
            if (type.HasTypeConverter())
                return GetConvertingEqualityComparer(type);
            if (!noPropertyComparer && type.GetPropertyDescriptors().Any())
                return GetComponentEqualityComparer(type);
            return GetDefaultEqualityComparer(type);
        }

        public IEqualityComparer<T> GetComparableEqualityComparer<T>() where T : class, IComparable<T> => ComparableEqualityComparer<T>.Default;

        public IEqualityComparer GetComparableEqualityComparer(Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            return (IEqualityComparer)typeof(ComparableEqualityComparer<>).MakeGenericType(type).GetField(nameof(ComparableEqualityComparer<string>.Default)).GetValue(null);
        }

        public IComparer<T> GetComponentComparer<T>() where T : class => ComponentEqualityComparer<T>.Default;

        public IComparer GetComponentComparer(Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            return (IComparer)typeof(ComponentEqualityComparer<>).MakeGenericType(type).GetField(nameof(ComponentEqualityComparer<object>.Default)).GetValue(null);
        }

        public IEqualityComparer<T> GetComponentEqualityComparer<T>() where T : class => ComponentEqualityComparer<T>.Default;

        public IEqualityComparer GetComponentEqualityComparer(Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            return (IEqualityComparer)typeof(ComponentEqualityComparer<>).MakeGenericType(type).GetField(nameof(ComponentEqualityComparer<object>.Default)).GetValue(null);
        }

        public IEqualityComparer GetDefaultEqualityComparer(Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            return (IEqualityComparer)typeof(EqualityComparer<>).MakeGenericType(type).GetProperty(nameof(EqualityComparer<object>.Default)).GetValue(null);
        }

        public IComparer GetDefaultComparer(Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            return (IComparer)typeof(Comparer<>).MakeGenericType(type).GetProperty(nameof(Comparer<object>.Default)).GetValue(null);
        }
    }
}

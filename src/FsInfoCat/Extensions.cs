using FsInfoCat.Collections;
using FsInfoCat.Providers;
using System;
using System.Collections.Generic;

namespace FsInfoCat
{
    public static class Extensions
    {
        public static IEqualityComparer<T> ToEqualityComparer<T>(this IComparer<T> comparer)
        {
            if (comparer is null)
                return EqualityComparer<T>.Default;
            if (comparer is IEqualityComparer<T> equalityComparer)
                return equalityComparer;
            return new ComparerToEqualityComparer<T>(comparer);
        }

        public static IGeneralizableOrderAndEqualityComparer<T> ToGeneralizableOrderAndEqualityComparer<T>(this IEqualityComparer<T> comparer)
        {
            if (comparer is null)
                return GeneralizableOrderAndEqualityComparer<T>.Default;
            if (comparer is IGeneralizableOrderAndEqualityComparer<T> g)
                return g;
            if (comparer is IComparer<T> c)
                return new GeneralizableOrderAndEqualityComparer<T>(comparer, c);
            return new GeneralizableOrderAndEqualityComparer<T>(comparer, null);
        }

        public static IGeneralizableOrderAndEqualityComparer<T> ToGeneralizableOrderAndEqualityComparer<T>(this IComparer<T> comparer)
        {
            if (comparer is null)
                return GeneralizableOrderAndEqualityComparer<T>.Default;
            if (comparer is IGeneralizableOrderAndEqualityComparer<T> g)
                return g;
            if (comparer is IEqualityComparer<T> c)
                return new GeneralizableOrderAndEqualityComparer<T>(c, comparer);
            return new GeneralizableOrderAndEqualityComparer<T>(null, comparer);
        }

        public static IGeneralizableEqualityComparer<T> ToGeneralizableEqualityComparer<T>(this IEqualityComparer<T> comparer)
        {
            if (comparer is null)
                return GeneralizableEqualityComparer<T>.Default;
            if (comparer is IGeneralizableEqualityComparer<T> g)
                return g;
            return new GeneralizableEqualityComparer<T>(comparer);
        }

        public static IGeneralizableEqualityComparer<T> ToGeneralizableEqualityComparer<T>(this IComparer<T> comparer)
        {
            if (comparer is null)
                return GeneralizableEqualityComparer<T>.Default;
            if (comparer is IGeneralizableEqualityComparer<T> g)
                return g;
            return new GeneralizableEqualityComparer<T>(comparer);
        }

        public static IGeneralizableComparer<T> ToGeneralizableComparer<T>(this IEqualityComparer<T> comparer)
        {
            if (comparer is null)
                return GeneralizableComparer<T>.Default;
            if (comparer is IGeneralizableComparer<T> g)
                return g;
            //return new GeneralizableComparer<T>(comparer);
            throw new NotImplementedException();
        }

        public static IGeneralizableComparer<T> ToGeneralizableComparer<T>(this IComparer<T> comparer)
        {
            if (comparer is null)
                return GeneralizableComparer<T>.Default;
            if (comparer is IGeneralizableComparer<T> g)
                return g;
            return new GeneralizableComparer<T>(comparer);
        }

        public static Predicate<T> ToPredicate<T>(this Func<T, bool> function) => new Predicate<T>(function);

        public static Func<T, bool> ToFunc<T>(this Predicate<T> predicate) => new Func<T, bool>(predicate);

        public static T GetService<T>(this IServiceProvider serviceProvider)
        {
            if (serviceProvider is null)
                throw new ArgumentNullException(nameof(serviceProvider));
            object service = serviceProvider.GetService(typeof(T));
            if (service is null)
            {
                if (serviceProvider is IProducer<T> p)
                    return p.Produce();
                if (serviceProvider is T t)
                    return t;
            }
            return (T)service;
        }

        public static TService GetService<TService, TArg>(this IServiceProvider<TArg> serviceProvider, TArg arg)
        {
            if (serviceProvider is null)
                throw new ArgumentNullException(nameof(serviceProvider));
            object service = serviceProvider.GetService(typeof(TService), arg);
            if (service is null && serviceProvider is IProducer<TArg, TService> c)
                return c.Produce(arg);
            return (TService)service;
        }

        public static TService GetService<TService, TArg1, TArg2>(this IServiceProvider<TArg1, TArg2> serviceProvider, TArg1 arg1, TArg2 arg2)
        {
            if (serviceProvider is null)
                throw new ArgumentNullException(nameof(serviceProvider));
            object service = serviceProvider.GetService(typeof(TService), arg1, arg2);
            if (service is null && serviceProvider is IProducer<TArg1, TArg2, TService> c)
                return c.Produce(arg1, arg2);
            return (TService)service;
        }

        public static TService GetService<TService, TArg1, TArg2, TArg3>(this IServiceProvider<TArg1, TArg2, TArg3> serviceProvider,
            TArg1 arg1, TArg2 arg2, TArg3 arg3)
        {
            if (serviceProvider is null)
                throw new ArgumentNullException(nameof(serviceProvider));
            object service = serviceProvider.GetService(typeof(TService), arg1, arg2, arg3);
            if (service is null && serviceProvider is IProducer<TArg1, TArg2, TArg3, TService> c)
                return c.Produce(arg1, arg2, arg3);
            return (TService)service;
        }

        public static bool TryGetService<T>(this IServiceProvider serviceProvider, out T service)
        {
            if (serviceProvider is null)
            {
                service = default;
                return false;
            }
            object obj;
            try { obj = serviceProvider.GetService(typeof(T)); }
            catch
            {
                service = default;
                return false;
            }
            if (obj is null)
            {
                if (serviceProvider is IProducer<T> p)
                {
                    try { service = p.Produce(); }
                    catch
                    {
                        service = default;
                        return false;
                    }
                    return true;
                }
                if (serviceProvider is T t)
                {
                    service = t;
                    return true;
                }
            }
            if (obj is T s)
            {
                service = s;
                return true;
            }
            service = default;
            return false;
        }

        public static bool TryGetService<TArg, TResult>(this IServiceProvider<TArg> serviceProvider, TArg arg, out TResult service)
        {
            if (serviceProvider is null)
            {
                service = default;
                return false;
            }
            object obj;
            try { obj = serviceProvider.GetService(typeof(TResult), arg); }
            catch
            {
                service = default;
                return false;
            }
            if (obj is null)
            {
                if (serviceProvider is IProducer<TArg, TResult> p)
                {
                    try { service = p.Produce(arg); }
                    catch
                    {
                        service = default;
                        return false;
                    }
                    return true;
                }
            }
            if (obj is TResult s)
            {
                service = s;
                return true;
            }
            service = default;
            return false;
        }

        public static bool TryGetService<TArg1, TArg2, TResult>(this IServiceProvider<TArg1, TArg2> serviceProvider, TArg1 arg1, TArg2 arg2, out TResult service)
        {
            if (serviceProvider is null)
            {
                service = default;
                return false;
            }
            object obj;
            try { obj = serviceProvider.GetService(typeof(TResult), arg1, arg2); }
            catch
            {
                service = default;
                return false;
            }
            if (obj is null)
            {
                if (serviceProvider is IProducer<TArg1, TArg2, TResult> p)
                {
                    try { service = p.Produce(arg1, arg2); }
                    catch
                    {
                        service = default;
                        return false;
                    }
                    return true;
                }
            }
            if (obj is TResult s)
            {
                service = s;
                return true;
            }
            service = default;
            return false;
        }

        public static bool TryGetService<TArg1, TArg2, TArg3, TResult>(this IServiceProvider<TArg1, TArg2, TArg3> serviceProvider,
            TArg1 arg1, TArg2 arg2, TArg3 arg3, out TResult service)
        {
            if (serviceProvider is null)
            {
                service = default;
                return false;
            }
            object obj;
            try { obj = serviceProvider.GetService(typeof(TResult), arg1, arg2, arg3); }
            catch
            {
                service = default;
                return false;
            }
            if (obj is null)
            {
                if (serviceProvider is IProducer<TArg1, TArg2, TArg3, TResult> p)
                {
                    try { service = p.Produce(arg1, arg2, arg3); }
                    catch
                    {
                        service = default;
                        return false;
                    }
                    return true;
                }
            }
            if (obj is TResult s)
            {
                service = s;
                return true;
            }
            service = default;
            return false;
        }
    }
}

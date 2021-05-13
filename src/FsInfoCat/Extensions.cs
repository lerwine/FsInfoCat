using FsInfoCat.Providers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;

namespace FsInfoCat
{
    public static class Extensions
    {
        public static readonly IServiceProvider ServiceProvider = new ServiceCollection()
            .AddSingleton<Services.IComparisonService, Internal.ComparisonService>()
            .AddSingleton<Services.ISuspendable, Internal.Suspendable>()
            .AddSingleton<Services.ISuspendableService, Internal.SuspendableService>()
            .BuildServiceProvider();

        public static bool IsNullableType(this Type type) => !(type is null) && type.IsValueType && type.IsGenericType && typeof(Nullable<>).Equals(type.GetGenericTypeDefinition());

        public static bool IsSelfEquatable(this Type type)
        {
            if (type is null)
                return false;
            Type t = typeof(IEquatable<>);
            return (type.IsInterface && type.IsGenericType && type.GetGenericTypeDefinition().Equals(t)) ||
                t.MakeGenericType(type).IsAssignableFrom(type);
        }

        public static bool IsSelfComparable(this Type type)
        {
            if (type is null)
                return false;
            Type t = typeof(IComparable<>);
            return (type.IsInterface && type.IsGenericType && type.GetGenericTypeDefinition().Equals(t)) ||
                t.MakeGenericType(type).IsAssignableFrom(type);
        }

        public static ISuspensionProvider NewSuspensionProvider() => new Internal.SuspensionProvider();

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

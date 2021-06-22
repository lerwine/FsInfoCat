using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FsInfoCat
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class NormalizationAttribute : System.Attribute
    {
        /// <summary>
        /// Gets the name of the normalization method.
        /// </summary>
        /// <value>
        /// The name of the normalization method.
        /// </value>
        public string Method { get; }

        /// <summary>
        /// Gets the type of the class that contains the normalization method.
        /// </summary>
        /// <value>
        /// The type of the class that contains the normalization method.
        /// </value>
        public Type NormalizerType { get; }

        /// <summary>
        /// Gets the method validation error.
        /// </summary>
        /// <value>
        /// The method validation error that occurred when the attribute was instantiated.
        /// </value>
        public Exception Error { get; }

        public NormalizationAttribute(Type normalizerType, string method)
        {
            NormalizerType = normalizerType;
            try
            {
                if (string.IsNullOrWhiteSpace(method))
                {
                    Method = null;
                    throw new ArgumentNullException(nameof(method));
                }
                Method = method;
                if (!(normalizerType is null || normalizerType.GetMethods(BindingFlags.Static).Any(m => m.Name == method)))
                    throw new ArgumentOutOfRangeException(nameof(method));
                Error = null;
            }
            catch (Exception exception) { Error = exception; }
        }

        private static IEnumerable<Tuple<MethodInfo, Type[]>> GetViableMethods(Type normalizerType, Type returnType, string methodName, bool includeInstance) =>
            (includeInstance ? normalizerType.GetMethods(BindingFlags.Instance).Concat(normalizerType.GetMethods(BindingFlags.Static)) :
            normalizerType.GetMethods(BindingFlags.Static)).Where(m => m.Name == methodName && returnType.IsAssignableFrom(m.ReturnType))
            .Select(m => new
            {
                M = m,
                P = m.GetParameters()
            }).Where(t => t.P.Length > 0 && !t.P.Any(p => p.IsOut || p.IsOptional) && t.P[0].ParameterType.IsAssignableFrom(returnType))
            .Select(a => new Tuple<MethodInfo, Type[]>(a.M, a.P.Skip(1).Select(p => p.ParameterType).ToArray()));

        public T Normalize<T>(object component, string propertyName, T value)
        {
            if (!(Error is null))
                throw new InvalidOperationException((Method is null) ? "Method not defined" : "Method not found", Error);
            Type normalizerType = NormalizerType;
            IEnumerable<Tuple<MethodInfo, Type[]>> viableMethods;
            IEnumerable<Tuple<MethodInfo, Type[]>> matching;
            if (normalizerType is null)
            {
                normalizerType = (component ?? throw new ArgumentNullException(nameof(component))).GetType();
                viableMethods = GetViableMethods(normalizerType, typeof(T), Method, true);
            }
            else
            {
                viableMethods = GetViableMethods(normalizerType, typeof(T), Method, false);
                if (!(component is null))
                {
                    Type c = component.GetType();
                    if ((matching = viableMethods.Where(t => t.Item2.Length == 2 && t.Item2[0].IsAssignableFrom(typeof(string)) &&
                        t.Item2[1].IsAssignableFrom(c))).Any())
                    {
                        if (matching.Skip(1).Any())
                            throw new AmbiguousMatchException();
                        return (T)matching.First().Item1.Invoke(null, new object[] { value, propertyName, component });
                    }
                    if ((matching = viableMethods.Where(t => t.Item2.Length == 2 && t.Item2[0].IsAssignableFrom(c) &&
                        t.Item2[1].IsAssignableFrom(typeof(string)))).Any())
                    {
                        if (matching.Skip(1).Any())
                            throw new AmbiguousMatchException();
                        return (T)matching.First().Item1.Invoke(null, new object[] { value, component, propertyName });
                    }
                    if ((matching = viableMethods.Where(t => t.Item2.Length == 1 && t.Item2[0].IsAssignableFrom(c))).Any())
                    {
                        if (matching.Skip(1).Any())
                            throw new AmbiguousMatchException();
                        return (T)matching.First().Item1.Invoke(null, new object[] { value, component });
                    }
                }
            }
            object[] arguments;
            if ((matching = viableMethods.Where(t => t.Item2.Length == 1 && t.Item2[0].IsAssignableFrom(typeof(string)))).Any())
                arguments = new object[] { value, propertyName };
            else if ((matching = viableMethods.Where(t => t.Item2.Length == 0)).Any())
                arguments = new object[] { value };
            else
                throw new MissingMethodException(normalizerType.FullName, Method);
            if (matching.Skip(1).Any())
                throw new AmbiguousMatchException();
            MethodInfo method = matching.First().Item1;
            return (T)method.Invoke(method.IsStatic ? null : component, arguments);
        }
    }
}

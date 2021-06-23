using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FsInfoCat
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class ServiceBuilderHandlerAttribute : Attribute
    {
        public int Priority { get; set; }

        public static void InvokeHandlers(IServiceCollection services)
        {
            object[] parameters = { services };
            foreach (MethodInfo method in GetHandlers())
                _ = method.Invoke(null, parameters);
        }

        public static IEnumerable<MethodInfo> GetHandlers(Type type)
        {
            Type r = typeof(void);
            Type a = typeof(IServiceCollection);
            ParameterInfo[] parameters;
            return type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).Select(m => (Method: m, Attribute: m.GetCustomAttribute<ServiceBuilderHandlerAttribute>()))
                .Where(t => t.Attribute is not null && t.Method.ReturnType.Equals(r) && (parameters = t.Method.GetParameters()).Length == 1 && parameters[0].ParameterType.Equals(a) && !(parameters[0].IsOut || parameters[0].IsRetval))
                .OrderBy(t => t.Attribute.Priority).Select(t => t.Method);
        }

        public static IEnumerable<MethodInfo> GetHandlers(Assembly assembly) => assembly.GetTypes().SelectMany(t => GetHandlers(t));

        public static IEnumerable<MethodInfo> GetHandlers() => AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => GetHandlers(a));
    }
}

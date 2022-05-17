using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FsInfoCat
{
    // TODO: Document ServiceBuilderHandlerAttribute class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class ServiceBuilderHandlerAttribute : Attribute
    {
        public int Priority { get; set; }

        public static IEnumerable<(MethodInfo Method, bool PassContext, int Order)> GetHandlers(Assembly[] assemblies)
        {
            //// Filter assemblies by name to make loading faster.
            //string f = nameof(FsInfoCat);
            //string f2 = $"{f}.";
            Type r = typeof(void);
            Type a = typeof(IServiceCollection);
            Type b = typeof(HostBuilderContext);
            foreach (Assembly assembly in assemblies)
            {
                System.Diagnostics.Debug.WriteLine($"Getting methods for {assembly.FullName}");
                foreach (Type type in assembly.GetTypes())
                {
                    foreach (MethodInfo method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
                    {
                        ServiceBuilderHandlerAttribute attribute = method.GetCustomAttribute<ServiceBuilderHandlerAttribute>();
                        if (attribute is not null && method.ReturnType.Equals(r))
                        {
                            ParameterInfo[] parameters = method.GetParameters();
                            ParameterInfo p1;
                            bool passContext = parameters.Length == 2;
                            if (passContext)
                            {
                                ParameterInfo p2 = parameters[1];
                                if (!p2.ParameterType.IsAssignableFrom(b) || p2.IsOut)
                                    continue;
                            }
                            else if (parameters.Length > 1)
                                continue;
                            p1 = parameters[0];
                            if (p1.ParameterType.IsAssignableFrom(a) && !p1.IsOut)
                                yield return (method, passContext, attribute.Priority);
                        }
                    }
                }
            }
        }

        public static void InvokeHandlers(HostBuilderContext context, IServiceCollection services, params Assembly[] assemblies)
        {
            object[] p1 = { services };
            object[] p2 = { services, context };
            foreach ((MethodInfo Method, bool PassContext, int Order) handler in GetHandlers(assemblies).OrderBy(t => t.Order))
                _ = handler.Method.Invoke(null, handler.PassContext ? p2 : p1);
            System.Diagnostics.Debug.WriteLine("Handlers initialized");
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}

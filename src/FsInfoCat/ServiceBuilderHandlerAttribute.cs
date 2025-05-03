using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Reflection;

namespace FsInfoCat
{
    /// <summary>
    /// Indicates that a method is a handler for configuring the <see cref="IServiceCollection"/> for the <see cref="Hosting.Host"/>.
    /// </summary>
    /// <remarks>The method return type should be <see langword="void"/> with 1 or 2 parameters. The first parameter must be of type <see cref="IServiceCollection"/>,
    /// and the second parameter, if present, must be of type <see cref="HostBuilderContext"/>.</remarks>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class ServiceBuilderHandlerAttribute : HostingInitializationHandlerAttribute
    {
        /// <summary>
        /// Invokes the service collection initialization handlers.
        /// </summary>
        /// <param name="context">The host builder context.</param>
        /// <param name="services">The services collection being initialized.</param>
        /// <param name="assemblies">The assemblies to search for handler methods.</param>
        public static void InvokeHandlers(HostBuilderContext context, IServiceCollection services, params Assembly[] assemblies)
        {
            object[] p1 = [services];
            object[] p2 = [services, context];
            foreach (HandlerInfo handler in GetHandlers<ServiceBuilderHandlerAttribute>(typeof(IServiceCollection), assemblies).OrderBy(h => h.Priority))
                _ = handler.Method.Invoke(null, handler.PassContext ? p2 : p1);
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"Methods marked with {nameof(ServiceBuilderHandlerAttribute)} invoked.");
#endif
        }
    }
}

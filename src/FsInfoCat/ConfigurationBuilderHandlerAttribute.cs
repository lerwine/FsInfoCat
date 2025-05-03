using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Reflection;

namespace FsInfoCat
{
    /// <summary>
    /// Indicates that a method is a handler for initializing the <see cref="IConfigurationBuilder"/> for the <see cref="Hosting.Host"/>.
    /// </summary>
    /// <remarks>The method will only be invoked if the return type is <see langword="void"/> and it has 1 or 2 parameters. The first parameter must be of type <see cref="IConfigurationBuilder"/>,
    /// and the second parameter, if present, must be of type <see cref="HostBuilderContext"/>.</remarks>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class ConfigurationBuilderHandlerAttribute : HostingInitializationHandlerAttribute
    {
        /// <summary>
        /// Invokes the configuration builder initialization handlers.
        /// </summary>
        /// <param name="context">The host builder context.</param>
        /// <param name="builder">The configuration builder being initialized.</param>
        /// <param name="assemblies">The assemblies to search for handler methods.</param>
        internal static void InvokeHandlers(HostBuilderContext context, IConfigurationBuilder builder, Assembly[] assemblies)
        {
            object[] p1 = { builder };
            object[] p2 = { builder, context };
            foreach (HandlerInfo handler in GetHandlers<ConfigurationBuilderHandlerAttribute>(typeof(IConfigurationBuilder), assemblies).OrderBy(h => h.Priority))
                _ = handler.Method.Invoke(null, handler.PassContext ? p2 : p1);
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"Methods marked with {nameof(ConfigurationBuilderHandlerAttribute)} invoked.");
#endif
        }
    }
}

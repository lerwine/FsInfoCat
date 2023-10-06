using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FsInfoCat
{
    /// <summary>
    /// Indicates that a method is a handler for initializing the command line switch dictionary for the <see cref="Hosting.Host"/>.
    /// </summary>
    /// <remarks>The method will only be invoked if the return type is <see langword="void"/> and it has 1 or 2 parameters.
    /// The first parameter must be of type <see cref="IDictionary{TKey, TValue}"/> with a <see cref="string"/> key and value type,
    /// and the second parameter, if present, must be of type <see cref="HostBuilderContext"/>.</remarks>
    public sealed class CommandLineSwitchMappingsInitializerAttribute : HostingInitializationHandlerAttribute
    {
        /// <summary>
        /// Invokes the switch mapping dictionary initialization handlers.
        /// </summary>
        /// <param name="context">The host builder context.</param>
        /// <param name="mappings">The switch mappings being initialized.</param>
        /// <param name="assemblies">The assemblies to search for handler methods.</param>
        internal static void InvokeHandlers(HostBuilderContext context, IDictionary<string, string> mappings, Assembly[] assemblies)
        {
            object[] p1 = { mappings };
            object[] p2 = { mappings, context };
            foreach (HandlerInfo handler in GetHandlers(typeof(IDictionary<string, string>), assemblies).OrderBy(h => h.Priority))
                _ = handler.Method.Invoke(null, handler.PassContext ? p2 : p1);
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"Methods marked with {nameof(CommandLineSwitchMappingsInitializerAttribute)} invoked.");
#endif
        }
    }
}

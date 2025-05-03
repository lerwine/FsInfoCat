using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace FsInfoCat
{
    /// <summary>
    /// Base class for attributes that indicate a method is a handler for configuring the <see cref="Hosting.Host"/>.
    /// </summary>
    /// <remarks>The method return type should be <see langword="void"/> with 1 or 2 parameters. The first parameter must be of type passed to the <see cref="GetHandlers(Type, Assembly[])"/> method,
    /// and the second parameter, if present, must be of type <see cref="HostBuilderContext"/>.</remarks>
    public abstract class HostingInitializationHandlerAttribute : Attribute
    {
        /// <summary>
        /// Gets the priority value that is used for determining the order of invocation.
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Gets the initialization handlers to be invoked.
        /// </summary>
        /// <param name="arg1Type">The required first argument of the handler method.</param>
        /// <param name="assemblies">The assemblies to search.</param>
        /// <typeparam name="T">The type of attribute that flags the handler</typeparam>
        /// <returns>The handlers to be invoked.</returns>
        protected static IEnumerable<HandlerInfo> GetHandlers<T>(Type arg1Type, Assembly[] assemblies)
            where T : HostingInitializationHandlerAttribute
        {
            Type returnType = typeof(void);
            Type builderContextType = typeof(HostBuilderContext);
            foreach (Assembly assembly in assemblies)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine($"Getting static methods marked with {typeof(T).FullName} for {assembly.FullName}");
#endif
                foreach (Type targetType in assembly.GetTypes())
                {
                    foreach (MethodInfo method in targetType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
                    {
                        T attribute = method.GetCustomAttribute<T>();
#if DEBUG
                        if (attribute is not null)
                        {
                            if (method.IsStatic)
                            {
                                if (method.ReturnType.Equals(returnType))
                                {
                                    ParameterInfo[] parameters = method.GetParameters();
                                    if (parameters.Length > 0)
                                    {
                                        if (parameters.Length > 2)
                                            System.Diagnostics.Debug.Write($"Warning: {method} of {targetType} was not invoked because it has more than 2 arguments.");
                                        else
                                        {
                                            ParameterInfo parameterInfo = parameters[0];
                                            if (parameterInfo.ParameterType.IsAssignableFrom(arg1Type))
                                            {
                                                if (parameterInfo.IsOut)
                                                    System.Diagnostics.Debug.Write($"Warning: {method} of {targetType} was not invoked because the first parameter is an output parameter.");
                                                else if (parameters.Length == 1)
                                                    yield return new(method, false, attribute.Priority);
                                                else if ((parameterInfo = parameters[1]).ParameterType.IsAssignableFrom(builderContextType))
                                                {
                                                    if (parameterInfo.IsOut)
                                                        System.Diagnostics.Debug.Write($"Warning: {method} of {targetType} was not invoked because the second parameter is an output parameter.");
                                                    else
                                                        yield return new(method, true, attribute.Priority);
                                                }
                                                else
                                                    System.Diagnostics.Debug.Write($"Warning: {method} of {targetType} was not invoked because the second parameter is not assignable from {arg1Type}.");
                                            }
                                            else
                                                System.Diagnostics.Debug.Write($"Warning: {method} of {targetType} was not invoked because the first parameter is not assignable from {arg1Type}.");
                                        }
                                    }
                                    else
                                        System.Diagnostics.Debug.Write($"Warning: {method} of {targetType} was not invoked because it has no parameters.");
                                }
                                else
                                    System.Diagnostics.Debug.Write($"Warning: {method} of {targetType} was not invoked because it does not return void.");
                            }
                            else
                                System.Diagnostics.Debug.Write($"Warning: {method} of {targetType} was not invoked because it is not static.");
                        }
#else
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
                            {
                                System.Diagnostics.Debug.Write($"Warning: {method} of {type} was not invoked because it had more than 2 arguments.");
                                continue;
                            }
                            p1 = parameters[0];
                            if (p1.ParameterType.IsAssignableFrom(a) && !p1.IsOut)
                                yield return (method, passContext, attribute.Priority);
                        }
#endif
                    }
                }
            }
        }

//         /// <summary>
//         /// Gets the initialization handlers to be invoked.
//         /// </summary>
//         /// <param name="arg1Type">The required first argument of the handler method.</param>
//         /// <param name="assemblies">The assemblies to search.</param>
//         /// <returns>The handlers to be invoked.</returns>
//         protected static IEnumerable<HandlerInfo> GetHandlers(Type arg1Type, Assembly[] assemblies)
//         {
//             Type returnType = typeof(void);
//             Type builderContextType = typeof(HostBuilderContext);
//             foreach (Assembly assembly in assemblies)
//             {
// #if DEBUG
//                 System.Diagnostics.Debug.WriteLine($"Getting static methods marked with {nameof(HostingInitializationHandlerAttribute)} for {assembly.FullName}");
// #endif
//                 foreach (Type targetType in assembly.GetTypes())
//                 {
//                     foreach (MethodInfo method in targetType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
//                     {
//                         HostingInitializationHandlerAttribute attribute = method.GetCustomAttribute<HostingInitializationHandlerAttribute>();
// #if DEBUG
//                         if (attribute is not null)
//                         {
//                             if (method.IsStatic)
//                             {
//                                 if (method.ReturnType.Equals(returnType))
//                                 {
//                                     ParameterInfo[] parameters = method.GetParameters();
//                                     if (parameters.Length > 0)
//                                     {
//                                         if (parameters.Length > 2)
//                                             System.Diagnostics.Debug.Write($"Warning: {method} of {targetType} was not invoked because it has more than 2 arguments.");
//                                         else
//                                         {
//                                             ParameterInfo parameterInfo = parameters[0];
//                                             if (parameterInfo.ParameterType.IsAssignableFrom(arg1Type))
//                                             {
//                                                 if (parameterInfo.IsOut)
//                                                     System.Diagnostics.Debug.Write($"Warning: {method} of {targetType} was not invoked because the first parameter is an output parameter.");
//                                                 else if (parameters.Length == 1)
//                                                     yield return new(method, false, attribute.Priority);
//                                                 else if ((parameterInfo = parameters[1]).ParameterType.IsAssignableFrom(builderContextType))
//                                                 {
//                                                     if (parameterInfo.IsOut)
//                                                         System.Diagnostics.Debug.Write($"Warning: {method} of {targetType} was not invoked because the second parameter is an output parameter.");
//                                                     else
//                                                         yield return new(method, true, attribute.Priority);
//                                                 }
//                                                 else
//                                                     System.Diagnostics.Debug.Write($"Warning: {method} of {targetType} was not invoked because the second parameter is not assignable from {arg1Type}.");
//                                             }
//                                             else
//                                                 System.Diagnostics.Debug.Write($"Warning: {method} of {targetType} was not invoked because the first parameter is not assignable from {arg1Type}.");
//                                         }
//                                     }
//                                     else
//                                         System.Diagnostics.Debug.Write($"Warning: {method} of {targetType} was not invoked because it has no parameters.");
//                                 }
//                                 else
//                                     System.Diagnostics.Debug.Write($"Warning: {method} of {targetType} was not invoked because it does not return void.");
//                             }
//                             else
//                                 System.Diagnostics.Debug.Write($"Warning: {method} of {targetType} was not invoked because it is not static.");
//                         }
// #else
//                         if (attribute is not null && method.ReturnType.Equals(r))
//                         {
//                             ParameterInfo[] parameters = method.GetParameters();
//                             ParameterInfo p1;
//                             bool passContext = parameters.Length == 2;
//                             if (passContext)
//                             {
//                                 ParameterInfo p2 = parameters[1];
//                                 if (!p2.ParameterType.IsAssignableFrom(b) || p2.IsOut)
//                                     continue;
//                             }
//                             else if (parameters.Length > 1)
//                             {
//                                 System.Diagnostics.Debug.Write($"Warning: {method} of {type} was not invoked because it had more than 2 arguments.");
//                                 continue;
//                             }
//                             p1 = parameters[0];
//                             if (p1.ParameterType.IsAssignableFrom(a) && !p1.IsOut)
//                                 yield return (method, passContext, attribute.Priority);
//                         }
// #endif
//                     }
//                 }
//             }
//         }

        /// <summary>
        /// Represents a handler to be invoked.
        /// </summary>
        /// <param name="Method">The method to be invoked.</param>
        /// <param name="PassContext"><see langword="true"/> if the method contains 2 parameters; otherwise, <see langword="false"/> if it contains only one parameter.</param>
        /// <param name="Priority">The priority value that indicates the order of invocation.</param>
        public record HandlerInfo(MethodInfo Method, bool PassContext, int Priority);
    }
}

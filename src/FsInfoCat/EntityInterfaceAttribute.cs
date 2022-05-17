using System;
using System.Linq;

namespace FsInfoCat
{
    /// <summary>
    /// Explicitly marks an interface as an interface for entity objects.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, Inherited = true, AllowMultiple = false)]
    public sealed class EntityInterfaceAttribute : Attribute
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public EntityInterfaceAttribute() { }

        public static bool IsEntityInterface(Type type)
        {
            if (type is null || !type.IsInterface) return false;
            Type a = typeof(EntityInterfaceAttribute);
            if (type.GetCustomAttributes(true).OfType<EntityInterfaceAttribute>().Any()) return true;
            return type.GetInterfaces().Any(i => i.GetCustomAttributes(true).OfType<EntityInterfaceAttribute>().Any());
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}

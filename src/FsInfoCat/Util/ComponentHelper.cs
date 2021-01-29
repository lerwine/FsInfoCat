using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace FsInfoCat.Util
{
    public static class ComponentHelper
    {
        public static string GetComponentName(this IComponent component)
        {
            if (component is null)
                return null;
            ISite site = component.Site;
            if (null != site)
                return site.Name;
            return (component is INamedComponent nc) ? nc.Name : null;
        }

        public static IComponent GetOwner(this ISite site)
        {
            if (null == site)
                return null;
            INestedContainer container = site.Container as INestedContainer;
            return (container is null) ? null : container.Owner;
        }

        public static IComponent GetOwner(this IComponent component)
        {
            INestedContainer container = component.GetContainer() as INestedContainer;
            return (container is null) ? null : container.Owner;
        }

        public static T FindOwner<T>(this IComponent component)
            where T : IComponent
        {
            for (IComponent c = component; null != c; c = c.GetOwner())
            {
                if (c is T t)
                    return t;
            }
            return default(T);
        }

        public static IContainer GetContainer(this IComponent component)
        {
            if (component is null)
                return null;
            ISite site = component.Site;
            return (site is null) ? null : site.Container;
        }

        private class Site : ISite
        {
            public IComponent Component { get; }

            public IContainer Container { get; }

            public string Name { get; }

            string ISite.Name { get => Name; set => throw new NotSupportedException(); }

            public bool DesignMode => false;

            internal Site(IComponent component, IContainer container, string name)
            {
                Component = component;
                Container = container;
                Name = name;
            }

            public object GetService(Type serviceType) => (serviceType == typeof(ISite)) ? (object)this : ((serviceType == typeof(IContainer)) ? Container : null);
        }

        public static Stack<ISite> GetSegmentsToRoot(this IComponent component, bool includeCurrent = false, bool throwOnCircularReference = false)
        {
            if (component is null)
                throw new System.ArgumentNullException(nameof(component));
            Stack<ISite> segments = new Stack<ISite>();
            ISite site = component.Site;

            if (null == site)
            {
                if (includeCurrent)
                    segments.Push(new Site(component, null, null));
                return segments;
            }
            if (includeCurrent)
                segments.Push((ReferenceEquals(site.Component, component)) ? site : new Site(component, site.Container, site.Name));

            while (null != (component = site.GetOwner()))
            {
                if (segments.Any(s => ReferenceEquals(s.Component, component)))
                {
                    if (throwOnCircularReference)
                        throw new InvalidOperationException("Circular nested component reference");
                    break;
                }
                if (null == (site = component.Site))
                {
                    segments.Push(new Site(component, null, null));
                    break;
                }
                segments.Push((ReferenceEquals(site.Component, component)) ? site : new Site(component, site.Container, site.Name));
            }
            return segments;
        }
    }
}

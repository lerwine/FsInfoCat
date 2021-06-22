using FsInfoCat.Models.Crawl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace FsInfoCat.Util
{
    public static class ComponentHelper
    {
        public static readonly StringComparer CASE_SENSITIVE_COMPARER = StringComparer.InvariantCulture;
        public static readonly StringComparer IGNORE_CASE_COMPARER = StringComparer.InvariantCultureIgnoreCase;

        public static bool TryGetFileUri(this IFsChildNode childNode, out FileUri result)
        {
            IComponent owner = childNode?.GetOwner();
            if (owner is null)
            {
                result = null;
                return false;
            }
            if (owner is FsRoot root)
                result = new FileUri(root.RootUri, childNode.Name);
            else if (owner is IFsChildNode parent && parent.TryGetFileUri(out FileUri fileUri))
                result = new FileUri(fileUri, childNode.Name);
            else
            {
                result = null;
                return false;
            }
            return true;
        }

        public static bool TryFindByName<TResult>(this IEnumerable<TResult> source, string name, IEqualityComparer<string> comparer, out TResult result)
            where TResult : class, INamedComponent
        {
            if (source is null)
            {
                result = null;
                return false;
            }
            foreach (TResult item in source)
            {
                if (!(item is null) && comparer.Equals(item.Name, name))
                {
                    result = item;
                    return true;
                }
            }
            result = default;
            return false;
        }

        public static string GetComponentName(this IComponent component)
        {
            if (component is null)
                return null;
            ISite site = component.Site;
            if (!(site is null))
                return site.Name;
            return (component is INamedComponent nc) ? nc.Name : null;
        }

        public static IComponent GetOwner(this ISite site) => (site?.Container is INestedContainer container) ? container.Owner : null;

        public static IComponent GetOwner(this IComponent component) => ((component?.Site)?.Container is INestedContainer container) ? container.Owner : null;

        public static T FindOwner<T>(this IComponent component)
            where T : IComponent
        {
            for (IComponent c = component; null != c; c = c.GetOwner())
            {
                if (c is T t)
                    return t;
            }
            return default;
        }

        public static IContainer GetContainer(this IComponent component) => (component?.Site)?.Container;

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

            if (site is null)
            {
                if (includeCurrent)
                    segments.Push(new Site(component, null, null));
                return segments;
            }
            if (includeCurrent)
                segments.Push((ReferenceEquals(site.Component, component)) ? site : new Site(component, site.Container, site.Name));

            while (!((component = site.GetOwner()) is null))
            {
                if (segments.Any(s => ReferenceEquals(s.Component, component)))
                {
                    if (throwOnCircularReference)
                        throw new InvalidOperationException("Circular nested component reference");
                    break;
                }
                if ((site = component.Site) is null)
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

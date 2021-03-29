using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace FsInfoCat.Util
{
    public abstract partial class ComponentList
    {
        public partial class AttachableContainer : ContainerBase
        {
            protected override object SyncRoot { get; } = new object();

            public override StringComparer NameComparer { get; }

            /// <summary>
            /// Creates a new <c>AttachableContainer</c>.
            /// </summary>
            /// <param name="owner">The owning <seealso cref="IComponent" />.</param>
            /// <exception cref="ArgumentNullException"><paramref name="owner"/> was <c>null</c>.</exception>
            public AttachableContainer(IComponent owner) : this(owner, false) { }

            public AttachableContainer(IComponent owner, bool caseSensitive) : this(owner, (caseSensitive) ? ComponentHelper.CASE_SENSITIVE_COMPARER : ComponentHelper.IGNORE_CASE_COMPARER) { }

            public AttachableContainer(IComponent owner, StringComparer nameComparer) : base(owner)
            {
                NameComparer = nameComparer ?? throw new ArgumentNullException(nameof(nameComparer));
            }

            /// <summary>
            /// Adds the specified <paramref name="component" /> to the current <see cref="AttachableContainer" />.
            /// </summary>
            /// <param name="component">The <seealso cref="IComponent" /> to add.</param>
            /// <remarks>If <paramref name="component" /> is <seealso cref="INamedComponent" />, then the value of the
            /// <seealso cref="INamedComponent.Name" /> property will be used to name the component within the
            /// current <see cref="AttachableContainer" />; otherwise, it will be unnamed.</remarks>
            /// <exception cref="ArgumentNullException"><paramref name="component"/> was <c>null</c>.</exception>
            /// <exception cref="ArgumentOutOfRangeException">Name assigned to <paramref name="component"/> was already used by another item.</exception>
            public override void Add(IComponent component) => Site.Add(component, component.GetComponentName(), this);

            /// <summary>
            /// Adds the specified <paramref name="component" /> to the current <see cref="AttachableContainer" /> and assigns a name to it.
            /// </summary>
            /// <param name="component">The <seealso cref="IComponent" /> to add.</param>
            /// <param name="name">The unique name to assign to the component.
            /// <para>-or-</para>
            /// <para><c>null</c> to leave it unnamed.</para></param>
            /// <exception cref="ArgumentNullException"><paramref name="component"/> was <c><c>null</c></c>.</exception>
            /// <exception cref="ArgumentOutOfRangeException"><paramref name="name"/> was already used by another item.</exception>
            public override void Add(IComponent component, string name) => Site.Add(component, name, this);

            /// <summary>
            /// Adds the specified <paramref name="item" /> to the attached <paramref name="componentList" /> as well as to the
            /// current <see cref="AttachableContainer" />.
            /// </summary>
            /// <param name="item">The <seealso cref="IComponent" /> to add.</param>
            /// <param name="componentList">The attached <seealso cref="IComponent" /> to add the component to.</param>
            /// <returns>The zero-based index at which the <paramref name="item" /> to the attached <paramref name="componentList" />.</returns>
            /// <remarks>If <paramref name="component" /> is <seealso cref="INamedComponent" />, then the value of the
            /// <seealso cref="INamedComponent.Name" /> property will be used to name the component within the
            /// current <see cref="AttachableContainer" />; otherwise, it will be unnamed.</remarks>
            /// <exception cref="ArgumentNullException"><paramref name="item"/> or <paramref name="componentList"/> was <c>null</c>.</exception>
            /// <exception cref="InvalidOperationException"><paramref name="componentList"/> was not attached to the
            /// current <see cref="AttachableContainer" />.</exception>
            /// <exception cref="ArgumentOutOfRangeException">Name assigned to <paramref name="item"/> was already used by another item.</exception>
            protected override int Add(IComponent item, ComponentList componentList) => Site.Add(item, item.GetComponentName(), this, componentList);

            /// <summary>
            /// Adds the specified <paramref name="item" /> to the attached <paramref name="componentList" /> as well as to the
            /// current <see cref="AttachableContainer" />, assigning a name to it.
            /// </summary>
            /// <param name="item">The <seealso cref="IComponent" /> to add.</param>
            /// <param name="name">The unique name to assign to the component.
            /// <para>-or-</para>
            /// <para><c>null</c> to leave it unnamed.</para></param>
            /// <param name="componentList">The attached <seealso cref="IComponent" /> to add the component to.</param>
            /// <returns>The zero-based index at which the <paramref name="item" /> to the attached <paramref name="componentList" />.</returns>
            /// <exception cref="ArgumentNullException"><paramref name="item"/> or <paramref name="componentList"/> was <c>null</c>.</exception>
            /// <exception cref="InvalidOperationException"><paramref name="componentList"/> was not attached to the
            /// current <see cref="AttachableContainer" />.</exception>
            /// <exception cref="ArgumentOutOfRangeException"><paramref name="name"/> was already used by another item.</exception>
            protected override int Add(IComponent item, string name, ComponentList componentList) => Site.Add(item, name, this, componentList);

            public void Attach(ComponentList list)
            {
                if (list is null)
                    throw new ArgumentNullException(nameof(list));
                Monitor.Enter(list._syncRoot);
                try
                {
                    if (list._container is AttachableContainer oldContainer)
                    {
                        if (ReferenceEquals(oldContainer, this))
                            return;
                        throw new ArgumentOutOfRangeException(nameof(list));
                    }
                    Monitor.Enter(SyncRoot);
                    try { Site.Attach(list, this); }
                    finally { Monitor.Exit(SyncRoot); }
                }
                finally { Monitor.Exit(list._syncRoot); }
            }

            public void Detach(ComponentList list)
            {
                if (list is null)
                    throw new ArgumentNullException(nameof(list));
                Monitor.Enter(list._syncRoot);
                try
                {
                    if (list._container is AttachableContainer oldContainer && ReferenceEquals(oldContainer, this))
                        Monitor.Enter(SyncRoot);
                    try { Site.Detach(list); }
                    finally { Monitor.Exit(SyncRoot); }
                }
                finally { Monitor.Exit(list._syncRoot); }
            }

            /// <summary>
            /// Clears all <seealso cref="IComponent" />s from the attached <paramref name="componentList" />.
            /// </summary>
            /// <param name="componentList">The attached <seealso cref="ComponentList" /> to remove all items from.</param>
            /// <remarks><seealso cref="IComponent" />s which were removed from
            /// the <paramref name="componentList" /> will also be removed from the current <see cref="AttachableContainer" />.</remarks>
            /// <exception cref="ArgumentNullException"><paramref name="componentList"/> was <c>null</c>.</exception>
            /// <exception cref="InvalidOperationException"><paramref name="componentList"/> was not attached to the
            /// current <see cref="AttachableContainer" />.</exception>
            protected override void Clear(ComponentList componentList) => Site.Clear(this, componentList);

            /// <summary>
            /// Gets all <seealso cref="IComponent" /> in the current <see cref="AttachableContainer" />.
            /// </summary>
            /// <returns>All <seealso cref="IComponent" /> in the current <see cref="AttachableContainer" />.</returns>
            protected override IEnumerable<IComponent> GetComponents() => Site.GetSites(this).Select(s => s.Component);

            private IEnumerable<string> GetNames() => Site.GetSites(this).Select(s => s.Name).Where(n => null != n);

            private IEnumerable<string> GetNames(IComponent ignore) => Site.GetSites(this).Where(s => !ReferenceEquals(s.Component, ignore)).Select(s => s.Name).Where(n => null != n);

            private IEnumerable<string> GetNames(IComponent ignore1, IComponent ignore2) => Site.GetSites(this).Where(s => !(ReferenceEquals(s.Component, ignore1) || ReferenceEquals(s.Component, ignore2))).Select(s => s.Name).Where(n => null != n);

            /// <summary>
            /// Inserts the specified <paramref name="item" /> into the attached <paramref name="componentList" /> at the
            /// specified <paramref name="index" /> as well as adding it to the current <see cref="AttachableContainer" />, assigning a name to it.
            /// </summary>
            /// <param name="index">The zero-based index at which to insert the <paramref name="item" /> into the attached <paramref name="componentList" /></param>
            /// <param name="item">The <seealso cref="IComponent" /> to insert.</param>
            /// <param name="name">The unique name to assign to the component.
            /// <para>-or-</para>
            /// <para><c>null</c> to leave it unnamed.</para></param>
            /// <param name="componentList">The attached <seealso cref="ComponentList" /> to insert the <paramref name="item" /> into.</param>
            /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> was less than zero or greater than the number of items in
            /// <paramref name="componentList" />
            /// <para>-or-</para>
            /// <para><paramref name="name"/> was already used by another item.</para></exception>
            /// <exception cref="ArgumentNullException"><paramref name="item"/> or <paramref name="componentList"/> was <c>null</c>.</exception>
            /// <exception cref="InvalidOperationException"><paramref name="componentList"/> was not attached to the
            /// current <see cref="AttachableContainer" />.</exception>
            protected override void Insert(int index, IComponent item, string name, ComponentList componentList) => Site.Insert(index, item, name, this, componentList);

            /// <summary>
            /// Inserts the specified <paramref name="item" /> into the attached <paramref name="componentList" /> at the
            /// specified <paramref name="index" /> as well as adding it to the current <see cref="AttachableContainer" />, assigning a name to it.
            /// </summary>
            /// <param name="index">The zero-based index at which to insert the <paramref name="item" /> into the attached <paramref name="componentList" /></param>
            /// <param name="item">The <seealso cref="IComponent" /> to insert.</param>
            /// <param name="componentList">The attached <seealso cref="ComponentList" /> to insert the <paramref name="item" /> into.</param>
            /// <remarks>If <paramref name="component" /> is <seealso cref="INamedComponent" />, then the value of the
            /// <seealso cref="INamedComponent.Name" /> property will be used to name the component within the
            /// current <see cref="AttachableContainer" />; otherwise, it will be unnamed.</remarks>
            /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> was less than zero or greater than the number of items in
            /// <paramref name="componentList" />
            /// <para>-or-</para>
            /// <para>name to be assigned to the <paramref name="item" /> was already used by another item.</para></exception>
            /// <exception cref="ArgumentNullException"><paramref name="item"/> or <paramref name="componentList"/> was <c>null</c>.</exception>
            /// <exception cref="InvalidOperationException"><paramref name="componentList"/> was not attached to the
            /// current <see cref="AttachableContainer" />.</exception>
            protected override void Insert(int index, IComponent item, ComponentList componentList) => Site.Insert(index, item, item.GetComponentName(), this, componentList);

            /// <summary>
            /// Removes the specified <paramref name="item" /> from current <see cref="AttachableContainer" /> as well as from any
            /// attached <paramref name="ComponentList" />s.
            /// </summary>
            /// <param name="component">The <seealso cref="IComponent" /> to remove.</param>
            public override void Remove(IComponent component) => Site.Remove(component, this);

            /// <summary>
            /// Removes the specified <paramref name="item" /> from the attached <paramref name="componentList" /> and from the
            /// current <see cref="AttachableContainer" />.
            /// </summary>
            /// <param name="item">The <seealso cref="IComponent" /> to remove.</param>
            /// <param name="componentList">The attached <seealso cref="ComponentList" /> to remove the <paramref name="item" /> from.</param>
            /// <returns><c>true</c> if <paramref name="item" /> was removed from the attached <paramref name="componentList" />;
            /// otherwise, <c>false</c></returns>
            /// <exception cref="ArgumentNullException"><paramref name="componentList"/> was <c>null</c>.</exception>
            /// <exception cref="InvalidOperationException"><paramref name="componentList"/> was not attached to the
            /// current <see cref="AttachableContainer" />.</exception>
            protected override bool Remove(IComponent item, ComponentList componentList) => Site.Remove(item, this, componentList);

            /// <summary>
            /// Removes the <seealso cref="IComponent" /> at the the specified <paramref name="index" /> of the
            /// <paramref name="componentList" /> and removes it from the current <see cref="AttachableContainer" /> as well.
            /// </summary>
            /// <param name="index">The zero-based index of the <seealso cref="IComponent" /> to remove.</param>
            /// <param name="componentList">The attached <seealso cref="ComponentList" /> to remove the
            /// <seealso cref="IComponent" /> from.</param>
            /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> was less than zero or greater than the number of items in
            /// <paramref name="componentList" /></exception>
            /// <exception cref="ArgumentNullException"><paramref name="componentList"/> was <c>null</c>.</exception>
            /// <exception cref="InvalidOperationException"><paramref name="componentList"/> was not attached to the
            /// current <see cref="AttachableContainer" />.</exception>
            protected override void RemoveAt(int index, ComponentList componentList) => Site.RemoveAt(index, this, componentList);

            /// <summary>
            /// Replaces the <seealso cref="IComponent" /> at the the specified <paramref name="index" /> in the
            /// <paramref name="componentList" /> with another <paramref name="item" />, replacing it in the current <see cref="AttachableContainer" /> as well.
            /// </summary>
            /// <param name="index">The zero-based index of the <seealso cref="IComponent" /> to replace.</param>
            /// <param name="item">The new <seealso cref="IComponent" /> to place at the specified index.</param>
            /// <param name="componentList">The attached <seealso cref="ComponentList" /> to replace the
            /// <seealso cref="IComponent" /> within.</param>
            /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> was less than zero or greater than the number of items in
            /// <paramref name="componentList" />
            /// <para>-or-</para>
            /// <para>name to be assigned to the <paramref name="item" /> was already used by another item.</para></exception>
            /// <exception cref="ArgumentNullException"><paramref name="item"/> or <paramref name="componentList"/> was <c>null</c>.</exception>
            /// <exception cref="InvalidOperationException"><paramref name="componentList"/> was not attached to the
            /// current <see cref="AttachableContainer" />.</exception>
            protected override void SetAt(int index, IComponent item, ComponentList componentList) => Site.SetAt(index, item, item.GetComponentName(), this, componentList);

            /// <summary>
            /// Replaces the <seealso cref="IComponent" /> at the the specified <paramref name="index" /> in the
            /// <paramref name="componentList" /> with another <paramref name="item" />, replacing it in the current <see cref="AttachableContainer" /> as well.
            /// </summary>
            /// <param name="index">The zero-based index of the <seealso cref="IComponent" /> to replace.</param>
            /// <param name="item">The new <seealso cref="IComponent" /> to place at the specified index.</param>
            /// <param name="name">The unique name to assign to the component.
            /// <para>-or-</para>
            /// <para><c>null</c> to leave it unnamed.</para></param>
            /// <param name="componentList">The attached <seealso cref="ComponentList" /> to replace the
            /// <seealso cref="IComponent" /> within.</param>
            /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> was less than zero or greater than the number of items in
            /// <paramref name="componentList" />
            /// <para>-or-</para>
            /// <para><paramref name="name" /> was already used by another item.</para></exception>
            /// <exception cref="ArgumentNullException"><paramref name="item"/> or <paramref name="componentList"/> was <c>null</c>.</exception>
            /// <exception cref="InvalidOperationException"><paramref name="componentList"/> was not attached to the
            /// current <see cref="AttachableContainer" />.</exception>
            protected override void SetAt(int index, IComponent item, string name, ComponentList componentList) => Site.SetAt(index, item, name, this, componentList);

            /// <summary>
            /// This gets called to dispose all <seealso cref="IComponent" />s in the current <see cref="AttachableContainer" />, clearing and
            /// detaching all attached <seealso cref="ComponentList" />s.
            /// </summary>
            protected override void OnDisposing()
            {
                ComponentList[] toDetach = GetAttachedLists().ToArray();
                foreach (ComponentList list in toDetach)
                    Detach(list);
            }

            private void ValidateName(IEnumerable<string> enumerable, string name)
            {
                if (enumerable.Contains(name, NameComparer))
                    throw new ArgumentOutOfRangeException(nameof(name), ERROR_MESSAGE_ITEM_WITH_NAME_EXISTS);
            }
        }
    }
}

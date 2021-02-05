using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace FsInfoCat.Util
{
    public abstract partial class ComponentList
    {
        public abstract partial class ContainerBase : INestedContainer, IServiceProvider
        {
            protected const string ERROR_MESSAGE_ITEM_WITH_NAME_EXISTS = "Another item with same name already exists.";
            private ComponentCollection _components = null;
            private bool _isDisposed;

            protected abstract object SyncRoot { get; }

            public IComponent Owner { get; private set; }

            ComponentCollection IContainer.Components
            {
                get
                {
                    ComponentCollection components = _components;
                    if (components is null)
                        _components = components = new ComponentCollection(GetComponents().ToArray());
                    return components;
                }
            }

            public abstract IEqualityComparer<string> NameComparer { get; }

            /// <summary>
            /// Creates a new <c>ContainerBase</c>.
            /// </summary>
            /// <param name="owner">The owning <seealso cref="IComponent" />.</param>
            /// <exception cref="ArgumentNullException"><paramref name="owner"/> was <c>null</c>.</exception>
            protected ContainerBase(IComponent owner)
            {
                Owner = owner ?? throw new ArgumentNullException(nameof(owner));
                Owner.Disposed += OnOwnerDisposed;
            }

            /// <summary>
            /// Adds the specified <paramref name="component" /> to the current <see cref="ContainerBase" />.
            /// </summary>
            /// <param name="component">The <seealso cref="IComponent" /> to add.</param>
            /// <remarks>If <paramref name="component" /> is <seealso cref="INamedComponent" />, then the value of the
            /// <seealso cref="INamedComponent.Name" /> property will be used to name the component within the
            /// current <see cref="ContainerBase" />; otherwise, it will be unnamed.</remarks>
            /// <exception cref="ArgumentNullException"><paramref name="component"/> was <c>null</c>.</exception>
            /// <exception cref="ArgumentOutOfRangeException">Name assigned to <paramref name="component"/> was already used by another item.</exception>
            public abstract void Add(IComponent component);

            /// <summary>
            /// Adds the specified <paramref name="component" /> to the current <see cref="ContainerBase" /> and assigns a name to it.
            /// </summary>
            /// <param name="component">The <seealso cref="IComponent" /> to add.</param>
            /// <param name="name">The unique name to assign to the component.
            /// <para>-or-</para>
            /// <para><c>null</c> to leave it unnamed.</para></param>
            /// <exception cref="ArgumentNullException"><paramref name="component"/> was <c>null</c>.</exception>
            /// <exception cref="ArgumentOutOfRangeException"><paramref name="name"/> was already used by another item.</exception>
            public abstract void Add(IComponent component, string name);

            /// <summary>
            /// Adds the specified <paramref name="item" /> to the attached <paramref name="componentList" /> as well as to the
            /// current <see cref="ContainerBase" />.
            /// </summary>
            /// <param name="item">The <seealso cref="IComponent" /> to add.</param>
            /// <param name="componentList">The attached <seealso cref="IComponent" /> to add the component to.</param>
            /// <returns>The zero-based index at which the <paramref name="item" /> to the attached <paramref name="componentList" />.</returns>
            /// <remarks>If <paramref name="component" /> is <seealso cref="INamedComponent" />, then the value of the
            /// <seealso cref="INamedComponent.Name" /> property will be used to name the component within the
            /// current <see cref="ContainerBase" />; otherwise, it will be unnamed.</remarks>
            /// <exception cref="ArgumentNullException"><paramref name="item"/> or <paramref name="componentList"/> was <c>null</c>.</exception>
            /// <exception cref="InvalidOperationException"><paramref name="componentList"/> was not attached to the
            /// current <see cref="ContainerBase" />.</exception>
            /// <exception cref="ArgumentOutOfRangeException">Name assigned to <paramref name="item"/> was already used by another item.</exception>
#warning Throws error when trying to re-add already-existing item
            protected abstract int Add(IComponent item, ComponentList componentList);

            /// <summary>
            /// Adds the specified <paramref name="item" /> to the attached <paramref name="componentList" /> as well as to the
            /// current <see cref="ContainerBase" />, assigning a name to it.
            /// </summary>
            /// <param name="item">The <seealso cref="IComponent" /> to add.</param>
            /// <param name="name">The unique name to assign to the component.
            /// <para>-or-</para>
            /// <para><c>null</c> to leave it unnamed.</para></param>
            /// <param name="componentList">The attached <seealso cref="IComponent" /> to add the component to.</param>
            /// <returns>The zero-based index at which the <paramref name="item" /> to the attached <paramref name="componentList" />.</returns>
            /// <exception cref="ArgumentNullException"><paramref name="item"/> or <paramref name="componentList"/> was <c>null</c>.</exception>
            /// <exception cref="InvalidOperationException"><paramref name="componentList"/> was not attached to the
            /// current <see cref="ContainerBase" />.</exception>
            /// <exception cref="ArgumentOutOfRangeException"><paramref name="name"/> was already used by another item.</exception>
            protected abstract int Add(IComponent item, string name, ComponentList componentList);

            /// <summary>
            /// Adds the specified <paramref name="item" /> to the attached <paramref name="componentList" /> as well as to the
            /// current <see cref="ContainerBase" />.
            /// </summary>
            /// <param name="item">The <seealso cref="IComponent" /> to add.</param>
            /// <param name="componentList">The attached <seealso cref="ComponentList" /> to add the <paramref name="item" /> to.</param>
            /// <returns>The zero-based index at which the <paramref name="item" /> to the attached <paramref name="componentList" />.</returns>
            /// <remarks>If <paramref name="component" /> is <seealso cref="INamedComponent" />, then the value of the
            /// <seealso cref="INamedComponent.Name" /> property will be used to name the component within the
            /// current <see cref="ContainerBase" />; otherwise, it will be unnamed.</remarks>
            /// <exception cref="ArgumentNullException"><paramref name="item"/> or <paramref name="componentList"/> was <c>null</c>.</exception>
            /// <exception cref="InvalidOperationException"><paramref name="componentList"/> was not attached to the
            /// current <see cref="ContainerBase" />.</exception>
            /// <exception cref="ArgumentOutOfRangeException">Name assigned to <paramref name="item"/> was already used by another item.</exception>
            internal int AddListItem(IComponent item, ComponentList componentList) => Add(item, componentList);

            /// <summary>
            /// Adds the specified <paramref name="item" /> to the attached <paramref name="componentList" /> as well as to the
            /// current <see cref="ContainerBase" />, assigning a name to it.
            /// </summary>
            /// <param name="item">The <seealso cref="IComponent" /> to add.</param>
            /// <param name="name">The unique name to assign to the component.
            /// <para>-or-</para>
            /// <para><c>null</c> to leave it unnamed.</para></param>
            /// <param name="componentList">The attached <seealso cref="ComponentList" /> to add the <paramref name="item" /> to.</param>
            /// <returns>The zero-based index at which the <paramref name="item" /> to the attached <paramref name="componentList" />.</returns>
            /// <exception cref="ArgumentNullException"><paramref name="item"/> or <paramref name="componentList"/> was <c>null</c>.</exception>
            /// <exception cref="InvalidOperationException"><paramref name="componentList"/> was not attached to the
            /// current <see cref="ContainerBase" />.</exception>
            /// <exception cref="ArgumentOutOfRangeException"><paramref name="name"/> was already used by another item.</exception>
            internal int AddListItem(IComponent item, string name, ComponentList componentList) => Add(item, name, componentList);

            /// <summary>
            /// Clears all <seealso cref="IComponent" />s from the attached <paramref name="componentList" />.
            /// </summary>
            /// <param name="componentList">The attached <seealso cref="ComponentList" /> to remove all items from.</param>
            /// <remarks><seealso cref="IComponent" />s which were removed from
            /// the <paramref name="componentList" /> will also be removed from the current <see cref="ContainerBase" />.</remarks>
            /// <exception cref="ArgumentNullException"><paramref name="componentList"/> was <c>null</c>.</exception>
            /// <exception cref="InvalidOperationException"><paramref name="componentList"/> was not attached to the
            /// current <see cref="ContainerBase" />.</exception>
            protected abstract void Clear(ComponentList componentList);

            /// <summary>
            /// Clears all <seealso cref="IComponent" />s from the attached <paramref name="componentList" />.
            /// </summary>
            /// <param name="componentList">The attached <seealso cref="ComponentList" /> to remove all items from.</param>
            /// <remarks><seealso cref="IComponent" />s which were removed from
            /// the <paramref name="componentList" /> will also be removed from the current <see cref="ContainerBase" />.</remarks>
            /// <exception cref="ArgumentNullException"><paramref name="componentList"/> was <c>null</c>.</exception>
            /// <exception cref="InvalidOperationException"><paramref name="componentList"/> was not attached to the
            /// current <see cref="ContainerBase" />.</exception>
            internal void ClearList(ComponentList componentList) => Clear(componentList);

            /// <summary>
            /// Gets all <seealso cref="IComponent" /> in the current <see cref="ContainerBase" />.
            /// </summary>
            /// <returns>All <seealso cref="IComponent" /> in the current <see cref="ContainerBase" />.</returns>
            protected abstract IEnumerable<IComponent> GetComponents();

            /// <summary>
            /// Inserts the specified <paramref name="item" /> into the attached <paramref name="componentList" /> at the
            /// specified <paramref name="index" /> as well as adding it to the current <see cref="ContainerBase" />, assigning a name to it.
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
            /// current <see cref="ContainerBase" />.</exception>
            protected abstract void Insert(int index, IComponent item, string name, ComponentList componentList);

            /// <summary>
            /// Inserts the specified <paramref name="item" /> into the attached <paramref name="componentList" /> at the
            /// specified <paramref name="index" /> as well as adding it to the current <see cref="ContainerBase" />, assigning a name to it.
            /// </summary>
            /// <param name="index">The zero-based index at which to insert the <paramref name="item" /> into the attached <paramref name="componentList" /></param>
            /// <param name="item">The <seealso cref="IComponent" /> to insert.</param>
            /// <param name="componentList">The attached <seealso cref="ComponentList" /> to insert the <paramref name="item" /> into.</param>
            /// <remarks>If <paramref name="component" /> is <seealso cref="INamedComponent" />, then the value of the
            /// <seealso cref="INamedComponent.Name" /> property will be used to name the component within the
            /// current <see cref="ContainerBase" />; otherwise, it will be unnamed.</remarks>
            /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> was less than zero or greater than the number of items in
            /// <paramref name="componentList" />
            /// <para>-or-</para>
            /// <para>name to be assigned to the <paramref name="item" /> was already used by another item.</para></exception>
            /// <exception cref="ArgumentNullException"><paramref name="item"/> or <paramref name="componentList"/> was <c>null</c>.</exception>
            /// <exception cref="InvalidOperationException"><paramref name="componentList"/> was not attached to the
            /// current <see cref="ContainerBase" />.</exception>
            protected abstract void Insert(int index, IComponent item, ComponentList componentList);

            /// <summary>
            /// Inserts the specified <paramref name="item" /> into the attached <paramref name="componentList" /> at the
            /// specified <paramref name="index" /> as well as adding it to the current <see cref="ContainerBase" />, assigning a name to it.
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
            /// current <see cref="ContainerBase" />.</exception>
            internal void InsertListItem(int index, IComponent item, string name, ComponentList componentList) => Insert(index, item, name, componentList);

            /// <summary>
            /// Inserts the specified <paramref name="item" /> into the attached <paramref name="componentList" /> at the
            /// specified <paramref name="index" /> as well as adding it to the current <see cref="ContainerBase" />, assigning a name to it.
            /// </summary>
            /// <param name="index">The zero-based index at which to insert the <paramref name="item" /> into the attached <paramref name="componentList" /></param>
            /// <param name="item">The <seealso cref="IComponent" /> to insert.</param>
            /// <param name="componentList">The attached <seealso cref="ComponentList" /> to insert the <paramref name="item" /> into.</param>
            /// <remarks>If <paramref name="component" /> is <seealso cref="INamedComponent" />, then the value of the
            /// <seealso cref="INamedComponent.Name" /> property will be used to name the component within the
            /// current <see cref="ContainerBase" />; otherwise, it will be unnamed.</remarks>
            /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> was less than zero or greater than the number of items in
            /// <paramref name="componentList" />
            /// <para>-or-</para>
            /// <para>name to be assigned to the <paramref name="item" /> was already used by another item.</para></exception>
            /// <exception cref="ArgumentNullException"><paramref name="item"/> or <paramref name="componentList"/> was <c>null</c>.</exception>
            /// <exception cref="InvalidOperationException"><paramref name="componentList"/> was not attached to the
            /// current <see cref="ContainerBase" />.</exception>
            internal void InsertListItem(int index, IComponent item, ComponentList componentList) => Insert(index, item, componentList);

            /// <summary>
            /// Removes the specified <paramref name="item" /> from current <see cref="ContainerBase" /> as well as from any
            /// attached <paramref name="ComponentList" />s.
            /// </summary>
            /// <param name="component">The <seealso cref="IComponent" /> to remove.</param>
            public abstract void Remove(IComponent component);

            /// <summary>
            /// Removes the specified <paramref name="item" /> from the attached <paramref name="componentList" /> and from the
            /// current <see cref="ContainerBase" />.
            /// </summary>
            /// <param name="item">The <seealso cref="IComponent" /> to remove.</param>
            /// <param name="componentList">The attached <seealso cref="ComponentList" /> to remove the <paramref name="item" /> from.</param>
            /// <returns><c>true</c> if <paramref name="item" /> was removed from the attached <paramref name="componentList" />;
            /// otherwise, <c>false</c></returns>
            /// <exception cref="ArgumentNullException"><paramref name="componentList"/> was <c>null</c>.</exception>
            /// <exception cref="InvalidOperationException"><paramref name="componentList"/> was not attached to the
            /// current <see cref="ContainerBase" />.</exception>
            protected abstract bool Remove(IComponent item, ComponentList componentList);

            /// <summary>
            /// Removes the specified <paramref name="item" /> from the attached <paramref name="componentList" /> and from the
            /// current <see cref="ContainerBase" />.
            /// </summary>
            /// <param name="item">The <seealso cref="IComponent" /> to remove.</param>
            /// <param name="componentList">The attached <seealso cref="ComponentList" /> to remove the <paramref name="item" /> from.</param>
            /// <returns><c>true</c> if <paramref name="item" /> was removed from the attached <paramref name="componentList" />;
            /// otherwise, <c>false</c></returns>
            /// <exception cref="ArgumentNullException"><paramref name="componentList"/> was <c>null</c>.</exception>
            /// <exception cref="InvalidOperationException"><paramref name="componentList"/> was not attached to the
            /// current <see cref="ContainerBase" />.</exception>
            internal bool RemoveListItem(IComponent item, ComponentList componentList) => Remove(item, componentList);

            /// <summary>
            /// Removes the <seealso cref="IComponent" /> at the the specified <paramref name="index" /> of the
            /// <paramref name="componentList" /> and removes it from the current <see cref="ContainerBase" /> as well.
            /// </summary>
            /// <param name="index">The zero-based index of the <seealso cref="IComponent" /> to remove.</param>
            /// <param name="componentList">The attached <seealso cref="ComponentList" /> to remove the
            /// <seealso cref="IComponent" /> from.</param>
            /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> was less than zero or greater than the number of items in
            /// <paramref name="componentList" /></exception>
            /// <exception cref="ArgumentNullException"><paramref name="componentList"/> was <c>null</c>.</exception>
            /// <exception cref="InvalidOperationException"><paramref name="componentList"/> was not attached to the
            /// current <see cref="ContainerBase" />.</exception>
            protected abstract void RemoveAt(int index, ComponentList componentList);

            /// <summary>
            /// Removes the <seealso cref="IComponent" /> at the the specified <paramref name="index" /> of the
            /// <paramref name="componentList" /> and removes it from the current <see cref="ContainerBase" /> as well.
            /// </summary>
            /// <param name="index">The zero-based index of the <seealso cref="IComponent" /> to remove.</param>
            /// <param name="componentList">The attached <seealso cref="ComponentList" /> to remove the
            /// <seealso cref="IComponent" /> from.</param>
            /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> was less than zero or greater than the number of items in
            /// <paramref name="componentList" /></exception>
            /// <exception cref="ArgumentNullException"><paramref name="componentList"/> was <c>null</c>.</exception>
            /// <exception cref="InvalidOperationException"><paramref name="componentList"/> was not attached to the
            /// current <see cref="ContainerBase" />.</exception>
            internal void RemoveListItemAt(int index, ComponentList componentList) => RemoveAt(index, componentList);

            /// <summary>
            /// Replaces the <seealso cref="IComponent" /> at the the specified <paramref name="index" /> in the
            /// <paramref name="componentList" /> with another <paramref name="item" />, replacing it in the current <see cref="ContainerBase" /> as well.
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
            /// current <see cref="ContainerBase" />.</exception>
            protected abstract void SetAt(int index, IComponent item, ComponentList componentList);

            /// <summary>
            /// Replaces the <seealso cref="IComponent" /> at the the specified <paramref name="index" /> in the
            /// <paramref name="componentList" /> with another <paramref name="item" />, replacing it in the current <see cref="ContainerBase" /> as well.
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
            /// current <see cref="ContainerBase" />.</exception>
            internal void SetListItemAt(int index, IComponent item, ComponentList componentList) => SetAt(index, item, componentList);

            /// <summary>
            /// Replaces the <seealso cref="IComponent" /> at the the specified <paramref name="index" /> in the
            /// <paramref name="componentList" /> with another <paramref name="item" />, replacing it in the current <see cref="ContainerBase" /> as well.
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
            /// current <see cref="ContainerBase" />.</exception>
            protected abstract void SetAt(int index, IComponent item, string name, ComponentList componentList);

            /// <summary>
            /// Replaces the <seealso cref="IComponent" /> at the the specified <paramref name="index" /> in the
            /// <paramref name="componentList" /> with another <paramref name="item" />, replacing it in the current <see cref="ContainerBase" /> as well.
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
            /// current <see cref="ContainerBase" />.</exception>
            internal void SetListItemAt(int index, IComponent item, string name, ComponentList componentList) => SetAt(index, item, name, componentList);

            /// <summary>
            /// Gets the service object of the specified type.
            /// </summary>
            /// <param name="serviceType">An object that specifies the type of service object to get.</param>
            /// <returns>A service object of type <paramref name="serviceType" />.
            /// <para>-or-</para>
            /// <para><c>null</c> if there is no service object of type <paramref name="serviceType" />.</para></returns>
            public object GetService(Type serviceType) => (serviceType == typeof(IContainer) || serviceType == typeof(INestedContainer)) ? this : null;

            private void OnOwnerDisposed(object sender, EventArgs e)
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (Owner is null)
                        return;
                    Owner.Disposed -= OnOwnerDisposed;
                    Owner = null;
                    if (!_isDisposed)
                        Dispose();
                }
                finally { Monitor.Exit(SyncRoot); }
            }

            protected virtual void Dispose(bool disposing)
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (_isDisposed)
                        return;
                    if (disposing)
                    {
                        if (null != Owner)
                        {
                            Owner.Disposed -= OnOwnerDisposed;
                            Owner = null;
                        }
                        OnDisposing();
                    }
                    _isDisposed = true;
                }
                finally { Monitor.Exit(SyncRoot); }
            }

            /// <summary>
            /// This gets called to dispose all <seealso cref="IComponent" />s in the current <see cref="ContainerBase" />, clearing and
            /// detaching all attached <seealso cref="ComponentList" />s.
            /// </summary>
            protected abstract void OnDisposing();

            public void Dispose()
            {
                Dispose(disposing: true);
                GC.SuppressFinalize(this);
            }
        }
    }
}

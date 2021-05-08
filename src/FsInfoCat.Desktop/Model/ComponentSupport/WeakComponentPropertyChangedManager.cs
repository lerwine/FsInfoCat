using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FsInfoCat.Desktop.Model.ComponentSupport
{
    /// <summary>
    /// Manages <see cref="WeakReference{T}"><code>WeakReference&lt;EventHandler&gt;</code></see> to
    /// handle <see cref="PropertyDescriptor.AddValueChanged(object, EventHandler)"/> callbacks.
    /// </summary>
    public static class WeakComponentPropertyChangedManager
    {
        private static readonly LinkedList<Owner> _owners = new LinkedList<Owner>();

        /// <summary>
        /// Adds an <see cref="EventHandler"/> as a <see cref="WeakReference{T}"><code>WeakReference&lt;EventHandler&gt;</code></see> using
        /// the <see cref="PropertyDescriptor.AddValueChanged(object, EventHandler)"/> method.
        /// </summary>
        /// <param name="component">The component to add the handler for.</param>
        /// <param name="propertyDescriptor">The <see cref="PropertyDescriptor"/> that will be used to add the handler.</param>
        /// <param name="eventHandler">The delegate to add as a <see cref="WeakReference{T}"><code>WeakReference&lt;EventHandler&gt;</code></see>
        /// listener.</param>
        public static void AddValueChanged(object component, PropertyDescriptor propertyDescriptor, EventHandler eventHandler)
        {
            lock (_owners)
            {
                if (None<object, Owner>(_owners, item => item.TryAdd(component, propertyDescriptor, eventHandler)))
                    _owners.AddLast(new Owner(component, propertyDescriptor, eventHandler));
            }
        }

        /// <summary>
        /// Removes an <see cref="EventHandler"/> which was referenced by
        /// a <see cref="WeakReference{T}"><code>WeakReference&lt;EventHandler&gt;</code></see> using
        /// the <see cref="PropertyDescriptor.RemoveValueChanged(object, EventHandler)"/> method.
        /// </summary>
        /// <param name="component">The component.</param>
        /// <param name="propertyDescriptor">The <see cref="PropertyDescriptor"/> that will be used to remove the handler.</param>
        /// <param name="eventHandler">The delegate that was added within a <see cref="WeakReference{T}"><code>WeakReference&lt;EventHandler&gt;</code></see>
        public static void RemoveValueChanged(object component, PropertyDescriptor propertyDescriptor, EventHandler handler)
        {
            lock (_owners)
            {
                if (TryFindItem<object, Owner>(_owners, o => ReferenceEquals(o, component), out Owner owner))
                    owner.RemoveValueChanged(handler, component, propertyDescriptor);
            }
        }

        private delegate bool TryGet<TIn, TOut>(TIn input, out TOut result);

        private static bool None<TTarget, TItem>(LinkedList<TItem> source, Func<TItem, bool> predicate)
            where TTarget : class
            where TItem : WeakReferenceItem<TTarget>
        {
            LinkedListNode<TItem> node = source.First;
            while (!(node is null))
            {
                LinkedListNode<TItem> next = node.Next;
                if (predicate(node.Value))
                    return false;
                node = next;
            }
            return true;
        }

        private static bool TryFind<TTarget, TItem, TResult>(LinkedList<TItem> source, TryGet<TItem, TResult> tryGet, out TResult result)
            where TTarget : class
            where TItem : WeakReferenceItem<TTarget>
        {
            LinkedListNode<TItem> node = source.First;
            while (!(node is null))
            {
                LinkedListNode<TItem> next = node.Next;
                if (tryGet(node.Value, out result))
                    return true;
                node = next;
            }
            result = default;
            return false;
        }

        private static bool TryFindItem<TTarget, TItem>(LinkedList<TItem> source, Func<TTarget, bool> predicate, out TItem result)
            where TTarget : class
            where TItem : WeakReferenceItem<TTarget>
        {
            LinkedListNode<TItem> node = source.First;
            while (!(node is null))
            {
                if (node.Value.TryGetTarget(out TTarget target))
                {
                    if (predicate(target))
                    {
                        result = node.Value;
                        return true;
                    }
                    node = node.Next;
                }
                else
                {
                    LinkedListNode<TItem> next = node.Next;
                    source.Remove(node);
                    node = next;
                }
            }
            result = null;
            return false;
        }

        private abstract class WeakReferenceItem<T>
            where T : class
        {
            private readonly WeakReference<T> _target;

            internal bool TryGetTarget(out T target)
            {
                if (_target.TryGetTarget(out target))
                    return true;
                OnTargetReclaimed();
                return false;
            }

            protected abstract void OnTargetReclaimed();

            public WeakReferenceItem(T target)
            {
                _target = new WeakReference<T>(target);
            }
        }

        private class Handler : WeakReferenceItem<EventHandler>
        {
            private readonly Descriptor _descriptor;

            public Handler(Descriptor descriptor, EventHandler target) : base(target)
            {
                _descriptor = descriptor;
            }

            protected override void OnTargetReclaimed() => _descriptor.Remove(this);

            internal void ValueChanged(object sender, EventArgs e)
            {
                if (TryGetTarget(out EventHandler target))
                    target(sender, e);
            }

            internal static void AddValueChanged(LinkedList<Handler> handlers, Descriptor descriptor, EventHandler eventHandler, out Handler handler)
            {
                if (None<EventHandler, Handler>(handlers, e => ReferenceEquals(e, eventHandler)))
                {
                    handler = new Handler(descriptor, eventHandler);
                    handlers.AddLast(handler);
                }
                else
                    handler = null;
            }
        }

        private class Descriptor : WeakReferenceItem<PropertyDescriptor>
        {
            private readonly Owner _owner;

            internal bool TryGetOwner(out object component) => _owner.TryGetTarget(out component);

            private LinkedList<Handler> _handlers = new LinkedList<Handler>();

            private Descriptor(Owner owner, PropertyDescriptor target) : base(target)
            {
                _owner = owner;
            }

            protected override void OnTargetReclaimed() => _owner.Remove(this);

            internal bool TryRemoveHandler(EventHandler eventHandler, PropertyDescriptor propertyDescriptor, out EventHandler removed)
            {
                if (TryGetTarget(out PropertyDescriptor pd) && ReferenceEquals(pd, propertyDescriptor))
                {
                    LinkedListNode<Handler> node = _handlers.First;
                    while (!(node is null))
                    {
                        LinkedListNode<Handler> next = node;
                        if (node.Value.TryGetTarget(out EventHandler e) && ReferenceEquals(e, eventHandler))
                        {
                            removed = node.Value.ValueChanged;
                            _handlers.Remove(node);
                            if (_handlers.Count == 0)
                                _owner.Remove(this);
                            return true;
                        }
                        node = next;
                    }
                }
                removed = null;
                return false;
            }

            internal void Remove(Handler handler)
            {
                if (_handlers.Remove(handler) && _handlers.Count == 0)
                    _owner.Remove(this);
            }

            private bool TryAdd(PropertyDescriptor propertyDescriptor, EventHandler eventHandler, out Handler handler)
            {
                if (TryGetTarget(out PropertyDescriptor d) && ReferenceEquals(d, propertyDescriptor))
                {
                    Handler.AddValueChanged(_handlers, this, eventHandler, out handler);
                    return true;
                }
                handler = null;
                return false;
            }

            internal static void AddValueChanged(LinkedList<Descriptor> descriptors, Owner owner, PropertyDescriptor propertyDescriptor,
                EventHandler eventHandler, out Handler handler)
            {
                if (!TryFind<PropertyDescriptor, Descriptor, Handler>(descriptors,
                    (Descriptor item, out Handler h) => item.TryAdd(propertyDescriptor, eventHandler, out h), out handler))
                {
                    Descriptor result = new Descriptor(owner, propertyDescriptor);
                    handler = new Handler(result, eventHandler);
                    result._handlers.AddLast(handler);
                }
            }

            internal static Descriptor Create(Owner owner, PropertyDescriptor propertyDescriptor, EventHandler eventHandler, out Handler handler)
            {
                Descriptor result = new Descriptor(owner, propertyDescriptor);
                handler = new Handler(result, eventHandler);
                result._handlers.AddLast(handler);
                return result;
            }
        }

        private class Owner : WeakReferenceItem<object>
        {
            private readonly LinkedList<Descriptor> _descriptors = new LinkedList<Descriptor>();

            internal Owner(object component, PropertyDescriptor target, EventHandler eventHandler) : base(component)
            {
                _descriptors.AddLast(Descriptor.Create(this, target, eventHandler, out Handler handler));
                target.AddValueChanged(component, handler.ValueChanged);
            }

            protected override void OnTargetReclaimed() => _owners.Remove(this);

            internal void Remove(Descriptor descriptor)
            {
                if (_descriptors.Remove(descriptor) && _descriptors.Count == 0)
                    _owners.Remove(this);
            }

            internal void RemoveValueChanged(EventHandler eventHandler, object component, PropertyDescriptor propertyDescriptor)
            {
                if (TryGetTarget(out object o) && ReferenceEquals(o, component))
                {
                    LinkedListNode<Descriptor> node = _descriptors.First;
                    while (!(node is null))
                    {
                        LinkedListNode<Descriptor> next = node;
                        if (node.Value.TryGetTarget(out PropertyDescriptor pd) && ReferenceEquals(pd, propertyDescriptor))
                        {
                            if (node.Value.TryRemoveHandler(eventHandler, propertyDescriptor, out EventHandler handler))
                                propertyDescriptor.RemoveValueChanged(component, handler);
                            return;
                        }
                        node = next;
                    }
                }
            }

            internal bool TryAdd(object component, PropertyDescriptor propertyDescriptor, EventHandler eventHandler)
            {
                if (TryGetTarget(out object c) && ReferenceEquals(c, component))
                {
                    Descriptor.AddValueChanged(_descriptors, this, propertyDescriptor, eventHandler, out Handler handler);
                    if (!(handler is null))
                        propertyDescriptor.AddValueChanged(component, handler.ValueChanged);
                    return true;
                }
                return false;
            }
        }
    }
}

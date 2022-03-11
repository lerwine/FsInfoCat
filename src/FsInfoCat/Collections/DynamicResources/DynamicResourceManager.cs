using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Collections.DynamicResources
{
    public abstract class Registration<TResource, TRegistration> //: IDynamicResourceRegistration<TResource>
        where TRegistration : Registration<TResource, TRegistration>
    {
        private readonly object _syncRoot = new object();
        private DynamicResourceManager _owner;
        private TRegistration _previous;
        private TRegistration _next;

        public TResource Resource
        {
            get
            {
                Monitor.Enter(_syncRoot);
                try
                {
                    if (_owner == null)
                        throw new ObjectDisposedException(GetType().FullName);
                    return _owner.Resource;
                }
                finally { Monitor.Exit(_syncRoot); }
            }
        }

        protected Registration(DynamicResourceManager owner) { _owner = owner; }

        protected virtual void Dispose(bool disposing)
        {
            Monitor.Enter(_syncRoot);
            try
            {
                if (_owner == null)
                    return;
                if (disposing)
                    _owner.Unregister(this);
                else
                    _owner = null;
            }
            finally { Monitor.Exit(_syncRoot); }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public abstract class DynamicResourceManager //: IDynamicResourceManager<TResource, TRegistration>
        {
            private readonly object _syncRoot = new object();
            private TRegistration _first;
            private TRegistration _last;

            public TResource Resource { get; private set; }

            public IEnumerable<TRegistration> GetRegistrations()
            {
                for (TRegistration registration = _first; registration != null; registration = registration._next)
                    yield return registration;
            }

            public TRegistration Register()
            {
                TRegistration registration;
                Monitor.Enter(_syncRoot);
                try
                {
                    if (_last == null)
                    {
                        Resource = InitializeResource();
                        registration = CreateNewRegistration();
                        _first = _last = registration;
                    }
                    else
                        ((registration = CreateNewRegistration())._previous = _last)._next = registration;
                }
                finally { Monitor.Exit(_syncRoot); }
                return registration;
            }

            internal void Unregister(Registration<TResource, TRegistration> registration)
            {
                Monitor.Enter(registration._syncRoot);
                try
                {
                    Monitor.Enter(_syncRoot);
                    try
                    {
                        if (registration._owner == null)
                            return;
                        if (!ReferenceEquals(registration._owner, this))
                            throw new InvalidOperationException();
                        if (registration._next == null)
                        {
                            if (registration._previous == null)
                            {
                                Resource = FinalizeResource(Resource);
                                _first = _last = null;
                            }
                            else
                                registration._previous = (_last = registration._previous)._next = null;
                        }
                        else
                        {
                            if ((registration._next._previous = registration._previous) == null)
                                _first = registration._next;
                            else
                            {
                                registration._previous._next = registration._next;
                                registration._previous = null;
                            }
                            registration._next = null;
                        }
                        registration._owner = null;
                    }
                    finally { Monitor.Exit(_syncRoot); }
                }
                finally { Monitor.Exit(registration._syncRoot); }
            }

            protected abstract TRegistration CreateNewRegistration();

            protected abstract TResource InitializeResource();

            protected abstract TResource FinalizeResource(TResource currentResource);

            //IEnumerable<IDynamicResourceRegistration<TResource>> IDynamicResourceManager<TResource>.GetRegistrations() { return GetRegistrations(); }

            //IDynamicResourceRegistration<TResource> IDynamicResourceManager<TResource>.Register() { return Register(); }
        }
    }

    public abstract class ParameterizedDynamicResourceManager<TKey, TResource> : Registration<ParameterizedDynamicResourceManager<TKey, TResource>.InnerManager, ParameterizedDynamicResourceManager<TKey, TResource>.Registration>.DynamicResourceManager
    {
        private readonly IEqualityComparer<TKey> _comparer;

        protected ParameterizedDynamicResourceManager(IEqualityComparer<TKey> comparer)
        {
            _comparer = (comparer == null) ? EqualityComparer<TKey>.Default : comparer;
        }

        public abstract InnerRegistration Register(TKey key);

        protected override Registration CreateNewRegistration() { return new Registration(this); }

        protected override InnerManager InitializeResource()
        {
            throw new NotImplementedException();
        }

        protected override InnerManager FinalizeResource(InnerManager currentResource)
        {
            throw new NotImplementedException();
        }

        public class Registration : Registration<InnerManager, Registration>
        {
            public Registration(ParameterizedDynamicResourceManager<TKey, TResource> owner) : base(owner) { }
        }

        public class InnerRegistration : Registration<TResource, InnerRegistration>
        {
            public InnerRegistration(InnerManager owner) : base(owner) { }
        }

        public class InnerManager : Registration<TResource, InnerRegistration>.DynamicResourceManager
        {
            public TKey Key { get; }

            public InnerManager(TKey key) { Key = key; }

            protected override InnerRegistration CreateNewRegistration() { return new InnerRegistration(this); }

            protected override TResource InitializeResource()
            {
                throw new NotImplementedException();
            }

            protected override TResource FinalizeResource(TResource currentResource)
            {
                throw new NotImplementedException();
            }
        }
    }

    public abstract class ParameterizedDynamicResourceManager<TKey, TResource, TManager, TRegistration, TInnerRegistration> : Registration<TManager, TRegistration>.DynamicResourceManager
        where TManager : ParameterizedDynamicResourceManager<TKey, TResource, TManager, TRegistration, TInnerRegistration>.InnerManager
        where TRegistration : Registration<TManager, TRegistration>
        where TInnerRegistration : Registration<TResource, TInnerRegistration>
    {
        private readonly IEqualityComparer<TKey> _comparer;

        protected ParameterizedDynamicResourceManager(IEqualityComparer<TKey> comparer)
        {
            _comparer = (comparer == null) ? EqualityComparer<TKey>.Default : comparer;
        }

        public abstract TInnerRegistration Register(TKey key);

        public abstract class InnerManager : Registration<TResource, TInnerRegistration>.DynamicResourceManager
        {
            public TKey Key { get; }

            public InnerManager(TKey key)
            {
                Key = key;
            }

            protected override TResource FinalizeResource(TResource currentResource)
            {
                throw new NotImplementedException();
            }
        }
    }
}

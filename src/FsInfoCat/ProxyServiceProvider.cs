using System;

namespace FsInfoCat
{
    public abstract class ProxyServiceProvider : IServiceProvider
    {
        private readonly IServiceProvider _backingServiceProvider;

        public ProxyServiceProvider(IServiceProvider backingServiceProvider)
        {
            _backingServiceProvider = backingServiceProvider;
        }

        protected abstract bool TryGetService(Type serviceType, out object service);

        public object GetService(Type serviceType)
        {
            if (TryGetService(serviceType, out object service))
                return service;
            return _backingServiceProvider?.GetService(serviceType);
        }
    }

    public class ProxyServiceProvider<T> : ProxyServiceProvider
        where T : class
    {
        private readonly WeakReference<T> _service;

        public ProxyServiceProvider(T service, IServiceProvider backingServiceProvider)
            : base(backingServiceProvider)
        {
            _service = new WeakReference<T>(service);
        }
        protected override bool TryGetService(Type serviceType, out object service)
        {
            if (serviceType is not null && serviceType.IsAssignableFrom(typeof(T)))
            {
                service = _service.TryGetTarget(out T s) ? s : null;
                return true;
            }
            service = null;
            return false;
        }
    }
}

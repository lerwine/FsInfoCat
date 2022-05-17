using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat
{
    // TODO: Document ProxyServiceProvider class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public abstract class ProxyServiceProvider : IServiceProvider
    {
        private readonly IServiceProvider _backingServiceProvider;

        public ProxyServiceProvider([DisallowNull] IServiceProvider backingServiceProvider) => _backingServiceProvider = backingServiceProvider ??
            throw new ArgumentNullException(nameof(backingServiceProvider));

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

        public ProxyServiceProvider([DisallowNull] T service, [DisallowNull] IServiceProvider backingServiceProvider) : base(backingServiceProvider)
            => _service = new WeakReference<T>(service ?? throw new ArgumentNullException(nameof(service)));

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
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}

using System;
using System.Collections.ObjectModel;

namespace FsInfoCat.Desktop.Model.ComponentSupport
{
    public abstract class ModelContextBase<TInstance, TPropertyContext> : IServiceProvider
        where TInstance : class
        where TPropertyContext : IPropertyContext<TInstance>
    {
        public event PropertyValueChangedEventHandler PropertyValueChanged;

        public string SimpleName => ModelDescriptor.SimpleName;

        public string FullName => ModelDescriptor.FullName;

        public TInstance Instance { get; }

        public ModelDescriptor<TInstance> ModelDescriptor { get; }

        public ReadOnlyCollection<TPropertyContext> Properties => throw new NotImplementedException();

        protected ModelContextBase(ModelDescriptor<TInstance> modelDescriptor, TInstance instance)
        {
            ModelDescriptor = modelDescriptor ?? throw new ArgumentNullException(nameof(modelDescriptor));
            Instance = instance ?? throw new ArgumentNullException(nameof(instance));
            // TODO: Create properties
        }

        object IServiceProvider.GetService(Type serviceType) => null;
    }
}

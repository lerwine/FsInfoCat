using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

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

        public ReadOnlyCollection<TPropertyContext> Properties { get; }

        protected ModelContextBase(ModelDescriptor<TInstance> modelDescriptor, TInstance instance,
            Func<ModelContextBase<TInstance, TPropertyContext>, IModelPropertyDescriptor<TInstance>, TPropertyContext> propertyFactory)
        {
            ModelDescriptor = modelDescriptor ?? throw new ArgumentNullException(nameof(modelDescriptor));
            Instance = instance ?? throw new ArgumentNullException(nameof(instance));
            Collection<TPropertyContext> properties = new Collection<TPropertyContext>();
            Properties = new ReadOnlyCollection<TPropertyContext>(properties);
            foreach (IModelPropertyDescriptor<TInstance> pd in modelDescriptor.Properties)
                properties.Add(propertyFactory(this, pd));
            foreach (TPropertyContext property in Properties)
                WeakEventManager<TPropertyContext, ValueChangedEventArgs>.AddHandler(property, nameof(IPropertyContext<TInstance>.ValueChanged),
                    OnPropertyValueChanged);
        }

        private void OnPropertyValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (sender is IPropertyContext propertyContext)
                OnPropertyValueChanged(new PropertyValueChangedEventArgs(e.OldValue, propertyContext));
        }

        protected virtual void OnPropertyValueChanged(PropertyValueChangedEventArgs args)
        {
            PropertyValueChanged?.Invoke(this, args);
        }

        object IServiceProvider.GetService(Type serviceType) => null;
    }
}

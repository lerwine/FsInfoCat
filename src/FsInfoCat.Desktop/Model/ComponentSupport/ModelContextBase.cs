using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace FsInfoCat.Desktop.Model.ComponentSupport
{
    /// <summary>
    /// Base class for objects that provide contextual information about an object type.
    /// </summary>
    /// <typeparam name="TInstance">The type of the object instance.</typeparam>
    /// <typeparam name="TPropertyContext">The type of the property context element.</typeparam>
    /// <seealso cref="System.IServiceProvider" />
    public abstract class ModelContextBase<TInstance, TPropertyContext> : IServiceProvider
        where TInstance : class
        where TPropertyContext : class, IPropertyContext<TInstance>
    {
        /// <summary>
        /// Occurs when the value of <typeparam name="TPropertyContext"> element of <see cref="Properties"/> has changed.
        /// </summary>
        public event PropertyValueChangedEventHandler PropertyValueChanged;

        /// <summary>
        /// Gets the simple name for the type of the underlying model instance.
        /// </summary>
        /// <value>
        /// The <see cref="RuntimeType.Name"/> for the type of the underlying model <see cref="ITypeDescriptorContext.Instance"/>.
        /// </value>
        public string SimpleName => ModelDescriptor.SimpleName;

        /// <summary>
        /// Gets the full name for the type of the underlying model instance.
        /// </summary>
        /// <value>
        /// The <see cref="Type.FullName"/> for the type of the underlying model <see cref="ITypeDescriptorContext.Instance"/>.
        /// </value>
        public string FullName => ModelDescriptor.FullName;

        /// <summary>
        /// Gets the object that is connected with this context.
        /// </summary>
        /// <value>
        /// The object that is connected with this context.
        /// </value>
        public TInstance Instance { get; }

        /// <summary>
        /// Gets the descriptor for the type of the underlying model instance.
        /// </summary>
        /// <value>
        /// The <see cref="ModelDescriptor{TModel}"/> that describes the type of the underlying model <see cref="ITypeDescriptorContext.Instance"/>.
        /// </value>
        public ModelDescriptor<TInstance> ModelDescriptor { get; }

        /// <summary>
        /// Gets the context objects that represent the properties of the underlying model instance.
        /// </summary>
        /// <value>
        /// The <see cref="TPropertyContext"/> objects that represent the properties of the underlying model <see cref="ITypeDescriptorContext.Instance"/>.
        /// </value>
        public ReadOnlyCollection<TPropertyContext> Properties { get; }

        /// <summary>
        /// Gets the list of property names that reference property contextual informationa.
        /// </summary>
        /// <value>
        /// The list of <see cref="IModelProperty.Name">property names</see> that can be used to look
        /// up <typeparamref name="TPropertyContext"/> which provide contextual information about properties of the
        /// associated <see cref="Instance"/>.
        /// </value>
        public IEnumerable<string> Keys => Properties.Select(p => p.Name);

        /// <summary>
        /// Gets the <see cref="TPropertyContext"/> with the specified <see cref="IModelProperty.Name"/>.
        /// </summary>
        /// <param name="key">The property <see cref="IModelProperty.Name"/>.</param>
        /// <returns>
        /// The <see cref="TPropertyContext"/> that has the given <see cref="IModelProperty.Name"/> or <see langword="null"/>
        /// if none were found.</returns>
        public TPropertyContext this[string key] => Properties.FirstOrDefault(p => p.Name.Equals(key));

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

        public bool ContainsKey(string key) => Properties.Any(p => p.Name.Equals(key));

        public bool TryGetValue(string key, out TPropertyContext value)
        {
            value = Properties.FirstOrDefault(p => p.Name.Equals(key));
            return !(value is null);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FsInfoCat.ComponentSupport
{
    internal abstract class ModelPropertyDescriptor<TModel> : IModelPropertyDescriptor<TModel> where TModel : class
    {
        internal ModelDescriptor<TModel> Owner { get; }

        internal PropertyDescriptor Descriptor { get; }

        public bool UseInvariantStringConversion { get; }

        public ReadOnlyCollection<ValidationAttribute> ValidationAttributes { get; }

        IReadOnlyList<ValidationAttribute> IModelPropertyDescriptor.ValidationAttributes => ValidationAttributes;

        IModelTypeDescriptor<TModel> IModelPropertyDescriptor<TModel>.Owner => Owner;

        IModelDescriptor<TModel> IModelProperty<TModel>.Owner => Owner;

        IModelTypeDescriptor IModelPropertyDescriptor.Owner => Owner;

        IModelDescriptor IModelProperty.Owner => Owner;

        public string Name => Descriptor.Name;

        public Type PropertyType => Descriptor.PropertyType;

        public string Description => Descriptor.Description ?? "";

        public string Category
        {
            get
            {
                string c = Descriptor.Category;
                return string.IsNullOrWhiteSpace(c) ? CategoryAttribute.Default.Category : c;
            }
        }

        public string DisplayName
        {
            get
            {
                string d = Descriptor.DisplayName;
                return string.IsNullOrWhiteSpace(d) ? Name : d;
            }
        }

        public bool IsReadOnly => Descriptor.IsReadOnly;

        public bool SupportsChangeEvents => Descriptor.SupportsChangeEvents;

        public bool AreStandardValuesExclusive => Descriptor.Converter.GetStandardValuesExclusive();

        public bool AreStandardValuesSupported => Descriptor.Converter.GetCreateInstanceSupported();

        protected ModelPropertyDescriptor(PropertyBuilder<TModel> builder)
        {
            UseInvariantStringConversion = builder.UseInvariantStringConversion;
            Descriptor = builder.Descriptor;
            Owner = builder.Owner;
            ValidationAttributes = new ReadOnlyCollection<ValidationAttribute>(builder.ValidationAttributes.ToArray());
        }

        internal abstract IModelPropertyContext<TModel> CreateContext(ModelContext<TModel> owner);

        protected abstract object BaseGetValue(TModel model);

        object IModelPropertyDescriptor<TModel>.GetValue(TModel model) => BaseGetValue(model);

        object IModelPropertyDescriptor.GetValue(object model) => BaseGetValue((TModel)model);
    }

    internal sealed class ModelPropertyDescriptor<TModel, TValue> : ModelPropertyDescriptor<TModel>, IModelPropertyDescriptor<TModel, TValue> where TModel : class
    {
        internal IEqualityComparer<TValue> Comparer { get; }

        internal ModelPropertyDescriptor(PropertyBuilder<TModel, TValue> builder)
            : base(builder)
        {
            Comparer = builder.Comparer ?? EqualityComparer<TValue>.Default;
        }

        public TValue GetValue(TModel model) => (TValue)Descriptor.GetValue(model);

        protected override object BaseGetValue(TModel model) => GetValue(model);

        internal override IModelPropertyContext<TModel> CreateContext(ModelContext<TModel> owner) => new PropertyContext<TModel, TValue>(owner, this);

        TValue ITypedModelPropertyDescriptor<TValue>.GetValue(object model) => GetValue((TModel)model);
    }
}

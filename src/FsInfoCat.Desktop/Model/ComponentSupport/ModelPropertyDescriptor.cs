using FsInfoCat.Desktop.Model.Validation;
using FsInfoCat.Desktop.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FsInfoCat.Desktop.Model.ComponentSupport
{
    public class ModelPropertyDescriptor<TModel, TValue> : IEquatable<ModelPropertyDescriptor<TModel, TValue>>, ITypedPropertyDescriptor<TValue>, IModelPropertyDescriptor<TModel>
        where TModel : class
    {
        private readonly PropertyDescriptor _descriptor;
        private readonly IEqualityComparer<TValue> _backingComparer;

        public ReadOnlyCollection<ValidationAttribute> ValidationAttributes { get; }

        IReadOnlyList<ValidationAttribute> IModelPropertyDescriptor.ValidationAttributes => ValidationAttributes;

        public ModelDescriptor<TModel> Owner { get; }

        IModelDescriptor IModelPropertyDescriptor.Owner => Owner;

        public Type PropertyType => _descriptor.PropertyType;

        public bool SupportsChangeEvents => _descriptor.SupportsChangeEvents;

        public bool AreStandardValuesSupported => _descriptor.Converter.GetStandardValuesSupported();

        public bool AreStandardValuesExclusive => _descriptor.Converter.GetStandardValuesExclusive();

        public string Name => _descriptor.Name;

        public string Description => _descriptor.Description ?? "";

        public string Category => string.IsNullOrWhiteSpace(_descriptor.Category) ? CategoryAttribute.Default.Category : _descriptor.Category;

        public string DisplayName => string.IsNullOrWhiteSpace(_descriptor.DisplayName) ? _descriptor.Name : _descriptor.DisplayName;

        public bool IsReadOnly => _descriptor.IsReadOnly;

        public ModelPropertyDescriptor(PropertyBuilder<TModel, TValue> builder)
        {
            Owner = (builder ?? throw new ArgumentNullException(nameof(builder))).ModelDescriptor;
            _descriptor = builder.PropertyDescriptor;
            _backingComparer = builder.EqualityComparer ?? EqualityComparer<TValue>.Default;
            ValidationAttributes = new ReadOnlyCollection<ValidationAttribute>(builder.ValidationAttributes.ToArray());
        }

        public TValue GetValue(TModel component) => (TValue)_descriptor.GetValue(component ?? throw new ArgumentNullException(nameof(component)));

        public PropertyValidationContext<TModel, TValue> CreateInstanceValidationProperty(ModelValidationContext<TModel> owner) =>
            new PropertyValidationContext<TModel, TValue>(owner, _descriptor, _backingComparer, ValidationAttributes);

        public PropertyContext<TModel, TValue> CreateInstanceProperty(ModelContext<TModel> owner) =>
            new PropertyContext<TModel, TValue>(owner, _descriptor, _backingComparer);

        public bool CanConvertFrom(Type sourceType) => _descriptor.Converter.CanConvertFrom(sourceType);

        public TValue ConvertFrom(object value) => (TValue)_descriptor.Converter.ConvertFrom(value);

        object IModelPropertyDescriptor.ConvertFrom(object value) => ConvertFrom(value);

        public TValue ConvertFromInvariantString(string text) => (TValue)_descriptor.Converter.ConvertFromInvariantString(text);

        object IModelPropertyDescriptor.ConvertFromInvariantString(string text) => ConvertFromInvariantString(text);

        public TValue ConvertFromString(string text) => (TValue)_descriptor.Converter.ConvertFromString(text);

        object IModelPropertyDescriptor.ConvertFromString(string text) => ConvertFromString(text);

        public string ConvertToInvariantString(TValue value) => _descriptor.Converter.ConvertToInvariantString(value);

        string IModelPropertyDescriptor.ConvertToInvariantString(object value)
        {
            TValue c;
            try { c = ConvertFrom(value); }
            catch (Exception exc)
            {
                throw new InvalidCastException(string.IsNullOrWhiteSpace(exc.Message) ? "Unable to convert value" : exc.Message, exc);
            }
            return ConvertToInvariantString(c);
        }

        public string ConvertToString(TValue value) => _descriptor.Converter.ConvertToString(value);

        string IModelPropertyDescriptor.ConvertToString(object value)
        {
            TValue c;
            try { c = ConvertFrom(value); }
            catch (Exception exc)
            {
                throw new InvalidCastException(string.IsNullOrWhiteSpace(exc.Message) ? "Unable to convert value" : exc.Message, exc);
            }
            return ConvertToString(c);
        }

        public IEnumerable<TValue> GetStandardValues()
        {
            ICollection values = _descriptor.Converter.GetStandardValues();
            if (values is null)
                return new TValue[0];
            return values.Cast<TValue>();
        }

        ICollection IModelProperty.GetStandardValues() => _descriptor.Converter.GetStandardValues();

        public bool IsAssignableFrom(object value) => _descriptor.Converter.IsValid(value);

        public override int GetHashCode()
        {
            int[] hc = new int[] { Name.GetHashCode(), IsReadOnly.GetHashCode(),
                AreStandardValuesSupported ? (AreStandardValuesExclusive ? 2 : 1) : 0, Owner.FullName.GetHashCode() };
            if (AreStandardValuesSupported)
            {
                IEnumerable<TValue> values = GetStandardValues();
                if (!(values is null))
                    return hc.Concat(values.Select(v => _backingComparer.GetHashCode(v))).GetAggregateHashCode();
            }
            return hc.GetAggregateHashCode();
        }

        private bool EqualTo(ITypedPropertyDescriptor<TValue> other)
        {
            if (Name.Equals(other.Name) && IsReadOnly == other.IsReadOnly)
            {
                if (AreStandardValuesSupported)
                {
                    if (other.AreStandardValuesSupported)
                    {
                        if (AreStandardValuesExclusive)
                        {
                            if (other.AreStandardValuesExclusive)
                            {
                                IEnumerable<TValue> a = GetStandardValues();
                                IEnumerable<TValue> b = other.GetStandardValues();
                                if (a is null)
                                    return b is null;
                                return !(b is null) && a.SequenceEqual(b);
                            }
                        }
                        else
                            return !other.AreStandardValuesExclusive;
                    }
                }
                else
                    return !other.AreStandardValuesSupported;
            }
            return false;
        }

        public bool Equals(ModelPropertyDescriptor<TModel, TValue> other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return EqualTo(other);
        }

        public bool Equals(IModelPropertyDescriptor<TModel> other)
        {
            if (other is null)
                return false;
            if (other is ModelPropertyDescriptor<TModel, TValue> mpd)
                return Equals(mpd);
            return other is ITypedPropertyDescriptor<TValue> t && EqualTo(t);
        }

        public bool Equals(ITypedPropertyDescriptor<TValue> other)
        {
            if (other is null)
                return false;
            if (other is ModelPropertyDescriptor<TModel, TValue> mpd)
                return Equals(mpd);
            return (other is IModelPropertyDescriptor<TModel> || other.Owner.ComponentType.IsAssignableFrom(typeof(TModel)) ||
                typeof(TModel).IsAssignableFrom(other.Owner.ComponentType)) && EqualTo(other);
        }

        bool IEquatable<IModelPropertyDescriptor>.Equals(IModelPropertyDescriptor other)
        {
            if (other is ModelPropertyDescriptor<TModel, TValue> mpd)
                return Equals(mpd);
            if (other is IModelPropertyDescriptor<TModel> m)
                return Equals(m);
            return other is ITypedPropertyDescriptor<TValue> t && Equals(t);
        }

        public override bool Equals(object obj) => Equals(obj as IModelPropertyDescriptor);

        public bool Equals(TValue x, TValue y) => _backingComparer.Equals(x, y);

        bool IEqualityComparer.Equals(object x, object y)
        {
            if (IsAssignableFrom(x))
            {
                if (IsAssignableFrom(y))
                    try { return Equals(ConvertFrom(x), ConvertFrom(y)); } catch { /* Ignored on purpose */ }
                else
                    return false;
            }
            else if (IsAssignableFrom(y))
                return false;
            if (x is null)
                return y is null;
            return x.Equals(y);
        }

        public int GetHashCode(TValue obj) => _backingComparer.GetHashCode(obj);

        int IEqualityComparer.GetHashCode(object obj)
        {
            if (IsAssignableFrom(obj))
                try { return GetHashCode(ConvertFrom(obj)); } catch { /* Ignored on purpose */ }
            return (obj is null) ? 0 : obj.GetHashCode();
        }

        public override string ToString()
        {
            return $@"{nameof(ModelPropertyDescriptor<TModel, TValue>)}<{Owner.SimpleName}, {PropertyType.Name}> {{
    Name = {TypeHelper.ToPseudoCsText(Name)},
    Category = {TypeHelper.ToPseudoCsText(Category)},
    DisplayName = {TypeHelper.ToPseudoCsText(DisplayName)},
    IsReadOnly = {TypeHelper.ToPseudoCsText(IsReadOnly)},
    PropertyType = {TypeHelper.ToPseudoCsText(PropertyType)},
    Description = {TypeHelper.ToPseudoCsText(Description)}
}}";
        }

        TValue ITypedPropertyDescriptor<TValue>.GetValue(object component) => GetValue((TModel)component);

        object IModelPropertyDescriptor<TModel>.GetValue(TModel component) => GetValue(component);

        object IModelPropertyDescriptor.GetValue(object component) => GetValue((TModel)component);

        IPropertyValidationContext<TModel> IModelPropertyDescriptor<TModel>.CreateInstanceValidationProperty(ModelValidationContext<TModel> owner) =>
            CreateInstanceValidationProperty(owner);

        IPropertyValidationContext IModelPropertyDescriptor.CreateInstanceValidationProperty(IModelValidationContext owner) =>
            CreateInstanceValidationProperty((ModelValidationContext<TModel>)owner);

        IPropertyContext<TModel> IModelPropertyDescriptor<TModel>.CreateInstanceProperty(ModelContext<TModel> owner) =>
            CreateInstanceProperty(owner);

        IPropertyContext IModelPropertyDescriptor.CreateInstanceProperty(IModelContext owner) =>
            CreateInstanceProperty((ModelContext<TModel>)owner);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop.Util
{
    public class ComponentModelCollection<TComponent> : IList<TComponent>, ICollection<TComponent>, IEnumerable<TComponent>, IEnumerable, IList, ICollection,
        IReadOnlyList<TComponent>, IReadOnlyCollection<TComponent>, INotifyCollectionChanged, INotifyPropertyChanged
        where TComponent : class
    {
        private readonly ObservableCollection<ComponentModelWrapper<TComponent>> _backingCollection;
        private readonly ICustomTypeDescriptor _typeDescriptor;

        public ReadOnlyCollection<PropertyDescriptorWrapper> ItemProperties { get; }

        public ComponentModelCollection(bool includeReadOnlyProperties = false)
        {
            Type type = typeof(TComponent);
            _typeDescriptor = TypeDescriptor.GetProvider(type).GetTypeDescriptor(type);
            ItemProperties = new ReadOnlyCollection<PropertyDescriptorWrapper>((includeReadOnlyProperties ?
                _typeDescriptor.GetProperties().Cast<PropertyDescriptor>().Where(pd => !pd.DesignTimeOnly) :
                _typeDescriptor.GetProperties().Cast<PropertyDescriptor>().Where(pd => !(pd.DesignTimeOnly || pd.IsReadOnly)))
                .Select(pd => (PropertyDescriptorWrapper)Activator.CreateInstance(typeof(PropertyDescriptorWrapper<>).MakeGenericType(pd.PropertyType), pd)).ToArray());
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        public TComponent this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        object IList.this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        TComponent IReadOnlyList<TComponent>.this[int index] => throw new NotImplementedException();

        public int Count => throw new NotImplementedException();

        bool IList.IsFixedSize => throw new NotImplementedException();

        bool ICollection<TComponent>.IsReadOnly => throw new NotImplementedException();

        bool IList.IsReadOnly => throw new NotImplementedException();

        bool ICollection.IsSynchronized => throw new NotImplementedException();

        object ICollection.SyncRoot => throw new NotImplementedException();

        public void Add(TComponent item)
        {
            throw new NotImplementedException();
        }

        int IList.Add(object value)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(TComponent item)
        {
            throw new NotImplementedException();
        }

        bool IList.Contains(object value)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(TComponent[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        void ICollection.CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public ComponentModelWrapper<TComponent> GetComponentModelWrapper(TComponent component)
        {
            throw new NotImplementedException();
        }

        public ComponentModelWrapper<TComponent> GetComponentModelWrapper(int index)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<TComponent> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int IndexOf(TComponent item)
        {
            throw new NotImplementedException();
        }

        int IList.IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, TComponent item)
        {
            throw new NotImplementedException();
        }

        void IList.Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public bool Remove(TComponent item)
        {
            throw new NotImplementedException();
        }

        void IList.Remove(object value)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public interface PropertyDescriptorWrapper
        {
            string Name { get; }
            string DisplayName { get; }
            string Category { get; }
            string Description { get; }
            bool IsReadOnly { get; }
            bool SupportsChangeEvents { get; }
            PropertyDescriptor Descriptor { get; }
            PropertyInstanceWrapper ToInstance(TComponent component);
            void AddValueChanged(TComponent component, EventHandler handler);
            object GetValue(TComponent component);
            IEnumerable<string> ValidateValue(object obj);
            object ConvertFrom(object obj);
            bool TryConvertFrom(object obj, out object result);
            string ToString(object obj);
            object FromString(string value);
            bool TryFromString(string value, out object result);
        }

        public class PropertyDescriptorWrapper<T> : PropertyDescriptorWrapper
        {
            private readonly PropertyDescriptor _propertyDescriptor;
            private readonly TypeConverter _converter;

            public string Name => _propertyDescriptor.Name;

            public string DisplayName { get; }

            public string Category { get; }

            public string Description => _propertyDescriptor.Description ?? "";

            public bool IsReadOnly => _propertyDescriptor.IsReadOnly;

            PropertyDescriptor PropertyDescriptorWrapper.Descriptor => _propertyDescriptor;

            internal bool SupportsChangeEvents => _propertyDescriptor.SupportsChangeEvents;

            bool PropertyDescriptorWrapper.SupportsChangeEvents => SupportsChangeEvents;

            internal PropertyDescriptorWrapper(PropertyDescriptor propertyDescriptor, Func<T, IEnumerable<string>> customValidator, Func<PropertyDescriptor, IEnumerable<ValidationAttribute>> validationAttributeFactory)
            {
                _converter = (_propertyDescriptor = propertyDescriptor ?? throw new ArgumentNullException(nameof(propertyDescriptor))).Converter;
                Category = string.IsNullOrWhiteSpace(_propertyDescriptor.Category) ? CategoryAttribute.Default.Category : _propertyDescriptor.Category;
                DisplayName = string.IsNullOrWhiteSpace(_propertyDescriptor.DisplayName) ? _propertyDescriptor.Name : _propertyDescriptor.DisplayName;
                _validator = CreateValidator<T>(customValidator, validationAttributeFactory);
            }

            public PropertyInstanceWrapper<T> ToInstance(TComponent component) => new PropertyInstanceWrapper<T>(this, component);

            PropertyInstanceWrapper PropertyDescriptorWrapper.ToInstance(TComponent component) => ToInstance(component);

            public T GetValue(TComponent component) => (T)_propertyDescriptor.GetValue(component);

            object PropertyDescriptorWrapper.GetValue(TComponent component) => GetValue(component);

            internal void AddValueChanged(TComponent component, EventHandler handler) => _propertyDescriptor.AddValueChanged(component, handler);

            void PropertyDescriptorWrapper.AddValueChanged(TComponent component, EventHandler handler) => AddValueChanged(component, handler);

            public IEnumerable<string> ValidateValue(T obj)
            {
                throw new NotImplementedException();
            }

            IEnumerable<string> PropertyDescriptorWrapper.ValidateValue(object obj) => ValidateValue(ConvertFrom(obj));

            public virtual T ConvertFrom(object obj) => (T)_converter.ConvertFrom(obj);

            object PropertyDescriptorWrapper.ConvertFrom(object obj) => ConvertFrom(obj);

            public virtual bool TryConvertFrom(object obj, out T result)
            {
                if (obj is T t)
                    result = t;
                else if (_converter.IsValid(obj))
                    result = (T)_converter.ConvertFrom(obj);
                else
                {
                    result = default;
                    return false;
                }
                return true;
            }

            bool PropertyDescriptorWrapper.TryConvertFrom(object obj, out object result)
            {
                bool r = TryConvertFrom(obj, out T t);
                result = t;
                return r;
            }

            public virtual string ToString(T obj) => _converter.ConvertToInvariantString(obj);

            string PropertyDescriptorWrapper.ToString(object obj) => ToString(ConvertFrom(obj));

            public virtual T FromString(string value) => (T)_converter.ConvertFromInvariantString(value);

            object PropertyDescriptorWrapper.FromString(string value) => FromString(value);

            public virtual bool TryFromString(string value, out T result)
            {
                if (_converter.IsValid(value))
                {
                    result = (T)_converter.ConvertFromInvariantString(value);
                    return true;
                }
                result = default;
                return false;
            }

            bool PropertyDescriptorWrapper.TryFromString(string value, out object result)
            {
                bool r = TryFromString(value, out T t);
                result = t;
                return r;
            }
        }

        public interface PropertyInstanceWrapper : INotifyPropertyChanged, INotifyDataErrorInfo, IEqualityComparer
        {
            object Value { get; }
            string Name { get; }
            string DisplayName { get; }
            string Category { get; }
            string Description { get; }
            bool IsReadOnly { get; }
            bool SupportsChangeEvents { get; }
            PropertyDescriptorWrapper Descriptor { get; }
            void PollValueChange();
        }

        public class PropertyInstanceWrapper<T> : PropertyInstanceWrapper
        {
            public static IEqualityComparer<T> EqualityComparer => EqualityComparer<T>.Default;
            private readonly TComponent _component;
            private readonly PropertyDescriptorWrapper<T> _descriptor;
            private T _lastKnownValue;
            
            public event PropertyChangedEventHandler PropertyChanged;
            public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

            internal PropertyInstanceWrapper(PropertyDescriptorWrapper<T> descriptor, TComponent component)
            {
                _component = component ?? throw new ArgumentNullException(nameof(component));
                _descriptor = descriptor ?? throw new ArgumentNullException(nameof(descriptor));
                PropertyDescriptor pd = ((PropertyDescriptorWrapper)descriptor).Descriptor;
                if (_descriptor.SupportsChangeEvents)
                    _descriptor.AddValueChanged(component, PropertyValueChanged);
                else
                    _lastKnownValue = descriptor.GetValue(component);
            }

            private void PropertyValueChanged(object sender, EventArgs e) => RaiseValueChanged();

            public T Value => _descriptor.GetValue(_component);

            object PropertyInstanceWrapper.Value => Value;

            public string Name => _descriptor.Name;

            public string DisplayName => _descriptor.DisplayName;

            public string Category => _descriptor.Category;

            public string Description => _descriptor.Description;

            public bool IsReadOnly => _descriptor.IsReadOnly;

            public bool HasErrors => throw new NotImplementedException();

            PropertyDescriptorWrapper PropertyInstanceWrapper.Descriptor => _descriptor;

            public bool SupportsChangeEvents => _descriptor.SupportsChangeEvents;

            bool IEqualityComparer.Equals(object x, object y) => (x is null) ? y is null : (!(y is null) && ((x is T a) ? (y is T b && EqualityComparer.Equals(a, b)) :
                (!(y is T) && x.Equals(y))));

            int IEqualityComparer.GetHashCode(object obj) => (obj is null) ? 0 : (obj is T t) ? EqualityComparer.GetHashCode(t) : obj.GetHashCode();

            public void PollValueChange()
            {
                if (_descriptor.SupportsChangeEvents)
                    return;
                T newValue = _descriptor.GetValue(_component);
                if (!EqualityComparer.Equals(_lastKnownValue, newValue))
                {
                    _lastKnownValue = newValue;
                    RaiseValueChanged();
                }
            }

            public IEnumerable GetErrors(string propertyName)
            {
                throw new NotImplementedException();
            }

            private void RaiseValueChanged()
            {
                throw new NotImplementedException();
            }
        }

        public class ComponentModelWrapper : ReadOnlyObservableCollection<PropertyInstanceWrapper>
        {
            public TComponent Component { get; }

            internal ComponentModelWrapper(ReadOnlyCollection<PropertyDescriptorWrapper> properties, TComponent component)
                : base(new ObservableCollection<PropertyInstanceWrapper>(properties.Select(pd => pd.ToInstance(component))))
            {
                Component = component;
            }
        }
    }
}

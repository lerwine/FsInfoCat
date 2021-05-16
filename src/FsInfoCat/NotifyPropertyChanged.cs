using FsInfoCat.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace FsInfoCat
{
    /// <summary>
    /// Base class for managing <see cref="INotifyPropertyChanged.PropertyChanged"/> events.
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged" />
    public abstract class NotifyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly IPropertyManager _propertyManager;
        private readonly Dictionary<string, object> _propertyValues = new Dictionary<string, object>();
        private readonly ISuspendableQueue<PropertyChangedEventArgs> _eventQueue = Extensions.GetSuspendableService().CreateSuspendableQueue<PropertyChangedEventArgs>();

        protected object SyncRoot => _eventQueue.SyncRoot;

        protected NotifyPropertyChanged() : this(null) { }

        protected NotifyPropertyChanged(IPropertyValueInitializer valueInitializer)
        {
            _eventQueue.CollectionChanged += EventQueue_CollectionChanged;
            _propertyManager = (IPropertyManager)typeof(PropertyManager<>).MakeGenericType(GetType())
                .GetField(nameof(PropertyManager<NotifyPropertyChanged>.Instance)).GetValue(null);
            if (valueInitializer is null)
                return;
            foreach (string propertyName in _propertyManager.GetPropertyNames(true))
            {
                if (valueInitializer.ContainsPropertyValue(propertyName))
                    _propertyValues.Add(propertyName, _propertyManager.VerifyPropertyValue(propertyName, valueInitializer.GetPropertyValue(propertyName)));
            }
        }

        protected ISuspension SuspendPropertyChangedEvents() => _eventQueue.Suspend();

        protected ISuspension SuspendPropertyChangedEvents(bool noThreadLock) => _eventQueue.Suspend(noThreadLock);

        private void EventQueue_CollectionChanged(object sender, NotifyCollectionChangedEventArgs<PropertyChangedEventArgs> e)
        {
            if (_eventQueue.HasItems)
                foreach (PropertyChangedEventArgs arg in _eventQueue.DequeueAll().GroupBy(a => a.PropertyName).Select(g => g.First()))
                    OnPropertyChanged(arg);
        }

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null) =>
            _eventQueue.Enqueue(new PropertyChangedEventArgs(string.IsNullOrWhiteSpace(propertyName) ? throw new ArgumentOutOfRangeException(nameof(propertyName)) :
                propertyName));

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args) => PropertyChanged?.Invoke(this, args);

        protected T GetValue<T>([CallerMemberName] string propertyName = null)
        {
            if (!_propertyManager.TryGetPropertyValueHandler<T>(propertyName, out IPropertyValueHandler<T> propertyValueHandler))
                throw new ArgumentOutOfRangeException(nameof(propertyName));

            using (ISuspension suspension = SuspendPropertyChangedEvents())
            {
                if (_propertyValues.ContainsKey(propertyName))
                    return (T)_propertyValues[propertyName];

                T value = propertyValueHandler.GetDefaultValue();
                _propertyValues.Add(propertyName, value);
                return value;
            }
        }

        protected bool SetValue<T>(T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!_propertyManager.TryGetPropertyValueHandler<T>(propertyName, out IPropertyValueHandler<T> propertyValueHandler))
                throw new ArgumentOutOfRangeException(nameof(propertyName));

            T oldValue;
            using (ISuspension suspension = SuspendPropertyChangedEvents())
            {
                oldValue = GetValue<T>(propertyName);
                if (!propertyValueHandler.CheckChanged(oldValue, newValue, out T normalizedValue))
                    return false;
                _propertyValues[propertyName] = normalizedValue;
                RaisePropertyChanged(propertyName);
            }
            return true;
        }

        protected bool SetValue<T>(T newValue, Action<T, T> onChange, [CallerMemberName] string propertyName = null)
        {
            if (!_propertyManager.TryGetPropertyValueHandler<T>(propertyName, out IPropertyValueHandler<T> propertyValueHandler))
                throw new ArgumentOutOfRangeException(nameof(propertyName));

            using (ISuspension suspension = SuspendPropertyChangedEvents())
            {
                T oldValue = GetValue<T>(propertyName);
                if (!propertyValueHandler.CheckChanged(oldValue, newValue, out T normalizedValue))
                    return false;
                _propertyValues[propertyName] = normalizedValue;
                try { RaisePropertyChanged(propertyName); }
                finally { onChange?.Invoke(oldValue, normalizedValue); }
            }
            return true;
        }

        protected bool ApplyValue<T>(T oldValue, T newValue, Action<T> mutator, [CallerMemberName] string propertyName = null)
        {
            if (!_propertyManager.TryGetPropertyValueHandler<T>(propertyName, out IPropertyValueHandler<T> propertyValueHandler))
                throw new ArgumentOutOfRangeException(nameof(propertyName));
            using (ISuspension suspension = SuspendPropertyChangedEvents())
            {
                if (!propertyValueHandler.CheckChanged(oldValue, newValue, out T normalizedValue))
                    return false;
                mutator(normalizedValue);
                RaisePropertyChanged(propertyName);
            }
            return true;
        }

        protected bool ApplyValue<T>(T oldValue, T newValue, Action<T> mutator, Action<T, T> onChange, [CallerMemberName] string propertyName = null)
        {
            if (!_propertyManager.TryGetPropertyValueHandler<T>(propertyName, out IPropertyValueHandler<T> propertyValueHandler))
                throw new ArgumentOutOfRangeException(nameof(propertyName));
            using (ISuspension suspension = SuspendPropertyChangedEvents())
            {
                if (!propertyValueHandler.CheckChanged(oldValue, newValue, out T normalizedValue))
                    return false;
                mutator(normalizedValue);
                try { RaisePropertyChanged(propertyName); }
                finally { onChange?.Invoke(oldValue, normalizedValue); }
            }
            return true;
        }

        public interface IPropertyValueInitializer
        {
            object GetPropertyValue(string name);
            bool ContainsPropertyValue(string name);
        }

        public interface IPropertyManager
        {
            IEnumerable<string> GetPropertyNames(bool writableOnly);
            bool TryGetPropertyValueHandler<T>(string propertyName, out IPropertyValueHandler<T> result);
            object VerifyPropertyValue(string propertyName, object v);
        }

        public interface IPropertyValueHandler
        {
            bool IsReadOnly { get; }
            object GetDefaultValue();
            object VerifyPropertyValue(object value);
            ICoersion Coersion { get; }
            IEqualityComparer EqualityComparer { get; }
            TypeConverter Converter { get; }
            NormalizationAttribute Normalization { get; }
        }

        public interface IPropertyValueHandler<T> : IPropertyValueHandler
        {
            new T GetDefaultValue();
            bool CheckChanged(T oldValue, T newValue, out T normalizedValue);
            new T VerifyPropertyValue(object value);
            new ICoersion<T> Coersion { get; }
            new IEqualityComparer<T> EqualityComparer { get; }
        }

        public class PropertyManager<TComponent> : IPropertyManager
        {
            internal static readonly ReadOnlyDictionary<string, IPropertyValueHandler> Properties;
            public static PropertyManager<TComponent> Instance = new PropertyManager<TComponent>();
            static PropertyManager()
            {
                Type type = typeof(TComponent);
                Type g = typeof(PropertyValueHandler<>);
                Properties = new ReadOnlyDictionary<string, IPropertyValueHandler>(TypeDescriptor.GetProperties(type).Cast<PropertyDescriptor>().GroupBy(p => p.Name)
                    .Select(d => d.Where(p => !p.DesignTimeOnly).DefaultIfEmpty(d.First()).First()).ToDictionary(k => k.Name,
                    v => (IPropertyValueHandler)Activator.CreateInstance(g.MakeGenericType(v.PropertyType), new object[] { v })));
            }

            private PropertyManager() { } // Singleton

            public IEnumerable<string> GetPropertyNames(bool writableOnly) => writableOnly ? Properties.Where(kvp => !kvp.Value.IsReadOnly).Select(kvp => kvp.Key) :
                Properties.Keys;

            public bool TryGetPropertyValueHandler<T>(string propertyName, out IPropertyValueHandler<T> result)
            {
                if (Properties.TryGetValue(propertyName, out IPropertyValueHandler handler) && handler is IPropertyValueHandler<T> t)
                {
                    result = t;
                    return true;
                }
                result = null;
                return false;
            }

            public object VerifyPropertyValue(string propertyName, object value)
            {
                if (!Properties.TryGetValue(propertyName, out IPropertyValueHandler handler))
                    throw new ArgumentOutOfRangeException(nameof(propertyName));
                return handler.VerifyPropertyValue(value);
            }
        }

        private class PropertyValueHandler<TProperty> : IPropertyValueHandler<TProperty>
        {
            private readonly PropertyDescriptor _descriptor;

            public TypeConverter Converter { get; }

            public bool IsReadOnly { get; }

            public ICoersion<TProperty> Coersion { get; }

            public IEqualityComparer<TProperty> EqualityComparer { get; }

            public NormalizationAttribute Normalization { get; }

            ICoersion IPropertyValueHandler.Coersion => Coersion;

            IEqualityComparer IPropertyValueHandler.EqualityComparer => EqualityComparer.ToGeneralizable();

            public PropertyValueHandler(PropertyDescriptor descriptor)
            {   
                Converter = (_descriptor = descriptor).Converter;
                IsReadOnly = descriptor.IsReadOnly;
                IComparisonService comparisonService = Extensions.GetComparisonService();
                Coersion = comparisonService.GetDefaultCoersion<TProperty>();
                EqualityComparer = comparisonService.GetEqualityComparer<TProperty>(true);
                Normalization = descriptor.Attributes.OfType<NormalizationAttribute>().FirstOrDefault();
            }

            public bool CheckChanged(TProperty oldValue, TProperty newValue, out TProperty normalizedValue)
            {
                normalizedValue = (Normalization is null) ? newValue : Normalization.Normalize(this, _descriptor.Name, newValue);
                return !EqualityComparer.Equals(oldValue, normalizedValue);
            }

            public TProperty GetDefaultValue()
            {
                DefaultValueAttribute a = _descriptor.Attributes.OfType<DefaultValueAttribute>().FirstOrDefault();
                TProperty value;
                if (a is null)
                    value = default;
                else
                {
                    if (!Coersion.TryCoerce(a.Value, out value))
                    {
                        if (a.Value is null)
                            value = Coersion.Cast(a.Value); // Purposely use this to throw exception.
                        else if (Converter.CanConvertFrom(a.Value.GetType()))
                            value = Coersion.Cast(Converter.ConvertFrom(a.Value));
                        else if (a.Value is string s)
                            value = Coersion.Cast(Converter.ConvertFromString(s));
                        else
                            value = Coersion.Cast(a.Value);
                    }
                }
                return (Normalization is null) ? value : Normalization.Normalize(this, _descriptor.Name, value);
            }

            object IPropertyValueHandler.GetDefaultValue() => GetDefaultValue();

            public TProperty VerifyPropertyValue(object value) => (Normalization is null) ? Coersion.Cast(value) :
                Normalization.Normalize(this, _descriptor.Name, Coersion.Cast(value));

            object IPropertyValueHandler.VerifyPropertyValue(object value) => VerifyPropertyValue(value);
        }

        public interface IPropertyHandler
        {
            TypeConverter Converter { get; }
            ICoersion Coersion { get; }
            IEqualityComparer EqualityComparer { get; }
        }

        public interface IPropertyHandler<T> : IPropertyHandler
        {
            new ICoersion<T> Coersion { get; }
            new IEqualityComparer<T> EqualityComparer { get; }
        }

        public abstract class PropertyHandlerBase<T> : IPropertyHandler<T>
        {
            private readonly IEqualityComparer _genericEqualityComparer;

            public TypeConverter Converter { get; }
            public abstract ICoersion<T> Coersion { get; }
            public abstract IEqualityComparer<T> EqualityComparer { get; }
            ICoersion IPropertyHandler.Coersion => Coersion;
            IEqualityComparer IPropertyHandler.EqualityComparer => _genericEqualityComparer;

            protected PropertyHandlerBase(PropertyDescriptor propertyDescriptor)
            {
                if (propertyDescriptor is null)
                    throw new ArgumentNullException(nameof(propertyDescriptor));
                if (!(typeof(ICoersion<>).MakeGenericType(propertyDescriptor.PropertyType).Equals(typeof(ICoersion<T>))))
                    throw new ArgumentOutOfRangeException(nameof(propertyDescriptor));
                _genericEqualityComparer = (EqualityComparer ?? throw new InvalidOperationException($"{nameof(EqualityComparer)} returned null.")).ToGeneralizable();
                Converter = propertyDescriptor.Converter;
            }

            protected PropertyHandlerBase()
            {
                _genericEqualityComparer = (EqualityComparer ?? throw new InvalidOperationException($"{nameof(EqualityComparer)} returned null.")).ToGeneralizable();
                Converter = TypeDescriptor.GetConverter(typeof(T));
            }
        }

        public class PropertyHandler<T> : PropertyHandlerBase<T>
        {
            private readonly ICoersion<T> _coersion;
            private readonly IEqualityComparer<T> _equalityComparer;

            public override ICoersion<T> Coersion => _coersion;

            public override IEqualityComparer<T> EqualityComparer => _equalityComparer;

            public PropertyHandler(PropertyDescriptor propertyDescriptor) : base(propertyDescriptor)
            {
                IComparisonService comparisonService = Extensions.GetComparisonService();
                _coersion = comparisonService.GetDefaultCoersion<T>();
                _equalityComparer = comparisonService.GetEqualityComparer<T>();
            }
        }
    }
}

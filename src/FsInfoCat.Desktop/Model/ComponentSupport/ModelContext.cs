using FsInfoCat.Desktop.Util;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace FsInfoCat.Desktop.Model.ComponentSupport
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TInstance">The type of the instance.</typeparam>
    /// <seealso cref="ModelContextBase{TInstance, IPropertyContext{TInstance}}" />
    /// <seealso cref="IModelContext{TInstance}" />
    public sealed class ModelContext<TInstance> : ModelContextBase<TInstance, IPropertyContext<TInstance>>, IModelContext<TInstance>
        where TInstance : class
    {
        IModelDescriptor IModelContext.ModelDescriptor => ModelDescriptor;

        IReadOnlyList<IPropertyContext<TInstance>> IModelContext<TInstance>.Properties => Properties;

        IReadOnlyList<IPropertyContext> IModelContext.Properties => Properties;

        IReadOnlyList<IModelProperty> IModelInfo.Properties => Properties;

        IContainer ITypeDescriptorContext.Container => null;

        object ITypeDescriptorContext.Instance => Instance;

        PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor => null;

        IPropertyContext IReadOnlyDictionary<string, IPropertyContext>.this[string key] => this[key];

        int IReadOnlyCollection<KeyValuePair<string, IPropertyContext>>.Count => Properties.Count;

        int IReadOnlyCollection<KeyValuePair<string, IPropertyContext<TInstance>>>.Count => Properties.Count;

        IEnumerable<IPropertyContext<TInstance>> IReadOnlyDictionary<string, IPropertyContext<TInstance>>.Values => Properties;

        IEnumerable<IPropertyContext> IReadOnlyDictionary<string, IPropertyContext>.Values => Properties;

        public ModelContext(ModelDescriptor<TInstance> modelDescriptor, TInstance instance)
            : base(modelDescriptor, instance,
                  (owner, pd) => Descriptors.CreatePropertyContext((ModelContext<TInstance>)owner, instance, pd))
        {
        }

        bool ITypeDescriptorContext.OnComponentChanging() => false;

        void ITypeDescriptorContext.OnComponentChanged() { }

        public override string ToString()
        {
            if (Properties.Count == 0)
                return $@"{nameof(ModelContext<TInstance>)}<{SimpleName}> {{ }}";
            return $@"{nameof(ModelContext<TInstance>)}<{SimpleName}> {{\n\t{
                string.Join(",\n\t", Properties.Select(pd => pd.Name + " = " + TypeHelper.ToPseudoCsText(pd.Value)))
            }\n}}";
        }

        bool IReadOnlyDictionary<string, IPropertyContext>.TryGetValue(string key, out IPropertyContext value)
        {
            if (TryGetValue(key, out IPropertyContext<TInstance> result))
            {
                value = result;
                return true;
            }
            value = null;
            return false;
        }

        IEnumerator<KeyValuePair<string, IPropertyContext<TInstance>>> IEnumerable<KeyValuePair<string, IPropertyContext<TInstance>>>.GetEnumerator()
        {
            return Properties.Select(p => new KeyValuePair<string, IPropertyContext<TInstance>>(p.Name, p)).GetEnumerator();
        }

        IEnumerator<KeyValuePair<string, IPropertyContext>> IEnumerable<KeyValuePair<string, IPropertyContext>>.GetEnumerator()
        {
            return Properties.Select(p => new KeyValuePair<string, IPropertyContext>(p.Name, p)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Properties.Select(p => new KeyValuePair<string, IPropertyContext<TInstance>>(p.Name, p))).GetEnumerator();
    }

}

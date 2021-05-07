using FsInfoCat.Desktop.Util;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace FsInfoCat.Desktop.Model.ComponentSupport
{
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

        public ModelContext(ModelDescriptor<TInstance> modelDescriptor, TInstance instance) : base(modelDescriptor, instance)
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
    }

}

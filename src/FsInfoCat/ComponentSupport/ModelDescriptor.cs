using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FsInfoCat.ComponentSupport
{
    internal sealed class ModelDescriptor<TModel> : IModelTypeDescriptor<TModel>
        where TModel : class
    {
        public Type ModelType { get; }

        public ReadOnlyCollection<ModelPropertyDescriptor<TModel>> Properties { get; }

        IReadOnlyList<IModelPropertyDescriptor<TModel>> IModelTypeDescriptor<TModel>.Properties => Properties;

        IReadOnlyList<IModelProperty<TModel>> IModelDescriptor<TModel>.Properties => Properties;

        IReadOnlyList<IModelPropertyDescriptor> IModelTypeDescriptor.Properties => Properties;

        IReadOnlyList<IModelProperty> IModelDescriptor.Properties => Properties;

        internal ModelDescriptor(ModelDescriptorBuilder<TModel> builder)
        {
            ModelType = builder.ModelType;
            Properties = new ReadOnlyCollection<ModelPropertyDescriptor<TModel>>(builder.BuildProperties(this));
        }

        internal ModelContext<TModel> CreateContext(TModel model) => new ModelContext<TModel>(this, model);

        IModelContext<TModel> IModelTypeDescriptor<TModel>.CreateContext(TModel model) => CreateContext(model);

        IModelContext IModelTypeDescriptor.CreateContext(object model) => CreateContext((TModel)model);
    }
}

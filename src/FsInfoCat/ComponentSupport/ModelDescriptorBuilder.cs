using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.ComponentSupport
{
    public sealed class ModelDescriptorBuilder<TModel> where TModel : class
    {
        public Type ModelType { get; }

        public ModelDescriptorBuilder()
        {
            ModelType = typeof(TModel);
        }

        public IModelTypeDescriptor<TModel> Build()
        {
            throw new NotImplementedException();
        }

        internal IList<ModelPropertyDescriptor<TModel>> BuildProperties()
        {
            throw new NotImplementedException();
        }

        internal class PropertyBuilder<TValue>
        {
            internal PropertyDescriptor Descriptor { get; }
            internal ModelDescriptor<TModel> Owner { get; }

            internal IList<ValidationAttribute> GetValidationAttributes()
            {
                throw new NotImplementedException();
            }
        }
    }
}

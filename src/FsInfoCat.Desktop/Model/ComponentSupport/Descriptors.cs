using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop.Model.ComponentSupport
{
    public static class Descriptors
    {
        public static IModelDescriptor CreateModelDescriptor(Type type, Func<PropertyDescriptor, bool> filter = null) =>
            (IModelDescriptor)typeof(ModelDescriptor<>).MakeGenericType(type).GetMethod(nameof(ModelDescriptor<object>.Create),
                new Type[] { typeof(Func<PropertyDescriptor, bool>), typeof(Func<,,>) }).Invoke(null, new object[] { filter, null });

    }
}

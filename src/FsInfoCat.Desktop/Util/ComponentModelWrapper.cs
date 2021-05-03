using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop.Util
{
    public class ComponentModelWrapper<TComponent> : ReadOnlyCollection<ComponentModelPropertyWrapper>, INotifyPropertyChanged
    {
        private readonly object _component;
        private readonly ICustomTypeDescriptor _typeDescriptor;

        public event PropertyChangedEventHandler PropertyChanged;

        public ComponentModelWrapper(object component, bool includeReadOnlyProperties = false) : base(new Collection<ComponentModelPropertyWrapper>())
        {
            if (component is null)
                throw new ArgumentNullException(nameof(component));
            _component = component;
            _typeDescriptor = TypeDescriptor.GetProvider(component).GetTypeDescriptor(component);
        }
    }
}

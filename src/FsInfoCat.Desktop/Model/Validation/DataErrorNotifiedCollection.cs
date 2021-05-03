using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace FsInfoCat.Desktop.Model.Validation
{
    public class DataErrorNotifiedCollection<TComponent, TWrapper> : PropertyChangeNotifiedCollection<TComponent, TWrapper>
        where TComponent : class
        where TWrapper : INotifyComponentPropertyChangeRelay<TComponent>
    {
        private ObservableCollection<INotifyComponentDataErrorInfoRelay<TComponent>> _backingCollection;
    }
}

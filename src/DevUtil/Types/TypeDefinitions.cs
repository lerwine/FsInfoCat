using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Xml.Serialization;

namespace DevUtil.Types
{
    [XmlRoot(nameof(TypeDefinitions))]
    public class TypeDefinitions : NamespaceType.Owner, INotifyDataErrorInfo
    {
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        [XmlIgnore]
        public bool HasErrors => throw new NotImplementedException();

        public IEnumerable GetErrors(string propertyName)
        {
            throw new NotImplementedException();
        }
    }
}

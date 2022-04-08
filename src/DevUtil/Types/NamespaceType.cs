using System;
using System.Collections;
using System.ComponentModel;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;

namespace DevUtil.Types
{
    [XmlRoot(RootElementName)]
    public partial class NamespaceType : DefinitionType.Owner, INotifyDataErrorInfo
    {
        private Owner _owner;
        private string _name;
        public const string RootElementName = "Namespace";

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        [XmlAttribute(AttributeName = nameof(Name))]
        public string Name
        {
            get => _name;
            set
            {
                string oldValue;
                Monitor.Enter(SyncRoot);
                try
                {
                    if (string.IsNullOrEmpty(value) ? (oldValue = _name).Length == 0 : (oldValue = _name) == value)
                        return;
                    RaisePropertyChanging(oldValue, value);
                    _name = value;
                    // TODO: Validate name
                    RaisePropertyChanged(oldValue, value);
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        [XmlIgnore]
        public bool HasErrors => throw new NotImplementedException();

        public IEnumerable GetErrors(string propertyName)
        {
            throw new NotImplementedException();
        }
    }
}

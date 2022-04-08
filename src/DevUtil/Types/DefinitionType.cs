using System.Threading;
using System.Xml;
using System.Xml.Serialization;

namespace DevUtil.Types
{
    public abstract partial class DefinitionType : NotifyPropertyChange
    {
        private Owner _owner;
        private string _name;

        protected object SyncRoot { get; } = new object();

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
    }
}

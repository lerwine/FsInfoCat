using System.Xml.Serialization;

namespace FsInfoCat.Test.FileUriConverterTestHelpers
{
    public abstract class HostInfo<TOwner> : IRegexMatch, IOwnable<TOwner>
        where TOwner : ISynchronized
    {
        internal object SyncRoot => _syncRoot;
        private readonly object _syncRoot = new object();
        object ISynchronized.SyncRoot => _syncRoot;
        private TOwner _owner;
        internal TOwner Owner
        {
            get => _owner;
            set
            {
                lock (_syncRoot)
                    _owner = value;
            }
        }
        TOwner IOwnable<TOwner>.Owner => _owner;
        ISynchronized IOwnable.Owner => _owner;

        private string _match = "";
        [XmlAttribute]
        public string Match
        {
            get => _match;
            set => _match = value ?? "";
        }

        private string _value = "";
        [XmlText]
        public string Value
        {
            get => _value;
            set => _value = value ?? "";
        }

        protected bool Equals(HostInfo<TOwner> other) => _value.Equals(other._value) && _match.Equals(other._match);
        public abstract string GetXPath();
    }
}

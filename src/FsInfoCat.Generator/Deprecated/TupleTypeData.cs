using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using System.Threading;

namespace FsInfoCat.Generator
{
    [System.Obsolete("Soon to be removed")]
    [XmlRoot(RootElementName)]
    public class TupleTypeData : TypeData
    {
        public const string RootElementName = "Tuple";

        private readonly object _syncRoot = new object();
        private Collection<TupleElement> _elements;

        public Collection<TupleElement> Elements
        {
            get
            {
                Monitor.Enter(_syncRoot);
                try
                {
                    if (_elements == null) _elements = new Collection<TupleElement>();
                    return _elements;
                }
                finally { Monitor.Exit(_syncRoot); }
            }
            set
            {
                Monitor.Enter(_syncRoot);
                try { _elements = value; }
                finally { Monitor.Exit(_syncRoot); }
            }
        }

        public TupleTypeData() { }

        public TupleTypeData(TupleTypeSyntax syntax) : base(syntax)
        {
            Collection<TupleElement> elements = new Collection<TupleElement>();
            foreach (TupleElementSyntax e in syntax.Elements)
                elements.Add(new TupleElement(e));
            _elements = elements;
        }
    }
}

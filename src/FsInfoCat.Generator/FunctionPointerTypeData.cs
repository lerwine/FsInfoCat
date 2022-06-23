using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using System.Threading;

namespace FsInfoCat.Generator
{
    [XmlRoot(RootElementName)]
    public class FunctionPointerTypeData : TypeData
    {
        public const string RootElementName = "FunctionPointer";

        private readonly object _syncRoot = new object();
        private Collection<TypeData> _parameters;

        public Collection<TypeData> Parameters
        {
            get
            {
                Monitor.Enter(_syncRoot);
                try
                {
                    if (_parameters == null) _parameters = new Collection<TypeData>();
                    return _parameters;
                }
                finally { Monitor.Exit(_syncRoot); }
            }
            set
            {
                Monitor.Enter(_syncRoot);
                try { _parameters = value; }
                finally { Monitor.Exit(_syncRoot); }
            }
        }

        public FunctionPointerTypeData() { }

        public FunctionPointerTypeData(FunctionPointerTypeSyntax syntax) : base(syntax)
        {
            Collection<TypeData> parameters = new Collection<TypeData>();
            foreach (FunctionPointerParameterSyntax p in syntax.ParameterList.Parameters)
                parameters.Add(TypeData.CreateTypeData(p.Type));
            _parameters = parameters;
        }
    }
}

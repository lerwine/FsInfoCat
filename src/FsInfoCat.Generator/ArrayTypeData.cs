using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using System.Threading;

namespace FsInfoCat.Generator
{
    [XmlRoot(RootElementName)]
    public class ArrayTypeData : TypeData
    {
        public const string RootElementName = "Array";

        private readonly object _syncRoot = new object();
        private Collection<int> _rankSpecifiers;

        public Collection<int> RankSpecifiers
        {
            get
            {
                Monitor.Enter(_syncRoot);
                try
                {
                    if (_rankSpecifiers == null) _rankSpecifiers = new Collection<int>();
                    return _rankSpecifiers;
                }
                finally { Monitor.Exit(_syncRoot); }
            }
            set
            {
                Monitor.Enter(_syncRoot);
                try { _rankSpecifiers = value; }
                finally { Monitor.Exit(_syncRoot); }
            }
        }

        public TypeData ElementType { get; set; }

        public ArrayTypeData() { }

        public ArrayTypeData(ArrayTypeSyntax syntax) : base(syntax)
        {
            ElementType = CreateTypeData(syntax.ElementType);
            Collection<int> rankSpecifiers = new Collection<int>();
            foreach (ArrayRankSpecifierSyntax r in syntax.RankSpecifiers)
                rankSpecifiers.Add(r.Rank);
            _rankSpecifiers = rankSpecifiers;
        }
    }
}

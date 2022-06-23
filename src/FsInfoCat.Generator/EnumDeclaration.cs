using System.Collections.ObjectModel;
using System.Threading;
using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    [XmlRoot(RootElementName)]
    public class EnumDeclaration : BaseTypeDeclaration
    {
        public const string RootElementName = "Enum";

        private readonly object _syncRoot = new object();
        private Collection<EnumMemberDeclaration> _members;

        [XmlAttribute()]
        public string Name { get; set; }
        public Collection<EnumMemberDeclaration> Members
        {
            get
            {
                Monitor.Enter(_syncRoot);
                try
                {
                    if (_members == null) _members = new Collection<EnumMemberDeclaration>();
                    return _members;
                }
                finally { Monitor.Exit(_syncRoot); }
            }
            set
            {
                Monitor.Enter(_syncRoot);
                try { _members = value; }
                finally { Monitor.Exit(_syncRoot); }
            }
        }

        public EnumDeclaration() { }

        public EnumDeclaration(EnumDeclarationSyntax syntax) : base(syntax)
        {
            Name = syntax.Identifier.ValueText;
            Collection<EnumMemberDeclaration> members = new Collection<EnumMemberDeclaration>();
            foreach (EnumMemberDeclarationSyntax memberDeclaration in syntax.Members)
                members.Add(new EnumMemberDeclaration(memberDeclaration));
            _members = members;
        }
    }
}

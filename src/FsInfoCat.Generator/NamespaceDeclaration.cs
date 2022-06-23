using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    [XmlRoot(RootElementName)]
    public class NamespaceDeclaration : MemberDeclaration
    {
        public const string RootElementName = "Namespace";

        private readonly object _syncRoot = new object();
        private Collection<UsingDirective> _usings;
        private Collection<MemberDeclaration> _members;

        [XmlAttribute()]
        public string Name { get; set; }

        public Collection<UsingDirective> Usings
        {
            get
            {
                Monitor.Enter(_syncRoot);
                try
                {
                    if (_usings == null) _usings = new Collection<UsingDirective>();
                    return _usings;
                }
                finally { Monitor.Exit(_syncRoot); }
            }
            set
            {
                Monitor.Enter(_syncRoot);
                try { _usings = value; }
                finally { Monitor.Exit(_syncRoot); }
            }
        }

        [XmlArrayItem(NamespaceDeclaration.RootElementName, typeof(NamespaceDeclaration))]
        [XmlArrayItem(UsingDirective.RootElementName, typeof(UsingDirective))]
        [XmlArrayItem(ClassDeclaration.RootElementName, typeof(ClassDeclaration))]
        [XmlArrayItem(RecordDeclaration.RootElementName, typeof(RecordDeclaration))]
        [XmlArrayItem(StructDeclaration.RootElementName, typeof(StructDeclaration))]
        [XmlArrayItem(InterfaceDeclaration.RootElementName, typeof(InterfaceDeclaration))]
        [XmlArrayItem(EnumDeclaration.RootElementName, typeof(EnumDeclaration))]
        [XmlArrayItem(DelegateDeclaration.RootElementName, typeof(DelegateDeclaration))]
        [XmlArrayItem(PropertyDeclaration.RootElementName, typeof(PropertyDeclaration))]
        [XmlArrayItem(EventPropertyDeclaration.RootElementName, typeof(EventPropertyDeclaration))]
        [XmlArrayItem(EventFieldDeclaration.RootElementName, typeof(EventFieldDeclaration))]
        [XmlArrayItem(MethodDeclaration.RootElementName, typeof(MethodDeclaration))]
        [XmlArrayItem(ConstructorDeclaration.RootElementName, typeof(ConstructorDeclaration))]
        [XmlArrayItem(DestructorDeclaration.RootElementName, typeof(DestructorDeclaration))]
        [XmlArrayItem(OperatorDeclaration.RootElementName, typeof(OperatorDeclaration))]
        [XmlArrayItem(ConversionOperatorDeclaration.RootElementName, typeof(ConversionOperatorDeclaration))]
        [XmlArrayItem(FieldDeclaration.RootElementName, typeof(FieldDeclaration))]
        [XmlArrayItem(UnsupportedMember.RootElementName, typeof(UnsupportedMember))]
        public Collection<MemberDeclaration> Members
        {
            get
            {
                Monitor.Enter(_syncRoot);
                try
                {
                    if (_members == null) _members = new Collection<MemberDeclaration>();
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

        public NamespaceDeclaration() { }

        public NamespaceDeclaration(BaseNamespaceDeclarationSyntax syntax) : base(syntax)
        {
            Name = syntax.Name.GetText().ToString();
            Collection<MemberDeclaration> members = new Collection<MemberDeclaration>();
            Collection<UsingDirective> usings = new Collection<UsingDirective>();
            Members = new Collection<MemberDeclaration>();
            foreach (UsingDirectiveSyntax usingDirective in syntax.Usings)
                usings.Add(new UsingDirective(usingDirective));
            foreach (MemberDeclarationSyntax memberDeclaration in syntax.Members)
                members.Add(CreateMemberDeclaration(memberDeclaration));
            Usings = usings;
            Members = members;
        }
    }
}

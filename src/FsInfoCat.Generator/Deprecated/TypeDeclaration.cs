using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    [Obsolete("Soon to be removed")]
    public abstract class TypeDeclaration : BaseTypeDeclaration
    {
        private readonly object _syncRoot = new object();
        private Collection<MemberDeclaration> _members;

        [XmlAttribute()]
        public string Name { get; set; }

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
        [XmlArrayItem(EnumMemberDeclaration.RootElementName, typeof(EnumMemberDeclaration))]
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

        protected TypeDeclaration() { }

        protected TypeDeclaration(TypeDeclarationSyntax syntax) : base(syntax)
        {
            Name = syntax.Identifier.ValueText;
            Collection<MemberDeclaration> members = new Collection<MemberDeclaration>();
            foreach (MemberDeclarationSyntax memberDeclaration in syntax.Members)
                members.Add(CreateMemberDeclaration(memberDeclaration));
            _members = members;
        }

        public static TypeDeclaration CreateTypeDeclaration(TypeDeclarationSyntax syntax)
        {
            if (syntax is ClassDeclarationSyntax classDeclaration) return new ClassDeclaration(classDeclaration);
            if (syntax is RecordDeclarationSyntax recordDeclarationSyntax) return new RecordDeclaration(recordDeclarationSyntax);
            if (syntax is StructDeclarationSyntax structDeclaration) return new StructDeclaration(structDeclaration);
            if (syntax is InterfaceDeclarationSyntax interfaceDeclaration) return new InterfaceDeclaration(interfaceDeclaration);
            return new UnsupportedTypeDeclaration(syntax);
        }
    }
}

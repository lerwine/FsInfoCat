using System;
using System.Threading;
using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    [XmlRoot(RootElementName)]
    public class NamespaceDeclaration : MemberDeclaration, IModelParent
    {
        public const string RootElementName = "Namespace";

        private readonly object _syncRoot = new object();

        private ModelCollection _components;

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
        [XmlArrayItem(UnsupportedMember.RootElementName, typeof(UnsupportedMember))]
        public ModelCollection Components
        {
            get
            {
                Monitor.Enter(_syncRoot);
                try
                {
                    if (_components is null) _components = new ModelCollection(this);
                    return _components;
                }
                finally { Monitor.Exit(_syncRoot); }
            }
        }

        protected internal override string GetName() => Name;

        public NamespaceDeclaration() { }

        public NamespaceDeclaration(BaseNamespaceDeclarationSyntax syntax)
        {
            Name = syntax.Name.GetText().ToString();
            foreach (UsingDirectiveSyntax usingDirective in syntax.Usings)
                Components.Add(UsingDirective.Create(usingDirective));
            foreach (MemberDeclarationSyntax memberDeclaration in syntax.Members)
                Components.Add(CreateMemberDeclaration(memberDeclaration));
        }
    }
}

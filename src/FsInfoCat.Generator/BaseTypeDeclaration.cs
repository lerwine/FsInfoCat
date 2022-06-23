using System.Linq;
using System.Threading;
using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    public abstract class BaseTypeDeclaration : MemberDeclaration, IModelParent
    {
        private readonly object _syncRoot = new object();
        private ModelCollection _components;

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

        [XmlArrayItem(ArrayTypeData.RootElementName, typeof(ArrayTypeData))]
        [XmlArrayItem(FunctionPointerTypeData.RootElementName, typeof(FunctionPointerTypeData))]
        [XmlArrayItem(OmittedTypeArgument.RootElementName, typeof(OmittedTypeArgument))]
        [XmlArrayItem(PointerTypeData.RootElementName, typeof(PointerTypeData))]
        [XmlArrayItem(PredefinedTypeData.RootElementName, typeof(PredefinedTypeData))]
        [XmlArrayItem(RefTypeData.RootElementName, typeof(RefTypeData))]
        [XmlArrayItem(TupleTypeData.RootElementName, typeof(TupleTypeData))]
        [XmlArrayItem(AliasQualifiedTypeName.RootElementName, typeof(AliasQualifiedTypeName))]
        [XmlArrayItem(QualifiedTypeName.RootElementName, typeof(QualifiedTypeName))]
        [XmlArrayItem(GenericTypeName.RootElementName, typeof(GenericTypeName))]
        [XmlArrayItem(IdentifierName.RootElementName, typeof(IdentifierName))]
        [XmlArrayItem(UnsupportedType.RootElementName, typeof(UnsupportedType))]
        public TypeData[] BaseTypes { get; internal set; }

        protected BaseTypeDeclaration() { }

        protected BaseTypeDeclaration(BaseTypeDeclarationSyntax baseTypeDeclaration)
        {
            BaseTypes = TypeData.CreateFromBaseList(baseTypeDeclaration.BaseList).ToArray();
        }

        public static BaseTypeDeclaration CreateBaseTypeDeclaration(BaseTypeDeclarationSyntax syntax)
        {
            if (syntax is EnumDeclarationSyntax enumDeclarationSyntax) return new EnumDeclaration(enumDeclarationSyntax);
            if (syntax is TypeDeclarationSyntax typeDeclaration) return TypeDeclaration.CreateTypeDeclaration(typeDeclaration);
            return new UnsupportedBaseTypeDeclaration(syntax);
        }
    }
}

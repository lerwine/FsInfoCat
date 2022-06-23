using System.Collections.ObjectModel;
using System.Threading;
using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    public abstract class BaseTypeDeclaration : MemberDeclaration
    {
        private readonly object _syncRoot = new object();
        private Collection<BaseTypeData> _baseTypes;

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
        public Collection<BaseTypeData> BaseTypes
        {
            get
            {
                Monitor.Enter(_syncRoot);
                try
                {
                    if (_baseTypes == null) _baseTypes = new Collection<BaseTypeData>();
                    return _baseTypes;
                }
                finally { Monitor.Exit(_syncRoot); }
            }
            set
            {
                Monitor.Enter(_syncRoot);
                try { _baseTypes = value; }
                finally { Monitor.Exit(_syncRoot); }
            }
        }

        protected BaseTypeDeclaration() { }

        protected BaseTypeDeclaration(BaseTypeDeclarationSyntax baseTypeDeclaration) : base(baseTypeDeclaration)
        {
            Collection<BaseTypeData> baseTypes = new Collection<BaseTypeData>();
            foreach (BaseTypeSyntax type in baseTypeDeclaration.BaseList.Types)
                baseTypes.Add(BaseTypeData.CreateBaseTypeData(type));
            _baseTypes = baseTypes;
        }

        public static BaseTypeDeclaration CreateBaseTypeDeclaration(BaseTypeDeclarationSyntax syntax)
        {
            if (syntax is EnumDeclarationSyntax enumDeclarationSyntax) return new EnumDeclaration(enumDeclarationSyntax);
            if (syntax is TypeDeclarationSyntax typeDeclaration) return TypeDeclaration.CreateTypeDeclaration(typeDeclaration);
            return new UnsupportedBaseTypeDeclaration(syntax);
        }
    }
}

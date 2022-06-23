using System.Xml.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    public abstract class TypeDeclaration : BaseTypeDeclaration
    {
        [XmlAttribute()]
        public string Name { get; set; }

        protected TypeDeclaration() { }

        protected TypeDeclaration(TypeDeclarationSyntax syntax) : base(syntax)
        {
            Name = syntax.Identifier.ValueText;
            foreach (MemberDeclarationSyntax memberDeclaration in syntax.Members)
                Components.Add(CreateMemberDeclaration(memberDeclaration));
        }

        protected internal override string GetName() => Name;

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

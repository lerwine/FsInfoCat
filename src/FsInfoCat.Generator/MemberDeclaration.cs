using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    public abstract class MemberDeclaration : SyntaxEntity
    {
        protected MemberDeclaration() { }

        protected MemberDeclaration(MemberDeclarationSyntax syntax) : base(syntax)
        {
        }

        public static MemberDeclaration CreateMemberDeclaration(MemberDeclarationSyntax syntax)
        {
            if (syntax is BaseNamespaceDeclarationSyntax namespaceDeclaration) return new NamespaceDeclaration(namespaceDeclaration);
            if (syntax is BaseTypeDeclarationSyntax baseTypeDeclaration) return BaseTypeDeclaration.CreateBaseTypeDeclaration(baseTypeDeclaration);
            if (syntax is DelegateDeclarationSyntax delegateDeclaration) return new DelegateDeclaration(delegateDeclaration);
            if (syntax is PropertyDeclarationSyntax propertyDeclaration) return new PropertyDeclaration(propertyDeclaration);
            if (syntax is EventDeclarationSyntax eventDeclarationSyntax) return new EventPropertyDeclaration(eventDeclarationSyntax);
            if (syntax is EventFieldDeclarationSyntax eventFieldDeclaration) return new EventFieldDeclaration(eventFieldDeclaration);
            if (syntax is MethodDeclarationSyntax methodDeclaration) return new MethodDeclaration(methodDeclaration);
            if (syntax is ConstructorDeclarationSyntax constructorDeclaration) return new ConstructorDeclaration(constructorDeclaration);
            if (syntax is DestructorDeclarationSyntax destructorDeclaration) return new DestructorDeclaration(destructorDeclaration);
            if (syntax is OperatorDeclarationSyntax operatorDeclaration) return new OperatorDeclaration(operatorDeclaration);
            if (syntax is ConversionOperatorDeclarationSyntax conversionOperatorDeclaration) return new ConversionOperatorDeclaration(conversionOperatorDeclaration);
            if (syntax is FieldDeclarationSyntax fieldDeclarationSyntax) return new FieldDeclaration(fieldDeclarationSyntax);
            return new UnsupportedMember(syntax);
        }
    }
}

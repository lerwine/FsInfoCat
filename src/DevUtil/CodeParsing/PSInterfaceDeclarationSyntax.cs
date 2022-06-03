using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DevUtil.CodeParsing
{
    /// <summary>
    /// Wrapper class for interface declarations.
    /// </summary>
    /// <seealso href="https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.syntax.interfacedeclarationsyntax?view=roslyn-dotnet-4.1.0"/>
    public sealed class PSInterfaceDeclarationSyntax : PSTypeDeclarationSyntax
    {
        internal InterfaceDeclarationSyntax Syntax { get; }

        internal override CSharpSyntaxNode BaseSyntax => Syntax;

        internal override TypeDeclarationSyntax TypeSyntax => Syntax;

        internal override BaseTypeDeclarationSyntax BaseTypeSyntax => Syntax;

        internal override MemberDeclarationSyntax BaseMemberSyntax => Syntax;

        internal PSInterfaceDeclarationSyntax([DisallowNull] InterfaceDeclarationSyntax syntax, [DisallowNull] PSSyntaxBase parent)
            : base(parent) => Syntax = syntax;
    }
}

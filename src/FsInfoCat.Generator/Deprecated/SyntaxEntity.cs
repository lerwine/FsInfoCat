using Microsoft.CodeAnalysis.CSharp;

namespace FsInfoCat.Generator
{
    [System.Obsolete("Soon to be removed")]
    public abstract class SyntaxEntity
    {
        public CommentDocumentation DocumentationComments { get; set; }

        public SyntaxEntity() { }

        public SyntaxEntity(CSharpSyntaxNode syntax)
        {
            DocumentationComments = CommentDocumentation.Create(syntax);
        }
    }
}

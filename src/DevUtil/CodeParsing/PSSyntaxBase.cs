using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DevUtil.CodeParsing
{
    /// <summary>
    /// Base class for syntax nodes.
    /// </summary>
    /// <seealso href="https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.csharpsyntaxnode?view=roslyn-dotnet-4.1.0"/>
    public abstract class PSSyntaxBase
    {
        private PSDocumentationComments _documentationComments;

        internal abstract CSharpSyntaxNode BaseSyntax { get; }

        public PSDocumentationComments DocumentationComments
        {
            get
            {
                Monitor.Enter(BaseSyntax);
                try
                {
                    if (_documentationComments is null)
                        _documentationComments = new(BaseSyntax.HasLeadingTrivia ? BaseSyntax.GetLeadingTrivia().OfType<DocumentationCommentTriviaSyntax>() : Enumerable.Empty<DocumentationCommentTriviaSyntax>(), this);
                }
                finally { Monitor.Exit(BaseSyntax); }
                return _documentationComments;
            }
        }

        protected IEnumerable<PSMemberDeclarationSyntax> CreateMembers(IEnumerable<MemberDeclarationSyntax> members)
        {
            if (members is null)
                yield break;
            foreach (MemberDeclarationSyntax syntax in members)
            {
                if (syntax is EventFieldDeclarationSyntax eventField)
                    yield return new PSEventFieldDeclarationSyntax(eventField, this);
                else if (syntax is FieldDeclarationSyntax field)
                    yield return new PSFieldDeclarationSyntax(field, this);
                else if (syntax is ConstructorDeclarationSyntax constructor)
                    yield return new PSConstructorDeclarationSyntax(constructor, this);
                else if (syntax is ConversionOperatorDeclarationSyntax conversionOperator)
                    yield return new PSConversionOperatorDeclarationSyntax(conversionOperator, this);
                else if (syntax is DestructorDeclarationSyntax destructor)
                    yield return new PSDestructorDeclarationSyntax(destructor, this);
                else if (syntax is MethodDeclarationSyntax method)
                    yield return new PSMethodDeclarationSyntax(method, this);
                else if (syntax is OperatorDeclarationSyntax op)
                    yield return new PSOperatorDeclarationSyntax(op, this);
                else if (syntax is FileScopedNamespaceDeclarationSyntax fsns)
                    yield return new PSFileScopedNamespaceDeclarationSyntax(fsns, this);
                else if (syntax is NamespaceDeclarationSyntax ns)
                    yield return new PSNamespaceDeclarationSyntax(ns, this);
                else if (syntax is EventDeclarationSyntax evt)
                    yield return new PSEventDeclarationSyntax(evt, this);
                else if (syntax is IndexerDeclarationSyntax indexer)
                    yield return new PSIndexerDeclarationSyntax(indexer, this);
                else if (syntax is PropertyDeclarationSyntax property)
                    yield return new PSPropertyDeclarationSyntax(property, this);
                else if (syntax is EnumDeclarationSyntax en)
                    yield return new PSEnumDeclarationSyntax(en, this);
                else if (syntax is ClassDeclarationSyntax c)
                    yield return new PSClassDeclarationSyntax(c, this);
                else if (syntax is InterfaceDeclarationSyntax i)
                    yield return new PSInterfaceDeclarationSyntax(i, this);
                else if (syntax is RecordDeclarationSyntax r)
                    yield return new PSRecordDeclarationSyntax(r, this);
                else if (syntax is StructDeclarationSyntax s)
                    yield return new PSStructDeclarationSyntax(s, this);
                else if (syntax is DelegateDeclarationSyntax d)
                    yield return new PSDelegateDeclarationSyntax(d, this);
                else if (syntax is EnumMemberDeclarationSyntax m)
                    yield return new PSEnumMemberDeclarationSyntax(m, this);
                else if (syntax is GlobalStatementSyntax g)
                    yield return new PSGlobalStatementSyntax(g, this);
                else if (syntax is IncompleteMemberSyntax incomplete)
                    yield return new PSIncompleteMemberSyntax(incomplete, this);
            }
        }
    }
}

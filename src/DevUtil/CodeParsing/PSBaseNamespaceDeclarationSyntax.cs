using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DevUtil.CodeParsing
{
    /// <summary>
    /// Base class for namespace declarations.
    /// </summary>
    /// <seealso href="https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.syntax.basenamespacedeclarationsyntax?view=roslyn-dotnet-4.1.0"/>
    public abstract class PSBaseNamespaceDeclarationSyntax : PSMemberDeclarationSyntax
    {
        private ReadOnlyCollection<PSMemberDeclarationSyntax> _members;
        private ReadOnlyCollection<PSExternAliasDirectiveSyntax> _externs;
        private ReadOnlyCollection<PSUsingDirectiveSyntax> _usings;

        internal abstract BaseNamespaceDeclarationSyntax BaseNamespaceSyntax { get; }

        public string Name => BaseNamespaceSyntax.Name.ToString();

        public ReadOnlyCollection<PSMemberDeclarationSyntax> Members
        {
            get
            {
                Monitor.Enter(BaseNamespaceSyntax);
                try
                {
                    if (_members is null)
                        _members = new(CreateMembers(BaseNamespaceSyntax.Members).ToArray());
                }
                finally { Monitor.Exit(BaseNamespaceSyntax); }
                return _members;
            }
        }

        public ReadOnlyCollection<PSExternAliasDirectiveSyntax> Externs
        {
            get
            {
                Monitor.Enter(BaseNamespaceSyntax);
                try
                {
                    if (_externs is null)
                        _externs = new(BaseNamespaceSyntax.Externs.Select(e => new PSExternAliasDirectiveSyntax(e, this)).ToArray());
                }
                finally { Monitor.Exit(BaseNamespaceSyntax); }
                return _externs;
            }
        }

        public ReadOnlyCollection<PSUsingDirectiveSyntax> Usings
        {
            get
            {
                Monitor.Enter(BaseNamespaceSyntax);
                try
                {
                    if (_usings is null)
                        _usings = new(BaseNamespaceSyntax.Usings.Select(e => new PSUsingDirectiveSyntax(e, this)).ToArray());
                }
                finally { Monitor.Exit(BaseNamespaceSyntax); }
                return _usings;
            }
        }

        protected PSBaseNamespaceDeclarationSyntax([DisallowNull] PSSyntaxBase parent) : base(parent) { }
    }
}

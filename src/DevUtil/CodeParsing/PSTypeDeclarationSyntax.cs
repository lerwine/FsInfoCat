using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DevUtil.CodeParsing
{
    /// <summary>
    /// Base class for type declarations.
    /// </summary>
    /// <seealso href="https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.syntax.typedeclarationsyntax?view=roslyn-dotnet-4.1.0"/>
    public abstract class PSTypeDeclarationSyntax : PSBaseTypeDeclarationSyntax
    {
        private ReadOnlyCollection<PSMemberDeclarationSyntax> _members;

        internal abstract TypeDeclarationSyntax TypeSyntax { get; }

        public ReadOnlyCollection<PSMemberDeclarationSyntax> Members
        {
            get
            {
                Monitor.Enter(TypeSyntax);
                try
                {
                    if (_members is null)
                        _members = new(CreateMembers(TypeSyntax.Members).ToArray());
                }
                finally { Monitor.Exit(TypeSyntax); }
                return _members;
            }
        }

        protected PSTypeDeclarationSyntax([DisallowNull] PSSyntaxBase parent) : base(parent) { }
    }
}

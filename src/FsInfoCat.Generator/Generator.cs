using Microsoft.CodeAnalysis;
using System.Linq;
using System.Collections.ObjectModel;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FsInfoCat.Generator
{
    [Generator]
    public class Generator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
        }

        // private void ProcessModelXml(string path, XElement rootElement, Dictionary<string, XElement> allModels)
        // {
        //     throw new NotImplementedException();
        // }

        public void Initialize(GeneratorInitializationContext context)
        {
            // Initialization not required.
        }
    }
}

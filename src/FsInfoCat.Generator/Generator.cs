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
            ModelData modelData = new ModelData();
            Collection<MemberDeclaration> members = new Collection<MemberDeclaration>();
            foreach (CSharpSyntaxTree syntaxTree in context.Compilation.SyntaxTrees.OfType<CSharpSyntaxTree>())
            {
                CompilationUnitSyntax compilationUnitSyntax = syntaxTree.GetCompilationUnitRoot();
                foreach (MemberDeclarationSyntax syntax in compilationUnitSyntax.Members)
                    members.Add(MemberDeclaration.CreateMemberDeclaration(syntax));
            }
            modelData.Members = members;
            // foreach (EnumDeclarationSyntax typeSyntax in syntaxReceiver.Enums)
            // {
            //     XElement typeElement = typesDoc.Root.AddXElement(ElementNames.Enum, new XAttribute("Name", typeSyntax.Identifier.ValueText));
            //     typeElement.AddDocumentationElement(typeSyntax, context);
            //     typeElement.AddBaseTypeElements(typeSyntax.BaseList);
            //     foreach (EnumMemberDeclarationSyntax memberSyntax in typeSyntax.Members)
            //     {
            //         XElement memberElement = typeElement.AddXElement(ElementNames.Field, new XAttribute("Name", memberSyntax.Identifier.ValueText));
            //         string value = memberSyntax.EqualsValue?.Value.GetText()?.ToString();
            //         if (!string.IsNullOrEmpty(value))
            //             memberElement.Add(new XAttribute("Value", value));
            //         memberElement.AddDocumentationElement(memberSyntax, context);
            //     }
            // }
            // foreach (InterfaceDeclarationSyntax typeSyntax in syntaxReceiver.Interfaces)
            // {
            //     XElement typeElement = typesDoc.Root.AddXElement(ElementNames.Interface, new XAttribute("Name", typeSyntax.Identifier.ValueText));
            //     typeElement.AddDocumentationElement(typeSyntax, context);
            //     typeElement.AddBaseTypeElements(typeSyntax.BaseList);
            //     foreach (MemberDeclarationSyntax memberSyntax in typeSyntax.Members)
            //     {
            //         if (memberSyntax is PropertyDeclarationSyntax propertySyntax)
            //         {
            //             XElement memberElement = typeElement.AddXElement(ElementNames.Property, new XAttribute("Name", propertySyntax.Identifier.ValueText));
            //             memberElement.AddDocumentationElement(propertySyntax, context);
            //             memberElement.AddTypeElement(propertySyntax.Type);
            //         }
            //         else
            //         {
            //             Location location = memberSyntax.GetLocation();
            //             context.ReportDiagnosticInfo(DiagnosticId.SkippedInfo, "Skipped interface member", "Skipped interface {0} member at line {1}", location, typeSyntax.Identifier.ValueText,
            //                 location.GetLineSpan().StartLinePosition);
            //         }
            //     }
            // }
            // foreach (StructDeclarationSyntax typeSyntax in syntaxReceiver.Structs)
            // {
            //     XElement typeElement = typesDoc.Root.AddXElement(ElementNames.Struct, new XAttribute("Name", typeSyntax.Identifier.ValueText));
            //     typeElement.AddDocumentationElement(typeSyntax, context);
            //     typeElement.AddBaseTypeElements(typeSyntax.BaseList);
            //     foreach (MemberDeclarationSyntax memberSyntax in typeSyntax.Members)
            //     {
            //         if (memberSyntax is PropertyDeclarationSyntax propertySyntax)
            //         {
            //             XElement memberElement = typeElement.AddXElement(ElementNames.Property, new XAttribute("Name", propertySyntax.Identifier.ValueText),
            //                 new XAttribute("Type", propertySyntax.Type.GetText().ToString()));
            //             memberElement.AddDocumentationElement(propertySyntax, context);
            //             memberElement.AddTypeElement(propertySyntax.Type);
            //         }
            //         else if (memberSyntax is FieldDeclarationSyntax fieldSyntax)
            //         {
            //             XElement memberElement = typeElement.AddXElement(ElementNames.Field, new XAttribute("Type", fieldSyntax.Declaration.Type.GetText().ToString()));
            //             memberElement.AddDocumentationElement(fieldSyntax, context);
            //         }
            //         else
            //         {
            //             Location location = memberSyntax.GetLocation();
            //             context.ReportDiagnosticInfo(DiagnosticId.SkippedInfo, "Skipped interface member", "Skipped interface {0} member at line {1}", location, typeSyntax.Identifier.ValueText,
            //                 location.GetLineSpan().StartLinePosition);
            //         }
            //     }
            // }

            // Dictionary<string, XElement> models = new Dictionary<string, XElement>(StringComparer.OrdinalIgnoreCase);
            // bool success = true;
            // foreach (AdditionalText modelFile in context.AdditionalFiles.Where(at => at.Path.EndsWith(".models.xml", StringComparison.OrdinalIgnoreCase)))
            // {
            //     if (models.ContainsKey(modelFile.Path))
            //     {
            //         context.ReportDiagnosticError(DiagnosticId.DuplicateModelPathError, "Duplicate model path", "Model path '{0}' is case-insensitive duplicate of '{1}'", modelFile.Path,
            //             models.Keys.FirstOrDefault(k => k.Equals(modelFile.Path, StringComparison.OrdinalIgnoreCase)));
            //         success = false;
            //     }
            //     else
            //     {
            //         string text = modelFile.GetText(context.CancellationToken).ToString();
            //         if (text is null)
            //         {
            //             context.ReportDiagnosticError(DiagnosticId.XmlParseError, "Could not read XML file", "Unable to read file '{0}'", modelFile.Path);
            //             success = false;
            //         }
            //         else
            //         {
            //             XDocument document;
            //             try { document = XDocument.Parse(text); }
            //             catch (XmlException exception)
            //             {
            //                 success = false;
            //                 if (string.IsNullOrWhiteSpace(exception.Message))
            //                 {
            //                     if (exception.TryGetLocation(text, modelFile.Path, out Location location))
            //                         context.ReportDiagnosticError(DiagnosticId.XmlParseError, "Could not parse XML file", "Error parsing XML file '{0}' at line {1}, column {2}", location, modelFile.Path,
            //                             exception.LineNumber, exception.LinePosition);
            //                     else
            //                         context.ReportDiagnosticError(DiagnosticId.XmlParseError, "Could not parse XML file", "Error parsing XML file '{0}' at line {1}, column {2}", modelFile.Path,
            //                             exception.LineNumber, exception.LinePosition);
            //                 }
            //                 else if (exception.TryGetLocation(text, modelFile.Path, out Location location))
            //                     context.ReportDiagnosticError(DiagnosticId.XmlParseError, "Could not parse XML file", "Error parsing XML file '{0}' at line {1}, column {2} ({3})", location, modelFile.Path,
            //                         exception.LineNumber, exception.LinePosition, exception.Message);
            //                 else
            //                     context.ReportDiagnosticError(DiagnosticId.XmlParseError, "Could not parse XML file", "Error parsing XML file '{0}' at line {1}, column {2} ({3})", modelFile.Path,
            //                         exception.LineNumber, exception.LinePosition, exception.Message);
            //                 continue;
            //             }
            //             catch (Exception exception)
            //             {
            //                 success = false;
            //                 if (string.IsNullOrWhiteSpace(exception.Message))
            //                     context.ReportDiagnosticError(DiagnosticId.XmlParseError, "Unexpected exception while parsing XML file", "Unexpected {0} while parsing XML file '{1}'", exception.GetType().Name,
            //                         modelFile.Path);
            //                 else
            //                     context.ReportDiagnosticError(DiagnosticId.XmlParseError, "Unexpected exception while parsing XML file", "Unexpected {0} while parsing XML file '{1}' ({2})", exception.GetType().Name,
            //                         modelFile.Path, exception.Message);
            //                 continue;
            //             }
            //             XElement root = document.Element(ElementNames.Models);
            //             if (root is null)
            //             {
            //                 context.ReportDiagnosticError(DiagnosticId.XmlParseError, "Root element missing", "{0} has no root element named {1}", modelFile.Path, ElementNames.Models.GetXName());
            //                 success = false;
            //             }
            //             else
            //                 models.Add(modelFile.Path, root);
            //         }
            //     }
            // }
            // if (success && models.Count > 0)
            //     foreach (KeyValuePair<string, XElement> pair in models)
            //         ProcessModelXml(pair.Key, pair.Value, models);
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

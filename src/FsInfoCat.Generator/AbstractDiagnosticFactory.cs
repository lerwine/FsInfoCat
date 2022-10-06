using Microsoft.CodeAnalysis;
using System;
using System.Linq;

namespace FsInfoCat.Generator
{
    public abstract class AbstractDiagnosticFactory
    {
        protected DiagnosticDescriptor Descriptor { get; }

        public DiagnosticId ID { get; }

        protected AbstractDiagnosticFactory(DiagnosticId id, string title, string messageFormat, bool isEnabledByDefault, string description, string helpLinkUri, params string[] customTags)
        {
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException($"'{nameof(title)}' cannot be null or whitespace.", nameof(title));
            if (string.IsNullOrWhiteSpace(messageFormat)) throw new ArgumentException($"'{nameof(messageFormat)}' cannot be null or whitespace.", nameof(messageFormat));
            ID = id;
            Descriptor = new($"MD{(ushort)id:X4}", title, messageFormat, DiagnostiCategoryAttribute.GetCategory(id, out DiagnosticSeverity defaultSeverity), defaultSeverity, isEnabledByDefault, description, helpLinkUri, customTags);
            if (customTags is null || (customTags = customTags.Where(t => !string.IsNullOrWhiteSpace(t)).ToArray()).Length == 0)
            {
                if (string.IsNullOrWhiteSpace(helpLinkUri))
                {
                    if (string.IsNullOrWhiteSpace(description))
                        Descriptor = new(((ushort)id).ToString("X4"), title, messageFormat, DiagnostiCategoryAttribute.GetCategory(id, out defaultSeverity), defaultSeverity, isEnabledByDefault);
                    else
                        Descriptor = new(((ushort)id).ToString("X4"), title, messageFormat, DiagnostiCategoryAttribute.GetCategory(id, out defaultSeverity), defaultSeverity, isEnabledByDefault, description);
                }
                else
                    Descriptor = new(((ushort)id).ToString("X4"), title, messageFormat, DiagnostiCategoryAttribute.GetCategory(id, out defaultSeverity), defaultSeverity, isEnabledByDefault, description, helpLinkUri);
            }
            else
                Descriptor = new(((ushort)id).ToString("X4"), title, messageFormat, DiagnostiCategoryAttribute.GetCategory(id, out defaultSeverity), defaultSeverity, isEnabledByDefault, description, helpLinkUri, customTags);
        }
    }
}

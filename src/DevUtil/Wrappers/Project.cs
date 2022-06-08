using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace DevUtil.Wrappers
{
    public class Project
    {
        public string FilePath => BaseObject.FilePath;

        public string OutputFilePath => BaseObject.OutputFilePath;

        public string OutputRefFilePath => BaseObject.OutputRefFilePath;

        public string DefaultNamespace => BaseObject.DefaultNamespace;

        public string AssemblyName => BaseObject.AssemblyName;

        public string Name => BaseObject.Name;

        public string Language => BaseObject.Language;

        public Guid ProjectId => BaseObject.Id.Id;

        internal Microsoft.CodeAnalysis.Project BaseObject { get; }

        internal Project([DisallowNull] Microsoft.CodeAnalysis.Project baseObject) => BaseObject = baseObject;

        internal IEnumerable<ProjectReference> GetProjectReferences() => BaseObject.ProjectReferences.Select(r => new ProjectReference(r));

        internal IEnumerable<Document> GetDocuments() => BaseObject.Documents.Select(d => new Document(d));

        internal IEnumerable<TextDocument> GetAdditionalDocuments() => BaseObject.AdditionalDocuments.Select(d => new TextDocument(d));

        internal bool TryGetDocument(Guid id, out ITextDocument result)
        {
            Microsoft.CodeAnalysis.TextDocument textDocument = BaseObject.GetDocument(Microsoft.CodeAnalysis.DocumentId.CreateFromSerialized(BaseObject.Id, id));
            if (textDocument is null)
            {
                result = null;
                return false;
            }
            if (textDocument is Microsoft.CodeAnalysis.Document document)
                result = new Document(document);
            else
                result = new TextDocument(textDocument);
            return true;
        }
    }
}

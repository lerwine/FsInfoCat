using System;
using System.Collections.Generic;

namespace DevUtil.Wrappers
{
    public class ProjectReference
    {
        public Guid ProjectId => BaseObject.ProjectId.Id;

        public bool EmbedInteropTypes => BaseObject.EmbedInteropTypes;

        public IReadOnlyList<string> Aliases => BaseObject.Aliases;

        internal Microsoft.CodeAnalysis.ProjectReference BaseObject { get; }

        public ProjectReference(Microsoft.CodeAnalysis.ProjectReference baseObject) => BaseObject = baseObject;
    }
}

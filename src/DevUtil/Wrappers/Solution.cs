using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DevUtil.Wrappers
{
    public class Solution
    {
        internal Microsoft.CodeAnalysis.Solution BaseObject { get; }

        public string FilePath => BaseObject.FilePath;

        public Guid SolutionId => BaseObject.Id.Id;

        internal Solution([DisallowNull] Microsoft.CodeAnalysis.Solution baseObject) => BaseObject = baseObject;

        internal IEnumerable<Project> GetAllProjects()
        {
            foreach (Microsoft.CodeAnalysis.Project baseObject in BaseObject.Projects)
                yield return new Project(baseObject);
        }

        internal bool TryGetProject(Guid id, out Project project)
        {
            Microsoft.CodeAnalysis.Project baseObject = BaseObject.GetProject(Microsoft.CodeAnalysis.ProjectId.CreateFromSerialized(id));
            if (baseObject is null)
            {
                project = null;
                return false;
            }
            project = new Project(baseObject);
            return true;
        }
    }
}

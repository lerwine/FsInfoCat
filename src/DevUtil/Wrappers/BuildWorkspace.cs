using MSBuild = Microsoft.CodeAnalysis.MSBuild;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Immutable;

namespace DevUtil.Wrappers
{
    public class BuildWorkspace
    {
        internal MSBuild.MSBuildWorkspace BaseObject { get; }

        public bool LoadMetadataForReferencedProjects => BaseObject.LoadMetadataForReferencedProjects;

        public bool SkipUnrecognizedProjects => BaseObject.SkipUnrecognizedProjects;

        public ImmutableDictionary<string, string> Properties => BaseObject.Properties;

        public BuildWorkspace() => BaseObject = MSBuild.MSBuildWorkspace.Create();

        public BuildWorkspace(MSBuild.MSBuildWorkspace baseObject) => BaseObject = baseObject;

        public BuildWorkspace([DisallowNull] IDictionary<string, string> properties) => BaseObject = MSBuild.MSBuildWorkspace.Create(properties);

        public Solution GetCurrentSolution()
        {
            Microsoft.CodeAnalysis.Solution solution = BaseObject.CurrentSolution;
            return (solution is null) ? null : new(solution);
        }

        public TaskJob OpenSolutionAsync(string solutionFilePath) => TaskJob.Create(token => BaseObject.OpenSolutionAsync(solutionFilePath, null, token), intermediate => new Solution(intermediate));
    }
}

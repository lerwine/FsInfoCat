using MSBuild = Microsoft.CodeAnalysis.MSBuild;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Immutable;
using System.Threading.Tasks;
using System.Threading;
using System.Management.Automation;

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

        private async Task<Microsoft.CodeAnalysis.Solution> OpenSolutionAsync(string solutionFilePath, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (string.IsNullOrWhiteSpace(solutionFilePath)) throw new PSArgumentException("Solution file path cannot be null or empty.", nameof(solutionFilePath));
            return await BaseObject.OpenSolutionAsync(solutionFilePath, null, cancellationToken);
        }

        public const string ActivityName_OpenSolution = "Open Solution";

        public TaskJob OpenSolutionAsync(params string[] solutionFilePath) => OpenSolutionAsync(0, solutionFilePath);

        public TaskJob OpenSolutionAsync(int activityId, params string[] solutionFilePath)
        {
            if (solutionFilePath is null || solutionFilePath.Length == 0) throw new PSArgumentException("Solution file path not provided.", nameof(solutionFilePath));
            if (solutionFilePath.Length == 1)
                return TaskJob.Create(token => OpenSolutionAsync(solutionFilePath[0], token), intermediate => new Solution(intermediate));
            return TaskJob.Create<Microsoft.CodeAnalysis.Solution, Solution>(async (writeProgress, writeWarning, writeError, writeOutput, token) =>
            {
                int percentComplete = 0;
                try
                {
                    double total = Convert.ToDouble(solutionFilePath.Length);
                    double count = 0.0;
                    foreach (string path in solutionFilePath)
                    {
                        writeProgress(new ProgressRecord(0, ActivityName_OpenSolution, $"Reading path {path}") { PercentComplete = percentComplete });
                        count += 100.0;
                        percentComplete = Convert.ToInt32(count / total);
                        Microsoft.CodeAnalysis.Solution solution;
                        try { solution = await OpenSolutionAsync(path, token); }
                        catch (Exception exception)
                        {
                            if (token.IsCancellationRequested) break;
                            if (exception is IContainsErrorRecord containsErrorRecord)
                                writeError(containsErrorRecord.ErrorRecord ?? new(exception, $"{nameof(BuildWorkspace)}.{nameof(OpenSolutionAsync)}:UnexpectedError", ErrorCategory.NotSpecified, path));
                            else
                                writeError(new(exception, $"{nameof(BuildWorkspace)}.{nameof(OpenSolutionAsync)}:UnexpectedError", ErrorCategory.NotSpecified, path));
                            continue;
                        }
                        if (solution is null)
                            writeWarning(new WarningRecord($"{nameof(BuildWorkspace)}.{nameof(OpenSolutionAsync)}:NotLoaded", $"Unable to load solution from path {path}"));
                        else
                            writeOutput(solution);
                    }
                }
                finally { writeProgress(new ProgressRecord(0, ActivityName_OpenSolution, token.IsCancellationRequested ? "Operation canceled" : "Operation completed") { PercentComplete = percentComplete, RecordType = ProgressRecordType.Completed }); }
            }, intermediate => new Solution(intermediate));
        }
    }
}

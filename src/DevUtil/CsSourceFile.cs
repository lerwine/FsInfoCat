using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Management.Automation;
using System.Threading;
using System.Threading.Tasks;

namespace DevUtil
{
    public sealed class CsSourceFile
    {
        private readonly object _syncRoot = new();
        private Task<CSharpSyntaxTree> _cSharpSyntaxTree;
        private Task<CompilationUnitSyntax> _compilationUnitRoot;

        public const string ActivityName_GetSyntaxTree = "Get Syntax Tree";

        public const string ActivityName_CompilationUnitRoot = "Get Compilation Unit Root";

        public FileInfo File { get; }

        internal static TaskJob StartGetSyntaxTree(Collection<CsSourceFile> sourceFiles) => StartGetSyntaxTree(0, null, sourceFiles);

        internal static TaskJob StartGetSyntaxTree(int activityId, Collection<CsSourceFile> sourceFiles) => StartGetSyntaxTree(activityId, null, sourceFiles);

        internal static TaskJob StartGetSyntaxTree(int activityId, int? parentActivityId, Collection<CsSourceFile> sourceFiles)
        {
            return new(async (writeOutput, writeProgress, writeError, cancellationToken) =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (sourceFiles is null) return;
                int percentComplete = 0;
                int completed = 0;
                try
                {
                    foreach (CsSourceFile f in sourceFiles)
                    {
                        completed++;
                        if (f is not null)
                        {
                            writeProgress(parentActivityId.HasValue ? new ProgressRecord(activityId, ActivityName_GetSyntaxTree, $"{completed} of {sourceFiles.Count}") {
                                CurrentOperation = f.File.FullName,
                                PercentComplete = percentComplete,
                                ParentActivityId = parentActivityId.Value
                            } : new ProgressRecord(activityId, ActivityName_GetSyntaxTree, $"{completed} of {sourceFiles.Count}") {
                                CurrentOperation = f.File.FullName,
                                PercentComplete = percentComplete
                            });
                            CSharpSyntaxTree syntaxTree = await f.GetSyntaxTreeAsync(cancellationToken);
                            if (syntaxTree is not null) writeOutput(PSObject.AsPSObject(syntaxTree));
                            percentComplete = Convert.ToInt32(Convert.ToDouble(completed) * 100.0 / Convert.ToDouble(sourceFiles.Count));
                        }
                    }
                }
                finally
                {
                    writeProgress(parentActivityId.HasValue ? new ProgressRecord(activityId, ActivityName_GetSyntaxTree, $"{completed} of {sourceFiles.Count}") {
                        PercentComplete = percentComplete,
                        ParentActivityId = parentActivityId.Value,
                        RecordType = ProgressRecordType.Completed
                    } : new ProgressRecord(activityId, ActivityName_GetSyntaxTree, $"{completed} of {sourceFiles.Count}") {
                        PercentComplete = percentComplete,
                        RecordType = ProgressRecordType.Completed
                    });
                }
            });
        }

        internal TaskJob StartGetSyntaxTree() => TaskJob.StartNew(GetSyntaxTreeAsync);

        internal Task<CSharpSyntaxTree> GetSyntaxTreeAsync(CancellationToken cancellationToken = default)
        {
            Monitor.Enter(_syncRoot);
            try
            {
                if (_cSharpSyntaxTree is null)
                    _cSharpSyntaxTree = File.OpenText().ReadToEndAsync().ContinueWith(task =>
                        CSharpSyntaxTree.ParseText(task.Result, CSharpParseOptions.Default, File.FullName, null, cancellationToken) as CSharpSyntaxTree, cancellationToken,
                        TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.Default);
                return _cSharpSyntaxTree;
            }
            finally { Monitor.Exit(_syncRoot); }
        }

        internal static TaskJob StartGetCompilationUnitRoot(Collection<CsSourceFile> sourceFiles) => StartGetCompilationUnitRoot(0, null, sourceFiles);

        internal static TaskJob StartGetCompilationUnitRoot(int activityId, Collection<CsSourceFile> sourceFiles) => StartGetCompilationUnitRoot(activityId, null, sourceFiles);

        internal static TaskJob StartGetCompilationUnitRoot(int activityId, int? parentActivityId, Collection<CsSourceFile> sourceFiles)
        {
            return new(async (writeOutput, writeProgress, writeError, cancellationToken) =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (sourceFiles is null) return;
                int percentComplete = 0;
                int completed = 0;
                try
                {
                    foreach (CsSourceFile f in sourceFiles)
                    {
                        completed++;
                        if (f is not null)
                        {
                            writeProgress(parentActivityId.HasValue ? new ProgressRecord(activityId, ActivityName_CompilationUnitRoot, $"{completed} of {sourceFiles.Count}") {
                                CurrentOperation = f.File.FullName,
                                PercentComplete = percentComplete,
                                ParentActivityId = parentActivityId.Value
                            } : new ProgressRecord(activityId, ActivityName_CompilationUnitRoot, $"{completed} of {sourceFiles.Count}") {
                                CurrentOperation = f.File.FullName,
                                PercentComplete = percentComplete
                            });
                            CompilationUnitSyntax syntax = await f.GetCompilationUnitRootAsync(cancellationToken);
                            if (syntax is not null) writeOutput(PSObject.AsPSObject(syntax));
                            percentComplete = Convert.ToInt32(Convert.ToDouble(completed) * 100.0 / Convert.ToDouble(sourceFiles.Count));
                        }
                    }
                }
                finally
                {
                    writeProgress(parentActivityId.HasValue ? new ProgressRecord(activityId, ActivityName_CompilationUnitRoot, $"{completed} of {sourceFiles.Count}") {
                        PercentComplete = percentComplete,
                        ParentActivityId = parentActivityId.Value,
                        RecordType = ProgressRecordType.Completed
                    } : new ProgressRecord(activityId, ActivityName_CompilationUnitRoot, $"{completed} of {sourceFiles.Count}") {
                        PercentComplete = percentComplete,
                        RecordType = ProgressRecordType.Completed
                    });
                }
            });
        }

        internal TaskJob StartGetCompilationUnitRoot() => TaskJob.StartNew(GetCompilationUnitRootAsync);

        internal Task<CompilationUnitSyntax> GetCompilationUnitRootAsync(CancellationToken cancellationToken = default)
        {
            Monitor.Enter(_syncRoot);
            try
            {
                if (_compilationUnitRoot is null)
                    _compilationUnitRoot = GetSyntaxTreeAsync(cancellationToken).ContinueWith(task => task.Result?.GetCompilationUnitRoot(cancellationToken), cancellationToken,
                        TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.Default);
                return _compilationUnitRoot;
            }
            finally { Monitor.Exit(_syncRoot); }
        }

        internal CsSourceFile([DisallowNull] FileInfo file) => File = file ?? throw new ArgumentNullException(nameof(file));
    }
}

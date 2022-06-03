using DevUtil.CodeParsing;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Provider;
using System.Text;

namespace DevUtil.Commands
{
    [Cmdlet(VerbsCommon.Open, "CSharpSyntaxTree", DefaultParameterSetName = ParameterSetName_Path)]
    [OutputType(typeof(SourceCodeFile))]
    public class Open_CSharpSyntaxTree : PSCmdlet
    {
        public const string ParameterSetName_Path = "Path";
        public const string ParameterSetName_LiteralPath = "LiteralPath";

        /// <summary>
        /// The name to give the table.
        /// </summary>
        [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = ParameterSetName_Path)]
        [ValidateNotNullOrEmpty()]
        [SupportsWildcards()]
        public string Path { get; set; }

        /// <summary>
        /// The name to give the table.
        /// </summary>
        [Parameter(Mandatory = true, ParameterSetName = ParameterSetName_LiteralPath)]
        [ValidateNotNullOrEmpty()]
        [Alias("FileName")]
        public string LiteralPath { get; set; }

        protected override void ProcessRecord()
        {
            void processPath(string path)
            {
                StringBuilder builder = new();
                Collection<IContentReader> readers = InvokeProvider.Content.GetReader(new string[] { path }, true, true);
                foreach (IContentReader r in readers)
                {
                    IList blocks = r.Read(0L);
                    if (blocks is IList<string> lines)
                        foreach (string l in lines)
                            builder.AppendLine(l);
                    else if (blocks is IList<byte> bytes)
                    {
                        using MemoryStream memoryStream = new((bytes is byte[] buffer) ? buffer : (bytes is List<byte> list) ? list.ToArray() : bytes.ToArray());
                        using StreamReader reader = new(memoryStream, true);
                        while (!reader.EndOfStream)
                            builder.AppendLine(reader.ReadLine());
                    }
                    else
                        foreach (object obj in blocks)
                            builder.Append((obj is string s) ? s : obj.ToString());
                }

                CSharpSyntaxTree syntaxTree;
                try { syntaxTree = (CSharpSyntaxTree)CSharpSyntaxTree.ParseText(builder.ToString(), null, path); }
                catch (Exception exception)
                {
                    ErrorRecord errorRecord = new(exception, $"{nameof(Open_CSharpSyntaxTree)}:{nameof(CSharpSyntaxTree)}.{nameof(CSharpSyntaxTree.ParseText)}", ErrorCategory.ParserError, path);
                    errorRecord.CategoryInfo.Reason = $"{nameof(CSharpSyntaxTree)}.{nameof(CSharpSyntaxTree.ParseText)} threw {exception.GetType().Name}.";
                    return;
                }
                WriteObject(new SourceCodeFile(syntaxTree, path));
            };
            if (ParameterSetName == ParameterSetName_LiteralPath)
            {
                string path;
                try { path = GetUnresolvedProviderPathFromPSPath(LiteralPath); }
                catch (Exception exception)
                {
                    if (exception is IContainsErrorRecord containsErrorRecord)
                        WriteError(containsErrorRecord.ErrorRecord);
                    else
                    {
                        ErrorRecord errorRecord = new(exception, $"{nameof(Open_CSharpSyntaxTree)}.{nameof(GetUnresolvedProviderPathFromPSPath)}", ErrorCategory.NotSpecified, LiteralPath);
                        errorRecord.CategoryInfo.Reason = $"{nameof(GetUnresolvedProviderPathFromPSPath)} threw exception {exception.GetType().Name}";
                        WriteError(errorRecord);
                    }
                    return;
                }
                processPath(path);
            }
            else
            {
                Collection<string> paths;
                try { paths = GetResolvedProviderPathFromPSPath(Path, out _);}
                catch (Exception exception)
                {
                    if (exception is IContainsErrorRecord containsErrorRecord)
                        WriteError(containsErrorRecord.ErrorRecord);
                    else
                    {
                        ErrorRecord errorRecord = new(exception, $"{nameof(Open_CSharpSyntaxTree)}.{nameof(GetResolvedProviderPathFromPSPath)}", ErrorCategory.NotSpecified, LiteralPath);
                        errorRecord.CategoryInfo.Reason = $"{nameof(GetResolvedProviderPathFromPSPath)} threw exception {exception.GetType().Name}";
                        WriteError(errorRecord);
                    }
                    return;
                }
                foreach (string p in paths)
                    processPath(p);
            }
        }
    }
}

using System;
using System.IO;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.VisualStudio.TextTemplating;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace StandaloneT4Host
{
    class Program
    {
        enum ExitCodes
        {
            Success = 0,
            NoProject,
            InvalidProjectPath,
            ProjectPathNotFound,
            InvalidProjectFile,
            InvalidTemplateFile,
            NoTemplateFiles,
            OtherError
        }
        const int EXIT_CODE_NO_PROJECT = 1;
        static readonly Regex NewLineRegex = new Regex(@"\r\n?|\n", RegexOptions.Compiled);
        static int Main(string[] args)
        {
            if (args.Length < 1 || string.IsNullOrWhiteSpace(args[0]))
            {
                Console.Error.WriteLine("You must provide the project path");
                return (int)(ExitCodes.NoProject);
            }
            Collection<ProjectInfo> projects = new Collection<ProjectInfo>();
            FileInfo fileInfo;
            try { fileInfo  = new FileInfo(args[0]); }
            catch (Exception exc)
            {
                Console.Error.WriteLine("Cannot validate project path: " + exc.Message);
                return (int)(ExitCodes.InvalidProjectPath);
            }
            if (!fileInfo.Exists)
            {
                Console.Error.WriteLine("Project file not found (" + fileInfo.FullName + ").");
                return (int)(ExitCodes.ProjectPathNotFound);
            }
            ProjectInfo currentProject;
            try { currentProject = new ProjectInfo(fileInfo); }
            catch (Exception exc)
            {
                Console.Error.WriteLine("Cannot parse project file xml: " + exc.Message);
                return (int)(ExitCodes.InvalidProjectFile);
            }
            Collection<FileInfo> items = new Collection<FileInfo>();
            for (int i = 1; i < args.Length; i++)
            {
                try { fileInfo  = new FileInfo(args[i]); }
                catch (Exception exc)
                {
                    Console.Error.WriteLine("Cannot validate path (argument index " + i.ToString() + "): " + exc.Message);
                    return (int)(ExitCodes.InvalidProjectPath);
                }
                if (!fileInfo.Exists)
                {
                    Console.Error.WriteLine("File not found (argument index " + i.ToString() + ")");
                    return (int)(ExitCodes.ProjectPathNotFound);
                }
                if (fileInfo.Extension.ToLower().EndsWith("proj"))
                {
                    if (items.Count > 0)
                    {
                        projects.Add(new ProjectInfo(currentProject, items));
                        items.Clear();
                    }
                    else
                        projects.Add(currentProject);
                    try { currentProject = new ProjectInfo(fileInfo); }
                    catch (Exception exc)
                    {
                        Console.Error.WriteLine("Cannot parse project file xml: " + exc.Message);
                        return (int)(ExitCodes.InvalidProjectFile);
                    }
                }
                else
                    items.Add(fileInfo);
            }
            if (items.Count > 0)
                projects.Add(new ProjectInfo(currentProject, items));
            else
                projects.Add(currentProject);
            var toProcess = projects.Select(p => new
            {
                Project = p,
                Items = p.Items.Where(i => null != i.ItemFile && ProjectItem.NameComparer.Equals(i.ItemFile.Extension, ".tt")).ToArray()
            }).Where(a => a.Items.Length > 0).ToArray();
            if (toProcess.Length == 0)
            {
                Console.WriteLine("No text templates found (" + currentProject.ProjectFile.FullName + ").");
                return (int)(ExitCodes.NoTemplateFiles);
            }

            CustomTextTemplatingHost host = new CustomTextTemplatingHost();
            Engine engine = new Engine();
            foreach (var a in toProcess)
            {
                host.Project = a.Project;
                foreach (ProjectItem item in a.Items)
                {
                    //Read the text template.
                    string input = File.ReadAllText(item.ItemFile.FullName);
                    host.TemplateFile = item.ItemFile.FullName;
                    //Transform the text template.
                    string output = engine.ProcessTemplate(input, host);
                    string outputFileName = Path.GetFileNameWithoutExtension(item.ItemFile.Name);
                    outputFileName = Path.Combine(item.ItemFile.DirectoryName, outputFileName) + host.FileExtension;
                    File.WriteAllText(outputFileName, output, host.FileEncoding);
                    if (host.Errors.Count > 0)
                    {
                        CompilerError[] warnings = host.Errors.OfType<CompilerError>().Where(e => e.IsWarning).ToArray();
                        int errorCount = host.Errors.Count - warnings.Length;
                        if (warnings.Length == 0)
                            Console.Error.WriteLine(((host.Errors.Count > 1) ? host.Errors.Count.ToString() + " errors in " : "1 error in ") + item.ItemFile.FullName + ":");
                        else
                        {
                            if (errorCount > 0)
                                Console.Error.WriteLine(((host.Errors.Count > 1) ? host.Errors.Count.ToString() + " errors in " : "1 error in ") + item.ItemFile.FullName +
                                    " and " + ((warnings.Length > 1) ? warnings.Length.ToString() + " warnings in " : "1 warning in ") + item.ItemFile.FullName + ":");
                            else
                                Console.WriteLine(((warnings.Length > 1) ? warnings.Length.ToString() + " warnings in " : "1 warning in ") + item.ItemFile.FullName + ":");
                        }

                        foreach (CompilerError error in host.Errors)
                        {
                            string[] lines = NewLineRegex.Split(error.ErrorText);
                            if (string.IsNullOrWhiteSpace(error.ErrorNumber))
                                lines[0] = "  [" + error.Line.ToString() + "," + error.Column.ToString() + "]:" + (" " + lines[0].TrimStart()).TrimEnd();
                            else
                                lines[0] = "  [" + error.ErrorNumber + " " + error.Line.ToString() + "," + error.Column.ToString() + "]:" + (" " + lines[0].TrimStart()).TrimEnd();
                            for (int i = 0; i < lines.Length; i++)
                                lines[i] = "    " + lines[i].TrimEnd();
                            if (error.IsWarning)
                                foreach (string l in lines)
                                    Console.WriteLine(l);
                            else
                                foreach (string l in lines)
                                    Console.Error.WriteLine(l);
                        }
                        if (errorCount > 0)
                            return (int)ExitCodes.OtherError;
                    }
                }
            }
            return (int)ExitCodes.Success;
        }
    }
}

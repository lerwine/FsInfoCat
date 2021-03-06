﻿using System;
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
            NoProject = 1,
            InvalidProjectPath = 2,
            ProjectPathNotFound = 3,
            InvalidProjectFile = 4,
            InvalidTemplateFile = 5,
            NoTemplateFiles = 6,
            OtherError = 7
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
            Tuple<ProjectInfo, Collection<FileInfo>> currentProject;
            try { currentProject = new Tuple<ProjectInfo, Collection<FileInfo>>(new ProjectInfo(fileInfo), new Collection<FileInfo>()); }
            catch (Exception exc)
            {
                Console.Error.WriteLine("Cannot parse project file xml: " + exc.Message);
                return (int)(ExitCodes.InvalidProjectFile);
            }
            Collection<Tuple<ProjectInfo, Collection<FileInfo>>> projects = new Collection<Tuple<ProjectInfo, Collection<FileInfo>>>();
            string binDir = "bin" + Path.DirectorySeparatorChar.ToString();
            string objDir = "obj" + Path.DirectorySeparatorChar.ToString();
            bool explictFile = false;
            for (int i = 1; i < args.Length; i++)
            {
                try { fileInfo  = new FileInfo(args[i]); }
                catch (Exception exc)
                {
                    Console.Error.WriteLine("Cannot validate path (argument index " + i.ToString() + "): " + exc.Message);
                    return (int)(ExitCodes.InvalidProjectPath);
                }
                if (fileInfo.Extension.ToLower().EndsWith("proj"))
                {
                    if (!fileInfo.Exists)
                    {
                        Console.Error.WriteLine("File not found (argument index " + i.ToString() + ")");
                        return (int)(ExitCodes.ProjectPathNotFound);
                    }
                    if (currentProject.Item2.Count > 0)
                        projects.Add(currentProject);
                    else if (!explictFile)
                    {
                        int l = currentProject.Item1.ProjectFile.DirectoryName.Length + 1;
                        foreach (FileInfo f in fileInfo.Directory.GetFiles("*.tt", SearchOption.AllDirectories))
                        {
                            string n = f.FullName.Substring(l).ToLower();
                            if (!(f.FullName.Substring(l).StartsWith(binDir) || f.FullName.Substring(l).StartsWith(objDir)))
                                currentProject.Item2.Add(f);
                        }
                        if (currentProject.Item2.Count == 0)
                            Console.Error.WriteLine("No text templates found (" + currentProject.Item1.ProjectFile.FullName + ").");
                        else
                            projects.Add(currentProject);
                    }
                    explictFile = false;
                    try { currentProject = new Tuple<ProjectInfo, Collection<FileInfo>>(new ProjectInfo(fileInfo), new Collection<FileInfo>()); }
                    catch (Exception exc)
                    {
                        Console.Error.WriteLine("Cannot parse project file xml: " + exc.Message);
                        return (int)(ExitCodes.InvalidProjectFile);
                    }
                }
                else
                {
                    explictFile = true;
                    if (!fileInfo.Exists)
                    {
                        Console.Error.WriteLine("File not found (argument index " + i.ToString() + ")");
                        return (int)(ExitCodes.ProjectPathNotFound);
                    }
                    currentProject.Item2.Add(fileInfo);
                }
            }
            if (currentProject.Item2.Count > 0)
                projects.Add(currentProject);
            else if (!explictFile)
            {
                int l = currentProject.Item1.ProjectFile.DirectoryName.Length + 1;
                foreach (FileInfo f in fileInfo.Directory.GetFiles("*.tt", SearchOption.AllDirectories))
                {
                    string n = f.FullName.Substring(l).ToLower();
                    if (!(f.FullName.Substring(l).StartsWith(binDir) || f.FullName.Substring(l).StartsWith(objDir)))
                        currentProject.Item2.Add(f);
                }
                if (currentProject.Item2.Count == 0)
                    Console.Error.WriteLine("No text templates found (" + currentProject.Item1.ProjectFile.FullName + ").");
                else
                    projects.Add(currentProject);
            }
            if (projects.Count == 0)
                return (int)(ExitCodes.NoTemplateFiles);

            CustomTextTemplatingHost host = new CustomTextTemplatingHost();
            Engine engine = new Engine();
            foreach (Tuple<ProjectInfo, Collection<FileInfo>> a in projects)
            {
                host.Project = a.Item1;
                foreach (FileInfo item in a.Item2)
                {
                    //Read the text template.
                    string input = File.ReadAllText(item.FullName);
                    host.TemplateFile = item.FullName;
                    //Transform the text template.
                    string output = engine.ProcessTemplate(input, host);
                    string outputFileName = Path.GetFileNameWithoutExtension(item.Name);
                    outputFileName = Path.Combine(item.DirectoryName, outputFileName) + host.FileExtension;
                    File.WriteAllText(outputFileName, output, host.FileEncoding);
                    if (host.Errors.Count > 0)
                    {
                        CompilerError[] warnings = host.Errors.OfType<CompilerError>().Where(e => e.IsWarning).ToArray();
                        int errorCount = host.Errors.Count - warnings.Length;
                        if (warnings.Length == 0)
                            Console.Error.WriteLine(((host.Errors.Count > 1) ? host.Errors.Count.ToString() + " errors in " : "1 error in ") + item.FullName + ":");
                        else
                        {
                            if (errorCount > 0)
                                Console.Error.WriteLine(((host.Errors.Count > 1) ? host.Errors.Count.ToString() + " errors in " : "1 error in ") + item.FullName +
                                    " and " + ((warnings.Length > 1) ? warnings.Length.ToString() + " warnings in " : "1 warning in ") + item.FullName + ":");
                            else
                                Console.WriteLine(((warnings.Length > 1) ? warnings.Length.ToString() + " warnings in " : "1 warning in ") + item.FullName + ":");
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

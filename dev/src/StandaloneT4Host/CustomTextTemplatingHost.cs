using System;
using System.IO;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TextTemplating;
using System.Collections.ObjectModel;
using System.Xml;

namespace StandaloneT4Host
{
    public class CustomTextTemplatingHost : MarshalByRefObject, ITextTemplatingEngineHost
    {
        public IList<string> StandardAssemblyReferences => new ReadOnlyCollection<string>(new string[]
        {
            typeof(System.Uri).Assembly.Location,
            typeof(System.Linq.Enumerable).Assembly.Location,
            typeof(System.Xml.XmlDocument).Assembly.Location,
            GetType().Assembly.Location
        });

        public IList<string> StandardImports => new ReadOnlyCollection<string>(new string[]
        {
            "System"
        });

        public string TemplateFile { get; internal set; } = "";
        public string FileExtension { get; private set; } = ".txt";
        public Encoding FileEncoding { get; private set; } = new UTF8Encoding(false, false);
        public ProjectInfo Project { get; internal set; }
        public CompilerErrorCollection Errors { get; private set; }

        public object GetHostOption(string optionName)
        {
            if (optionName == "CacheAssemblies")
                return true;
            return null;
        }

        public bool LoadIncludeText(string requestFileName, out string content, out string location)
        {
            content = location = "";

            if (File.Exists(requestFileName))
            {
                content = File.ReadAllText(requestFileName);
                return true;
            }
            return false;
        }

        public void LogErrors(CompilerErrorCollection errors)
        {
            Errors = errors;
        }

        public AppDomain ProvideTemplatingAppDomain(string content)
        {
            return AppDomain.CreateDomain("Generation App Domain");
        }

        //The engine calls this method to resolve assembly references used in
        //the generated transformation class project and for the optional
        //assembly directive if the user has specified it in the text template.
        //This method can be called 0, 1, or more times.
        public string ResolveAssemblyReference(string assemblyReference)
        {
            if (File.Exists(assemblyReference))
                return assemblyReference;
            //Maybe the assembly is in the same folder as the text template that
            //called the directive.
            //----------------------------------------------------------------
            string candidate = Path.Combine(Path.GetDirectoryName(this.TemplateFile), assemblyReference);
            if (File.Exists(candidate))
                return candidate;

            return "";
        }

        //The engine calls this method based on the directives the user has
        //specified in the text template.
        //This method can be called 0, 1, or more times.
        public Type ResolveDirectiveProcessor(string processorName)
        {
            //This host will not resolve any specific processors.
            //Check the processor name, and if it is the name of a processor the
            //host wants to support, return the type of the processor.
            //---------------------------------------------------------------------
            // if (string.Compare(processorName, "XYZ", StringComparison.OrdinalIgnoreCase) == 0)
            // {
            //     //return typeof();
            // }
            //This can be customized to search specific paths for the file
            //or to search the GAC
            //If the directive processor cannot be found, throw an error.
            throw new Exception("Directive Processor not found");
        }

        //If a call to a directive in a text template does not provide a value
        //for a required parameter, the directive processor can try to get it
        //from the host by calling this method.
        //This method can be called 0, 1, or more times.
        public string ResolveParameterValue(string directiveId, string processorName, string parameterName)
        {
            if (directiveId == null)
                throw new ArgumentNullException("directiveId");
            if (processorName == null)
                throw new ArgumentNullException("processorName");
            if (parameterName == null)
                throw new ArgumentNullException("parameterName");
            //Code to provide "hard-coded" parameter values goes here.
            //This code depends on the directive processors this host will interact with.
            //If we cannot do better, return the empty string.
            return "";
        }

        //A directive processor can call this method if a file name does not
        //have a path.
        //The host can attempt to provide path information by searching
        //specific paths for the file and returning the file and path if found.
        //This method can be called 0, 1, or more times.
        //---------------------------------------------------------------------
        public string ResolvePath(string path)
        {
            if (path == null)
                throw new ArgumentNullException("path");
            //If the argument is the fully qualified path of an existing file,
            //then we are done
            //----------------------------------------------------------------
            if (!File.Exists(path))
            {
                string candidate = Path.Combine(Path.GetDirectoryName(this.TemplateFile), path);
                if (File.Exists(candidate))
                    return candidate;
            }
            return path;
        }

        public void SetFileExtension(string extension)
        {
            FileExtension = (string.IsNullOrEmpty(extension)) ? "" : (extension[0] == '.') ? extension : "." + extension;
        }

        public void SetOutputEncoding(Encoding encoding, bool fromOutputDirective)
        {
            FileEncoding = (null == encoding) ? new UTF8Encoding(false, false) : encoding;
        }
    }
}

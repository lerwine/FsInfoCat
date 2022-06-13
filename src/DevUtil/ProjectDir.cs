using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Xml.Linq;

namespace DevUtil
{
    public class ProjectDir
    {
        public ReadOnlyDictionary<string, CsSourceFile> SourceFiles { get; }

        public ProjectDir(DirectoryInfo directoryInfo)
        {
            if (directoryInfo is null) throw new ArgumentNullException(nameof(directoryInfo));
            if (!directoryInfo.Exists) throw new ItemNotFoundException($"{directoryInfo.FullName} not found.");

            Dictionary<string, CsSourceFile> sourceFiles = new();
            void crawlFiles(string uri, DirectoryInfo subdir)
            {
                foreach (FileInfo file in subdir.GetFiles("*.cs", new EnumerationOptions() {
                    IgnoreInaccessible = true,
                    MatchCasing = MatchCasing.CaseInsensitive
                }))
                    sourceFiles.Add(Path.Combine(uri, file.Name), new(file));

                foreach (DirectoryInfo dir in subdir.GetDirectories())
                    crawlFiles(Path.Combine(uri, dir.Name), dir);
            };

            foreach (FileInfo file in directoryInfo.GetFiles("*.cs", new EnumerationOptions() {
                IgnoreInaccessible = true,
                MatchCasing = MatchCasing.CaseInsensitive
            }))
                sourceFiles.Add(file.Name, new(file));

            foreach (DirectoryInfo dir in directoryInfo.GetDirectories())
            {
                if (!(string.Equals(dir.Name, "obj", StringComparison.InvariantCultureIgnoreCase) || string.Equals(dir.Name, "bin", StringComparison.InvariantCultureIgnoreCase)))
                    crawlFiles(dir.Name, dir);
            }

            SourceFiles = new(sourceFiles);
        }
    }
}

using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace StandaloneT4Host
{
    public sealed class ProjectItem : MarshalByRefObject
    {
        public static readonly StringComparer NameComparer = StringComparer.InvariantCultureIgnoreCase;

        public ProjectItem(string kind, string include, string copyToOutputDirectory, string subType, string dependentUpon, ProjectInfo project)
        {
            Kind = kind;
            Include = include;
            CopyToOutputDirectory = (string.IsNullOrWhiteSpace(copyToOutputDirectory)) ? "" : copyToOutputDirectory;
            SubType = (string.IsNullOrWhiteSpace(subType)) ? "" : subType;
            DependentUpon = (string.IsNullOrWhiteSpace(dependentUpon)) ? "" : dependentUpon;
            try
            {
                FileInfo f = new FileInfo(Path.Combine(project.ProjectFile.DirectoryName, Include));
                ItemFile = (f.Exists) ? f : null;
            }
            catch { ItemFile = null; }
        }

        public static void Fill(DirectoryInfo parentDirectory, Collection<ProjectItem> itemCollection)
        {
            Fill(parentDirectory, 0, itemCollection);
        }

        private static void Fill(DirectoryInfo parentDirectory, int depth, Collection<ProjectItem> itemCollection)
        {
            foreach (FileInfo f in parentDirectory.GetFiles().Where(f =>
            {
                switch (f.Extension.ToLower())
                {
                    case ".suo":
                    case ".sln":
                    case ".user":
                        return false;
                    default:
                        return !f.Extension.EndsWith("proj");
                }
            }))
            {
                ProjectItem item = new ProjectItem(f, depth);
                if (!itemCollection.Any(i => NameComparer.Equals(i.Include, item.Include)))
                    itemCollection.Add(item);
            }
            depth++;
            foreach (DirectoryInfo d in parentDirectory.GetDirectories().Where(f =>
            {
                switch (f.Name.ToLower())
                {
                    case "bin":
                    case "obj":
                    case "packages":
                    case "node_modules":
                    case "wwwroot":
                        return false;
                    default:
                        return f.Name[0] != '.';
                }
            }))
                Fill(d, depth, itemCollection);
        }

        public ProjectItem(FileInfo fileInfo, int depth)
        {
            switch (fileInfo.Extension.ToLower())
            {
                case ".cs":
                    Kind = "Compile";
                    break;
                case ".tt":
                case ".md":
                case "":
                    Kind = "None";
                    CopyToOutputDirectory = "Never";
                    break;
                default:
                    Kind = "Content";
                    CopyToOutputDirectory = "Always";
                    break;
            }
            string n = fileInfo.Name;
            DirectoryInfo d = fileInfo.Directory;
            for (int i = 0; i < depth; i++)
            {
                n = Path.Combine(d.Name, n);
                if (null == (d = d.Parent))
                    break;
            }
            Include = n;
            SubType = DependentUpon = "";
        }

        public string Kind { get; }
        public string Include { get; }
        public string CopyToOutputDirectory { get; }
        public string SubType { get; }
        public string DependentUpon { get; }
        public FileInfo ItemFile { get; }
    }
}

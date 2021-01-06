using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml;

namespace StandaloneT4Host
{
    public sealed class ProjectInfo : MarshalByRefObject
    {
        public FileInfo ProjectFile { get; }
        public string Sdk { get; }
        public string TargetFramework { get; }
        public Guid? ProjectGuid { get; }
        public string RootNamespace { get; }
        public string Company { get; }
        public string AssemblyTitle { get; }
        public string Description { get; }
        public string Product { get; }
        public string Copyright { get; }
        public Version AssemblyVersion { get; }
        public Version FileVersion { get; }
        public bool IsCompatible { get; }
        public ReadOnlyCollection<ProjectItem> Items { get; }

        public ProjectInfo(ProjectInfo projectInfo, IEnumerable<FileInfo> include)
        {
            ProjectFile = projectInfo.ProjectFile;
            Sdk = projectInfo.Sdk;
            TargetFramework = projectInfo.TargetFramework;
            ProjectGuid = projectInfo.ProjectGuid;
            ProjectFile = projectInfo.ProjectFile;
            RootNamespace = projectInfo.RootNamespace;
            Company = projectInfo.Company;
            AssemblyTitle = projectInfo.AssemblyTitle;
            Description = projectInfo.Description;
            Product = projectInfo.Product;
            Copyright = projectInfo.Copyright;
            AssemblyVersion = projectInfo.AssemblyVersion;
            FileVersion = projectInfo.FileVersion;
            IsCompatible = projectInfo.IsCompatible;
            Items = new ReadOnlyCollection<ProjectItem>(include.Select(f =>
            {
                ProjectItem m = projectInfo.Items.FirstOrDefault(i => null != i.ItemFile && ProjectItem.NameComparer.Equals(i.ItemFile.FullName, f.FullName));
                if (null != m)
                    return m;

                DirectoryInfo d = f.Directory;
                int depth = 0;
                while (!ProjectItem.NameComparer.Equals(d.FullName, ProjectFile.DirectoryName)) {
                    if (null == (d = d.Parent))
                        throw new Exception("Item does not belong to project");
                    depth++;
                }
                return new ProjectItem(f, depth);
            }).ToArray());
        }

        public ProjectInfo(FileInfo projectFile)
        {
            if (null == projectFile)
                throw new ArgumentNullException("projectFile");
            if (!projectFile.Exists)
                throw new FileNotFoundException("Project file not found", projectFile.FullName);
            ProjectFile = projectFile;
            XmlDocument xmlDocument = new XmlDocument();
            try { xmlDocument.Load(projectFile.FullName); }
            catch (Exception exc)
            {
                throw new ArgumentException(
                    (string.IsNullOrWhiteSpace(exc.Message)) ? "Unable to load project file." : "Unable to load project file: " + exc.Message,
                    "projectFile", exc
                );
            }
            if (null == xmlDocument.DocumentElement)
                throw new ArgumentException("No XML data loaded");
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDocument.NameTable);
            nsmgr.AddNamespace("msb", xmlDocument.DocumentElement.NamespaceURI);
            Func<XmlNode, string, string> getNodeText = (node, xpath) =>
            {
                XmlNode n = node.SelectSingleNode(xpath, nsmgr);
                if (null != n)
                {
                    if (n is XmlText)
                        return n.InnerText;
                    if (n is XmlElement)
                    {
                        XmlElement e = (XmlElement)n;
                        if (!e.IsEmpty)
                            return e.InnerText;
                    }
                    else
                        return n.Value;
                }
                return null;
            };
            Sdk = getNodeText(xmlDocument, "/msb:Project/@Sdk");
            TargetFramework = getNodeText(xmlDocument, "/msb:Project/msb:PropertyGroup/msb:TargetFramework");
            string s = getNodeText(xmlDocument, "/msb:Project/msb:PropertyGroup/msb:ProjectGuid");
            ProjectGuid = (string.IsNullOrWhiteSpace(s) || !Guid.TryParse(s, out Guid guid)) ? (Guid?)null : guid;
            s = getNodeText(xmlDocument, "/msb:Project/msb:PropertyGroup/msb:RootNamespace");
            RootNamespace = (string.IsNullOrWhiteSpace(s)) ? ProjectFile.Directory.Name : s;
            Company = getNodeText(xmlDocument, "/msb:Project/msb:PropertyGroup/msb:Company");
            AssemblyTitle = getNodeText(xmlDocument, "/msb:Project/msb:PropertyGroup/msb:Company");
            Description = getNodeText(xmlDocument, "/msb:Project/msb:PropertyGroup/msb:Description");
            Product = getNodeText(xmlDocument, "/msb:Project/msb:PropertyGroup/msb:Product");
            Copyright = getNodeText(xmlDocument, "/msb:Project/msb:PropertyGroup/msb:Copyright");
            s = getNodeText(xmlDocument, "/msb:Project/msb:PropertyGroup/msb:AssemblyVersion");
            AssemblyVersion = (string.IsNullOrWhiteSpace(s) || !Version.TryParse(s, out Version version)) ? null : version;
            s = getNodeText(xmlDocument, "/msb:Project/msb:PropertyGroup/msb:FileVersion");
            FileVersion = (string.IsNullOrWhiteSpace(s) || !Version.TryParse(s, out version)) ? null : version;
            IsCompatible = null != Sdk && !string.IsNullOrWhiteSpace(TargetFramework) && xmlDocument.DocumentElement.NamespaceURI.Length == 0;
            Collection<ProjectItem> items = new Collection<ProjectItem>();
            foreach (XmlAttribute attribute in xmlDocument.SelectNodes("/msb:Project/msb:ItemGroup/msb:*/@Include", nsmgr))
            {
                XmlElement e = (XmlElement)attribute.OwnerElement.SelectSingleNode("msb:CopyToOutputDirectory", nsmgr);
                string copyToOutputDirectory = (null == e || e.IsEmpty) ? null : e.InnerText;
                e = (XmlElement)attribute.OwnerElement.SelectSingleNode("msb:SubType", nsmgr);
                string subType = (null == e || e.IsEmpty) ? null : e.InnerText;
                e = (XmlElement)attribute.OwnerElement.SelectSingleNode("msb:DependentUpon", nsmgr);
                string dependentUpon = (null == e || e.IsEmpty) ? null : e.InnerText;
                items.Add(new ProjectItem(attribute.OwnerElement.LocalName, attribute.Value, copyToOutputDirectory, subType, dependentUpon, this));
            }
            if (IsCompatible)
                ProjectItem.Fill(projectFile.Directory, items);
            Items = new ReadOnlyCollection<ProjectItem>(items);
        }

    }
}

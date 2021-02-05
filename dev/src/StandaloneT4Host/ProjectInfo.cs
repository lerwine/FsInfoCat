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
        }

        public ProjectInfo(FileInfo projectFile)
        {
            if (projectFile is null)
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
            if (xmlDocument.DocumentElement is null)
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
        }

    }
}

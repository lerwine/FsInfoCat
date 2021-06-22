using System;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

namespace FsInfoCat.PS.Export
{
    public class Comparison : File.ComparisonBase
    {
        [XmlAttribute(nameof(TargetFileId))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_TargetFileId { get => TargetFileId.ToGuidXml(); set => TargetFileId = value.FromXmlGuid(TargetFileId); }
#pragma warning restore IDE1006 // Naming Styles
        [XmlIgnore]
        public Guid TargetFileId { get; set; }

        [XmlAttribute(nameof(AreEqual))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_AreEqual { get => AreEqual.ToBooleanXml(false); set => AreEqual = value.FromXmlBoolean(AreEqual); }
#pragma warning restore IDE1006 // Naming Styles
        [XmlIgnore]
        public bool AreEqual { get; set; }

        [XmlAttribute(nameof(ComparedOn))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_ComparedOn { get => ComparedOn.ToDateTimeXml(); set => ComparedOn = value.FromXmlDateTime(ComparedOn); }
#pragma warning restore IDE1006 // Naming Styles
        [XmlIgnore]
        public DateTime ComparedOn { get; set; }

        public File GetTargetFile()
        {
            ExportSet exportSet = SourceFile?.Parent?.Volume?.FileSystem?.ExportSet;
            if (exportSet is null)
                return null;
            Guid id = TargetFileId;
            return exportSet.FileSystems.SelectMany(fs => fs.Volumes.Select(v => v.RootDirectory)).Where(d => d is not null)
                .SelectMany(d => d.GetAllFiles()).FirstOrDefault(f => f.Id == id);
        }
    }
}

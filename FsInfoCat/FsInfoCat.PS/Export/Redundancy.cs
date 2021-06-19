using System;
using System.Xml.Serialization;

namespace FsInfoCat.PS.Export
{
    public class Redundancy : RedundantSet.RedundancyBase
    {
        private string _reference = "";
        private string _notes;

        [XmlAttribute]
        public string Reference { get => _reference; set => _reference = value.TrimmedNotNull(); }

        [XmlText]
        public string Notes { get => _notes; set => _notes = value.NullIfWhitespace(); }

        public File GetFile() => throw new NotImplementedException();
    }
}

using System.ComponentModel;
using System.Xml.Serialization;

namespace FsInfoCat.PS.Export
{
    public abstract class AccessErrorElement<TTarget> : EntityExportElement, IOwnedElement<TTarget>
        where TTarget : ExportElement
    {
        private string _message = "";
        private string _details;

        [XmlAttribute(nameof(ErrorCode))]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string __XML_ErrorCode { get => ErrorCode.ToAccessErrorCodeXml(); set => ErrorCode = value.FromAccessErrorCode(ErrorCode); }
        [XmlIgnore]
        public AccessErrorCode ErrorCode { get; set; }

        [XmlAttribute]
        public string Message { get => _message; set => _message = value.EmptyIfNullOrWhiteSpace(); }

        [XmlText]
        public string Details { get => _details; set => _details = value.NullIfWhitespace(); }

        [XmlIgnore]
        public abstract TTarget Target { get; }

        TTarget IOwnedElement<TTarget>.Owner => Target;
    }
}

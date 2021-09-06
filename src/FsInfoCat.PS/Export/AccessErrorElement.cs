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
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_ErrorCode { get => ErrorCode.ToAccessErrorCodeXml(); set => ErrorCode = value.FromAccessErrorCode(ErrorCode); }
#pragma warning restore IDE1006 // Naming Styles
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

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FsInfoCat.Desktop.Model.Local
{
    using System;
    using System.Collections.Generic;
    
    public partial class FsSymbolicName
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FsSymbolicName()
        {
            this.Notes = "\"\"";
        }
    
        public System.Guid Id { get; set; }
        public string Name { get; set; }
        public System.Guid FileSystemId { get; set; }
        public string Notes { get; set; }
        public bool IsInactive { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.DateTime ModifiedOn { get; set; }
    
        public virtual FileSystem FileSystem { get; set; }
    }
}
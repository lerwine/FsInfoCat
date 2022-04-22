using FsInfoCat;
using FsInfoCat.Local;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace DevUtil
{
    public class ILocalTagDefinitionRow
    {
        public Guid? UpstreamId { get; }
        public DateTime? LastSynchronizedOn { get; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public String Name { get; }
        public String Description { get; }
        public bool IsInactive { get; }
        public Guid Id { get; }
    }
}

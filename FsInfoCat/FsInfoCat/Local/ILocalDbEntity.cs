using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    public interface ILocalDbEntity : IDbEntity
    {
        Guid? UpstreamId { get; set; }

        DateTime? LastSynchronizedOn { get; set; }
    }
}

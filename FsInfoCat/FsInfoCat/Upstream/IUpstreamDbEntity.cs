using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsInfoCat.Upstream
{
    public interface IUpstreamDbEntity : IDbEntity
    {
        IUserProfile CreatedBy { get; set; }
        IUserProfile ModifiedBy { get; set; }
    }
}

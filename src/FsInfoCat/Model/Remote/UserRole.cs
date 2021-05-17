using System;

namespace FsInfoCat.Model.Remote
{
    [Flags]
    public enum UserRole : byte
    {
        None = 0,
        Reader = 1,
        Auditor = 2,
        Contributor = 4,
        ITSupport = 8,
        ChangeAdministrator = 16,
        AppAdministrator = 32,
        SystemAdmin = 64
    }
}

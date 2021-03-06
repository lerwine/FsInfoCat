using System.ComponentModel;
using FsInfoCat.Models.Accounts;

namespace FsInfoCat.Models.Accounts
{
    public enum UserRole : byte
    {
        [Description(EditAccount.DisplayName_Inactive)]
        None = 0,

        [Description(EditAccount.DisplayName_Viewer)]
        [AmbientValue(ModelHelper.ROLE_NAME_VIEWER)]
        Viewer = 1,

        [Description(EditAccount.DisplayName_Normal_User)]
        [AmbientValue(ModelHelper.ROLE_NAME_USER)]
        User = 2,

        [Description(EditAccount.DisplayName_App_Contributor)]
        [AmbientValue(ModelHelper.ROLE_NAME_APP_CONTRIB)]
        Crawler = 3,

        [Description(EditAccount.DisplayName_App_Administrator)]
        [AmbientValue(ModelHelper.ROLE_NAME_ADMIN)]
        Admin = 4
    }
}

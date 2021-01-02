using System.ComponentModel;

namespace FsInfoCat.Models
{
    public enum UserRole : byte
    {
        [Description(EditAccount.DisplayName_Inactive)]
        None = 0,

        [Description(EditAccount.DisplayName_Viewer)]
        [AmbientValue(ModelHelper.Role_Name_Viewer)]
        Viewer = 1,

        [Description(EditAccount.DisplayName_Normal_User)]
        [AmbientValue(ModelHelper.Role_Name_User)]
        User = 2,

        [Description(EditAccount.DisplayName_App_Contributor)]
        [AmbientValue(ModelHelper.Role_Name_App_Contrib)]
        Crawler = 3,

        [Description(EditAccount.DisplayName_App_Administrator)]
        [AmbientValue(ModelHelper.Role_Name_Admin)]
        Admin = 4
    }
}

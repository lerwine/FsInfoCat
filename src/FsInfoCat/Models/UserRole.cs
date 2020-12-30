using System.ComponentModel;

namespace FsInfoCat.Models
{
    public enum UserRole : byte
    {
        None = 0,

        [AmbientValue(ModelHelper.Role_Name_Viewer)]
        Viewer = 1,

        [AmbientValue(ModelHelper.Role_Name_User)]
        User = 2,

        [AmbientValue(ModelHelper.Role_Name_App_Contrib)]
        Crawler = 3,

        [AmbientValue(ModelHelper.Role_Name_Admin)]
        Admin = 4
    }
}

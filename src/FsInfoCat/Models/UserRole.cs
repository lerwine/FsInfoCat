using System.ComponentModel;

namespace FsInfoCat.Models
{
    public enum UserRole : byte
    {
        None = 0,
        [AmbientValue(AppUser.Role_Name_Viewer)]
        Viewer = 1,
        [AmbientValue(AppUser.Role_Name_User)]
        User = 2,
        [AmbientValue(AppUser.Role_Name_Crawler)]
        Crawler = 3,
        [AmbientValue(AppUser.Role_Name_Admin)]
        Admin = 4
    }
}

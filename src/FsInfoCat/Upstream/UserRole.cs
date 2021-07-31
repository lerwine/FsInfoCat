using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream
{
    /// <summary>
    /// <summary>Defines an application user role.</summary>
    [Flags]
    public enum UserRole : byte
    {
        /// <summary>No application roles.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_UserRole_None), ResourceType = typeof(Properties.Resources))]
        None = 0,

        /// <summary>Read-only application access.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_UserRole_Reader), ResourceType = typeof(Properties.Resources))]
        Reader = 1,

        /// <summary>Extended read-only application access for auditing purposes.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_UserRole_Auditor), ResourceType = typeof(Properties.Resources))]
        Auditor = 2,

        /// <summary>User can read and write file system crawl results for their own shared crawl results as well as read and write task records.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_UserRole_Contributor), ResourceType = typeof(Properties.Resources))]
        Contributor = 4,

        /// <summary>User can read and write all file system crawl results and task records.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_UserRole_ITSupport), ResourceType = typeof(Properties.Resources))]
        ITSupport = 8,

        /// <summary>User can assign manual remediation tasks as well as determine desired state.</summary>
        /// <remarks>This role implies the privileges of the <see cref="Contributor" /> and <see cref="ITSupport" /> roles as well.</remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_UserRole_ChangeAdministrator), ResourceType = typeof(Properties.Resources))]
        ChangeAdministrator = 16,

        /// <summary>User can make changes to most application settings.</summary>
        /// <remarks>This role implies the privileges of the <see cref="ChangeAdministrator" />, <see cref="Contributor" /> and <see cref="ITSupport" /> roles as well.</remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_UserRole_AppAdministrator), ResourceType = typeof(Properties.Resources))]
        AppAdministrator = 32,

        /// <summary>User can make changes to all application settings as well as the ability to read and write all records.</summary>
        [Display(Name = nameof(Properties.Resources.DisplayName_UserRole_SystemAdmin), ResourceType = typeof(Properties.Resources))]
        SystemAdmin = 64
    }
}

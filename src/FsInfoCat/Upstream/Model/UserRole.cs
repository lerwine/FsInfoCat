using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Defines an application user role.
    /// </summary>
    [Flags]
    public enum UserRole : byte
    {
        /// <summary>
        /// No application roles/access.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.NoAccess), ShortName = nameof(Properties.Resources.None),
            Description = nameof(Properties.Resources.Description_NoAccess), ResourceType = typeof(Properties.Resources))]
        None = 0,

        /// <summary>
        /// Read-only application access.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.ReadOnly), ShortName = nameof(Properties.Resources.ReadOnly),
            Description = nameof(Properties.Resources.Description_ReadOnly), ResourceType = typeof(Properties.Resources))]
        Reader = 1,

        /// <summary>
        /// Extended read-only application access for auditing purposes.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.ApplicationAuditor), ShortName = nameof(Properties.Resources.ApplicationAuditor),
            Description = nameof(Properties.Resources.Description_ApplicationAuditor), ResourceType = typeof(Properties.Resources))]
        Auditor = 2,

        /// <summary>
        /// User can read and write file system crawl results for their own shared crawl results as well as read and write task records.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.Contributor), ShortName = nameof(Properties.Resources.Contrib),
            Description = nameof(Properties.Resources.Description_Contributor), ResourceType = typeof(Properties.Resources))]
        Contributor = 4,

        /// <summary>
        /// User can read and write all file system crawl results and task records, including ability to reassign tasks.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.ITSupport), ShortName = nameof(Properties.Resources.IT),
            Description = nameof(Properties.Resources.Description_ITSupport), ResourceType = typeof(Properties.Resources))]
        ITSupport = 8,

        /// <summary>
        /// User can create new remediation tasks as well as determine desired state of files and directories.
        /// </summary>
        /// <remarks>This role implies the privileges of the <see cref="Contributor" /> and <see cref="ITSupport" /> roles as well.</remarks>
        [Display(Name = nameof(Properties.Resources.ChangeAdministrator), ShortName = nameof(Properties.Resources.ChangeAdmin),
            Description = nameof(Properties.Resources.Description_ChangeAdministrator), ResourceType = typeof(Properties.Resources))]
        ChangeAdministrator = 16,

        /// <summary>
        /// User can make changes to most application settings.
        /// </summary>
        /// <remarks>This role implies the privileges of the <see cref="ChangeAdministrator" />, <see cref="Contributor" /> and <see cref="ITSupport" /> roles as well.</remarks>
        [Display(Name = nameof(Properties.Resources.ApplicationAdministrator), ShortName = nameof(Properties.Resources.AppAdmin),
            Description = nameof(Properties.Resources.Description_ApplicationAdministrator), ResourceType = typeof(Properties.Resources))]
        AppAdministrator = 32,

        /// <summary>
        /// User can make changes to all application settings as well as the ability to read and write all records.
        /// </summary>
        [Display(Name = nameof(Properties.Resources.HostSystemAdministrator), ShortName = nameof(Properties.Resources.SysAdmin),
            Description = nameof(Properties.Resources.Description_HostSystemAdministrator), ResourceType = typeof(Properties.Resources))]
        SystemAdmin = 64
    }
}

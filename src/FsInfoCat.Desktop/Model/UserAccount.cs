//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FsInfoCat.Desktop.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserAccount
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserAccount()
        {
            this.IsInactive = false;
            this.Notes = "\"\"";
            this.CreatedHostDevices = new HashSet<HostDevice>();
            this.ModifiedHostDevices = new HashSet<HostDevice>();
            this.CreatedUserAccounts = new HashSet<UserAccount>();
            this.ModifiedUserAccounts = new HashSet<UserAccount>();
            this.CreatedVolumes = new HashSet<Volume>();
            this.ModifiedVolumes = new HashSet<Volume>();
            this.Memberships = new HashSet<GroupMember>();
            this.AddedGroupMembers = new HashSet<GroupMember>();
            this.CreatedUserGroups = new HashSet<UserGroup>();
            this.ModifiedUserGroups = new HashSet<UserGroup>();
            this.FilesCreated = new HashSet<File>();
            this.FilesModified = new HashSet<File>();
            this.SubdirectoriesCreated = new HashSet<Subdirectory>();
            this.SubdirectoriesModified = new HashSet<Subdirectory>();
        }
    
        public System.Guid Id { get; set; }
        public string DisplayName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MI { get; set; }
        public string Suffix { get; set; }
        public string Title { get; set; }
        public bool IsInactive { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.Guid CreatedById { get; set; }
        public System.DateTime ModifiedOn { get; set; }
        public System.Guid ModifiedById { get; set; }
        public UserRole ExplicitRoles { get; set; }
        public string Notes { get; set; }
        public Nullable<int> DbPrincipalId { get; set; }
        public byte[] SID { get; set; }
        public string LoginName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HostDevice> CreatedHostDevices { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HostDevice> ModifiedHostDevices { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserAccount> CreatedUserAccounts { get; set; }
        public virtual UserAccount CreatedBy { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserAccount> ModifiedUserAccounts { get; set; }
        public virtual UserAccount ModifiedBy { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Volume> CreatedVolumes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Volume> ModifiedVolumes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GroupMember> Memberships { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GroupMember> AddedGroupMembers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserGroup> CreatedUserGroups { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserGroup> ModifiedUserGroups { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<File> FilesCreated { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<File> FilesModified { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Subdirectory> SubdirectoriesCreated { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Subdirectory> SubdirectoriesModified { get; set; }
    }
}
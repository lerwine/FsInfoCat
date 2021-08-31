using FsInfoCat.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream
{
    public interface IUserProfileRow : IUpstreamDbEntity
    {
        /// <summary>Gets the user's display name</summary>
        /// <value>The user's display name</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_DisplayName), ResourceType = typeof(Properties.Resources))]
        string DisplayName { get; }

        /// <summary>Gets the user's first name.</summary>
        /// <value>The user's first name.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_FirstName), ResourceType = typeof(Properties.Resources))]
        string FirstName { get; }

        /// <summary>Gets the user's last name.</summary>
        /// <value>The user's last name.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_LastName), ResourceType = typeof(Properties.Resources))]
        string LastName { get; }

        /// <summary>Gets the user's middle initial.</summary>
        /// <value>The user's middle initial.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_MI), ResourceType = typeof(Properties.Resources))]
        string MI { get; }

        /// <summary>Gets the suffix that is to be appended to the user's display name.</summary>
        /// <value>The suffix that is to be appended to the user's display name.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Suffix), ResourceType = typeof(Properties.Resources))]
        string Suffix { get; }

        /// <summary>Gets the user's professional title, position or rank.</summary>
        /// <value>The user's professional title, position or rank.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Title), ResourceType = typeof(Properties.Resources))]
        string Title { get; }

        /// <summary>Gets the database principal ID for the current user profile record.</summary>
        /// <value>The database principal ID for the current user profile record.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_DbPrincipalId), ResourceType = typeof(Properties.Resources))]
        int? DbPrincipalId { get; }

        /// <summary>Gets the Security Identifier for the user associated with the current user profile record.</summary>
        /// <value>The Security Identifier for the user associated with the current user profile record.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_SID), ResourceType = typeof(Properties.Resources))]
        ByteValues SID { get; }

        /// <summary>Gets the custom notes to be associated with the current user profile.</summary>
        /// <value>The custom notes to be associated with the current user profile.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Notes), ResourceType = typeof(Properties.Resources))]
        string Notes { get; }

        /// <summary>Indicates whether the current user is inactive.</summary>
        /// <value><see langword="true" /> if the current user is inactive; otherwise, <see langword="false" />.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_IsInactive), ResourceType = typeof(Properties.Resources))]
        bool IsInactive { get; }
    }
    /// <summary>Base interface for user profile database entities.</summary>
    /// <seealso cref="IUpstreamDbEntity" />
    public interface IUserProfile : IUserProfileRow
    {
        /// <summary>Gets the membership objects that determine what groups the current user belongs to.</summary>
        /// <value>The <see cref="IGroupMembership">membership objects</see> that determine what groups the current user belongs to.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_MemberOf), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IGroupMembership> MemberOf { get; }

        /// <summary>Gets the tasks that are assigned to the current user.</summary>
        /// <value>The <see cref="IMitigationTask">tasks</see> that are assigned to the current user.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Tasks), ResourceType = typeof(Properties.Resources))]
        IEnumerable<IMitigationTask> Tasks { get; }
    }
    public interface IUserProfileListItem : IUserProfileRow
    {
        long MemberOfCount { get; }

        long TaskCount { get; }
    }
}

using FsInfoCat.Collections;
using System;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for a list item entity that associated an <see cref="IUserGroup"/> with an <see cref="IUserProfile"/> entity.
    /// </summary>
    /// <seealso cref="IGroupMembershipRow" />
    public interface IGroupMemberListItem : IGroupMembershipRow
    {
        /// <summary>
        /// Gets primary key of the associated user profile.
        /// </summary>
        /// <value>The <see cref="IHasSimpleIdentifier.Id"/> of the associated <see cref="IUserProfile"/>.</value>
        Guid UserId { get; }

        /// <summary>
        /// Gets primary key of the associated user group.
        /// </summary>
        /// <value>The <see cref="IHasSimpleIdentifier.Id"/> of the associated <see cref="IUserGroup"/>.</value>
        Guid GroupId { get; }

        /// <summary>
        /// Gets the display name of the associated user profile.
        /// </summary>
        /// <value>The <see cref="IUserProfileRow.DisplayName"/> of the associated <see cref="IUserProfile"/>.</value>
        string DisplayName { get; }

        /// <summary>
        /// Gets the first name of the associated user profile.
        /// </summary>
        /// <value>The <see cref="IUserProfileRow.FirstName"/> of the associated <see cref="IUserProfile"/>.</value>
        string FirstName { get; }

        /// <summary>
        /// Gets the last name of the associated user profile.
        /// </summary>
        /// <value>The <see cref="IUserProfileRow.LastName"/> of the associated <see cref="IUserProfile"/>.</value>
        string LastName { get; }

        /// <summary>
        /// Gets the database principal ID of the associated user profile.
        /// </summary>
        /// <value>The <see cref="IUserProfileRow.DbPrincipalId"/> of the associated <see cref="IUserProfile"/>.</value>
        int? DbPrincipalId { get; }

        /// <summary>
        /// Gets the SID of the associated user profile.
        /// </summary>
        /// <value>The <see cref="IUserProfileRow.SID"/> of the associated <see cref="IUserProfile"/>.</value>
        ByteValues SID { get; }
    }
}

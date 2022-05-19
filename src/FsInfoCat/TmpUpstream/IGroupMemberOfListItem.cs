using M = FsInfoCat.Model;
using System;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Generic interface for a list item entity that associated an <see cref="IUserProfile"/> with an <see cref="IUserGroup"/> entity.
    /// </summary>
    /// <seealso cref="IGroupMembershipRow" />
    public interface IGroupMemberOfListItem : IGroupMembershipRow
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
        /// Gets name of the associated user group.
        /// </summary>
        /// <value>The <see cref="IUserGroupRow.Name"/> of the associated <see cref="IUserGroup"/>.</value>
        string Name { get; }

        /// <summary>
        /// Gets roles for the associated user group.
        /// </summary>
        /// <value>The <see cref="IUserGroupRow.Roles"/> for the associated <see cref="IUserGroup"/>.</value>
        UserRole Roles { get; }

        /// <summary>
        /// Gets a value indicating whether the associated is user group inactive.
        /// </summary>
        /// <value><see langword="true"/> if the associated <see cref="IUserGroup"/> is inactive (archived); otherwise, <see langword="false"/> if it is active.</value>
        bool IsInactive { get; }
    }
}

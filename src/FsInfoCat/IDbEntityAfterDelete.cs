﻿using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat
{
    public interface IDbEntityAfterDelete
    {
        /// <summary>
        /// Asynchronous callback method that gets invoked after the current entity has been deleted from the database.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token that can be used to receive notice of cancellation.</param>
        /// <returns>The <see cref="Task"/> for the asynchronous post-delete activity implemented by the this callback method.</returns>
        Task AfterDeleteAsync(CancellationToken cancellationToken);
    }
}

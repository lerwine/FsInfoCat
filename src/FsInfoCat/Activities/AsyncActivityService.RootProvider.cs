using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Activities
{
    public partial class AsyncActivityService
    {
        /// <summary>
        /// The root <see cref="AsyncActivityProvider"/> (having no parent activity) for the <see cref="AsyncActivityService"/>.
        /// </summary>
        /// <seealso cref="AsyncActivityProvider" />
        sealed class RootProvider : AsyncActivityProvider
        {
            /// <summary>
            /// Gets or sets a value indicating whether any <see cref="IAsyncActivity"/> objects started by this <c>RootProvider</c> are still running.
            /// </summary>
            /// <value><see langword="true"/> if one or more <see cref="IAsyncActivity"/> objects started by this <c>RootProvider</c> are still running; otherwise, <see langword="false"/>.</value>
            internal bool IsActive { get; private set; }

            /// <summary>
            /// Gets the source object for pushing change notifications for the <see cref="IsActive"/> property.
            /// </summary>
            /// <value>The <see cref="Observable{bool}.Source"/> for pushing change notifications for the <see cref="IsActive"/> property.</value>
            internal Observable<bool>.Source ActiveStatusSource { get; } = new();

            /// <summary>
            /// Initializes a new instance of the <c>RootProvider</c> class.
            /// </summary>
            internal RootProvider() : base(Hosting.GetRequiredService<ILogger<RootProvider>>(), null) { }

            /// <summary>
            /// Notifies this <c>RootProvider</c> that an <see cref="IAsyncActivity"/> is starting.
            /// </summary>
            /// <param name="asyncActivity">The asynchronous activity that is starting.</param>
            /// <returns>The <see cref="LinkedListNode{IAsyncActivity}"/> that was appended.</returns>
            /// <exception cref="ArgumentNullException"><paramref name="asyncActivity"/> is <see langword="null"/>.</exception>
            /// <remarks>If this <c>RootProvider</c> had no running activities before the specified <paramref name="asyncActivity"/> was appened, then <see cref="IsActive"/> is set to <see langword="true"/> and a <see langword="true"/>
            /// value is pushed to <see cref="ActiveStatusSource"/>.</remarks>
            protected override LinkedListNode<IAsyncActivity> OnStarting([DisallowNull] IAsyncActivity asyncActivity)
            {
                bool isFirst;
                LinkedListNode<IAsyncActivity> node;
                Monitor.Enter(SyncRoot);
                try
                {
                    isFirst = IsEmpty();
                    node = base.OnStarting(asyncActivity);
                    if (isFirst && !IsActive)
                        try
                        {
                            IsActive = true;
                            Logger.LogDebug("Raising active status True");
                            ActiveStatusSource.RaiseNext(true);
                        }
                        catch
                        {
                            base.OnCompleted(node);
                            IsActive = false;
                            throw;
                        }
                }
                finally { Monitor.Exit(SyncRoot); }
                return node;
            }

            /// <summary>
            /// Notifies this <c>RootProvider</c> than an <see cref="IAsyncActivity"/> has been completed.
            /// </summary>
            /// <param name="node">The <see cref="LinkedListNode{IAsyncActivity}"/> to remove which references the <see cref="IAsyncActivity"/> that ran to completion, faulted, or was canceled.</param>
            /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>.</exception>
            /// <remarks>This obtains an exclusive <see cref="Monitor"/> lock on <see cref="SyncRoot"/>, removes the specified <paramref name="node"/> from the underlying list.
            /// <para>If there are no more running <see cref="IAsyncActivity"/> objects in the underlying list, then <see cref="IsActive"/> is set to <see langword="false"/> and a <see langword="false"/> value is pushed
            /// to the <see cref="ActiveStatusSource"/> object.</para></remarks>
            protected override void OnCompleted([DisallowNull] LinkedListNode<IAsyncActivity> node)
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    base.OnCompleted(node);
                    if (IsEmpty() && IsActive)
                    {
                        IsActive = false;
                        Logger.LogDebug("Raising active status False");
                        ActiveStatusSource.RaiseNext(false);
                    }
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }
    }
}

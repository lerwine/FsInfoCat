using System;

namespace FsInfoCat
{
    /// <summary>
    /// An abstraction of the windonws navigation service for dependency injection.
    /// </summary>
    public interface IApplicationNavigation
    {
        /// <summary>
        /// Gets the URI of the current content, or the URI of new content that is currently being navigated to.
        /// </summary>
        /// <value>
        /// A <see cref="Uri"/> that contains the URI for the current content, or the content that is currently being navigated to.        
        /// </value>
        public Uri Source { get; }

        /// <summary>
        /// Gets the URI of the content that was last navigated to.
        /// </summary>
        /// <value>
        /// A <see cref="Uri"/> for the content that was last navigated to, if navigated to by using a URI; otherwise, <see langword="null"/>.
        /// </value>
        public Uri CurrentSource { get; }
        
        /// <summary>
        /// Gets a reference to the object that contains the current content.
        /// </summary>
        /// <value>
        /// An object that is a reference to the object that contains the current content.
        /// </value>
        public object Content { get; }
                     
        /// <summary>
        /// Gets a value that indicates whether there is at least one entry in forward navigation history.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if there is at least one entry in forward navigation history; otherwise, <see langword="false"/>.
        /// </value>
        public bool CanGoForward { get; }

        /// <summary>
        /// Gets a value that indicates whether there is at least one entry in back navigation history.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if there is at least one entry in back navigation history; otherwise, <see langword="false"/>.
        /// </value>
        public bool CanGoBack { get; }

        /// <summary>
        /// Navigates to the most recent entry in back navigation history, if there is one.
        /// </summary>
        /// <remarks><see langword="true"/> if navigation to the most recent backward navigation entry was initiated;
        /// otherwise <see langword="false"/> if there are no entries in back navigation history.</remarks>
        public bool GoBack();
        
        /// <summary>
        /// Navigate to the most recent entry in forward navigation history, if there is one.
        /// </summary>
        /// <returns><see langword="true"/> if navigation to the most recent forward navigation entry was initiated;
        /// otherwise <see langword="false"/> if there are no entries in forward navigation history.</returns>
        public bool GoForward();

        /// <summary>
        /// Navigate asynchronously to source content located at a URI, pass an object containing navigation state for processing during navigation, and sandbox the content.
        /// </summary>
        /// <param name="source">A <see cref="Uri"/> object initialized with the URI for the desired content.</param>
        /// <param name="navigationState">An object that contains data to be used for processing during navigation.</param>
        /// <param name="sandboxExternalContent">Download content into a partial trust security sandbox (with the default Internet zone set of permissions, if <see langword="true"/>.
        /// The default is <see langword="false"/>.</param>
        /// <returns><see langword="true"/> if a navigation is not canceled; otherwise, <see langword="false"/>.</returns>
        public bool Navigate(Uri source, object navigationState, bool sandboxExternalContent);
        
        /// <summary>
        /// Navigate asynchronously to source content located at a URI, and pass an object that contains data to be used for processing during navigation.
        /// </summary>
        /// <param name="source">A <see cref="Uri"/> object initialized with the URI for the desired content.</param>
        /// <param name="navigationState">An object that contains data to be used for processing during navigation.</param>
        /// <returns><see langword="true"/> if a navigation is not canceled; otherwise, <see langword="false"/>.</returns>
        public bool Navigate(Uri source, object navigationState);
        
        /// <summary>
        ///  Navigate asynchronously to content that is specified by a URI.
        /// </summary>
        /// <param name="source">A <see cref="Uri"/> object initialized with the URI for the desired content.</param>
        /// <returns><see langword="true"/> if a navigation is not canceled; otherwise, <see langword="false"/>.</returns>
        public bool Navigate(Uri source);
        
        /// <summary>
        /// Navigate asynchronously to content that is contained by an object, and pass an object that contains data to be used for processing during navigation.
        /// </summary>
        /// <param name="root">An object that contains the content to navigate to.</param>
        /// <param name="navigationState">An object that contains data to be used for processing during navigation.</param>
        /// <returns><see langword="true"/> if a navigation is not canceled; otherwise, <see langword="false"/>.</returns>
        public bool Navigate(object root, object navigationState);
        
        /// <summary>
        /// Navigate asynchronously to content that is contained by an object.
        /// </summary>
        /// <param name="root">An object that contains the content to navigate to.</param>
        /// <returns><see langword="true"/> if a navigation is not canceled; otherwise, <see langword="false"/>.</returns>
        public bool Navigate(object root);
        
        /// <summary>
        /// Reloads the current content.
        /// </summary>
        public void Refresh();
    }
}

using System.ComponentModel;

namespace FsInfoCat.Models.Crawl.CrawlComponentModel
{
    /// <summary>
    /// Named crawl component
    /// </summary>
    public interface ICrawlComponent : IComponent
    {
        /// <summary>
        /// Gets the name to associate with the component.
        /// </summary>
        /// <returns>The name to associate with the component or null if it has no name.</returns>
        string GetName();

        /// <summary>
        /// The site object.
        /// </summary>
        new ICrawlSite Site { get; }
    }

}

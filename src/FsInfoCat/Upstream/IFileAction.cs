using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream
{
    /// <summary></summary>
    /// <seealso cref="IUpstreamDbEntity" />
    public interface IFileAction : IUpstreamDbEntity
    {
        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Task), ResourceType = typeof(Properties.Resources))]
        IMitigationTask Task { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Source), ResourceType = typeof(Properties.Resources))]
        IUpstreamFile Source { get; }

        /// <summary></summary>
        /// <value></value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Target), ResourceType = typeof(Properties.Resources))]
        IUpstreamSubdirectory Target { get; }
    }
}

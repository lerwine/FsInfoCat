using FsInfoCat.Model;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Database context for the local application database.
    /// </summary>
    /// <seealso cref="BaseDbContext" />
    /// <seealso cref="ILocalDbContext" />
    /// <seealso cref="AudioPropertiesListItem" />
    /// <seealso cref="AudioPropertySet" />
    /// <seealso cref="BinaryPropertySet" />
    /// <seealso cref="FileComparison" />
    /// <seealso cref="CrawlConfigReportItem" />
    /// <seealso cref="CrawlConfiguration" />
    /// <seealso cref="CrawlConfigListItem" />
    /// <seealso cref="CrawlJobLogListItem" />
    /// <seealso cref="CrawlJobLog" />
    /// <seealso cref="DocumentPropertiesListItem" />
    /// <seealso cref="DocumentPropertySet" />
    /// <seealso cref="DRMPropertiesListItem" />
    /// <seealso cref="DRMPropertySet" />
    /// <seealso cref="DbFile" />
    /// <seealso cref="FileAccessError" />
    /// <seealso cref="FileWithAncestorNames" />
    /// <seealso cref="FileWithBinaryProperties" />
    /// <seealso cref="FileWithBinaryPropertiesAndAncestorNames" />
    /// <seealso cref="FileSystem" />
    /// <seealso cref="FileSystemListItem" />
    /// <seealso cref="GPSPropertiesListItem" />
    /// <seealso cref="GPSPropertySet" />
    /// <seealso cref="ImagePropertiesListItem" />
    /// <seealso cref="ImagePropertySet" />
    /// <seealso cref="MediaPropertiesListItem" />
    /// <seealso cref="MediaPropertySet" />
    /// <seealso cref="MusicPropertiesListItem" />
    /// <seealso cref="MusicPropertySet" />
    /// <seealso cref="PersonalFileTag" />
    /// <seealso cref="PersonalSubdirectoryTag" />
    /// <seealso cref="PersonalTagDefinition" />
    /// <seealso cref="PersonalVolumeTag" />
    /// <seealso cref="PhotoPropertiesListItem" />
    /// <seealso cref="PhotoPropertySet" />
    /// <seealso cref="RecordedTVPropertiesListItem" />
    /// <seealso cref="RecordedTVPropertySet" />
    /// <seealso cref="Redundancy" />
    /// <seealso cref="RedundantSet" />
    /// <seealso cref="RedundantSetListItem" />
    /// <seealso cref="SharedFileTag" />
    /// <seealso cref="SharedSubdirectoryTag" />
    /// <seealso cref="SharedTagDefinition" />
    /// <seealso cref="SharedVolumeTag" />
    /// <seealso cref="Subdirectory" />
    /// <seealso cref="SubdirectoryAccessError" />
    /// <seealso cref="Model.SubdirectoryAncestorNames" />
    /// <seealso cref="SubdirectoryListItem" />
    /// <seealso cref="SubdirectoryListItemWithAncestorNames" />
    /// <seealso cref="SummaryPropertiesListItem" />
    /// <seealso cref="SummaryPropertySet" />
    /// <seealso cref="SymbolicName" />
    /// <seealso cref="SymbolicNameListItem" />
    /// <seealso cref="VideoPropertiesListItem" />
    /// <seealso cref="VideoPropertySet" />
    /// <seealso cref="Volume" />
    /// <seealso cref="VolumeAccessError" />
    /// <seealso cref="VolumeListItem" />
    /// <seealso cref="VolumeListItemWithFileSystem" />
    public partial class LocalDbContext : BaseDbContext, ILocalDbContext
    {
        private static readonly object _syncRoot = new();
        private static bool _connectionStringValidated;
        private readonly ILogger<LocalDbContext> _logger;

        /// <summary>
        /// Filesystem entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="FileSystem" /> entities from the FileSystem table in the local database.</value>
        /// <summary>
        /// FileSystem entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="FileSystem" /> entities from the <c>FileSystems</c> table in the local database.</value>
        public virtual DbSet<FileSystem> FileSystems { get; set; }

        /// <summary>
        /// SymbolicName entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="SymbolicName" /> entities from the <c>SymbolicNames</c> table in the local database.</value>
        public virtual DbSet<SymbolicName> SymbolicNames { get; set; }

        /// <summary>
        /// Volume entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="Volume" /> entities from the <c>Volumes</c> table in the local database.</value>
        public virtual DbSet<Volume> Volumes { get; set; }

        /// <summary>
        /// VolumeAccessError entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="VolumeAccessError" /> entities from the <c>VolumeAccessErrors</c> table in the local database.</value>
        public virtual DbSet<VolumeAccessError> VolumeAccessErrors { get; set; }

        /// <summary>
        /// Subdirectory entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="Subdirectory" /> entities from the <c>Subdirectories</c> table in the local database.</value>
        public virtual DbSet<Subdirectory> Subdirectories { get; set; }

        /// <summary>
        /// SubdirectoryAccessError entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="SubdirectoryAccessError" /> entities from the <c>SubdirectoryAccessErrors</c> table in the local database.</value>
        public virtual DbSet<SubdirectoryAccessError> SubdirectoryAccessErrors { get; set; }

        /// <summary>
        /// DbFile entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="DbFile" /> entities from the <c>Files</c> table in the local database.</value>
        public virtual DbSet<DbFile> Files { get; set; }

        /// <summary>
        /// FileAccessError entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="FileAccessError" /> entities from the <c>FileAccessErrors</c> table in the local database.</value>
        public virtual DbSet<FileAccessError> FileAccessErrors { get; set; }

        /// <summary>
        /// SummaryPropertySet entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="SummaryPropertySet" /> entities from the <c>SummaryPropertySets</c> table in the local database.</value>
        public virtual DbSet<SummaryPropertySet> SummaryPropertySets { get; set; }

        /// <summary>
        /// DocumentPropertySet entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="DocumentPropertySet" /> entities from the <c>DocumentPropertySets</c> table in the local database.</value>
        public virtual DbSet<DocumentPropertySet> DocumentPropertySets { get; set; }

        /// <summary>
        /// AudioPropertySet entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="AudioPropertySet" /> entities from the <c>AudioPropertySets</c> table in the local database.</value>
        public virtual DbSet<AudioPropertySet> AudioPropertySets { get; set; }

        /// <summary>
        /// DRMPropertySet entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="DRMPropertySet" /> entities from the <c>DRMPropertySets</c> table in the local database.</value>
        public virtual DbSet<DRMPropertySet> DRMPropertySets { get; set; }

        /// <summary>
        /// GPSPropertySet entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="GPSPropertySet" /> entities from the <c>GPSPropertySets</c> table in the local database.</value>
        public virtual DbSet<GPSPropertySet> GPSPropertySets { get; set; }

        /// <summary>
        /// ImagePropertySet entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="ImagePropertySet" /> entities from the <c>ImagePropertySets</c> table in the local database.</value>
        public virtual DbSet<ImagePropertySet> ImagePropertySets { get; set; }

        /// <summary>
        /// MediaPropertySet entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="MediaPropertySet" /> entities from the <c>MediaPropertySets</c> table in the local database.</value>
        public virtual DbSet<MediaPropertySet> MediaPropertySets { get; set; }

        /// <summary>
        /// MusicPropertySet entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="MusicPropertySet" /> entities from the <c>MusicPropertySets</c> table in the local database.</value>
        public virtual DbSet<MusicPropertySet> MusicPropertySets { get; set; }

        /// <summary>
        /// PhotoPropertySet entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="PhotoPropertySet" /> entities from the <c>PhotoPropertySets</c> table in the local database.</value>
        public virtual DbSet<PhotoPropertySet> PhotoPropertySets { get; set; }

        /// <summary>
        /// RecordedTVPropertySet entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="RecordedTVPropertySet" /> entities from the <c>RecordedTVPropertySets</c> table in the local database.</value>
        public virtual DbSet<RecordedTVPropertySet> RecordedTVPropertySets { get; set; }

        /// <summary>
        /// VideoPropertySet entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="VideoPropertySet" /> entities from the <c>VideoPropertySets</c> table in the local database.</value>
        public virtual DbSet<VideoPropertySet> VideoPropertySets { get; set; }

        /// <summary>
        /// BinaryPropertySet entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="BinaryPropertySet" /> entities from the <c>BinaryPropertySets</c> table in the local database.</value>
        public virtual DbSet<BinaryPropertySet> BinaryPropertySets { get; set; }

        /// <summary>
        /// PersonalTagDefinition entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="PersonalTagDefinition" /> entities from the <c>PersonalTagDefinitions</c> table in the local database.</value>
        public virtual DbSet<PersonalTagDefinition> PersonalTagDefinitions { get; set; }

        /// <summary>
        /// PersonalFileTag entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="PersonalFileTag" /> entities from the <c>PersonalFileTags</c> table in the local database.</value>
        public virtual DbSet<PersonalFileTag> PersonalFileTags { get; set; }

        /// <summary>
        /// PersonalSubdirectoryTag entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="PersonalSubdirectoryTag" /> entities from the <c>PersonalSubdirectoryTags</c> table in the local database.</value>
        public virtual DbSet<PersonalSubdirectoryTag> PersonalSubdirectoryTags { get; set; }

        /// <summary>
        /// PersonalVolumeTag entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="PersonalVolumeTag" /> entities from the <c>PersonalVolumeTags</c> table in the local database.</value>
        public virtual DbSet<PersonalVolumeTag> PersonalVolumeTags { get; set; }

        /// <summary>
        /// SharedTagDefinition entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="SharedTagDefinition" /> entities from the <c>SharedTagDefinitions</c> table in the local database.</value>
        public virtual DbSet<SharedTagDefinition> SharedTagDefinitions { get; set; }

        /// <summary>
        /// SharedFileTag entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="SharedFileTag" /> entities from the <c>SharedFileTags</c> table in the local database.</value>
        public virtual DbSet<SharedFileTag> SharedFileTags { get; set; }

        /// <summary>
        /// SharedSubdirectoryTag entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="SharedSubdirectoryTag" /> entities from the <c>SharedSubdirectoryTags</c> table in the local database.</value>
        public virtual DbSet<SharedSubdirectoryTag> SharedSubdirectoryTags { get; set; }

        /// <summary>
        /// SharedVolumeTag entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="SharedVolumeTag" /> entities from the <c>SharedVolumeTags</c> table in the local database.</value>
        public virtual DbSet<SharedVolumeTag> SharedVolumeTags { get; set; }

        /// <summary>
        /// FileComparison entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="FileComparison" /> entities from the <c>Comparisons</c> table in the local database.</value>
        public virtual DbSet<FileComparison> Comparisons { get; set; }

        /// <summary>
        /// RedundantSet entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="RedundantSet" /> entities from the <c>RedundantSets</c> table in the local database.</value>
        public virtual DbSet<RedundantSet> RedundantSets { get; set; }

        /// <summary>
        /// Redundancy entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="Redundancy" /> entities from the <c>Redundancies</c> table in the local database.</value>
        public virtual DbSet<Redundancy> Redundancies { get; set; }

        /// <summary>
        /// CrawlConfiguration entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="CrawlConfiguration" /> entities from the <c>CrawlConfigurations</c> table in the local database.</value>
        public virtual DbSet<CrawlConfiguration> CrawlConfigurations { get; set; }

        /// <summary>
        /// CrawlJobLog entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="CrawlJobLog" /> entities from the <c>CrawlJobLogs</c> table in the local database.</value>
        public virtual DbSet<CrawlJobLog> CrawlJobLogs { get; set; }

        /// <summary>
        /// SymbolicNameListItem entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="SymbolicNameListItem" /> entities from the <see cref="SymbolicNameListItem.VIEW_NAME">vSymbolicNameListing</see> view in the local database.</value>
        public virtual DbSet<SymbolicNameListItem> SymbolicNameListing { get; set; }

        /// <summary>
        /// FileSystemListItem entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="FileSystemListItem" /> entities from the <see cref="FileSystemListItem.VIEW_NAME">vFileSystemListing</see> view in the local database.</value>
        public virtual DbSet<FileSystemListItem> FileSystemListing { get; set; }

        /// <summary>
        /// PersonalTagDefinitionListItem entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="PersonalTagDefinitionListItem" /> entities from the <see cref="PersonalTagDefinitionListItem.VIEW_NAME">vPersonalTagDefinitionListing</see> view in the local database.</value>
        public virtual DbSet<PersonalTagDefinitionListItem> PersonalTagDefinitionListing { get; set; }

        /// <summary>
        /// SharedTagDefinitionListItem entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="SharedTagDefinitionListItem" /> entities from the <see cref="SharedTagDefinitionListItem.VIEW_NAME">vSharedTagDefinitionListing</see> view in the local database.</value>
        public virtual DbSet<SharedTagDefinitionListItem> SharedTagDefinitionListing { get; set; }

        /// <summary>
        /// RedundantSetListItem entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="RedundantSetListItem" /> entities from the <see cref="RedundantSetListItem.VIEW_NAME">vRedundantSetListing</see> view in the local database.</value>
        public virtual DbSet<RedundantSetListItem> RedundantSetListing { get; set; }

        /// <summary>
        /// VolumeListItem entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="VolumeListItem" /> entities from the <see cref="VolumeListItem.VIEW_NAME">vVolumeListing</see> view in the local database.</value>
        public virtual DbSet<VolumeListItem> VolumeListing { get; set; }

        /// <summary>
        /// VolumeListItemWithFileSystem entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="VolumeListItemWithFileSystem" /> entities from the <see cref="VolumeListItemWithFileSystem.VIEW_NAME_WITH_FILESYSTEM">vVolumeListingWithFileSystem</see> view in the local database.</value>
        public virtual DbSet<VolumeListItemWithFileSystem> VolumeListingWithFileSystem { get; set; }

        /// <summary>
        /// SubdirectoryListItem entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="SubdirectoryListItem" /> entities from the <see cref="SubdirectoryListItem.VIEW_NAME">vSubdirectoryListing</see> view in the local database.</value>
        public virtual DbSet<SubdirectoryListItem> SubdirectoryListing { get; set; }

        /// <summary>
        /// SubdirectoryListItemWithAncestorNames entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="SubdirectoryListItemWithAncestorNames" /> entities from the <see cref="SubdirectoryListItemWithAncestorNames.VIEW_NAME_WITH_ANCESTOR_NAMES">vSubdirectoryListingWithAncestorNames</see> view in the local database.</value>
        public virtual DbSet<SubdirectoryListItemWithAncestorNames> SubdirectoryListingWithAncestorNames { get; set; }

        /// <summary>
        /// SubdirectoryAncestorNames entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="Model.SubdirectoryAncestorNames" /> entities from the <see cref="SubdirectoryAncestorNames.VIEW_NAME">vSubdirectoryAncestorNames</see> view in the local database.</value>
        public virtual DbSet<SubdirectoryAncestorNames> SubdirectoryAncestorNames { get; set; }

        /// <summary>
        /// FileWithAncestorNames entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="FileWithAncestorNames" /> entities from the <see cref="FileWithAncestorNames.VIEW_NAME">vFileListingWithAncestorNames</see> view in the local database.</value>
        public virtual DbSet<FileWithAncestorNames> FileListingWithAncestorNames { get; set; }

        /// <summary>
        /// FileWithBinaryProperties entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="FileWithBinaryProperties" /> entities from the <see cref="FileWithBinaryProperties.VIEW_NAME">vFileListingWithBinaryProperties</see> view table in the local database.</value>
        public virtual DbSet<FileWithBinaryProperties> FileListingWithBinaryProperties { get; set; }

        /// <summary>
        /// FileWithBinaryPropertiesAndAncestorNames entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="FileWithBinaryPropertiesAndAncestorNames" /> entities from the <see cref="FileWithBinaryPropertiesAndAncestorNames.VIEW_NAME">vFileListingWithBinaryPropertiesAndAncestorNames</see> view table in the local database.</value>
        public virtual DbSet<FileWithBinaryPropertiesAndAncestorNames> FileListingWithBinaryPropertiesAndAncestorNames { get; set; }

        /// <summary>
        /// CrawlConfigListItem entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="CrawlConfigListItem" /> entities from the <see cref="CrawlConfigListItem.VIEW_NAME">vCrawlConfigListing</see> view in the local database.</value>
        public virtual DbSet<CrawlConfigListItem> CrawlConfigListing { get; set; }

        /// <summary>
        /// CrawlConfigReportItem entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="CrawlConfigReportItem" /> entities from the <see cref="CrawlConfigReportItem.VIEW_NAME">vCrawlConfigReport</see> view in the local database.</value>
        public virtual DbSet<CrawlConfigReportItem> CrawlConfigReport { get; set; }

        /// <summary>
        /// CrawlJobLogListItem entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="CrawlJobLogListItem" /> entities from the <see cref="CrawlJobLogListItem.VIEW_NAME">vCrawlJobListing</see> view in the local database.</value>
        public virtual DbSet<CrawlJobLogListItem> CrawlJobListing { get; set; }

        /// <summary>
        /// SummaryPropertiesListItem entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="SummaryPropertiesListItem" /> entities from the <see cref="SummaryPropertiesListItem.VIEW_NAME">vSummaryPropertiesListing</see> view in the local database.</value>
        public virtual DbSet<SummaryPropertiesListItem> SummaryPropertiesListing { get; set; }

        /// <summary>
        /// DocumentPropertiesListItem entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="DocumentPropertiesListItem" /> entities from the <see cref="DocumentPropertiesListItem.VIEW_NAME">vDocumentPropertiesListing</see> view in the local database.</value>
        public virtual DbSet<DocumentPropertiesListItem> DocumentPropertiesListing { get; set; }

        /// <summary>
        /// AudioPropertiesListItem entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="AudioPropertiesListItem" /> entities from the <see cref="AudioPropertiesListItem.VIEW_NAME">vAudioPropertiesListing</see> view in the local database.</value>
        public virtual DbSet<AudioPropertiesListItem> AudioPropertiesListing { get; set; }

        /// <summary>
        /// DRMPropertiesListItem entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="DRMPropertiesListItem" /> entities from the <see cref="DRMPropertiesListItem.VIEW_NAME">vDRMPropertiesListing</see> view in the local database.</value>
        public virtual DbSet<DRMPropertiesListItem> DRMPropertiesListing { get; set; }

        /// <summary>
        /// GPSPropertiesListItem entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="GPSPropertiesListItem" /> entities from the <see cref="GPSPropertiesListItem.VIEW_NAME">vGPSPropertiesListing</see> view in the local database.</value>
        public virtual DbSet<GPSPropertiesListItem> GPSPropertiesListing { get; set; }

        /// <summary>
        /// ImagePropertiesListItem entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="ImagePropertiesListItem" /> entities from the <see cref="ImagePropertiesListItem.VIEW_NAME">vImagePropertiesListing</see> view in the local database.</value>
        public virtual DbSet<ImagePropertiesListItem> ImagePropertiesListing { get; set; }

        /// <summary>
        /// MediaPropertiesListItem entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="MediaPropertiesListItem" /> entities from the <see cref="MediaPropertiesListItem.VIEW_NAME">vMediaPropertiesListing</see> view in the local database.</value>
        public virtual DbSet<MediaPropertiesListItem> MediaPropertiesListing { get; set; }

        /// <summary>
        /// MusicPropertiesListItem entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="MusicPropertiesListItem" /> entities from the <see cref="MusicPropertiesListItem.VIEW_NAME">vMusicPropertiesListing</see> view in the local database.</value>
        public virtual DbSet<MusicPropertiesListItem> MusicPropertiesListing { get; set; }

        /// <summary>
        /// PhotoPropertiesListItem entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="PhotoPropertiesListItem" /> entities from the <see cref="PhotoPropertiesListItem.VIEW_NAME">vPhotoPropertiesListing</see> view in the local database.</value>
        public virtual DbSet<PhotoPropertiesListItem> PhotoPropertiesListing { get; set; }

        /// <summary>
        /// RecordedTVPropertiesListItem entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="RecordedTVPropertiesListItem" /> entities from the <see cref="RecordedTVPropertiesListItem.VIEW_NAME">vRecordedTVPropertiesListing</see> view in the local database.</value>
        public virtual DbSet<RecordedTVPropertiesListItem> RecordedTVPropertiesListing { get; set; }

        /// <summary>
        /// VideoPropertiesListItem entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="VideoPropertiesListItem" /> entities from the <see cref="VideoPropertiesListItem.VIEW_NAME">vVideoPropertiesListing</see> view in the local database.</value>
        public virtual DbSet<VideoPropertiesListItem> VideoPropertiesListing { get; set; }

        /// <summary>
        /// PersonalVolumeTagListItem entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="PersonalVolumeTagListItem" /> entities from the <see cref="PersonalVolumeTagListItem.VIEW_NAME">vPersonalVolumeTagListing</see> view in the local database.</value>
        public virtual DbSet<PersonalVolumeTagListItem> PersonalVolumeTagListing { get; set; }

        /// <summary>
        /// SharedVolumeTagListItem entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="SharedVolumeTagListItem" /> entities from the <see cref="SharedVolumeTagListItem.VIEW_NAME">vSharedVolumeTagListing</see> view in the local database.</value>
        public virtual DbSet<SharedVolumeTagListItem> SharedVolumeTagListing { get; set; }

        /// <summary>
        /// PersonalSubdirectoryTagListItem entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="PersonalSubdirectoryTagListItem" /> entities from the <see cref="PersonalSubdirectoryTagListItem.VIEW_NAME">vPersonalSubdirectoryTagListing</see> view in the local database.</value>
        public virtual DbSet<PersonalSubdirectoryTagListItem> PersonalSubdirectoryTagListing { get; set; }

        /// <summary>
        /// SharedSubdirectoryTagListItem entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="SharedSubdirectoryTagListItem" /> entities from the <see cref="SharedSubdirectoryTagListItem.VIEW_NAME">vSharedSubdirectoryTagListing</see> view in the local database.</value>
        public virtual DbSet<SharedSubdirectoryTagListItem> SharedSubdirectoryTagListing { get; set; }

        /// <summary>
        /// PersonalFileTagListItem entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="PersonalFileTagListItem" /> entities from the <see cref="PersonalFileTagListItem.VIEW_NAME">vPersonalFileTagListing</see> view in the local database.</value>
        public virtual DbSet<PersonalFileTagListItem> PersonalFileTagListing { get; set; }

        /// <summary>
        /// SharedFileTagListItem entity database set.
        /// </summary>
        /// <value>A <see cref="DbSet{T}" /> that can be used to query <see cref="SharedFileTagListItem" /> entities from the <see cref="SharedFileTagListItem.VIEW_NAME">vSharedFileTagListing</see> view in the local database.</value>
        public virtual DbSet<SharedFileTagListItem> SharedFileTagListing { get; set; }

        /// <summary>
        /// Instantiates a new <see cref="LocalDbContext" />.
        /// </summary>
        /// <param name="options">The options for this context.</param>
        /// <seealso cref="BaseDbContext(DbContextOptions)" />
        public LocalDbContext(DbContextOptions<LocalDbContext> options)
            : base(options)
        {
            _logger = Hosting.ServiceProvider.GetRequiredService<ILogger<LocalDbContext>>();
            lock (_syncRoot)
            {
                if (!_connectionStringValidated)
                {
                    _connectionStringValidated = true;
                    SqliteConnectionStringBuilder builder = new(Database.GetConnectionString());
                    string connectionString = builder.ConnectionString;
                    _logger.LogInformation($"Using {nameof(SqliteConnectionStringBuilder.ConnectionString)} {{{nameof(SqliteConnectionStringBuilder.ConnectionString)}}}",
                        connectionString);
                    if (!File.Exists(builder.DataSource))
                    {
                        builder.Mode = SqliteOpenMode.ReadWriteCreate;
                        _logger.LogInformation("Initializing new database");
                        using SqliteConnection connection = new(builder.ConnectionString);
                        connection.Open();
                        foreach (var element in XDocument.Parse(Properties.Resources.DbCommands).Root.Elements("DbCreation").Elements("Text"))
                        {
                            _logger.LogTrace($"{{Message}}; {nameof(SqliteCommand)}={{{nameof(SqliteCommand.CommandText)}}}",
                                element.Attributes("Message").Select(a => a.Value).DefaultIfEmpty("").First(), element.Value);
                            using SqliteCommand command = connection.CreateCommand();
                            command.CommandText = element.Value;
                            command.CommandType = System.Data.CommandType.Text;
                            try { _ = command.ExecuteNonQuery(); }
                            catch (Exception exception)
                            {
#pragma warning disable CA2201 // Exception type System.Exception is not sufficiently specific
                                throw new Exception($"Error executing query '{element.Value}': {exception.Message}");
#pragma warning restore CA2201 // Exception type System.Exception is not sufficiently specific
                            }
                        }
                    }
                }
            }
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        [SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "Inherited class will have called SuppressFinalize if necessary.")]
        public override void Dispose()
        {
            _logger.LogInformation($"Disposing {nameof(LocalDbContext)}: {nameof(DbContextId.InstanceId)}={{{nameof(DbContextId.InstanceId)}}}, {nameof(DbContextId.Lease)}={{{nameof(DbContextId.Lease)}}}",
                ContextId.InstanceId, ContextId.Lease);
            base.Dispose();
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        /// <summary>
        /// Configures the data model.
        /// </summary>
        /// <param name="modelBuilder"> The builder being used to construct the model for this context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _ = modelBuilder//.HasDefaultSchema("dbo")
                .Entity<SymbolicName>(SymbolicName.OnBuildEntity)
                .Entity<Volume>(Volume.OnBuildEntity)
                .Entity<Subdirectory>(Subdirectory.OnBuildEntity)
                .Entity<CrawlConfiguration>(CrawlConfiguration.OnBuildEntity)
                .Entity<CrawlJobLog>(CrawlJobLog.OnBuildEntity)
                .Entity<DbFile>(DbFile.OnBuildEntity)
                .Entity<BinaryPropertySet>(BinaryPropertySet.OnBuildEntity)
                .Entity<FileComparison>(FileComparison.OnBuildEntity)
                .Entity<RedundantSet>(RedundantSet.OnBuildEntity)
                .Entity<Redundancy>(Redundancy.OnBuildEntity)
                .Entity<SummaryPropertySet>(SummaryPropertySet.OnBuildEntity)
                .Entity<DocumentPropertySet>(DocumentPropertySet.OnBuildEntity)
                .Entity<GPSPropertySet>(GPSPropertySet.OnBuildEntity)
                .Entity<MediaPropertySet>(MediaPropertySet.OnBuildEntity)
                .Entity<MusicPropertySet>(MusicPropertySet.OnBuildEntity)
                .Entity<PhotoPropertySet>(PhotoPropertySet.OnBuildEntity)
                .Entity<VideoPropertySet>(VideoPropertySet.OnBuildEntity)
                .Entity<PersonalFileTag>(PersonalFileTag.OnBuildEntity)
                .Entity<PersonalSubdirectoryTag>(PersonalSubdirectoryTag.OnBuildEntity)
                .Entity<PersonalVolumeTag>(PersonalVolumeTag.OnBuildEntity)
                .Entity<SharedFileTag>(SharedFileTag.OnBuildEntity)
                .Entity<SharedSubdirectoryTag>(SharedSubdirectoryTag.OnBuildEntity)
                .Entity<SharedVolumeTag>(SharedVolumeTag.OnBuildEntity)
                .Entity<FileAccessError>(FileAccessError.OnBuildEntity)
                .Entity<SubdirectoryAccessError>(SubdirectoryAccessError.OnBuildEntity)
                .Entity<VolumeAccessError>(VolumeAccessError.OnBuildEntity)
                .Entity<FileSystemListItem>(FileSystemListItem.OnBuildEntity)
                .Entity<SymbolicNameListItem>(SymbolicNameListItem.OnBuildEntity)
                .Entity<PersonalTagDefinitionListItem>(PersonalTagDefinitionListItem.OnBuildEntity)
                .Entity<SharedTagDefinitionListItem>(SharedTagDefinitionListItem.OnBuildEntity)
                .Entity<VolumeListItem>(VolumeListItem.OnBuildEntity)
                .Entity<VolumeListItemWithFileSystem>(VolumeListItemWithFileSystem.OnBuildEntity)
                .Entity<SubdirectoryListItem>(SubdirectoryListItem.OnBuildEntity)
                .Entity<SubdirectoryListItemWithAncestorNames>(SubdirectoryListItemWithAncestorNames.OnBuildEntity)
                .Entity<SubdirectoryAncestorNames>(Local.Model.SubdirectoryAncestorNames.OnBuildEntity)
                .Entity<CrawlConfigListItem>(CrawlConfigListItem.OnBuildEntity)
                .Entity<CrawlConfigReportItem>(CrawlConfigReportItem.OnBuildEntity)
                .Entity<CrawlJobLogListItem>(CrawlJobLogListItem.OnBuildEntity)
                .Entity<FileWithAncestorNames>(FileWithAncestorNames.OnBuildEntity)
                .Entity<FileWithBinaryProperties>(FileWithBinaryProperties.OnBuildEntity)
                .Entity<FileWithBinaryPropertiesAndAncestorNames>(FileWithBinaryPropertiesAndAncestorNames.OnBuildEntity)
                .Entity<RedundantSetListItem>(RedundantSetListItem.OnBuildEntity)
                .Entity<SummaryPropertiesListItem>(SummaryPropertiesListItem.OnBuildEntity)
                .Entity<DocumentPropertiesListItem>(DocumentPropertiesListItem.OnBuildEntity)
                .Entity<AudioPropertiesListItem>(AudioPropertiesListItem.OnBuildEntity)
                .Entity<DRMPropertiesListItem>(DRMPropertiesListItem.OnBuildEntity)
                .Entity<GPSPropertiesListItem>(GPSPropertiesListItem.OnBuildEntity)
                .Entity<ImagePropertiesListItem>(ImagePropertiesListItem.OnBuildEntity)
                .Entity<MediaPropertiesListItem>(MediaPropertiesListItem.OnBuildEntity)
                .Entity<MusicPropertiesListItem>(MusicPropertiesListItem.OnBuildEntity)
                .Entity<PhotoPropertiesListItem>(PhotoPropertiesListItem.OnBuildEntity)
                .Entity<RecordedTVPropertiesListItem>(RecordedTVPropertiesListItem.OnBuildEntity)
                .Entity<VideoPropertiesListItem>(VideoPropertiesListItem.OnBuildEntity)
                .Entity<PersonalVolumeTagListItem>(PersonalVolumeTagListItem.OnBuildEntity)
                .Entity<SharedVolumeTagListItem>(SharedVolumeTagListItem.OnBuildEntity)
                .Entity<PersonalSubdirectoryTagListItem>(PersonalSubdirectoryTagListItem.OnBuildEntity)
                .Entity<SharedSubdirectoryTagListItem>(SharedSubdirectoryTagListItem.OnBuildEntity)
                .Entity<PersonalFileTagListItem>(PersonalFileTagListItem.OnBuildEntity)
                .Entity<SharedFileTagListItem>(SharedFileTagListItem.OnBuildEntity);
        }

        /// <summary>
        /// Registers the <see cref="LocalDbContext" /> as a service in the <see cref="Hosting.ServiceProvider" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="assembly">The <see cref="Assembly" /> for the application that is registering the sevice.</param>
        /// <param name="dbFileName">The database file name (usually, just file name, not full path).</param>
        public static void AddDbContextPool(IServiceCollection services, Assembly assembly, string dbFileName) => AddDbContextPool(services, GetDbFilePath(assembly, dbFileName));

        /// <summary>
        /// Registers the <see cref="LocalDbContext" /> as a service in the <see cref="Hosting.ServiceProvider" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="dbPath">The path to the local database file.</param>
        public static void AddDbContextPool(IServiceCollection services, string dbPath)
        {
            string connectionString = GetConnectionString(dbPath);
            _ = services.AddDbContextPool<LocalDbContext>(options => options.AddInterceptors(new Interceptor()).UseSqlite(connectionString));
        }

        /// <summary>
        /// Creates a database connection string for a local database file.
        /// </summary>
        /// <param name="dbPath">The path to the local database file.</param>
        /// <returns>A connection string for creating a new <see cref="SqliteConnection" />.</returns>
        public static string GetConnectionString(string dbPath)
        {
            var builder = new SqliteConnectionStringBuilder
            {
                DataSource = dbPath,
                ForeignKeys = true,
                Mode = SqliteOpenMode.ReadWrite
            };
            return builder.ConnectionString;
        }

        /// <summary>
        /// Gets the full name of the application database file.
        /// </summary>
        /// <param name="assembly">The <see cref="Assembly" /> that hosts the local database.</param>
        /// <param name="dbFileName">The database file name (usually, just file name, not full path).</param>
        /// <returns>The full path to the application database file.</returns>
        public static string GetDbFilePath(Assembly assembly, string dbFileName)
        {
            if (string.IsNullOrWhiteSpace(dbFileName))
                dbFileName = Hosting.DEFAULT_LOCAL_DB_FILENAME;
            return Path.IsPathFullyQualified(dbFileName) ? Path.GetFullPath(dbFileName) : Path.Combine(Hosting.GetAppDataPath(assembly), dbFileName);
        }

        /// <summary>
        /// Asynchronously gets a related entity collection.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity containing the navigation property.</typeparam>
        /// <typeparam name="TProperty">The entity that contains the navigation property.</typeparam>
        /// <param name="entity">The <see cref="Expression{T}" /> that refers to the navigation property.</param>
        /// <param name="propertyExpression">The <see cref="Expression{T}" /> that refers to the navigation property.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task{T}" /> that returns an enumeration of related <typeparamref name="TEntity" /> entities.</returns>
        /// <example><c>IEnumerable&lt;DbFile&gt; = await dbContext.GetRelatedCollectionAsync(musicProperties, p => p.Files, cancellationToken);</c></example>
        public async Task<IEnumerable<TProperty>> GetRelatedCollectionAsync<TEntity, TProperty>([DisallowNull] TEntity entity,
            [DisallowNull] Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression, CancellationToken cancellationToken)
            where TEntity : class
            where TProperty : class => await Entry(entity).GetRelatedCollectionAsync(propertyExpression, cancellationToken);

        /// <summary>
        /// Asynchronously finds a <see cref="SummaryPropertySet" /> by property values.
        /// </summary>
        /// <param name="properties">A <see cref="IGPSProperties" /> containing the property values to match.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task{T}" /> that returns a matching <see cref="SummaryPropertySet" /> or <see langword="null" /> if not match was found.</returns>
        public async Task<SummaryPropertySet> FindMatchingAsync(ISummaryProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            string applicationName = properties.ApplicationName.TrimmedOrNullIfWhiteSpace();
            //MultiStringValue author = MultiStringValue.NullIfNotAny(properties.Author);
            // TODO: Implement FindMatchingAsync(ISummaryProperties, CancellationToken);
            return await SummaryPropertySets.FirstOrDefaultAsync(p => p.ApplicationName == applicationName, cancellationToken);
        }

        /// <summary>
        /// Asynchronously finds a <see cref="DocumentPropertySet" /> by property values.
        /// </summary>
        /// <param name="properties">A <see cref="IDocumentProperties" /> containing the property values to match.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task{T}" /> that returns a matching <see cref="DocumentPropertySet" /> or <see langword="null" /> if not match was found.</returns>
        public async Task<DocumentPropertySet> FindMatchingAsync(IDocumentProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            string clientID = properties.ClientID.TrimmedOrNullIfWhiteSpace();
            // TODO: Implement FindMatchingAsync(IDocumentProperties, CancellationToken);
            return await DocumentPropertySets.FirstOrDefaultAsync(p => p.ClientID == clientID, cancellationToken);
        }

        /// <summary>
        /// Asynchronously finds a <see cref="AudioPropertySet" /> by property values.
        /// </summary>
        /// <param name="properties">A <see cref="IAudioProperties" /> containing the property values to match.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task{T}" /> that returns a matching <see cref="AudioPropertySet" /> or <see langword="null" /> if not match was found.</returns>
        public async Task<AudioPropertySet> FindMatchingAsync(IAudioProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            string format = properties.Format.TrimmedOrNullIfWhiteSpace();
            // TODO: Implement FindMatchingAsync(IAudioProperties, CancellationToken);
            return await AudioPropertySets.FirstOrDefaultAsync(p => p.Format == format, cancellationToken);
        }

        /// <summary>
        /// Asynchronously finds a <see cref="DRMPropertySet" /> by property values.
        /// </summary>
        /// <param name="properties">A <see cref="IDRMProperties" /> containing the property values to match.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task{T}" /> that returns a matching <see cref="DRMPropertySet" /> or <see langword="null" /> if not match was found.</returns>
        public async Task<DRMPropertySet> FindMatchingAsync(IDRMProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            string description = properties.Description.TrimmedOrNullIfWhiteSpace();
            // TODO: Implement FindMatchingAsync(IDRMProperties, CancellationToken);
            return await DRMPropertySets.FirstOrDefaultAsync(p => p.Description == description, cancellationToken);
        }

        /// <summary>
        /// Asynchronously finds a <see cref="GPSPropertySet" /> by property values.
        /// </summary>
        /// <param name="properties">A <see cref="IGPSProperties" /> containing the property values to match.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task{T}" /> that returns a matching <see cref="GPSPropertySet" /> or <see langword="null" /> if not match was found.</returns>
        public async Task<GPSPropertySet> FindMatchingAsync(IGPSProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            string areaInformation = properties.AreaInformation.TrimmedOrNullIfWhiteSpace();
            // TODO: Implement FindMatchingAsync(IGPSProperties, CancellationToken);
            return await GPSPropertySets.FirstOrDefaultAsync(p => p.AreaInformation == areaInformation, cancellationToken);
        }

        /// <summary>
        /// Asynchronously finds a <see cref="ImagePropertySet" /> by property values.
        /// </summary>
        /// <param name="properties">A <see cref="IImageProperties" /> containing the property values to match.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task{T}" /> that returns a matching <see cref="ImagePropertySet" /> or <see langword="null" /> if not match was found.</returns>
        public async Task<ImagePropertySet> FindMatchingAsync(IImageProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            string imageID = properties.ImageID.TrimmedOrNullIfWhiteSpace();
            // TODO: Implement FindMatchingAsync(IImageProperties, CancellationToken);
            return await ImagePropertySets.FirstOrDefaultAsync(p => p.ImageID == imageID, cancellationToken);
        }

        /// <summary>
        /// Asynchronously finds a <see cref="MediaPropertySet" /> by property values.
        /// </summary>
        /// <param name="properties">A <see cref="IMediaProperties" /> containing the property values to match.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task{T}" /> that returns a matching <see cref="MediaPropertySet" /> or <see langword="null" /> if not match was found.</returns>
        public async Task<MediaPropertySet> FindMatchingAsync(IMediaProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            string dvdID = properties.DVDID.TrimmedOrNullIfWhiteSpace();
            // TODO: Implement FindMatchingAsync(IMediaProperties, CancellationToken);
            return await MediaPropertySets.FirstOrDefaultAsync(p => p.DVDID == dvdID, cancellationToken);
        }

        /// <summary>
        /// Asynchronously finds a <see cref="MusicPropertySet" /> by property values.
        /// </summary>
        /// <param name="properties">A <see cref="IMusicProperties" /> containing the property values to match.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task{T}" /> that returns a matching <see cref="MusicPropertySet" /> or <see langword="null" /> if not match was found.</returns>
        public async Task<MusicPropertySet> FindMatchingAsync(IMusicProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            string displayArtist = properties.DisplayArtist.TrimmedOrNullIfWhiteSpace();
            // TODO: Implement FindMatchingAsync(IMusicProperties, CancellationToken);
            return await MusicPropertySets.FirstOrDefaultAsync(p => p.DisplayArtist == displayArtist, cancellationToken);
        }

        /// <summary>
        /// Asynchronously finds a <see cref="PhotoPropertySet" /> by property values.
        /// </summary>
        /// <param name="properties">A <see cref="IPhotoProperties" /> containing the property values to match.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task{T}" /> that returns a matching <see cref="PhotoPropertySet" /> or <see langword="null" /> if not match was found.</returns>
        public async Task<PhotoPropertySet> FindMatchingAsync(IPhotoProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            string exifVersion = properties.EXIFVersion.TrimmedOrNullIfWhiteSpace();
            // TODO: Implement FindMatchingAsync(IPhotoProperties, CancellationToken);
            return await PhotoPropertySets.FirstOrDefaultAsync(p => p.EXIFVersion == exifVersion, cancellationToken);
        }

        /// <summary>
        /// Asynchronously finds a <see cref="RecordedTVPropertySet" /> by property values.
        /// </summary>
        /// <param name="properties">A <see cref="IRecordedTVProperties" /> containing the property values to match.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task{T}" /> that returns a matching <see cref="RecordedTVPropertySet" /> or <see langword="null" /> if not match was found.</returns>
        public async Task<RecordedTVPropertySet> FindMatchingAsync(IRecordedTVProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            string episodeName = properties.EpisodeName.TrimmedOrNullIfWhiteSpace();
            // TODO: Implement FindMatchingAsync(IRecordedTVProperties, CancellationToken);
            return await RecordedTVPropertySets.FirstOrDefaultAsync(p => p.EpisodeName == episodeName, cancellationToken);
        }

        /// <summary>
        /// Asynchronously finds a <see cref="VideoPropertySet" /> by property values.
        /// </summary>
        /// <param name="properties">A <see cref="IVideoProperties" /> containing the property values to match.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task{T}" /> that returns a matching <see cref="VideoPropertySet" /> or <see langword="null" /> if not match was found.</returns>
        public async Task<VideoPropertySet> FindMatchingAsync(IVideoProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            string compression = properties.Compression.NullIfWhiteSpaceOrNormalized();
            // TODO: Implement FindMatchingAsync(IVideoProperties, CancellationToken);
            return await VideoPropertySets.FirstOrDefaultAsync(p => p.Compression == compression, cancellationToken);
        }

        /// <summary>
        /// Asynchronously finds a <see cref="SummaryPropertySet" /> by property values, creating a new <see cref="SummaryPropertySet" /> if none is found.
        /// </summary>
        /// <param name="properties">A <see cref="ISummaryProperties" /> containing the property values to match.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task{T}" /> that returns a matching <see cref="SummaryPropertySet" /> or <see langword="null" /> if not match was found.</returns>
        public async Task<SummaryPropertySet> GetMatchingAsync(ISummaryProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            SummaryPropertySet result = await FindMatchingAsync(properties, cancellationToken);
            if (result is null)
            {
                // TODO: Implement GetMatchingAsync(ISummaryProperties, CancellationToken);
                throw new NotImplementedException();
            }
            return result;
        }

        /// <summary>
        /// Asynchronously finds a <see cref="DocumentPropertySet" /> by property values, creating a new <see cref="DocumentPropertySet" /> if none is found.
        /// </summary>
        /// <param name="properties">A <see cref="IDocumentProperties" /> containing the property values to match.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task{T}" /> that returns a matching <see cref="DocumentPropertySet" /> or <see langword="null" /> if not match was found.</returns>
        public async Task<DocumentPropertySet> GetMatchingAsync(IDocumentProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            DocumentPropertySet result = await FindMatchingAsync(properties, cancellationToken);
            if (result is null)
            {
                // TODO: Implement GetMatchingAsync(IDocumentProperties, CancellationToken);
                throw new NotImplementedException();
            }
            return result;
        }

        /// <summary>
        /// Asynchronously finds a <see cref="AudioPropertySet" /> by property values, creating a new <see cref="AudioPropertySet" /> if none is found.
        /// </summary>
        /// <param name="properties">A <see cref="IAudioProperties" /> containing the property values to match.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task{T}" /> that returns a matching <see cref="AudioPropertySet" /> or <see langword="null" /> if not match was found.</returns>
        public async Task<AudioPropertySet> GetMatchingAsync(IAudioProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            AudioPropertySet result = await FindMatchingAsync(properties, cancellationToken);
            if (result is null)
            {
                // TODO: Implement GetMatchingAsync(IAudioProperties, CancellationToken);
                throw new NotImplementedException();
            }
            return result;
        }

        /// <summary>
        /// Asynchronously finds a <see cref="DRMPropertySet" /> by property values, creating a new <see cref="DRMPropertySet" /> if none is found.
        /// </summary>
        /// <param name="properties">A <see cref="IDRMProperties" /> containing the property values to match.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task{T}" /> that returns a matching <see cref="DRMPropertySet" /> or <see langword="null" /> if not match was found.</returns>
        public async Task<DRMPropertySet> GetMatchingAsync(IDRMProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            DRMPropertySet result = await FindMatchingAsync(properties, cancellationToken);
            if (result is null)
            {
                // TODO: Implement GetMatchingAsync(IDRMProperties, CancellationToken);
                throw new NotImplementedException();
            }
            return result;
        }

        /// <summary>
        /// Asynchronously finds a <see cref="GPSPropertySet" /> by property values, creating a new <see cref="GPSPropertySet" /> if none is found.
        /// </summary>
        /// <param name="properties">A <see cref="IGPSProperties" /> containing the property values to match.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task{T}" /> that returns a matching <see cref="GPSPropertySet" /> or <see langword="null" /> if not match was found.</returns>
        public async Task<GPSPropertySet> GetMatchingAsync(IGPSProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            GPSPropertySet result = await FindMatchingAsync(properties, cancellationToken);
            if (result is null)
            {
                // TODO: Implement GetMatchingAsync(IGPSProperties, CancellationToken);
                throw new NotImplementedException();
            }
            return result;
        }

        /// <summary>
        /// Asynchronously finds a <see cref="ImagePropertySet" /> by property values, creating a new <see cref="ImagePropertySet" /> if none is found.
        /// </summary>
        /// <param name="properties">A <see cref="IImageProperties" /> containing the property values to match.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task{T}" /> that returns a matching <see cref="ImagePropertySet" /> or <see langword="null" /> if not match was found.</returns>
        public async Task<ImagePropertySet> GetMatchingAsync(IImageProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            ImagePropertySet result = await FindMatchingAsync(properties, cancellationToken);
            if (result is null)
            {
                // TODO: Implement GetMatchingAsync(IImageProperties, CancellationToken);
                throw new NotImplementedException();
            }
            return result;
        }

        /// <summary>
        /// Asynchronously finds a <see cref="MediaPropertySet" /> by property values, creating a new <see cref="MediaPropertySet" /> if none is found.
        /// </summary>
        /// <param name="properties">A <see cref="IMediaProperties" /> containing the property values to match.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task{T}" /> that returns a matching <see cref="MediaPropertySet" /> or <see langword="null" /> if not match was found.</returns>
        public async Task<MediaPropertySet> GetMatchingAsync(IMediaProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            MediaPropertySet result = await FindMatchingAsync(properties, cancellationToken);
            if (result is null)
            {
                // TODO: Implement GetMatchingAsync(IMediaProperties, CancellationToken);
                throw new NotImplementedException();
            }
            return result;
        }

        /// <summary>
        /// Asynchronously finds a <see cref="MusicPropertySet" /> by property values, creating a new <see cref="MusicPropertySet" /> if none is found.
        /// </summary>
        /// <param name="properties">A <see cref="IMusicProperties" /> containing the property values to match.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task{T}" /> that returns a matching <see cref="MusicPropertySet" /> or <see langword="null" /> if not match was found.</returns>
        public async Task<MusicPropertySet> GetMatchingAsync(IMusicProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            MusicPropertySet result = await FindMatchingAsync(properties, cancellationToken);
            if (result is null)
            {
                // TODO: Implement GetMatchingAsync(IMusicProperties, CancellationToken);
                throw new NotImplementedException();
            }
            return result;
        }

        /// <summary>
        /// Asynchronously finds a <see cref="PhotoPropertySet" /> by property values, creating a new <see cref="PhotoPropertySet" /> if none is found.
        /// </summary>
        /// <param name="properties">A <see cref="IPhotoProperties" /> containing the property values to match.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task{T}" /> that returns a matching <see cref="PhotoPropertySet" /> or <see langword="null" /> if not match was found.</returns>
        public async Task<PhotoPropertySet> GetMatchingAsync(IPhotoProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            PhotoPropertySet result = await FindMatchingAsync(properties, cancellationToken);
            if (result is null)
            {
                // TODO: Implement GetMatchingAsync(IPhotoProperties, CancellationToken);
                throw new NotImplementedException();
            }
            return result;
        }

        /// <summary>
        /// Asynchronously finds a <see cref="RecordedTVPropertySet" /> by property values, creating a new <see cref="RecordedTVPropertySet" /> if none is found.
        /// </summary>
        /// <param name="properties">A <see cref="IRecordedTVProperties" /> containing the property values to match.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task{T}" /> that returns a matching <see cref="RecordedTVPropertySet" /> or <see langword="null" /> if not match was found.</returns>
        public async Task<RecordedTVPropertySet> GetMatchingAsync(IRecordedTVProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            RecordedTVPropertySet result = await FindMatchingAsync(properties, cancellationToken);
            if (result is null)
            {
                // TODO: Implement GetMatchingAsync(IRecordedTVProperties, CancellationToken);
                throw new NotImplementedException();
            }
            return result;
        }

        /// <summary>
        /// Asynchronously finds a <see cref="VideoPropertySet" /> by property values, creating a new <see cref="VideoPropertySet" /> if none is found.
        /// </summary>
        /// <param name="properties">A <see cref="IVideoProperties" /> containing the property values to match.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task{T}" /> that returns a matching <see cref="VideoPropertySet" /> or <see langword="null" /> if not match was found.</returns>
        public async Task<VideoPropertySet> GetMatchingAsync(IVideoProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            VideoPropertySet result = await FindMatchingAsync(properties, cancellationToken);
            if (result is null)
            {
                // TODO: Implement GetMatchingAsync(IVideoProperties, CancellationToken);
                throw new NotImplementedException();
            }
            return result;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(BinaryPropertySet binaryProperties, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            EntityEntry<BinaryPropertySet> entry = Entry(binaryProperties);
            if ((await entry.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any() || !(await entry.GetRelatedCollectionAsync(p => p.RedundantSets, cancellationToken)).Any())
                return false;
            _ = BinaryPropertySets.Remove(binaryProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(VideoPropertySet videoProperties, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if ((await GetRelatedCollectionAsync(videoProperties, p => p.Files, cancellationToken)).Any())
                return false;
            _ = VideoPropertySets.Remove(videoProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(RecordedTVPropertySet recordedTVProperties, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if ((await GetRelatedCollectionAsync(recordedTVProperties, p => p.Files, cancellationToken)).Any())
                return false;
            _ = RecordedTVPropertySets.Remove(recordedTVProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(PhotoPropertySet photoProperties, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if ((await GetRelatedCollectionAsync(photoProperties, p => p.Files, cancellationToken)).Any())
                return false;
            _ = PhotoPropertySets.Remove(photoProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(MusicPropertySet musicProperties, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if ((await GetRelatedCollectionAsync(musicProperties, p => p.Files, cancellationToken)).Any())
                return false;
            _ = MusicPropertySets.Remove(musicProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(MediaPropertySet mediaProperties, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if ((await GetRelatedCollectionAsync(mediaProperties, p => p.Files, cancellationToken)).Any())
                return false;
            _ = MediaPropertySets.Remove(mediaProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(ImagePropertySet imageProperties, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if ((await GetRelatedCollectionAsync(imageProperties, p => p.Files, cancellationToken)).Any())
                return false;
            _ = ImagePropertySets.Remove(imageProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(GPSPropertySet gpsProperties, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if ((await GetRelatedCollectionAsync(gpsProperties, p => p.Files, cancellationToken)).Any())
                return false;
            _ = GPSPropertySets.Remove(gpsProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(DRMPropertySet drmProperties, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if ((await GetRelatedCollectionAsync(drmProperties, p => p.Files, cancellationToken)).Any())
                return false;
            _ = DRMPropertySets.Remove(drmProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(AudioPropertySet audioProperties, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if ((await GetRelatedCollectionAsync(audioProperties, p => p.Files, cancellationToken)).Any())
                return false;
            _ = AudioPropertySets.Remove(audioProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(DocumentPropertySet documentProperties, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if ((await GetRelatedCollectionAsync(documentProperties, p => p.Files, cancellationToken)).Any())
                return false;
            _ = DocumentPropertySets.Remove(documentProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(SummaryPropertySet summaryProperties, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if ((await GetRelatedCollectionAsync(summaryProperties, p => p.Files, cancellationToken)).Any())
                return false;
            _ = SummaryPropertySets.Remove(summaryProperties);
            return true;
        }

        private void ForceDeleteRedundancy(Redundancy target)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            var redundantSet = target.RedundantSet;
            _ = Redundancies.Remove(target);
            _ = SaveChanges();
            if (redundantSet.Redundancies.Count == 0)
                _ = RedundantSets.Remove(redundantSet);
        }

        #region Overrides

        /// <inheritdoc/>
        protected override IEnumerable<IComparison> GetGenericComparisons() => Comparisons;

        /// <inheritdoc/>
        protected override IEnumerable<IBinaryPropertySet> GetGenericBinaryPropertySets() => BinaryPropertySets;

        /// <inheritdoc/>
        protected override IEnumerable<ISummaryPropertySet> GetGenericSummaryPropertySets() => SummaryPropertySets;

        /// <inheritdoc/>
        protected override IEnumerable<IDocumentPropertySet> GetGenericDocumentPropertySets() => DocumentPropertySets;

        /// <inheritdoc/>
        protected override IEnumerable<IAudioPropertySet> GetGenericAudioPropertySets() => AudioPropertySets;

        /// <inheritdoc/>
        protected override IEnumerable<IDRMPropertySet> GetGenericDRMPropertySets() => DRMPropertySets;

        /// <inheritdoc/>
        protected override IEnumerable<IGPSPropertySet> GetGenericGPSPropertySets() => GPSPropertySets;

        /// <inheritdoc/>
        protected override IEnumerable<IImagePropertySet> GetGenericImagePropertySets() => ImagePropertySets;

        /// <inheritdoc/>
        protected override IEnumerable<IMediaPropertySet> GetGenericMediaPropertySets() => MediaPropertySets;

        /// <inheritdoc/>
        protected override IEnumerable<IMusicPropertySet> GetGenericMusicPropertySets() => MusicPropertySets;

        /// <inheritdoc/>
        protected override IEnumerable<IPhotoPropertySet> GetGenericPhotoPropertySets() => PhotoPropertySets;

        /// <inheritdoc/>
        protected override IEnumerable<IRecordedTVPropertySet> GetGenericRecordedTVPropertySets() => RecordedTVPropertySets;

        /// <inheritdoc/>
        protected override IEnumerable<IVideoPropertySet> GetGenericVideoPropertySets() => VideoPropertySets;

        /// <inheritdoc/>
        protected override IEnumerable<IFileAccessError> GetGenericFileAccessErrors() => FileAccessErrors;

        /// <inheritdoc/>
        protected override IEnumerable<IFile> GetGenericFiles() => Files;

        /// <inheritdoc/>
        protected override IEnumerable<IFileSystem> GetGenericFileSystems() => FileSystems;

        /// <inheritdoc/>
        protected override IEnumerable<IRedundancy> GetGenericRedundancies() => Redundancies;

        /// <inheritdoc/>
        protected override IEnumerable<IRedundantSet> GetGenericRedundantSets() => RedundantSets;

        /// <inheritdoc/>
        protected override IEnumerable<ISubdirectory> GetGenericSubdirectories() => Subdirectories;

        /// <inheritdoc/>
        protected override IEnumerable<ISubdirectoryAccessError> GetGenericSubdirectoryAccessErrors() => SubdirectoryAccessErrors;

        /// <inheritdoc/>
        protected override IEnumerable<ISymbolicName> GetGenericSymbolicNames() => SymbolicNames;

        /// <inheritdoc/>
        protected override IEnumerable<IVolumeAccessError> GetGenericVolumeAccessErrors() => VolumeAccessErrors;

        /// <inheritdoc/>
        protected override IEnumerable<IVolume> GetGenericVolumes() => Volumes;

        /// <inheritdoc/>
        protected override IEnumerable<ICrawlConfiguration> GetGenericCrawlConfigurations() => CrawlConfigurations;

        /// <inheritdoc/>
        protected override IEnumerable<IFileAccessError> GetFileAccessErrors() => FileAccessErrors;

        /// <inheritdoc/>
        protected override IEnumerable<ISubdirectoryAccessError> GetSubdirectoryAccessErrors() => SubdirectoryAccessErrors;

        /// <inheritdoc/>
        protected override IEnumerable<IVolumeAccessError> GetVolumeAccessErrors() => VolumeAccessErrors;

        /// <inheritdoc/>
        protected override IEnumerable<IPersonalTagDefinition> GetPersonalTagDefinitions() => PersonalTagDefinitions;

        /// <inheritdoc/>
        protected override IEnumerable<IPersonalFileTag> GetPersonalFileTags() => PersonalFileTags;

        /// <inheritdoc/>
        protected override IEnumerable<IPersonalSubdirectoryTag> GetPersonalSubdirectoryTags() => PersonalSubdirectoryTags;

        /// <inheritdoc/>
        protected override IEnumerable<IPersonalVolumeTag> GetPersonalVolumeTags() => PersonalVolumeTags;

        /// <inheritdoc/>
        protected override IEnumerable<ISharedTagDefinition> GetSharedTagDefinitions() => SharedTagDefinitions;

        /// <inheritdoc/>
        protected override IEnumerable<ISharedFileTag> GetSharedFileTags() => SharedFileTags;

        /// <inheritdoc/>
        protected override IEnumerable<ISharedSubdirectoryTag> GetSharedSubdirectoryTags() => SharedSubdirectoryTags;

        /// <inheritdoc/>
        protected override IEnumerable<ISharedVolumeTag> GetSharedVolumeTags() => SharedVolumeTags;

        /// <inheritdoc/>
        protected override IEnumerable<ICrawlJobLog> GetCrawlJobLogs() => CrawlJobLogs;

        /// <inheritdoc/>
        protected override IEnumerable<ISymbolicNameListItem> GetSymbolicNameListing() => SymbolicNameListing;

        /// <inheritdoc/>
        protected override IEnumerable<IFileSystemListItem> GetFileSystemListing() => FileSystemListing;

        /// <inheritdoc/>
        protected override IEnumerable<ITagDefinitionListItem> GetPersonalTagDefinitionListing() => PersonalTagDefinitionListing;

        /// <inheritdoc/>
        protected override IEnumerable<ITagDefinitionListItem> GetSharedTagDefinitionListing() => SharedTagDefinitionListing;

        /// <inheritdoc/>
        protected override IEnumerable<IRedundantSetListItem> GetRedundantSetListing() => RedundantSetListing;

        /// <inheritdoc/>
        protected override IEnumerable<IVolumeListItem> GetVolumeListing() => VolumeListing;

        /// <inheritdoc/>
        protected override IEnumerable<IVolumeListItemWithFileSystem> GetVolumeListingWithFileSystem() => VolumeListingWithFileSystem;

        /// <inheritdoc/>
        protected override IEnumerable<ISubdirectoryListItem> GetSubdirectoryListing() => SubdirectoryListing;

        /// <inheritdoc/>
        protected override IEnumerable<ISubdirectoryListItemWithAncestorNames> GetSubdirectoryListingWithAncestorNames() => SubdirectoryListingWithAncestorNames;

        /// <inheritdoc/>
        protected override IEnumerable<ISubdirectoryAncestorName> GetSubdirectoryAncestorNames() => SubdirectoryAncestorNames;

        /// <inheritdoc/>
        protected override IEnumerable<IFileListItemWithAncestorNames> GetFileListingWithAncestorNames() => FileListingWithAncestorNames;

        /// <inheritdoc/>
        protected override IEnumerable<IFileListItemWithBinaryProperties> GetFileListingWithBinaryProperties() => FileListingWithBinaryProperties;

        /// <inheritdoc/>
        protected override IEnumerable<IFileListItemWithBinaryPropertiesAndAncestorNames> GetFileListingWithBinaryPropertiesAndAncestorNames() => FileListingWithBinaryPropertiesAndAncestorNames;

        /// <inheritdoc/>
        protected override IEnumerable<ICrawlConfigurationListItem> GetCrawlConfigListing() => CrawlConfigListing;

        /// <inheritdoc/>
        protected override IEnumerable<ICrawlConfigReportItem> GetCrawlConfigReport() => CrawlConfigReport;

        /// <inheritdoc/>
        protected override IEnumerable<ICrawlJobListItem> GetCrawlJobListing() => CrawlJobListing;

        /// <inheritdoc/>
        protected override IEnumerable<ISummaryPropertiesListItem> GetSummaryPropertiesListing() => SummaryPropertiesListing;

        /// <inheritdoc/>
        protected override IEnumerable<IDocumentPropertiesListItem> GetDocumentPropertiesListing() => DocumentPropertiesListing;

        /// <inheritdoc/>
        protected override IEnumerable<IAudioPropertiesListItem> GetAudioPropertiesListing() => AudioPropertiesListing;

        /// <inheritdoc/>
        protected override IEnumerable<IDRMPropertiesListItem> GetDRMPropertiesListing() => DRMPropertiesListing;

        /// <inheritdoc/>
        protected override IEnumerable<IGPSPropertiesListItem> GetGPSPropertiesListing() => GPSPropertiesListing;

        /// <inheritdoc/>
        protected override IEnumerable<IImagePropertiesListItem> GetImagePropertiesListing() => ImagePropertiesListing;

        /// <inheritdoc/>
        protected override IEnumerable<IMediaPropertiesListItem> GetMediaPropertiesListing() => MediaPropertiesListing;

        /// <inheritdoc/>
        protected override IEnumerable<IMusicPropertiesListItem> GetMusicPropertiesListing() => MusicPropertiesListing;

        /// <inheritdoc/>
        protected override IEnumerable<IPhotoPropertiesListItem> GetPhotoPropertiesListing() => PhotoPropertiesListing;

        /// <inheritdoc/>
        protected override IEnumerable<IRecordedTVPropertiesListItem> GetRecordedTVPropertiesListing() => RecordedTVPropertiesListing;

        /// <inheritdoc/>
        protected override IEnumerable<IVideoPropertiesListItem> GetVideoPropertiesListing() => VideoPropertiesListing;

        /// <inheritdoc/>
        protected override IEnumerable<IItemTagListItem> GetPersonalVolumeTagListing() => PersonalVolumeTagListing;

        /// <inheritdoc/>
        protected override IEnumerable<IItemTagListItem> GetSharedVolumeTagListing() => SharedVolumeTagListing;

        /// <inheritdoc/>
        protected override IEnumerable<IItemTagListItem> GetPersonalSubdirectoryTagListing() => PersonalSubdirectoryTagListing;

        /// <inheritdoc/>
        protected override IEnumerable<IItemTagListItem> GetSharedSubdirectoryTagListing() => SharedSubdirectoryTagListing;

        /// <inheritdoc/>
        protected override IEnumerable<IItemTagListItem> GetPersonalFileTagListing() => PersonalFileTagListing;

        /// <inheritdoc/>
        protected override IEnumerable<IItemTagListItem> GetSharedFileTagListing() => SharedFileTagListing;

        /// <inheritdoc/>
        protected async override Task<IGPSPropertySet> FindGenericMatchingAsync(IGPSProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        /// <inheritdoc/>
        protected async override Task<IImagePropertySet> FindGenericMatchingAsync(IImageProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        /// <inheritdoc/>
        protected async override Task<IMediaPropertySet> FindGenericMatchingAsync(IMediaProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        /// <inheritdoc/>
        protected async override Task<IMusicPropertySet> FindGenericMatchingAsync(IMusicProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        /// <inheritdoc/>
        protected async override Task<IPhotoPropertySet> FindGenericMatchingAsync(IPhotoProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        /// <inheritdoc/>
        protected async override Task<IRecordedTVPropertySet> FindGenericMatchingAsync(IRecordedTVProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        /// <inheritdoc/>
        protected async override Task<IVideoPropertySet> FindGenericMatchingAsync(IVideoProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        /// <inheritdoc/>
        protected async override Task<ISummaryPropertySet> FindGenericMatchingAsync(ISummaryProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        /// <inheritdoc/>
        protected async override Task<IDocumentPropertySet> FindGenericMatchingAsync(IDocumentProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        /// <inheritdoc/>
        protected async override Task<IAudioPropertySet> FindGenericMatchingAsync(IAudioProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        /// <inheritdoc/>
        protected async override Task<IDRMPropertySet> FindGenericMatchingAsync(IDRMProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        #endregion

        #region Explicit Members

        IEnumerable<ILocalComparison> ILocalDbContext.Comparisons => Comparisons;

        IEnumerable<ILocalBinaryPropertySet> ILocalDbContext.BinaryPropertySets => BinaryPropertySets;

        IEnumerable<ILocalSummaryPropertySet> ILocalDbContext.SummaryPropertySets => SummaryPropertySets;

        IEnumerable<ILocalDocumentPropertySet> ILocalDbContext.DocumentPropertySets => DocumentPropertySets;

        IEnumerable<ILocalAudioPropertySet> ILocalDbContext.AudioPropertySets => AudioPropertySets;

        IEnumerable<ILocalDRMPropertySet> ILocalDbContext.DRMPropertySets => DRMPropertySets;

        IEnumerable<ILocalGPSPropertySet> ILocalDbContext.GPSPropertySets => GPSPropertySets;

        IEnumerable<ILocalImagePropertySet> ILocalDbContext.ImagePropertySets => ImagePropertySets;

        IEnumerable<ILocalMediaPropertySet> ILocalDbContext.MediaPropertySets => MediaPropertySets;

        IEnumerable<ILocalMusicPropertySet> ILocalDbContext.MusicPropertySets => MusicPropertySets;

        IEnumerable<ILocalPhotoPropertySet> ILocalDbContext.PhotoPropertySets => PhotoPropertySets;

        IEnumerable<ILocalRecordedTVPropertySet> ILocalDbContext.RecordedTVPropertySets => RecordedTVPropertySets;

        IEnumerable<ILocalVideoPropertySet> ILocalDbContext.VideoPropertySets => VideoPropertySets;

        IEnumerable<ILocalFileAccessError> ILocalDbContext.FileAccessErrors => FileAccessErrors;

        IEnumerable<ILocalFile> ILocalDbContext.Files => Files;

        IEnumerable<ILocalFileSystem> ILocalDbContext.FileSystems => FileSystems;

        IEnumerable<ILocalRedundancy> ILocalDbContext.Redundancies => Redundancies;

        IEnumerable<ILocalRedundantSet> ILocalDbContext.RedundantSets => RedundantSets;

        IEnumerable<ILocalSubdirectory> ILocalDbContext.Subdirectories => Subdirectories;

        IEnumerable<ILocalSubdirectoryAccessError> ILocalDbContext.SubdirectoryAccessErrors => SubdirectoryAccessErrors;

        IEnumerable<ILocalSymbolicName> ILocalDbContext.SymbolicNames => SymbolicNames;

        IEnumerable<ILocalVolumeAccessError> ILocalDbContext.VolumeAccessErrors => VolumeAccessErrors;

        IEnumerable<ILocalVolume> ILocalDbContext.Volumes => Volumes;

        IEnumerable<ILocalCrawlConfiguration> ILocalDbContext.CrawlConfigurations => CrawlConfigurations;

        IEnumerable<ILocalPersonalTagDefinition> ILocalDbContext.PersonalTagDefinitions => PersonalTagDefinitions;

        IEnumerable<ILocalPersonalFileTag> ILocalDbContext.PersonalFileTags => PersonalFileTags;

        IEnumerable<ILocalPersonalSubdirectoryTag> ILocalDbContext.PersonalSubdirectoryTags => PersonalSubdirectoryTags;

        IEnumerable<ILocalPersonalVolumeTag> ILocalDbContext.PersonalVolumeTags => PersonalVolumeTags;

        IEnumerable<ILocalSharedTagDefinition> ILocalDbContext.SharedTagDefinitions => SharedTagDefinitions;

        IEnumerable<ILocalSharedFileTag> ILocalDbContext.SharedFileTags => SharedFileTags;

        IEnumerable<ILocalSharedSubdirectoryTag> ILocalDbContext.SharedSubdirectoryTags => SharedSubdirectoryTags;

        IEnumerable<ILocalSharedVolumeTag> ILocalDbContext.SharedVolumeTags => SharedVolumeTags;

        IEnumerable<ILocalCrawlJobLog> ILocalDbContext.CrawlJobLogs => CrawlJobLogs;

        IEnumerable<ICrawlJobLog> IDbContext.CrawlJobLogs => CrawlJobLogs;

        IEnumerable<ILocalFileSystemListItem> ILocalDbContext.FileSystemListing => FileSystemListing;

        IEnumerable<ILocalTagDefinitionListItem> ILocalDbContext.PersonalTagDefinitionListing => PersonalTagDefinitionListing;

        IEnumerable<ILocalTagDefinitionListItem> ILocalDbContext.SharedTagDefinitionListing => SharedTagDefinitionListing;

        IEnumerable<ILocalRedundantSetListItem> ILocalDbContext.RedundantSetListing => RedundantSetListing;

        IEnumerable<ILocalVolumeListItem> ILocalDbContext.VolumeListing => VolumeListing;

        IEnumerable<ILocalVolumeListItemWithFileSystem> ILocalDbContext.VolumeListingWithFileSystem => VolumeListingWithFileSystem;

        IEnumerable<ILocalSubdirectoryListItem> ILocalDbContext.SubdirectoryListing => SubdirectoryListing;

        IEnumerable<ILocalSubdirectoryListItemWithAncestorNames> ILocalDbContext.SubdirectoryListingWithAncestorNames => SubdirectoryListingWithAncestorNames;

        IEnumerable<ILocalFileListItemWithAncestorNames> ILocalDbContext.FileListingWithAncestorNames => FileListingWithAncestorNames;

        IEnumerable<ILocalFileListItemWithBinaryProperties> ILocalDbContext.FileListingWithBinaryProperties => FileListingWithBinaryProperties;

        IEnumerable<ILocalFileListItemWithBinaryPropertiesAndAncestorNames> ILocalDbContext.FileListingWithBinaryPropertiesAndAncestorNames => FileListingWithBinaryPropertiesAndAncestorNames;

        IEnumerable<ILocalCrawlConfigurationListItem> ILocalDbContext.CrawlConfigListing => CrawlConfigListing;

        IEnumerable<ILocalCrawlConfigReportItem> ILocalDbContext.CrawlConfigReport => CrawlConfigReport;

        IEnumerable<ILocalCrawlJobListItem> ILocalDbContext.CrawlJobListing => CrawlJobListing;

        IEnumerable<ILocalSummaryPropertiesListItem> ILocalDbContext.SummaryPropertiesListing => SummaryPropertiesListing;

        IEnumerable<ILocalDocumentPropertiesListItem> ILocalDbContext.DocumentPropertiesListing => DocumentPropertiesListing;

        IEnumerable<ILocalAudioPropertiesListItem> ILocalDbContext.AudioPropertiesListing => AudioPropertiesListing;

        IEnumerable<ILocalDRMPropertiesListItem> ILocalDbContext.DRMPropertiesListing => DRMPropertiesListing;

        IEnumerable<ILocalGPSPropertiesListItem> ILocalDbContext.GPSPropertiesListing => GPSPropertiesListing;

        IEnumerable<ILocalImagePropertiesListItem> ILocalDbContext.ImagePropertiesListing => ImagePropertiesListing;

        IEnumerable<ILocalMediaPropertiesListItem> ILocalDbContext.MediaPropertiesListing => MediaPropertiesListing;

        IEnumerable<ILocalMusicPropertiesListItem> ILocalDbContext.MusicPropertiesListing => MusicPropertiesListing;

        IEnumerable<ILocalPhotoPropertiesListItem> ILocalDbContext.PhotoPropertiesListing => PhotoPropertiesListing;

        IEnumerable<ILocalRecordedTVPropertiesListItem> ILocalDbContext.RecordedTVPropertiesListing => RecordedTVPropertiesListing;

        IEnumerable<ILocalVideoPropertiesListItem> ILocalDbContext.VideoPropertiesListing => VideoPropertiesListing;

        IEnumerable<ILocalItemTagListItem> ILocalDbContext.PersonalVolumeTagListing => PersonalVolumeTagListing;

        IEnumerable<ILocalItemTagListItem> ILocalDbContext.SharedVolumeTagListing => SharedVolumeTagListing;

        IEnumerable<ILocalItemTagListItem> ILocalDbContext.PersonalSubdirectoryTagListing => PersonalSubdirectoryTagListing;

        IEnumerable<ILocalItemTagListItem> ILocalDbContext.SharedSubdirectoryTagListing => SharedSubdirectoryTagListing;

        IEnumerable<ILocalItemTagListItem> ILocalDbContext.PersonalFileTagListing => PersonalFileTagListing;

        IEnumerable<ILocalItemTagListItem> ILocalDbContext.SharedFileTagListing => SharedFileTagListing;

        async Task<ILocalSummaryPropertySet> ILocalDbContext.FindMatchingAsync(ISummaryProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        async Task<ILocalDocumentPropertySet> ILocalDbContext.FindMatchingAsync(IDocumentProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        async Task<ILocalAudioPropertySet> ILocalDbContext.FindMatchingAsync(IAudioProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        async Task<ILocalDRMPropertySet> ILocalDbContext.FindMatchingAsync(IDRMProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        async Task<ILocalGPSPropertySet> ILocalDbContext.FindMatchingAsync(IGPSProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        async Task<ILocalImagePropertySet> ILocalDbContext.FindMatchingAsync(IImageProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        async Task<ILocalMediaPropertySet> ILocalDbContext.FindMatchingAsync(IMediaProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        async Task<ILocalMusicPropertySet> ILocalDbContext.FindMatchingAsync(IMusicProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        async Task<ILocalPhotoPropertySet> ILocalDbContext.FindMatchingAsync(IPhotoProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        async Task<ILocalRecordedTVPropertySet> ILocalDbContext.FindMatchingAsync(IRecordedTVProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        async Task<ILocalVideoPropertySet> ILocalDbContext.FindMatchingAsync(IVideoProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        #endregion
    }
}

using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    public class FileItemVM : DbEntityItemVM<DbFile>
    {
        #region AccessErrorCount Property Members

        private static readonly DependencyPropertyKey AccessErrorCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(AccessErrorCount), typeof(int), typeof(FileItemVM),
                new PropertyMetadata(0));

        /// <summary>
        /// Identifies the <see cref="AccessErrorCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AccessErrorCountProperty = AccessErrorCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public int AccessErrorCount { get => (int)GetValue(AccessErrorCountProperty); private set => SetValue(AccessErrorCountPropertyKey, value); }

        #endregion
        #region BaselineComparisonCount Property Members

        private static readonly DependencyPropertyKey BaselineComparisonCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(BaselineComparisonCount), typeof(int), typeof(FileItemVM),
                new PropertyMetadata(0));

        /// <summary>
        /// Identifies the <see cref="BaselineComparisonCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BaselineComparisonCountProperty = BaselineComparisonCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public int BaselineComparisonCount { get => (int)GetValue(BaselineComparisonCountProperty); private set => SetValue(BaselineComparisonCountPropertyKey, value); }

        #endregion
        #region CorrelativeComparisonCount Property Members

        private static readonly DependencyPropertyKey CorrelativeComparisonCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CorrelativeComparisonCount), typeof(int), typeof(FileItemVM),
                new PropertyMetadata(0));

        /// <summary>
        /// Identifies the <see cref="CorrelativeComparisonCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CorrelativeComparisonCountProperty = CorrelativeComparisonCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public int CorrelativeComparisonCount { get => (int)GetValue(CorrelativeComparisonCountProperty); private set => SetValue(CorrelativeComparisonCountPropertyKey, value); }

        #endregion
        #region LastAccessed Property Members

        private static readonly DependencyPropertyKey LastAccessedPropertyKey = DependencyProperty.RegisterReadOnly(nameof(LastAccessed), typeof(DateTime), typeof(FileItemVM),
                new PropertyMetadata(default(DateTime)));

        /// <summary>
        /// Identifies the <see cref="LastAccessed"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LastAccessedProperty = LastAccessedPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public DateTime LastAccessed { get => (DateTime)GetValue(LastAccessedProperty); private set => SetValue(LastAccessedPropertyKey, value); }

        #endregion
        #region LastHashCalculation Property Members

        private static readonly DependencyPropertyKey LastHashCalculationPropertyKey = DependencyProperty.RegisterReadOnly(nameof(LastHashCalculation), typeof(DateTime?), typeof(FileItemVM),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="LastHashCalculation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LastHashCalculationProperty = LastHashCalculationPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public DateTime? LastHashCalculation { get => (DateTime?)GetValue(LastHashCalculationProperty); private set => SetValue(LastHashCalculationPropertyKey, value); }

        #endregion
        #region LastWriteTime Property Members

        private static readonly DependencyPropertyKey LastWriteTimePropertyKey = DependencyProperty.RegisterReadOnly(nameof(LastWriteTime), typeof(DateTime), typeof(FileItemVM),
                new PropertyMetadata(default(DateTime)));

        /// <summary>
        /// Identifies the <see cref="LastWriteTime"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LastWriteTimeProperty = LastWriteTimePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public DateTime LastWriteTime { get => (DateTime)GetValue(LastWriteTimeProperty); private set => SetValue(LastWriteTimePropertyKey, value); }

        #endregion
        #region Name Property Members

        private static readonly DependencyPropertyKey NamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(Name), typeof(string), typeof(FileItemVM), new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="Name"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NameProperty = NamePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Name { get => GetValue(NameProperty) as string; private set => SetValue(NamePropertyKey, value); }

        #endregion
        #region Notes Property Members

        private static readonly DependencyPropertyKey NotesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Notes), typeof(string), typeof(FileItemVM), new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="Notes"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NotesProperty = NotesPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Notes { get => GetValue(NotesProperty) as string; private set => SetValue(NotesPropertyKey, value); }

        #endregion
        #region CreationTime Property Members

        private static readonly DependencyPropertyKey CreationTimePropertyKey = DependencyProperty.RegisterReadOnly(nameof(CreationTime), typeof(DateTime), typeof(FileItemVM),
                new PropertyMetadata(default(DateTime)));

        /// <summary>
        /// Identifies the <see cref="CreationTime"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CreationTimeProperty = CreationTimePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public DateTime CreationTime { get => (DateTime)GetValue(CreationTimeProperty); private set => SetValue(CreationTimePropertyKey, value); }

        #endregion
        #region Options Property Members

        private static readonly DependencyPropertyKey OptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Options), typeof(FileCrawlOptions), typeof(FileItemVM),
                new PropertyMetadata(FileCrawlOptions.None));

        /// <summary>
        /// Identifies the <see cref="Options"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OptionsProperty = OptionsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public FileCrawlOptions Options { get => (FileCrawlOptions)GetValue(OptionsProperty); private set => SetValue(OptionsPropertyKey, value); }

        #endregion
        #region Status Property Members

        private static readonly DependencyPropertyKey StatusPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Status), typeof(FileCorrelationStatus), typeof(FileItemVM),
                new PropertyMetadata(FileCorrelationStatus.Dissociated));

        /// <summary>
        /// Identifies the <see cref="Status"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StatusProperty = StatusPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public FileCorrelationStatus Status { get => (FileCorrelationStatus)GetValue(StatusProperty); private set => SetValue(StatusPropertyKey, value); }

        #endregion
        #region BinaryProperties Property Members

        private static readonly DependencyPropertyKey BinaryPropertiesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(BinaryProperties), typeof(BinaryPropertySetItemVM), typeof(FileItemVM),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="BinaryProperties"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BinaryPropertiesProperty = BinaryPropertiesPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public BinaryPropertySetItemVM BinaryProperties { get => (BinaryPropertySetItemVM)GetValue(BinaryPropertiesProperty); private set => SetValue(BinaryPropertiesPropertyKey, value); }

        #endregion
        #region Parent Property Members

        private static readonly DependencyPropertyKey ParentPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Parent), typeof(SubdirectoryItemVM), typeof(FileItemVM),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Parent"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ParentProperty = ParentPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public SubdirectoryItemVM Parent { get => (SubdirectoryItemVM)GetValue(ParentProperty); private set => SetValue(ParentPropertyKey, value); }

        #endregion
        #region AudioProperties Property Members

        private static readonly DependencyPropertyKey AudioPropertiesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(AudioProperties), typeof(AudioPropertiesItemVM), typeof(FileItemVM),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="AudioProperties"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AudioPropertiesProperty = AudioPropertiesPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public AudioPropertiesItemVM AudioProperties { get => (AudioPropertiesItemVM)GetValue(AudioPropertiesProperty); private set => SetValue(AudioPropertiesPropertyKey, value); }

        #endregion
        #region DocumentProperties Property Members

        private static readonly DependencyPropertyKey DocumentPropertiesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(DocumentProperties), typeof(DocumentPropertiesItemVM), typeof(FileItemVM),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="DocumentProperties"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DocumentPropertiesProperty = DocumentPropertiesPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public DocumentPropertiesItemVM DocumentProperties { get => (DocumentPropertiesItemVM)GetValue(DocumentPropertiesProperty); private set => SetValue(DocumentPropertiesPropertyKey, value); }

        #endregion
        #region SummaryProperties Property Members

        private static readonly DependencyPropertyKey SummaryPropertiesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SummaryProperties), typeof(SummaryPropertiesItemVM), typeof(FileItemVM),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="SummaryProperties"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SummaryPropertiesProperty = SummaryPropertiesPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public SummaryPropertiesItemVM SummaryProperties { get => (SummaryPropertiesItemVM)GetValue(SummaryPropertiesProperty); private set => SetValue(SummaryPropertiesPropertyKey, value); }

        #endregion
        #region DRMProperties Property Members

        private static readonly DependencyPropertyKey DRMPropertiesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(DRMProperties), typeof(DRMPropertiesItemVM), typeof(FileItemVM),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="DRMProperties"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DRMPropertiesProperty = DRMPropertiesPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public DRMPropertiesItemVM DRMProperties { get => (DRMPropertiesItemVM)GetValue(DRMPropertiesProperty); private set => SetValue(DRMPropertiesPropertyKey, value); }

        #endregion
        #region GPSProperties Property Members

        private static readonly DependencyPropertyKey GPSPropertiesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(GPSProperties), typeof(GPSPropertiesItemVM), typeof(FileItemVM),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="GPSProperties"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty GPSPropertiesProperty = GPSPropertiesPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public GPSPropertiesItemVM GPSProperties { get => (GPSPropertiesItemVM)GetValue(GPSPropertiesProperty); private set => SetValue(GPSPropertiesPropertyKey, value); }

        #endregion
        #region ImageProperties Property Members

        private static readonly DependencyPropertyKey ImagePropertiesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ImageProperties), typeof(ImagePropertiesItemVM), typeof(FileItemVM),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ImageProperties"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ImagePropertiesProperty = ImagePropertiesPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public ImagePropertiesItemVM ImageProperties { get => (ImagePropertiesItemVM)GetValue(ImagePropertiesProperty); private set => SetValue(ImagePropertiesPropertyKey, value); }

        #endregion
        #region MediaProperties Property Members

        private static readonly DependencyPropertyKey MediaPropertiesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(MediaProperties), typeof(MediaPropertiesItemVM), typeof(FileItemVM),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="MediaProperties"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MediaPropertiesProperty = MediaPropertiesPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public MediaPropertiesItemVM MediaProperties { get => (MediaPropertiesItemVM)GetValue(MediaPropertiesProperty); private set => SetValue(MediaPropertiesPropertyKey, value); }

        #endregion
        #region MusicProperties Property Members

        private static readonly DependencyPropertyKey MusicPropertiesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(MusicProperties), typeof(MusicPropertiesItemVM), typeof(FileItemVM),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="MusicProperties"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MusicPropertiesProperty = MusicPropertiesPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public MusicPropertiesItemVM MusicProperties { get => (MusicPropertiesItemVM)GetValue(MusicPropertiesProperty); private set => SetValue(MusicPropertiesPropertyKey, value); }

        #endregion
        #region PhotoProperties Property Members

        private static readonly DependencyPropertyKey PhotoPropertiesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(PhotoProperties), typeof(PhotoPropertiesItemVM), typeof(FileItemVM),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="PhotoProperties"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PhotoPropertiesProperty = PhotoPropertiesPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public PhotoPropertiesItemVM PhotoProperties { get => (PhotoPropertiesItemVM)GetValue(PhotoPropertiesProperty); private set => SetValue(PhotoPropertiesPropertyKey, value); }

        #endregion
        #region RecordedTVProperties Property Members

        private static readonly DependencyPropertyKey RecordedTVPropertiesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(RecordedTVProperties), typeof(RecordedTVPropertiesItemVM), typeof(FileItemVM),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="RecordedTVProperties"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RecordedTVPropertiesProperty = RecordedTVPropertiesPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public RecordedTVPropertiesItemVM RecordedTVProperties { get => (RecordedTVPropertiesItemVM)GetValue(RecordedTVPropertiesProperty); private set => SetValue(RecordedTVPropertiesPropertyKey, value); }

        #endregion
        #region VideoProperties Property Members

        private static readonly DependencyPropertyKey VideoPropertiesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(VideoProperties), typeof(VideoPropertiesItemVM), typeof(FileItemVM),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="VideoProperties"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VideoPropertiesProperty = VideoPropertiesPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public VideoPropertiesItemVM VideoProperties { get => (VideoPropertiesItemVM)GetValue(VideoPropertiesProperty); private set => SetValue(VideoPropertiesPropertyKey, value); }

        #endregion

        public FileItemVM([DisallowNull] DbFile model) : base(model)
        {
            // DEFERRED: Populate tags
            BinaryProperties = model.BinaryProperties.ToItemViewModel();
            AccessErrorCount = model.AccessErrors.Count();
            BaselineComparisonCount = model.BaselineComparisons.Count();
            CorrelativeComparisonCount = model.CorrelativeComparisons.Count();
            LastAccessed = model.LastAccessed;
            LastHashCalculation = model.LastHashCalculation;
            LastWriteTime = model.LastWriteTime;
            Name = model.Name;
            Notes = model.Notes;
            CreationTime = model.CreationTime;
            Options = model.Options;
            //Parent = model.Parent.ToItemViewModel();
            Status = model.Status;
            AudioProperties = model.AudioProperties.ToItemViewModel();
            DocumentProperties = model.DocumentProperties.ToItemViewModel();
            SummaryProperties = model.SummaryProperties.ToItemViewModel();
            DRMProperties = model.DRMProperties.ToItemViewModel();
            GPSProperties = model.GPSProperties.ToItemViewModel();
            ImageProperties = model.ImageProperties.ToItemViewModel();
            MediaProperties = model.MediaProperties.ToItemViewModel();
            MusicProperties = model.MusicProperties.ToItemViewModel();
            PhotoProperties = model.PhotoProperties.ToItemViewModel();
            RecordedTVProperties = model.RecordedTVProperties.ToItemViewModel();
            VideoProperties = model.VideoProperties.ToItemViewModel();
        }

        protected override DbSet<DbFile> GetDbSet(LocalDbContext dbContext) => dbContext.Files;
        
        protected override void OnNestedModelPropertyChanged(string propertyName)
        {
            switch(propertyName)
            {
                case nameof(IFile.BinaryProperties):
                    Dispatcher.CheckInvoke(() => BinaryProperties = Model.BinaryProperties.ToItemViewModel());
                    break;
                case nameof(IFile.LastAccessed):
                    Dispatcher.CheckInvoke(() => LastAccessed = Model.LastAccessed);
                    break;
                case nameof(IFile.LastHashCalculation):
                    Dispatcher.CheckInvoke(() => LastHashCalculation = Model.LastHashCalculation);
                    break;
                case nameof(IFile.LastWriteTime):
                    Dispatcher.CheckInvoke(() => LastWriteTime = Model.LastWriteTime);
                    break;
                case nameof(IFile.Name):
                    Dispatcher.CheckInvoke(() => Name = Model.Name);
                    break;
                case nameof(IFile.Notes):
                    Dispatcher.CheckInvoke(() => Notes = Model.Notes);
                    break;
                case nameof(IFile.CreationTime):
                    Dispatcher.CheckInvoke(() => CreationTime = Model.CreationTime);
                    break;
                case nameof(IFile.Options):
                    Dispatcher.CheckInvoke(() => Options = Model.Options);
                    break;
                //case nameof(IFile.Parent):
                //    Dispatcher.CheckInvoke(() => Parent = Model.Parent.ToItemViewModel());
                //    break;
                case nameof(IFile.Status):
                    Dispatcher.CheckInvoke(() => Status = Model.Status);
                    break;
                case nameof(IFile.AudioProperties):
                    Dispatcher.CheckInvoke(() => AudioProperties = Model.AudioProperties.ToItemViewModel());
                    break;
                case nameof(IFile.DocumentProperties):
                    Dispatcher.CheckInvoke(() => DocumentProperties = Model.DocumentProperties.ToItemViewModel());
                    break;
                case nameof(IFile.SummaryProperties):
                    Dispatcher.CheckInvoke(() => SummaryProperties = Model.SummaryProperties.ToItemViewModel());
                    break;
                case nameof(IFile.DRMProperties):
                    Dispatcher.CheckInvoke(() => DRMProperties = Model.DRMProperties.ToItemViewModel());
                    break;
                case nameof(IFile.GPSProperties):
                    Dispatcher.CheckInvoke(() => GPSProperties = Model.GPSProperties.ToItemViewModel());
                    break;
                case nameof(IFile.ImageProperties):
                    Dispatcher.CheckInvoke(() => ImageProperties = Model.ImageProperties.ToItemViewModel());
                    break;
                case nameof(IFile.MediaProperties):
                    Dispatcher.CheckInvoke(() => MediaProperties = Model.MediaProperties.ToItemViewModel());
                    break;
                case nameof(IFile.MusicProperties):
                    Dispatcher.CheckInvoke(() => MusicProperties = Model.MusicProperties.ToItemViewModel());
                    break;
                case nameof(IFile.PhotoProperties):
                    Dispatcher.CheckInvoke(() => PhotoProperties = Model.PhotoProperties.ToItemViewModel());
                    break;
                case nameof(IFile.RecordedTVProperties):
                    Dispatcher.CheckInvoke(() => RecordedTVProperties = Model.RecordedTVProperties.ToItemViewModel());
                    break;
                case nameof(IFile.VideoProperties):
                    Dispatcher.CheckInvoke(() => VideoProperties = Model.VideoProperties.ToItemViewModel());
                    break;
            }
        }
    }
}

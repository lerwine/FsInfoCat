using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public abstract class SummaryPropertiesColumnVisibilityOptions<TEntity, TViewModel> : ColumnVisibilityOptionsViewModel<TEntity, TViewModel>
        where TEntity : DbEntity, ISummaryPropertiesListItem
        where TViewModel : SummaryPropertiesListItemViewModel<TEntity>
    {
        #region TotalFileCount Property Members

        /// <summary>
        /// Identifies the <see cref="TotalFileCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TotalFileCountProperty = DependencyPropertyBuilder<SummaryPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(TotalFileCount))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as SummaryPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool TotalFileCount { get => (bool)GetValue(TotalFileCountProperty); set => SetValue(TotalFileCountProperty, value); }

        #endregion
        #region ApplicationName Property Members

        /// <summary>
        /// Identifies the <see cref="ApplicationName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ApplicationNameProperty = DependencyPropertyBuilder<SummaryPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(ApplicationName))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as SummaryPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool ApplicationName { get => (bool)GetValue(ApplicationNameProperty); set => SetValue(ApplicationNameProperty, value); }

        #endregion
        #region Author Property Members

        /// <summary>
        /// Identifies the <see cref="Author"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AuthorProperty = DependencyPropertyBuilder<SummaryPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(Author))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as SummaryPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool Author { get => (bool)GetValue(AuthorProperty); set => SetValue(AuthorProperty, value); }

        #endregion
        #region Comment Property Members

        /// <summary>
        /// Identifies the <see cref="Comment"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommentProperty = DependencyPropertyBuilder<SummaryPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(Comment))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as SummaryPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool Comment { get => (bool)GetValue(CommentProperty); set => SetValue(CommentProperty, value); }

        #endregion
        #region Keywords Property Members

        /// <summary>
        /// Identifies the <see cref="Keywords"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty KeywordsProperty = DependencyPropertyBuilder<SummaryPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(Keywords))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as SummaryPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool Keywords { get => (bool)GetValue(KeywordsProperty); set => SetValue(KeywordsProperty, value); }

        #endregion
        #region Subject Property Members

        /// <summary>
        /// Identifies the <see cref="Subject"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SubjectProperty = DependencyPropertyBuilder<SummaryPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(Subject))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as SummaryPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool Subject { get => (bool)GetValue(SubjectProperty); set => SetValue(SubjectProperty, value); }

        #endregion
        #region Title Property Members

        /// <summary>
        /// Identifies the <see cref="Title"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TitleProperty = DependencyPropertyBuilder<SummaryPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(Title))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as SummaryPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool Title { get => (bool)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }

        #endregion
        #region Company Property Members

        /// <summary>
        /// Identifies the <see cref="Company"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CompanyProperty = DependencyPropertyBuilder<SummaryPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(Company))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as SummaryPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool Company { get => (bool)GetValue(CompanyProperty); set => SetValue(CompanyProperty, value); }

        #endregion
        #region ContentType Property Members

        /// <summary>
        /// Identifies the <see cref="ContentType"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentTypeProperty = DependencyPropertyBuilder<SummaryPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(ContentType))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as SummaryPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool ContentType { get => (bool)GetValue(ContentTypeProperty); set => SetValue(ContentTypeProperty, value); }

        #endregion
        #region Copyright Property Members

        /// <summary>
        /// Identifies the <see cref="Copyright"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CopyrightProperty = DependencyPropertyBuilder<SummaryPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(Copyright))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as SummaryPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool Copyright { get => (bool)GetValue(CopyrightProperty); set => SetValue(CopyrightProperty, value); }

        #endregion
        #region ParentalRating Property Members

        /// <summary>
        /// Identifies the <see cref="ParentalRating"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ParentalRatingProperty = DependencyPropertyBuilder<SummaryPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(ParentalRating))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as SummaryPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool ParentalRating { get => (bool)GetValue(ParentalRatingProperty); set => SetValue(ParentalRatingProperty, value); }

        #endregion
        #region Rating Property Members

        /// <summary>
        /// Identifies the <see cref="Rating"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RatingProperty = DependencyPropertyBuilder<SummaryPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(Rating))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as SummaryPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool Rating { get => (bool)GetValue(RatingProperty); set => SetValue(RatingProperty, value); }

        #endregion
        #region ItemAuthors Property Members

        /// <summary>
        /// Identifies the <see cref="ItemAuthors"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemAuthorsProperty = DependencyPropertyBuilder<SummaryPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(ItemAuthors))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as SummaryPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool ItemAuthors { get => (bool)GetValue(ItemAuthorsProperty); set => SetValue(ItemAuthorsProperty, value); }

        #endregion
        #region ItemType Property Members

        /// <summary>
        /// Identifies the <see cref="ItemType"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemTypeProperty = DependencyPropertyBuilder<SummaryPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(ItemType))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as SummaryPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool ItemType { get => (bool)GetValue(ItemTypeProperty); set => SetValue(ItemTypeProperty, value); }

        #endregion
        #region ItemTypeText Property Members

        /// <summary>
        /// Identifies the <see cref="ItemTypeText"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemTypeTextProperty = DependencyPropertyBuilder<SummaryPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(ItemTypeText))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as SummaryPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool ItemTypeText { get => (bool)GetValue(ItemTypeTextProperty); set => SetValue(ItemTypeTextProperty, value); }

        #endregion
        #region Kind Property Members

        /// <summary>
        /// Identifies the <see cref="Kind"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty KindProperty = DependencyPropertyBuilder<SummaryPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(Kind))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as SummaryPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool Kind { get => (bool)GetValue(KindProperty); set => SetValue(KindProperty, value); }

        #endregion
        #region MIMEType Property Members

        /// <summary>
        /// Identifies the <see cref="MIMEType"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MIMETypeProperty = DependencyPropertyBuilder<SummaryPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(MIMEType))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as SummaryPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool MIMEType { get => (bool)GetValue(MIMETypeProperty); set => SetValue(MIMETypeProperty, value); }

        #endregion
        #region ParentalRatingReason Property Members

        /// <summary>
        /// Identifies the <see cref="ParentalRatingReason"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ParentalRatingReasonProperty = DependencyPropertyBuilder<SummaryPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(ParentalRatingReason))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as SummaryPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool ParentalRatingReason { get => (bool)GetValue(ParentalRatingReasonProperty); set => SetValue(ParentalRatingReasonProperty, value); }

        #endregion
        #region ParentalRatingsOrganization Property Members

        /// <summary>
        /// Identifies the <see cref="ParentalRatingsOrganization"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ParentalRatingsOrganizationProperty = DependencyPropertyBuilder<SummaryPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(ParentalRatingsOrganization))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as SummaryPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool ParentalRatingsOrganization { get => (bool)GetValue(ParentalRatingsOrganizationProperty); set => SetValue(ParentalRatingsOrganizationProperty, value); }

        #endregion
        #region Sensitivity Property Members

        /// <summary>
        /// Identifies the <see cref="Sensitivity"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SensitivityProperty = DependencyPropertyBuilder<SummaryPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(Sensitivity))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as SummaryPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool Sensitivity { get => (bool)GetValue(SensitivityProperty); set => SetValue(SensitivityProperty, value); }

        #endregion
        #region SensitivityText Property Members

        /// <summary>
        /// Identifies the <see cref="SensitivityText"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SensitivityTextProperty = DependencyPropertyBuilder<SummaryPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(SensitivityText))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as SummaryPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool SensitivityText { get => (bool)GetValue(SensitivityTextProperty); set => SetValue(SensitivityTextProperty, value); }

        #endregion
        #region SimpleRating Property Members

        /// <summary>
        /// Identifies the <see cref="SimpleRating"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SimpleRatingProperty = DependencyPropertyBuilder<SummaryPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(SimpleRating))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as SummaryPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool SimpleRating { get => (bool)GetValue(SimpleRatingProperty); set => SetValue(SimpleRatingProperty, value); }

        #endregion
        #region Trademarks Property Members

        /// <summary>
        /// Identifies the <see cref="Trademarks"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TrademarksProperty = DependencyPropertyBuilder<SummaryPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(Trademarks))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as SummaryPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool Trademarks { get => (bool)GetValue(TrademarksProperty); set => SetValue(TrademarksProperty, value); }

        #endregion
        #region ProductName Property Members

        /// <summary>
        /// Identifies the <see cref="ProductName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ProductNameProperty = DependencyPropertyBuilder<SummaryPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(ProductName))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as SummaryPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool ProductName { get => (bool)GetValue(ProductNameProperty); set => SetValue(ProductNameProperty, value); }

        #endregion

        protected SummaryPropertiesColumnVisibilityOptions() : base() { }
    }
}

using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class SummaryPropertiesRowViewModel<TEntity> : DbEntityRowViewModel<TEntity>
        where TEntity : DbEntity, ISummaryProperties
    {
#pragma warning disable IDE0060 // Remove unused parameter
        #region ApplicationName Property Members

        /// <summary>
        /// Identifies the <see cref="ApplicationName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ApplicationNameProperty = DependencyProperty.Register(nameof(ApplicationName), typeof(string),
            typeof(SummaryPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as SummaryPropertiesRowViewModel<TEntity>)?.OnApplicationNamePropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string ApplicationName { get => GetValue(ApplicationNameProperty) as string; set => SetValue(ApplicationNameProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ApplicationName"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ApplicationName"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ApplicationName"/> property.</param>
        protected void OnApplicationNamePropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnApplicationNamePropertyChanged Logic
        }

        #endregion
        #region Comment Property Members

        /// <summary>
        /// Identifies the <see cref="Comment"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommentProperty = DependencyProperty.Register(nameof(Comment), typeof(string),
            typeof(SummaryPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as SummaryPropertiesRowViewModel<TEntity>)?.OnCommentPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Comment { get => GetValue(CommentProperty) as string; set => SetValue(CommentProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Comment"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Comment"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Comment"/> property.</param>
        protected void OnCommentPropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnCommentPropertyChanged Logic
        }

        #endregion
        #region Subject Property Members

        /// <summary>
        /// Identifies the <see cref="Subject"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SubjectProperty = DependencyProperty.Register(nameof(Subject), typeof(string),
            typeof(SummaryPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as SummaryPropertiesRowViewModel<TEntity>)?.OnSubjectPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Subject { get => GetValue(SubjectProperty) as string; set => SetValue(SubjectProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Subject"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Subject"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Subject"/> property.</param>
        protected void OnSubjectPropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnSubjectPropertyChanged Logic
        }

        #endregion
        #region Title Property Members

        /// <summary>
        /// Identifies the <see cref="Title"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(string), typeof(SummaryPropertiesRowViewModel<TEntity>),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as SummaryPropertiesRowViewModel<TEntity>)?.OnTitlePropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Title { get => GetValue(TitleProperty) as string; set => SetValue(TitleProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Title"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Title"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Title"/> property.</param>
        protected void OnTitlePropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnTitlePropertyChanged Logic
        }

        #endregion
        #region Company Property Members

        /// <summary>
        /// Identifies the <see cref="Company"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CompanyProperty = DependencyProperty.Register(nameof(Company), typeof(string),
            typeof(SummaryPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as SummaryPropertiesRowViewModel<TEntity>)?.OnCompanyPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Company { get => GetValue(CompanyProperty) as string; set => SetValue(CompanyProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Company"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Company"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Company"/> property.</param>
        protected void OnCompanyPropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnCompanyPropertyChanged Logic
        }

        #endregion
        #region ContentType Property Members

        /// <summary>
        /// Identifies the <see cref="ContentType"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentTypeProperty = DependencyProperty.Register(nameof(ContentType), typeof(string),
            typeof(SummaryPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as SummaryPropertiesRowViewModel<TEntity>)?.OnContentTypePropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string ContentType { get => GetValue(ContentTypeProperty) as string; set => SetValue(ContentTypeProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ContentType"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ContentType"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ContentType"/> property.</param>
        protected void OnContentTypePropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnContentTypePropertyChanged Logic
        }

        #endregion
        #region Copyright Property Members

        /// <summary>
        /// Identifies the <see cref="Copyright"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CopyrightProperty = DependencyProperty.Register(nameof(Copyright), typeof(string),
            typeof(SummaryPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as SummaryPropertiesRowViewModel<TEntity>)?.OnCopyrightPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Copyright { get => GetValue(CopyrightProperty) as string; set => SetValue(CopyrightProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Copyright"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Copyright"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Copyright"/> property.</param>
        protected void OnCopyrightPropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnCopyrightPropertyChanged Logic
        }

        #endregion
        #region ParentalRating Property Members

        /// <summary>
        /// Identifies the <see cref="ParentalRating"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ParentalRatingProperty = DependencyProperty.Register(nameof(ParentalRating), typeof(string),
            typeof(SummaryPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as SummaryPropertiesRowViewModel<TEntity>)?.OnParentalRatingPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string ParentalRating { get => GetValue(ParentalRatingProperty) as string; set => SetValue(ParentalRatingProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ParentalRating"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ParentalRating"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ParentalRating"/> property.</param>
        protected void OnParentalRatingPropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnParentalRatingPropertyChanged Logic
        }

        #endregion
        #region Rating Property Members

        /// <summary>
        /// Identifies the <see cref="Rating"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RatingProperty = DependencyProperty.Register(nameof(Rating), typeof(uint?), typeof(SummaryPropertiesRowViewModel<TEntity>),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as SummaryPropertiesRowViewModel<TEntity>)?.OnRatingPropertyChanged((uint?)e.OldValue, (uint?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public uint? Rating { get => (uint?)GetValue(RatingProperty); set => SetValue(RatingProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Rating"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Rating"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Rating"/> property.</param>
        protected void OnRatingPropertyChanged(uint? oldValue, uint? newValue)
        {
            // TODO: Implement OnRatingPropertyChanged Logic
        }

        #endregion
        #region ItemType Property Members

        /// <summary>
        /// Identifies the <see cref="ItemType"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemTypeProperty = DependencyProperty.Register(nameof(ItemType), typeof(string),
            typeof(SummaryPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as SummaryPropertiesRowViewModel<TEntity>)?.OnItemTypePropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string ItemType { get => GetValue(ItemTypeProperty) as string; set => SetValue(ItemTypeProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ItemType"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ItemType"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ItemType"/> property.</param>
        protected void OnItemTypePropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnItemTypePropertyChanged Logic
        }

        #endregion
        #region ItemTypeText Property Members

        /// <summary>
        /// Identifies the <see cref="ItemTypeText"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemTypeTextProperty = DependencyProperty.Register(nameof(ItemTypeText), typeof(string),
            typeof(SummaryPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as SummaryPropertiesRowViewModel<TEntity>)?.OnItemTypeTextPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string ItemTypeText { get => GetValue(ItemTypeTextProperty) as string; set => SetValue(ItemTypeTextProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ItemTypeText"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ItemTypeText"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ItemTypeText"/> property.</param>
        protected void OnItemTypeTextPropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnItemTypeTextPropertyChanged Logic
        }

        #endregion
        #region MIMEType Property Members

        /// <summary>
        /// Identifies the <see cref="MIMEType"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MIMETypeProperty = DependencyProperty.Register(nameof(MIMEType), typeof(string),
            typeof(SummaryPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as SummaryPropertiesRowViewModel<TEntity>)?.OnMIMETypePropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string MIMEType { get => GetValue(MIMETypeProperty) as string; set => SetValue(MIMETypeProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="MIMEType"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="MIMEType"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="MIMEType"/> property.</param>
        protected void OnMIMETypePropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnMIMETypePropertyChanged Logic
        }

        #endregion
        #region ParentalRatingReason Property Members

        /// <summary>
        /// Identifies the <see cref="ParentalRatingReason"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ParentalRatingReasonProperty = DependencyProperty.Register(nameof(ParentalRatingReason), typeof(string),
            typeof(SummaryPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as SummaryPropertiesRowViewModel<TEntity>)?.OnParentalRatingReasonPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string ParentalRatingReason { get => GetValue(ParentalRatingReasonProperty) as string; set => SetValue(ParentalRatingReasonProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ParentalRatingReason"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ParentalRatingReason"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ParentalRatingReason"/> property.</param>
        protected void OnParentalRatingReasonPropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnParentalRatingReasonPropertyChanged Logic
        }

        #endregion
        #region ParentalRatingsOrganization Property Members

        /// <summary>
        /// Identifies the <see cref="ParentalRatingsOrganization"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ParentalRatingsOrganizationProperty = DependencyProperty.Register(nameof(ParentalRatingsOrganization), typeof(string),
            typeof(SummaryPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as SummaryPropertiesRowViewModel<TEntity>)?.OnParentalRatingsOrganizationPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string ParentalRatingsOrganization { get => GetValue(ParentalRatingsOrganizationProperty) as string; set => SetValue(ParentalRatingsOrganizationProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ParentalRatingsOrganization"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ParentalRatingsOrganization"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ParentalRatingsOrganization"/> property.</param>
        protected void OnParentalRatingsOrganizationPropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnParentalRatingsOrganizationPropertyChanged Logic
        }

        #endregion
        #region Sensitivity Property Members

        /// <summary>
        /// Identifies the <see cref="Sensitivity"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SensitivityProperty = DependencyProperty.Register(nameof(Sensitivity), typeof(ushort?), typeof(SummaryPropertiesRowViewModel<TEntity>),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as SummaryPropertiesRowViewModel<TEntity>)?.OnSensitivityPropertyChanged((ushort?)e.OldValue, (ushort?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public ushort? Sensitivity { get => (ushort?)GetValue(SensitivityProperty); set => SetValue(SensitivityProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Sensitivity"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Sensitivity"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Sensitivity"/> property.</param>
        protected void OnSensitivityPropertyChanged(ushort? oldValue, ushort? newValue)
        {
            // TODO: Implement OnSensitivityPropertyChanged Logic
        }

        #endregion
        #region SensitivityText Property Members

        /// <summary>
        /// Identifies the <see cref="SensitivityText"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SensitivityTextProperty = DependencyProperty.Register(nameof(SensitivityText), typeof(string),
            typeof(SummaryPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as SummaryPropertiesRowViewModel<TEntity>)?.OnSensitivityTextPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string SensitivityText { get => GetValue(SensitivityTextProperty) as string; set => SetValue(SensitivityTextProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="SensitivityText"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="SensitivityText"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="SensitivityText"/> property.</param>
        protected void OnSensitivityTextPropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnSensitivityTextPropertyChanged Logic
        }

        #endregion
        #region SimpleRating Property Members

        /// <summary>
        /// Identifies the <see cref="SimpleRating"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SimpleRatingProperty = DependencyProperty.Register(nameof(SimpleRating), typeof(uint?),
            typeof(SummaryPropertiesRowViewModel<TEntity>), new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as SummaryPropertiesRowViewModel<TEntity>)?.OnSimpleRatingPropertyChanged((uint?)e.OldValue, (uint?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public uint? SimpleRating { get => (uint?)GetValue(SimpleRatingProperty); set => SetValue(SimpleRatingProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="SimpleRating"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="SimpleRating"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="SimpleRating"/> property.</param>
        protected void OnSimpleRatingPropertyChanged(uint? oldValue, uint? newValue)
        {
            // TODO: Implement OnSimpleRatingPropertyChanged Logic
        }

        #endregion
        #region Trademarks Property Members

        /// <summary>
        /// Identifies the <see cref="Trademarks"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TrademarksProperty = DependencyProperty.Register(nameof(Trademarks), typeof(string),
            typeof(SummaryPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as SummaryPropertiesRowViewModel<TEntity>)?.OnTrademarksPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Trademarks { get => GetValue(TrademarksProperty) as string; set => SetValue(TrademarksProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Trademarks"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Trademarks"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Trademarks"/> property.</param>
        protected void OnTrademarksPropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnTrademarksPropertyChanged Logic
        }

        #endregion
        #region ProductName Property Members

        /// <summary>
        /// Identifies the <see cref="ProductName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ProductNameProperty = DependencyProperty.Register(nameof(ProductName), typeof(string),
            typeof(SummaryPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as SummaryPropertiesRowViewModel<TEntity>)?.OnProductNamePropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string ProductName { get => GetValue(ProductNameProperty) as string; set => SetValue(ProductNameProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ProductName"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ProductName"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ProductName"/> property.</param>
        protected void OnProductNamePropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnProductNamePropertyChanged Logic
        }

        #endregion
#pragma warning restore IDE0060 // Remove unused parameter

        public SummaryPropertiesRowViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            ApplicationName = entity.ApplicationName;
            Comment = entity.Comment;
            Subject = entity.Subject;
            Title = entity.Title;
            Company = entity.Company;
            ContentType = entity.ContentType;
            Copyright = entity.Copyright;
            ParentalRating = entity.ParentalRating;
            Rating = entity.Rating;
            ItemType = entity.ItemType;
            ItemTypeText = entity.ItemTypeText;
            MIMEType = entity.MIMEType;
            ParentalRatingReason = entity.ParentalRatingReason;
            ParentalRatingsOrganization = entity.ParentalRatingsOrganization;
            Sensitivity = entity.Sensitivity;
            SensitivityText = entity.SensitivityText;
            SimpleRating = entity.SimpleRating;
            Trademarks = entity.Trademarks;
            ProductName = entity.ProductName;
            ItemType = entity.ItemType;
            ItemTypeText = entity.ItemTypeText;
            MIMEType = entity.MIMEType;
            ParentalRatingReason = entity.ParentalRatingReason;
            ParentalRatingsOrganization = entity.ParentalRatingsOrganization;
            Sensitivity = entity.Sensitivity;
            SensitivityText = entity.SensitivityText;
            SimpleRating = entity.SimpleRating;
            Trademarks = entity.Trademarks;
            ProductName = entity.ProductName;
        }

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(ISummaryProperties.ApplicationName):
                    Dispatcher.CheckInvoke(() => ApplicationName = Entity.ApplicationName);
                    break;
                case nameof(ISummaryProperties.Comment):
                    Dispatcher.CheckInvoke(() => Comment = Entity.Comment);
                    break;
                case nameof(ISummaryProperties.Subject):
                    Dispatcher.CheckInvoke(() => Subject = Entity.Subject);
                    break;
                case nameof(ISummaryProperties.Title):
                    Dispatcher.CheckInvoke(() => Title = Entity.Title);
                    break;
                case nameof(ISummaryProperties.Company):
                    Dispatcher.CheckInvoke(() => Company = Entity.Company);
                    break;
                case nameof(ISummaryProperties.ContentType):
                    Dispatcher.CheckInvoke(() => ContentType = Entity.ContentType);
                    break;
                case nameof(ISummaryProperties.Copyright):
                    Dispatcher.CheckInvoke(() => Copyright = Entity.Copyright);
                    break;
                case nameof(ISummaryProperties.ParentalRating):
                    Dispatcher.CheckInvoke(() => ParentalRating = Entity.ParentalRating);
                    break;
                case nameof(ISummaryProperties.Rating):
                    Dispatcher.CheckInvoke(() => Rating = Entity.Rating);
                    break;
                case nameof(ISummaryProperties.ItemType):
                    Dispatcher.CheckInvoke(() => ItemType = Entity.ItemType);
                    break;
                case nameof(ISummaryProperties.ItemTypeText):
                    Dispatcher.CheckInvoke(() => ItemTypeText = Entity.ItemTypeText);
                    break;
                case nameof(ISummaryProperties.MIMEType):
                    Dispatcher.CheckInvoke(() => MIMEType = Entity.MIMEType);
                    break;
                case nameof(ISummaryProperties.ParentalRatingReason):
                    Dispatcher.CheckInvoke(() => ParentalRatingReason = Entity.ParentalRatingReason);
                    break;
                case nameof(ISummaryProperties.ParentalRatingsOrganization):
                    Dispatcher.CheckInvoke(() => ParentalRatingsOrganization = Entity.ParentalRatingsOrganization);
                    break;
                case nameof(ISummaryProperties.Sensitivity):
                    Dispatcher.CheckInvoke(() => Sensitivity = Entity.Sensitivity);
                    break;
                case nameof(ISummaryProperties.SensitivityText):
                    Dispatcher.CheckInvoke(() => SensitivityText = Entity.SensitivityText);
                    break;
                case nameof(ISummaryProperties.SimpleRating):
                    Dispatcher.CheckInvoke(() => SimpleRating = Entity.SimpleRating);
                    break;
                case nameof(ISummaryProperties.Trademarks):
                    Dispatcher.CheckInvoke(() => Trademarks = Entity.Trademarks);
                    break;
                case nameof(ISummaryProperties.ProductName):
                    Dispatcher.CheckInvoke(() => ProductName = Entity.ProductName);
                    break;
                default:
                    base.OnEntityPropertyChanged(propertyName);
                    break;
            }
        }
    }
}

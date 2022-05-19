using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class SummaryPropertiesRowViewModel<TEntity> : DbEntityRowViewModel<TEntity>
        where TEntity : Model.DbEntity, Model.ISummaryProperties
    {
        #region ApplicationName Property Members

        /// <summary>
        /// Identifies the <see cref="ApplicationName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ApplicationNameProperty = ColumnPropertyBuilder<string, SummaryPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.ISummaryProperties.ApplicationName))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as SummaryPropertiesRowViewModel<TEntity>)?.OnApplicationNamePropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string ApplicationName { get => GetValue(ApplicationNameProperty) as string; set => SetValue(ApplicationNameProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ApplicationName"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ApplicationName"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ApplicationName"/> property.</param>
        protected virtual void OnApplicationNamePropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region Comment Property Members

        /// <summary>
        /// Identifies the <see cref="Comment"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommentProperty = ColumnPropertyBuilder<string, SummaryPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.ISummaryProperties.Comment))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as SummaryPropertiesRowViewModel<TEntity>)?.OnCommentPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string Comment { get => GetValue(CommentProperty) as string; set => SetValue(CommentProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Comment"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Comment"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Comment"/> property.</param>
        protected virtual void OnCommentPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region Subject Property Members

        /// <summary>
        /// Identifies the <see cref="Subject"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SubjectProperty = ColumnPropertyBuilder<string, SummaryPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.ISummaryProperties.Subject))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as SummaryPropertiesRowViewModel<TEntity>)?.OnSubjectPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string Subject { get => GetValue(SubjectProperty) as string; set => SetValue(SubjectProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Subject"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Subject"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Subject"/> property.</param>
        protected virtual void OnSubjectPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region Title Property Members

        /// <summary>
        /// Identifies the <see cref="Title"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TitleProperty = ColumnPropertyBuilder<string, SummaryPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.ISummaryProperties.Title))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as SummaryPropertiesRowViewModel<TEntity>)?.OnTitlePropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string Title { get => GetValue(TitleProperty) as string; set => SetValue(TitleProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Title"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Title"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Title"/> property.</param>
        protected virtual void OnTitlePropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region Company Property Members

        /// <summary>
        /// Identifies the <see cref="Company"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CompanyProperty = ColumnPropertyBuilder<string, SummaryPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.ISummaryProperties.Company))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as SummaryPropertiesRowViewModel<TEntity>)?.OnCompanyPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string Company { get => GetValue(CompanyProperty) as string; set => SetValue(CompanyProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Company"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Company"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Company"/> property.</param>
        protected virtual void OnCompanyPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region ContentType Property Members

        /// <summary>
        /// Identifies the <see cref="ContentType"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentTypeProperty = ColumnPropertyBuilder<string, SummaryPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.ISummaryProperties.ContentType))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as SummaryPropertiesRowViewModel<TEntity>)?.OnContentTypePropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string ContentType { get => GetValue(ContentTypeProperty) as string; set => SetValue(ContentTypeProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ContentType"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ContentType"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ContentType"/> property.</param>
        protected virtual void OnContentTypePropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region Copyright Property Members

        /// <summary>
        /// Identifies the <see cref="Copyright"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CopyrightProperty = ColumnPropertyBuilder<string, SummaryPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.ISummaryProperties.Copyright))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as SummaryPropertiesRowViewModel<TEntity>)?.OnCopyrightPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string Copyright { get => GetValue(CopyrightProperty) as string; set => SetValue(CopyrightProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Copyright"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Copyright"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Copyright"/> property.</param>
        protected virtual void OnCopyrightPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region ParentalRating Property Members

        /// <summary>
        /// Identifies the <see cref="ParentalRating"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ParentalRatingProperty = ColumnPropertyBuilder<string, SummaryPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.ISummaryProperties.ParentalRating))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as SummaryPropertiesRowViewModel<TEntity>)?.OnParentalRatingPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string ParentalRating { get => GetValue(ParentalRatingProperty) as string; set => SetValue(ParentalRatingProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ParentalRating"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ParentalRating"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ParentalRating"/> property.</param>
        protected virtual void OnParentalRatingPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region Rating Property Members

        /// <summary>
        /// Identifies the <see cref="Rating"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RatingProperty = ColumnPropertyBuilder<uint?, SummaryPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.ISummaryProperties.Rating))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as SummaryPropertiesRowViewModel<TEntity>)?.OnRatingPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public uint? Rating { get => (uint?)GetValue(RatingProperty); set => SetValue(RatingProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Rating"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Rating"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Rating"/> property.</param>
        protected virtual void OnRatingPropertyChanged(uint? oldValue, uint? newValue) { }

        #endregion
        #region ItemType Property Members

        /// <summary>
        /// Identifies the <see cref="ItemType"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemTypeProperty = ColumnPropertyBuilder<string, SummaryPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.ISummaryProperties.ItemType))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as SummaryPropertiesRowViewModel<TEntity>)?.OnItemTypePropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string ItemType { get => GetValue(ItemTypeProperty) as string; set => SetValue(ItemTypeProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ItemType"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ItemType"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ItemType"/> property.</param>
        protected virtual void OnItemTypePropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region ItemTypeText Property Members

        /// <summary>
        /// Identifies the <see cref="ItemTypeText"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemTypeTextProperty = ColumnPropertyBuilder<string, SummaryPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.ISummaryProperties.ItemTypeText))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as SummaryPropertiesRowViewModel<TEntity>)?.OnItemTypeTextPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string ItemTypeText { get => GetValue(ItemTypeTextProperty) as string; set => SetValue(ItemTypeTextProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ItemTypeText"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ItemTypeText"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ItemTypeText"/> property.</param>
        protected virtual void OnItemTypeTextPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region MIMEType Property Members

        /// <summary>
        /// Identifies the <see cref="MIMEType"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MIMETypeProperty = ColumnPropertyBuilder<string, SummaryPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.ISummaryProperties.MIMEType))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as SummaryPropertiesRowViewModel<TEntity>)?.OnMIMETypePropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string MIMEType { get => GetValue(MIMETypeProperty) as string; set => SetValue(MIMETypeProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="MIMEType"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="MIMEType"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="MIMEType"/> property.</param>
        protected virtual void OnMIMETypePropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region ParentalRatingReason Property Members

        /// <summary>
        /// Identifies the <see cref="ParentalRatingReason"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ParentalRatingReasonProperty = ColumnPropertyBuilder<string, SummaryPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.ISummaryProperties.ParentalRatingReason))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as SummaryPropertiesRowViewModel<TEntity>)?.OnParentalRatingReasonPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string ParentalRatingReason { get => GetValue(ParentalRatingReasonProperty) as string; set => SetValue(ParentalRatingReasonProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ParentalRatingReason"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ParentalRatingReason"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ParentalRatingReason"/> property.</param>
        protected virtual void OnParentalRatingReasonPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region ParentalRatingsOrganization Property Members

        /// <summary>
        /// Identifies the <see cref="ParentalRatingsOrganization"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ParentalRatingsOrganizationProperty = ColumnPropertyBuilder<string, SummaryPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.ISummaryProperties.ParentalRatingsOrganization))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as SummaryPropertiesRowViewModel<TEntity>)?.OnParentalRatingsOrganizationPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string ParentalRatingsOrganization { get => GetValue(ParentalRatingsOrganizationProperty) as string; set => SetValue(ParentalRatingsOrganizationProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ParentalRatingsOrganization"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ParentalRatingsOrganization"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ParentalRatingsOrganization"/> property.</param>
        protected virtual void OnParentalRatingsOrganizationPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region Sensitivity Property Members

        /// <summary>
        /// Identifies the <see cref="Sensitivity"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SensitivityProperty = ColumnPropertyBuilder<ushort?, SummaryPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.ISummaryProperties.Sensitivity))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as SummaryPropertiesRowViewModel<TEntity>)?.OnSensitivityPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public ushort? Sensitivity { get => (ushort?)GetValue(SensitivityProperty); set => SetValue(SensitivityProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Sensitivity"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Sensitivity"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Sensitivity"/> property.</param>
        protected virtual void OnSensitivityPropertyChanged(ushort? oldValue, ushort? newValue) { }

        #endregion
        #region SensitivityText Property Members

        /// <summary>
        /// Identifies the <see cref="SensitivityText"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SensitivityTextProperty = ColumnPropertyBuilder<string, SummaryPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.ISummaryProperties.SensitivityText))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as SummaryPropertiesRowViewModel<TEntity>)?.OnSensitivityTextPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string SensitivityText { get => GetValue(SensitivityTextProperty) as string; set => SetValue(SensitivityTextProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="SensitivityText"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="SensitivityText"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="SensitivityText"/> property.</param>
        protected virtual void OnSensitivityTextPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region SimpleRating Property Members

        /// <summary>
        /// Identifies the <see cref="SimpleRating"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SimpleRatingProperty = ColumnPropertyBuilder<uint?, SummaryPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.ISummaryProperties.SimpleRating))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as SummaryPropertiesRowViewModel<TEntity>)?.OnSimpleRatingPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public uint? SimpleRating { get => (uint?)GetValue(SimpleRatingProperty); set => SetValue(SimpleRatingProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="SimpleRating"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="SimpleRating"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="SimpleRating"/> property.</param>
        protected virtual void OnSimpleRatingPropertyChanged(uint? oldValue, uint? newValue) { }

        #endregion
        #region Trademarks Property Members

        /// <summary>
        /// Identifies the <see cref="Trademarks"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TrademarksProperty = ColumnPropertyBuilder<string, SummaryPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.ISummaryProperties.Trademarks))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as SummaryPropertiesRowViewModel<TEntity>)?.OnTrademarksPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string Trademarks { get => GetValue(TrademarksProperty) as string; set => SetValue(TrademarksProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Trademarks"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Trademarks"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Trademarks"/> property.</param>
        protected virtual void OnTrademarksPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region ProductName Property Members

        /// <summary>
        /// Identifies the <see cref="ProductName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ProductNameProperty = ColumnPropertyBuilder<string, SummaryPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.ISummaryProperties.ProductName))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as SummaryPropertiesRowViewModel<TEntity>)?.OnProductNamePropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string ProductName { get => GetValue(ProductNameProperty) as string; set => SetValue(ProductNameProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ProductName"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ProductName"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ProductName"/> property.</param>
        protected virtual void OnProductNamePropertyChanged(string oldValue, string newValue) { }

        #endregion

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
        }

        public IEnumerable<(string DisplayName, string Value)> GetNameValuePairs()
        {
            yield return (FsInfoCat.Properties.Resources.DisplayName_Title, Title.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            yield return (FsInfoCat.Properties.Resources.DisplayName_Subject, Subject.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            yield return (FsInfoCat.Properties.Resources.DisplayName_ContentType, ContentType.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            string itemType = ItemTypeText.AsWsNormalizedOrEmpty().TruncateWithElipses(256);
            yield return (FsInfoCat.Properties.Resources.DisplayName_ItemType, (itemType.Length > 0) ? itemType : ItemType.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            yield return (FsInfoCat.Properties.Resources.DisplayName_MIMEType, MIMEType.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            yield return (FsInfoCat.Properties.Resources.DisplayName_SimpleRating, SimpleRating?.ToString());
            yield return (FsInfoCat.Properties.Resources.DisplayName_Rating, Rating?.ToString());
            yield return (FsInfoCat.Properties.Resources.DisplayName_ParentalRating, ParentalRating.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            yield return (FsInfoCat.Properties.Resources.DisplayName_ParentalRatingReason, ParentalRatingReason.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            yield return (FsInfoCat.Properties.Resources.DisplayName_ParentalRatingsOrganization, ParentalRatingsOrganization.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            yield return (FsInfoCat.Properties.Resources.DisplayName_ApplicationName, ApplicationName.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            yield return (FsInfoCat.Properties.Resources.DisplayName_ProductName, ProductName.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            yield return (FsInfoCat.Properties.Resources.DisplayName_Company, Company.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            yield return (FsInfoCat.Properties.Resources.DisplayName_Trademarks, Trademarks.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            yield return (FsInfoCat.Properties.Resources.DisplayName_Copyright, Copyright.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            string sensitivity = SensitivityText.AsWsNormalizedOrEmpty().TruncateWithElipses(256);
            yield return (FsInfoCat.Properties.Resources.DisplayName_Sensitivity, (sensitivity.Length > 0) ? sensitivity : Sensitivity?.ToString());
            yield return (FsInfoCat.Properties.Resources.DisplayName_Comment, Comment.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
        }

        internal string CalculateDisplayText(Func<(string DisplayName, string Value), bool> filter = null) => (filter is null) ?
            StringExtensionMethods.ToKeyValueListString(GetNameValuePairs()) : StringExtensionMethods.ToKeyValueListString(GetNameValuePairs().Where(filter));

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Model.ISummaryProperties.ApplicationName):
                    Dispatcher.CheckInvoke(() => ApplicationName = Entity.ApplicationName);
                    break;
                case nameof(Model.ISummaryProperties.Comment):
                    Dispatcher.CheckInvoke(() => Comment = Entity.Comment);
                    break;
                case nameof(Model.ISummaryProperties.Subject):
                    Dispatcher.CheckInvoke(() => Subject = Entity.Subject);
                    break;
                case nameof(Model.ISummaryProperties.Title):
                    Dispatcher.CheckInvoke(() => Title = Entity.Title);
                    break;
                case nameof(Model.ISummaryProperties.Company):
                    Dispatcher.CheckInvoke(() => Company = Entity.Company);
                    break;
                case nameof(Model.ISummaryProperties.ContentType):
                    Dispatcher.CheckInvoke(() => ContentType = Entity.ContentType);
                    break;
                case nameof(Model.ISummaryProperties.Copyright):
                    Dispatcher.CheckInvoke(() => Copyright = Entity.Copyright);
                    break;
                case nameof(Model.ISummaryProperties.ParentalRating):
                    Dispatcher.CheckInvoke(() => ParentalRating = Entity.ParentalRating);
                    break;
                case nameof(Model.ISummaryProperties.Rating):
                    Dispatcher.CheckInvoke(() => Rating = Entity.Rating);
                    break;
                case nameof(Model.ISummaryProperties.ItemType):
                    Dispatcher.CheckInvoke(() => ItemType = Entity.ItemType);
                    break;
                case nameof(Model.ISummaryProperties.ItemTypeText):
                    Dispatcher.CheckInvoke(() => ItemTypeText = Entity.ItemTypeText);
                    break;
                case nameof(Model.ISummaryProperties.MIMEType):
                    Dispatcher.CheckInvoke(() => MIMEType = Entity.MIMEType);
                    break;
                case nameof(Model.ISummaryProperties.ParentalRatingReason):
                    Dispatcher.CheckInvoke(() => ParentalRatingReason = Entity.ParentalRatingReason);
                    break;
                case nameof(Model.ISummaryProperties.ParentalRatingsOrganization):
                    Dispatcher.CheckInvoke(() => ParentalRatingsOrganization = Entity.ParentalRatingsOrganization);
                    break;
                case nameof(Model.ISummaryProperties.Sensitivity):
                    Dispatcher.CheckInvoke(() => Sensitivity = Entity.Sensitivity);
                    break;
                case nameof(Model.ISummaryProperties.SensitivityText):
                    Dispatcher.CheckInvoke(() => SensitivityText = Entity.SensitivityText);
                    break;
                case nameof(Model.ISummaryProperties.SimpleRating):
                    Dispatcher.CheckInvoke(() => SimpleRating = Entity.SimpleRating);
                    break;
                case nameof(Model.ISummaryProperties.Trademarks):
                    Dispatcher.CheckInvoke(() => Trademarks = Entity.Trademarks);
                    break;
                case nameof(Model.ISummaryProperties.ProductName):
                    Dispatcher.CheckInvoke(() => ProductName = Entity.ProductName);
                    break;
                default:
                    base.OnEntityPropertyChanged(propertyName);
                    break;
            }
        }
    }
}

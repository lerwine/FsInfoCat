using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class DRMPropertiesRowViewModel<TEntity> : DbEntityRowViewModel<TEntity>
        where TEntity : Model.DbEntity, Model.IDRMProperties
    {
        #region DatePlayExpires Property Members

        /// <summary>
        /// Identifies the <see cref="DatePlayExpires"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DatePlayExpiresProperty = ColumnPropertyBuilder<DateTime?, DRMPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IDRMProperties.DatePlayExpires))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as DRMPropertiesRowViewModel<TEntity>)?.OnDatePlayExpiresPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public DateTime? DatePlayExpires { get => (DateTime?)GetValue(DatePlayExpiresProperty); set => SetValue(DatePlayExpiresProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="DatePlayExpires"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="DatePlayExpires"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="DatePlayExpires"/> property.</param>
        protected virtual void OnDatePlayExpiresPropertyChanged(DateTime? oldValue, DateTime? newValue) { }

        #endregion
        #region DatePlayStarts Property Members

        /// <summary>
        /// Identifies the <see cref="DatePlayStarts"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DatePlayStartsProperty = ColumnPropertyBuilder<DateTime?, DRMPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IDRMProperties.DatePlayStarts))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as DRMPropertiesRowViewModel<TEntity>)?.OnDatePlayStartsPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public DateTime? DatePlayStarts { get => (DateTime?)GetValue(DatePlayStartsProperty); set => SetValue(DatePlayStartsProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="DatePlayStarts"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="DatePlayStarts"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="DatePlayStarts"/> property.</param>
        protected virtual void OnDatePlayStartsPropertyChanged(DateTime? oldValue, DateTime? newValue) { }

        #endregion
        #region Description Property Members

        /// <summary>
        /// Identifies the <see cref="Description"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DescriptionProperty = ColumnPropertyBuilder<string, DRMPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IDRMProperties.Description))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as DRMPropertiesRowViewModel<TEntity>)?.OnDescriptionPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string Description { get => GetValue(DescriptionProperty) as string; set => SetValue(DescriptionProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Description"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Description"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Description"/> property.</param>
        protected virtual void OnDescriptionPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region IsProtected Property Members

        /// <summary>
        /// Identifies the <see cref="IsProtected"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsProtectedProperty = ColumnPropertyBuilder<bool?, DRMPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IDRMProperties.IsProtected))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as DRMPropertiesRowViewModel<TEntity>)?.OnIsProtectedPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public bool? IsProtected { get => (bool?)GetValue(IsProtectedProperty); set => SetValue(IsProtectedProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="IsProtected"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="IsProtected"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="IsProtected"/> property.</param>
        protected virtual void OnIsProtectedPropertyChanged(bool? oldValue, bool? newValue) { }

        #endregion
        #region PlayCount Property Members

        /// <summary>
        /// Identifies the <see cref="PlayCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PlayCountProperty = ColumnPropertyBuilder<uint?, DRMPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IDRMProperties.PlayCount))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as DRMPropertiesRowViewModel<TEntity>)?.OnPlayCountPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public uint? PlayCount { get => (uint?)GetValue(PlayCountProperty); set => SetValue(PlayCountProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="PlayCount"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="PlayCount"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="PlayCount"/> property.</param>
        protected virtual void OnPlayCountPropertyChanged(uint? oldValue, uint? newValue) { }

        #endregion

        public DRMPropertiesRowViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            DatePlayExpires = entity.DatePlayExpires;
            DatePlayStarts = entity.DatePlayStarts;
            Description = entity.Description;
            IsProtected = entity.IsProtected;
            PlayCount = entity.PlayCount;
        }

        public IEnumerable<(string DisplayName, string Value)> GetNameValuePairs()
        {
            yield return (FsInfoCat.Properties.Resources.DisplayName_DatePlayStarts, DatePlayStarts?.ToString());
            yield return (FsInfoCat.Properties.Resources.DisplayName_DatePlayExpires, DatePlayExpires?.ToString());
            yield return (FsInfoCat.Properties.Resources.DisplayName_PlayCount, PlayCount?.ToString());
            yield return (FsInfoCat.Properties.Resources.DisplayName_IsProtected, Converters.BooleanToStringConverter.Convert(IsProtected));
            yield return (FsInfoCat.Properties.Resources.DisplayName_Description, Description.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
        }

        internal string CalculateDisplayText(Func<(string DisplayName, string Value), bool> filter = null) => (filter is null) ?
            StringExtensionMethods.ToKeyValueListString(GetNameValuePairs()) : StringExtensionMethods.ToKeyValueListString(GetNameValuePairs().Where(filter));
    }
}

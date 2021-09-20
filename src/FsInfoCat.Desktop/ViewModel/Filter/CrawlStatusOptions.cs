using FsInfoCat.Local;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using LinqExpression = System.Linq.Expressions.Expression;


namespace FsInfoCat.Desktop.ViewModel.Filter
{
    public class CrawlStatusOptions<TEntity> : Filter<TEntity>
        where TEntity : class, ICrawlConfigurationRow
    {
        #region Owner Attached Property Members

        /// <summary>
        /// The name of the <see cref="OwnerProperty">Owner</see> attached dependency property.
        /// </summary>
        public const string PropertyName_Owner = "Owner";

        private static readonly DependencyPropertyKey OwnerPropertyKey = DependencyPropertyBuilder<CrawlStatusOptions<TEntity>, CrawlStatusOptions<TEntity>>
            .RegisterAttached(PropertyName_Owner)
            .DefaultValue(null)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Owner"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OwnerProperty = OwnerPropertyKey.DependencyProperty;

        public static CrawlStatusOptions<TEntity> GetOwner([DisallowNull] DependencyObject obj) => (CrawlStatusOptions<TEntity>)obj.GetValue(OwnerProperty);

        private static void SetOwner([DisallowNull] DependencyObject obj, CrawlStatusOptions<TEntity> value) => obj.SetValue(OwnerPropertyKey, value);

        #endregion
        #region OptionItems Property Members

        private static readonly DependencyPropertyKey OptionItemsPropertyKey = DependencyPropertyBuilder<CrawlStatusOptions<TEntity>, ReadOnlyObservableCollection<CrawlStatusItemVM<TEntity>>>
            .Register(nameof(OptionItems))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="OptionItems"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OptionItemsProperty = OptionItemsPropertyKey.DependencyProperty;

        public ReadOnlyObservableCollection<CrawlStatusItemVM<TEntity>> OptionItems => (ReadOnlyObservableCollection<CrawlStatusItemVM<TEntity>>)GetValue(OptionItemsProperty);

        #endregion
        #region NotRunning Property Members

        /// <summary>
        /// Identifies the <see cref="NotRunning"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NotRunningProperty = DependencyPropertyBuilder<CrawlStatusOptions<TEntity>, bool>
            .Register(nameof(NotRunning))
            .DefaultValue(false)
            .OnChanged((d, oldValue, newValue) => (d as CrawlStatusOptions<TEntity>)?.OnNotRunningPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public bool NotRunning { get => (bool)GetValue(NotRunningProperty); set => SetValue(NotRunningProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="NotRunning"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="NotRunning"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="NotRunning"/> property.</param>
        protected virtual void OnNotRunningPropertyChanged(bool oldValue, bool newValue) => OptionItems.First(o => o.Value == CrawlStatus.NotRunning).IsSelected = newValue;

        #endregion
        #region InProgress Property Members

        /// <summary>
        /// Identifies the <see cref="InProgress"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty InProgressProperty = DependencyPropertyBuilder<CrawlStatusOptions<TEntity>, bool>
            .Register(nameof(InProgress))
            .DefaultValue(false)
            .OnChanged((d, oldValue, newValue) => (d as CrawlStatusOptions<TEntity>)?.OnInProgressPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public bool InProgress { get => (bool)GetValue(InProgressProperty); set => SetValue(InProgressProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="InProgress"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="InProgress"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="InProgress"/> property.</param>
        protected virtual void OnInProgressPropertyChanged(bool oldValue, bool newValue) => OptionItems.First(o => o.Value == CrawlStatus.InProgress).IsSelected = newValue;

        #endregion
        #region Completed Property Members

        /// <summary>
        /// Identifies the <see cref="Completed"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CompletedProperty = DependencyPropertyBuilder<CrawlStatusOptions<TEntity>, bool>
            .Register(nameof(Completed))
            .DefaultValue(false)
            .OnChanged((d, oldValue, newValue) => (d as CrawlStatusOptions<TEntity>)?.OnCompletedPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public bool Completed { get => (bool)GetValue(CompletedProperty); set => SetValue(CompletedProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Completed"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Completed"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Completed"/> property.</param>
        protected virtual void OnCompletedPropertyChanged(bool oldValue, bool newValue) => OptionItems.First(o => o.Value == CrawlStatus.Completed).IsSelected = newValue;

        #endregion
        #region AllottedTimeElapsed Property Members

        /// <summary>
        /// Identifies the <see cref="AllottedTimeElapsed"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AllottedTimeElapsedProperty = DependencyPropertyBuilder<CrawlStatusOptions<TEntity>, bool>
            .Register(nameof(AllottedTimeElapsed))
            .DefaultValue(false)
            .OnChanged((d, oldValue, newValue) => (d as CrawlStatusOptions<TEntity>)?.OnAllottedTimeElapsedPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public bool AllottedTimeElapsed { get => (bool)GetValue(AllottedTimeElapsedProperty); set => SetValue(AllottedTimeElapsedProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="AllottedTimeElapsed"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="AllottedTimeElapsed"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="AllottedTimeElapsed"/> property.</param>
        protected virtual void OnAllottedTimeElapsedPropertyChanged(bool oldValue, bool newValue) => OptionItems.First(o => o.Value == CrawlStatus.AllottedTimeElapsed).IsSelected = newValue;

        #endregion
        #region MaxItemCountReached Property Members

        /// <summary>
        /// Identifies the <see cref="MaxItemCountReached"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaxItemCountReachedProperty = DependencyPropertyBuilder<CrawlStatusOptions<TEntity>, bool>
            .Register(nameof(MaxItemCountReached))
            .DefaultValue(false)
            .OnChanged((d, oldValue, newValue) => (d as CrawlStatusOptions<TEntity>)?.OnMaxItemCountReachedPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public bool MaxItemCountReached { get => (bool)GetValue(MaxItemCountReachedProperty); set => SetValue(MaxItemCountReachedProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="MaxItemCountReached"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="MaxItemCountReached"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="MaxItemCountReached"/> property.</param>
        protected virtual void OnMaxItemCountReachedPropertyChanged(bool oldValue, bool newValue) => OptionItems.First(o => o.Value == CrawlStatus.MaxItemCountReached).IsSelected = newValue;

        #endregion
        #region Canceled Property Members

        /// <summary>
        /// Identifies the <see cref="Canceled"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CanceledProperty = DependencyPropertyBuilder<CrawlStatusOptions<TEntity>, bool>
            .Register(nameof(Canceled))
            .DefaultValue(false)
            .OnChanged((d, oldValue, newValue) => (d as CrawlStatusOptions<TEntity>)?.OnCanceledPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public bool Canceled { get => (bool)GetValue(CanceledProperty); set => SetValue(CanceledProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Canceled"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Canceled"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Canceled"/> property.</param>
        protected virtual void OnCanceledPropertyChanged(bool oldValue, bool newValue) => OptionItems.First(o => o.Value == CrawlStatus.Canceled).IsSelected = newValue;

        #endregion
        #region Failed Property Members

        /// <summary>
        /// Identifies the <see cref="Failed"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FailedProperty = DependencyPropertyBuilder<CrawlStatusOptions<TEntity>, bool>
            .Register(nameof(Failed))
            .DefaultValue(false)
            .OnChanged((d, oldValue, newValue) => (d as CrawlStatusOptions<TEntity>)?.OnFailedPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public bool Failed { get => (bool)GetValue(FailedProperty); set => SetValue(FailedProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Failed"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Failed"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Failed"/> property.</param>
        protected virtual void OnFailedPropertyChanged(bool oldValue, bool newValue) => OptionItems.First(o => o.Value == CrawlStatus.Failed).IsSelected = newValue;

        #endregion
        #region Disabled Property Members

        /// <summary>
        /// Identifies the <see cref="Disabled"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DisabledProperty = DependencyPropertyBuilder<CrawlStatusOptions<TEntity>, bool>
            .Register(nameof(Disabled))
            .DefaultValue(false)
            .OnChanged((d, oldValue, newValue) => (d as CrawlStatusOptions<TEntity>)?.OnDisabledPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public bool Disabled { get => (bool)GetValue(DisabledProperty); set => SetValue(DisabledProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Disabled"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Disabled"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Disabled"/> property.</param>
        protected virtual void OnDisabledPropertyChanged(bool oldValue, bool newValue) => OptionItems.First(o => o.Value == CrawlStatus.Disabled).IsSelected = newValue;

        #endregion
        #region IsExclusive Property Members

        /// <summary>
        /// Identifies the <see cref="IsExclusive"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsExclusiveProperty = DependencyPropertyBuilder<CrawlStatusOptions<TEntity>, bool>
            .Register(nameof(IsExclusive))
            .DefaultValue(false)
            .OnChanged((d, oldValue, newValue) => (d as CrawlStatusOptions<TEntity>)?.OnIsExclusivePropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public bool IsExclusive { get => (bool)GetValue(IsExclusiveProperty); set => SetValue(IsExclusiveProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="IsExclusive"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="IsExclusive"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="IsExclusive"/> property.</param>
        protected virtual void OnIsExclusivePropertyChanged(bool oldValue, bool newValue)
        {
            // TODO: Implement OnIsExclusivePropertyChanged Logic
        }

        #endregion

        public CrawlStatusOptions()
        {
            ObservableCollection<CrawlStatusItemVM<TEntity>> backingOptionItems = new();
            foreach (CrawlStatus status in Enum.GetValues<CrawlStatus>())
            {
                CrawlStatusItemVM<TEntity> item = new(status);
                SetOwner(item, this);
                backingOptionItems.Add(item);
            }
            SetValue(OptionItemsPropertyKey, new ReadOnlyObservableCollection<CrawlStatusItemVM<TEntity>>(backingOptionItems));
        }

        public override BinaryExpression CreateExpression(ParameterExpression parameterExpression) => IsExclusive ?
            OptionItems.Where(o => o.IsSelected).Select(o => LinqExpression.Equal(LinqExpression.Property(parameterExpression, nameof(CrawlConfigReportItem.StatusValue)), LinqExpression.Constant(o.Value.Value))).Aggregate(LinqExpression.AndAlso) :
            OptionItems.Where(o => o.IsSelected).Select(o => LinqExpression.Equal(LinqExpression.Property(parameterExpression, nameof(CrawlConfigReportItem.StatusValue)), LinqExpression.Constant(o.Value.Value))).Aggregate(LinqExpression.OrElse);

        internal void Select(CrawlStatus value)
        {
            switch (value)
            {
                case CrawlStatus.InProgress:
                    InProgress = true;
                    break;
                case CrawlStatus.Completed:
                    Completed = true;
                    break;
                case CrawlStatus.AllottedTimeElapsed:
                    AllottedTimeElapsed = true;
                    break;
                case CrawlStatus.MaxItemCountReached:
                    MaxItemCountReached = true;
                    break;
                case CrawlStatus.Canceled:
                    Canceled = true;
                    break;
                case CrawlStatus.Failed:
                    Failed = true;
                    break;
                case CrawlStatus.Disabled:
                    Disabled = true;
                    break;
                default:
                    NotRunning = true;
                    break;
            }
        }

        internal bool IsMatch(CrawlStatus value)
        {
            return value switch
            {
                CrawlStatus.InProgress => InProgress != IsExclusive,
                CrawlStatus.Completed => Completed != IsExclusive,
                CrawlStatus.AllottedTimeElapsed => AllottedTimeElapsed != IsExclusive,
                CrawlStatus.MaxItemCountReached => MaxItemCountReached != IsExclusive,
                CrawlStatus.Canceled => Canceled != IsExclusive,
                CrawlStatus.Failed => Failed != IsExclusive,
                CrawlStatus.Disabled => Disabled != IsExclusive,
                _ => NotRunning != IsExclusive,
            };
        }

        internal void Deselect(CrawlStatus value)
        {
            switch (value)
            {
                case CrawlStatus.InProgress:
                    InProgress = false;
                    break;
                case CrawlStatus.Completed:
                    Completed = false;
                    break;
                case CrawlStatus.AllottedTimeElapsed:
                    AllottedTimeElapsed = false;
                    break;
                case CrawlStatus.MaxItemCountReached:
                    MaxItemCountReached = false;
                    break;
                case CrawlStatus.Canceled:
                    Canceled = false;
                    break;
                case CrawlStatus.Failed:
                    Failed = false;
                    break;
                case CrawlStatus.Disabled:
                    Disabled = false;
                    break;
                default:
                    NotRunning = false;
                    break;
            }
        }

        public override bool IsMatch(TEntity entity) => entity is not null && IsMatch(entity.StatusValue);
    }
}

using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class ComparisonRowViewModel<TEntity> : DbEntityRowViewModel<TEntity>
        where TEntity : Model.DbEntity, Model.IComparison
    {
        #region AreEqual Property Members

        /// <summary>
        /// Identifies the <see cref="AreEqual"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AreEqualProperty = ColumnPropertyBuilder<bool, ComparisonRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IComparison.AreEqual))
            .DefaultValue(false)
            .OnChanged((DependencyObject d, bool oldValue, bool newValue) =>
                (d as ComparisonRowViewModel<TEntity>).OnAreEqualPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public bool AreEqual { get => (bool)GetValue(AreEqualProperty); set => SetValue(AreEqualProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="AreEqual"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="AreEqual"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="AreEqual"/> property.</param>
        protected virtual void OnAreEqualPropertyChanged(bool oldValue, bool newValue) { }

        #endregion
        #region ComparedOn Property Members

        /// <summary>
        /// Identifies the <see cref="ComparedOn"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ComparedOnProperty = ColumnPropertyBuilder<DateTime, ComparisonRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IComparison.ComparedOn))
            .OnChanged((DependencyObject d, DateTime oldValue, DateTime newValue) =>
                (d as ComparisonRowViewModel<TEntity>).OnComparedOnPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public DateTime ComparedOn { get => (DateTime)GetValue(ComparedOnProperty); set => SetValue(ComparedOnProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ComparedOn"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ComparedOn"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ComparedOn"/> property.</param>
        protected virtual void OnComparedOnPropertyChanged(DateTime oldValue, DateTime newValue) { }

        #endregion

        public ComparisonRowViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            AreEqual = entity.AreEqual;
            ComparedOn = entity.ComparedOn;
        }

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Model.IComparison.AreEqual):
                    Dispatcher.CheckInvoke(() => AreEqual = Entity.AreEqual);
                    break;
                case nameof(Model.IComparison.ComparedOn):
                    Dispatcher.CheckInvoke(() => ComparedOn = Entity.ComparedOn);
                    break;
                default:
                    base.OnEntityPropertyChanged(propertyName);
                    break;
            }
        }
    }
}

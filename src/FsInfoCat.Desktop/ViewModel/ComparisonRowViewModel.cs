using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class ComparisonRowViewModel<TEntity> : DbEntityRowViewModel<TEntity>
        where TEntity : DbEntity, IComparison
    {
#pragma warning disable IDE0060 // Remove unused parameter
        #region AreEqual Property Members

        /// <summary>
        /// Identifies the <see cref="AreEqual"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AreEqualProperty = DependencyProperty.Register(nameof(AreEqual), typeof(bool), typeof(ComparisonRowViewModel<TEntity>),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as ComparisonRowViewModel<TEntity>)?.OnAreEqualPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool AreEqual { get => (bool)GetValue(AreEqualProperty); set => SetValue(AreEqualProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="AreEqual"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="AreEqual"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="AreEqual"/> property.</param>
        protected void OnAreEqualPropertyChanged(bool oldValue, bool newValue) { }

        #endregion
        #region ComparedOn Property Members

        /// <summary>
        /// Identifies the <see cref="ComparedOn"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ComparedOnProperty = DependencyProperty.Register(nameof(ComparedOn), typeof(DateTime), typeof(ComparisonRowViewModel<TEntity>),
                new PropertyMetadata(default(DateTime), (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as ComparisonRowViewModel<TEntity>)?.OnComparedOnPropertyChanged((DateTime)e.OldValue, (DateTime)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public DateTime ComparedOn { get => (DateTime)GetValue(ComparedOnProperty); set => SetValue(ComparedOnProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ComparedOn"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ComparedOn"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ComparedOn"/> property.</param>
        protected void OnComparedOnPropertyChanged(DateTime oldValue, DateTime newValue) { }

        #endregion
#pragma warning restore IDE0060 // Remove unused parameter

        public ComparisonRowViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            AreEqual = entity.AreEqual;
            ComparedOn = entity.ComparedOn;
        }

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(IComparison.AreEqual):
                    Dispatcher.CheckInvoke(() => AreEqual = Entity.AreEqual);
                    break;
                case nameof(IComparison.ComparedOn):
                    Dispatcher.CheckInvoke(() => ComparedOn = Entity.ComparedOn);
                    break;
                default:
                    base.OnEntityPropertyChanged(propertyName);
                    break;
            }
        }
    }
}

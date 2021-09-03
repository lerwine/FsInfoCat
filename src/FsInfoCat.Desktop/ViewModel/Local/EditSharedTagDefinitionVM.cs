using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    /// <summary>
    /// View Model for <see cref="View.Local.EditSharedTagDefinitionWindow"/>.
    /// </summary>
    public class EditSharedTagDefinitionVM : EditDbEntityVM<SharedTagDefinition>
    {
        #region IsInactive Property Members

        /// <summary>
        /// Identifies the <see cref="IsInactive"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsInactiveProperty = DependencyProperty.Register(nameof(IsInactive), typeof(bool), typeof(EditSharedTagDefinitionVM),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as EditSharedTagDefinitionVM)?.OnIsInactivePropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool IsInactive { get => (bool)GetValue(IsInactiveProperty); set => SetValue(IsInactiveProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="IsInactive"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="IsInactive"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="IsInactive"/> property.</param>
        protected void OnIsInactivePropertyChanged(bool oldValue, bool newValue)
        {
            // DEFERRED: Implement OnIsInactivePropertyChanged Logic
        }

        #endregion
        #region Name Property Members

        /// <summary>
        /// Identifies the <see cref="Name"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NameProperty = DependencyProperty.Register(nameof(Name), typeof(string), typeof(EditSharedTagDefinitionVM),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as EditSharedTagDefinitionVM)?.OnNamePropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Name { get => GetValue(NameProperty) as string; set => SetValue(NameProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Name"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Name"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Name"/> property.</param>
        protected void OnNamePropertyChanged(string oldValue, string newValue)
        {
            // DEFERRED: Implement OnNamePropertyChanged Logic
        }

        #endregion
        #region Description Property Members

        /// <summary>
        /// Identifies the <see cref="Description"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(nameof(Description), typeof(string), typeof(EditSharedTagDefinitionVM),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as EditSharedTagDefinitionVM)?.OnDescriptionPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Description { get => GetValue(DescriptionProperty) as string; set => SetValue(DescriptionProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Description"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Description"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Description"/> property.</param>
        protected void OnDescriptionPropertyChanged(string oldValue, string newValue)
        {
            // DEFERRED: Implement OnDescriptionPropertyChanged Logic
        }

        #endregion

        protected override DbSet<SharedTagDefinition> GetDbSet([DisallowNull] LocalDbContext dbContext) => dbContext.SharedTagDefinitions;

        protected override void OnModelPropertyChanged(SharedTagDefinition oldValue, SharedTagDefinition newValue)
        {
            if (newValue is null)
            {
                Name = Description = "";
                IsInactive = false;
            }
            else
            {
                Name = newValue.Name;
                Description = newValue.Description;
                IsInactive = newValue.IsInactive;
            }
        }

        protected override bool OnBeforeSave()
        {
            SharedTagDefinition model = Model;
            if (model is null)
                return false;
            model.Name = Name;
            model.Description = Description;
            model.IsInactive = IsInactive;
            return true;
        }
    }
}

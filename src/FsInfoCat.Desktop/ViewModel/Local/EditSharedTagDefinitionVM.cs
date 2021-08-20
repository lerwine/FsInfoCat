using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using System;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Local
{
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
        private void OnIsInactivePropertyChanged(bool oldValue, bool newValue)
        {
            // TODO: Implement OnIsInactivePropertyChanged Logic
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
        private void OnNamePropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnNamePropertyChanged Logic
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
        private void OnDescriptionPropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnDescriptionPropertyChanged Logic
        }

        #endregion

        protected override DbSet<SharedTagDefinition> GetDbSet(LocalDbContext dbContext) => dbContext.SharedTagDefinitions;

        protected override SharedTagDefinition InitializeNewModel() => new() { CreatedOn = DateTime.Now };

        protected override void UpdateModelForSave(SharedTagDefinition model, bool isNew)
        {
            model.Name = Name;
            model.Description = Description;
            model.IsInactive = IsInactive;
        }
    }
}

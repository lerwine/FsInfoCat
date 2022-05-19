using System;

namespace FsInfoCat.Desktop.ViewModel
{
    public interface ICrudEntityRowViewModel : IDbEntityRowViewModel
    {
        event EventHandler<Commands.CommandEventArgs> OpenCommand;

        event EventHandler<Commands.CommandEventArgs> EditCommand;

        event EventHandler<Commands.CommandEventArgs> DeleteCommand;

        Commands.RelayCommand Open { get; }

        Commands.RelayCommand Edit { get; }

        Commands.RelayCommand Delete { get; }
    }

    public interface ICrudEntityRowViewModel<TEntity> : IDbEntityRowViewModel<TEntity>, ICrudEntityRowViewModel where TEntity : Model.DbEntity { }
}

using System;

namespace FsInfoCat.Desktop.ViewModel
{
    public class SqlConnectionStringEventArgs : EventArgs
    {
        public string ConnectionString { get; }

        private readonly Action _onCompleted;

        public void SetCompleted() => _onCompleted();

        public SqlConnectionStringEventArgs(string connectionString, Action onCompleted)
        {
            ConnectionString = connectionString;
            _onCompleted = onCompleted;
        }
    }
}

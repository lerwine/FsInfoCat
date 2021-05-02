using System;
using System.Data.SqlClient;

namespace FsInfoCat.Desktop.ViewModel
{
    public class SqlConnectionStringBuilderEventArgs : EventArgs
    {
        private Action<SqlConnectionStringBuilder> _onCompleted;

        public SqlConnectionStringBuilder Builder { get; }

        public void SetCompleted(bool acceptChanges) => _onCompleted(acceptChanges ? Builder : null);

        public SqlConnectionStringBuilderEventArgs(SqlConnectionStringBuilder builder, Action<SqlConnectionStringBuilder> onCompleted)
        {
            Builder = new SqlConnectionStringBuilder((builder is null) ? "" : builder.ConnectionString);
            _onCompleted = onCompleted;
        }
    }
}

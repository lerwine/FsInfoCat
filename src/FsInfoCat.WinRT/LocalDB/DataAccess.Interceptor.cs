using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.LocalDB
{
    public partial class DataAccess
    {
        public class Interceptor : DbCommandInterceptor
        {
            private ILogger<DataAccess> _logger;

            public ILogger<DataAccess> Logger
            {
                get
                {
                    if (_logger is null)
                        _logger = App.ServiceProvider.GetRequiredService<ILogger<DataAccess>>();
                    return _logger;
                }
            }

            public override InterceptionResult<int> NonQueryExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<int> result)
            {
                Logger.LogInformation("NonQueryExecuting: {CommandText}; ConnectionId={ConnectionId}, CommandId={CommandId}, EventId={EventId}",
                    command.CommandText, eventData.ConnectionId, eventData.CommandId, eventData.EventId);
                return base.NonQueryExecuting(command, eventData, result);
            }

            public override Task<InterceptionResult<int>> NonQueryExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
            {
                Logger.LogInformation("NonQueryExecutingAsync: {CommandText}; ConnectionId={ConnectionId}, CommandId={CommandId}, EventId={EventId}",
                    command.CommandText, eventData.ConnectionId, eventData.CommandId, eventData.EventId);
                return base.NonQueryExecutingAsync(command, eventData, result, cancellationToken);
            }

            public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
            {
                Logger.LogInformation("ReaderExecuting: {CommandText}; ConnectionId={ConnectionId}, CommandId={CommandId}, EventId={EventId}",
                    command.CommandText, eventData.ConnectionId, eventData.CommandId, eventData.EventId);
                return base.ReaderExecuting(command, eventData, result);
            }

            public override Task<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
            {
                Logger.LogInformation("ReaderExecutingAsync: {CommandText}; ConnectionId={ConnectionId}, CommandId={CommandId}, EventId={EventId}",
                    command.CommandText, eventData.ConnectionId, eventData.CommandId, eventData.EventId);
                return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
            }

            public override InterceptionResult<object> ScalarExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<object> result)
            {
                Logger.LogInformation("ScalarExecuting: {CommandText}; ConnectionId={ConnectionId}, CommandId={CommandId}, EventId={EventId}",
                    command.CommandText, eventData.ConnectionId, eventData.CommandId, eventData.EventId);
                return base.ScalarExecuting(command, eventData, result);
            }

            public override Task<InterceptionResult<object>> ScalarExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<object> result, CancellationToken cancellationToken = default)
            {
                Logger.LogInformation("ScalarExecutingAsync: {CommandText}; ConnectionId={ConnectionId}, CommandId={CommandId}, EventId={EventId}",
                    command.CommandText, eventData.ConnectionId, eventData.CommandId, eventData.EventId);
                return base.ScalarExecutingAsync(command, eventData, result, cancellationToken);
            }

            public override DbCommand CommandCreated(CommandEndEventData eventData, DbCommand result)
            {
                Logger.LogInformation("CommandCreated: {CommandText}; ConnectionId={ConnectionId}, CommandId={CommandId}, EventId={EventId}",
                    result.CommandText, eventData.ConnectionId, eventData.CommandId, eventData.EventId);
                return base.CommandCreated(eventData, result);
            }

            public override void CommandFailed(DbCommand command, CommandErrorEventData eventData)
            {
                Logger.LogError(eventData.Exception, "CommandFailed: {Message}; CommandText={CommandText}, ConnectionId={ConnectionId}, CommandId={CommandId}, EventId={EventId}",
                    command.CommandText, eventData.ConnectionId, eventData.CommandId, eventData.EventId, eventData.Exception.Message);
                base.CommandFailed(command, eventData);
            }

            public override Task CommandFailedAsync(DbCommand command, CommandErrorEventData eventData, CancellationToken cancellationToken = default)
            {
                Logger.LogError(eventData.Exception, "CommandFailedAsync: {Message}; CommandText={CommandText}, ConnectionId={ConnectionId}, CommandId={CommandId}, EventId={EventId}",
                    command.CommandText, eventData.ConnectionId, eventData.CommandId, eventData.EventId, eventData.Exception.Message);
                return base.CommandFailedAsync(command, eventData, cancellationToken);
            }
        }
    }
}

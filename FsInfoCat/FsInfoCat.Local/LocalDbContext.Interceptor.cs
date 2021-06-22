using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    public partial class LocalDbContext
    {
        public class Interceptor : DbCommandInterceptor
        {
            private readonly ILogger<Interceptor> _logger;

            public static Interceptor Instance { get; } = new Interceptor();

            public Interceptor() { _logger = Services.ServiceProvider.GetRequiredService<ILogger<Interceptor>>(); }

            public override InterceptionResult<int> NonQueryExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<int> result)
            {
                _logger.LogInformation("NonQueryExecuting: {CommandText}; ConnectionId={ConnectionId}, CommandId={CommandId}, EventId={EventId},EventIdCode= {EventIdCode}",
                    command.CommandText, eventData.ConnectionId, eventData.CommandId, eventData.EventId, eventData.EventIdCode);
                return base.NonQueryExecuting(command, eventData, result);
            }

            public override ValueTask<InterceptionResult<int>> NonQueryExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<int> result,
                CancellationToken cancellationToken = default)
            {
                _logger.LogInformation("NonQueryExecutingAsync: {CommandText}; ConnectionId={ConnectionId}, CommandId={CommandId}, EventId={EventId},EventIdCode= {EventIdCode}",
                    command.CommandText, eventData.ConnectionId, eventData.CommandId, eventData.EventId, eventData.EventIdCode);
                return base.NonQueryExecutingAsync(command, eventData, result, cancellationToken);
            }

            public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
            {
                _logger.LogInformation("ReaderExecuting: {CommandText}; ConnectionId={ConnectionId}, CommandId={CommandId}, EventId={EventId},EventIdCode= {EventIdCode}",
                    command.CommandText, eventData.ConnectionId, eventData.CommandId, eventData.EventId, eventData.EventIdCode);
                return base.ReaderExecuting(command, eventData, result);
            }

            public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
            {
                _logger.LogInformation("ReaderExecutingAsync: {CommandText}; ConnectionId={ConnectionId}, CommandId={CommandId}, EventId={EventId},EventIdCode= {EventIdCode}",
                    command.CommandText, eventData.ConnectionId, eventData.CommandId, eventData.EventId, eventData.EventIdCode);
                return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
            }

            public override InterceptionResult<object> ScalarExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<object> result)
            {
                _logger.LogInformation("ScalarExecuting: {CommandText}; ConnectionId={ConnectionId}, CommandId={CommandId}, EventId={EventId},EventIdCode= {EventIdCode}",
                    command.CommandText, eventData.ConnectionId, eventData.CommandId, eventData.EventId, eventData.EventIdCode);
                return base.ScalarExecuting(command, eventData, result);
            }

            public override ValueTask<InterceptionResult<object>> ScalarExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<object> result, CancellationToken cancellationToken = default)
            {
                _logger.LogInformation("ScalarExecutingAsync: {CommandText}; ConnectionId={ConnectionId}, CommandId={CommandId}, EventId={EventId},EventIdCode= {EventIdCode}",
                    command.CommandText, eventData.ConnectionId, eventData.CommandId, eventData.EventId, eventData.EventIdCode);
                return base.ScalarExecutingAsync(command, eventData, result, cancellationToken);
            }

            public override DbCommand CommandCreated(CommandEndEventData eventData, DbCommand result)
            {
                _logger.LogInformation("CommandCreated: {CommandText}; ConnectionId={ConnectionId}, CommandId={CommandId}, EventId={EventId},EventIdCode= {EventIdCode}",
                    result.CommandText, eventData.ConnectionId, eventData.CommandId, eventData.EventId, eventData.EventIdCode);
                return base.CommandCreated(eventData, result);
            }

            public override void CommandFailed(DbCommand command, CommandErrorEventData eventData)
            {
                _logger.LogError(eventData.Exception, "CommandFailed: {Message}; CommandText={CommandText}, ConnectionId={ConnectionId}, CommandId={CommandId}, EventId={EventId},EventIdCode= {EventIdCode}",
                    command.CommandText, eventData.ConnectionId, eventData.CommandId, eventData.EventId, eventData.EventIdCode, eventData.Exception.Message);
                base.CommandFailed(command, eventData);
            }

            public override Task CommandFailedAsync(DbCommand command, CommandErrorEventData eventData, CancellationToken cancellationToken = default)
            {
                _logger.LogError(eventData.Exception, "CommandFailedAsync: {Message}; CommandText={CommandText}, ConnectionId={ConnectionId}, CommandId={CommandId}, EventId={EventId},EventIdCode= {EventIdCode}",
                    command.CommandText, eventData.ConnectionId, eventData.CommandId, eventData.EventId, eventData.EventIdCode, eventData.Exception.Message);
                return base.CommandFailedAsync(command, eventData, cancellationToken);
            }
        }
    }
}

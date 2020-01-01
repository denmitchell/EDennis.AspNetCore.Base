using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;


namespace EDennis.AspNetCore.Base.Testing {

    /// <summary>
    /// sample use
    /// </summary>
    internal class X {
        DbContextOptionsBuilder builder;
        public X() {
            builder.AddInterceptors(new IInterceptor[]
                { new PkRewriterInterceptor(
                    new PkRewriter(501,-999000)) 
                });
        }
    }

    public class PkRewriterInterceptor : IDbCommandInterceptor {


        public PkRewriter PkRewriter { get; }

        public PkRewriterInterceptor(PkRewriter pkRewriter) {
            PkRewriter = pkRewriter;
        }

        public DbCommand CommandCreated(CommandEndEventData eventData, DbCommand result) {
            return result;
        }

        public InterceptionResult<DbCommand> CommandCreating(CommandCorrelatedEventData eventData, InterceptionResult<DbCommand> result) {
            return result;
        }

        public void CommandFailed(DbCommand command, CommandErrorEventData eventData) {
            return;
        }

        public Task CommandFailedAsync(DbCommand command, CommandErrorEventData eventData, CancellationToken cancellationToken = default) {
            return Task.CompletedTask;
        }

        public InterceptionResult DataReaderDisposing(DbCommand command, DataReaderDisposingEventData eventData, InterceptionResult result) {
            return result;
        }

        public int NonQueryExecuted(DbCommand command, CommandExecutedEventData eventData, int result) {
            return result;
        }

        public Task<int> NonQueryExecutedAsync(DbCommand command, CommandExecutedEventData eventData, int result, CancellationToken cancellationToken = default) {
            return Task.FromResult(result);
        }

        public InterceptionResult<int> NonQueryExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<int> result) {
            command.CommandText = PkRewriter.Encode(command.CommandText);
            PkRewriter.Encode(command.Parameters);
            return result;
        }

        public Task<InterceptionResult<int>> NonQueryExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default) {
            command.CommandText = PkRewriter.Encode(command.CommandText);
            PkRewriter.Encode(command.Parameters);
            return Task.FromResult(result);
        }

        public DbDataReader ReaderExecuted(DbCommand command, CommandExecutedEventData eventData, DbDataReader result) {
            command.CommandText = PkRewriter.Encode(command.CommandText);
            PkRewriter.Encode(command.Parameters);
            return new PkRewriterDataReader(result, PkRewriter);
        }

        public Task<DbDataReader> ReaderExecutedAsync(DbCommand command, CommandExecutedEventData eventData, DbDataReader result, CancellationToken cancellationToken = default) {
            command.CommandText = PkRewriter.Encode(command.CommandText);
            PkRewriter.Encode(command.Parameters);
            return Task.FromResult(new PkRewriterDataReader(result, PkRewriter) as DbDataReader);
        }

        public InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result) {
            return result;
        }

        public Task<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default) {
            return Task.FromResult(result);
        }

        public object ScalarExecuted(DbCommand command, CommandExecutedEventData eventData, object result) {

            var type = result.GetType();
            if (type == typeof(int))
                return PkRewriter.Decode((int)result);
            else if (type == typeof(short))
                return PkRewriter.Decode((short)result);
            else if (type == typeof(long))
                return PkRewriter.Decode((long)result);
            else if (type == typeof(string))
                return PkRewriter.Decode((string)result);
            else
                return result;
        }

        public Task<object> ScalarExecutedAsync(DbCommand command, CommandExecutedEventData eventData, object result, CancellationToken cancellationToken = default) {
            var type = result.GetType();
            if (type == typeof(int))
                return Task.FromResult((object)PkRewriter.Decode((int)result));
            else if (type == typeof(short))
                return Task.FromResult((object)PkRewriter.Decode((short)result));
            else if (type == typeof(long))
                return Task.FromResult((object)PkRewriter.Decode((long)result));
            else if (type == typeof(string))
                return Task.FromResult((object)PkRewriter.Decode((string)result));
            else
                return Task.FromResult(result);
        }

        public InterceptionResult<object> ScalarExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<object> result) {
            command.CommandText = PkRewriter.Encode(command.CommandText);
            PkRewriter.Encode(command.Parameters);
            return result;
        }

        public Task<InterceptionResult<object>> ScalarExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<object> result, CancellationToken cancellationToken = default) {
            command.CommandText = PkRewriter.Encode(command.CommandText);
            PkRewriter.Encode(command.Parameters);
            return Task.FromResult(result);
        }
    }
}

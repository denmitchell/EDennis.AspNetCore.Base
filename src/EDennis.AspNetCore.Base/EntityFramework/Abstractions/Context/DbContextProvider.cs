using EDennis.AspNetCore.Base;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Data.Common;

namespace EDennis.AspNetCore.Base.EntityFramework {
    public class DbContextProvider<TContext>
        where TContext : DbContext {
        public TContext Context { get; set; }


        public DbContextProvider(TContext context, StoredProcedureDefs<TContext> spDefs) {
            Context = context;
            if (typeof(ISqlServerDbContext<TContext>).IsAssignableFrom(typeof(TContext))) {
                (Context as ISqlServerDbContext<TContext>).StoredProcedureDefs = spDefs;
            }
        }


        /// <summary>
        /// Use this static method with AddDbContext(...) in order to build the 
        /// DbContextOptionsBuilder using settings (typically from configuration).
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="settings"></param>
        public static void BuildBuilder(DbContextOptionsBuilder builder, DbContextSettings<TContext> settings) {

            switch (settings.DatabaseProvider) {
                case DatabaseProvider.SqlServer:
                    ConfigureForSqlServer(builder, settings.ConnectionString);
                    break;
                case DatabaseProvider.Sqlite:
                    ConfigureForSqlite(builder, settings.ConnectionString);
                    break;
                case DatabaseProvider.InMemory:
                    ConfigureForInMemory(builder);
                    break;
            }

        }

    

        /// <summary>
        /// Use this static method to generate a new DbContext based upon settings
        /// and, whenever possible, using the provided DbConnection and DbTransaction
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <param name="instanceName"></param>
        /// <returns></returns>
        public static TContext GetInterceptorContext(DbContextSettings<TContext> settings, 
            CachedConnection<TContext> cachedConnection) {

            var interceptorSettings = settings.Interceptor;
            interceptorSettings.ConnectionString = interceptorSettings.ConnectionString ?? settings.ConnectionString;
            interceptorSettings.DatabaseProvider = (interceptorSettings.DatabaseProvider == DatabaseProvider.Unspecified) ? settings.DatabaseProvider : interceptorSettings.DatabaseProvider;

            var builder = new DbContextOptionsBuilder<TContext>();

            switch (interceptorSettings.DatabaseProvider) {
                case DatabaseProvider.SqlServer:
                    ConfigureForSqlServer(builder, interceptorSettings, cachedConnection);
                    break;
                case DatabaseProvider.Sqlite:
                    ConfigureForSqlite(builder, interceptorSettings, cachedConnection);
                    break;
                case DatabaseProvider.InMemory:
                    ConfigureForInMemory(builder, cachedConnection);
                    break;
            }


            var context = (TContext)Activator.CreateInstance(typeof(TContext), new object[] { builder.Options });

            context.Database.AutoTransactionsEnabled = false;
            context.Database.UseTransaction(cachedConnection.DbTransaction);

            return context;

        }


        private static void ConfigureForSqlServer(DbContextOptionsBuilder builder, string connectionString) {
            builder.UseSqlServer(connectionString);
        }

        private static void ConfigureForSqlServer(DbContextOptionsBuilder builder, DbContextInterceptorSettings<TContext> settings, CachedConnection<TContext> cachedConnection) {
            if (cachedConnection.DbConnection == null) {
                cachedConnection.DbConnection = new SqlConnection(settings.ConnectionString);
                cachedConnection.DbConnection.Open();
                cachedConnection.DbTransaction = cachedConnection.DbConnection.BeginTransaction(settings.IsolationLevel);
            }
            builder.UseSqlServer(cachedConnection.DbConnection as SqlConnection);
            if (settings.EnableSensitiveDataLogging)
                builder.EnableSensitiveDataLogging();
        }

        private static void ConfigureForSqlite(DbContextOptionsBuilder builder, string connectionString) {
            builder.UseSqlite(connectionString);
        }


        private static void ConfigureForSqlite(DbContextOptionsBuilder builder, DbContextInterceptorSettings<TContext> settings, CachedConnection<TContext> cachedConnection) {
            if (cachedConnection.DbConnection == null) {
                cachedConnection.DbConnection = new SqlConnection(settings.ConnectionString);
                cachedConnection.DbConnection.Open();
                cachedConnection.DbTransaction = cachedConnection.DbConnection.BeginTransaction(settings.IsolationLevel);
            }
            builder.UseSqlite(cachedConnection.DbConnection as SqliteConnection);
            if (settings.EnableSensitiveDataLogging)
                builder.EnableSensitiveDataLogging();
        }


        private static void ConfigureForInMemory(DbContextOptionsBuilder builder) {
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        }

        private static void ConfigureForInMemory(DbContextOptionsBuilder builder, CachedConnection<TContext> cachedConnection) {
            if (cachedConnection.InstanceName == null)
                cachedConnection.InstanceName = Guid.NewGuid().ToString();

            builder.UseInMemoryDatabase(cachedConnection.InstanceName);
        }


        /// <summary>
        /// Resets a connection/transaction.  This should be used in nonproduction
        /// scenarios only.
        /// </summary>
        public static void Reset(DbContextSettings<TContext> settings,
            CachedConnection<TContext> cachedConnection, ILogger logger = null) {

            var interceptorSettings = settings.Interceptor;
            interceptorSettings.ConnectionString = interceptorSettings.ConnectionString ?? settings.ConnectionString;
            interceptorSettings.DatabaseProvider = (interceptorSettings.DatabaseProvider == DatabaseProvider.Unspecified) ? settings.DatabaseProvider : interceptorSettings.DatabaseProvider;

            var canRollback = (cachedConnection.DbConnection != null && cachedConnection.DbConnection.State == ConnectionState.Open && cachedConnection.DbTransaction != null);
            switch (interceptorSettings.DatabaseProvider) {
                case DatabaseProvider.SqlServer:
                    if (canRollback) {
                        cachedConnection.DbTransaction.Rollback();
                        if (logger != null)
                            logger.LogInformation("Db Interceptor rolling back {DbContext}-{Instance}", typeof(TContext).Name, cachedConnection.InstanceName ?? "");
                    }
                    if (interceptorSettings.ResetSqlServerSequences)
                        ResetSqlServerSequences(interceptorSettings);
                    if (interceptorSettings.ResetSqlServerIdentities)
                        ResetSqlServerIdentities(interceptorSettings);
                    if (cachedConnection.DbConnection == null)
                        cachedConnection.DbConnection = new SqlConnection(interceptorSettings.ConnectionString);
                    if (cachedConnection.DbConnection.State == ConnectionState.Closed)
                        cachedConnection.DbConnection.Open();
                    cachedConnection.DbTransaction = cachedConnection.DbConnection.BeginTransaction();
                    break;
                case DatabaseProvider.Sqlite:
                    if (logger != null)
                        logger.LogInformation("Db Interceptor resetting {DbContext}-{Instance}", typeof(TContext).Name, cachedConnection.InstanceName ?? "");
                    if (canRollback) {
                        cachedConnection.DbTransaction.Rollback();
                        if (logger != null)
                            logger.LogInformation("Db Interceptor rolling back {DbContext}-{Instance}", typeof(TContext).Name, cachedConnection.InstanceName ?? "");
                    }
                    if (cachedConnection.DbConnection == null)
                        cachedConnection.DbConnection = new SqliteConnection(interceptorSettings.ConnectionString);
                    if (cachedConnection.DbConnection.State == ConnectionState.Closed)
                        cachedConnection.DbConnection.Open();
                    cachedConnection.DbTransaction = cachedConnection.DbConnection.BeginTransaction();
                    break;
                case DatabaseProvider.InMemory:
                    if (logger != null)
                        logger.LogInformation("Db Interceptor resetting {DbContext}-{Instance}", typeof(TContext).Name, cachedConnection.InstanceName ?? "");
                    cachedConnection.InstanceName = Guid.NewGuid().ToString();
                    break;
            }
        }


        public static void ResetSqlServerIdentities(DbContextInterceptorSettings<TContext> interceptorSettings) {
            using var connection = new SqlConnection(interceptorSettings.ConnectionString);
            using var command = new SqlCommand(RESET_IDENTITIES, connection);
            command.ExecuteNonQuery();
        }

        public static void ResetSqlServerSequences(DbContextInterceptorSettings<TContext> interceptorSettings) {
            using var connection = new SqlConnection(interceptorSettings.ConnectionString);
            using var command = new SqlCommand(RESET_SEQUENCES, connection);
            command.ExecuteNonQuery();
        }

        public const string RESET_IDENTITIES = @"

declare @SchemaName varchar(255)
declare @TableName varchar(255)
declare @ColumnName varchar(255)
declare @sql nvarchar(255)
declare @max bigint
declare @adjust bit

declare crsr cursor for
	select s.name SchemaName, t.name TableName, i.name ColumnName
		from sys.schemas s
		inner join sys.tables t
			on s.schema_id = t.schema_id
		inner join sys.identity_columns i
			on i.object_id = t.object_id
open crsr
fetch next from crsr 
	into @SchemaName, @TableName, @ColumnName
while @@fetch_status = 0
	begin
		if @SchemaName is not null 
			begin
				set @sql = N'select @max = max(' + @ColumnName + ') from [' + @SchemaName + '].[' + @TableName + '];'
				exec sp_executesql @sql,
					N'@max bigint OUTPUT',
					@max OUTPUT;

				set @sql = N'select @adjust = case when last_value is not null then 1 else 0 end from sys.identity_columns WHERE object_id = object_id(' + char(39) + '[' + @SchemaName + '].[' + @TableName + ']' + char(39) + ') ;'
				exec sp_executesql @sql,
					N'@adjust bit OUTPUT',
					@adjust OUTPUT;

				set @sql = 	'dbcc checkident (' + char(39) + @SchemaName + '.' + @TableName + char(39) + ', reseed, ' + convert(varchar(20), isnull(@max,1) - isnull(@adjust,0)) + ')'
				exec (@sql)
			end
		fetch next from crsr 
			into @SchemaName, @TableName, @ColumnName
	end
close crsr
deallocate crsr
";


        public const string RESET_SEQUENCES = @"

	declare @SequenceSchema varchar(255), @TableSchema varchar(255)
	declare @SequenceName varchar(255), @TableName nvarchar(255), @ColumnName nvarchar(255)
	declare @NextValueSql nvarchar(max), @SqlParamDef nvarchar(max), @NextValue int

	set @SqlParamDef = N'@NextValue int OUTPUT'

	declare @seqMax table(
		SequenceSchema varchar(255),
		SequenceName varchar(255),
		MaxValue int
	);

	declare @ResetSql nvarchar(max)

--
-- Get all sequence values used as defaults for columns, and use dynamic SQL to
-- calculate the maximum value of columns using that sequence as a default
--
	declare c_seqtab cursor for
	select 
		s.sequence_schema SequenceSchema, s.sequence_name SequenceName, 
		c.table_schema TableSchema, c.table_name TableName, c.column_name ColumnName,
		'select @NextValue = max([' + c.column_name + ']) + 1 from [' + c.table_schema + '].[' + c.table_name + ']' NextValueSql
		from INFORMATION_SCHEMA.COLUMNS c
		inner join INFORMATION_SCHEMA.SEQUENCES s
			on rtrim(replace(replace(replace(replace(replace(c.COLUMN_DEFAULT,'[',''),']',''),')',''),'(',''),'dbo',''))
				LIKE '%' + 
				case when s.sequence_schema <> 'dbo' then s.sequence_schema + '.' else '' end +  s.SEQUENCE_NAME 
	open c_seqtab
	fetch next from c_seqtab into @SequenceSchema, @SequenceName, @TableSchema, @TableName, @ColumnName, @NextValueSql
	while @@FETCH_STATUS = 0
	begin
		if @SequenceSchema is not null
		begin

			exec sp_executesql	@NextValueSql, 
								@SqlParamDef,
								@NextValue = @NextValue OUTPUT;
			insert into @seqMax (SequenceSchema, SequenceName, MaxValue)
				values (@SequenceSchema, @SequenceName, ISNULL(@NextValue,1))
		end
		fetch next from c_seqtab into @SequenceSchema, @SequenceName, @TableSchema, @TableName, @ColumnName, @NextValueSql	
	end
	close c_seqtab
	deallocate c_seqtab

--
-- Because sequences may be used by more than one table, get the maximum value across all usages,
-- and use that value to reset the sequence
--
	declare @MaxValue int

	declare c_seqmax cursor for
		select SequenceSchema, SequenceName, max(MaxValue) MaxValue
		from @seqMax
		group by SequenceSchema, SequenceName

	open c_seqmax
	fetch next from c_seqmax into @SequenceSchema, @SequenceName, @MaxValue
	while @@fetch_status = 0
	begin
		if @SequenceSchema is not null
		begin
			set @ResetSql = 'alter sequence [' + @SequenceSchema + '].[' + @SequenceName + '] restart with ' + convert(varchar,ISNULL(@MaxValue,1))	
			exec sp_executesql @ResetSql;			
		end
		fetch next from c_seqmax into @SequenceSchema, @SequenceName, @MaxValue
	end
	close c_seqmax
	deallocate c_seqmax

";


    }
}

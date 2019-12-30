using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;


namespace EDennis.AspNetCore.Base.EntityFramework {
    public class DbConnectionManager {



        public static DbConnection<TContext> GetDbConnection<TContext>(DbContextInterceptorSettings<TContext> settings)
            where TContext : DbContext {

            return (settings.DatabaseProvider) switch
            {
                DatabaseProvider.SqlServer => GetSqlServerDbConnection(settings),
                DatabaseProvider.Sqlite => GetSqliteDbConnection(settings),
                DatabaseProvider.InMemory => GetInMemoryDbConnection<TContext>(),
                _ => null
            };
        }


        public static DbContextOptionsBuilder ConfigureDbContextOptionsBuilder<TContext>(DbContextOptionsBuilder builder, DbContextSettings<TContext> settings)
            where TContext : DbContext {

            return (settings.DatabaseProvider) switch
            {
                DatabaseProvider.SqlServer => builder.UseSqlServer(settings.ConnectionString),
                DatabaseProvider.Sqlite => builder.UseSqlite(settings.ConnectionString),
                DatabaseProvider.InMemory => builder.UseInMemoryDatabase(Guid.NewGuid().ToString()),
                _ => null
            };

        }


        public static TContext GetDbContext<TContext>(DbConnection<TContext> dbConnection, DbContextSettings<TContext> settings)
            where TContext : DbContext {

            var builder = new DbContextOptionsBuilder<TContext>();
            builder = (settings.DatabaseProvider) switch
            {
                DatabaseProvider.SqlServer => builder.UseSqlServer(dbConnection.IDbConnection as SqlConnection),
                DatabaseProvider.Sqlite => builder.UseSqlite(dbConnection.IDbConnection as SqliteConnection),
                DatabaseProvider.InMemory => builder.UseInMemoryDatabase(Guid.NewGuid().ToString()),
                _ => null
            };

            var dbContextOptionsProvider = new DbContextOptionsProvider<TContext>(builder.Options);
            var context = (TContext)Activator.CreateInstance(typeof(TContext), new object[] { dbContextOptionsProvider });
            return context;
        }


        public static TContext GetDbContext<TContext>(DbContextSettings<TContext> settings)
            where TContext : DbContext {

            var builder = new DbContextOptionsBuilder<TContext>();
            builder = (settings.DatabaseProvider) switch
            {
                DatabaseProvider.SqlServer => builder.UseSqlServer(settings.ConnectionString),
                DatabaseProvider.Sqlite => builder.UseSqlite(settings.ConnectionString),
                DatabaseProvider.InMemory => builder.UseInMemoryDatabase(Guid.NewGuid().ToString()),
                _ => null
            };

            var dbContextOptionsProvider = new DbContextOptionsProvider<TContext>(builder.Options);
            var context = (TContext)Activator.CreateInstance(typeof(TContext), new object[] { dbContextOptionsProvider });
            return context;
        }



        public static DbConnection<TContext> GetInMemoryDbConnection<TContext>()
                    where TContext : DbContext {

            var builder = new DbContextOptionsBuilder<TContext>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString());

            var dbContextOptionsProvider = new DbContextOptionsProvider<TContext>(builder.Options);
            var context = (TContext)Activator.CreateInstance(typeof(TContext), new object[] { dbContextOptionsProvider });
            context.Database.EnsureCreated();

            return
                new DbConnection<TContext> {
                    DbContextOptionsBuilder = builder,
                    IDbConnection = null,
                    IDbTransaction = null
                };

        }


        public static DbConnection<TContext> GetSqlServerDbConnection<TContext>(DbContextInterceptorSettings<TContext> settings)
            where TContext : DbContext {

            var dbConnection = new DbConnection<TContext>();
            var builder = new DbContextOptionsBuilder<TContext>();

            dbConnection.IDbConnection = new SqlConnection(settings.ConnectionString);
            dbConnection.IDbConnection.Open();
            dbConnection.IDbTransaction = dbConnection.IDbConnection.BeginTransaction(settings.IsolationLevel);

            builder.UseSqlServer(dbConnection.IDbConnection as SqlConnection);
            dbConnection.DbContextOptionsBuilder = builder;
            return dbConnection;
        }

        public static DbConnection<TContext> GetSqliteDbConnection<TContext>(DbContextInterceptorSettings<TContext> settings)
            where TContext : DbContext {

            var dbConnection = new DbConnection<TContext>();
            var builder = new DbContextOptionsBuilder<TContext>();

            dbConnection.IDbConnection = new SqlConnection(FixSqliteConnectionString(settings.ConnectionString, settings.IsolationLevel));
            dbConnection.IDbConnection.Open();
            dbConnection.IDbTransaction = dbConnection.IDbConnection.BeginTransaction(settings.IsolationLevel);

            builder.UseSqlite(dbConnection.IDbConnection as SqlConnection);
            dbConnection.DbContextOptionsBuilder = builder;
            return dbConnection;
        }


        public static string FixSqliteConnectionString(string connectionString, IsolationLevel? isolationLevel) {
            //fix Sqlite
            var cxnString = connectionString;
            if (isolationLevel == IsolationLevel.ReadUncommitted &&
                !Regex.IsMatch(connectionString, "cache\\s*=\\s*shared", RegexOptions.IgnoreCase))
                cxnString = (connectionString + ";cache=shared").Replace(";;", ";");

            return cxnString;

        }


        public static void Reset<TContext>(IDbConnection connection, DbContextSettings<TContext> settings)
            where TContext : DbContext {
            if (settings.Interceptor.ResetSqlServerIdentities)
                ResetSqlServerIdentities(connection);
            if (settings.Interceptor.ResetSqlServerSequences)
                ResetSqlServerSequences(connection);
        }

        public static void Reset<TContext>(IDbConnection connection, DbContextInterceptorSettings<TContext> settings)
            where TContext : DbContext {
            if (settings.ResetSqlServerIdentities)
                ResetSqlServerIdentities(connection);
            if (settings.ResetSqlServerSequences)
                ResetSqlServerSequences(connection);
        }


        public static void ResetSqlServerIdentities(IDbConnection connection) {
            connection.Open();
            using var command = new SqlCommand(RESET_IDENTITIES);
            command.ExecuteNonQuery();
        }

        public static void ResetSqlServerSequences(IDbConnection connection) {
            connection.Open();
            using var command = new SqlCommand(RESET_SEQUENCES);
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

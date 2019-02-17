using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;

namespace EDennis.AspNetCore.Base.EntityFramework {

    /// <summary>
    /// This class provides SqlConnection extension methods
    /// that reset identity values and reset sequence values
    /// in a SQL Server database.  This is especially useful
    /// for situations in which a transaction is rolled back.
    /// (The rollback of a transaction does not reset
    /// identities or sequences by itself.)
    /// </summary>
    public static class SqlConnectionExtensions {

        /// <summary>
        /// Resets all identity values
        /// </summary>
        /// <param name="connection">SqlConnection</param>
        public static void ResetIdentities(this SqlConnection connection) {
            using (SqlCommand cmd = new SqlCommand(sqlResetIdentities, connection)) {
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Resets all sequence values
        /// </summary>
        /// <param name="connection">SqlConnection</param>
        public static void ResetSequences(this SqlConnection connection) {
            using (SqlCommand cmd = new SqlCommand(sqlResetSequences, connection)) {
                cmd.ExecuteNonQuery();
            }
        }


        public static void RollbackAll(this SqlConnection connection, SqlTransaction trans) {
            var sql = $"IF EXISTS (SELECT * FROM sys.sysprocesses WHERE open_tran = 1) BEGIN ALTER DATABASE {connection.Database} SET SINGLE_USER WITH ROLLBACK IMMEDIATE; ALTER DATABASE {connection.Database} SET MULTI_USER; END";

            trans.Rollback();
            using (SqlCommand cmd = new SqlCommand(sql, connection)) {
                cmd.ExecuteNonQuery();
            }
        }

        public static void DropDatabase(this SqlConnection connection) {
            var db = connection.Database;
            var sql = $"USE MASTER; DROP DATABASE {db}";
            using (SqlCommand cmd = new SqlCommand(sql, connection)) {
                cmd.ExecuteNonQuery();
            }
        }


        //SQL script for resetting identities
        private static readonly string sqlResetIdentities =
@"
    declare @SchemaName varchar(255)
    declare @TableName varchar(255)
    declare @ColumnName varchar(255)
    declare @sql nvarchar(255)
    declare @max bigint

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
				    set @sql = 	'dbcc checkident (' + char(39) + @SchemaName + '.' + @TableName + char(39) + ', reseed, ' + convert(varchar(20), isnull(@max,1)) + ')'
				    exec (@sql)
			    end
		    fetch next from crsr 
			    into @SchemaName, @TableName, @ColumnName
	    end
    close crsr
    deallocate crsr

";

        //SQL script for resetting sequences
        private static readonly string sqlResetSequences =
@"
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

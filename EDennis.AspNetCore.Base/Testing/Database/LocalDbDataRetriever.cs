using System.Collections.Generic;
using Dapper;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System;

namespace EDennis.AspNetCore.Base.Testing {
    public static class DataRetriever {

        public static TEntity[] Retrieve<TContext,TEntity>(TContext context)
            where TContext: DbContext
            where TEntity: class, new() {
            var data = context.Query<TEntity>()
                .AsNoTracking()
                .ToList();
            return data.ToArray();
        }


        public static TEntity[] Retrieve<TContext, TEntity>(
            TContext context, 
            Expression<Func<TEntity, bool>> linqExpression, 
            int pageNumber, int pageSize)
            where TContext : DbContext
            where TEntity : class, new() {
            var data = context.Query<TEntity>()
                .AsNoTracking()
                .ToList();
            return data.ToArray();
        }


        public static TEntity[] Retrieve<TEntity>(string dbName, string tableName) {
            var server = "(localdb)\\mssqllocaldb";
            return Retrieve<TEntity>(dbName, tableName, server);
        }

        public static TEntity[] Retrieve<TEntity>(string dbName, string tableName, string server) {
            var sqlCols = GetColumnSql(tableName);
            var sql = $"select * from {tableName}";
            using (var cxn = new SqlConnection($"Server={server};Database={dbName};Trusted_Connection=True;")) {
                var cols = cxn.Query<string>(sqlCols).AsList().ToArray();
                sql = sql.Replace("*", string.Join(',', cols));
                var data = cxn.Query<TEntity>(sql).AsList().ToArray();
                return data;
            }
        }

        public static TEntity[] Retrieve<TEntity>(string dbName, string tableName, string server, string sql) {
            using (var cxn = new SqlConnection($"Server={server};Database={dbName};Trusted_Connection=True;")) {
                var data = cxn.Query<TEntity>(sql).AsList().ToArray();
                return data;
            }
        }



        private static string GetColumnSql(string tableName) =>
    $@"
select c.name 
	from sys.columns c 
	inner join sys.objects o
		on c.object_id = o.object_id
	inner join sys.schemas s
		on s.schema_id = o.schema_id
	inner join information_schema.columns ic
		on ic.column_name = c.name
		and ic.table_name = o.name
		and ic.table_schema = s.name
	where c.object_id = object_id('{tableName}','U') 
		and is_computed = 0 
		and generated_always_type_desc = 'NOT_APPLICABLE'	
	order by ic.ordinal_position;
";

    }


}

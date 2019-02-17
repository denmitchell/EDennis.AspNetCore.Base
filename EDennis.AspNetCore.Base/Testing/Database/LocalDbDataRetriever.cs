using System.Collections.Generic;
using Dapper;
using System.Data.SqlClient;

namespace EDennis.AspNetCore.Base.Testing {
    public static class LocalDbDataRetriever {
        public static TEntity[] Retrieve<TEntity>(string dbName, string tableName) {
            var sqlCols =
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
            var sql = $"select * from {tableName}";
            using (var cxn = new SqlConnection($"Server=(localdb)\\mssqllocaldb;Database={dbName};Trusted_Connection=True;")) {
                var cols = cxn.Query<string>(sqlCols).AsList().ToArray();
                sql = sql.Replace("*", string.Join(',', cols));
                var data = cxn.Query<TEntity>(sql).AsList().ToArray();
                return data;
            }
        }
    }
}

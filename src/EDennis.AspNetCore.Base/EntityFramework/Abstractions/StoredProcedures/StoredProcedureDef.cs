using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace EDennis.AspNetCore.Base.EntityFramework {
    public class StoredProcedureDef {


        public string Schema { get; set; }
        public string StoredProcedureName { get; set; }
        public string ParameterName { get; set; }
        public int Order { get; set; }
        public string DbTypeName { get; set; }
        public DbType DbType() => TypeMap.Types.Single(a => a.DbTypeName == DbTypeName).DbType;
        public Type ClrType() => TypeMap.Types.Single(a => a.DbTypeName == DbTypeName).ClrType;
        public bool IsOutput { get; set; }
        public bool IsReadonly { get; set; }
        public bool IsNullable { get; set; }
        public bool HasDefaultValue { get; set; }
        public int? Length { get; set; }
        public int? Precision { get; set; }
        public int? Scale { get; set; }


        public DbParameter ToDbParameter(object parmValue) {
            DbParameter parm = new Microsoft.Data.SqlClient.SqlParameter {
                ParameterName = ParameterName,
                DbType = DbType()
            };
            parm.Direction = GetParameterDirection(parmValue);
            if (parm.Direction != ParameterDirection.Output)
                try {
                    parm.Value = Convert.ChangeType(parmValue, ClrType());
                } catch {
                    throw new ArgumentException($"Cannot convert the supplied parameter value '{parmValue}' for {parm.ParameterName} for Stored Procedure {StoredProcedureName}");
                }
            return parm;
        }


        public ParameterDirection GetParameterDirection(object parmValue) {
            if (IsOutput)
                if (parmValue == null)
                    return ParameterDirection.Output;
                else
                    return ParameterDirection.InputOutput;
            else
                return ParameterDirection.Input;

        }

    }


    internal class TypeMap {
        public readonly static List<TypeMap> Types = new List<TypeMap>() {

        new TypeMap { DbTypeName = "bit", DbType = DbType.Boolean, ClrType = typeof(bool) },
        new TypeMap { DbTypeName = "tinyint", DbType = DbType.Byte, ClrType =  typeof(byte) },
        new TypeMap { DbTypeName = "smallint", DbType = DbType.Int16, ClrType =  typeof(int) },
        new TypeMap  { DbTypeName = "int", DbType = DbType.Int32, ClrType =  typeof(int) },
        new TypeMap  { DbTypeName = "bigint", DbType = DbType.Int64, ClrType =  typeof(long) },

        new TypeMap  { DbTypeName = "decimal", DbType = DbType.Decimal, ClrType =  typeof(decimal) },
        new TypeMap  { DbTypeName = "numeric", DbType = DbType.Decimal, ClrType =  typeof(decimal) },
        new TypeMap  { DbTypeName = "money", DbType = DbType.Decimal, ClrType =  typeof(decimal) },
        new TypeMap  { DbTypeName = "smallmoney", DbType = DbType.Decimal, ClrType =  typeof(decimal) },
        new TypeMap  { DbTypeName = "float", DbType = DbType.Double, ClrType =  typeof(double) },
        new TypeMap  { DbTypeName = "real", DbType = DbType.Single, ClrType =  typeof(float) },

        new TypeMap  { DbTypeName = "varchar", DbType = DbType.String, ClrType = typeof(string) },
        new TypeMap  { DbTypeName = "nvarchar", DbType = DbType.String, ClrType = typeof(string) },
        new TypeMap  { DbTypeName = "char", DbType = DbType.String, ClrType = typeof(string) },
        new TypeMap  { DbTypeName = "nchar", DbType = DbType.String, ClrType = typeof(string) },
        new TypeMap  { DbTypeName = "text", DbType = DbType.String, ClrType = typeof(string) },
        new TypeMap  { DbTypeName = "ntext", DbType = DbType.String, ClrType = typeof(string) },

        new TypeMap  { DbTypeName = "smalldatetime", DbType = DbType.DateTime, ClrType = typeof(DateTime) },
        new TypeMap  { DbTypeName = "date", DbType = DbType.Date, ClrType = typeof(DateTime) },
        new TypeMap  { DbTypeName = "datetime", DbType = DbType.DateTime, ClrType = typeof(DateTime) },
        new TypeMap  { DbTypeName = "datetime2", DbType = DbType.DateTime2, ClrType = typeof(DateTime) },
        new TypeMap  { DbTypeName = "datetimeoffset", DbType = DbType.DateTimeOffset, ClrType = typeof(DateTimeOffset) },
        new TypeMap  { DbTypeName = "time", DbType = DbType.Time, ClrType = typeof(TimeSpan) },

        new TypeMap  { DbTypeName ="uniqueidentifier", DbType = DbType.Guid, ClrType = typeof(Guid) }
        };

        public string DbTypeName { get; set; }
        public DbType DbType { get; set; }
        public Type ClrType { get; set; }
    }


}

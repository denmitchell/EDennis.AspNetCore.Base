using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Text.Json;

namespace EDennis.AspNetCore.Base.Testing.Database {
    public class PkRewriterDataReader : DbDataReader {

        public PkRewriter PkRewriter { get; }
        public DbDataReader BaseReader { get; }

        public PkRewriterDataReader(DbDataReader baseReader, PkRewriter pkRewriter) {
            PkRewriter = pkRewriter;
            BaseReader = baseReader;
        }

        public override object this[int ordinal] => BaseReader[ordinal];

        public override object this[string name] => BaseReader[name];

        public override int Depth => BaseReader.Depth;

        public override int FieldCount => BaseReader.FieldCount;

        public override bool HasRows => BaseReader.HasRows;

        public override bool IsClosed => BaseReader.IsClosed;

        public override int RecordsAffected => BaseReader.RecordsAffected;

        public override bool GetBoolean(int ordinal) => BaseReader.GetBoolean(ordinal);

        public override byte GetByte(int ordinal) => BaseReader.GetByte(ordinal);

        public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
            => BaseReader.GetBytes(ordinal, dataOffset, buffer, bufferOffset, length);

        public override char GetChar(int ordinal) => BaseReader.GetChar(ordinal);

        public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
            => BaseReader.GetChars(ordinal, dataOffset, buffer, bufferOffset, length);

        public override string GetDataTypeName(int ordinal) => BaseReader.GetDataTypeName(ordinal);

        public override DateTime GetDateTime(int ordinal) => BaseReader.GetDateTime(ordinal);

        public override decimal GetDecimal(int ordinal) => BaseReader.GetDecimal(ordinal);

        public override double GetDouble(int ordinal) => BaseReader.GetDouble(ordinal);

        public override IEnumerator GetEnumerator() => BaseReader.GetEnumerator();

        public override Type GetFieldType(int ordinal) => BaseReader.GetFieldType(ordinal);

        public override float GetFloat(int ordinal) => BaseReader.GetFloat(ordinal);

        public override Guid GetGuid(int ordinal) => BaseReader.GetGuid(ordinal);

        public override short GetInt16(int ordinal) {
            var value = BaseReader.GetInt16(ordinal);
            var adjusted = PkRewriter.Decode(value);
            return adjusted;
        }

        public override int GetInt32(int ordinal) {
            var value = BaseReader.GetInt32(ordinal);
            var adjusted = PkRewriter.Decode(value);
            return adjusted;
        }

        public override long GetInt64(int ordinal) {
            var value = BaseReader.GetInt64(ordinal);
            var adjusted = PkRewriter.Decode(value);
            return adjusted;
        }

        public override string GetName(int ordinal) => BaseReader.GetName(ordinal);

        public override int GetOrdinal(string name) => BaseReader.GetOrdinal(name);

        public override string GetString(int ordinal) {
            var value = BaseReader.GetString(ordinal);
            var adjusted = PkRewriter.Decode(value);
            return adjusted;
        }

        public override object GetValue(int ordinal) {
            var type = GetFieldType(ordinal);
            if (type == typeof(int))
                return GetInt32(ordinal);
            else if (type == typeof(short))
                return GetInt16(ordinal);
            else if (type == typeof(long))
                return GetInt64(ordinal);
            else if (type == typeof(string))
                return GetString(ordinal);
            else
                return BaseReader.GetValue(ordinal);
        }

        public override int GetValues(object[] values) {
            if (values != null) {
                for (int i = 0; i < values.Length; i++)
                    values[i] = GetValue(i);
                return values.Length;
            } else {
                return 0;
            }
        }

        public override bool IsDBNull(int ordinal) => BaseReader.IsDBNull(ordinal);

        public override bool NextResult() => BaseReader.NextResult();

        public override bool Read() => BaseReader.Read();
    }
}

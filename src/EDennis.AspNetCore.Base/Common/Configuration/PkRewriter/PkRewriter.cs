using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EDennis.AspNetCore.Base {

    public class PkRewriter {

        public const int BASE_PREFIX_DEFAULT = -999;
        public string BasePrefix { get; }
        public string DeveloperPrefix { get; }

        public string Encode(string s) => s.Replace(BasePrefix, DeveloperPrefix);        
        public string Decode(string s) => s.Replace(DeveloperPrefix, BasePrefix);
        public int Encode(int i) => int.Parse(Encode(i.ToString()));
        public int Decode(int i) => int.Parse(Decode(i.ToString()));
        public long Encode(long i) => long.Parse(Encode(i.ToString()));
        public long Decode(long i) => long.Parse(Decode(i.ToString()));
        public short Encode(short i) => short.Parse(Encode(i.ToString()));
        public short Decode(short i) => short.Parse(Decode(i.ToString()));


        public void Encode(DbParameterCollection parameters) {
            foreach (DbParameter parameter in parameters)
                switch (parameter.DbType) {
                    case DbType.String:
                        parameter.Value = Encode(parameter.Value.ToString());
                        break;
                    case DbType.Int16:
                        parameter.Value = Encode((short)parameter.Value);
                        break;
                    case DbType.Int32:
                        parameter.Value = Encode((int)parameter.Value);
                        break;
                    case DbType.Int64:
                        parameter.Value = Encode((long)parameter.Value);
                        break;
                }
        }


        public PkRewriter(int developerPrefix, int basePrefix = BASE_PREFIX_DEFAULT) {
            BasePrefix = basePrefix.ToString();
            DeveloperPrefix = developerPrefix.ToString();
        }

    }
}

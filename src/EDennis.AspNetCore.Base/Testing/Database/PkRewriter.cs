using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace EDennis.AspNetCore.Base.Testing {

    public class PkRewriter {
        public int BaseMultiplier { get; }
        public string BasePrefix { get; }
        public int DeveloperMultiplier { get; }
        public string DeveloperPrefix { get; }
        public int LeastCommonDenominator { get; }

        public string Encode(string s) => s.Replace(BasePrefix, DeveloperPrefix);
        public string Decode(string s) => s.Replace(DeveloperPrefix, BasePrefix);

        public int Encode(int i) {
            if (i >= BaseMultiplier && i < (BaseMultiplier + LeastCommonDenominator))
                return i - BaseMultiplier + DeveloperMultiplier;
            else
                return i;
        }

        public int Decode(int i) {
            if (i >= DeveloperMultiplier && i < (DeveloperMultiplier + LeastCommonDenominator))
                return i - DeveloperMultiplier + BaseMultiplier;
            else
                return i;
        }


        public long Encode(long i) {
            if (i >= BaseMultiplier && i < (BaseMultiplier + LeastCommonDenominator))
                return i - BaseMultiplier + DeveloperMultiplier;
            else
                return i;
        }

        public long Decode(long i) {
            if (i >= DeveloperMultiplier && i < (DeveloperMultiplier + LeastCommonDenominator))
                return i - DeveloperMultiplier + BaseMultiplier;
            else
                return i;
        }

        public short Encode(short i) {
            if (i >= BaseMultiplier && i < (BaseMultiplier + LeastCommonDenominator))
                return (short)(i - BaseMultiplier + DeveloperMultiplier);
            else
                return i;
        }

        public short Decode(short i) {
            if (i >= DeveloperMultiplier && i < (DeveloperMultiplier + LeastCommonDenominator))
                return (short)(i - DeveloperMultiplier + BaseMultiplier);
            else
                return i;
        }

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


        public PkRewriter(int baseMultiplier, int developerPrefix) {

            BaseMultiplier = baseMultiplier;
            var bm = baseMultiplier.ToString();
            var mag = bm
                .Substring(1 + bm.LastIndexOfAny(
                    new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9' }))
                .Count();

            LeastCommonDenominator = (int)Math.Pow(10, mag);

            DeveloperMultiplier = developerPrefix * LeastCommonDenominator;

        }

    }
}

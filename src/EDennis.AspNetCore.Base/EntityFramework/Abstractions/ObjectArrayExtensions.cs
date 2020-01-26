using System.Linq;

namespace EDennis.AspNetCore.Base.EntityFramework{
    public static class ObjectArrayExtensions {
        public static bool EqualsAll(this object[] obj, object[] other) {
            if (obj == null || other == null 
                || obj.Length == 0 || other.Length == 0
                || obj.Length != other.Length)
                return false;
            for (int i = 0; i < obj.Length; i++)
                if (obj[i].ToString() != other[i].ToString())
                    return false;
            return true;
        }
        public static string ToTildaDelimited(this object[] obj) {
            if (obj == null)
                return null;
            if (obj.Length == 0)
                return "";
            var s = string.Join('~', obj.Select(o => o.ToString()));
            return s;
        }
    }
}

using System.Collections.Generic;

namespace EDennis.AspNetCore.Base.EntityFramework
{
    public class StoredProcedureCall
    {
        public string StoredProcedureName { get; set; }
        public List<DynamicParameter> Parameters { get; set; }
            = new List<DynamicParameter>();
    }

}

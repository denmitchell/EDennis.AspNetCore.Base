using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.EntityFramework
{
    public class StoredProcedureDef
    {
        public string Schema { get; set; }
        public string StoredProcedureName { get; set; }
        public string ParameterName { get; set; }
        public int Order { get; set; }
        public string Type { get; set; }
        public bool IsOutput { get; set; }
        public int? Length { get; set; }
        public int? Precision { get; set; }
        public int? Scale { get; set; }
    }
}

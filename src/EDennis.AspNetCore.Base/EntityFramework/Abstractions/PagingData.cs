using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.EntityFramework {
    public class PagingData {
        public int RecordCount { get; set; } = -1;
        public int PageCount { get; set; } = -1;
        public int PageNumber { get; set; } = -1;
        public int PageSize { get; set; } = -1;
    }
}

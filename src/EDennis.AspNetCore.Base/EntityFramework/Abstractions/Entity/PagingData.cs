using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.EntityFramework {
    public class PagingData {
        public int RowCount { get; set; } = -1;
        public int PageCount { get; set; } = -1;
        public int CurrentPage { get; set; } = -1;
        public int PageSize { get; set; } = -1;
    }
}

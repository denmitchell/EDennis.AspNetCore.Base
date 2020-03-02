using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Hr.BlazorApp.Lib.Data.Services {
    public interface IGetPage {
        Task<PagedResult> GetPageAsync(string where = null, string orderBy = null, int? currentPage = 1, int? pageSize = 20, int? totalRecords = null);
    }
}

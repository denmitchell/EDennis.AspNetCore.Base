using Hr.BlazorApp.Lib.Data.ApiClients;
using Hr.BlazorApp.Lib.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Hr.BlazorApp.Lib.Data.Services {
    public class PersonService : IPersonService {

        readonly HrApiClient _api;

        public PersonService(HrApiClient api) {
            _api = api;
        }

        public Task<Person> CreateAsync(Person person) {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id) {
            throw new NotImplementedException();
        }

        public Task<Person> GetAsync(int id) {
            throw new NotImplementedException();
        }

        public Task<PagedResult> GetPageAsync(string where = null, string orderBy = null, int? currentPage = 1, int? pageSize = 20, int? totalRecords = null) {
            throw new NotImplementedException();
        }

        public Task<Person> UpdateAsync(Person person) {
            throw new NotImplementedException();
        }
    }
}

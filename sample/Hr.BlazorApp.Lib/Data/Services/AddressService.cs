using Hr.BlazorApp.Lib.Data.ApiClients;
using Hr.BlazorApp.Lib.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hr.BlazorApp.Lib.Data.Services {
    public class AddressService : IAddressService {


        readonly HrApiClient _api;

        public AddressService(HrApiClient api) {
            _api = api;
        }

        public Task<Address> CreateAsync(Address address) {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id) {
            throw new NotImplementedException();
        }

        public Task<Address> GetAsync(int id) {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Address>> GetForPersonAsync(int personId) {
            throw new NotImplementedException();
        }

        public Task<Address> UpdateAsync(Address address) {
            throw new NotImplementedException();
        }
    }
}

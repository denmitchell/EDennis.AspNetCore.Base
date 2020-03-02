using Hr.BlazorApp.Lib.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hr.BlazorApp.Lib.Data.Services {
    public interface IStateService {
        Task<IEnumerable<State>> GetAllAsync();
    }
}

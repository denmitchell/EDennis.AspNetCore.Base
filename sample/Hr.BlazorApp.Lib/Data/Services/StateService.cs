using Hr.BlazorApp.Lib.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hr.BlazorApp.Lib.Data.Services {
    public class StateService : IStateService {
        public async Task<IEnumerable<State>> GetAllAsync() => 
            await Task.Run(()=>
            new List<State>{
            new State { Code = "AL", Name = "Alabama"},
            new State { Code = "AK", Name = "Alaska"},
            new State { Code = "AS", Name = "American Samoa"},
            new State { Code = "AZ", Name = "Arizona"},
            new State { Code = "AR", Name = "Arkansas"},
            new State { Code = "CA", Name = "California"},
            new State { Code = "CO", Name = "Colorado"},
            new State { Code = "CT", Name = "Connecticut"},
            new State { Code = "DE", Name = "Delaware"},
            new State { Code = "DC", Name = "District of Columbia"},
            new State { Code = "FM", Name = "Federated States of Micronesia"},
            new State { Code = "FL", Name = "Florida"},
            new State { Code = "GA", Name = "Georgia"},
            new State { Code = "GU", Name = "Guam"},
            new State { Code = "HI", Name = "Hawaii"},
            new State { Code = "ID", Name = "Idaho"},
            new State { Code = "IL", Name = "Illinois"},
            new State { Code = "IN", Name = "Indiana"},
            new State { Code = "IA", Name = "Iowa"},
            new State { Code = "KS", Name = "Kansas"},
            new State { Code = "KY", Name = "Kentucky"},
            new State { Code = "LA", Name = "Louisiana"},
            new State { Code = "ME", Name = "Maine"},
            new State { Code = "MH", Name = "Marshall Islands"},
            new State { Code = "MD", Name = "Maryland"},
            new State { Code = "MA", Name = "Massachusetts"},
            new State { Code = "MI", Name = "Michigan"},
            new State { Code = "MN", Name = "Minnesota"},
            new State { Code = "MS", Name = "Mississippi"},
            new State { Code = "MO", Name = "Missouri"},
            new State { Code = "MT", Name = "Montana"},
            new State { Code = "NE", Name = "Nebraska"},
            new State { Code = "NV", Name = "Nevada"},
            new State { Code = "NH", Name = "New Hampshire"},
            new State { Code = "NJ", Name = "New Jersey"},
            new State { Code = "NM", Name = "New Mexico"},
            new State { Code = "NY", Name = "New York"},
            new State { Code = "NC", Name = "North Carolina"},
            new State { Code = "ND", Name = "North Dakota"},
            new State { Code = "MP", Name = "Northern Mariana Islands"},
            new State { Code = "OH", Name = "Ohio"},
            new State { Code = "OK", Name = "Oklahoma"},
            new State { Code = "OR", Name = "Oregon"},
            new State { Code = "PW", Name = "Palau"},
            new State { Code = "PA", Name = "Pennsylvania"},
            new State { Code = "PR", Name = "Puerto Rico"},
            new State { Code = "RI", Name = "Rhode Island"},
            new State { Code = "SC", Name = "South Carolina"},
            new State { Code = "SD", Name = "South Dakota"},
            new State { Code = "TN", Name = "Tennessee"},
            new State { Code = "TX", Name = "Texas"},
            new State { Code = "UT", Name = "Utah"},
            new State { Code = "VT", Name = "Vermont"},
            new State { Code = "VI", Name = "Virgin Islands"},
            new State { Code = "VA", Name = "Virginia"},
            new State { Code = "WA", Name = "Washington"},
            new State { Code = "WV", Name = "West Virginia"},
            new State { Code = "WI", Name = "Wisconsin"},
            new State { Code = "WY", Name = "Wyoming"}
        });

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base {
    public class Profiles : Dictionary<string,Profile> {
        public bool NamesUpdated { get; set; } = false;
        public void UpdateNames(){ 
            foreach(var entry in this) {
                entry.Value.Name = entry.Key;
            }
            NamesUpdated = true;
        }
    }
}

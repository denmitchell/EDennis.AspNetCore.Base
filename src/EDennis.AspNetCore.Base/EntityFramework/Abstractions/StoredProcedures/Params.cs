using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.EntityFramework {
    public class Params : List<KeyValuePair<string,string>> {
        public Params Add(string key, string value) {
            Add(KeyValuePair.Create(key, value));
            return this;
        }
            
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace EDennis.AspNetCore.Base.Common {
    public class HeaderDictionary : Dictionary<string,StringValues> {

        public void LoadHeaderDictionary(IHeaderDictionary dict) {
            foreach(var key in dict.Keys) 
                Add(key, dict[key]);            
        }

        public void LoadInHttpClient(HttpClient client) {
            foreach (var key in Keys) {
                if (this[key].Count > 0)
                    client.DefaultRequestHeaders.Add(key, this[key].ToArray());
                else
                    client.DefaultRequestHeaders.Add(key, this[key].ToString());
            }
        }
    }
}

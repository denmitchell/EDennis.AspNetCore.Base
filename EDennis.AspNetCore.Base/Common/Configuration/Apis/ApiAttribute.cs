using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base { 

    [AttributeUsage(AttributeTargets.Property)]
    public class ApiAttribute : Attribute {
        public string Key { get; }
        public ApiAttribute (string key) {
            Key = key;
        }
    }
}

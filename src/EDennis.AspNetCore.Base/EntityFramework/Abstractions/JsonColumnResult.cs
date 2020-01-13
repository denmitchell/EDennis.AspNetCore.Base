using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.EntityFramework {
    public class JsonColumnResult {
        private string _json;
        public string JSON {
            get {
                return _json;
            }
            set {
                _json = value;
            }
        }
        public string Json {
            get {
                return _json;
            }
            set {
                _json = value;
            }
        }
        public string json {
            get {
                return _json;
            }
            set {
                _json = value;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.EntityFramework {
    public class DbException : ApplicationException {

        public DbException(string message, Exception inner) :
            base(message,inner) { }
    }
}

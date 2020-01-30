using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.EntityFramework {
    public class DbOperationException : ApplicationException {

        public DbOperationException(string message, Exception inner) :
            base(message,inner) { }
    }
}

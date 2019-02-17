using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace EDennis.AspNetCore.Base.Testing {
    public class SqlConnectionAndTransaction {
        public SqlConnection SqlConnection { get; set; }
        public SqlTransaction SqlTransaction { get; set; }
    }
}

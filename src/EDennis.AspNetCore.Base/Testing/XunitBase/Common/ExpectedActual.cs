using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.Testing {
    public class ExpectedActual<TEntity> {
        public TEntity Expected { get; set; }
        public TEntity Actual { get; set; }
    }
}

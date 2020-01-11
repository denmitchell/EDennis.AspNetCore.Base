using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.Testing {
    public class ExpectedActualList<TEntity> {
        public List<TEntity> Expected { get; set; }
        public List<TEntity> Actual { get; set; }
    }
}

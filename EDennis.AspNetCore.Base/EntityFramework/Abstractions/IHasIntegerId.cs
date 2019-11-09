using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.EntityFramework {
    public interface IHasIntegerId {
        public int Id { get; set; }
    }
}

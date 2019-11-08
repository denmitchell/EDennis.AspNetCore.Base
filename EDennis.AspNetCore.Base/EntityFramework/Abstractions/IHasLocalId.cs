using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.EntityFramework.Abstractions {
    public interface IHasLocalId {
        int LocalId { get; set; }
    }
}

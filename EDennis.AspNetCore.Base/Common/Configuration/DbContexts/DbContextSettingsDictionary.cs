using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base {
    public class DbContextSettingsDictionary<TContext> : Dictionary<string,DbContextSettings<TContext>>
        where TContext : DbContext {
    }
}

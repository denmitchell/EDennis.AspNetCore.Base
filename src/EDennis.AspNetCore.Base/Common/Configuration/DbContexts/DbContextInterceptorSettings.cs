﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace EDennis.AspNetCore.Base {
    public class DbContextInterceptorSettings<TContext> : DbContextBaseSettings<TContext> 
        where TContext: DbContext{
        public UserSource[] InstanceNameSource { get; set; } = new UserSource[] { Base.UserSource.JwtNameClaim };
        public bool IsInMemory { get; set; }
        public IsolationLevel IsolationLevel { get; set; }
        public bool ResetSqlServerIdentities { get; set; }
        public bool ResetSqlServerSequences { get; set; }
    }
}
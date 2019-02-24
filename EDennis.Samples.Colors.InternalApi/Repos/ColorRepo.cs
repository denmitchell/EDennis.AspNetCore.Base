using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EDennis.Samples.Colors.InternalApi.Models {

    public class ColorRepo : WriteableTemporalRepo<Color,ColorDbContext,ColorHistoryDbContext> {

        public ColorRepo(ColorDbContext context,ColorHistoryDbContext historyContext, ScopeProperties scopeProperties) 
            : base(context, historyContext, scopeProperties) {}

    }
}

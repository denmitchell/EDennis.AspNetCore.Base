using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.EntityFramework {
    public interface IGettablePrimaryKey<TEntity>
        where TEntity : class, new(){
        object[] GetPrimaryKey();
        bool Exists(IQueryable<TEntity> dbSet, params object[] id);
    }
}

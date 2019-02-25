using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.EntityFramework {

    public static class IQueryableExtensions {

        public static List<TEntity> ToPagedList<TEntity>(this IQueryable<TEntity> query, int pageNumber = 1, int pageSize = 10000) where TEntity : class {
            var qry = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return qry;
        }

        public static async Task<List<TEntity>> ToPagedListAsync<TEntity>(this IQueryable<TEntity> query, int pageNumber = 1, int pageSize = 10000) where TEntity : class {
            var qry = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return qry;
        }


    }
}

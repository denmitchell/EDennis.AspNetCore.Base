using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace EDennis.AspNetCore.Base.EntityFramework.Abstractions {
    public class CompoundSequenceCache<TEntity,TContext>
        where TEntity : class, IHasSysUser, IHasLongId, new()
        where TContext : DbContext {
        
        public const long HI_INCREMENT = 1000000;
        public const long LO_INCREMENT = 1;
        private static ConcurrentDictionary<string, long> _cache = new ConcurrentDictionary<string, long>();

        public CompoundSequenceCache(DbContextOptionsProvider<TContext> optionsProvider) {
            var context = (TContext)Activator.CreateInstance(typeof(TContext), new object[] { optionsProvider });
            var max = context.Set<TEntity>().Select(x => x.Id).Max();
            _cache.TryAdd("", max);
        }

        public static long GetNextValue(string sysUser) {
            var highValue = _cache.AddOrUpdate("", HI_INCREMENT, (key, value) => value + HI_INCREMENT );
            var lowValue = _cache.AddOrUpdate(sysUser, LO_INCREMENT, (key, value) => value + LO_INCREMENT);
            return highValue + lowValue;
        }

    }
}

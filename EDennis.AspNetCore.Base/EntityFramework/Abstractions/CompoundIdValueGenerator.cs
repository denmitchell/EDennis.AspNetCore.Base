using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EDennis.AspNetCore.Base.EntityFramework.Abstractions {

    /// <summary>
    /// ** Requires adding a CompoundSequenceCache for each TEntity as a
    /// singleton and ensuring that it is instantiated before Next is called
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TContext"></typeparam>
    public class CompoundIdValueGenerator<TEntity,TContext> : ValueGenerator<long>
        where TEntity : class, IHasLongId, IHasSysUser, new()
        where TContext : DbContext {

        public override bool GeneratesTemporaryValues => false;


        public override long Next(EntityEntry entry) {
            var entity = entry.Entity as TEntity;
            try {
                return CompoundSequenceCache<TEntity, TContext>.GetNextValue(entity.SysUser);
            } catch {
                throw new ApplicationException($"CompoundSequenceCache for {typeof(TEntity).Name}  not instantiated");
            }
        }

    }
}

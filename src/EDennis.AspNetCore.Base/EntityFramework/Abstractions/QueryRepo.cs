using EDennis.AspNetCore.Base.Logging;
using MethodBoundaryAspect.Fody.Attributes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Dynamic.Core.Exceptions;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.EntityFramework {

    /// <summary>
    /// A read/write base repository class, backed by
    /// a DbSet, exposing basic CRUD methods, as well
    /// as methods exposed by QueryableRepo 
    /// </summary>
    /// <typeparam name="TEntity">The associated model class</typeparam>
    /// <typeparam name="TContext">The associated DbContextBase class</typeparam>
    [ScopedTraceLogger]
    [AspectSkipProperties(true)]
    public partial class QueryRepo<TEntity, TContext> : IRepo, IQueryRepo<TEntity, TContext>
        where TEntity : class, IHasSysUser, new()
        where TContext : DbContext {

        public TContext Context { get; set; }
        public IScopeProperties ScopeProperties { get; set; }


        /// <summary>
        /// Constructs a new RepoBase object using the provided DbContext
        /// </summary>
        /// <param name="context">Entity Framework DbContext</param>
        public QueryRepo(DbContextProvider<TContext> provider,
            IScopeProperties scopeProperties) {
            Context = provider.Context;
            ScopeProperties = scopeProperties;
        }

        /// <summary>
        /// Get by Dynamic Linq Expression
        /// https://github.com/StefH/System.Linq.Dynamic.Core
        /// https://github.com/StefH/System.Linq.Dynamic.Core/wiki/Dynamic-Expressions
        /// </summary>
        /// <param name="where">string Where expression</param>
        /// <param name="orderBy">string OrderBy expression (with support for descending)</param>
        /// <param name="select">string Select expression</param>
        /// <param name="skip">int number of records to skip</param>
        /// <param name="take">int number of records to return</param>
        /// <returns>dynamic-typed object</returns>

        public virtual DynamicLinqResult GetWithDynamicLinq(
                string select,
                string where = null,
                string orderBy = null,
                int? skip = null,
                int? take = null,
                int? totalRecords = null) {

            IQueryable qry = BuildLinqQuery(select, where, orderBy, skip, take, totalRecords,
                out DynamicLinqResult pagedResult);

            var result = qry.ToDynamicList();
            pagedResult.Data = result;

            return pagedResult;

        }





        /// <summary>
        /// Get by Dynamic Linq Expression
        /// https://github.com/StefH/System.Linq.Dynamic.Core
        /// https://github.com/StefH/System.Linq.Dynamic.Core/wiki/Dynamic-Expressions
        /// </summary>
        /// <param name="where">string Where expression</param>
        /// <param name="orderBy">string OrderBy expression (with support for descending)</param>
        /// <param name="select">string Select expression</param>
        /// <param name="skip">int number of records to skip</param>
        /// <param name="take">int number of records to return</param>
        /// <returns>dynamic-typed object</returns>

        public virtual async Task<DynamicLinqResult> GetWithDynamicLinqAsync(
                string select,
                string where = null,
                string orderBy = null,
                int? skip = null,
                int? take = null,
                int? totalRecords = null) {

            IQueryable qry = BuildLinqQuery(select, where, orderBy, skip, take, totalRecords,
                out DynamicLinqResult pagedResult);

            var result = await qry.ToDynamicListAsync();
            pagedResult.Data = result;

            return pagedResult;

        }


        /// <summary>
        /// Get by Dynamic Linq Expression
        /// https://github.com/StefH/System.Linq.Dynamic.Core
        /// https://github.com/StefH/System.Linq.Dynamic.Core/wiki/Dynamic-Expressions
        /// </summary>
        /// <param name="where">string Where expression</param>
        /// <param name="orderBy">string OrderBy expression (with support for descending)</param>
        /// <param name="select">string Select expression</param>
        /// <param name="skip">int number of records to skip</param>
        /// <param name="take">int number of records to return</param>
        /// <returns>dynamic-typed object</returns>

        public virtual DynamicLinqResult<TEntity> GetWithDynamicLinq(
                string where = null,
                string orderBy = null,
                int? skip = null,
                int? take = null,
                int? totalRecords = null) {

            IQueryable<TEntity> qry = BuildLinqQuery(where, orderBy, skip, take, totalRecords,
                out DynamicLinqResult<TEntity> pagedResult);

            var result = qry.ToDynamicList<TEntity>();
            pagedResult.Data = result;

            return pagedResult;

        }


        /// <summary>
        /// Get by Dynamic Linq Expression
        /// https://github.com/StefH/System.Linq.Dynamic.Core
        /// https://github.com/StefH/System.Linq.Dynamic.Core/wiki/Dynamic-Expressions
        /// </summary>
        /// <param name="where">string Where expression</param>
        /// <param name="orderBy">string OrderBy expression (with support for descending)</param>
        /// <param name="select">string Select expression</param>
        /// <param name="skip">int number of records to skip</param>
        /// <param name="take">int number of records to return</param>
        /// <returns>dynamic-typed object</returns>

        public virtual async Task<DynamicLinqResult<TEntity>> GetWithDynamicLinqAsync(
                string where = null,
                string orderBy = null,
                int? skip = null,
                int? take = null,
                int? totalRecords = null) {

            IQueryable<TEntity> qry = BuildLinqQuery(where, orderBy, skip, take, totalRecords,
                out DynamicLinqResult<TEntity> pagedResult);

            var result = await qry.ToDynamicListAsync<TEntity>();
            pagedResult.Data = result;

            return pagedResult;
        }






        private IQueryable BuildLinqQuery(string select, string where, string orderBy, int? skip, int? take, int? totalRecords, out DynamicLinqResult pagedResult) {

            IQueryable qry = Query;

            try {
                if (!string.IsNullOrWhiteSpace(where))
                    qry = qry.Where(where);
                if (!string.IsNullOrWhiteSpace(orderBy))
                    qry = qry.OrderBy(orderBy);
                if (!string.IsNullOrWhiteSpace(select))
                    qry = qry.Select(select);
            } catch (ParseException ex) {
                throw new ArgumentException(ex.Message);
            }

            if (totalRecords == null || totalRecords.Value < 0)
                totalRecords = qry.Count();

            var skipValue = skip == null ? 0 : skip.Value;
            var takeValue = take == null ? totalRecords.Value - skipValue : take.Value;
            var pageCount = (int)Math.Ceiling(totalRecords.Value / (double)takeValue);

            pagedResult = new DynamicLinqResult {
                CurrentPage = 1 + (int)Math.Ceiling((skipValue) / (double)takeValue),
                PageCount = pageCount,
                PageSize = takeValue,
                RowCount = totalRecords.Value
            };
            if (skipValue != 0)
                qry = qry.Skip(skipValue);
            if (take != null && take.Value > 0)
                qry = qry.Take(takeValue);

            return qry;
        }



        private IQueryable<TEntity> BuildLinqQuery(string where, string orderBy, int? skip, int? take, int? totalRecords, out DynamicLinqResult<TEntity> pagedResult) {

            IQueryable<TEntity> qry = Query;

            try {
                if (!string.IsNullOrWhiteSpace(where))
                    qry = qry.Where(where);
                if (!string.IsNullOrWhiteSpace(orderBy))
                    qry = qry.OrderBy(orderBy);
            } catch (ParseException ex) {
                throw new ArgumentException(ex.Message);
            }

            if (totalRecords == null || totalRecords.Value < 0)
                totalRecords = qry.Count();

            var skipValue = skip == null ? 0 : skip.Value;
            var takeValue = take == null ? totalRecords.Value - skipValue : take.Value;
            var pageCount = (int)Math.Ceiling(totalRecords.Value / (double)takeValue);

            pagedResult = new DynamicLinqResult<TEntity> {
                CurrentPage = 1 + (int)Math.Ceiling((skipValue) / (double)takeValue),
                PageCount = pageCount,
                PageSize = takeValue,
                RowCount = totalRecords.Value
            };
            if (skipValue != 0)
                qry = qry.Skip(skipValue);
            if (take != null && take.Value > 0)
                qry = qry.Take(takeValue);

            return qry;
        }



        /// <summary>
        /// Provides direct access to the Query property of the context,
        /// allowing any query to be constructed via Linq expression
        /// </summary>
        public virtual IQueryable<TEntity> Query { get => Context.Set<TEntity>().AsNoTracking(); }


    }


}


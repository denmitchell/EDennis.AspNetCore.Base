using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.EntityFramework {

    /// <summary>
    /// A read-only base repository class, backed by
    /// a DbQuery, exposing methods that allow one to
    /// query by a Linq expression or a parameterized
    /// SQL SELECT statement.  The results are pageable.
    /// </summary>
    /// <typeparam name="TEntity">The model class</typeparam>
    /// <typeparam name="TContext">The DbContext (or DbContextBase) subclass</typeparam>
    /// <see cref="WriteableRepo{TEntity, TContext}"/>
    public class QueryableRepo<TEntity, TContext> : IRepo
            where TEntity : class, new()
            where TContext : DbContext {

        //reference to DbContext class
        protected TContext Context { get; }

        //reference to the underlying DbQuery (analogous to DbSet)
        protected DbQuery<TEntity> _dbquery;


        /// <summary>
        /// Constructs a new QueryRepoBase object using the provided DbContext
        /// </summary>
        /// <param name="context">Entity Framework DbContext</param>
        public QueryableRepo(TContext context) {
            Context = context;

            //get a reference to the DbQuery
            _dbquery = Context.Query<TEntity>();
        }



        /// <summary>
        /// Retrieves a page of all records defined by the provided LINQ expression
        /// </summary>
        /// <param name="linqExpression">Valid LINQ expression</param>
        /// <param name="pageNumber">The target result page</param>
        /// <param name="pageSize">The number of record per page</param>
        /// <returns>A list of all TEntity objects</returns>
        public virtual List<TEntity> GetByLinq(Expression<Func<TEntity, bool>> linqExpression, int pageNumber, int pageSize) {
            return _dbquery.Where(linqExpression)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }


        /// <summary>
        /// Asynchronously retrieves a page of all records defined by the provided LINQ expression.
        /// </summary>
        /// <param name="linqExpression">Valid LINQ expression</param>
        /// <param name="pageNumber">The target result page</param>
        /// <param name="pageSize">The number of record per page</param>
        /// <returns>A list of all TEntity objects</returns>
        public virtual async Task<List<TEntity>> GetByLinqAsync(Expression<Func<TEntity, bool>> linqExpression, int pageNumber, int pageSize) {
            return await _dbquery.Where(linqExpression)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }


        /// <summary>
        /// Retrieves a page of all records defined by the provided SQL and parameters
        /// </summary>
        /// <param name="sql">Valid SQL select or exec stored procedure</param>
        /// <param name="pageNumber">The target result page</param>
        /// <param name="pageSize">The number of record per page</param>
        /// <param name="parameters">an array of parameters for the SQL statement</param>
        /// <returns>A list of all TEntity objects</returns>
        public virtual List<TEntity> GetFromSql(string sql, int pageNumber, int pageSize, params object[] parameters) {
            return _dbquery.FromSql(sql,parameters)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }


        /// <summary>
        /// Retrieves a page of all records defined by the provided SQL and parameters
        /// </summary>
        /// <param name="sql">Valid SQL select or exec stored procedure</param>
        /// <param name="pageNumber">The target result page</param>
        /// <param name="pageSize">The number of record per page</param>
        /// <param name="parameters">an array of parameters for the SQL statement</param>
        /// <returns>A list of all TEntity objects</returns>
        public virtual async Task<List<TEntity>> GetFromSqlAsync(string sql, int pageNumber, int pageSize, params object[] parameters) {
            return await _dbquery.FromSql(sql, parameters)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
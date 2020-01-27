

using System.Collections.Generic;
using System.Linq.Dynamic.Core;

namespace EDennis.AspNetCore.Base.EntityFramework {

    /// <summary>
    /// The result returned from a Dynamic Linq query.  This
    /// version of DynamicLinqResult provides a dynamic
    /// response.  The class is used when the Select property
    /// IS provided (and hence only a subset of properties are 
    /// returned).
    /// </summary>
    public class DynamicLinqResult : DynamicLinqResult<dynamic> {
        public override List<dynamic> Data { get; set; } 
    }


    /// <summary>
    /// The result returned from a Dynamic Linq query.  This
    /// generic version of DynamicLinqResult provides a typed
    /// response.  The class is used when the Select property
    /// is not provided (and hence all properties are returned).
    /// </summary>
    /// <typeparam name="TEntity">The relevant model class</typeparam>
    public class DynamicLinqResult<TEntity> {
        public virtual List<TEntity> Data { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }
    }
}
    

//using EDennis.AspNetCore.Base.Serialization;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace EDennis.AspNetCore.Base.EntityFramework {
//    public class PagedResult<TEntity> {
//        public IEnumerable<TEntity> Data { get; set; }
//        public PagingData PagingData { get; set; }

//    }


//    /// <summary>
//    /// Dynamic version of a PagedResult
//    /// </summary>
//    public class PagedResult : PagedResult<dynamic> {

//        public List<TEntity> ToList<TEntity>()
//        where TEntity : new() {
//                var list = new List<TEntity>();
//                foreach (var item in Data) {
//                    TEntity obj = new TEntity();
//                    Projection<TEntity>.Patch(item, obj);
//                    list.Add(obj);
//                }
//                return list;
//            }
//    }

//}

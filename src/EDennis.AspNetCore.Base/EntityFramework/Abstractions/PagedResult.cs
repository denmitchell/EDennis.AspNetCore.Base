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

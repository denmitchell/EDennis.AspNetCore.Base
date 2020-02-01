using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Web.Abstractions {
    public interface IRepoControllerApiClient<TEntity> : IApiClient
        where TEntity : class, IHasSysUser, new(){
        /// <summary>
        /// Returns the controller URL associated with a given 
        /// TEntity or TContext.
        /// For IRepoControllerApiClient, this is typeof(TEntity)
        /// For ISqlServerControllerApiClient, this is typeof(TContext)
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        string GetControllerUrl(Type type);
    }

    public interface ISqlServerControllerApiClient<TContext> : IApiClient
        where TContext : DbContext, ISqlServerDbContext<TContext>{
        /// <summary>
        /// Returns the controller URL associated with a given 
        /// TEntity or TContext.
        /// For IRepoControllerApiClient, this is typeof(TEntity)
        /// For ISqlServerControllerApiClient, this is typeof(TContext)
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        string GetControllerUrl(Type type);
    }

    public class RContext : DbContext, ISqlServerDbContext<RContext> {
        public StoredProcedureDefs<RContext> StoredProcedureDefs { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }

    public class P : IHasSysUser { public string SysUser { get; set; } }
    public class Q : IHasSysUser { public string SysUser { get; set; } }
    public class Tmp : IRepoControllerApiClient<P>, IRepoControllerApiClient<Q>, ISqlServerControllerApiClient<RContext> {
        public Api Api => throw new NotImplementedException();

        public string ApiKey => throw new NotImplementedException();

        public HttpClient HttpClient => throw new NotImplementedException();

        public IScopeProperties ScopeProperties => throw new NotImplementedException();

        public string GetControllerUrl(Type type) {
            if (type == typeof(P))
                return "api/jfkdaj";
            else if (type == typeof(Q))
                return "api/dfjks";
            else if (type == typeof(RContext))
                return "api/sdkl";
            else
                return null;
        }
    }


    public static class IRepoControllerApiClientExtensionMethods {

        public static void Test<TEntity>(this IRepoControllerApiClient<TEntity> client)
        where TEntity : class, IHasSysUser, new() {

        }

        public static ObjectResult Get<TEntity>(
            this IRepoControllerApiClient<TEntity> api, params object[] id)
            where TEntity : class, IHasSysUser, new() {
            var url = $"{api.GetControllerUrl(typeof(TEntity))}/{id.ToTildaDelimited()}";
            return api.HttpClient.Get<TEntity>(url);
        }

        public static async Task<ObjectResult> GetAsync<TEntity>(
            this IRepoControllerApiClient<TEntity> api, params object[] id)
            where TEntity : class, IHasSysUser, new() {
            var url = $"{api.GetControllerUrl(typeof(TEntity))}/{id.ToTildaDelimited()}";
            return await api.HttpClient.GetAsync<TEntity>(url);
        }

        public static ObjectResult Create<TEntity>(
            this IRepoControllerApiClient<TEntity> api, TEntity entity)
            where TEntity : class, IHasSysUser, new() {
            var url = $"{api.GetControllerUrl(typeof(TEntity))}";
            return api.HttpClient.Post(url, entity);
        }

        public static async Task<ObjectResult> CreateAsync<TEntity>(
            this IRepoControllerApiClient<TEntity> api, TEntity entity)
            where TEntity : class, IHasSysUser, new() {
            var url = $"{api.GetControllerUrl(typeof(TEntity))}";
            return await api.HttpClient.PostAsync(url, entity);
        }

        public static ObjectResult Update<TEntity>(
            this IRepoControllerApiClient<TEntity> api, TEntity entity, params object[] id)
            where TEntity : class, IHasSysUser, new() {
            var url = $"{api.GetControllerUrl(typeof(TEntity))}/{id.ToTildaDelimited()}";
            return api.HttpClient.Put(url, entity);
        }

        public static async Task<ObjectResult> UpdateAsync<TEntity>(
            this IRepoControllerApiClient<TEntity> api, TEntity entity, params object[] id)
            where TEntity : class, IHasSysUser, new() {
            var url = $"{api.GetControllerUrl(typeof(TEntity))}/{id.ToTildaDelimited()}";
            return await api.HttpClient.PutAsync(url, entity);
        }


        public static ObjectResult Patch<TEntity>(
            this IRepoControllerApiClient<TEntity> api, dynamic partialEntity, params object[] id)
            where TEntity : class, IHasSysUser, new() {
            var url = $"{api.GetControllerUrl(typeof(TEntity))}/{id.ToTildaDelimited()}";
            return HttpClientExtensions.Patch<TEntity>(api.HttpClient, url, partialEntity);
        }

        public static async Task<ObjectResult> PatchAsync<TEntity>(
            this IRepoControllerApiClient<TEntity> api, dynamic partialEntity, params object[] id)
            where TEntity : class, IHasSysUser, new() {
            var url = $"{api.GetControllerUrl(typeof(TEntity))}/{id.ToTildaDelimited()}";
            return await HttpClientExtensions.PatchAsync<TEntity>(api.HttpClient, url, partialEntity);
        }


        public static StatusCodeResult Delete<TEntity>(
            this IRepoControllerApiClient<TEntity> api, params object[] id)
            where TEntity : class, IHasSysUser, new() {
            var url = $"{api.GetControllerUrl(typeof(TEntity))}/{id.ToTildaDelimited()}";
            return api.HttpClient.Delete<TEntity>(url);
        }

        public static async Task<StatusCodeResult> DeleteAsync<TEntity>(
            this IRepoControllerApiClient<TEntity> api, params object[] id)
            where TEntity : class, IHasSysUser, new() {
            var url = $"{api.GetControllerUrl(typeof(TEntity))}/{id.ToTildaDelimited()}";
            return await api.HttpClient.DeleteAsync<TEntity>(url);
        }


        public static DynamicLinqResult<TEntity> GetWithDynamicLinq<TEntity>(
            this IRepoControllerApiClient<TEntity> api,
            string where = null, string orderBy = null,
            string select = null, int? skip = null,
            int? take = null, int? totalRecords = null)
            where TEntity : class, IHasSysUser, new() {

            var url = $"{api.GetControllerUrl(typeof(TEntity))}/linq{GetDynamicLinqQueryString(where, orderBy, select, skip, take, totalRecords)}";
            var result = api.HttpClient.Get<DynamicLinqResult<TEntity>, TEntity>(url);
            var obj = result.GetObject<DynamicLinqResult<TEntity>>();
            return obj;
        }


        public static async Task<DynamicLinqResult<TEntity>> GetWithDynamicLinqAsync<TEntity>(
            this IRepoControllerApiClient<TEntity> api,
            string where = null, string orderBy = null,
            string select = null, int? skip = null,
            int? take = null, int? totalRecords = null)
            where TEntity : class, IHasSysUser, new() {

            var url = $"{api.GetControllerUrl(typeof(TEntity))}/linq/async{GetDynamicLinqQueryString(where, orderBy, select, skip, take, totalRecords)}";
            var result = await api.HttpClient.GetAsync<DynamicLinqResult<TEntity>, TEntity>(url);
            var obj = result.GetObject<DynamicLinqResult<TEntity>>();
            return obj;
        }



        public static ObjectResult GetWithDevExtreme<TEntity>(
            this IRepoControllerApiClient<TEntity> api, HttpRequest request)
            where TEntity : class, IHasSysUser, new() {
            var url = $"{api.GetControllerUrl(typeof(TEntity))}/devextreme";
            var result = api.HttpClient.Forward<TEntity>(request, url);
            return result;
        }


        public static async Task<ObjectResult> GetWithDevExtremeAsync<TEntity>(
            this IRepoControllerApiClient<TEntity> api, HttpRequest request)
            where TEntity : class, IHasSysUser, new() {
            var url = $"{api.GetControllerUrl(typeof(TEntity))}/devextreme/async";
            var result = await api.HttpClient.ForwardAsync<TEntity>(request, url);
            return result;
        }



        public static ObjectResult GetWithDevExtreme<TEntity>(
            this IRepoControllerApiClient<TEntity> api, 
            string select = null, string sort = null,
            string filter = null, string totalSummary = null,
            string group = null, string groupSummary = null,
            int? skip = null, int? take = null ) 
            where TEntity : class, IHasSysUser, new() {
            var qryString = GetDevExtremeQueryString(select, sort, filter, totalSummary, group, groupSummary, skip, take);
            var url = $"{api.GetControllerUrl(typeof(TEntity))}/devextreme?{qryString}";
            var result = api.HttpClient.Get<DeserializableLoadResult<TEntity>>(url);
            return result;
        }

        public static async Task<ObjectResult> GetWithDevExtremeAsync<TEntity>(
            this IRepoControllerApiClient<TEntity> api,
            string select = null, string sort = null,
            string filter = null, string totalSummary = null,
            string group = null, string groupSummary = null,
            int? skip = null, int? take = null)
            where TEntity : class, IHasSysUser, new() {
            var qryString = GetDevExtremeQueryString(select, sort, filter, totalSummary, group, groupSummary, skip, take);
            var url = $"{api.GetControllerUrl(typeof(TEntity))}/devextreme?{qryString}";
            var result = await api.HttpClient.GetAsync<DeserializableLoadResult<TEntity>>(url);
            return result;
        }

        private static string GetDynamicLinqQueryString(string where = null,
            string orderBy = null, string select = null, int? skip = null,
            int? take = null, int? totalRecords = null) {

            var q = new List<string>();
            if (!string.IsNullOrWhiteSpace(where))
                q.Add($"where={where}");
            if (!string.IsNullOrWhiteSpace(orderBy))
                q.Add($"orderBy={orderBy}");
            if (!string.IsNullOrWhiteSpace(select))
                q.Add($"select={select}");
            if (skip != null)
                q.Add($"skip={skip.Value}");
            if (take != null)
                q.Add($"take={take}");
            if (totalRecords != null)
                q.Add($"totalRecords={totalRecords}");

            var qString = "?" + string.Join('&', q);

            return qString;
        }


        private static string GetDevExtremeQueryString(
            string select = null, string sort = null, 
            string filter = null, string totalSummary = null,
            string group = null, string groupSummary = null, 
            int? skip = null, int? take = null ) {

            var q = new List<string>();
            if (!string.IsNullOrWhiteSpace(select))
                q.Add($"select={select}");
            if (!string.IsNullOrWhiteSpace(sort))
                q.Add($"sort={sort}");
            if (!string.IsNullOrWhiteSpace(filter))
                q.Add($"filter={filter}");
            if (totalSummary != null)
                q.Add($"totalSummary={totalSummary}");
            if (group != null)
                q.Add($"group={group}");
            if (groupSummary != null)
                q.Add($"group={groupSummary}");

            q.Add($"skip={skip ?? 0}");
            q.Add($"take={take ?? int.MaxValue}");

            var qString = "?" + string.Join('&', q);

            return qString;
        }


    }

}

using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Web {
    public interface IIntegerIdRepoControllerApiClient<TEntity> : IApiClient
        where TEntity : class, IHasSysUser, new(){
        /// <summary>
        /// Returns the controller URL associated with a given 
        /// TEntity or TContext.
        /// For IIntegerIdRepoControllerApiClient, this is typeof(TEntity)
        /// For IRepoControllerApiClient, this is typeof(TEntity)
        /// For ISqlServerControllerApiClient, this is typeof(TContext)
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        string GetControllerUrl(Type type);
    }

    public static class IIntegerIdRepoControllerApiClientExtensionMethods {


        public static ObjectResult<TEntity> Get<TEntity>(
            this IIntegerIdRepoControllerApiClient<TEntity> api, int id)
            where TEntity : class, IHasSysUser, new() {
            var url = $"{api.GetControllerUrl(typeof(TEntity))}/{id}";
            return api.HttpClient.Get<TEntity>(url);
        }

        public static async Task<ObjectResult<TEntity>> GetAsync<TEntity>(
            this IIntegerIdRepoControllerApiClient<TEntity> api, int id)
            where TEntity : class, IHasSysUser, new() {
            var url = $"{api.GetControllerUrl(typeof(TEntity))}/{id}";
            return await api.HttpClient.GetAsync<TEntity>(url);
        }

        public static ObjectResult<TEntity> Create<TEntity>(
            this IIntegerIdRepoControllerApiClient<TEntity> api, TEntity entity)
            where TEntity : class, IHasSysUser, new() {
            var url = $"{api.GetControllerUrl(typeof(TEntity))}";
            return api.HttpClient.Post(url, entity);
        }

        public static async Task<ObjectResult<TEntity>> CreateAsync<TEntity>(
            this IIntegerIdRepoControllerApiClient<TEntity> api, TEntity entity)
            where TEntity : class, IHasSysUser, new() {
            var url = $"{api.GetControllerUrl(typeof(TEntity))}";
            return await api.HttpClient.PostAsync(url, entity);
        }

        public static ObjectResult<TEntity> Update<TEntity>(
            this IIntegerIdRepoControllerApiClient<TEntity> api, TEntity entity, params object[] id)
            where TEntity : class, IHasSysUser, new() {
            var url = $"{api.GetControllerUrl(typeof(TEntity))}/{id.ToTildaDelimited()}";
            return api.HttpClient.Put(url, entity);
        }

        public static async Task<ObjectResult<TEntity>> UpdateAsync<TEntity>(
            this IIntegerIdRepoControllerApiClient<TEntity> api, TEntity entity, int id)
            where TEntity : class, IHasSysUser, new() {
            var url = $"{api.GetControllerUrl(typeof(TEntity))}/{id}";
            return await api.HttpClient.PutAsync(url, entity);
        }


        public static ObjectResult<TEntity> Patch<TEntity>(
            this IIntegerIdRepoControllerApiClient<TEntity> api, dynamic partialEntity, int id)
            where TEntity : class, IHasSysUser, new() {
            var url = $"{api.GetControllerUrl(typeof(TEntity))}/{id}";
            return HttpClientExtensions.Patch<TEntity>(api.HttpClient, url, partialEntity);
        }

        public static async Task<ObjectResult<TEntity>> PatchAsync<TEntity>(
            this IIntegerIdRepoControllerApiClient<TEntity> api, dynamic partialEntity, int id)
            where TEntity : class, IHasSysUser, new() {
            var url = $"{api.GetControllerUrl(typeof(TEntity))}/{id}";
            return await HttpClientExtensions.PatchAsync<TEntity>(api.HttpClient, url, partialEntity);
        }


        public static StatusCodeResult Delete<TEntity>(
            this IIntegerIdRepoControllerApiClient<TEntity> api, int id)
            where TEntity : class, IHasSysUser, new() {
            var url = $"{api.GetControllerUrl(typeof(TEntity))}/{id}";
            return api.HttpClient.Delete<TEntity>(url);
        }

        public static async Task<StatusCodeResult> DeleteAsync<TEntity>(
            this IIntegerIdRepoControllerApiClient<TEntity> api, int id)
            where TEntity : class, IHasSysUser, new() {
            var url = $"{api.GetControllerUrl(typeof(TEntity))}/{id}";
            return await api.HttpClient.DeleteAsync<TEntity>(url);
        }


        public static ObjectResult<DynamicLinqResult<TEntity>> GetWithDynamicLinq<TEntity>(
            this IIntegerIdRepoControllerApiClient<TEntity> api,
            string where = null, string orderBy = null,
            string select = null, int? skip = null,
            int? take = null, int? totalRecords = null)
            where TEntity : class, IHasSysUser, new() {

            var url = $"{api.GetControllerUrl(typeof(TEntity))}/linq{GetDynamicLinqQueryString(where, orderBy, select, skip, take, totalRecords)}";
            var result = api.HttpClient.Get<DynamicLinqResult<TEntity>, TEntity>(url);
            return result;
        }


        public static async Task<ObjectResult<DynamicLinqResult<TEntity>>> GetWithDynamicLinqAsync<TEntity>(
            this IIntegerIdRepoControllerApiClient<TEntity> api,
            string where = null, string orderBy = null,
            string select = null, int? skip = null,
            int? take = null, int? totalRecords = null)
            where TEntity : class, IHasSysUser, new() {

            var url = $"{api.GetControllerUrl(typeof(TEntity))}/linq/async{GetDynamicLinqQueryString(where, orderBy, select, skip, take, totalRecords)}";
            var result = await api.HttpClient.GetAsync<DynamicLinqResult<TEntity>, TEntity>(url);
            return result;
        }



        public static ObjectResult<DeserializableLoadResult<TEntity>> GetWithDevExtreme<TEntity>(
            this IIntegerIdRepoControllerApiClient<TEntity> api, HttpRequest request)
            where TEntity : class, IHasSysUser, new() {
            var url = $"{api.GetControllerUrl(typeof(TEntity))}/devextreme";
            var result = api.HttpClient.Forward<DeserializableLoadResult<TEntity>>(request, url);
            return result;
        }


        public static async Task<ObjectResult<DeserializableLoadResult<TEntity>>> GetWithDevExtremeAsync<TEntity>(
            this IIntegerIdRepoControllerApiClient<TEntity> api, HttpRequest request)
            where TEntity : class, IHasSysUser, new() {
            var url = $"{api.GetControllerUrl(typeof(TEntity))}/devextreme/async";
            var result = await api.HttpClient.ForwardAsync<DeserializableLoadResult<TEntity>>(request, url);
            return result;
        }


        public static ObjectResult<DeserializableLoadResult<TEntity>> GetWithDevExtreme<TEntity>(
            this IIntegerIdRepoControllerApiClient<TEntity> api, 
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

        public static async Task<ObjectResult<DeserializableLoadResult<TEntity>>> GetWithDevExtremeAsync<TEntity>(
            this IIntegerIdRepoControllerApiClient<TEntity> api,
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

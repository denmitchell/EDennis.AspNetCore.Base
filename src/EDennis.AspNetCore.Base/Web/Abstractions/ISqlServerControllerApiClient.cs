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

    public static class ISqlServerControllerApiClientExtensionMethods {


        public static ObjectResult<string> GetJsonArrayFromStoredProcedure<TContext>(
            this ISqlServerControllerApiClient<TContext> api, string spName, Dictionary<string,string> parameters)
            where TContext : DbContext, ISqlServerDbContext<TContext> {
            var qryString = GetStoredProcedureQueryString(spName, parameters);
            var url = $"{api.GetControllerUrl(typeof(TContext))}/array{qryString}";
            return api.HttpClient.Get<string>(url);
        }

        public static async Task<ObjectResult<string>> GetJsonArrayFromStoredProcedureAsync<TContext>(
            this ISqlServerControllerApiClient<TContext> api, string spName, Dictionary<string, string> parameters)
            where TContext : DbContext, ISqlServerDbContext<TContext> {
            var qryString = GetStoredProcedureQueryString(spName, parameters);
            var url = $"{api.GetControllerUrl(typeof(TContext))}/array/async{qryString}";
            return await api.HttpClient.GetAsync<string>(url);
        }


        public static ObjectResult<string> GetJsonObjectFromStoredProcedure<TContext>(
            this ISqlServerControllerApiClient<TContext> api, string spName, Dictionary<string, string> parameters)
            where TContext : DbContext, ISqlServerDbContext<TContext> {
            var qryString = GetStoredProcedureQueryString(spName, parameters);
            var url = $"{api.GetControllerUrl(typeof(TContext))}/object{qryString}";
            return api.HttpClient.Get<string>(url);
        }

        public static async Task<ObjectResult<string>> GetJsonObjectFromStoredProcedureAsync<TContext>(
            this ISqlServerControllerApiClient<TContext> api, string spName, Dictionary<string, string> parameters)
            where TContext : DbContext, ISqlServerDbContext<TContext> {
            var qryString = GetStoredProcedureQueryString(spName, parameters);
            var url = $"{api.GetControllerUrl(typeof(TContext))}/object/async{qryString}";
            return await api.HttpClient.GetAsync<string>(url);
        }


        public static ObjectResult<string> GetJsonFromJsonStoredProcedure<TContext>(
            this ISqlServerControllerApiClient<TContext> api, string spName, Dictionary<string, string> parameters)
            where TContext : DbContext, ISqlServerDbContext<TContext> {
            var qryString = GetStoredProcedureQueryString(spName, parameters);
            var url = $"{api.GetControllerUrl(typeof(TContext))}/forjson{qryString}";
            return api.HttpClient.Get<string>(url);
        }

        public static async Task<ObjectResult<string>> GetJsonFromJsonStoredProcedureAsync<TContext>(
            this ISqlServerControllerApiClient<TContext> api, string spName, Dictionary<string, string> parameters)
            where TContext : DbContext, ISqlServerDbContext<TContext> {
            var qryString = GetStoredProcedureQueryString(spName, parameters);
            var url = $"{api.GetControllerUrl(typeof(TContext))}/forjson/async{qryString}";
            return await api.HttpClient.GetAsync<string>(url);
        }


        public static ObjectResult<TResult> GetScalarFromStoredProcedure<TContext,TResult>(
            this ISqlServerControllerApiClient<TContext> api, string spName, Dictionary<string, string> parameters)
            where TContext : DbContext, ISqlServerDbContext<TContext> {
            var qryString = GetStoredProcedureQueryString(spName, parameters);
            var url = $"{api.GetControllerUrl(typeof(TContext))}/scalar{qryString}";
            return api.HttpClient.Get<TResult>(url);
        }

        public static async Task<ObjectResult<TResult>> GetScalarFromStoredProcedureAsync<TContext,TResult>(
            this ISqlServerControllerApiClient<TContext> api, string spName, Dictionary<string, string> parameters)
            where TContext : DbContext, ISqlServerDbContext<TContext> {
            var qryString = GetStoredProcedureQueryString(spName, parameters);
            var url = $"{api.GetControllerUrl(typeof(TContext))}/scalar/async{qryString}";
            return await api.HttpClient.GetAsync<TResult>(url);
        }

        private static string GetStoredProcedureQueryString(string spName, 
            Dictionary<string,string> parameters) {

            var q = new List<string>();
            q.Add($"spName={spName}");
            foreach(var parameter in parameters)
                q.Add($"{parameter.Key}={parameter.Value}");

            var qString = "?" + string.Join('&', q);

            return qString;
        }


    }

}

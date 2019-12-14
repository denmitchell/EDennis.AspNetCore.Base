using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.MultipleConfigsApi.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EDennis.Samples.MultipleConfigsApi {
    public class Service {
        //public string Name { get; set; }
        //public ServiceLifetime Lifetime { get; set; }
        //[JsonIgnore]
        //public object Instance { get; set; }
        //public Dictionary<string,object> Properties { 
        //    get {
        //        var dict = new Dictionary<string, object>();
        //        if (Instance is MyApiClient obj1) {
        //            dict.Add("Api", obj1.Api != null);
        //            dict.Add("ApiKey", obj1.ApiKey);
        //            dict.Add("Logger", obj1.Logger != null);
        //            dict.Add("ScopedLogger", obj1.ScopedLogger != null);
        //            dict.Add("HttpClient.BaseAddress", obj1.HttpClient?.BaseAddress?.ToString());
        //            dict.Add("ScopeProperties", obj1.ScopeProperties != null);
        //        } else if (Instance is MySecureApiClient obj2) {
        //            dict.Add("ApiKey", obj2.ApiKey);
        //            dict.Add("HttpClient.BaseAddress", obj2.HttpClient?.BaseAddress?.ToString());
        //            dict.Add("Logger", obj2.Logger != null);
        //            dict.Add("ScopedLogger", obj2.ScopedLogger != null);
        //            dict.Add("ScopeProperties", obj2.ScopeProperties != null);
        //        } else if (Instance is MyDbContext obj3) {
        //            dict.Add("EntityTypes", string.Join(',',obj3.Model.GetEntityTypes().Select(e=>e.Name).ToArray()));
        //        } else if (Instance is MyTemporalDbContext obj4) {
        //            dict.Add("EntityTypes", string.Join(',', obj4.Model.GetEntityTypes().Select(e => e.Name).ToArray()));
        //        } else if (Instance is MyTemporalHistoryDbContext obj5) {
        //            dict.Add("EntityTypes", string.Join(',', obj5.Model.GetEntityTypes().Select(e => e.Name).ToArray()));
        //        } else if (Instance is MyRepo obj6) {
        //            dict.Add("Context", obj6.Context != null);
        //            dict.Add("Logger", obj6.Logger != null);
        //            dict.Add("ScopedLogger", obj6.ScopedLogger != null);
        //            dict.Add("ScopeProperties", obj6.ScopeProperties != null);
        //        } else if (Instance is MyTemporalRepo obj7) {
        //            dict.Add("Context", obj7.Context != null);
        //            dict.Add("Context", obj7.HistoryContext != null);
        //            dict.Add("Logger", obj7.Logger != null);
        //            dict.Add("ScopedLogger", obj7.ScopedLogger != null);
        //            dict.Add("ScopeProperties", obj7.ScopeProperties != null);
        //        } else if (Instance is IOptionsMonitor<Apis> obj8) {
        //            dict.Add("IOptionsMonitor<Apis>", obj8);
        //        } else if (Instance is IOptionsMonitor<ScopePropertiesSettings> obj9) {
        //            dict.Add("IOptionsMonitor<ScopePropertiesSettings>", obj9);
        //        } else if (Instance is IOptionsMonitor<UserLoggerSettings> obj10) {
        //            dict.Add("IOptionsMonitor<UserLoggerSettings>", obj10);
        //        } else if (Instance is IOptionsMonitor<DbContextSettings<MyDbContext>> obj11) {
        //            dict.Add("IOptionsMonitor<DbContextSettings<MyDbContext>>", obj11);
        //        } else if (Instance is IOptionsMonitor<DbContextSettings<MyTemporalDbContext>> obj12) {
        //            dict.Add("IOptionsMonitor<DbContextSettings<MyTemporalDbContext>>", obj12);
        //        } else if (Instance is IOptionsMonitor<DbContextSettings<MyTemporalHistoryDbContext>> obj13) {
        //            dict.Add("IOptionsMonitor<DbContextSettings<MyTemporalHistoryDbContext>>", obj13);
        //        } else if (Instance is IOptionsMonitor<DbContextInterceptorSettings<MyDbContext>> obj14) {
        //            dict.Add("IOptionsMonitor<DbContextInterceptorSettings<MyDbContext>>", obj14);
        //        } else if (Instance is IOptionsMonitor<DbContextInterceptorSettings<MyTemporalDbContext>> obj15) {
        //            dict.Add("IOptionsMonitor<DbContextInterceptorSettings<MyTemporalDbContext>>", obj15);
        //        } else if (Instance is IOptionsMonitor<DbContextInterceptorSettings<MyTemporalHistoryDbContext>> obj16) {
        //            dict.Add("IOptionsMonitor<DbContextInterceptorSettings<MyTemporalHistoryDbContext>>", obj16);
        //        } else if (Instance is IOptionsMonitor<HeadersToClaims> obj17) {
        //            dict.Add("IOptionsMonitor<HeadersToClaims>", obj17);
        //        } else if (Instance is IOptionsMonitor<ActiveMockClientSettings> obj18) {
        //            dict.Add("IOptionsMonitor<ActiveMockClientSettings>", obj18);
        //        } else if (Instance is IOptionsMonitor<MockHeaderSettingsCollection> obj19) {
        //            dict.Add("IOptionsMonitor<MockHeaderSettingsCollection>", obj19);
        //        }
        //        return dict;
        //    }                
        //}
    }
}

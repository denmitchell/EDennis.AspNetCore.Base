using EDennis.AspNetCore.Base;
using EDennis.Samples.MultipleConfigsApi.Models;
using EDennis.Samples.MultipleConfigsApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Linq;


namespace EDennis.Samples.MultipleConfigsApi.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class ScopePropertiesSettingsController : ControllerBase {

        private readonly IOptionsMonitor<ScopePropertiesSettings> _instance;
        private readonly ServiceDescriptor _serviceDescriptor;

        public ScopePropertiesSettingsController(IOptionsMonitor<ScopePropertiesSettings> instance, IServiceCollection services) {
            _instance = instance;
            _serviceDescriptor = services.FirstOrDefault(s => s.ImplementationType == instance.GetType());
        }

        //[HttpGet]
        //public Service Get() {
        //    var service = new Service {
        //        Instance = _instance,
        //        Name = _instance.GetType().CSharpName(),
        //        Lifetime = _serviceDescriptor.Lifetime
        //    };
        //    return service;
        //}
    }
}

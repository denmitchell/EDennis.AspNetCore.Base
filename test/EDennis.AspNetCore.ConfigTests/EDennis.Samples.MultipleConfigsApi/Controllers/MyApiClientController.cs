using EDennis.Samples.MultipleConfigsApi.Models;
using EDennis.Samples.MultipleConfigsApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;


namespace EDennis.Samples.MultipleConfigsApi.Controllers {

    [ApiController]
    [Route("[controller]")]
    public class MyApiClientController : ControllerBase {

        private readonly MyApiClient _instance;
        private readonly ServiceDescriptor _serviceDescriptor;

        public MyApiClientController(MyApiClient instance, IServiceCollection services) {
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

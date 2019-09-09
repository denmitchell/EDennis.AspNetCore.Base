For auto-generation of controllers, add this to Startup:
        public void ConfigureServices(IServiceCollection services) {
            services
                .AddMvc(options => {
                    options.Conventions.Add(new WriteableControllerRouteConvention());
                    options.Conventions.Add(new ReadonlyControllerRouteConvention());
                    //and then, if desired ...
                    options.Conventions.Add(new AddDefaultAuthorizationPolicyConvention(HostingEnvironment, Configuration));
                })
                .ConfigureApplicationPartManager(m => {
                    m.FeatureProviders.Add(new WriteableControllerFeatureProvider());
                    m.FeatureProviders.Add(new ReadonlyControllerFeatureProvider());
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
		...
		}

Add one or both of these attributes to each entity model class:
[WriteableController("api/myentity/writeable")]
[ReadonlyController("api/myentity/readonly")]
public class MyEntity{
  //...
}
For auto-generation of controllers, add this to Startup:
    services.
        AddMvc(options => {
		    options.Conventions.Add(new WriteableControllerRouteConvention());
		    options.Conventions.Add(new ReadonlyControllerRouteConvention());
            //and then, if desired ...
			options.Conventions.Add(new AddDefaultAuthorizationPolicyConvention(HostingEnvironment, Configuration));
	    })
        .ConfigureApplicationPartManager(m => {
            m.FeatureProviders.Add(new WriteableControllerFeatureProvider());
            m.FeatureProviders.Add(new ReadonlyControllerFeatureProvider());
		);

Add one or both of these attributes to each entity model class:
[WriteableController("iapi/myentity/writeable")]
[ReadonlyController("iapi/myentity/readonly")]
public class MyEntity{
  //...
}
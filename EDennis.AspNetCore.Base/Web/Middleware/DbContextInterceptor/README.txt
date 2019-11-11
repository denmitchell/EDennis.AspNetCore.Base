Configuration is done through calling IServiceCollectionExtensions_DbContext.AddDbContext
	(which returns an IDbContextSettingsBuilder) 
	and then (fluently) calling .AddInterceptor.
Because both DbContextInterceptor configuration and Repo configuration depend upon
	DbContext configuration, the configuration classes are centralized under Web\Extensions\Db
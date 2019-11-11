Configuration is done through calling IServiceCollectionExtensions_Api.AddApi
	(which returns an IApiSettingsBuilder) 
	and then (fluently) calling .AddLauncher.
Because both ApiLauncher configuration and ApiClient configuration depend upon
	Api configuration, the configuration classes are centralized under Web\Extensions\Api
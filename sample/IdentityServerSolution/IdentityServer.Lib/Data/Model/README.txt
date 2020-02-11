*Need to create easy way to add app-related configs
	(ApiResource/Scopes, AppUserClaims, Clients)
	to IdentityServer

	From scopes provided in AppUserClaims and Clients,
		the ApiResource/Scopes can be determined
	AppUserClaims should always have same ClientId = ProjectName
	For OIDC apps, the sole ClientId can be determined,
	  as can redirect URIs
		
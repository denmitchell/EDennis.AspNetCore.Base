If using NuGet packages for Child APIs, configuration files should follow a specific pattern,
   where the Child APIs use both 
    (a) an embedded appsettings.{env}.json file
    (b) a shared, regular appsettings.{Shared}.json file that lives in the Entry Point (parent) app


In Entry Point (Parent) App, include appsettings.Shared.json with Apis section with ports


In Child APIs, ensure that project file includes support for embedded file providers ...

<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>netcoreapp3.0</TargetFramework>
    <OutputType>Library</OutputType>
    <GenerateEmbeddedFilesManifest>true</GenerateEmbeddedFilesManifest>
    <GeneratePackageOnBuild>true</GeneratePackageOnBuild>
    <Version>1.0.0</Version>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.Extensions.Configuration.Json" Version="3.0.0" />
    <PackageReference Include="Microsoft.Extensions.FileProviders.Embedded" Version="3.0.0" />
    ...other package references
  </ItemGroup>

  <ItemGroup>
    <EmbeddedResource Include="appsettings.Development.json" />
  </ItemGroup>
  
</Project>


In Child APIs, ensure that Program.cs configuration uses ManifestEmbeddedFileProvider
   for appsettings.{env}.json and the regular file provider for appsettings.Shared.json
   (appsettings.Share.json is marked as optional)

            var fileProvider = new ManifestEmbeddedFileProvider(GetType().Assembly);
            Configuration = new ConfigurationBuilder()
                .AddJsonFile(fileProvider, "appsettings.{env}.json", true, true)
                .AddJsonFile("appsettings.Shared.json", true)
                .Build();



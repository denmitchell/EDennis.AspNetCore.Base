using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base {
    public class MvcOptionsBuilder : MvcOptions {
        private IServiceConfig _serviceConfig;

        public MvcOptionsBuilder(IServiceConfig serviceConfig) {
            _serviceConfig = serviceConfig;            
        }

        public IServiceConfig AddControllers() {
            _serviceConfig.Services.AddControllers(
                (config)=> {
                    foreach (var convention in Conventions)
                        config.Conventions.Add(convention);
                    foreach (var modelBinderProvider in ModelBinderProviders)
                        config.ModelBinderProviders.Add(modelBinderProvider);
                    foreach (var filter in Filters)
                        config.Filters.Add(filter);
                    foreach (var valueProviderFactory in ValueProviderFactories)
                        config.ValueProviderFactories.Add(valueProviderFactory);
                    foreach (var outputFormatter in OutputFormatters)
                        config.OutputFormatters.Add(outputFormatter);
                    foreach (var modelValidatorProvider in ModelValidatorProviders)
                        config.ModelValidatorProviders.Add(modelValidatorProvider);
                });
            return _serviceConfig;
        }

    }
}

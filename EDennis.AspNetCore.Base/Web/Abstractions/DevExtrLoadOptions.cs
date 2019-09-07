using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Web
{
    [ModelBinder(BinderType = typeof(DevExtrLoadOptionsBinder))]
    public class DevExtrLoadOptions : DataSourceLoadOptionsBase { }

    public class DevExtrLoadOptionsBinder : IModelBinder
    {

        public Task BindModelAsync(ModelBindingContext context) {
            var loadOptions = new DevExtrLoadOptions();

            DataSourceLoadOptionsParser.Parse(loadOptions, key => 
                context.ValueProvider.GetValue(key).FirstOrDefault());

            context.Result = ModelBindingResult.Success(loadOptions);

            return Task.CompletedTask;
        }

    }
}

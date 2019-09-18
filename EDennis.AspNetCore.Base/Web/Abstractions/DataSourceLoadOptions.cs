using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Web
{

    public static class DataSourceLoadOptionsBuilder{
        public static DataSourceLoadOptions Build(
            string select, string sort, string filter, int skip, int take,
            string totalSummary, string group, string groupSummary) {
            var loadOptions = new DataSourceLoadOptions {
                Select = (select == null) ? null : JToken.Parse(select).ToObject<string[]>(),
                Sort = (sort == null) ? null : JToken.Parse(sort).ToObject<SortingInfo[]>(),
                Filter = (filter == null) ? null : JToken.Parse(filter).ToObject<List<dynamic>>(),
                Skip = skip,
                Take = take,
                TotalSummary = (totalSummary == null) ? null : JToken.Parse(totalSummary).ToObject<SummaryInfo[]>(),
                Group = (group == null) ? null : JToken.Parse(group).ToObject<GroupingInfo[]>(),
                GroupSummary = (groupSummary == null) ? null : JToken.Parse(groupSummary).ToObject<SummaryInfo[]>(),
            };
            return loadOptions;
        }
    }


    [ModelBinder(BinderType = typeof(DataSourceLoadOptionsBinder))]
    public class DataSourceLoadOptions : DataSourceLoadOptionsBase { }

    public class DataSourceLoadOptionsBinder : IModelBinder
    {

        public Task BindModelAsync(ModelBindingContext context) {
            var loadOptions = new DataSourceLoadOptions();

            DataSourceLoadOptionsParser.Parse(loadOptions, key =>
                context.ValueProvider.GetValue(key).FirstOrDefault());

            context.Result = ModelBindingResult.Success(loadOptions);

            return Task.CompletedTask;
        }

    }




}



/*
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.Helpers;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

namespace EDennis.AspNetCore.Base.Web
{
    //from https://github.com/DevExpress/devextreme-examples/blob/17_2/datagrid-webapi/datagrid-webapi/DataSourceLoadOptions.cs
    [ModelBinder(typeof(DataSourceLoadOptionsHttpBinder))]
    public class DataSourceLoadOptions : DataSourceLoadOptionsBase { }

    public class DataSourceLoadOptionsHttpBinder : IModelBinder
    {

        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext) {
            var loadOptions = new DataSourceLoadOptions();
            DataSourceLoadOptionsParser.Parse(loadOptions, key => bindingContext.ValueProvider.GetValue(key)?.AttemptedValue);
            bindingContext.Model = loadOptions;
            return true;
        }

    }
}
*/

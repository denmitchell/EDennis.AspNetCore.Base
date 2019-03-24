using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Web {

    /// <summary>
    /// Typed version of ObjectResult class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectResult<T> : IActionResult, IStatusCodeActionResult {

        public ObjectResult() {
            Formatters = new FormatterCollection<IOutputFormatter>();
            ContentTypes = new MediaTypeCollection();
        }

        public int? StatusCode { get; set; }

        public T Value { get; set; }

        public FormatterCollection<IOutputFormatter> Formatters { get; set; }
        public MediaTypeCollection ContentTypes { get; set; }
        public Type DeclaredType { get; set; }

        public Task ExecuteResultAsync(ActionContext context) {
            var executor = context.HttpContext.RequestServices.GetRequiredService<IActionResultExecutor<ObjectResult<T>>>();
            return executor.ExecuteAsync(context, this);
        }

        public virtual void OnFormatting(ActionContext context) {
            if (context == null) {
                throw new ArgumentNullException(nameof(context));
            }

            if (StatusCode.HasValue) {
                context.HttpContext.Response.StatusCode = StatusCode.Value;
            }
        }
    }

}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.Web {
    /// <summary>
    /// These extension methods return the body object and status
    /// code from an ActionResult object.
    /// </summary>
    /// 
    public static class ActionResultExtensions {

        /// <summary>
        /// Returns the object contained in the response body
        /// </summary>
        /// <typeparam name="T">The type of the object</typeparam>
        /// <param name="result">The HTTP response object</param>
        /// <returns>The response body object or null/default value</returns>
        public static T GetObject<T>(this ActionResult<T> actionResult) {
            var result = actionResult.Result;
            if (result is ObjectResult) {
                var objResult = result as ObjectResult;
                return (T)objResult.Value;
            } else if (result is JsonResult) {
                var jsonResult = result as JsonResult;
                return (T)jsonResult.Value;
            } else
                return default;
        }

        /// <summary>
        /// Gets the Status Code from an action result or 0
        /// if the result does not have a StatusCode property
        /// </summary>
        /// <param name="result">IActionResult object</param>
        /// <returns>Status code of the HTTP response</returns>
        public static int GetStatusCode<T>(this ActionResult<T> actionResult) {
            var result = actionResult.Result;
            if (result is StatusCodeResult)
                return (result as StatusCodeResult).StatusCode;
            else if (result is ObjectResult)
                return (int)(result as ObjectResult).StatusCode;
            else if (result is ContentResult)
                return (int)(result as ContentResult).StatusCode;
            else if (result is JsonResult)
                return (int)(result as JsonResult).StatusCode;
            else if (result is ViewResult)
                return (int)(result as ViewResult).StatusCode;
            else if (result is ViewComponentResult)
                return (int)(result as ViewComponentResult).StatusCode;
            else if (result is PartialViewResult)
                return (int)(result as PartialViewResult).StatusCode;
            else if (result is PageResult)
                return (int)(result as PageResult).StatusCode;
            else
                return 0;
        }

    }
}

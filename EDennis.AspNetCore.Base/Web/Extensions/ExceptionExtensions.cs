using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using System;

namespace EDennis.AspNetCore.Base.Web {
    public static class ExceptionExtensions {

        public static ProblemDetails GetProblemDetails(this RequestException ex) {
            var pd = new ProblemDetails {
                Title = ex.Title,
                Detail = ex.Message                
            };
            return pd;
        }

        public static ProblemDetails GetProblemDetails(this Exception ex) {
            var pd = new ProblemDetails {
                Title = $"Exception at {ex.TargetSite.Name}",
                Detail = ex.Message
            };
            return pd;
        }


    }
}

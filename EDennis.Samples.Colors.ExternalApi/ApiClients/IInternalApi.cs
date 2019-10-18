using System.Collections.Generic;
using EDennis.AspNetCore.Base.Logging;
using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.Colors.ExternalApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EDennis.Samples.Colors.ExternalApi {
    public interface IInternalApi : IHasILogger {
        ObjectResult Create(Color color);
        ObjectResult Forward(HttpRequest request);
        ObjectResult GetColor(int id);
        ObjectResult GetColors();
    }
}
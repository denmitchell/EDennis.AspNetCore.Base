using System.Collections.Generic;
using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.Colors.ExternalApi.Models;
using Microsoft.AspNetCore.Http;

namespace EDennis.Samples.Colors.ExternalApi {
    public interface IInternalApi {
        HttpClientResult<Color> Create(Color color);
        HttpClientResult<Color> Forward(HttpRequest request);
        HttpClientResult<Color> GetColor(int id);
        HttpClientResult<List<Color>> GetColors();
    }
}
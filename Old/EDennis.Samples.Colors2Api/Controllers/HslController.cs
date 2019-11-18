using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.Colors2Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace EDennis.Samples.Colors2Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HslController : ReadonlyController<Hsl, ColorsDbContext>
    {
        public HslController(HslRepo repo) : base(repo) {
        }
    }
}

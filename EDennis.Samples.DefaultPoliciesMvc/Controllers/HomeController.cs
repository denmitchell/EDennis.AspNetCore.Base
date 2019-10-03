using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EDennis.Samples.DefaultPoliciesMvc.Models;
using Microsoft.AspNetCore.Authentication;


namespace EDennis.Samples.DefaultPoliciesMvc.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() {

            ViewData["Claims"] = User?.Claims?.ToDictionary(c => c.Type, c => c.Value);
            ViewData["AuthProps"] = (HttpContext.AuthenticateAsync()?.Result?.Properties?.Items);

            return View();
        }

        public IActionResult Privacy() {
            return View();
        }

        


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

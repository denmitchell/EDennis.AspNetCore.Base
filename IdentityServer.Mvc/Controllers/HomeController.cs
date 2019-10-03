using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IdentityServer.Mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace IdentityServer.Mvc.Controllers {

    public class HomeController : Controller {

        public IActionResult Index() {
            return View();
        }

        [Authorize]
        public IActionResult Secure() {
            ViewData["Message"] = "Secure page.";

            return View();
        }

        public IActionResult Logout() {
            return SignOut("Cookies", "oidc");
        }

        [Authorize]
        public async Task<IActionResult> CallPersonGetAll() {
            using (var client = await GetSecureClient()) {
                var content = await client.GetStringAsync("http://localhost:5006/api/Person/");

                ViewBag.Json = JArray.Parse(content).ToString();
            }
            return View("json");
        }

        public async Task<IActionResult> CallPersonGetOne() {
            using (var client = await GetSecureClient()) {
                var content = await client.GetStringAsync("http://localhost:5006/api/Person/1");

                ViewBag.Json = JObject.Parse(content).ToString();
            }
            return View("json");
        }

        public async Task<IActionResult> CallPersonDeleteOne() {
            using (var client = await GetSecureClient()) {
                await client.DeleteAsync("http://localhost:5006/api/Person/1");
            }

            return NoContent();
        }


        private async Task<HttpClient> GetSecureClient() {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            return client;
        }
    }
}

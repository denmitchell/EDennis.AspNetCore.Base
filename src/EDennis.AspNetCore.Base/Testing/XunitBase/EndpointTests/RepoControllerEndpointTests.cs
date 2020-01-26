using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Web;
using EDennis.NetCoreTestingUtilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing.XunitBase.EndpointTests {
    public abstract class RepoControllerEndpointTests<TEntity, TProgram, TLauncher>        
        : LauncherEndpointTests<TProgram, TLauncher>
        where TEntity : class, new()
        where TProgram : IProgram, new()
        where TLauncher : ILauncher, new() {
        public RepoControllerEndpointTests(ITestOutputHelper output, LauncherFixture<TProgram, TLauncher> launcherFixture) : base(output, launcherFixture) {
        }


        public ExpectedActual<EndpointTestResult<TEntity>> GetById_ExpectedActual(
            string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<string>("Id", Output);
            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);
            var expectedStatusCode = jsonTestCase.GetObject<int>("ExpectedStatusCode", Output);
            var expected = jsonTestCase.GetObject<TEntity>("Expected");

            var url = $"{controllerPath}/{id}";
            Output.WriteLine($"url: {url}");

            IActionResult getResult = HttpClient.Get<TEntity>(url);

            var eaResult = new ExpectedActual<EndpointTestResult<TEntity>> {
                Expected = new EndpointTestResult<TEntity> {
                    StatusCode = expectedStatusCode,
                    Data = expected
                },
                Actual = new EndpointTestResult<TEntity> {
                    StatusCode = getResult.GetStatusCode(),
                }
            };
            if (eaResult.Actual.StatusCode < 300 && eaResult.Expected.Data != null)
                eaResult.Actual.Data = getResult.GetObject<TEntity>();

            return eaResult;
        }

        public async Task<ExpectedActual<EndpointTestResult<TEntity>>> GetById_ExpectedActualAsync(
            string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<string>("Id", Output);
            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);
            var expectedStatusCode = jsonTestCase.GetObject<int>("ExpectedStatusCode", Output);
            var expected = jsonTestCase.GetObject<TEntity>("Expected");

            var url = $"{controllerPath}/async/{id}";
            Output.WriteLine($"url: {url}");

            IActionResult getResult = await HttpClient.GetAsync<TEntity>(url);

            var eaResult = new ExpectedActual<EndpointTestResult<TEntity>> {
                Expected = new EndpointTestResult<TEntity> {
                    StatusCode = expectedStatusCode,
                    Data = expected
                },
                Actual = new EndpointTestResult<TEntity> {
                    StatusCode = getResult.GetStatusCode(),
                }
            };
            if (eaResult.Actual.StatusCode < 300 && eaResult.Expected.Data != null)
                eaResult.Actual.Data = getResult.GetObject<TEntity>();

            return eaResult;
        }

        public ExpectedActual<EndpointTestResult<List<TEntity>>> Delete_ExpectedActual(
            string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<string>("Id", Output);
            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);
            var linqWhere = jsonTestCase.GetObject<string>("LinqWhere", Output);
            var expected = jsonTestCase.GetObject<List<TEntity>>("Expected");
            var expectedStatusCode = jsonTestCase.GetObject<int>("ExpectedStatusCode");

            var url = $"{controllerPath}/{id}";
            Output.WriteLine($"url: {url}");

            IActionResult deleteResult = HttpClient.Delete<TEntity>(url);

            var getUrl = $"{controllerPath}/linq?where={linqWhere}&X-Testing-Reset";

            IActionResult getResult = HttpClient.Get<DynamicLinqResult<TEntity>>(getUrl);

            var eaResult = new ExpectedActual<EndpointTestResult<List<TEntity>>> {
                Expected = new EndpointTestResult<List<TEntity>> {
                    StatusCode = expectedStatusCode,
                    Data = expected
                },
                Actual = new EndpointTestResult<List<TEntity>> {
                    StatusCode = deleteResult.GetStatusCode(),
                }
            };
            if (eaResult.Actual.StatusCode < 300 && eaResult.Expected.Data != null)
                eaResult.Actual.Data = getResult.GetObject<DynamicLinqResult<TEntity>>().Data;

            return eaResult;
        }


        public async Task<ExpectedActual<EndpointTestResult<List<TEntity>>>> Delete_ExpectedActualAsync(
            string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<string>("Id", Output);
            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);
            var linqWhere = jsonTestCase.GetObject<string>("LinqWhere", Output);
            var expected = jsonTestCase.GetObject<List<TEntity>>("Expected");
            var expectedStatusCode = jsonTestCase.GetObject<int>("ExpectedStatusCode");

            var url = $"{controllerPath}/async/{id}";
            Output.WriteLine($"url: {url}");

            IActionResult deleteResult = await HttpClient.DeleteAsync<TEntity>(url);

            var getUrl = $"{controllerPath}/linq?where={linqWhere}&X-Testing-Reset";

            IActionResult getResult = HttpClient.Get<DynamicLinqResult<TEntity>>(getUrl);

            var eaResult = new ExpectedActual<EndpointTestResult<List<TEntity>>> {
                Expected = new EndpointTestResult<List<TEntity>> {
                    StatusCode = expectedStatusCode,
                    Data = expected
                },
                Actual = new EndpointTestResult<List<TEntity>> {
                    StatusCode = deleteResult.GetStatusCode(),
                }
            };
            if (eaResult.Actual.StatusCode < 300 && eaResult.Expected.Data != null)
                eaResult.Actual.Data = getResult.GetObject<DynamicLinqResult<TEntity>>().Data;

            return eaResult;
        }


        public ExpectedActual<EndpointTestResult<List<TEntity>>> Post_ExpectedActual(
            string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var input = jsonTestCase.GetObject<TEntity>("Input", Output);
            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);
            var linqWhere = jsonTestCase.GetObject<string>("LinqWhere", Output);
            var expected = jsonTestCase.GetObject<List<TEntity>>("Expected");
            var expectedStatusCode = jsonTestCase.GetObject<int>("ExpectedStatusCode");

            var url = $"{controllerPath}";
            Output.WriteLine($"url: {url}");

            IActionResult postResult = HttpClient.Post(url,input);

            var getUrl = $"{controllerPath}/linq?where={linqWhere}&X-Testing-Reset";

            IActionResult getResult = HttpClient.Get<DynamicLinqResult<TEntity>>(getUrl);

            var eaResult = new ExpectedActual<EndpointTestResult<List<TEntity>>> {
                Expected = new EndpointTestResult<List<TEntity>> {
                    StatusCode = expectedStatusCode,
                    Data = expected
                },
                Actual = new EndpointTestResult<List<TEntity>> {
                    StatusCode = postResult.GetStatusCode(),
                }
            };
            if (eaResult.Actual.StatusCode < 300 && eaResult.Expected.Data != null)
                eaResult.Actual.Data = getResult.GetObject<DynamicLinqResult<TEntity>>().Data;

            return eaResult;
        }



        public async Task<ExpectedActual<EndpointTestResult<List<TEntity>>>> Post_ExpectedActualAsync(
            string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var input = jsonTestCase.GetObject<TEntity>("Input", Output);
            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);
            var linqWhere = jsonTestCase.GetObject<string>("LinqWhere", Output);
            var expected = jsonTestCase.GetObject<List<TEntity>>("Expected");
            var expectedStatusCode = jsonTestCase.GetObject<int>("ExpectedStatusCode");

            var url = $"{controllerPath}/async";
            Output.WriteLine($"url: {url}");

            IActionResult postResult = await HttpClient.PostAsync(url, input);

            var getUrl = $"{controllerPath}/linq?where={linqWhere}&X-Testing-Reset";

            IActionResult getResult = HttpClient.Get<DynamicLinqResult<TEntity>>(getUrl);

            var eaResult = new ExpectedActual<EndpointTestResult<List<TEntity>>> {
                Expected = new EndpointTestResult<List<TEntity>> {
                    StatusCode = expectedStatusCode,
                    Data = expected
                },
                Actual = new EndpointTestResult<List<TEntity>> {
                    StatusCode = postResult.GetStatusCode(),
                }
            };
            if (eaResult.Actual.StatusCode < 300 && eaResult.Expected.Data != null)
                eaResult.Actual.Data = getResult.GetObject<DynamicLinqResult<TEntity>>().Data;

            return eaResult;
        }




        public ExpectedActual<EndpointTestResult<List<TEntity>>> Put_ExpectedActual(
            string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<TEntity>("Id", Output);
            var input = jsonTestCase.GetObject<TEntity>("Input", Output);
            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);
            var linqWhere = jsonTestCase.GetObject<string>("LinqWhere", Output);
            var expected = jsonTestCase.GetObject<List<TEntity>>("Expected");
            var expectedStatusCode = jsonTestCase.GetObject<int>("ExpectedStatusCode");

            var url = $"{controllerPath}/{id}";
            Output.WriteLine($"url: {url}");

            IActionResult putResult = HttpClient.Put(url, input);

            var getUrl = $"{controllerPath}/linq?where={linqWhere}&X-Testing-Reset";

            IActionResult getResult = HttpClient.Get<DynamicLinqResult<TEntity>>(getUrl);

            var eaResult = new ExpectedActual<EndpointTestResult<List<TEntity>>> {
                Expected = new EndpointTestResult<List<TEntity>> {
                    StatusCode = expectedStatusCode,
                    Data = expected
                },
                Actual = new EndpointTestResult<List<TEntity>> {
                    StatusCode = putResult.GetStatusCode(),
                }
            };
            if (eaResult.Actual.StatusCode < 300 && eaResult.Expected.Data != null)
                eaResult.Actual.Data = getResult.GetObject<DynamicLinqResult<TEntity>>().Data;

            return eaResult;
        }



        public async Task<ExpectedActual<EndpointTestResult<List<TEntity>>>> Put_ExpectedActualAsync(
            string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<TEntity>("Id", Output);
            var input = jsonTestCase.GetObject<TEntity>("Input", Output);
            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);
            var linqWhere = jsonTestCase.GetObject<string>("LinqWhere", Output);
            var expected = jsonTestCase.GetObject<List<TEntity>>("Expected");
            var expectedStatusCode = jsonTestCase.GetObject<int>("ExpectedStatusCode");

            var url = $"{controllerPath}/async/{id}";
            Output.WriteLine($"url: {url}");

            IActionResult putResult = await HttpClient.PostAsync(url, input);

            var getUrl = $"{controllerPath}/linq?where={linqWhere}&X-Testing-Reset";

            IActionResult getResult = HttpClient.Get<DynamicLinqResult<TEntity>>(getUrl);

            var eaResult = new ExpectedActual<EndpointTestResult<List<TEntity>>> {
                Expected = new EndpointTestResult<List<TEntity>> {
                    StatusCode = expectedStatusCode,
                    Data = expected
                },
                Actual = new EndpointTestResult<List<TEntity>> {
                    StatusCode = putResult.GetStatusCode(),
                }
            };
            if (eaResult.Actual.StatusCode < 300 && eaResult.Expected.Data != null)
                eaResult.Actual.Data = getResult.GetObject<DynamicLinqResult<TEntity>>().Data;

            return eaResult;
        }



        public ExpectedActual<EndpointTestResult<List<TEntity>>> Patch_ExpectedActual(
            string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<TEntity>("Id", Output);
            var input = jsonTestCase.GetObject<dynamic,TEntity>("Input", Output);
            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);
            var linqWhere = jsonTestCase.GetObject<string>("LinqWhere", Output);
            var expected = jsonTestCase.GetObject<List<TEntity>>("Expected");
            var expectedStatusCode = jsonTestCase.GetObject<int>("ExpectedStatusCode");

            var url = $"{controllerPath}/{id}";
            Output.WriteLine($"url: {url}");

            IActionResult patchResult = HttpClientExtensions.Patch(HttpClient,url, input);

            var getUrl = $"{controllerPath}/linq?where={linqWhere}&X-Testing-Reset";

            IActionResult getResult = HttpClient.Get<DynamicLinqResult<TEntity>>(getUrl);

            var eaResult = new ExpectedActual<EndpointTestResult<List<TEntity>>> {
                Expected = new EndpointTestResult<List<TEntity>> {
                    StatusCode = expectedStatusCode,
                    Data = expected
                },
                Actual = new EndpointTestResult<List<TEntity>> {
                    StatusCode = patchResult.GetStatusCode(),
                }
            };
            if (eaResult.Actual.StatusCode < 300 && eaResult.Expected.Data != null)
                eaResult.Actual.Data = getResult.GetObject<DynamicLinqResult<TEntity>>().Data;

            return eaResult;
        }



        public async Task<ExpectedActual<EndpointTestResult<List<TEntity>>>> Patch_ExpectedActualAsync(
            string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<TEntity>("Id", Output);
            var input = jsonTestCase.GetObject<dynamic,TEntity>("Input", Output);
            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);
            var linqWhere = jsonTestCase.GetObject<string>("LinqWhere", Output);
            var expected = jsonTestCase.GetObject<List<TEntity>>("Expected");
            var expectedStatusCode = jsonTestCase.GetObject<int>("ExpectedStatusCode");

            var url = $"{controllerPath}/async/{id}";
            Output.WriteLine($"url: {url}");

            IActionResult patchResult = await HttpClientExtensions.PatchAsync(HttpClient, url, input);

            var getUrl = $"{controllerPath}/linq?where={linqWhere}&X-Testing-Reset";

            IActionResult getResult = HttpClient.Get<DynamicLinqResult<TEntity>>(getUrl);

            var eaResult = new ExpectedActual<EndpointTestResult<List<TEntity>>> {
                Expected = new EndpointTestResult<List<TEntity>> {
                    StatusCode = expectedStatusCode,
                    Data = expected
                },
                Actual = new EndpointTestResult<List<TEntity>> {
                    StatusCode = patchResult.GetStatusCode(),
                }
            };
            if (eaResult.Actual.StatusCode < 300 && eaResult.Expected.Data != null)
                eaResult.Actual.Data = getResult.GetObject<DynamicLinqResult<TEntity>>().Data;

            return eaResult;
        }


        /// <summary>
        /// Returns actual and expected results from GetDevExtreme.
        /// Note: this method looks for the following optional TestJson
        /// parameters (case sensitive):
        /// Select, Filter, Sort, Skip, Take, 
        ///    and ControllerPath
        /// </summary>
        /// <param name="jsonTestCase"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public ExpectedActual<EndpointTestResult<List<TEntity>>> GetDevExtreme_ExpectedActual(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);
            var select = jsonTestCase.GetObjectOrDefault<string>("Select", Output);
            var filter = jsonTestCase.GetObjectOrDefault<string>("Filter", Output);
            var sort = jsonTestCase.GetObjectOrDefault<string>("Sort", Output);
            var skip = jsonTestCase.GetObjectOrDefault<int>("Skip", Output);
            var take = jsonTestCase.GetObjectOrDefault<int>("Take", Output);
            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);
            var expected = jsonTestCase.GetObject<List<TEntity>>("Expected");
            var expectedStatusCode = jsonTestCase.GetObject<int>("ExpectedStatusCode");


            if (take == default)
                take = int.MaxValue;

            var queryStrings = new List<string>();

            if (select != default)
                queryStrings.Add($"select={select}");
            if (filter != default)
                queryStrings.Add($"filter={filter}");
            if (sort != default)
                queryStrings.Add($"sort={sort}");

            queryStrings.Add($"skip={skip}");
            queryStrings.Add($"take={take}");

            var url = $"{controllerPath}/devextreme?{string.Join('&', queryStrings)}";

            Output.WriteLine($"url: {url}");

            var actualLoadResult = HttpClient.Get<DeserializableLoadResult<TEntity>>(url);

            var eaResult = new ExpectedActual<EndpointTestResult<List<TEntity>>> {
                Expected = new EndpointTestResult<List<TEntity>> {
                    StatusCode = expectedStatusCode,
                    Data = expected
                },
                Actual = new EndpointTestResult<List<TEntity>> {
                    StatusCode = 200,
                }
            };
            if (eaResult.Actual.StatusCode < 300 && eaResult.Expected.Data != null)
                eaResult.Actual.Data = actualLoadResult.GetObject<DeserializableLoadResult<TEntity>>().data.ToList();
            

            return eaResult;

        }


        /// <summary>
        /// Returns actual and expected results from GetDevExtreme.
        /// Note: this method looks for the following optional TestJson
        /// parameters (case sensitive):
        /// Select, Filter, Sort, Skip, Take, 
        ///    and ControllerPath
        /// </summary>
        /// <param name="jsonTestCase"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public async Task<ExpectedActual<EndpointTestResult<List<TEntity>>>> GetDevExtreme_ExpectedActualAsync(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);
            var select = jsonTestCase.GetObjectOrDefault<string>("Select", Output);
            var filter = jsonTestCase.GetObjectOrDefault<string>("Filter", Output);
            var sort = jsonTestCase.GetObjectOrDefault<string>("Sort", Output);
            var skip = jsonTestCase.GetObjectOrDefault<int>("Skip", Output);
            var take = jsonTestCase.GetObjectOrDefault<int>("Take", Output);
            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);
            var expected = jsonTestCase.GetObject<List<TEntity>>("Expected");
            var expectedStatusCode = jsonTestCase.GetObject<int>("ExpectedStatusCode");


            if (take == default)
                take = int.MaxValue;

            var queryStrings = new List<string>();

            if (select != default)
                queryStrings.Add($"select={select}");
            if (filter != default)
                queryStrings.Add($"filter={filter}");
            if (sort != default)
                queryStrings.Add($"sort={sort}");

            queryStrings.Add($"skip={skip}");
            queryStrings.Add($"take={take}");

            var url = $"{controllerPath}/devextreme/async?{string.Join('&', queryStrings)}";

            Output.WriteLine($"url: {url}");

            var actualLoadResult = await HttpClient.GetAsync<DeserializableLoadResult<TEntity>>(url);

            var eaResult = new ExpectedActual<EndpointTestResult<List<TEntity>>> {
                Expected = new EndpointTestResult<List<TEntity>> {
                    StatusCode = expectedStatusCode,
                    Data = expected
                },
                Actual = new EndpointTestResult<List<TEntity>> {
                    StatusCode = 200,
                }
            };
            if (eaResult.Actual.StatusCode < 300 && eaResult.Expected.Data != null)
                eaResult.Actual.Data = actualLoadResult.GetObject<DeserializableLoadResult<TEntity>>().data.ToList();


            return eaResult;

        }



        /// <summary>
        /// Returns actual and expected results from GetDynamicLinq.
        /// Note: this method looks for the following optional TestJson
        /// parameters (case sensitive):
        /// Where, OrderBy, Select, Skip, Take, and ControllerPath
        /// </summary>
        /// <param name="jsonTestCase"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        private ExpectedActual<EndpointTestResult<DynamicLinqResult<dynamic>>> GetDynamicLinq_ExpectedActual(string t, JsonTestCase jsonTestCase, bool isAsync) {
            Output.WriteLine(t);
            var where = jsonTestCase.GetObjectOrDefault<string>("Where", Output);
            var orderBy = jsonTestCase.GetObjectOrDefault<string>("OrderBy", Output);
            var select = jsonTestCase.GetObjectOrDefault<string>("Select", Output);
            var skip = jsonTestCase.GetObjectOrDefault<int?>("Skip", Output);
            var take = jsonTestCase.GetObjectOrDefault<int?>("Take", Output);
            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);
            var expected = jsonTestCase.GetObject<DynamicLinqResult<dynamic>,TEntity>("Expected");
            var expectedStatusCode = jsonTestCase.GetObject<int>("ExpectedStatusCode");

            var queryStrings = new List<string>();

            if (where != default)
                queryStrings.Add($"where={where}");
            if (orderBy != default)
                queryStrings.Add($"orderBy={orderBy}");
            if (select != default)
                queryStrings.Add($"select={select}");
            if (skip != default)
                queryStrings.Add($"skip={skip}");
            if (take != default)
                queryStrings.Add($"take={take}");

            var url = $"{controllerPath}/linq?{string.Join('&', queryStrings)}";

            Output.WriteLine($"url: {url}");

            IActionResult actualResult = HttpClient.Get<DynamicLinqResult<TEntity>,TEntity>(url);


            var eaResult = new ExpectedActual<EndpointTestResult<DynamicLinqResult<dynamic>>> {
                Expected = new EndpointTestResult<DynamicLinqResult<dynamic>> {
                    StatusCode = expectedStatusCode,
                    Data = expected
                },
                Actual = new EndpointTestResult<DynamicLinqResult<dynamic>> {
                    StatusCode = 200,
                }
            };
            if (eaResult.Actual.StatusCode < 300 && eaResult.Expected.Data != null)
                eaResult.Actual.Data = actualResult.GetObject<DynamicLinqResult<dynamic>>();

            return eaResult;
        }



        /// <summary>
        /// Returns actual and expected results from GetDynamicLinq.
        /// Note: this method looks for the following optional TestJson
        /// parameters (case sensitive):
        /// Where, OrderBy, Select, Skip, Take, and ControllerPath
        /// </summary>
        /// <param name="jsonTestCase"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        private async Task<ExpectedActual<EndpointTestResult<DynamicLinqResult<dynamic>>>> GetDynamicLinq_ExpectedActualAsync(string t, JsonTestCase jsonTestCase, bool isAsync) {
            Output.WriteLine(t);
            var where = jsonTestCase.GetObjectOrDefault<string>("Where", Output);
            var orderBy = jsonTestCase.GetObjectOrDefault<string>("OrderBy", Output);
            var select = jsonTestCase.GetObjectOrDefault<string>("Select", Output);
            var skip = jsonTestCase.GetObjectOrDefault<int?>("Skip", Output);
            var take = jsonTestCase.GetObjectOrDefault<int?>("Take", Output);
            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);
            var expected = jsonTestCase.GetObject<DynamicLinqResult<dynamic>, TEntity>("Expected");
            var expectedStatusCode = jsonTestCase.GetObject<int>("ExpectedStatusCode");

            var queryStrings = new List<string>();

            if (where != default)
                queryStrings.Add($"where={where}");
            if (orderBy != default)
                queryStrings.Add($"orderBy={orderBy}");
            if (select != default)
                queryStrings.Add($"select={select}");
            if (skip != default)
                queryStrings.Add($"skip={skip}");
            if (take != default)
                queryStrings.Add($"take={take}");

            var url = $"{controllerPath}/linq?{string.Join('&', queryStrings)}";

            Output.WriteLine($"url: {url}");

            IActionResult actualResult = await HttpClient.GetAsync<DynamicLinqResult<TEntity>, TEntity>(url);


            var eaResult = new ExpectedActual<EndpointTestResult<DynamicLinqResult<dynamic>>> {
                Expected = new EndpointTestResult<DynamicLinqResult<dynamic>> {
                    StatusCode = expectedStatusCode,
                    Data = expected
                },
                Actual = new EndpointTestResult<DynamicLinqResult<dynamic>> {
                    StatusCode = 200,
                }
            };
            if (eaResult.Actual.StatusCode < 300 && eaResult.Expected.Data != null)
                eaResult.Actual.Data = actualResult.GetObject<DynamicLinqResult<dynamic>>();

            return eaResult;
        }


    }
}
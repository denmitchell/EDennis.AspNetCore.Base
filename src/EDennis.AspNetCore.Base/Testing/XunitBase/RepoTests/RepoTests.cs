using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.NetCoreTestingUtilities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core.Exceptions;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {

    public abstract class RepoTests<TRepo, TEntity, TContext>
            : IDisposable
        where TEntity : class, IHasSysUser, new()
        where TContext : DbContext, ISqlServerDbContext<TContext>
        where TRepo : Repo<TEntity, TContext> {

        protected ITestOutputHelper Output { get; }
        protected TRepo Repo { get; }
        protected TestRepoFactory<TRepo, TEntity, TContext> Factory { get; }

        public RepoTests(ITestOutputHelper output) {
            Output = output;
            Factory = new TestRepoFactory<TRepo, TEntity, TContext>();
            Repo = Factory.CreateRepo();
        }


        protected static JsonSerializerOptions DynamicJsonSerializerOptions { get; }

        static RepoTests() {
            DynamicJsonSerializerOptions = Base.DynamicJsonSerializerOptions.Create<TEntity>();
        }


        /// <summary>
        /// <para>Returns the expected and actual result for a call to Get.</para>
        /// <para>----------------------------------</para>
        /// <para>REQUIRED TESTJSON FILES</para>
        /// <para>----------------------------------</para>
        /// <list type="table">
        /// <item>
        ///     <term>Id</term><description>Primary key</description>
        /// </item>
        /// <item>
        ///     <term>Expected</term><description>Expected result (required)</description>
        /// </item>
        /// <item>
        ///     <term>Exception</term><description>"ArgumentException" or null (optional)</description>
        /// </item>
        /// </list>
        /// <para>NOTE: An ArgumentException is thrown when the Id parameter is not the correct type</para>
        /// </summary>
        /// <param name="t">TestScenario(TestCase)</param>
        /// <param name="jsonTestCase">Test input parameters and expected results</param>
        /// <returns>An object holding expected and actual results</returns>
        public ExpectedActual<RepoTestResult<TEntity>> Get_ExpectedActual(
            string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var idStr = jsonTestCase.GetObject<string>("Id", Output);
            var expected = jsonTestCase.GetObject<TEntity>("Expected");
            var exception = jsonTestCase.GetObject<string>("Exception", Output);

            var eaResult = new ExpectedActual<RepoTestResult<TEntity>> {
                Expected = new RepoTestResult<TEntity> { Data = expected, Exception = exception },
                Actual = new RepoTestResult<TEntity> { }
            };

            try {
                var id = Repo<TEntity, TContext>.ParseId(idStr);
                eaResult.Actual.Data = Repo.Get(id);
            } catch (Exception ex) {
                eaResult.Actual.Exception = ex.GetType().Name;
                Output.WriteLine($"EXCEPTION ({ex.GetType().Name}): {ex.Message}");
                Output.WriteLine($"STACK TRACE:\n {ex.StackTrace}");
            }

            return eaResult;
        }

        /// <summary>
        /// <para>Returns the expected and actual result for a call to GetAsync.</para>
        /// <para>----------------------------------</para>
        /// <para>REQUIRED TESTJSON FILES</para>
        /// <para>----------------------------------</para>
        /// <list type="table">
        /// <item>
        ///     <term>Id</term><description>Primary key</description>
        /// </item>
        /// <item>
        ///     <term>Expected</term><description>Expected result (required)</description>
        /// </item>
        /// <item>
        ///     <term>Exception</term><description>"ArgumentException" or null (optional)</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="t">TestScenario(TestCase)</param>
        /// <param name="jsonTestCase">Test input parameters and expected results</param>
        /// <returns>An object holding expected and actual results</returns>
        public async Task<ExpectedActual<RepoTestResult<TEntity>>> GetAsync_ExpectedActual(
            string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var idStr = jsonTestCase.GetObject<string>("Id", Output);
            var expected = jsonTestCase.GetObject<TEntity>("Expected");
            var exception = jsonTestCase.GetObject<string>("Exception", Output);

            var eaResult = new ExpectedActual<RepoTestResult<TEntity>> {
                Expected = new RepoTestResult<TEntity> { Data = expected, Exception = exception },
                Actual = new RepoTestResult<TEntity> { }
            };

            try {
                var id = Repo<TEntity, TContext>.ParseId(idStr);
                eaResult.Actual.Data = await Repo.GetAsync(id);
            } catch (Exception ex) {
                eaResult.Actual.Exception = ex.GetType().Name;
                Output.WriteLine($"EXCEPTION ({ex.GetType().Name}): {ex.Message}");
                Output.WriteLine($"STACK TRACE:\n {ex.StackTrace}");
            }

            return eaResult;
        }

        /// <summary>
        /// <para>Invokes Delete on a repo and returns the expected and
        /// actual result of a follow-up verification query using a 
        /// Linq Where clause.</para>
        /// <para>----------------------------------</para>
        /// <para>REQUIRED TESTJSON FILES</para>
        /// <para>----------------------------------</para>
        /// <list type="table">
        /// <item>
        ///     <term>Id</term><description>Primary key</description>
        /// </item>
        /// <item>
        ///     <term>Expected</term><description>Expected result (required)</description>
        /// </item>
        /// <item>
        ///     <term>Exception</term><description>"ArgumentException", "MissingEntityException" or null(optional)</description>
        /// </item>
        /// <item>
        ///     <term>LinqWhere</term><description>Dynamic Linq expression for follow-up GET request</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="t">TestScenario(TestCase)</param>
        /// <param name="jsonTestCase">Test input parameters and expected results</param>
        /// <returns>An object holding expected and actual results</returns>
        public ExpectedActual<RepoTestResult<List<TEntity>>> Delete_ExpectedActual(
            string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var idStr = jsonTestCase.GetObject<string>("Id", Output);
            var linqWhere = jsonTestCase.GetObject<string>("LinqWhere", Output);
            var expected = jsonTestCase.GetObjectOrDefault<List<TEntity>>("Expected");
            var exception = jsonTestCase.GetObject<string>("Exception", Output);

            var eaResult = new ExpectedActual<RepoTestResult<List<TEntity>>> {
                Expected = new RepoTestResult<List<TEntity>> { Data = expected, Exception = exception },
                Actual = new RepoTestResult<List<TEntity>> { }
            };

            try {
                var id = Repo<TEntity, TContext>.ParseId(idStr);
                Repo.Delete(id);
            } catch (Exception ex) {
                eaResult.Actual.Exception = ex.GetType().Name;
                Output.WriteLine($"EXCEPTION ({ex.GetType().Name}): {ex.Message}");
                Output.WriteLine($"STACK TRACE:\n {ex.StackTrace}");
            }

            if(eaResult.Expected.Data != null)
                eaResult.Actual.Data = Repo.GetWithDynamicLinq(where: linqWhere).Data;

            return eaResult;
        }


        /// <summary>
        /// <para>Invokes DeleteAsync on a repo and returns the expected and
        /// actual result of a follow-up verification query using a 
        /// Linq Where clause.</para>
        /// <para>----------------------------------</para>
        /// <para>REQUIRED TESTJSON FILES</para>
        /// <para>----------------------------------</para>
        /// <list type="table">
        /// <item>
        ///     <term>Id</term><description>Primary key</description>
        /// </item>
        /// <item>
        ///     <term>Expected</term><description>Expected result (required)</description>
        /// </item>
        /// <item>
        ///     <term>Exception</term><description>"ArgumentException", "MissingEntityException" or null (optional)</description>
        /// </item>
        /// <item>
        ///     <term>LinqWhere</term><description>Dynamic Linq expression for follow-up GET request</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="t">TestScenario(TestCase)</param>
        /// <param name="jsonTestCase">Test input parameters and expected results</param>
        /// <returns>An object holding expected and actual results</returns>
        public async Task<ExpectedActual<RepoTestResult<List<TEntity>>>> DeleteAsync_ExpectedActual(
            string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var idStr = jsonTestCase.GetObject<string>("Id", Output);
            var linqWhere = jsonTestCase.GetObject<string>("LinqWhere", Output);
            var expected = jsonTestCase.GetObject<List<TEntity>>("Expected");
            var exception = jsonTestCase.GetObject<string>("Exception", Output);

            var eaResult = new ExpectedActual<RepoTestResult<List<TEntity>>> {
                Expected = new RepoTestResult<List<TEntity>> { Data = expected, Exception = exception },
                Actual = new RepoTestResult<List<TEntity>> { }
            };

            try {
                var id = Repo<TEntity, TContext>.ParseId(idStr);
                await Repo.DeleteAsync(id);
            } catch (Exception ex) {
                eaResult.Actual.Exception = ex.GetType().Name;
                Output.WriteLine($"EXCEPTION ({ex.GetType().Name}): {ex.Message}");
                Output.WriteLine($"STACK TRACE:\n {ex.StackTrace}");
            }

            if (eaResult.Expected.Data != null)
                eaResult.Actual.Data = Repo.GetWithDynamicLinq(where: linqWhere).Data;

            return eaResult;
        }


        /// <summary>
        /// <para>Invokes Create on a repo and returns the expected and
        /// actual result of a follow-up verification query using a 
        /// Linq Where clause.</para>
        /// <para>----------------------------------</para>
        /// <para>REQUIRED TESTJSON FILES</para>
        /// <para>----------------------------------</para>
        /// <list type="table">
        /// <item>
        ///     <term>Input</term><description>JSON input object</description>
        /// </item>
        /// <item>
        ///     <term>Expected</term><description>Expected result (required)</description>
        /// </item>
        /// <item>
        ///     <term>Exception</term><description>"DbUpdateException" or null (optional)</description>
        /// </item>
        /// <item>
        ///     <term>LinqWhere</term><description>Dynamic Linq expression for follow-up GET request</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="t">TestScenario(TestCase)</param>
        /// <param name="jsonTestCase">Test input parameters and expected results</param>
        /// <returns>An object holding expected and actual results</returns>
        public ExpectedActual<RepoTestResult<List<TEntity>>> Create_ExpectedActual(
            string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var input = jsonTestCase.GetObject<TEntity>("Input", Output);
            var linqWhere = jsonTestCase.GetObject<string>("LinqWhere", Output);
            var expected = jsonTestCase.GetObject<List<TEntity>>("Expected");
            var exception = jsonTestCase.GetObject<string>("Exception", Output);

            var eaResult = new ExpectedActual<RepoTestResult<List<TEntity>>> {
                Expected = new RepoTestResult<List<TEntity>> { Data = expected, Exception = exception },
                Actual = new RepoTestResult<List<TEntity>> { }
            };

            try {
                Repo.Create(input);
            } catch (Exception ex) {
                eaResult.Actual.Exception = ex.GetType().Name;
                Output.WriteLine($"EXCEPTION ({ex.GetType().Name}): {ex.Message}");
                Output.WriteLine($"STACK TRACE:\n {ex.StackTrace}");
            }

            if (eaResult.Expected.Data != null)
                eaResult.Actual.Data = Repo.GetWithDynamicLinq(where: linqWhere).Data;

            return eaResult;
        }



        /// <summary>
        /// <para>Invokes CreateAsync on a repo and returns the expected and
        /// actual result of a follow-up verification query using a 
        /// Linq Where clause.</para>
        /// <para>----------------------------------</para>
        /// <para>REQUIRED TESTJSON FILES</para>
        /// <para>----------------------------------</para>
        /// <list type="table">
        /// <item>
        ///     <term>Input</term><description>JSON input object</description>
        /// </item>
        /// <item>
        ///     <term>Expected</term><description>Expected result (required)</description>
        /// </item>
        /// <item>
        ///     <term>Exception</term><description>"DbUpdateException" or null (optional)</description>
        /// </item>
        /// <item>
        ///     <term>LinqWhere</term><description>Dynamic Linq expression for follow-up GET request</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="t">TestScenario(TestCase)</param>
        /// <param name="jsonTestCase">Test input parameters and expected results</param>
        /// <returns>An object holding expected and actual results</returns>
        public async Task<ExpectedActual<RepoTestResult<List<TEntity>>>> CreateAsync_ExpectedActual(
            string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var input = jsonTestCase.GetObject<TEntity>("Input", Output);
            var linqWhere = jsonTestCase.GetObject<string>("LinqWhere", Output);
            var expected = jsonTestCase.GetObject<List<TEntity>>("Expected");
            var exception = jsonTestCase.GetObject<string>("Exception", Output);

            var eaResult = new ExpectedActual<RepoTestResult<List<TEntity>>> {
                Expected = new RepoTestResult<List<TEntity>> { Data = expected, Exception = exception },
                Actual = new RepoTestResult<List<TEntity>> { }
            };

            try {
                await Repo.CreateAsync(input);
            } catch (Exception ex) {
                eaResult.Actual.Exception = ex.GetType().Name;
                Output.WriteLine($"EXCEPTION ({ex.GetType().Name}): {ex.Message}");
                Output.WriteLine($"STACK TRACE:\n {ex.StackTrace}");
            }

            if (eaResult.Expected.Data != null)
                eaResult.Actual.Data = Repo.GetWithDynamicLinq(where: linqWhere).Data;

            return eaResult;
        }




        /// <summary>
        /// <para>Invokes Update on a repo and returns the expected and
        /// actual result of a follow-up verification query using a 
        /// Linq Where clause.</para>
        /// <para>----------------------------------</para>
        /// <para>REQUIRED TESTJSON FILES</para>
        /// <para>----------------------------------</para>
        /// <list type="table">
        /// <item>
        ///     <term>Id</term><description>Primary key</description>
        /// </item>
        /// <item>
        ///     <term>Input</term><description>JSON input object</description>
        /// </item>
        /// <item>
        ///     <term>Expected</term><description>Expected result (required)</description>
        /// </item>
        /// <item>
        ///     <term>Exception</term><description>"ArgumentException", "MissingEntityException" or null (optional)</description>
        /// </item>
        /// <item>
        ///     <term>LinqWhere</term><description>Dynamic Linq expression for follow-up GET request</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="t">TestScenario(TestCase)</param>
        /// <param name="jsonTestCase">Test input parameters and expected results</param>
        /// <returns>An object holding expected and actual results</returns>
        public ExpectedActual<RepoTestResult<List<TEntity>>> Update_ExpectedActual(
            string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var idStr = jsonTestCase.GetObject<string>("Id", Output);
            var input = jsonTestCase.GetObject<TEntity>("Input", Output);
            var linqWhere = jsonTestCase.GetObject<string>("LinqWhere", Output);
            var expected = jsonTestCase.GetObject<List<TEntity>>("Expected");
            var exception = jsonTestCase.GetObject<string>("Exception", Output);


            var eaResult = new ExpectedActual<RepoTestResult<List<TEntity>>> {
                Expected = new RepoTestResult<List<TEntity>> { Data = expected, Exception = exception },
                Actual = new RepoTestResult<List<TEntity>> { }
            };

            try {
                var id = Repo<TEntity, TContext>.ParseId(idStr);
                Repo.Update(input, id);
            } catch (Exception ex) {
                eaResult.Actual.Exception = ex.GetType().Name;
                Output.WriteLine($"EXCEPTION ({ex.GetType().Name}): {ex.Message}");
                Output.WriteLine($"STACK TRACE:\n {ex.StackTrace}");
            }

            if (eaResult.Expected.Data != null)
                eaResult.Actual.Data = Repo.GetWithDynamicLinq(where: linqWhere).Data;

            return eaResult;
        }



        /// <summary>
        /// <para>Invokes UpdateAsync on a repo and returns the expected and
        /// actual result of a follow-up verification query using a 
        /// Linq Where clause.</para>
        /// <para>----------------------------------</para>
        /// <para>REQUIRED TESTJSON FILES</para>
        /// <para>----------------------------------</para>
        /// <list type="table">
        /// <item>
        ///     <term>Id</term><description>Primary key</description>
        /// </item>
        /// <item>
        ///     <term>Input</term><description>JSON input object</description>
        /// </item>
        /// <item>
        ///     <term>Expected</term><description>Expected result (required)</description>
        /// </item>
        /// <item>
        ///     <term>Exception</term><description>"ArgumentException", "MissingEntityException" or null (optional)</description>
        /// </item>
        /// <item>
        ///     <term>LinqWhere</term><description>Dynamic Linq expression for follow-up GET request</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="t">TestScenario(TestCase)</param>
        /// <param name="jsonTestCase">Test input parameters and expected results</param>
        /// <returns>An object holding expected and actual results</returns>
        public async Task<ExpectedActual<RepoTestResult<List<TEntity>>>> UpdateAsync_ExpectedActual(
            string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var idStr = jsonTestCase.GetObject<string>("Id", Output);
            var input = jsonTestCase.GetObject<TEntity>("Input", Output);
            var linqWhere = jsonTestCase.GetObject<string>("LinqWhere", Output);
            var expected = jsonTestCase.GetObject<List<TEntity>>("Expected");
            var exception = jsonTestCase.GetObject<string>("Exception", Output);


            var eaResult = new ExpectedActual<RepoTestResult<List<TEntity>>> {
                Expected = new RepoTestResult<List<TEntity>> { Data = expected, Exception = exception },
                Actual = new RepoTestResult<List<TEntity>> { }
            };

            try {
                var id = Repo<TEntity, TContext>.ParseId(idStr);
                await Repo.UpdateAsync(input, id);
            } catch (Exception ex) {
                eaResult.Actual.Exception = ex.GetType().Name;
                Output.WriteLine($"EXCEPTION ({ex.GetType().Name}): {ex.Message}");
                Output.WriteLine($"STACK TRACE:\n {ex.StackTrace}");
            }

            if (eaResult.Expected.Data != null)
                eaResult.Actual.Data = Repo.GetWithDynamicLinq(where: linqWhere).Data;

            return eaResult;
        }



        /// <summary>
        /// <para>Invokes Patch on a repo and returns the expected and
        /// actual result of a follow-up verification query using a 
        /// Linq Where clause.</para>
        /// <para>----------------------------------</para>
        /// <para>REQUIRED TESTJSON FILES</para>
        /// <para>----------------------------------</para>
        /// <list type="table">
        /// <item>
        ///     <term>Id</term><description>Primary key</description>
        /// </item>
        /// <item>
        ///     <term>Input</term><description>JSON input object</description>
        /// </item>
        /// <item>
        ///     <term>Expected</term><description>Expected result (required)</description>
        /// </item>
        /// <item>
        ///     <term>Exception</term><description>Expected exception name (optional)</description>
        /// </item>
        /// <item>
        ///     <term>LinqWhere</term><description>Dynamic Linq expression for follow-up GET request</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="t">TestScenario(TestCase)</param>
        /// <param name="jsonTestCase">Test input parameters and expected results</param>
        /// <returns>An object holding expected and actual results</returns>
        public ExpectedActual<List<TEntity>> Patch_ExpectedActual(
            string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<string>("Id", Output);
            var input = jsonTestCase.GetObject<dynamic, TEntity>("Input", Output);
            var linqWhere = jsonTestCase.GetObject<string>("LinqWhere", Output);
            var expected = jsonTestCase.GetObject<List<TEntity>>("Expected");
            var exception = jsonTestCase.GetObject<string>("Exception", Output);

            var eaResult = new ExpectedActual<List<TEntity>> {
                Expected = expected,
                Actual = null
            };

            if (!string.IsNullOrEmpty(exception)) {
                if (exception.Equals("MissingEntityException", StringComparison.OrdinalIgnoreCase))
                    Assert.Throws<MissingEntityException>(() => { Repo.Patch(input, id); });
                else if (exception.Equals("ArgumentException", StringComparison.OrdinalIgnoreCase))
                    Assert.Throws<ArgumentException>(() => { Repo.Patch(input, id); });
            } else {
                Repo.Patch(input, id);
                eaResult.Actual = Repo.GetWithDynamicLinq(where: linqWhere).Data;
            }

            return eaResult;
        }



        /// <summary>
        /// <para>Invokes PatchAsync on a repo and returns the expected and
        /// actual result of a follow-up verification query using a 
        /// Linq Where clause.</para>
        /// <para>----------------------------------</para>
        /// <para>REQUIRED TESTJSON FILES</para>
        /// <para>----------------------------------</para>
        /// <list type="table">
        /// <item>
        ///     <term>Id</term><description>Primary key</description>
        /// </item>
        /// <item>
        ///     <term>Input</term><description>JSON input object</description>
        /// </item>
        /// <item>
        ///     <term>Expected</term><description>Expected result (required)</description>
        /// </item>
        /// <item>
        ///     <term>Exception</term><description>"ArgumentException", "MissingEntityException" or null (optional)</description>
        /// </item>
        /// <item>
        ///     <term>LinqWhere</term><description>Dynamic Linq expression for follow-up GET request</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="t">TestScenario(TestCase)</param>
        /// <param name="jsonTestCase">Test input parameters and expected results</param>
        /// <returns>An object holding expected and actual results</returns>
        public async Task<ExpectedActual<List<TEntity>>> PatchAsync_ExpectedActual(
            string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<string>("Id", Output);
            var input = jsonTestCase.GetObject<dynamic, TEntity>("Input", Output);
            var linqWhere = jsonTestCase.GetObject<string>("LinqWhere", Output);
            var expected = jsonTestCase.GetObject<List<TEntity>>("Expected");
            var exception = jsonTestCase.GetObject<string>("Exception", Output);

            var eaResult = new ExpectedActual<List<TEntity>> {
                Expected = expected,
                Actual = null
            };

            if (!string.IsNullOrEmpty(exception)) {
                if (exception.Equals("MissingEntityException", StringComparison.OrdinalIgnoreCase))
                    await Assert.ThrowsAsync<MissingEntityException>(async () => { await Repo.PatchAsync(input, id); });
                else if (exception.Equals("ArgumentException", StringComparison.OrdinalIgnoreCase))
                    await Assert.ThrowsAsync<ArgumentException>(async () => { await Repo.PatchAsync(input, id); });
            } else {
                await Repo.PatchAsync(input, id);
                eaResult.Actual = Repo.GetWithDynamicLinq(where: linqWhere).Data;
            }

            return eaResult;
        }




        /// <summary>
        /// <para>Returns the expected and actual result of a call to
        /// GetWithDynamicLinq.</para>
        /// <para>----------------------------------</para>
        /// <para>REQUIRED/OPTIONAL TESTJSON FILES</para>
        /// <para>----------------------------------</para>
        /// <list type="table">
        /// <item>
        ///     <term>Select</term><description>Dynamic Linq Select expression (optional)</description>
        /// </item>
        /// <item>
        ///     <term>Where</term><description>Dynamic Linq Where expression(optional)</description>
        /// </item>
        /// <item>
        ///     <term>OrderBy</term><description>Dynamic Linq OrderBy expression (optional)</description>
        /// </item>
        /// <item>
        ///     <term>Skip</term><description>Number of records to skip (optional)</description>
        /// </item>
        /// <item>
        ///     <term>Take</term><description>Page size (optional)</description>
        /// </item>
        /// <item>
        ///     <term>Expected</term><description>Expected result (required)</description>
        /// </item>
        /// <item>
        ///     <term>Exception</term><description>ParseException or null (optional)</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="t">TestScenario(TestCase)</param>
        /// <param name="jsonTestCase">Test input parameters and expected results</param>
        /// <returns>An object holding expected and actual results</returns>
        public ExpectedActual<DynamicLinqResult<dynamic>> GetWithDynamicLinq_ExpectedActual(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var select = jsonTestCase.GetObjectOrDefault<string>("Select", Output);
            var where = jsonTestCase.GetObjectOrDefault<string>("Where", Output);
            var orderBy = jsonTestCase.GetObjectOrDefault<string>("OrderBy", Output);
            var skip = jsonTestCase.GetObjectOrDefault<int?>("Skip", Output);
            var take = jsonTestCase.GetObjectOrDefault<int?>("Take", Output);
            var expected = jsonTestCase.GetObject<DynamicLinqResult<dynamic>, TEntity>("Expected");
            var exception = jsonTestCase.GetObject<string>("Exception", Output);

            var eaResult = new ExpectedActual<DynamicLinqResult<dynamic>> {
                Expected = expected,
                Actual = null
            };

            if (!string.IsNullOrEmpty(exception)) {
                if (exception.Equals("ParseException", StringComparison.OrdinalIgnoreCase))
                    Assert.Throws<ParseException>(() => { Repo.GetWithDynamicLinq(select, where, orderBy, skip, take); });
            } else {
                eaResult.Actual = Repo.GetWithDynamicLinq(select, where, orderBy, skip, take);
            }

            return eaResult;
        }



        /// <summary>
        /// <para>Returns the expected and actual result of a call to
        /// GetWithDynamicLinqAsync.</para>
        /// <para>----------------------------------</para>
        /// <para>REQUIRED/OPTIONAL TESTJSON FILES</para>
        /// <para>----------------------------------</para>
        /// <list type="table">
        /// <item>
        ///     <term>Select</term><description>Dynamic Linq Select expression (optional)</description>
        /// </item>
        /// <item>
        ///     <term>Where</term><description>Dynamic Linq Where expression(optional)</description>
        /// </item>
        /// <item>
        ///     <term>OrderBy</term><description>Dynamic Linq OrderBy expression (optional)</description>
        /// </item>
        /// <item>
        ///     <term>Skip</term><description>Number of records to skip (optional)</description>
        /// </item>
        /// <item>
        ///     <term>Take</term><description>Page size (optional)</description>
        /// </item>
        /// <item>
        ///     <term>Expected</term><description>Expected result (required)</description>
        /// </item>
        /// <item>
        ///     <term>Exception</term><description>"ArgumentException" or null (optional)</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="t">TestScenario(TestCase)</param>
        /// <param name="jsonTestCase">Test input parameters and expected results</param>
        /// <returns>An object holding expected and actual results</returns>
        public async Task<ExpectedActual<DynamicLinqResult<dynamic>>> GetWithDynamicLinqAsync_ExpectedActual(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var select = jsonTestCase.GetObjectOrDefault<string>("Select", Output);
            var where = jsonTestCase.GetObjectOrDefault<string>("Where", Output);
            var orderBy = jsonTestCase.GetObjectOrDefault<string>("OrderBy", Output);
            var skip = jsonTestCase.GetObjectOrDefault<int?>("Skip", Output);
            var take = jsonTestCase.GetObjectOrDefault<int?>("Take", Output);
            var expected = jsonTestCase.GetObject<DynamicLinqResult<dynamic>, TEntity>("Expected");
            var exception = jsonTestCase.GetObject<string>("Exception", Output);

            var eaResult = new ExpectedActual<DynamicLinqResult<dynamic>> {
                Expected = expected,
                Actual = null
            };

            if (!string.IsNullOrEmpty(exception)) {
                if (exception.Equals("ParseException", StringComparison.OrdinalIgnoreCase))
                    await Assert.ThrowsAsync<ParseException>(async () => { await Repo.GetWithDynamicLinqAsync(select, where, orderBy, skip, take); });
            } else {
                eaResult.Actual = await Repo.GetWithDynamicLinqAsync(select, where, orderBy, skip, take);
            }


            return eaResult;
        }



        /// <summary>
        /// <para>Returns the expected and actual result of a call to
        /// GetJsonObjectFromStoredProcedure.</para>
        /// <para>----------------------------------</para>
        /// <para>REQUIRED/OPTIONAL TESTJSON FILES</para>
        /// <para>----------------------------------</para>
        /// <list type="table">
        /// <item>
        ///     <term>SpName</term><description>The name of the stored procedure</description>
        /// </item>
        /// <item>
        ///     <term>Params</term><description>A JSON object holding the stored procedure parameters</description>
        /// </item>
        /// </list>
        /// <item>
        ///     <term>Expected</term><description>Expected result (required)</description>
        /// </item>
        /// <item>
        ///     <term>Exception</term><description>"SqlException" or null (optional)</description>
        /// </item>
        /// </summary>
        /// <param name="t">TestScenario(TestCase)</param>
        /// <param name="jsonTestCase">Test input parameters and expected results</param>
        /// <returns>An object holding expected and actual results</returns>
        public ExpectedActual<dynamic> GetJsonObjectFromStoredProcedure_ExpectedActual(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine(t);

            var spName = jsonTestCase.GetObject<string>("SpName", Output);
            var parameters = jsonTestCase.GetObject<Dictionary<string, object>>("Params", Output);
            var expected = jsonTestCase.GetObject<dynamic, TEntity>("Expected");
            var exception = jsonTestCase.GetObject<string>("Exception", Output);

            var eaResult = new ExpectedActual<dynamic> {
                Expected = expected,
                Actual = null
            };

            if (!string.IsNullOrEmpty(exception)) {
                if (exception.Equals("SqlException", StringComparison.OrdinalIgnoreCase))
                    Assert.Throws<SqlException>(() => { Repo.Context.GetJsonObjectFromStoredProcedure(spName, parameters); });
            } else {
                var actualJson = Repo.Context.GetJsonObjectFromStoredProcedure(spName, parameters);
                eaResult.Actual = JsonSerializer.Deserialize<dynamic>(actualJson, DynamicJsonSerializerOptions);
            }

            return eaResult;
        }



        /// <summary>
        /// <para>Returns the expected and actual result of a call to
        /// GetJsonObjectFromStoredProcedureAsync.</para>
        /// <para>----------------------------------</para>
        /// <para>REQUIRED/OPTIONAL TESTJSON FILES</para>
        /// <para>----------------------------------</para>
        /// <list type="table">
        /// <item>
        ///     <term>SpName</term><description>The name of the stored procedure -- also the relative path of the action method</description>
        /// </item>
        /// <item>
        ///     <term>Params</term><description>A JSON object holding the stored procedure parameters</description>
        /// </item>
        /// <item>
        ///     <term>Expected</term><description>Expected result (required)</description>
        /// </item>
        /// <item>
        ///     <term>Exception</term><description>"SqlException" or null (optional)</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="t">TestScenario(TestCase)</param>
        /// <param name="jsonTestCase">Test input parameters and expected results</param>
        /// <returns>An object holding expected and actual results</returns>
        public async Task<ExpectedActual<dynamic>> GetJsonObjectFromStoredProcedureAsync_ExpectedActual(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine(t);

            var spName = jsonTestCase.GetObject<string>("SpName", Output);
            var parameters = jsonTestCase.GetObject<Dictionary<string, object>>("Params", Output);
            var expected = jsonTestCase.GetObject<dynamic, TEntity>("Expected");
            var exception = jsonTestCase.GetObject<string>("Exception", Output);

            var eaResult = new ExpectedActual<dynamic> {
                Expected = expected,
                Actual = null
            };

            if (!string.IsNullOrEmpty(exception)) {
                if (exception.Equals("SqlException", StringComparison.OrdinalIgnoreCase))
                    await Assert.ThrowsAsync<SqlException>(async () => { await Repo.Context.GetJsonObjectFromStoredProcedureAsync(spName, parameters); });
            } else {
                var actualJson = await Repo.Context.GetJsonObjectFromStoredProcedureAsync(spName, parameters);
                eaResult.Actual = JsonSerializer.Deserialize<dynamic>(actualJson, DynamicJsonSerializerOptions);
            }

            return eaResult;
        }




        /// <summary>
        /// <para>Returns the expected and actual result of a call to
        /// GetJsonArrayFromStoredProcedure.</para>
        /// <para>----------------------------------</para>
        /// <para>REQUIRED/OPTIONAL TESTJSON FILES</para>
        /// <para>----------------------------------</para>
        /// <list type="table">
        /// <item>
        ///     <term>SpName</term><description>The name of the stored procedure</description>
        /// </item>
        /// <item>
        ///     <term>Params</term><description>A JSON object holding the stored procedure parameters</description>
        /// </item>
        /// <item>
        ///     <term>Expected</term><description>Expected result (required)</description>
        /// </item>
        /// <item>
        ///     <term>Exception</term><description>"SqlException" or null (optional)</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="t">TestScenario(TestCase)</param>
        /// <param name="jsonTestCase">Test input parameters and expected results</param>
        /// <returns>An object holding expected and actual results</returns>
        public ExpectedActual<List<dynamic>> GetJsonArrayFromStoredProcedure_ExpectedActual(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine(t);

            var spName = jsonTestCase.GetObject<string>("SpName", Output);
            var parameters = jsonTestCase.GetObject<Dictionary<string, object>>("Params", Output);
            var expected = jsonTestCase.GetObject<List<dynamic>, TEntity>("Expected");
            var exception = jsonTestCase.GetObject<string>("Exception", Output);

            var eaResult = new ExpectedActual<List<dynamic>> {
                Expected = expected,
                Actual = null
            };

            if (!string.IsNullOrEmpty(exception)) {
                if (exception.Equals("SqlException", StringComparison.OrdinalIgnoreCase))
                    Assert.Throws<SqlException>(() => { Repo.Context.GetJsonArrayFromStoredProcedure(spName, parameters); });
            } else {
                var actualJson = Repo.Context.GetJsonArrayFromStoredProcedure(spName, parameters);
                eaResult.Actual = JsonSerializer.Deserialize<List<dynamic>>(actualJson, DynamicJsonSerializerOptions);
            }

            return eaResult;
        }



        /// <summary>
        /// <para>Returns the expected and actual result of a call to
        /// GetJsonArrayFromStoredProcedureAsync.</para>
        /// <para>----------------------------------</para>
        /// <para>REQUIRED/OPTIONAL TESTJSON FILES</para>
        /// <para>----------------------------------</para>
        /// <list type="table">
        /// <item>
        ///     <term>SpName</term><description>The name of the stored procedure</description>
        /// </item>
        /// <item>
        ///     <term>Params</term><description>A JSON object holding the stored procedure parameters</description>
        /// </item>
        /// <item>
        ///     <term>Expected</term><description>Expected result (required)</description>
        /// </item>
        /// <item>
        ///     <term>Exception</term><description>"SqlException" or null (optional)</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="t">TestScenario(TestCase)</param>
        /// <param name="jsonTestCase">Test input parameters and expected results</param>
        /// <returns>An object holding expected and actual results</returns>
        public async Task<ExpectedActual<List<dynamic>>> GetJsonArrayFromStoredProcedureAsync_ExpectedActual(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine(t);

            var spName = jsonTestCase.GetObject<string>("SpName", Output);
            var parameters = jsonTestCase.GetObject<Dictionary<string, object>>("Params", Output);
            var expected = jsonTestCase.GetObject<List<dynamic>, TEntity>("Expected");
            var exception = jsonTestCase.GetObject<string>("Exception", Output);

            var eaResult = new ExpectedActual<List<dynamic>> {
                Expected = expected,
                Actual = null
            };

            if (!string.IsNullOrEmpty(exception)) {
                if (exception.Equals("SqlException", StringComparison.OrdinalIgnoreCase))
                    await Assert.ThrowsAsync<SqlException>(async () => { await Repo.Context.GetJsonArrayFromStoredProcedureAsync(spName, parameters); });
            } else {
                var actualJson = await Repo.Context.GetJsonArrayFromStoredProcedureAsync(spName, parameters);
                eaResult.Actual = JsonSerializer.Deserialize<List<dynamic>>(actualJson, DynamicJsonSerializerOptions);
            }

            return eaResult;
        }



        /// <summary>
        /// <para>Returns the expected and actual result of a call to
        /// GetScalarFromStoredProcedure.</para>
        /// <para>----------------------------------</para>
        /// <para>REQUIRED/OPTIONAL TESTJSON FILES</para>
        /// <para>----------------------------------</para>
        /// <list type="table">
        /// <item>
        ///     <term>SpName</term><description>The name of the stored procedure</description>
        /// </item>
        /// <item>
        ///     <term>Params</term><description>A JSON object holding the stored procedure parameters</description>
        /// </item>
        /// <item>
        ///     <term>Expected</term><description>Expected scalar value (required)</description>
        /// </item>
        /// <item>
        ///     <term>Exception</term><description>"SqlException" or null (optional)</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="t">TestScenario(TestCase)</param>
        /// <param name="jsonTestCase">Test input parameters and expected results</param>
        /// <returns>An object holding expected and actual results</returns>
        public ExpectedActual<TScalarType> GetScalarFromStoredProcedure_ExpectedActual<TScalarType>(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine(t);

            var spName = jsonTestCase.GetObject<string>("SpName", Output);
            var parameters = jsonTestCase.GetObject<Dictionary<string, object>>("Params", Output);
            var expected = jsonTestCase.GetObject<TScalarType>("Expected");
            var exception = jsonTestCase.GetObject<string>("Exception", Output);

            var eaResult = new ExpectedActual<TScalarType> {
                Expected = expected,
                Actual = default
            };

            if (!string.IsNullOrEmpty(exception)) {
                if (exception.Equals("SqlException", StringComparison.OrdinalIgnoreCase))
                    Assert.Throws<SqlException>(() => { Repo.Context.GetScalarFromStoredProcedure<TContext, TScalarType>(spName, parameters); });
            } else {
                eaResult.Actual = Repo.Context.GetScalarFromStoredProcedure<TContext, TScalarType>(spName, parameters);
            }

            return eaResult;
        }



        /// <summary>
        /// <para>Returns the expected and actual result of a call to
        /// GetScalarFromStoredProcedureAsync.</para>
        /// <para>----------------------------------</para>
        /// <para>REQUIRED/OPTIONAL TESTJSON FILES</para>
        /// <para>----------------------------------</para>
        /// <list type="table">
        /// <item>
        ///     <term>SpName</term><description>The name of the stored procedure</description>
        /// </item>
        /// <item>
        ///     <term>Params</term><description>A JSON object holding the stored procedure parameters</description>
        /// </item>
        /// <item>
        ///     <term>Expected</term><description>Expected scalar value (required)</description>
        /// </item>
        /// <item>
        ///     <term>Exception</term><description>"SqlException" or null (optional)</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="t">TestScenario(TestCase)</param>
        /// <param name="jsonTestCase">Test input parameters and expected results</param>
        /// <returns>An object holding expected and actual results</returns>
        public async Task<ExpectedActual<TScalarType>> GetScalarFromStoredProcedureAsync_ExpectedActual<TScalarType>(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine(t);

            var spName = jsonTestCase.GetObject<string>("SpName", Output);
            var parameters = jsonTestCase.GetObject<Dictionary<string, object>>("Params", Output);
            var expected = jsonTestCase.GetObject<TScalarType>("Expected");
            var exception = jsonTestCase.GetObject<string>("Exception", Output);

            var eaResult = new ExpectedActual<TScalarType> {
                Expected = expected,
                Actual = default
            };

            if (!string.IsNullOrEmpty(exception)) {
                if (exception.Equals("SqlException", StringComparison.OrdinalIgnoreCase))
                    await Assert.ThrowsAsync<SqlException>(async () => { await Repo.Context.GetScalarFromStoredProcedureAsync<TContext, TScalarType>(spName, parameters); });
            } else {
                eaResult.Actual = await Repo.Context.GetScalarFromStoredProcedureAsync<TContext, TScalarType>(spName, parameters);
            }

            return eaResult;
        }



        public void Dispose() {
            Factory.ResetRepo();
        }
    }
}

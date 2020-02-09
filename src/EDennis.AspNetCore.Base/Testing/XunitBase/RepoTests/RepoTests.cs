using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Serialization;
using EDennis.NetCoreTestingUtilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {

    public abstract class RepoTests<TRepo, TEntity, TContext>
            : IDisposable
        where TEntity : class, IHasSysUser, new()
        where TContext : DbContext, ISqlServerDbContext<TContext>
        where TRepo : Repo<TEntity, TContext> {

        public virtual string ProjectName { get; set; } = typeof(TRepo).Assembly.GetName().Name;

        protected ITestOutputHelper Output { get; }
        protected TRepo Repo { get; }
        protected TestRepoFactory<TRepo, TEntity, TContext> Factory { get; }

        public RepoTests(ITestOutputHelper output) {
            Output = output;
            Factory = new TestRepoFactory<TRepo, TEntity, TContext>(ProjectName);
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
        ///     <term>Id</term><description>Primary key (required)</description>
        /// </item>
        /// <item>
        ///     <term>Expected</term><description>Expected result (required)</description>
        /// </item>
        /// <item>
        ///     <term>ThrowsException</term><description>1 or 0 (required)</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="t">TestScenario(TestCase)</param>
        /// <param name="jsonTestCase">Test input parameters and expected results</param>
        /// <returns>An object holding expected and actual results</returns>
        public ExpectedActual<RepoTestResult<TEntity>> Get_ExpectedActual(
            string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var idStr = jsonTestCase.GetObject<string>("Id", Output);
            var expected = jsonTestCase.GetObjectOrDefault<TEntity>("Expected");
            var exception = jsonTestCase.GetObjectOrDefault<string>("Exception", Output);

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
        ///     <term>Id</term><description>Primary key (required)</description>
        /// </item>
        /// <item>
        ///     <term>Expected</term><description>Expected result (required)</description>
        /// </item>
        /// <item>
        ///     <term>ThrowsException</term><description>1 or 0 (required)</description>
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
            var expected = jsonTestCase.GetObjectOrDefault<TEntity>("Expected");
            var exception = jsonTestCase.GetObjectOrDefault<string>("Exception", Output);

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
        ///     <term>Id</term><description>Primary key (required)</description>
        /// </item>
        /// <item>
        ///     <term>Expected</term><description>Expected result (required)</description>
        /// </item>
        /// <item>
        ///     <term>ThrowsException</term><description>1 or 0 (required)</description>
        /// </item>
        /// <item>
        ///     <term>LinqWhere</term><description>Dynamic Linq expression for follow-up GET request (required)</description>
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
            var exception = jsonTestCase.GetObjectOrDefault<string>("Exception", Output);

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
        ///     <term>Id</term><description>Primary key (required)</description>
        /// </item>
        /// <item>
        ///     <term>Expected</term><description>Expected result (required)</description>
        /// </item>
        /// <item>
        ///     <term>ThrowsException</term><description>1 or 0 (required)</description>
        /// </item>
        /// <item>
        ///     <term>LinqWhere</term><description>Dynamic Linq expression for follow-up GET request (required)</description>
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
            var expected = jsonTestCase.GetObjectOrDefault<List<TEntity>>("Expected");
            var exception = jsonTestCase.GetObjectOrDefault<string>("Exception", Output);

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
        ///     <term>Input</term><description>JSON input object (required)</description>
        /// </item>
        /// <item>
        ///     <term>Expected</term><description>Expected result (required)</description>
        /// </item>
        /// <item>
        ///     <term>ThrowsException</term><description>1 or 0 (required)</description>
        /// </item>
        /// <item>
        ///     <term>LinqWhere</term><description>Dynamic Linq expression for follow-up GET request (required)</description>
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
            var expected = jsonTestCase.GetObjectOrDefault<List<TEntity>>("Expected");
            var exception = jsonTestCase.GetObjectOrDefault<string>("Exception", Output);

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
        ///     <term>Input</term><description>JSON input object (required)</description>
        /// </item>
        /// <item>
        ///     <term>Expected</term><description>Expected result (required)</description>
        /// </item>
        /// <item>
        ///     <term>ThrowsException</term><description>1 or 0 (required)</description>
        /// </item>
        /// <item>
        ///     <term>LinqWhere</term><description>Dynamic Linq expression for follow-up GET request (required)</description>
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
            var expected = jsonTestCase.GetObjectOrDefault<List<TEntity>>("Expected");
            var exception = jsonTestCase.GetObjectOrDefault<string>("Exception", Output);

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
        ///     <term>Id</term><description>Primary key (required)</description>
        /// </item>
        /// <item>
        ///     <term>Input</term><description>JSON input object (required)</description>
        /// </item>
        /// <item>
        ///     <term>Expected</term><description>Expected result (required)</description>
        /// </item>
        /// <item>
        ///     <term>ThrowsException</term><description>1 or 0 (required)</description>
        /// </item>
        /// <item>
        ///     <term>LinqWhere</term><description>Dynamic Linq expression for follow-up GET request (required)</description>
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
            var expected = jsonTestCase.GetObjectOrDefault<List<TEntity>>("Expected");
            var exception = jsonTestCase.GetObjectOrDefault<string>("Exception", Output);


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
        ///     <term>Id</term><description>Primary key (required)</description>
        /// </item>
        /// <item>
        ///     <term>Input</term><description>JSON input object (required)</description>
        /// </item>
        /// <item>
        ///     <term>Expected</term><description>Expected result (required)</description>
        /// </item>
        /// <item>
        ///     <term>ThrowsException</term><description>1 or 0 (required)</description>
        /// </item>
        /// <item>
        ///     <term>LinqWhere</term><description>Dynamic Linq expression for follow-up GET request (required)</description>
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
            var expected = jsonTestCase.GetObjectOrDefault<List<TEntity>>("Expected");
            var exception = jsonTestCase.GetObjectOrDefault<string>("Exception", Output);


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
        ///     <term>Id</term><description>Primary key (required)</description>
        /// </item>
        /// <item>
        ///     <term>Input</term><description>JSON input object (required)</description>
        /// </item>
        /// <item>
        ///     <term>Expected</term><description>Expected result (required)</description>
        /// </item>
        /// <item>
        ///     <term>ThrowsException</term><description>1 or 0 (required)</description>
        /// </item>
        /// <item>
        ///     <term>LinqWhere</term><description>Dynamic Linq expression for follow-up GET request (required)</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="t">TestScenario(TestCase)</param>
        /// <param name="jsonTestCase">Test input parameters and expected results</param>
        /// <returns>An object holding expected and actual results</returns>
        public ExpectedActual<RepoTestResult<List<TEntity>>> Patch_ExpectedActual(
            string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var idStr = jsonTestCase.GetObject<string>("Id", Output);
            var input = jsonTestCase.GetObject<dynamic, TEntity>("Input", Output); //test bad input
            var linqWhere = jsonTestCase.GetObject<string>("LinqWhere", Output);
            var expected = jsonTestCase.GetObjectOrDefault<List<TEntity>>("Expected");
            var exception = jsonTestCase.GetObjectOrDefault<string>("Exception", Output);


            var eaResult = new ExpectedActual<RepoTestResult<List<TEntity>>> {
                Expected = new RepoTestResult<List<TEntity>> { Data = expected, Exception = exception },
                Actual = new RepoTestResult<List<TEntity>> { }
            };

            try {
                var id = Repo<TEntity, TContext>.ParseId(idStr);
                Repo.Patch(input, id);
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
        /// <para>Invokes PatchAsync on a repo and returns the expected and
        /// actual result of a follow-up verification query using a 
        /// Linq Where clause.</para>
        /// <para>----------------------------------</para>
        /// <para>REQUIRED TESTJSON FILES</para>
        /// <para>----------------------------------</para>
        /// <list type="table">
        /// <item>
        ///     <term>Id</term><description>Primary key (required)</description>
        /// </item>
        /// <item>
        ///     <term>Input</term><description>JSON input object (required)</description>
        /// </item>
        /// <item>
        ///     <term>Expected</term><description>Expected result (required)</description>
        /// </item>
        /// <item>
        ///     <term>ThrowsException</term><description>1 or 0 (required)</description>
        /// </item>
        /// <item>
        ///     <term>LinqWhere</term><description>Dynamic Linq expression for follow-up GET request (required)</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="t">TestScenario(TestCase)</param>
        /// <param name="jsonTestCase">Test input parameters and expected results</param>
        /// <returns>An object holding expected and actual results</returns>
        public async Task<ExpectedActual<RepoTestResult<List<TEntity>>>> PatchAsync_ExpectedActual(
            string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var idStr = jsonTestCase.GetObject<string>("Id", Output);
            var linqWhere = jsonTestCase.GetObject<string>("LinqWhere", Output);
            var expected = jsonTestCase.GetObjectOrDefault<List<TEntity>>("Expected");
            var exception = jsonTestCase.GetObjectOrDefault<string>("Exception", Output);

            var eaResult = new ExpectedActual<RepoTestResult<List<TEntity>>> {
                Expected = new RepoTestResult<List<TEntity>> { Data = expected, Exception = exception },
                Actual = new RepoTestResult<List<TEntity>> { }
            };

            try {
                var input = jsonTestCase.GetObject<dynamic, TEntity>("Input", Output); //test bad input
                var id = Repo<TEntity, TContext>.ParseId(idStr);
                await Repo.PatchAsync(input, id);
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
        ///     <term>ThrowsException</term><description>1 or 0 (required)</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="t">TestScenario(TestCase)</param>
        /// <param name="jsonTestCase">Test input parameters and expected results</param>
        /// <returns>An object holding expected and actual results</returns>
        public ExpectedActual<RepoTestResult<DynamicLinqResult<dynamic>>> GetWithDynamicLinq_ExpectedActual(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var select = jsonTestCase.GetObjectOrDefault<string>("Select", Output);
            var where = jsonTestCase.GetObjectOrDefault<string>("Where", Output);
            var orderBy = jsonTestCase.GetObjectOrDefault<string>("OrderBy", Output);
            var skip = jsonTestCase.GetObjectOrDefault<int?>("Skip", Output);
            var take = jsonTestCase.GetObjectOrDefault<int?>("Take", Output);
            var expected = jsonTestCase.GetObject<DynamicLinqResult<dynamic>, TEntity>("Expected");
            var exception = jsonTestCase.GetObjectOrDefault<string>("Exception", Output);

            var eaResult = new ExpectedActual<RepoTestResult<DynamicLinqResult<dynamic>>> {
                Expected = new RepoTestResult<DynamicLinqResult<dynamic>> { Data = expected, Exception = exception },
                Actual = new RepoTestResult<DynamicLinqResult<dynamic>> { }
            };

            try {
                eaResult.Actual.Data = Repo.GetWithDynamicLinq(select, where, orderBy, skip, take);
            } catch (Exception ex) {
                eaResult.Actual.Exception = ex.GetType().Name;
                Output.WriteLine($"EXCEPTION ({ex.GetType().Name}): {ex.Message}");
                Output.WriteLine($"STACK TRACE:\n {ex.StackTrace}");
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
        ///     <term>ThrowsException</term><description>1 or 0 (required)</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="t">TestScenario(TestCase)</param>
        /// <param name="jsonTestCase">Test input parameters and expected results</param>
        /// <returns>An object holding expected and actual results</returns>
        public async Task<ExpectedActual<RepoTestResult<DynamicLinqResult<dynamic>>>> GetWithDynamicLinqAsync_ExpectedActual(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var select = jsonTestCase.GetObjectOrDefault<string>("Select", Output);
            var where = jsonTestCase.GetObjectOrDefault<string>("Where", Output);
            var orderBy = jsonTestCase.GetObjectOrDefault<string>("OrderBy", Output);
            var skip = jsonTestCase.GetObjectOrDefault<int?>("Skip", Output);
            var take = jsonTestCase.GetObjectOrDefault<int?>("Take", Output);
            var expected = jsonTestCase.GetObject<DynamicLinqResult<dynamic>, TEntity>("Expected");
            var exception = jsonTestCase.GetObjectOrDefault<string>("Exception", Output);

            var eaResult = new ExpectedActual<RepoTestResult<DynamicLinqResult<dynamic>>> {
                Expected = new RepoTestResult<DynamicLinqResult<dynamic>> { Data = expected, Exception = exception },
                Actual = new RepoTestResult<DynamicLinqResult<dynamic>> { }
            };

            try {
                eaResult.Actual.Data = await Repo.GetWithDynamicLinqAsync(select, where, orderBy, skip, take);
            } catch (Exception ex) {
                eaResult.Actual.Exception = ex.GetType().Name;
                Output.WriteLine($"EXCEPTION ({ex.GetType().Name}): {ex.Message}");
                Output.WriteLine($"STACK TRACE:\n {ex.StackTrace}");
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
        ///     <term>ThrowsException</term><description>1 or 0 (required)</description>
        /// </item>
        /// </summary>
        /// <param name="t">TestScenario(TestCase)</param>
        /// <param name="jsonTestCase">Test input parameters and expected results</param>
        /// <returns>An object holding expected and actual results</returns>
        public ExpectedActual<RepoTestResult<dynamic>> GetJsonObjectFromStoredProcedure_ExpectedActual(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine(t);

            var spName = jsonTestCase.GetObject<string>("SpName", Output);
            var parameters = jsonTestCase.GetObject<Dictionary<string, string>>("Params", Output);
            var objParms = parameters.ToDictionary(x => x.Key, x => (object)x.Value);
            var expected = jsonTestCase.GetObject<dynamic, TEntity>("Expected");
            var exception = jsonTestCase.GetObjectOrDefault<string>("Exception", Output);

            var eaResult = new ExpectedActual<RepoTestResult<dynamic>> {
                Expected = new RepoTestResult<dynamic> { Data = expected, Exception = exception },
                Actual = new RepoTestResult<dynamic> { }
            };

            try {
                var actualJson = Repo.Context.GetJsonObjectFromStoredProcedure(spName, objParms);
                eaResult.Actual.Data = JsonSerializer.Deserialize<dynamic>(actualJson, DynamicJsonSerializerOptions);
            } catch (Exception ex) {
                eaResult.Actual.Exception = ex.GetType().Name;
                Output.WriteLine($"EXCEPTION ({ex.GetType().Name}): {ex.Message}");
                Output.WriteLine($"STACK TRACE:\n {ex.StackTrace}");
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
        ///     <term>ThrowsException</term><description>1 or 0 (required)</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="t">TestScenario(TestCase)</param>
        /// <param name="jsonTestCase">Test input parameters and expected results</param>
        /// <returns>An object holding expected and actual results</returns>
        public async Task<ExpectedActual<RepoTestResult<dynamic>>> GetJsonObjectFromStoredProcedureAsync_ExpectedActual(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine(t);

            var spName = jsonTestCase.GetObject<string>("SpName", Output);
            var parameters = jsonTestCase.GetObject<Dictionary<string, string>>("Params", Output);
            var objParms = parameters.ToDictionary(x => x.Key, x => (object)x.Value);
            var expected = jsonTestCase.GetObject<dynamic, TEntity>("Expected");
            var exception = jsonTestCase.GetObjectOrDefault<string>("Exception", Output);

            var eaResult = new ExpectedActual<RepoTestResult<dynamic>> {
                Expected = new RepoTestResult<dynamic> { Data = expected, Exception = exception },
                Actual = new RepoTestResult<dynamic> { }
            };

            try {
                var actualJson = await Repo.Context.GetJsonObjectFromStoredProcedureAsync(spName, objParms);
                eaResult.Actual.Data = JsonSerializer.Deserialize<dynamic>(actualJson, DynamicJsonSerializerOptions);
            } catch (Exception ex) {
                eaResult.Actual.Exception = ex.GetType().Name;
                Output.WriteLine($"EXCEPTION ({ex.GetType().Name}): {ex.Message}");
                Output.WriteLine($"STACK TRACE:\n {ex.StackTrace}");
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
        ///     <term>ThrowsException</term><description>1 or 0 (required)</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="t">TestScenario(TestCase)</param>
        /// <param name="jsonTestCase">Test input parameters and expected results</param>
        /// <returns>An object holding expected and actual results</returns>
        public ExpectedActual<RepoTestResult<List<dynamic>>> GetJsonArrayFromStoredProcedure_ExpectedActual(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine(t);

            var spName = jsonTestCase.GetObject<string>("SpName", Output);
            var parameters = jsonTestCase.GetObject<Dictionary<string, string>>("Params", Output);
            var objParms = parameters.ToDictionary(x => x.Key, x => (object)x.Value);
            var expected = jsonTestCase.GetObject<List<dynamic>, TEntity>("Expected");
            var exception = jsonTestCase.GetObjectOrDefault<string>("Exception", Output);

            var eaResult = new ExpectedActual<RepoTestResult<List<dynamic>>> {
                Expected = new RepoTestResult<List<dynamic>> { Data = expected, Exception = exception },
                Actual = new RepoTestResult<List<dynamic>> { }
            };

            try {
                var actualJson = Repo.Context.GetJsonArrayFromStoredProcedure(spName, objParms);
                eaResult.Actual.Data = JsonSerializer.Deserialize<List<dynamic>>(actualJson, DynamicJsonSerializerOptions);
            } catch (Exception ex) {
                eaResult.Actual.Exception = ex.GetType().Name;
                Output.WriteLine($"EXCEPTION ({ex.GetType().Name}): {ex.Message}");
                Output.WriteLine($"STACK TRACE:\n {ex.StackTrace}");
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
        ///     <term>ThrowsException</term><description>1 or 0 (required)</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="t">TestScenario(TestCase)</param>
        /// <param name="jsonTestCase">Test input parameters and expected results</param>
        /// <returns>An object holding expected and actual results</returns>
        public async Task<ExpectedActual<RepoTestResult<List<dynamic>>>> GetJsonArrayFromStoredProcedureAsync_ExpectedActual(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine(t);

            var spName = jsonTestCase.GetObject<string>("SpName", Output);
            var parameters = jsonTestCase.GetObject<Dictionary<string, string>>("Params", Output);
            var objParms = parameters.ToDictionary(x => x.Key, x => (object)x.Value);
            var expected = jsonTestCase.GetObject<List<dynamic>, TEntity>("Expected");
            var exception = jsonTestCase.GetObjectOrDefault<string>("Exception", Output);

            var eaResult = new ExpectedActual<RepoTestResult<List<dynamic>>> {
                Expected = new RepoTestResult<List<dynamic>> { Data = expected, Exception = exception },
                Actual = new RepoTestResult<List<dynamic>> { }
            };

            try {
                var actualJson = await Repo.Context.GetJsonArrayFromStoredProcedureAsync(spName, objParms);
                eaResult.Actual.Data = JsonSerializer.Deserialize<List<dynamic>>(actualJson, DynamicJsonSerializerOptions);
            } catch (Exception ex) {
                eaResult.Actual.Exception = ex.GetType().Name;
                Output.WriteLine($"EXCEPTION ({ex.GetType().Name}): {ex.Message}");
                Output.WriteLine($"STACK TRACE:\n {ex.StackTrace}");
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
        ///     <term>ThrowsException</term><description>1 or 0 (required)</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="t">TestScenario(TestCase)</param>
        /// <param name="jsonTestCase">Test input parameters and expected results</param>
        /// <returns>An object holding expected and actual results</returns>
        public ExpectedActual<RepoTestResult<TScalarType>> GetScalarFromStoredProcedure_ExpectedActual<TScalarType>(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine(t);

            var spName = jsonTestCase.GetObject<string>("SpName", Output);
            var parameters = jsonTestCase.GetObject<Dictionary<string, string>>("Params", Output);
            var objParms = parameters.ToDictionary(x => x.Key, x => (object)x.Value);
            var expected = jsonTestCase.GetObjectOrDefault<TScalarType>("Expected");
            var exception = jsonTestCase.GetObjectOrDefault<string>("Exception", Output);

            var eaResult = new ExpectedActual<RepoTestResult<TScalarType>> {
                Expected = new RepoTestResult<TScalarType> { Data = expected, Exception = exception },
                Actual = new RepoTestResult<TScalarType> { }
            };

            try {
                eaResult.Actual.Data = Repo.Context.GetScalarFromStoredProcedure<TContext,TScalarType>(spName, objParms);
            } catch (Exception ex) {
                eaResult.Actual.Exception = ex.GetType().Name;
                Output.WriteLine($"EXCEPTION ({ex.GetType().Name}): {ex.Message}");
                Output.WriteLine($"STACK TRACE:\n {ex.StackTrace}");
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
        ///     <term>ThrowsException</term><description>1 or 0 (required)</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="t">TestScenario(TestCase)</param>
        /// <param name="jsonTestCase">Test input parameters and expected results</param>
        /// <returns>An object holding expected and actual results</returns>
        public async Task<ExpectedActual<RepoTestResult<TScalarType>>> GetScalarFromStoredProcedureAsync_ExpectedActual<TScalarType>(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine(t);

            var spName = jsonTestCase.GetObject<string>("SpName", Output);
            var parameters = jsonTestCase.GetObject<Dictionary<string, string>>("Params", Output);
            var objParms = parameters.ToDictionary(x => x.Key, x => (object)x.Value);
            var expected = jsonTestCase.GetObjectOrDefault<TScalarType>("Expected");
            var exception = jsonTestCase.GetObjectOrDefault<string>("Exception", Output);

            var eaResult = new ExpectedActual<RepoTestResult<TScalarType>> {
                Expected = new RepoTestResult<TScalarType> { Data = expected, Exception = exception },
                Actual = new RepoTestResult<TScalarType> { }
            };

            try {
                eaResult.Actual.Data = await Repo.Context.GetScalarFromStoredProcedureAsync<TContext, TScalarType>(spName, objParms);
            } catch (Exception ex) {
                eaResult.Actual.Exception = ex.GetType().Name;
                Output.WriteLine($"EXCEPTION ({ex.GetType().Name}): {ex.Message}");
                Output.WriteLine($"STACK TRACE:\n {ex.StackTrace}");
            }

            return eaResult;
        }



        public void Dispose() {
            Factory.ResetRepo();
        }
    }
}

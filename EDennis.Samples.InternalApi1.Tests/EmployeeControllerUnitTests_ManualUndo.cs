using EDennis.AspNetCore.Base.Testing;
using EDennis.Samples.InternalApi1.Controllers;
using EDennis.Samples.InternalApi1.Models;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.Samples.InternalApi1.Tests {


    /// <summary>
    /// SPECIAL NOTE: These tests have a special role -- 
    /// making sure that the read-only QueryTests don't
    /// initiate a rollback.  Only debug these tests in 
    /// isolation of the other tests.
    /// </summary>
    [TestCaseOrderer("EDennis.Samples.InternalApi1.Tests.PriorityOrderer", "TestOrderExamples")]
    public class EmployeeControllerUnitTests_ManualUndo : QueryTest<HrContext>, IDisposable {

        private EmployeeController _controller;
        private EmployeeRepo _repo;
        private readonly ITestOutputHelper _output;

        public EmployeeControllerUnitTests_ManualUndo(ITestOutputHelper output) {
            _repo = new EmployeeRepo(Context);
            _controller = new EmployeeController(_repo);
            _output = output;
        }

        //[Theory, TestPriority(1)]
        //[InlineData("Regis")]
        //[InlineData("Wink")]
        public void TestCreateEmployee(string firstName) {
            var response = _controller.CreateEmployee(new Employee { FirstName = firstName });
        }

        //[Theory, TestPriority(2)]
        //[InlineData(5)]
        //[InlineData(6)]
        public void TestDeleteEmployee(int id) {
            _repo.Delete(id);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    Context.Database.ExecuteSqlCommand("exec _maintenance.ResetIdentities");
                }
                disposedValue = true;
            }
        }

        public void Dispose() {
            Dispose(true);
        }
        #endregion

    }
}

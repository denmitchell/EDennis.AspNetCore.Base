using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using EDennis.Samples.InternalApi1.Models;
using EDennis.AspNetCore.Testing;
using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace EDennis.Samples.InternalApi1.Tests {


    /// <summary>
    /// SPECIAL NOTE: These tests have a special role -- 
    /// making sure that the read-only QueryTests don't
    /// initiate a rollback.  Only debug these tests in 
    /// isolation of the other tests.
    /// </summary>
    [TestCaseOrderer("EDennis.Samples.InternalApi1.Tests.PriorityOrderer", "TestOrderExamples")]
    public class EmployeeRepoUnitTests_ManualUndo : QueryTest<HrContext>, IDisposable {

        private EmployeeRepo _repo;
        private readonly ITestOutputHelper _output;

        public EmployeeRepoUnitTests_ManualUndo(ITestOutputHelper output) {
            _repo = new EmployeeRepo(Context);
            _output = output;
        }

        //[Theory, TestPriority(1)]
        //[InlineData("Regis")]
        //[InlineData("Wink")]
        public void TestCreateEmployee(string firstName) {
            _repo.Create(new Employee { FirstName = firstName });
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

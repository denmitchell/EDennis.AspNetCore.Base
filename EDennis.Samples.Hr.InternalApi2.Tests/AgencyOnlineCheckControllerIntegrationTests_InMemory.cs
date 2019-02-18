﻿using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Testing;
using EDennis.AspNetCore.Base.Web;
using EDennis.NetCoreTestingUtilities.Extensions;
using EDennis.Samples.Hr.InternalApi2.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Xunit;
using Xunit.Abstractions;


namespace EDennis.Samples.Hr.InternalApi2.Tests {

    public class AgencyOnlineCheckControllerIntegrationTests_InMemory : InMemoryIntegrationTests<Startup> {


        private readonly static string[] PROPS_FILTER = new string[] { "SysStart", "SysEnd" };
        private readonly static string[] PROPS_FILTER_WITH_ID = new string[] { "SysStart", "SysEnd", "Id" };

        private const string AGENCY_ONLINE_URL = "iapi/agencyonlinecheck";

        public AgencyOnlineCheckControllerIntegrationTests_InMemory(ITestOutputHelper output, InMemoryWebApplicationFactory<Startup> factory)
            : base(output, factory) { }


        [Theory]
        [InlineData(1, "2018-12-01", "Pass")]
        [InlineData(2, "2018-12-02", "Fail")]
        [InlineData(3, "2018-12-03", "Pass")]
        [InlineData(4, "2018-12-04", "Fail")]
        public void TestCreateAgencyOnlineCheck(int employeeId, string strDateCompleted, string status) {

            _output.WriteLine($"Instance Name:{_instanceName}");

            var input = new AgencyOnlineCheck {
                EmployeeId = employeeId,
                DateCompleted = DateTime.Parse(strDateCompleted),
                Status = status
            };

            _client.Post(AGENCY_ONLINE_URL, input);


            var allRecs = _client.Get<List<AgencyInvestigatorCheck>>(AGENCY_ONLINE_URL).Value;

            var targetRec = allRecs
                .OrderBy(e => e.Id)
                .LastOrDefault();

            Assert.True(input.IsEqualOrWrite(targetRec, PROPS_FILTER_WITH_ID, _output));

            Assert.Equal(employeeId, targetRec.EmployeeId);
            Assert.Equal(DateTime.Parse(strDateCompleted), targetRec.DateCompleted);
            Assert.Equal(status, targetRec.Status);


        }


    }
}
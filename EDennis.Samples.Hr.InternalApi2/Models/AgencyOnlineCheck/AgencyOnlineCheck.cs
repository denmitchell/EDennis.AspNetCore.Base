﻿using EDennis.AspNetCore.Base.EntityFramework;
using System;

namespace EDennis.Samples.Hr.InternalApi2.Models {
    public class AgencyOnlineCheck : IHasIntegerId, IHasSysUser {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime DateCompleted { get; set; }
        public string Status { get; set; }
        public string SysUser { get; set; }
    }
}

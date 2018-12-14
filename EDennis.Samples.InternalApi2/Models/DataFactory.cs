using System;
namespace EDennis.Samples.InternalApi2.Models {

    public static class AgencyInvestigatorCheckContextDataFactory {
        public static AgencyInvestigatorCheck[] AgencyInvestigatorCheckRecords { get; set; }
            = new AgencyInvestigatorCheck[] {
                new AgencyInvestigatorCheck {
                        Id = 1,
                        EmployeeId = 1,
                        DateCompleted = new DateTime(2018,1,1),
                        Status = "Pass"
                },
                new AgencyInvestigatorCheck {
                        Id = 2,
                        EmployeeId = 2,
                        DateCompleted = new DateTime(2018,2,2),
                        Status = "Pass"
                },
                new AgencyInvestigatorCheck {
                        Id = 3,
                        EmployeeId = 3,
                        DateCompleted = new DateTime(2018,3,3),
                        Status = "Fail"
                },
                new AgencyInvestigatorCheck {
                        Id = 4,
                        EmployeeId = 4,
                        DateCompleted = new DateTime(2018,4,4),
                        Status = "Pass"
                },
            };
    }

    public static class AgencyOnlineCheckContextDataFactory {
        public static AgencyOnlineCheck[] AgencyOnlineCheckRecords { get; set; }
            = new AgencyOnlineCheck[] {
                new AgencyOnlineCheck {
                        Id = 1,
                        EmployeeId = 1,
                        DateCompleted = new DateTime(2018,1,1),
                        Status = "Pass"
                },
                new AgencyOnlineCheck {
                        Id = 2,
                        EmployeeId = 2,
                        DateCompleted = new DateTime(2018,2,2),
                        Status = "Pass"
                },
                new AgencyOnlineCheck {
                        Id = 3,
                        EmployeeId = 3,
                        DateCompleted = new DateTime(2018,3,3),
                        Status = "Fail"
                },
                new AgencyOnlineCheck {
                        Id = 4,
                        EmployeeId = 4,
                        DateCompleted = new DateTime(2018,4,4),
                        Status = "Pass"
                },
            };
    }


    public static class FederalBackgroundCheckContextDataFactory {
        public static FederalBackgroundCheck[] FederalBackgroundCheckRecords { get; set; }
            = new FederalBackgroundCheck[] {
                new FederalBackgroundCheck {
                        Id = 1,
                        EmployeeId = 1,
                        DateCompleted = new DateTime(2018,1,1),
                        Status = "Pass"
                },
                new FederalBackgroundCheck {
                        Id = 2,
                        EmployeeId = 2,
                        DateCompleted = new DateTime(2018,2,2),
                        Status = "Pass"
                },
                new FederalBackgroundCheck {
                        Id = 3,
                        EmployeeId = 3,
                        DateCompleted = new DateTime(2018,3,3),
                        Status = "Fail"
                },
                new FederalBackgroundCheck {
                        Id = 4,
                        EmployeeId = 4,
                        DateCompleted = new DateTime(2018,4,4),
                        Status = "Pass"
                },
            };
    }


    public static class StateBackgroundCheckContextDataFactory {
        public static StateBackgroundCheck[] StateBackgroundCheckRecords { get; set; }
            = new StateBackgroundCheck[] {
                new StateBackgroundCheck {
                        Id = 1,
                        EmployeeId = 1,
                        DateCompleted = new DateTime(2018,1,1),
                        Status = "Pass"
                },
                new StateBackgroundCheck {
                        Id = 2,
                        EmployeeId = 2,
                        DateCompleted = new DateTime(2018,2,2),
                        Status = "Pass"
                },
                new StateBackgroundCheck {
                        Id = 3,
                        EmployeeId = 3,
                        DateCompleted = new DateTime(2018,3,3),
                        Status = "Fail"
                },
                new StateBackgroundCheck {
                        Id = 4,
                        EmployeeId = 4,
                        DateCompleted = new DateTime(2018,4,4),
                        Status = "Pass"
                },
            };
    }

}

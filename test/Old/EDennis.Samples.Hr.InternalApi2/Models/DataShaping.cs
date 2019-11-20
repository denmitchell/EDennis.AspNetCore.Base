using System.Dynamic;

namespace EDennis.Samples.Hr.InternalApi2.Models {

    public static class DataShaping {

        public static dynamic CombineCheckData(

            AgencyInvestigatorCheck agencyInvestigatorCheck,
            AgencyOnlineCheck agencyOnlineCheck,
            FederalBackgroundCheckView federalBackgroundCheck,
            StateBackgroundCheckView stateBackgroundCheck) {

            dynamic data = new {
                AgencyInvestigatorCheck =
                    new {
                        agencyInvestigatorCheck.DateCompleted,
                        agencyInvestigatorCheck.Status
                    },

                AgencyOnlineCheck =
                    new {
                        agencyOnlineCheck.DateCompleted,
                        agencyOnlineCheck.Status
                    },

                FederalBackgroundCheck =
                    new {
                        federalBackgroundCheck.DateCompleted,
                        federalBackgroundCheck.Status
                    },

                StateBackgroundCheck =
                    new {
                        stateBackgroundCheck.DateCompleted,
                        stateBackgroundCheck.Status
                    },
            };

            return data;
        }
        

    }

}

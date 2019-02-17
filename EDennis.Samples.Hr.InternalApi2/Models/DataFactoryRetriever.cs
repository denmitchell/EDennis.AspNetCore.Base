using EDennis.AspNetCore.Base.Testing;

namespace EDennis.Samples.Hr.InternalApi2.Models {

    public static partial class AgencyInvestigatorCheckContextDataFactory {
        public static AgencyInvestigatorCheck[] AgencyInvestigatorCheckRecordsFromRetriever { get; set; }
            = LocalDbDataRetriever.Retrieve<AgencyInvestigatorCheck>("AgencyInvestigatorCheck", "AgencyInvestigatorCheck");
    }
    public static partial class AgencyOnlineCheckContextDataFactory {
        public static AgencyOnlineCheck[] AgencyOnlineCheckRecordsFromRetriever { get; set; }
        = LocalDbDataRetriever.Retrieve<AgencyOnlineCheck>("AgencyOnlineCheck", "AgencyOnlineCheck");
    }
    public static partial class FederalBackgroundCheckContextDataFactory {
        public static FederalBackgroundCheck[] FederalBackgroundCheckRecordsFromRetriever { get; set; }
        = LocalDbDataRetriever.Retrieve<FederalBackgroundCheck>("FederalBackgroundCheck", "FederalBackgroundCheck");
    }
    public static partial class StateBackgroundCheckContextDataFactory {
        public static StateBackgroundCheck[] StateBackgroundCheckRecordsFromRetriever { get; set; }
        = LocalDbDataRetriever.Retrieve<StateBackgroundCheck>("StateBackgroundCheck", "StateBackgroundCheck");
    }
}

using EDennis.AspNetCore.Base.Testing;

namespace EDennis.Samples.Hr.InternalApi2.Models {

    public static partial class AgencyInvestigatorCheckContextDataFactory {
        public static AgencyInvestigatorCheck[] AgencyInvestigatorCheckRecordsFromRetriever { get; set; }
            = DataRetriever.Retrieve<AgencyInvestigatorCheck>("AgencyInvestigatorCheck", "AgencyInvestigatorCheck");
    }
    public static partial class AgencyInvestigatorCheckHistoryContextDataFactory {
        public static AgencyInvestigatorCheck[] AgencyInvestigatorCheckHistoryRecordsFromRetriever { get; set; }
            = DataRetriever.Retrieve<AgencyInvestigatorCheck>("AgencyInvestigatorCheck", "dbo_history.AgencyInvestigatorCheck");
    }
    public static partial class AgencyOnlineCheckContextDataFactory {
        public static AgencyOnlineCheck[] AgencyOnlineCheckRecordsFromRetriever { get; set; }
        = DataRetriever.Retrieve<AgencyOnlineCheck>("AgencyOnlineCheck", "AgencyOnlineCheck");
    }
    public static partial class AgencyOnlineCheckHistoryContextDataFactory {
        public static AgencyOnlineCheck[] AgencyOnlineCheckHistoryRecordsFromRetriever { get; set; }
            = DataRetriever.Retrieve<AgencyOnlineCheck>("AgencyOnlineCheck", "dbo_history.AgencyOnlineCheck");
    }
    public static partial class FederalBackgroundCheckContextDataFactory {
        public static FederalBackgroundCheck[] FederalBackgroundCheckRecordsFromRetriever { get; set; }
        = DataRetriever.Retrieve<FederalBackgroundCheck>("FederalBackgroundCheck", "FederalBackgroundCheck");
    }
    public static partial class FederalBackgroundCheckHistoryContextDataFactory {
        public static FederalBackgroundCheck[] FederalBackgroundCheckHistoryRecordsFromRetriever { get; set; }
            = DataRetriever.Retrieve<FederalBackgroundCheck>("FederalBackgroundCheck", "dbo_history.FederalBackgroundCheck");
    }
    public static partial class StateBackgroundCheckContextDataFactory {
        public static StateBackgroundCheck[] StateBackgroundCheckRecordsFromRetriever { get; set; }
        = DataRetriever.Retrieve<StateBackgroundCheck>("StateBackgroundCheck", "StateBackgroundCheck");
    }
    public static partial class StateBackgroundCheckHistoryContextDataFactory {
        public static StateBackgroundCheck[] StateBackgroundCheckHistoryRecordsFromRetriever { get; set; }
            = DataRetriever.Retrieve<StateBackgroundCheck>("StateBackgroundCheck", "dbo_history.StateBackgroundCheck");
    }
}

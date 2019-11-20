using EDennis.AspNetCore.Base.Testing;

namespace EDennis.Samples.Hr.InternalApi1.Models {
    public static partial class HrContextDataFactory {

        public static Employee[] EmployeeRecordsFromRetriever { get; set; }
            = DataRetriever.Retrieve<Employee>("Hr", "Employee");
        public static Position[] PositionRecordsFromRetriever { get; set; }
            = DataRetriever.Retrieve<Position>("Hr", "Position");
        public static EmployeePosition[] EmployeePositionRecordsFromRetriever { get; set; }
            = DataRetriever.Retrieve<EmployeePosition>("Hr", "EmployeePosition");

        public static Employee[] EmployeeHistoryRecordsFromRetriever { get; set; }
            = DataRetriever.Retrieve<Employee>("Hr", "dbo_history.Employee");
        public static Position[] PositionHistoryRecordsFromRetriever { get; set; }
            = DataRetriever.Retrieve<Position>("Hr", "dbo_history.Position");
        public static EmployeePosition[] EmployeePositionHistoryRecordsFromRetriever { get; set; }
            = DataRetriever.Retrieve<EmployeePosition>("Hr", "dbo_history.EmployeePosition");

    }
}

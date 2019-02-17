using EDennis.AspNetCore.Base.Testing;

namespace EDennis.Samples.Hr.InternalApi1.Models {
    public static partial class HrContextDataFactory {
        public static Employee[] EmployeeRecordsFromRetriever { get; set; }
            = LocalDbDataRetriever.Retrieve<Employee>("Hr", "Employee");
        public static EmployeePosition[] EmployeePositionRecordsFromRetriever { get; set; }
            = LocalDbDataRetriever.Retrieve<EmployeePosition>("Hr", "EmployeePosition");
        public static Position[] PositionRecordsFromRetriever { get; set; }
            = LocalDbDataRetriever.Retrieve<Position>("Hr", "Position");
    }
}

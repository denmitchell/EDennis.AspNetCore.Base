using EDennis.AspNetCore.Base.Testing;

namespace EDennis.Samples.Hr.InternalApi1.Models {
    public static partial class HrContextDataFactory {
        public static Employee[] EmployeeRecordsFromRetriever { get; set; }
            = DataRetriever.Retrieve<Employee>("Hr", "Employee");
        public static EmployeePosition[] EmployeePositionRecordsFromRetriever { get; set; }
            = DataRetriever.Retrieve<EmployeePosition>("Hr", "EmployeePosition");
        public static Position[] PositionRecordsFromRetriever { get; set; }
            = DataRetriever.Retrieve<Position>("Hr", "Position");
    }
}

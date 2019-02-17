using System;

namespace EDennis.Samples.Hr.InternalApi1.Models {
    public static partial class HrContextDataFactory {
        public static Employee[] EmployeeRecords { get; set; }
            = new Employee[] {
                new Employee {
                        Id = 1,
                        FirstName = "Bob",
                        SysStart = default(DateTime),
                        SysEnd = default(DateTime)
                },
                new Employee {
                        Id = 2,
                        FirstName = "Monty",
                        SysStart = default(DateTime),
                        SysEnd = default(DateTime)
                },
                new Employee {
                        Id = 3,
                        FirstName = "Drew",
                        SysStart = default(DateTime),
                        SysEnd = default(DateTime)
                },
                new Employee {
                        Id = 4,
                        FirstName = "Wayne",
                        SysStart = default(DateTime),
                        SysEnd = default(DateTime)
                },
            };
        public static EmployeePosition[] EmployeePositionRecords { get; set; }
            = new EmployeePosition[] {
                new EmployeePosition {
                        EmployeeId = 1,
                        PositionId = 1,
                        SysStart = default(DateTime),
                        SysEnd = default(DateTime)
                },
                new EmployeePosition {
                        EmployeeId = 2,
                        PositionId = 1,
                        SysStart = default(DateTime),
                        SysEnd = default(DateTime)
                },
                new EmployeePosition {
                        EmployeeId = 3,
                        PositionId = 2,
                        SysStart = default(DateTime),
                        SysEnd = default(DateTime)
                },
                new EmployeePosition {
                        EmployeeId = 4,
                        PositionId = 2,
                        SysStart = default(DateTime),
                        SysEnd = default(DateTime)
                },
            };
        public static Position[] PositionRecords { get; set; }
            = new Position[] {
                new Position {
                        Id = 1,
                        Title = "Game Show Manager",
                        IsManager = true,
                        SysStart = default(DateTime),
                        SysEnd = default(DateTime)
                },
                new Position {
                        Id = 2,
                        Title = "Game Show Host",
                        IsManager = false,
                        SysStart = default(DateTime),
                        SysEnd = default(DateTime)
                },
            };
    }
}

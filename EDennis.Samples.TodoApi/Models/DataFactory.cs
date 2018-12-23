using System;

namespace EDennis.Samples.TodoApi.Models {
    public static class TodoContextDataFactory {
        public static Task[] TaskRecords { get; set; }
            = new Task[] {
                new Task {
                        Id = 1,
                        Title = "Take out the garbage",
                        DueDate = new DateTime(2018,01,01),
                        PercentComplete = 100,
                        SysStart = default(DateTime),
                        SysEnd = default(DateTime)
                },
                new Task {
                        Id = 2,
                        Title = "Clean the attic",
                        DueDate = new DateTime(2018,01,02),
                        PercentComplete = 50,
                        SysStart = default(DateTime),
                        SysEnd = default(DateTime)
                },
                new Task {
                        Id = 3,
                        Title = "Clean the basement",
                        DueDate = new DateTime(2018,01,03),
                        PercentComplete = 0,
                        SysStart = default(DateTime),
                        SysEnd = default(DateTime)
                }
            };
    }
}

using EDennis.AspNetCore.Base.EntityFramework;

namespace EDennis.Samples.TodoApi.Models {
    public class TaskRepo : WriteableRepo<Task,TodoContext> {
        public TaskRepo(TodoContext context) : base(context) { }
    }
}

using System.Linq;

namespace EDennis.AspNetCore.Base {
    public enum DatabaseProvider {
        Unspecified = default,
        SqlServer,
        Sqlite,
        InMemory
    }
}

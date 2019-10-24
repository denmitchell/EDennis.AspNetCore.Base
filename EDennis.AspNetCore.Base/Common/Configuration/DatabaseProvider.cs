using System.Linq;

namespace EDennis.AspNetCore.Base {
    public enum DatabaseProvider {
        SqlServer,
        Sqlite,
        InMemory
    }

    public static class DatabaseProviderExtensions {

        public static DatabaseProvider InferProvider(string connectionString) {
            DatabaseProvider provider;
            var sqlitePatterns = new string[] { ":memory:", ".db", ".sqlite" };
            if (connectionString.ToLower().Contains("localdb")
                || connectionString.ToLower().Contains("initial catalog")
                || connectionString.ToLower().Contains("trusted_connection")
                || connectionString.ToLower().Contains("multipleactiveresultsets"))
                provider = DatabaseProvider.SqlServer;
            else if (connectionString.Contains("Data Source")
                && sqlitePatterns.Any(c => connectionString.Contains(c)))
                provider = DatabaseProvider.Sqlite;
            else
                provider = DatabaseProvider.SqlServer;
            return provider;
        }
    }

}

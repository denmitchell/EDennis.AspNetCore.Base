using System;

namespace EDennis.AspNetCore.Base {
    public enum ConnectionType {
        AutoCommit,
        Rollback, 
        InMemory 
    }

    public static class ConnectionTypeExtensions {
        public static string CodeValue(this ConnectionType connectionType) =>
            connectionType switch
            {
                ConnectionType.AutoCommit => "A",
                ConnectionType.Rollback => "R",
                ConnectionType.InMemory => "M",
                _ => throw new ArgumentException(message: "Invalid enum value", paramName: nameof(connectionType))
            };

        public static ConnectionType EnumValue(dynamic value) =>
            value switch
            {
                "A" => ConnectionType.AutoCommit,
                "a" => ConnectionType.AutoCommit,
                "R" => ConnectionType.Rollback,
                "r" => ConnectionType.Rollback,
                "M" => ConnectionType.InMemory,
                "m" => ConnectionType.InMemory,
                'A' => ConnectionType.AutoCommit,
                'a' => ConnectionType.AutoCommit,
                'R' => ConnectionType.Rollback,
                'r' => ConnectionType.Rollback,
                'M' => ConnectionType.InMemory,
                'm' => ConnectionType.InMemory,
                _ => throw new ArgumentException(message: "Invalid enum value", paramName: typeof(ConnectionType).Name)
            };

    }

}

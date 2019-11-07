using System;

namespace EDennis.AspNetCore.Base {
    public enum TransactionType {
        AutoCommit,
        Rollback//, 
//        InMemory 
    }

    public static class ConnectionTypeExtensions {
        public static string CodeValue(this TransactionType connectionType) =>
            connectionType switch
            {
                TransactionType.AutoCommit => "A",
                TransactionType.Rollback => "R",
                //ConnectionType.InMemory => "M",
                _ => throw new ArgumentException(message: "Invalid enum value", paramName: nameof(connectionType))
            };

        public static TransactionType EnumValue(dynamic value) =>
            value switch
            {
                "A" => TransactionType.AutoCommit,
                "a" => TransactionType.AutoCommit,
                "R" => TransactionType.Rollback,
                "r" => TransactionType.Rollback,
                //"M" => ConnectionType.InMemory,
                //"m" => ConnectionType.InMemory,
                'A' => TransactionType.AutoCommit,
                'a' => TransactionType.AutoCommit,
                'R' => TransactionType.Rollback,
                'r' => TransactionType.Rollback,
                //'M' => ConnectionType.InMemory,
                //'m' => ConnectionType.InMemory,
                _ => throw new ArgumentException(message: "Invalid enum value", paramName: typeof(TransactionType).Name)
            };

    }

}

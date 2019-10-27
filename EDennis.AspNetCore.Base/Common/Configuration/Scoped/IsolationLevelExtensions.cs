using System;
using System.Data;

namespace EDennis.AspNetCore.Base {

    public static class IsolationLevelExtensions {
        public static string CodeValue(this IsolationLevel isolationLevel) =>
            isolationLevel switch
            {
                IsolationLevel.ReadUncommitted => "U",
                IsolationLevel.ReadCommitted => "C",
                IsolationLevel.Serializable => "S",
                _ => throw new ArgumentException(message: "Invalid enum value", paramName: nameof(isolationLevel))
            };

        public static IsolationLevel EnumValue(dynamic value) =>
            value switch
            {
                "U" => IsolationLevel.ReadUncommitted,
                "u" => IsolationLevel.ReadUncommitted,
                "C" => IsolationLevel.ReadCommitted,
                "c" => IsolationLevel.ReadCommitted,
                "S" => IsolationLevel.Serializable,
                "s" => IsolationLevel.Serializable,
                'U' => IsolationLevel.ReadUncommitted,
                'u' => IsolationLevel.ReadUncommitted,
                'C' => IsolationLevel.ReadCommitted,
                'c' => IsolationLevel.ReadCommitted,
                'S' => IsolationLevel.Serializable,
                's' => IsolationLevel.Serializable,
                _ => throw new ArgumentException(message: "Invalid enum value", paramName: typeof(ConnectionType).Name)
            };


    }
}

using System;

namespace EDennis.AspNetCore.Base {
    public enum ToggleValue {
        _0,  //0
        _1, //1
        Reset   //*
    }

    public static class ToggleValueExtensions {
        public static string CodeValue(this ToggleValue ToggleValue) =>
            ToggleValue switch
            {
                ToggleValue._0 => "0",
                ToggleValue._1 => "1",
                ToggleValue.Reset => "*",
                _ => throw new ArgumentException(message: "Invalid enum value", paramName: nameof(ToggleValue))
            };

        public static ToggleValue EnumValue(dynamic value) =>
            value switch
            {
                "0" => ToggleValue._0,
                "1" => ToggleValue._1,
                "*" => ToggleValue.Reset,
                '0' => ToggleValue._0,
                '1' => ToggleValue._1,
                '*' => ToggleValue.Reset,
                _ => throw new ArgumentException(message: "Invalid enum value", paramName: typeof(ConnectionType).Name)
            };
    }

}

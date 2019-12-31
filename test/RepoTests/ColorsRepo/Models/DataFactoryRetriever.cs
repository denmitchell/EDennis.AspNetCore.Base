using EDennis.AspNetCore.Base.Testing;

namespace Colors.Models {
    public static partial class ColorDbContextDataFactory {
        public static Color[] ColorRecordsFromRetriever { get; set; }
            = DataRetriever.Retrieve<Color>("ColorDb", "Color");
    }
    public static partial class ColorHistoryDbContextDataFactory {
        public static Color[] ColorHistoryRecordsFromRetriever { get; set; }
            = DataRetriever.Retrieve<Color>("ColorDb", "dbo_history.Color");
    }
}

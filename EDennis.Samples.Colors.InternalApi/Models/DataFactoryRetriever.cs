using EDennis.AspNetCore.Base.Testing;

namespace EDennis.Samples.Colors.InternalApi.Models {
    public static partial class ColorDbContextDataFactory {
        public static Color[] ColorRecordsFromRetriever { get; set; }
            = DataRetriever.Retrieve<Color>("ColorDb", "Colors");
    }
    public static partial class ColorHistoryDbContextDataFactory {
        public static Color[] ColorHistoryRecordsFromRetriever { get; set; }
            = DataRetriever.Retrieve<Color>("ColorDb", "dbo_history.Colors");
    }
}

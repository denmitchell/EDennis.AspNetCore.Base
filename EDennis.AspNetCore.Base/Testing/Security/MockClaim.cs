namespace EDennis.AspNetCore.Base.Testing
{
    /// <summary>
    /// This class is designed to support IdentityServerMockDataLoader.
    /// *** Special Note: this class is not yet tested.
    /// </summary>
    public class MockClaim {
        public string Type { get; set; }
        public string Value { get; set; }
    }
}

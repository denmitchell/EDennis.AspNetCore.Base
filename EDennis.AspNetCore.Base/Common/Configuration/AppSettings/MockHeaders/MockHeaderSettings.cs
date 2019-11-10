using Microsoft.Extensions.Primitives;

namespace EDennis.AspNetCore.Base
{
    public class MockHeaderSettings {
        public StringValues Values { get; set; }
        public MockHeaderConflictResolution ConflictResolution { get; set; }
    }
}

using Microsoft.Extensions.Primitives;

namespace EDennis.AspNetCore.Base
{
    public class MockHeaderSettings {

        public string[] Values { get; set; } = new string[] { };
        public MockHeaderConflictResolution ConflictResolution { get; set; } = MockHeaderConflictResolution.Overwrite;
    }
}

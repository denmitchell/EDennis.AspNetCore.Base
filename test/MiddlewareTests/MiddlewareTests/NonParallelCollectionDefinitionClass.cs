using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MiddlewareTests {
    [CollectionDefinition("Sequential", DisableParallelization = true)]
    public class NonParallelCollectionDefinitionClass {
    }
}

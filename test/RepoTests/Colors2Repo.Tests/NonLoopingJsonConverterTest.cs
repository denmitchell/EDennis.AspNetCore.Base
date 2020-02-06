using System.Collections.Generic;
using System.Text.Json;
using Xunit;
using EDennis.AspNetCore.Base.Serialization;
using Xunit.Abstractions;

namespace Colors2Repo.Tests {

    public class A {
        public int Id { get; set; }
        public List<B> Bs { get; set; }
    }
    public class B {
        public int Id { get; set; }
        public A A { get; set; }
    }

    public class NonLoopingJsonConverterTest {

        ITestOutputHelper _output;

        public NonLoopingJsonConverterTest(ITestOutputHelper output) {
            _output = output;
        }

        [Fact] 
        public void TestIt() {
            var a = new A { Id = 1 };
            var b1 = new B { Id = 4, A = a };
            var b2 = new B { Id = 5, A = a};
            a.Bs = new List<B> { b1, b2 };

            JsonSerializerOptions options = new JsonSerializerOptions();
            options.Converters.Add(new NonLoopingJsonConverter());

            var json = JsonSerializer.Serialize(a, options);

            _output.WriteLine(json);

        }

    }
}

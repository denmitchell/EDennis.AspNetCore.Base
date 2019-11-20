using System;
using Microsoft.Extensions.Options;

namespace EDennis.AspNetCore.Base.Testing {
    /// <summary>
    /// https://stackoverflow.com/a/57090899/10896865
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TestOptionsMonitor<T> : IOptionsMonitor<T>
        where T : class, new() {
        public TestOptionsMonitor(T currentValue) {
            CurrentValue = currentValue;
        }

        public T Get(string name) {
            return CurrentValue;
        }

        public IDisposable OnChange(Action<T, string> listener) {
            throw new NotImplementedException();
        }

        public T CurrentValue { get; }
    }

}


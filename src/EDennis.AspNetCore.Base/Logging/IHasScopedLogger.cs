
namespace EDennis.AspNetCore.Base.Logging {
    public interface IHasScopedLogger {
        public IScopedLogger ScopedLogger { get; }
    }
}

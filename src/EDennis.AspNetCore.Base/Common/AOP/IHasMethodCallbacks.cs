using MethodBoundaryAspect.Fody.Attributes;

namespace EDennis.AspNetCore.Base {
    public interface IHasMethodCallbacks {
        void OnEntry(MethodExecutionArgs args);
        void OnExit(MethodExecutionArgs args);
        void OnException(MethodExecutionArgs args);
    }
}

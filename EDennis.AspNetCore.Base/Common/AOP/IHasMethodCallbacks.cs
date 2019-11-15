using MethodBoundaryAspect.Fody.Attributes;

namespace EDennis.AspNetCore.Base {
    interface IHasMethodCallbacks {
        void OnEntry(MethodExecutionArgs args);
        void OnExit(MethodExecutionArgs args);
        void OnException(MethodExecutionArgs args);
    }
}

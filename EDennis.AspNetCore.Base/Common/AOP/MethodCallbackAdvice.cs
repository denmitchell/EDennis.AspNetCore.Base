using MethodBoundaryAspect.Fody.Attributes;

namespace EDennis.AspNetCore.Base {
    public sealed class MethodCallbackAttribute : OnMethodBoundaryAspect {

        public override void OnEntry(MethodExecutionArgs args) =>
            GetTypedInstance(args)?.OnEntry(args);

        public override void OnExit(MethodExecutionArgs args) =>
            GetTypedInstance(args)?.OnExit(args);

        public override void OnException(MethodExecutionArgs args) =>
            GetTypedInstance(args)?.OnException(args);

        private static IHasMethodCallbacks GetTypedInstance(MethodExecutionArgs args) {
            if (typeof(IHasMethodCallbacks).IsAssignableFrom(args.Instance.GetType()))
                return (args.Instance as IHasMethodCallbacks);
            else
                return null;
        }
    }
}
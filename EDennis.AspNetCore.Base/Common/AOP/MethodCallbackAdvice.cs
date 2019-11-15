using EDennis.AspNetCore.Base.Logging;
using MethodBoundaryAspect.Fody.Attributes;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using WebApplication4;

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
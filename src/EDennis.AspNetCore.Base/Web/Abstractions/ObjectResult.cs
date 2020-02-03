using Microsoft.AspNetCore.Mvc;

namespace EDennis.AspNetCore.Base.Web {
    public class ObjectResult<TResult> : ObjectResult {
        public TResult TypedValue { get; }
        public ObjectResult(object value) : base(value) {
            TypedValue = (TResult)value;
        }
    }
}

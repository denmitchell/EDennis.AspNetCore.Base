using System;
using System.Collections.Generic;
using System.Net.Http;

namespace EDennis.AspNetCore.Base.Testing {

    public abstract class TestApisBase  : IDisposable{

        public virtual string InstanceName { get; } = Guid.NewGuid().ToString();

        public Dictionary<string, Func<HttpClient>> CreateClient { get; }
            = new Dictionary<string, Func<HttpClient>> ();

        protected Dictionary<string, Action> _dispose
            = new Dictionary<string, Action>();

        public abstract Dictionary<string,Type> EntryPoints { get; }
        public virtual string Environment { get; }

        public TestApisBase() : this(null) { }
        public TestApisBase(string env) {
            Environment = env;

            //now populate the dictionary with TestApi instances
            foreach (var httpClientName in EntryPoints.Keys) {
                Type[] typeParams = new Type[] { EntryPoints[httpClientName] };
                Type classType = typeof(TestApi<>);
                Type constructedType = classType.MakeGenericType(typeParams);

                Activator.CreateInstance(constructedType, 
                    new object[] { CreateClient, _dispose, 
                        httpClientName, InstanceName, Environment });

            }

        }

        public void Dispose() {
            foreach (var key in _dispose.Keys)
                _dispose[key]();
        }



    }
}

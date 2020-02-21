using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.NetCoreTestingUtilities;
using System.Collections.Generic;

namespace MiddlewareTests {

    public class CrudTestCases<TEntity> : List<CrudTestCase<TEntity>> 
        where TEntity : IHasIntegerId { 
        public CrudTestCases(JsonTestCase jsonTestCase, string basePath = null) {
            var bp = basePath ?? typeof(TEntity).Name;
            Add(new CrudTestCase<TEntity>(0, jsonTestCase, bp));
            Add(new CrudTestCase<TEntity>(1, jsonTestCase, bp));
        }
    }

    public class CrudTestCase<TEntity>
        where TEntity : IHasIntegerId {

        public CrudTestCase(int userIndex, JsonTestCase jsonTestCase, string basePath) {
            BasePath = basePath;
            BaseExpected = jsonTestCase.GetObject<List<TEntity>>($"BaseExpected");
            User = jsonTestCase.GetObject<string>($"User{userIndex}");
            CreateInputs = jsonTestCase.GetObject<List<TEntity>>($"CreateInputs{userIndex}");
            CreateExpected = jsonTestCase.GetObject<List<TEntity>>($"CreateExpected{userIndex}");
            UpdateInputs = jsonTestCase.GetObject<List<TEntity>>($"UpdateInputs{userIndex}");
            UpdateExpected = jsonTestCase.GetObject<List<TEntity>>($"UpdateExpected{userIndex}");
            DeleteIds = jsonTestCase.GetObject<int[]>($"DeleteIds{userIndex}");
            DeleteExpected = jsonTestCase.GetObject<List<TEntity>>($"DeleteExpected{userIndex}");
        }

        public string BasePath { get; set; }
        public string User { get; set; }
        public List<TEntity> BaseExpected { get; set; }
        public List<TEntity> CreateInputs { get; set; }
        public List<TEntity> CreateExpected { get; set; }
        public List<TEntity> UpdateInputs { get; set; }
        public List<TEntity> UpdateExpected { get; set; }
        public int[] DeleteIds { get; set; }
        public List<TEntity> DeleteExpected { get; set; }

        public string GetPostUrl() 
            => $"{BasePath}?X-User={User}&X-DeveloperName={User}";

        public string PutDeleteUrl(int id) 
            => $"{BasePath}/{id}?X-User={User}&X-DeveloperName={User}";

        public string ResetUrl()
            => $"{BasePath}/?X-User={User}&X-DeveloperName={User}&{Constants.TESTING_RESET_KEY}={User}";

    }
}

using System.Linq;

namespace EDennis.AspNetCore.Base.Web
{
    public class ODataQueryBuilder {
        public string Query { get; set; }
        private char Operator {
            get {
                return (Query ?? "").Contains("?") ? '&' : '?';
            }
        }
        public ODataQueryBuilder Select(params string[] properties) {
            Query += $"{Operator}$select={string.Join(',', properties)}";
            return this;
        }
        public ODataQueryBuilder Select<TEntity>(params string[] properties)
            where TEntity : class {
            var props = typeof(TEntity).GetProperties().Select(x=>x.Name).ToArray();
            return Select(props);
        }

        public ODataQueryBuilder Filter(string filter) {
            Query += $"{Operator}$filter={filter}";
            return this;
        }

        public ODataQueryBuilder OrderBy(string orderBy) {
            Query += $"{Operator}$orderBy={orderBy}";
            return this;
        }


        public ODataQueryBuilder Top(int top) {
            Query += $"{Operator}$top={top}";
            return this;
        }

        public ODataQueryBuilder Skip(int skip) {
            Query += $"{Operator}$skip={skip}";
            return this;
        }

    }
}

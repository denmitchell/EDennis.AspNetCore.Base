using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;

namespace EDennis.AspNetCore.Base.EntityFramework.Abstractions {

    /// <summary>
    /// Utility class for building dynamic linq where clauses using
    /// predicates (helpful for building where clauses as query string parameters
    /// in HTTP requests).
    /// </summary>
    public class DynamicLinqWhereBuilder {

        private readonly List<string> clauses = new List<string>();
        
        public string Build() {
            return string.Join(" and ", clauses);
        }


        public DynamicLinqWhereBuilder Contains<TEntity>(Expression<Func<TEntity,string>> propertySelector, string propertyValue) {
            var propName = GetPropertyName(propertySelector);
            if (propertyValue.StartsWith("="))
                clauses.Add($"{propName} eq \"{propertyValue.Substring(1)}\")");
            else
                clauses.Add($"{propName}.Contains(\"{propertyValue.ToString()}\")");
            return this;
        }

        public DynamicLinqWhereBuilder GreaterOrEqual<TEntity>(Expression<Func<TEntity, int>> propertySelector, int propertyValue)
            => BinaryOp(propertySelector, propertyValue, "ge");
        public DynamicLinqWhereBuilder GreaterOrEqual<TEntity>(Expression<Func<TEntity, long>> propertySelector, long propertyValue)
            => BinaryOp(propertySelector, propertyValue, "ge");
        public DynamicLinqWhereBuilder GreaterOrEqual<TEntity>(Expression<Func<TEntity, decimal>> propertySelector, decimal propertyValue)
            => BinaryOp(propertySelector, propertyValue, "ge");
        public DynamicLinqWhereBuilder GreaterOrEqual<TEntity>(Expression<Func<TEntity, double>> propertySelector, double propertyValue)
            => BinaryOp(propertySelector, propertyValue, "ge");
        public DynamicLinqWhereBuilder GreaterOrEqual<TEntity>(Expression<Func<TEntity, bool>> propertySelector, bool propertyValue)
            => BinaryOp(propertySelector, propertyValue, "ge");
        public DynamicLinqWhereBuilder GreaterOrEqual<TEntity>(Expression<Func<TEntity, DateTime>> propertySelector, DateTime propertyValue)
            => BinaryOp(propertySelector, propertyValue, "ge");
        public DynamicLinqWhereBuilder GreaterOrEqual<TEntity>(Expression<Func<TEntity, TimeSpan>> propertySelector, TimeSpan propertyValue)
            => BinaryOp(propertySelector, propertyValue, "ge");


        public DynamicLinqWhereBuilder LessOrEqual<TEntity>(Expression<Func<TEntity, int>> propertySelector, int propertyValue)
            => BinaryOp(propertySelector, propertyValue, "le");
        public DynamicLinqWhereBuilder LessOrEqual<TEntity>(Expression<Func<TEntity, long>> propertySelector, long propertyValue)
            => BinaryOp(propertySelector, propertyValue, "le");
        public DynamicLinqWhereBuilder LessOrEqual<TEntity>(Expression<Func<TEntity, decimal>> propertySelector, decimal propertyValue)
            => BinaryOp(propertySelector, propertyValue, "le");
        public DynamicLinqWhereBuilder LessOrEqual<TEntity>(Expression<Func<TEntity, double>> propertySelector, double propertyValue)
            => BinaryOp(propertySelector, propertyValue, "le");
        public DynamicLinqWhereBuilder LessOrEqual<TEntity>(Expression<Func<TEntity, bool>> propertySelector, bool propertyValue)
            => BinaryOp(propertySelector, propertyValue, "le");
        public DynamicLinqWhereBuilder LessOrEqual<TEntity>(Expression<Func<TEntity, DateTime>> propertySelector, DateTime propertyValue)
            => BinaryOp(propertySelector, propertyValue, "le");
        public DynamicLinqWhereBuilder LessOrEqual<TEntity>(Expression<Func<TEntity, TimeSpan>> propertySelector, TimeSpan propertyValue)
            => BinaryOp(propertySelector, propertyValue, "le");



        public DynamicLinqWhereBuilder BinaryOp<TEntity>(Expression<Func<TEntity, DateTime>> propertySelector, DateTime propertyValue, string op) {
            var propName = GetPropertyName(propertySelector);
            var clause = $"{propName} {op} \"{propertyValue.ToString("U")}\"";
            clauses.Add(clause);
            return this;
        }
        public DynamicLinqWhereBuilder BinaryOp<TEntity>(Expression<Func<TEntity, TimeSpan>> propertySelector, TimeSpan propertyValue, string op) {
            var propName = GetPropertyName(propertySelector);
            var clause = $"{propName} {op} \"{propertyValue.ToString("hh:mm:ss")}\"";
            clauses.Add(clause);
            return this;
        }
        private DynamicLinqWhereBuilder BinaryOp<TEntity,TProperty>(Expression<Func<TEntity, TProperty>> propertySelector, TProperty propertyValue, string op) {
            var propName = GetPropertyName(propertySelector);
            var clause = $"{propName} {op} {propertyValue}";
            clauses.Add(clause);
            return this;
        }

        public string GetPropertyName<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> propertySelector) {
            var paramLength = propertySelector.Parameters[0].ToString().Length;
            var propName = propertySelector.Body.ToString().Substring(paramLength + 1);
            return propName;
        }

    }
}

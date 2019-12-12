using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace EDennis.AspNetCore.Base {
    public class DbContextInterceptorSettings<TContext> : DbContextBaseSettings<TContext> 
        where TContext: DbContext{

        public readonly static HashSet<UserSource> DEFAULT_INSTANCE_NAME_SOURCE = new HashSet<UserSource> { Base.UserSource.JwtNameClaim };

        private HashSet<UserSource> _instanceNameSource = new HashSet<UserSource>();
        private bool _instanceNameSourceSet = false;

        /// <summary>
        /// The source of information for the user.
        /// Implementation Note:
        ///    The implementation required a workaround
        ///    to make explicitly configured values replace
        ///    (rather than add to) the default item
        /// </summary>
        public HashSet<UserSource> InstanceNameSource {
            get {
                if (_instanceNameSource.Count == 0)
                    _instanceNameSource.UnionWith(DEFAULT_INSTANCE_NAME_SOURCE);

                return _instanceNameSource;
            }
            set {
                if (!_instanceNameSourceSet) {
                    value.RemoveWhere(x => DEFAULT_INSTANCE_NAME_SOURCE.Contains(x));
                }
                _instanceNameSource = value;
                _instanceNameSourceSet = true;
            }
        }


        public bool IsInMemory { get; set; }
        public IsolationLevel IsolationLevel { get; set; } = IsolationLevel.ReadUncommitted;
        public bool ResetSqlServerIdentities { get; set; } = false;
        public bool ResetSqlServerSequences { get; set; } = false;
    }
}

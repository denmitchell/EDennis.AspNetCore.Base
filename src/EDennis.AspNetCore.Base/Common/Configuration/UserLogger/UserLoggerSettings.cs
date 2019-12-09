using IdentityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base {
    public class UserLoggerSettings {

        public readonly static HashSet<UserSource> DEFAULT_USER_SOURCE = new HashSet<UserSource> { Base.UserSource.JwtNameClaim };

        private HashSet<UserSource> _userSource = new HashSet<UserSource>();
        private bool _userSourceSet = false;

        /// <summary>
        /// The source of information for the user.
        /// Implementation Note:
        ///    The implementation required a workaround
        ///    to make explicitly configured values replace
        ///    (rather than add to) the default item
        /// </summary>
        public HashSet<UserSource> UserSource {
            get {
                if (_userSource.Count == 0)
                    _userSource.UnionWith(DEFAULT_USER_SOURCE);

                return _userSource;
            }
            set {
                if (!_userSourceSet) {
                    value.RemoveWhere(x => DEFAULT_USER_SOURCE.Contains(x));
                }
                _userSource = value;
                _userSourceSet = true;
            }
        }
    }
}

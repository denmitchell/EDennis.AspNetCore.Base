using IdentityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base {
    public class ScopePropertiesSettings {


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
                    _userSource = new HashSet<UserSource> { Base.UserSource.JwtNameClaim };
                
                return _userSource;
            }
            set {
                if (!_userSourceSet) {
                    value.Remove(Base.UserSource.JwtNameClaim);
                }
                _userSource = value;
                _userSourceSet = true;
            }
        }

        public bool CopyHeaders { get; set; } = true;
        public bool CopyClaims { get; set; } = true;

        public bool AppendHostPath { get; set; } = false;

    }
}

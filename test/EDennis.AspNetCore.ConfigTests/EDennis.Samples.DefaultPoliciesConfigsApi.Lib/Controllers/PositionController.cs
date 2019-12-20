﻿using EDennis.AspNetCore.Base.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace EDennis.Samples.DefaultPoliciesConfigsApi.Lib.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class PositionController : ControllerBase {

        private readonly ILogger<PositionController> _logger;
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, bool>> _policyPatternCacheSet;

        public PositionController(
            IAuthorizationPolicyProvider authorizationPolicyProvider,
            ILogger<PositionController> logger) {
            _logger = logger;
            _policyPatternCacheSet = (authorizationPolicyProvider as DefaultPoliciesAuthorizationPolicyProvider).PolicyPatternCacheSet;
        }

        [HttpGet("Admin")]
        public string GetAdmin() {
            return "Hello, Admin!";
        }

        [HttpGet("User")]
        public string GetUser() {
            return "Hello, User!";
        }

        [HttpGet("PolicyPatternCacheSet")]
        public ConcurrentDictionary<string, ConcurrentDictionary<string, bool>> GetPolicyPatternCacheSet() {
            return _policyPatternCacheSet;
        }

    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace EDennis.AspNetCore.Base {
    public class TestConfig {



        public string ProfileName { get; set; } = "Default";

        /// <summary>
        /// Valid values are 'AC', 'R' and 'M'
        /// Whether AutoCommit (normal), Rollback, or In-Memory
        /// </summary>
        public ConnectionType ConnectionType { get; set; } = ConnectionType.AutoCommit;


        /// <summary>
        /// Valid values are 'AC', 'R' and 'M'
        /// Whether AutoCommit (normal), Rollback, or In-Memory
        /// </summary>
        public IsolationLevel IsolationLevel { get; set; } = IsolationLevel.ReadCommitted;


        /// <summary>
        /// Valid values are '0', '1', and '*'
        /// When changed, the old connection with the same ConnectionStringKey and ConnectionType is dropped
        /// </summary>
        public ToggleValue ToggleValue { get; set; } = ToggleValue._0;


        /// <summary>
        /// The instance name used as a key for the TestDbContextOptionsCache
        /// </summary>
        public string InstanceName {
            get {
                return $"{ProfileName}-{ConnectionType.CodeValue()}{ToggleValue}-{IsolationLevel.CodeValue()}";
            }
        }



        /// <summary>
        /// Returns the first matching instance name, ignoring toggle,
        /// along with the toggle comparison result.
        /// </summary>
        /// <returns></returns>
        public FindResult Find(IEnumerable<string> instanceNames) {
            var match = instanceNames
                .FirstOrDefault(x => x.StartsWith($"{ProfileName}-{ConnectionType.CodeValue()}"));

            if (match == null)
                return new FindResult();

            ToggleComparisonResult toggleComparisonResult;

            if (ToggleValue == ToggleValue.Reset)
                toggleComparisonResult = ToggleComparisonResult.Reset;
            else {
                var matchingConfig = new TestConfigParser().Parse(match);
                if (matchingConfig.ToggleValue == ToggleValue)
                    toggleComparisonResult = ToggleComparisonResult.Same;
                else
                    toggleComparisonResult = ToggleComparisonResult.Different;
            }

            return new FindResult {
                MatchingInstanceName = match,
                ToggleComparisonResult = toggleComparisonResult
            };

        }
    }



    public class FindResult {
        public ToggleComparisonResult ToggleComparisonResult { get; set; }
        public string MatchingInstanceName { get; set; }
    }


}

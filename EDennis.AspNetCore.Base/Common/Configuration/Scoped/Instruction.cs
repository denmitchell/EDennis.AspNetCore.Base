using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace EDennis.AspNetCore.Base {

    /// <summary>
    /// Scope-level processing instruction.
    /// </summary>
    public class Instruction {

        public const string HEADER = "X-Instruction";
        public const string CLAIM_TYPE = "X-Instruction";
        public const string CLAIM_VALUE_SEPARATOR = "::";

        public string SysUser { get; set; }
        public TransactionType ConnectionType { get; set; } = TransactionType.AutoCommit;
        public IsolationLevel IsolationLevel { get; set; } = IsolationLevel.ReadCommitted;
        public ToggleValue ToggleValue { get; set; } = ToggleValue._0;

        /// <summary>
        /// The instance name used as a key for the TestDbContextOptionsCache
        /// </summary>
        public string InstanceName {
            get {
                return $"{SysUser}-{ConnectionType.CodeValue()}{ToggleValue}-{IsolationLevel.CodeValue()}";
            }
        }

        public override string ToString() {
            return InstanceName;
        }


        /// <summary>
        /// Returns the first matching instance name, ignoring toggle,
        /// along with the toggle comparison result.
        /// </summary>
        /// <returns></returns>
        public FindResult Find(IEnumerable<string> instanceNames) {
            var match = instanceNames
                .FirstOrDefault(x => x.StartsWith($"{SysUser}-{ConnectionType.CodeValue()}"));

            if (match == null)
                return new FindResult();

            ToggleComparisonResult toggleComparisonResult;

            if (ToggleValue == ToggleValue.Reset)
                toggleComparisonResult = ToggleComparisonResult.Reset;
            else {
                var matchingConfig = new InstructionParser().Parse(match);
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

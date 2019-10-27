using Microsoft.Extensions.Configuration;
using System;

namespace EDennis.AspNetCore.Base {

    //X-Instruction: YY1-A0    -- only used for testing scenarios where entire db can be restored
    //X-Instruction: YY1-R0-U  -- new read-uncommitted transaction; preceded by rollback if R1 is active
    //                            -- most performant; useful for single-connection dbs and SQL Server RLS  
    //X-Instruction: YY1-R1-C  -- new read-committed transaction; preceeded by rollback if R0 is active
    //                            -- SQL Server default  
    //X-Instruction: YY1-R1    -- transaction-level not specified -- use database default
    //                            
    //X-Instruction: YY1-R0-S  -- new serializable transaction; preceeded by rollback if R1 is active
    //                            -- most likely to create deadlocks; SQLite default  
    //X-Instruction: YY1-R*    -- rollback only; no new transaction
    //X-Instruction: YY1-M0    -- new in
    //X-Instruction: YY1-M1
    //X-Instruction: YY1-M*


    /// <summary>
    /// Holds Test Configuration data transmitted via claims or headers.
    /// </summary>
    public class InstructionParser {

        public InstructionParser() { }

        public Instruction Parse(string instructionUnparsed) {

            var instruction = new Instruction();

            var components = instructionUnparsed.Split('-');

            instruction.ProfileName = components[0];


            if (components.Length > 1) {
                try {
                    instruction.ConnectionType = ConnectionTypeExtensions.EnumValue(components[1][0]);
                } catch {
                    throw new ApplicationException($"For the provided Instruction ({instructionUnparsed}), the second component (connection type) does not have a valid value (A, R, or M)");
                }
                try {
                    instruction.ToggleValue = ToggleValueExtensions.EnumValue(components[1][1]);
                } catch {
                    throw new ApplicationException($"For the provided Instruction ({instructionUnparsed}), the second component does not have a valid toggle value (0, 1, or *)");
                }
            }

            if (components.Length > 2) {
                try {
                    instruction.IsolationLevel = IsolationLevelExtensions.EnumValue(components[1][0]);
                } catch {
                    throw new ApplicationException($"For the provided Instruction ({instructionUnparsed}), the third component (isolation level) does not have a valid value (U, C, or S)");
                }
            }

            return instruction;

        }

    }
}

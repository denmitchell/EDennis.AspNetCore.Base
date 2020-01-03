using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MethodBoundaryAspect.Fody.Attributes;
using Microsoft.EntityFrameworkCore.Migrations;
using EDennis.MigrationsExtensions;

namespace EDennis.AspNetCore.Base.EntityFramework {
    public class DownAttribute : OnMethodBoundaryAspect {

        public bool DropTestJsonTemporalTable { get; set; }
        public bool DropSqlServerTemporalTables { get; set; }

        public DownAttribute(bool dropTestJsonTemporalTable = true, 
            bool dropSqlServerTemporalTables = true) {
            DropTestJsonTemporalTable = dropTestJsonTemporalTable;
            DropSqlServerTemporalTables = dropSqlServerTemporalTables;
        }

        public override void OnExit(MethodExecutionArgs args) {

            MigrationBuilder migrationBuilder = (MigrationBuilder)args.Arguments[0];

            if (DropTestJsonTemporalTable)
                migrationBuilder.DropTestJsonTableSupport();

            migrationBuilder.DropMaintenanceProcedures();

        }
    }
}

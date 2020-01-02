using Colors2.Models;
using EDennis.AspNetCore.Base.EntityFramework;
using System.Collections.Generic;

namespace Colors2Repo.Migrator {
    partial class Program {

        static readonly Dictionary<string, ProjectInfo> projectInfo = new Dictionary<string, ProjectInfo> {
            {"Colors2", new ProjectInfo {Namespace = "Colors2",Directory="../../../../Colors2Repo/Migrations/"} }
        };

        static void Main(string[] args) {

            var migrationName = MigrationsUtils.GetMigrationName();
            var command = args[0];

            switch (command) {
                case "AddMigration=Colors2":
                    MigrationsUtils.AddMigrations<Color2DbContext>(migrationName, "Colors2", projectInfo["Colors2"].Directory);
                    break;
                case "UpdateDatabase=Colors2":
                    MigrationsUtils.UpdateDatabase<Color2DbContext>(true);
                    break;
            }

        }

    }
}

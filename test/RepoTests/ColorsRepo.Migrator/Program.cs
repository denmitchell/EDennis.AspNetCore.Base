using Colors.Models;
using EDennis.AspNetCore.Base.EntityFramework;
using System.Collections.Generic;

namespace ColorsRepo.Migrator {
    partial class Program {

        static readonly Dictionary<string, ProjectInfo> projectInfo = new Dictionary<string, ProjectInfo> {
            {"Colors", new ProjectInfo {Namespace = "Colors", Directory="../../../../ColorsRepo/Migrations/"} },
        };

        static void Main(string[] args) {

            var migrationName = MigrationsUtils.GetMigrationName();
            var command = args[0];

            switch (command) {
                case "AddMigration=Colors":
                    MigrationsUtils.AddMigrations<ColorHistoryDbContext>(migrationName + "Hist", "Colors", projectInfo["Colors"].Directory);
                    MigrationsUtils.AddMigrations<ColorDbContext>(migrationName, "Colors", projectInfo["Colors"].Directory);
                    break;
                case "UpdateDatabase=Colors":
                    MigrationsUtils.UpdateDatabase<ColorDbContext>(true);
                    MigrationsUtils.UpdateDatabase<ColorHistoryDbContext>(false);
                    break;
            }

        }

    }
}

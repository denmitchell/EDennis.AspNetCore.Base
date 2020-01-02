using Colors.Models;
using Colors2.Models;
using EDennis.AspNetCore.Base.EntityFramework;
using System.Collections.Generic;

namespace Migrator {
    partial class Program {

        static readonly Dictionary<string, ProjectInfo> projectInfo = new Dictionary<string, ProjectInfo> {
            {"Colors", new ProjectInfo {Namespace = "Colors", Directory="../../../../ColorsRepo/Migrations/"} },
            {"Colors2", new ProjectInfo {Namespace = "Colors2",Directory="../../../../Colors2Repo/Migrations/"} }
        };

        static void Main(string[] args) {

            var migrationName = MigrationsUtils.GetMigrationName();
            var command = args[0];

            switch (command) {
                case "AddMigration=Colors":
                    MigrationsUtils.AddMigrations<ColorDbContext>(migrationName, "Colors", projectInfo["Colors"].Directory);
                    MigrationsUtils.AddMigrations<ColorHistoryDbContext>(migrationName + "Hist", "Colors", projectInfo["Colors"].Directory);
                    break;
                case "AddMigration=Colors2":
                    MigrationsUtils.AddMigrations<Color2DbContext>(migrationName, "Colors2", projectInfo["Colors2"].Directory);
                    break;
                case "UpdateDatabase=Colors":
                    MigrationsUtils.UpdateDatabase<ColorDbContext>(true);
                    MigrationsUtils.UpdateDatabase<ColorHistoryDbContext>(false);
                    break;
                case "UpdateDatabase=Colors2":
                    MigrationsUtils.UpdateDatabase<Color2DbContext>(true);
                    break;
            }

        }

    }
}

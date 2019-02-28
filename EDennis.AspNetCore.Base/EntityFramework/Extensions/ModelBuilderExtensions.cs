using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDennis.AspNetCore.Base.EntityFramework {
    public static class MigrationBuilderExtensions {

        public static ModelBuilder RemoveNavigationProperties(this ModelBuilder modelBuilder) {

            var entityTypes = modelBuilder.Model.GetEntityTypes().ToList();
            for (int i = 0; i < entityTypes.Count(); i++) {
                var entityType = entityTypes[i];
                var navs = entityType.GetNavigations().ToList();
                for (int j = 0; j < navs.Count(); j++) {
                    modelBuilder.Entity(entityType.Name).Ignore(navs[j].Name);
                }
            }
            return modelBuilder;
        }
    }
}

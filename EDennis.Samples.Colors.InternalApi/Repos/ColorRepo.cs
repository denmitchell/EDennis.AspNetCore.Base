using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EDennis.Samples.Colors.InternalApi.Models {

    public class ColorRepo : SqlRepo<Color,ColorDbContext> {

        public ColorRepo(ColorDbContext context) : base(context) {}


        #region Custom Queries

        public Color GetByIdAsOf(DateTime asOf, int id) {

            var entity = Context.Set<Color>()
                .FromSql($@"
                    select * from Colors 
                        for system_time as of {asOf.ToString("yyyy-MM-ddTHH:mm:ss")}
                        where Id = {id}")
                .AsNoTracking()
                .FirstOrDefault();

            return entity;
        }


        public List<Color> GetByIdHistory(int id) {

            var list = Context.Set<Color>()
                .FromSql($@"
                    select * from Colors
                        for system_time all
                        where Id = {id}")
                .AsNoTracking()
                .ToList();

            return list;
        }


        #endregion



    }
}

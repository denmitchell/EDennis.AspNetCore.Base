using EDennis.AspNetCore.Base.EntityFramework;
using System;

namespace EDennis.Samples.Colors2Repo.Models
{
    public class Rgb: IHasIntegerId, IHasSysUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }
        public string SysUser { get; set; }
        public DateTime DateAdded { get; set; }
    }
}

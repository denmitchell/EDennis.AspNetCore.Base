using EDennis.AspNetCore.Base.EntityFramework;
using System;

namespace EDennis.Samples.Colors2Repo.Models
{
    public class Hsl : IHasSysUser {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Hue { get; set; }
        public int Saturation { get; set; }
        public int Luminance { get; set; }
        public string SysUser { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
